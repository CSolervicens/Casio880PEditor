using System.IO.Ports;
using System.Text;

namespace Casio880PEditor
{
    /// <summary>
    /// Wrapper class for COM port communication with Casio FX-880P calculator
    /// Default settings: 2400 baud, N parity, 8 data bits, 1 stop bit
    /// Descriptor: 5,N,8,1,N,N,N,N
    /// </summary>
    public class CasioSerialPort : IDisposable
    {
        private SerialPort? _serialPort;
        private bool _disposed = false;

        // Casio FX-880P default communication settings
        public const int DEFAULT_BAUD_RATE = 2400;
        public const Parity DEFAULT_PARITY = Parity.None;
        public const int DEFAULT_DATA_BITS = 8;
        public const StopBits DEFAULT_STOP_BITS = StopBits.One;
        public const string DESCRIPTOR = "5,N,8,1,N,N,N,N";

        // Communication timeout settings
        public const int READ_TIMEOUT = 5000;  // 5 seconds
        public const int WRITE_TIMEOUT = 5000; // 5 seconds

        // Termination character options
        public enum EofCharacter
        {
            CtrlZ = 0x1A,  // ASCII 26 - Standard DOS EOF
            CtrlD = 0x04,  // ASCII 4 - Unix EOF
            None = 0x00    // No EOF character
        }

        public EofCharacter EndOfFileCharacter { get; set; } = EofCharacter.CtrlZ;

        public bool IsOpen => _serialPort?.IsOpen ?? false;
        public string? PortName => _serialPort?.PortName;

        /// <summary>
        /// Get list of available COM ports on the system
        /// </summary>
        public static string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Open a connection to the specified COM port with Casio FX-880P settings
        /// </summary>
        public void Open(string portName)
        {
            Close(); // Close any existing connection

            _serialPort = new SerialPort(portName)
            {
                BaudRate = DEFAULT_BAUD_RATE,
                Parity = DEFAULT_PARITY,
                DataBits = DEFAULT_DATA_BITS,
                StopBits = DEFAULT_STOP_BITS,
                Handshake = Handshake.None,
                ReadTimeout = READ_TIMEOUT,
                WriteTimeout = WRITE_TIMEOUT,
                Encoding = Encoding.ASCII
            };

            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                _serialPort?.Dispose();
                _serialPort = null;
                throw new IOException($"Failed to open COM port {portName}", ex);
            }
        }

        /// <summary>
        /// Close the COM port connection
        /// </summary>
        public void Close()
        {
            if (_serialPort != null)
            {
                if (_serialPort.IsOpen)
                {
                    try
                    {
                        _serialPort.Close();
                    }
                    catch (Exception)
                    {
                        // Ignore errors during close
                    }
                }
                _serialPort.Dispose();
                _serialPort = null;
            }
        }

        /// <summary>
        /// Validate if the program has proper line numbers for Casio FX-880P
        /// </summary>
        public static bool ValidateProgramFormat(string programText, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(programText))
            {
                errorMessage = "Program is empty";
                return false;
            }

            var lines = programText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                // Check if line starts with a number (line number)
                if (!char.IsDigit(trimmedLine[0]))
                {
                    errorMessage = $"Line does not start with a line number: '{trimmedLine}'\n\n" +
                                 "Casio FX-880P BASIC programs require line numbers.\n" +
                                 "Example:\n10 PRINT \"HELLO\"\n20 END";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Send a program to the Casio FX-880P calculator
        /// </summary>
        public async Task SendProgramAsync(string programText, IProgress<string>? progress = null)
        {
            if (!IsOpen)
                throw new InvalidOperationException("COM port is not open");

            progress?.Report("Preparing to send program...");

            try
            {
                // Clear any existing data in the buffer
                _serialPort!.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                // Split into lines but remove completely empty lines
                var lines = programText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                progress?.Report($"Sending {lines.Length} lines...");

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();

                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // According to Casio FX-880P manual, each line needs an extra newline
                    // Send line + CR + CR (double carriage return)
                    _serialPort.Write(line + "\r\r");

                    // Wait for the calculator to process the line
                    await Task.Delay(200); // Increased delay for reliability

                    // Report progress
                    if (progress != null && (i + 1) % 10 == 0)
                    {
                        progress.Report($"Sent {i + 1}/{lines.Length} lines...");
                    }
                }

                // Send End-of-File (EOF) signal if configured
                if (EndOfFileCharacter != EofCharacter.None)
                {
                    progress?.Report("Sending EOF signal...");
                    await Task.Delay(200);
                    _serialPort.Write(new byte[] { (byte)EndOfFileCharacter }, 0, 1);
                    await Task.Delay(300); // Extra delay after EOF
                }

                progress?.Report($"Successfully sent {lines.Length} lines");
            }
            catch (TimeoutException)
            {
                progress?.Report("Timeout: Calculator did not respond");
                throw new IOException("Timeout while sending data to calculator");
            }
            catch (Exception ex)
            {
                progress?.Report($"Error: {ex.Message}");
                throw new IOException("Failed to send program to calculator", ex);
            }
        }

        /// <summary>
        /// Receive a program from the Casio FX-880P calculator
        /// </summary>
        public async Task<string> ReceiveProgramAsync(IProgress<string>? progress = null)
        {
            if (!IsOpen)
                throw new InvalidOperationException("COM port is not open");

            progress?.Report("Waiting to receive program...");

            try
            {
                // Clear any existing data in the buffer
                _serialPort!.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                StringBuilder programText = new StringBuilder();
                int lineCount = 0;
                bool receiving = true;

                // Set a reasonable timeout for the first line
                _serialPort.ReadTimeout = 10000; // 10 seconds for first line

                progress?.Report("Receiving data...");

                while (receiving)
                {
                    try
                    {
                        string line = _serialPort.ReadLine();

                        // Trim extra newlines and whitespace from the line
                        line = line.TrimEnd('\r', '\n', ' ', '\t');

                        if (string.IsNullOrEmpty(line))
                        {
                            // Empty line might indicate end of transmission
                            await Task.Delay(100);
                            if (_serialPort.BytesToRead == 0)
                                break;
                            continue;
                        }

                        programText.AppendLine(line);
                        lineCount++;

                        // Report progress every 10 lines
                        if (progress != null && lineCount % 10 == 0)
                        {
                            progress.Report($"Received {lineCount} lines...");
                        }

                        // Shorter timeout for subsequent lines
                        _serialPort.ReadTimeout = 2000;
                    }
                    catch (TimeoutException)
                    {
                        // Timeout indicates end of transmission
                        receiving = false;
                    }
                }

                progress?.Report($"Successfully received {lineCount} lines");
                return programText.ToString();
            }
            catch (Exception ex)
            {
                progress?.Report($"Error: {ex.Message}");
                throw new IOException("Failed to receive program from calculator", ex);
            }
        }

        /// <summary>
        /// Send a raw command to the calculator
        /// </summary>
        public void SendCommand(string command)
        {
            if (!IsOpen)
                throw new InvalidOperationException("COM port is not open");

            _serialPort!.WriteLine(command);
        }

        /// <summary>
        /// Read a line from the calculator
        /// </summary>
        public string ReadLine()
        {
            if (!IsOpen)
                throw new InvalidOperationException("COM port is not open");

            return _serialPort!.ReadLine();
        }

        /// <summary>
        /// Check if data is available to read
        /// </summary>
        public bool DataAvailable => _serialPort?.BytesToRead > 0;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
                _disposed = true;
            }
        }

        ~CasioSerialPort()
        {
            Dispose(false);
        }
    }
}
