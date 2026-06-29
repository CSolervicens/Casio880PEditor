namespace Casio880PEditor
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            menuStrip = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exitToolStripMenuItem = new ToolStripMenuItem();
            editToolStripMenuItem = new ToolStripMenuItem();
            autoNumberingToolStripMenuItem = new ToolStripMenuItem();
            renumberToolStripMenuItem = new ToolStripMenuItem();
            comToolStripMenuItem = new ToolStripMenuItem();
            refreshPortsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            uploadToCasioToolStripMenuItem = new ToolStripMenuItem();
            downloadFromCasioToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            eofSettingsToolStripMenuItem = new ToolStripMenuItem();
            eofCtrlZToolStripMenuItem = new ToolStripMenuItem();
            eofCtrlDToolStripMenuItem = new ToolStripMenuItem();
            eofNoneToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            toolStrip = new ToolStrip();
            newToolStripButton = new ToolStripButton();
            openToolStripButton = new ToolStripButton();
            saveToolStripButton = new ToolStripButton();
            toolStripSeparator3 = new ToolStripSeparator();
            toolStripLabel1 = new ToolStripLabel();
            comPortComboBox = new ToolStripComboBox();
            connectButton = new ToolStripButton();
            toolStripSeparator4 = new ToolStripSeparator();
            uploadButton = new ToolStripButton();
            downloadButton = new ToolStripButton();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            comStatusLabel = new ToolStripStatusLabel();
            monacoEditor = new MonacoEditor();
            menuStrip.SuspendLayout();
            toolStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)monacoEditor).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, comToolStripMenuItem, helpToolStripMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1000, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.N;
            newToolStripMenuItem.Size = new Size(195, 22);
            newToolStripMenuItem.Text = "&New";
            newToolStripMenuItem.Click += NewToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.O;
            openToolStripMenuItem.Size = new Size(195, 22);
            openToolStripMenuItem.Text = "&Open...";
            openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            saveToolStripMenuItem.Size = new Size(195, 22);
            saveToolStripMenuItem.Text = "&Save";
            saveToolStripMenuItem.Click += SaveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            saveAsToolStripMenuItem.Size = new Size(195, 22);
            saveAsToolStripMenuItem.Text = "Save &As...";
            saveAsToolStripMenuItem.Click += SaveAsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(192, 6);
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(195, 22);
            exitToolStripMenuItem.Text = "E&xit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { autoNumberingToolStripMenuItem, renumberToolStripMenuItem });
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.Size = new Size(39, 20);
            editToolStripMenuItem.Text = "&Edit";
            // 
            // autoNumberingToolStripMenuItem
            // 
            autoNumberingToolStripMenuItem.Checked = true;
            autoNumberingToolStripMenuItem.CheckOnClick = true;
            autoNumberingToolStripMenuItem.CheckState = CheckState.Checked;
            autoNumberingToolStripMenuItem.Name = "autoNumberingToolStripMenuItem";
            autoNumberingToolStripMenuItem.ShortcutKeys = Keys.F7;
            autoNumberingToolStripMenuItem.Size = new Size(208, 22);
            autoNumberingToolStripMenuItem.Text = "Auto &Line Numbering";
            autoNumberingToolStripMenuItem.Click += AutoNumberingToolStripMenuItem_Click;
            // 
            // renumberToolStripMenuItem
            // 
            renumberToolStripMenuItem.Name = "renumberToolStripMenuItem";
            renumberToolStripMenuItem.ShortcutKeys = Keys.F8;
            renumberToolStripMenuItem.Size = new Size(208, 22);
            renumberToolStripMenuItem.Text = "&Renumber Program";
            renumberToolStripMenuItem.Click += RenumberToolStripMenuItem_Click;
            // 
            // comToolStripMenuItem
            // 
            comToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { refreshPortsToolStripMenuItem, toolStripSeparator2, uploadToCasioToolStripMenuItem, downloadFromCasioToolStripMenuItem, toolStripSeparator5, eofSettingsToolStripMenuItem });
            comToolStripMenuItem.Name = "comToolStripMenuItem";
            comToolStripMenuItem.Size = new Size(47, 20);
            comToolStripMenuItem.Text = "&COM";
            // 
            // refreshPortsToolStripMenuItem
            // 
            refreshPortsToolStripMenuItem.Name = "refreshPortsToolStripMenuItem";
            refreshPortsToolStripMenuItem.Size = new Size(208, 22);
            refreshPortsToolStripMenuItem.Text = "&Refresh Ports";
            refreshPortsToolStripMenuItem.Click += RefreshPortsToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(205, 6);
            // 
            // uploadToCasioToolStripMenuItem
            // 
            uploadToCasioToolStripMenuItem.Name = "uploadToCasioToolStripMenuItem";
            uploadToCasioToolStripMenuItem.ShortcutKeys = Keys.F5;
            uploadToCasioToolStripMenuItem.Size = new Size(208, 22);
            uploadToCasioToolStripMenuItem.Text = "&Upload to Casio";
            uploadToCasioToolStripMenuItem.Click += UploadToCasioToolStripMenuItem_Click;
            // 
            // downloadFromCasioToolStripMenuItem
            // 
            downloadFromCasioToolStripMenuItem.Name = "downloadFromCasioToolStripMenuItem";
            downloadFromCasioToolStripMenuItem.ShortcutKeys = Keys.F6;
            downloadFromCasioToolStripMenuItem.Size = new Size(208, 22);
            downloadFromCasioToolStripMenuItem.Text = "&Download from Casio";
            downloadFromCasioToolStripMenuItem.Click += DownloadFromCasioToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(205, 6);
            // 
            // eofSettingsToolStripMenuItem
            // 
            eofSettingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { eofCtrlZToolStripMenuItem, eofCtrlDToolStripMenuItem, eofNoneToolStripMenuItem });
            eofSettingsToolStripMenuItem.Name = "eofSettingsToolStripMenuItem";
            eofSettingsToolStripMenuItem.Size = new Size(208, 22);
            eofSettingsToolStripMenuItem.Text = "&EOF Settings";
            // 
            // eofCtrlZToolStripMenuItem
            // 
            eofCtrlZToolStripMenuItem.Checked = true;
            eofCtrlZToolStripMenuItem.CheckState = CheckState.Checked;
            eofCtrlZToolStripMenuItem.Name = "eofCtrlZToolStripMenuItem";
            eofCtrlZToolStripMenuItem.Size = new Size(193, 22);
            eofCtrlZToolStripMenuItem.Text = "Ctrl+Z (0x1A) - Default";
            eofCtrlZToolStripMenuItem.Click += EofCtrlZToolStripMenuItem_Click;
            // 
            // eofCtrlDToolStripMenuItem
            // 
            eofCtrlDToolStripMenuItem.Name = "eofCtrlDToolStripMenuItem";
            eofCtrlDToolStripMenuItem.Size = new Size(193, 22);
            eofCtrlDToolStripMenuItem.Text = "Ctrl+D (0x04)";
            eofCtrlDToolStripMenuItem.Click += EofCtrlDToolStripMenuItem_Click;
            // 
            // eofNoneToolStripMenuItem
            // 
            eofNoneToolStripMenuItem.Name = "eofNoneToolStripMenuItem";
            eofNoneToolStripMenuItem.Size = new Size(193, 22);
            eofNoneToolStripMenuItem.Text = "None";
            eofNoneToolStripMenuItem.Click += EofNoneToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "&About";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { newToolStripButton, openToolStripButton, saveToolStripButton, toolStripSeparator3, toolStripLabel1, comPortComboBox, connectButton, toolStripSeparator4, uploadButton, downloadButton });
            toolStrip.Location = new Point(0, 24);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(1000, 25);
            toolStrip.TabIndex = 1;
            toolStrip.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            newToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            newToolStripButton.Image = (Image)resources.GetObject("newToolStripButton.Image");
            newToolStripButton.ImageTransparentColor = Color.Magenta;
            newToolStripButton.Name = "newToolStripButton";
            newToolStripButton.Size = new Size(23, 22);
            newToolStripButton.Text = "New";
            newToolStripButton.Click += NewToolStripMenuItem_Click;
            // 
            // openToolStripButton
            // 
            openToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            openToolStripButton.Image = (Image)resources.GetObject("openToolStripButton.Image");
            openToolStripButton.ImageTransparentColor = Color.Magenta;
            openToolStripButton.Name = "openToolStripButton";
            openToolStripButton.Size = new Size(23, 22);
            openToolStripButton.Text = "Open";
            openToolStripButton.Click += OpenToolStripMenuItem_Click;
            // 
            // saveToolStripButton
            // 
            saveToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            saveToolStripButton.Image = (Image)resources.GetObject("saveToolStripButton.Image");
            saveToolStripButton.ImageTransparentColor = Color.Magenta;
            saveToolStripButton.Name = "saveToolStripButton";
            saveToolStripButton.Size = new Size(23, 22);
            saveToolStripButton.Text = "Save";
            saveToolStripButton.Click += SaveToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new Size(6, 25);
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(63, 22);
            toolStripLabel1.Text = "COM Port:";
            // 
            // comPortComboBox
            // 
            comPortComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comPortComboBox.Name = "comPortComboBox";
            comPortComboBox.Size = new Size(121, 25);
            comPortComboBox.ToolTipText = "Select COM port";
            // 
            // connectButton
            // 
            connectButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            connectButton.ImageTransparentColor = Color.Magenta;
            connectButton.Name = "connectButton";
            connectButton.Size = new Size(56, 22);
            connectButton.Text = "Connect";
            connectButton.Click += ConnectButton_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new Size(6, 25);
            // 
            // uploadButton
            // 
            uploadButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            uploadButton.ImageTransparentColor = Color.Magenta;
            uploadButton.Name = "uploadButton";
            uploadButton.Size = new Size(49, 22);
            uploadButton.Text = "Upload";
            uploadButton.ToolTipText = "Upload program to Casio (F5)";
            uploadButton.Click += UploadToCasioToolStripMenuItem_Click;
            // 
            // downloadButton
            // 
            downloadButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            downloadButton.ImageTransparentColor = Color.Magenta;
            downloadButton.Name = "downloadButton";
            downloadButton.Size = new Size(65, 22);
            downloadButton.Text = "Download";
            downloadButton.ToolTipText = "Download program from Casio (F6)";
            downloadButton.Click += DownloadFromCasioToolStripMenuItem_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel, comStatusLabel });
            statusStrip.Location = new Point(0, 603);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1000, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(872, 17);
            statusLabel.Spring = true;
            statusLabel.Text = "Ready";
            statusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comStatusLabel
            // 
            comStatusLabel.Name = "comStatusLabel";
            comStatusLabel.Size = new Size(113, 17);
            comStatusLabel.Text = "COM: Disconnected";
            comStatusLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // monacoEditor
            // 
            monacoEditor.AllowExternalDrop = true;
            monacoEditor.CreationProperties = null;
            monacoEditor.DefaultBackgroundColor = Color.White;
            monacoEditor.Dock = DockStyle.Fill;
            monacoEditor.Location = new Point(0, 49);
            monacoEditor.Name = "monacoEditor";
            monacoEditor.Size = new Size(1000, 554);
            monacoEditor.TabIndex = 3;
            monacoEditor.ZoomFactor = 1D;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 625);
            Controls.Add(monacoEditor);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Controls.Add(menuStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "frmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Casio FX-880P Program Editor";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)monacoEditor).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem autoNumberingToolStripMenuItem;
        private ToolStripMenuItem renumberToolStripMenuItem;
        private ToolStripMenuItem comToolStripMenuItem;
        private ToolStripMenuItem refreshPortsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem uploadToCasioToolStripMenuItem;
        private ToolStripMenuItem downloadFromCasioToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem eofSettingsToolStripMenuItem;
        private ToolStripMenuItem eofCtrlZToolStripMenuItem;
        private ToolStripMenuItem eofCtrlDToolStripMenuItem;
        private ToolStripMenuItem eofNoneToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripButton newToolStripButton;
        private ToolStripButton openToolStripButton;
        private ToolStripButton saveToolStripButton;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripLabel toolStripLabel1;
        private ToolStripComboBox comPortComboBox;
        private ToolStripButton connectButton;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton uploadButton;
        private ToolStripButton downloadButton;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel comStatusLabel;
        private MonacoEditor monacoEditor;
    }
}
