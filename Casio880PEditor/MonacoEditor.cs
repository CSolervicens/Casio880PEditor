using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;
using System.ComponentModel;

namespace Casio880PEditor
{
    /// <summary>
    /// Monaco Editor wrapper for Casio FX-880P BASIC syntax highlighting
    /// </summary>
    public class MonacoEditor : WebView2
    {
        private bool _isInitialized = false;
        private string _pendingText = string.Empty;
        private TaskCompletionSource<bool>? _initializationTcs;

        public event EventHandler? TextChanged;

        public MonacoEditor()
        {
            Dock = DockStyle.Fill;
        }

        /// <summary>
        /// Initialize the Monaco Editor with Casio BASIC syntax highlighting
        /// </summary>
        public async Task InitializeAsync()
        {
            if (_isInitialized)
                return;

            _initializationTcs = new TaskCompletionSource<bool>();

            try
            {
                await EnsureCoreWebView2Async(null);

                // Navigate to the Monaco Editor HTML
                string html = GetMonacoHtml();
                CoreWebView2.NavigateToString(html);

                // Wait for initialization
                await _initializationTcs.Task;
                _isInitialized = true;

                // Set pending text if any
                if (!string.IsNullOrEmpty(_pendingText))
                {
                    await SetTextAsync(_pendingText);
                    _pendingText = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to initialize Monaco Editor", ex);
            }
        }

        /// <summary>
        /// Get the HTML for Monaco Editor with Casio BASIC language definition
        /// </summary>
        private string GetMonacoHtml()
        {
            return @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body { margin: 0; padding: 0; overflow: hidden; }
        #container { width: 100vw; height: 100vh; }
    </style>
</head>
<body>
    <div id='container'></div>
    <script src='https://cdn.jsdelivr.net/npm/monaco-editor@0.45.0/min/vs/loader.js'></script>
    <script>
        require.config({ paths: { vs: 'https://cdn.jsdelivr.net/npm/monaco-editor@0.45.0/min/vs' } });

        require(['vs/editor/editor.main'], function() {
            // Define Casio BASIC language
            monaco.languages.register({ id: 'casio-basic' });

            monaco.languages.setMonarchTokensProvider('casio-basic', {
                keywords: [
                    'PRINT', 'INPUT', 'IF', 'THEN', 'ELSE', 'GOTO', 'GOSUB', 'RETURN',
                    'FOR', 'TO', 'STEP', 'NEXT', 'DIM', 'LET', 'REM', 'END', 'STOP',
                    'ON', 'RUN', 'LIST', 'NEW', 'CLEAR', 'DEF', 'FN', 'READ', 'DATA',
                    'RESTORE', 'AND', 'OR', 'NOT', 'ABS', 'SQR', 'INT', 'SGN', 'SIN',
                    'COS', 'TAN', 'ATN', 'LOG', 'EXP', 'RND', 'LEFT$', 'RIGHT$', 'MID$',
                    'LEN', 'VAL', 'STR$', 'CHR$', 'ASC', 'PEEK', 'POKE', 'BEEP', 'PAUSE',
                    'WAIT', 'CURSOR', 'LINE', 'POINT', 'CIRCLE', 'DEGREE', 'RADIAN',
                    'MODE', 'STAT', 'OPEN', 'CLOSE', 'SAVE', 'LOAD', 'MERGE', 'VERIFY'
                ],
                operators: ['+', '-', '*', '/', '^', '=', '<', '>', '<=', '>=', '<>'],
                tokenizer: {
                    root: [
                        [/\b[A-Z]+\$?/, { cases: { '@keywords': 'keyword', '@default': 'identifier' } }],
                        [/\d+\.?\d*/, 'number'],
                        [/'.*$/, 'comment'],
                        [/REM\s+.*$/, 'comment'],
                        [/""([^""]|"""")*""/, 'string'],
                        [/[+\-*\/^=<>]/, 'operator']
                    ]
                }
            });

            monaco.languages.setLanguageConfiguration('casio-basic', {
                comments: { lineComment: ""'"" },
                brackets: [['(', ')']],
                autoClosingPairs: [
                    { open: '(', close: ')' },
                    { open: '""', close: '""', notIn: ['string'] }
                ]
            });

            // Create editor
            window.editor = monaco.editor.create(document.getElementById('container'), {
                value: '',
                language: 'casio-basic',
                theme: 'vs',
                fontSize: 14,
                lineNumbers: 'on',
                minimap: { enabled: false },
                scrollBeyondLastLine: false,
                automaticLayout: true
            });

            // Notify C# that initialization is complete
            window.chrome.webview.postMessage({ type: 'initialized' });

            // Listen for text changes
            window.editor.onDidChangeModelContent(() => {
                window.chrome.webview.postMessage({ type: 'textChanged', value: window.editor.getValue() });
            });

            // Auto-numbering settings
            window.autoNumberingEnabled = false;
            window.autoNumberIncrement = 10;

            // Add Enter key handler for auto-numbering
            window.editor.addCommand(monaco.KeyCode.Enter, () => {
                if (window.autoNumberingEnabled) {
                    const position = window.editor.getPosition();
                    const model = window.editor.getModel();
                    const currentLine = model.getLineContent(position.lineNumber);

                    // Extract line number from current line
                    const match = currentLine.match(/^(\d+)\s/);
                    if (match) {
                        const currentNum = parseInt(match[1]);
                        const nextNum = currentNum + window.autoNumberIncrement;

                        // Insert newline and next line number
                        const range = new monaco.Range(
                            position.lineNumber, 
                            position.column, 
                            position.lineNumber, 
                            position.column
                        );

                        window.editor.executeEdits('auto-number', [{
                            range: range,
                            text: '\n' + nextNum + ' '
                        }]);

                        return;
                    }
                }

                // Default behavior - insert newline
                window.editor.trigger('keyboard', 'type', { text: '\n' });
            });

            // Function to enable/disable auto-numbering
            window.setAutoNumbering = function(enabled, increment) {
                window.autoNumberingEnabled = enabled;
                if (increment) window.autoNumberIncrement = increment;
            };

            // Function to get editor text
            window.getEditorText = function() {
                return window.editor.getValue();
            };

            // Function to set editor text
            window.setEditorText = function(text) {
                window.editor.setValue(text);
            };
        });
    </script>
</body>
</html>";
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!DesignMode && CoreWebView2 == null)
            {
                _ = InitializeAsync();
            }
        }

        protected override void InitLayout()
        {
            base.InitLayout();

            if (CoreWebView2 != null && !DesignMode)
            {
                CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
            }
            else if (!DesignMode)
            {
                CoreWebView2InitializationCompleted += (s, e) =>
                {
                    if (e.IsSuccess)
                    {
                        CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
                    }
                };
            }
        }

        private void CoreWebView2_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var message = System.Text.Json.JsonDocument.Parse(e.WebMessageAsJson);
                var messageType = message.RootElement.GetProperty("type").GetString();

                if (messageType == "initialized")
                {
                    _initializationTcs?.TrySetResult(true);
                }
                else if (messageType == "textChanged")
                {
                    TextChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            catch
            {
                // Ignore JSON parsing errors
            }
        }

        /// <summary>
        /// Enable or disable auto line numbering
        /// </summary>
        public async Task SetAutoNumberingAsync(bool enabled, int increment = 10)
        {
            if (!_isInitialized)
                return;

            try
            {
                await CoreWebView2.ExecuteScriptAsync($"setAutoNumbering({(enabled ? "true" : "false")}, {increment})");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to set auto-numbering", ex);
            }
        }

        /// <summary>
        /// Get the current editor text
        /// </summary>
        public async Task<string> GetTextAsync()
        {
            if (!_isInitialized)
                return _pendingText;

            try
            {
                var result = await CoreWebView2.ExecuteScriptAsync("getEditorText()");
                // Remove quotes from JSON string result
                return System.Text.Json.JsonSerializer.Deserialize<string>(result) ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Set the editor text
        /// </summary>
        public async Task SetTextAsync(string text)
        {
            if (!_isInitialized)
            {
                _pendingText = text;
                return;
            }

            try
            {
                string escapedText = System.Text.Json.JsonSerializer.Serialize(text);
                await CoreWebView2.ExecuteScriptAsync($"setEditorText({escapedText})");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to set editor text", ex);
            }
        }

        /// <summary>
        /// Synchronous property for text (for compatibility)
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string EditorText
        {
            get
            {
                if (!_isInitialized)
                    return _pendingText;

                var task = GetTextAsync();
                task.Wait();
                return task.Result;
            }
            set
            {
                if (!_isInitialized)
                {
                    _pendingText = value;
                    return;
                }

                var task = SetTextAsync(value);
                task.Wait();
            }
        }
    }
}
