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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.executeLabel = new System.Windows.Forms.Label();
            this.tokenizedFtpFolder = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.configureServer = new System.Windows.Forms.Button();
            this.ftpHost = new ShipWorks.UI.Controls.PathTextBox();
            this.labelFtpServer = new System.Windows.Forms.Label();
            this.browseFtpFolder = new System.Windows.Forms.Button();
            this.tokenizedFtpFilename = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // executeLabel
            // 
            this.executeLabel.AutoSize = true;
            this.executeLabel.Location = new System.Drawing.Point(7, 65);
            this.executeLabel.Name = "executeLabel";
            this.executeLabel.Size = new System.Drawing.Size(62, 13);
            this.executeLabel.TabIndex = 1;
            this.executeLabel.Text = "FTP Folder:";
            // 
            // tokenizedFtpFolder
            // 
            this.tokenizedFtpFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tokenizedFtpFolder.Location = new System.Drawing.Point(79, 60);
            this.tokenizedFtpFolder.MaxLength = 32767;
            this.tokenizedFtpFolder.Multiline = true;
            this.tokenizedFtpFolder.Name = "tokenizedFtpFolder";
            this.tokenizedFtpFolder.Size = new System.Drawing.Size(273, 26);
            this.tokenizedFtpFolder.TabIndex = 2;
            this.tokenizedFtpFolder.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
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
            this.browseFtpFolder.Location = new System.Drawing.Point(358, 61);
            this.browseFtpFolder.Name = "browseFtpFolder";
            this.browseFtpFolder.Size = new System.Drawing.Size(75, 23);
            this.browseFtpFolder.TabIndex = 112;
            this.browseFtpFolder.Text = "Browse...";
            this.browseFtpFolder.UseVisualStyleBackColor = true;
            this.browseFtpFolder.Click += new System.EventHandler(this.OnBrowseFtp);
            // 
            // tokenizedFtpFilename
            // 
            this.tokenizedFtpFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tokenizedFtpFilename.Location = new System.Drawing.Point(79, 94);
            this.tokenizedFtpFilename.MaxLength = 32767;
            this.tokenizedFtpFilename.Multiline = true;
            this.tokenizedFtpFilename.Name = "tokenizedFtpFilename";
            this.tokenizedFtpFilename.Size = new System.Drawing.Size(273, 26);
            this.tokenizedFtpFilename.TabIndex = 114;
            this.tokenizedFtpFilename.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 113;
            this.label1.Text = "Filename:";
            // 
            // FtpFileTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tokenizedFtpFilename);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.browseFtpFolder);
            this.Controls.Add(this.configureServer);
            this.Controls.Add(this.ftpHost);
            this.Controls.Add(this.labelFtpServer);
            this.Controls.Add(this.tokenizedFtpFolder);
            this.Controls.Add(this.executeLabel);
            this.Name = "FtpFileTaskEditor";
            this.Size = new System.Drawing.Size(441, 127);
            this.Controls.SetChildIndex(this.executeLabel, 0);
            this.Controls.SetChildIndex(this.tokenizedFtpFolder, 0);
            this.Controls.SetChildIndex(this.labelFtpServer, 0);
            this.Controls.SetChildIndex(this.ftpHost, 0);
            this.Controls.SetChildIndex(this.configureServer, 0);
            this.Controls.SetChildIndex(this.browseFtpFolder, 0);
            this.Controls.SetChildIndex(this.labelTemplate, 0);
            this.Controls.SetChildIndex(this.templateCombo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.tokenizedFtpFilename, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label executeLabel;
        private Templates.Tokens.TemplateTokenTextBox tokenizedFtpFolder;
        private System.Windows.Forms.Button configureServer;
        private ShipWorks.UI.Controls.PathTextBox ftpHost;
        private System.Windows.Forms.Label labelFtpServer;
        private System.Windows.Forms.Button browseFtpFolder;
        private Templates.Tokens.TemplateTokenTextBox tokenizedFtpFilename;
        private System.Windows.Forms.Label label1;

    }
}
