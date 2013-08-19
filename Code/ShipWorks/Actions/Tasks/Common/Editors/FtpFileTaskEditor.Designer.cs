namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class FtpFileTaskEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.executeLabel = new System.Windows.Forms.Label();
            this.tokenizedFtpFolderFilename = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.configureServer = new System.Windows.Forms.Button();
            this.ftpHost = new ShipWorks.UI.Controls.PathTextBox();
            this.labelFtpServer = new System.Windows.Forms.Label();
            this.browseFtpFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // executeLabel
            // 
            this.executeLabel.AutoSize = true;
            this.executeLabel.Location = new System.Drawing.Point(6, 53);
            this.executeLabel.Name = "executeLabel";
            this.executeLabel.Size = new System.Drawing.Size(134, 13);
            this.executeLabel.TabIndex = 1;
            this.executeLabel.Text = "Output path and filename:";
            // 
            // tokenizedFtpFolderFilename
            // 
            this.tokenizedFtpFolderFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tokenizedFtpFolderFilename.Location = new System.Drawing.Point(9, 69);
            this.tokenizedFtpFolderFilename.MaxLength = 32767;
            this.tokenizedFtpFolderFilename.Multiline = true;
            this.tokenizedFtpFolderFilename.Name = "tokenizedFtpFolderFilename";
            this.tokenizedFtpFolderFilename.Size = new System.Drawing.Size(343, 26);
            this.tokenizedFtpFolderFilename.TabIndex = 2;
            this.tokenizedFtpFolderFilename.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // configureServer
            // 
            this.configureServer.Location = new System.Drawing.Point(358, 27);
            this.configureServer.Name = "configureServer";
            this.configureServer.Size = new System.Drawing.Size(75, 23);
            this.configureServer.TabIndex = 111;
            this.configureServer.Text = "Configure...";
            this.configureServer.UseVisualStyleBackColor = true;
            this.configureServer.Click += new System.EventHandler(this.OnConfigureFtp);
            // 
            // ftpHost
            // 
            this.ftpHost.Location = new System.Drawing.Point(79, 30);
            this.ftpHost.Name = "ftpHost";
            this.ftpHost.ReadOnly = true;
            this.ftpHost.Size = new System.Drawing.Size(273, 21);
            this.ftpHost.TabIndex = 110;
            // 
            // labelFtpServer
            // 
            this.labelFtpServer.AutoSize = true;
            this.labelFtpServer.Location = new System.Drawing.Point(6, 33);
            this.labelFtpServer.Name = "labelFtpServer";
            this.labelFtpServer.Size = new System.Drawing.Size(63, 13);
            this.labelFtpServer.TabIndex = 109;
            this.labelFtpServer.Text = "FTP server:";
            // 
            // browseFtpFolder
            // 
            this.browseFtpFolder.Location = new System.Drawing.Point(358, 69);
            this.browseFtpFolder.Name = "browseFtpFolder";
            this.browseFtpFolder.Size = new System.Drawing.Size(75, 23);
            this.browseFtpFolder.TabIndex = 112;
            this.browseFtpFolder.Text = "Browse...";
            this.browseFtpFolder.UseVisualStyleBackColor = true;
            this.browseFtpFolder.Click += new System.EventHandler(this.OnBrowseFtp);
            // 
            // FtpFileTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browseFtpFolder);
            this.Controls.Add(this.configureServer);
            this.Controls.Add(this.ftpHost);
            this.Controls.Add(this.labelFtpServer);
            this.Controls.Add(this.tokenizedFtpFolderFilename);
            this.Controls.Add(this.executeLabel);
            this.Name = "FtpFileTaskEditor";
            this.Size = new System.Drawing.Size(441, 154);
            this.Controls.SetChildIndex(this.executeLabel, 0);
            this.Controls.SetChildIndex(this.tokenizedFtpFolderFilename, 0);
            this.Controls.SetChildIndex(this.labelFtpServer, 0);
            this.Controls.SetChildIndex(this.ftpHost, 0);
            this.Controls.SetChildIndex(this.configureServer, 0);
            this.Controls.SetChildIndex(this.browseFtpFolder, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label executeLabel;
        private Templates.Tokens.TemplateTokenTextBox tokenizedFtpFolderFilename;
        private System.Windows.Forms.Button configureServer;
        private ShipWorks.UI.Controls.PathTextBox ftpHost;
        private System.Windows.Forms.Label labelFtpServer;
        private System.Windows.Forms.Button browseFtpFolder;

    }
}
