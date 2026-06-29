namespace Casio880PEditor
{
    public partial class frmMain : Form
    {
        private CasioSerialPort _casioPort;
        private string? _currentFilePath;
        private bool _isModified;
        private bool _isEditorInitialized = false;
        private bool _autoNumbering = false;
        private int _autoNumberIncrement = BasicLineNumbering.DEFAULT_INCREMENT;

        public frmMain()
        {
            InitializeComponent();
            InitializeComPorts();
            _casioPort = new CasioSerialPort();
            UpdateTitle();
            UpdateComStatus(); // Initialize COM status on startup
            UpdateStatus("Initializing editor...");
        }

        private async void Form1_Load(object? sender, EventArgs e)
        {
            await InitializeEditorAsync();
        }

        private async Task InitializeEditorAsync()
        {
            try
            {
                await monacoEditor.InitializeAsync();
                monacoEditor.TextChanged += MonacoEditor_TextChanged;
                _isEditorInitialized = true;
                UpdateStatus("Ready");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error initializing editor: {ex.Message}",
                    "Initialization Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                UpdateStatus("Editor initialization failed");
            }
        }

        private void InitializeComPorts()
        {
            RefreshComPorts();
        }

        private void RefreshComPorts()
        {
            comPortComboBox.Items.Clear();
            string[] ports = CasioSerialPort.GetAvailablePorts();

            if (ports.Length > 0)
            {
                comPortComboBox.Items.AddRange(ports);
                comPortComboBox.SelectedIndex = 0;
            }
            else
            {
                comPortComboBox.Items.Add("No ports available");
                comPortComboBox.SelectedIndex = 0;
            }
        }

        private void MonacoEditor_TextChanged(object? sender, EventArgs e)
        {
            if (!_isModified && _isEditorInitialized)
            {
                _isModified = true;
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            string fileName = string.IsNullOrEmpty(_currentFilePath) 
                ? "Untitled" 
                : Path.GetFileName(_currentFilePath);
            string modified = _isModified ? "*" : "";
            Text = $"{fileName}{modified} - Casio FX-880P Program Editor";
        }

        private void UpdateStatus(string message)
        {
            statusLabel.Text = message;
        }

        private void UpdateComStatus()
        {
            if (_casioPort.IsOpen)
            {
                comStatusLabel.Text = $"COM: Connected ({_casioPort.PortName})";
                connectButton.Text = "Disconnect";
                uploadButton.Enabled = true;
                downloadButton.Enabled = true;
                uploadToCasioToolStripMenuItem.Enabled = true;
                downloadFromCasioToolStripMenuItem.Enabled = true;
            }
            else
            {
                comStatusLabel.Text = "COM: Disconnected";
                connectButton.Text = "Connect";
                uploadButton.Enabled = false;
                downloadButton.Enabled = false;
                uploadToCasioToolStripMenuItem.Enabled = false;
                downloadFromCasioToolStripMenuItem.Enabled = false;
            }
        }

        private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isModified)
            {
                var result = MessageBox.Show(
                    "Do you want to save changes to your program?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    if (_isModified) // Save was cancelled
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                _casioPort.Dispose();
            }
        }

        // Event handler placeholders - will be implemented in next steps
        private void NewToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            NewFile();
        }

        private void OpenToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            OpenFile();
        }

        private void SaveToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveAsToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void ExitToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void RefreshPortsToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            RefreshComPorts();
            UpdateStatus("COM ports refreshed");
        }

        private void UploadToCasioToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Before uploading, execute this on your calculator:\n\n" +
                "LOAD \"COM0:5,N,8,1\"\n\n" +
                "Make sure the calculator is waiting for data before clicking OK.\n\n" +
                "Note: Your BASIC program lines should have line numbers (e.g., 10, 20, 30...)",
                "Upload Instructions",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                UploadToCasio();
            }
        }

        private void DownloadFromCasioToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Before downloading, execute this on your calculator:\n\n" +
                "LOAD \"COM0:5,N,8,1\"\n\n" +
                "Make sure the calculator is waiting for data before clicking OK.\n\n" +
                "Note: Your BASIC program lines should have line numbers (e.g., 10, 20, 30...)",
                "Download Instructions",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                DownloadFromCasio();
            }
        }

        private void AboutToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                $"Casio FX-880P Program Editor\n\n" +
                $"A text editor for Casio FX-880P BASIC programs\n" +
                $"with COM port communication support.\n\n" +
                $"Communication Settings:\n" +
                $"Baud Rate: {CasioSerialPort.DEFAULT_BAUD_RATE}\n" +
                $"Parity: {CasioSerialPort.DEFAULT_PARITY}\n" +
                $"Data Bits: {CasioSerialPort.DEFAULT_DATA_BITS}\n" +
                $"Stop Bits: {CasioSerialPort.DEFAULT_STOP_BITS}\n" +
                $"Descriptor: {CasioSerialPort.DESCRIPTOR}\n" +
                $"EOF Character: {_casioPort.EndOfFileCharacter}\n\n"+
                "Author: Cristian Solervicéns\n"+
                "Github: https://github.com/CSolervicens/Casio880PEditor",
                "About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void EofCtrlZToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _casioPort.EndOfFileCharacter = CasioSerialPort.EofCharacter.CtrlZ;
            UpdateEofMenuChecks();
            UpdateStatus("EOF character set to Ctrl+Z (0x1A)");
        }

        private void EofCtrlDToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _casioPort.EndOfFileCharacter = CasioSerialPort.EofCharacter.CtrlD;
            UpdateEofMenuChecks();
            UpdateStatus("EOF character set to Ctrl+D (0x04)");
        }

        private void EofNoneToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _casioPort.EndOfFileCharacter = CasioSerialPort.EofCharacter.None;
            UpdateEofMenuChecks();
            UpdateStatus("EOF character disabled");
        }

        private void UpdateEofMenuChecks()
        {
            eofCtrlZToolStripMenuItem.Checked = (_casioPort.EndOfFileCharacter == CasioSerialPort.EofCharacter.CtrlZ);
            eofCtrlDToolStripMenuItem.Checked = (_casioPort.EndOfFileCharacter == CasioSerialPort.EofCharacter.CtrlD);
            eofNoneToolStripMenuItem.Checked = (_casioPort.EndOfFileCharacter == CasioSerialPort.EofCharacter.None);
        }

        private void ConnectButton_Click(object? sender, EventArgs e)
        {
            if (_casioPort.IsOpen)
            {
                // Disconnect
                try
                {
                    _casioPort.Close();
                    UpdateComStatus();
                    UpdateStatus("Disconnected from COM port");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error disconnecting: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                // Connect
                if (comPortComboBox.SelectedItem == null || 
                    comPortComboBox.SelectedItem.ToString() == "No ports available")
                {
                    MessageBox.Show(
                        "Please select a valid COM port.",
                        "No Port Selected",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    string portName = comPortComboBox.SelectedItem.ToString()!;
                    _casioPort.Open(portName);
                    UpdateComStatus();
                    UpdateStatus($"Connected to {portName} at {CasioSerialPort.DEFAULT_BAUD_RATE} baud");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error connecting to COM port: {ex.Message}",
                        "Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private async void UploadToCasio()
        {
            if (!_isEditorInitialized)
            {
                MessageBox.Show(
                    "Editor is not initialized yet.",
                    "Not Ready",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!_casioPort.IsOpen)
            {
                MessageBox.Show(
                    "Please connect to a COM port first.",
                    "Not Connected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string programText = await monacoEditor.GetTextAsync();

            if (string.IsNullOrWhiteSpace(programText))
            {
                MessageBox.Show(
                    "There is no program to upload.",
                    "Empty Program",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            // Validate program format
            if (!CasioSerialPort.ValidateProgramFormat(programText, out string errorMessage))
            {
                var result = MessageBox.Show(
                    $"{errorMessage}\n\nDo you want to upload anyway?",
                    "Program Format Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return;
            }

            // Disable UI during upload
            SetControlsEnabled(false);
            UpdateStatus("Uploading program to Casio FX-880P...");

            try
            {
                var progress = new Progress<string>(message => UpdateStatus(message));
                await _casioPort.SendProgramAsync(programText, progress);

                MessageBox.Show(
                    "Program uploaded successfully!",
                    "Upload Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error uploading program: {ex.Message}",
                    "Upload Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
                UpdateStatus("Ready");
            }
        }

        private async void DownloadFromCasio()
        {
            if (!_isEditorInitialized)
            {
                MessageBox.Show(
                    "Editor is not initialized yet.",
                    "Not Ready",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (!_casioPort.IsOpen)
            {
                MessageBox.Show(
                    "Please connect to a COM port first.",
                    "Not Connected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (_isModified)
            {
                var result = MessageBox.Show(
                    "Current program has unsaved changes. Downloading will replace it.\nContinue?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            // Disable UI during download
            SetControlsEnabled(false);
            UpdateStatus("Downloading program from Casio FX-880P...");

            try
            {
                var progress = new Progress<string>(message => UpdateStatus(message));
                string program = await _casioPort.ReceiveProgramAsync(progress);

                if (!string.IsNullOrWhiteSpace(program))
                {
                    await monacoEditor.SetTextAsync(program);
                    _currentFilePath = null;
                    _isModified = true;
                    UpdateTitle();

                    MessageBox.Show(
                        "Program downloaded successfully!",
                        "Download Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "No program received from calculator.",
                        "Download Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error downloading program: {ex.Message}",
                    "Download Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetControlsEnabled(true);
                UpdateStatus("Ready");
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            menuStrip.Enabled = enabled;
            toolStrip.Enabled = enabled;
            monacoEditor.Enabled = enabled;
        }

        // File operation helper methods
        private async void NewFile()
        {
            if (!_isEditorInitialized)
                return;

            if (_isModified)
            {
                var result = MessageBox.Show(
                    "Do you want to save changes to your current program?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    if (_isModified) // Save was cancelled
                        return;
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            await monacoEditor.SetTextAsync("");
            _currentFilePath = null;
            _isModified = false;
            UpdateTitle();
            UpdateStatus("New file created");
        }

        private async void OpenFile()
        {
            if (!_isEditorInitialized)
                return;

            if (_isModified)
            {
                var result = MessageBox.Show(
                    "Do you want to save changes to your current program?",
                    "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveFile();
                    if (_isModified) // Save was cancelled
                        return;
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            using OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "CASIO Files (*.cas)|*.cas|BASIC Files (*.bas)|*.bas|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Open Casio BASIC Program"
            };

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string content = File.ReadAllText(openDialog.FileName);
                    await monacoEditor.SetTextAsync(content);
                    _currentFilePath = openDialog.FileName;
                    _isModified = false;
                    UpdateTitle();
                    UpdateStatus($"Opened: {Path.GetFileName(openDialog.FileName)}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error opening file: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private async void SaveFile()
        {
            if (!_isEditorInitialized)
                return;

            if (string.IsNullOrEmpty(_currentFilePath))
            {
                SaveFileAs();
            }
            else
            {
                try
                {
                    string content = await monacoEditor.GetTextAsync();
                    File.WriteAllText(_currentFilePath, content);
                    _isModified = false;
                    UpdateTitle();
                    UpdateStatus($"Saved: {Path.GetFileName(_currentFilePath)}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error saving file: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private async void SaveFileAs()
        {
            if (!_isEditorInitialized)
                return;

            using SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "CASIO Files (*.cas)|*.cas|BASIC Files (*.bas)|*.bas|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                FilterIndex = 1,
                Title = "Save Casio BASIC Program",
                DefaultExt = "bas"
            };

            if (!string.IsNullOrEmpty(_currentFilePath))
            {
                saveDialog.FileName = Path.GetFileName(_currentFilePath);
                saveDialog.InitialDirectory = Path.GetDirectoryName(_currentFilePath);
            }

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string content = await monacoEditor.GetTextAsync();
                    File.WriteAllText(saveDialog.FileName, content);
                    _currentFilePath = saveDialog.FileName;
                    _isModified = false;
                    UpdateTitle();
                    UpdateStatus($"Saved as: {Path.GetFileName(saveDialog.FileName)}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error saving file: {ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private async void AutoNumberingToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            _autoNumbering = autoNumberingToolStripMenuItem.Checked;

            if (_isEditorInitialized)
            {
                await monacoEditor.SetAutoNumberingAsync(_autoNumbering, _autoNumberIncrement);
            }

            if (_autoNumbering)
            {
                UpdateStatus($"Auto line numbering enabled (increment: {_autoNumberIncrement})");
            }
            else
            {
                UpdateStatus("Auto line numbering disabled");
            }
        }

        private async void RenumberToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (!_isEditorInitialized)
                return;

            var result = MessageBox.Show(
                "This will renumber all lines in multiples of 10\n" +
                "and update all GOTO/GOSUB/THEN/ELSE references.\n\n" +
                "Do you want to continue?",
                "Renumber Program",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                string programText = await monacoEditor.GetTextAsync();

                if (string.IsNullOrWhiteSpace(programText))
                {
                    MessageBox.Show(
                        "Program is empty",
                        "Renumber",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                UpdateStatus("Renumbering program...");
                string renumbered = BasicLineNumbering.RenumberProgram(programText, 
                    BasicLineNumbering.DEFAULT_START_LINE, 
                    _autoNumberIncrement);

                await monacoEditor.SetTextAsync(renumbered);
                UpdateStatus("Program renumbered successfully");

                MessageBox.Show(
                    "Program has been renumbered and all line references updated.\n" +
                    "(GOTO, GOSUB, THEN, ELSE)",
                    "Renumber Complete",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error renumbering program: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                UpdateStatus("Renumber failed");
            }
        }
    }
}
