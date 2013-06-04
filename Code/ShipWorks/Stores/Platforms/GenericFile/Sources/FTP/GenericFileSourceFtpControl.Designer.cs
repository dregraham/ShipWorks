namespace ShipWorks.Stores.Platforms.GenericFile.Sources.FTP
{
    partial class GenericFileSourceFtpControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericFileSourceFtpControl));
            this.ftpFolder = new ShipWorks.UI.Controls.PathTextBox();
            this.pictureFtp = new System.Windows.Forms.PictureBox();
            this.labelFtpFolder = new System.Windows.Forms.Label();
            this.browseIncoming = new System.Windows.Forms.Button();
            this.labelFtpServer = new System.Windows.Forms.Label();
            this.configureServer = new System.Windows.Forms.Button();
            this.ftpHost = new ShipWorks.UI.Controls.PathTextBox();
            this.actionsControl = new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceActionsSetupControl();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.infotipWeightFormat = new ShipWorks.UI.Controls.InfoTip();
            this.importCantMatchPattern = new System.Windows.Forms.TextBox();
            this.importMustMatch = new System.Windows.Forms.CheckBox();
            this.importCantMatch = new System.Windows.Forms.CheckBox();
            this.importMustMatchPattern = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize) (this.pictureFtp)).BeginInit();
            this.SuspendLayout();
            // 
            // ftpFolder
            // 
            this.ftpFolder.Location = new System.Drawing.Point(164, 33);
            this.ftpFolder.Name = "ftpFolder";
            this.ftpFolder.ReadOnly = true;
            this.ftpFolder.Size = new System.Drawing.Size(273, 21);
            this.ftpFolder.TabIndex = 91;
            // 
            // pictureFtp
            // 
            this.pictureFtp.Image = global::ShipWorks.Properties.Resources.server_client;
            this.pictureFtp.Location = new System.Drawing.Point(23, 4);
            this.pictureFtp.Name = "pictureFtp";
            this.pictureFtp.Size = new System.Drawing.Size(32, 32);
            this.pictureFtp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureFtp.TabIndex = 90;
            this.pictureFtp.TabStop = false;
            // 
            // labelFtpFolder
            // 
            this.labelFtpFolder.AutoSize = true;
            this.labelFtpFolder.Location = new System.Drawing.Point(69, 36);
            this.labelFtpFolder.Name = "labelFtpFolder";
            this.labelFtpFolder.Size = new System.Drawing.Size(85, 13);
            this.labelFtpFolder.TabIndex = 104;
            this.labelFtpFolder.Text = "Incoming folder:";
            // 
            // browseIncoming
            // 
            this.browseIncoming.Location = new System.Drawing.Point(443, 29);
            this.browseIncoming.Name = "browseIncoming";
            this.browseIncoming.Size = new System.Drawing.Size(75, 23);
            this.browseIncoming.TabIndex = 105;
            this.browseIncoming.Text = "Browse...";
            this.browseIncoming.UseVisualStyleBackColor = true;
            this.browseIncoming.Click += new System.EventHandler(this.OnBrowseImportFolder);
            // 
            // labelFtpServer
            // 
            this.labelFtpServer.AutoSize = true;
            this.labelFtpServer.Location = new System.Drawing.Point(91, 9);
            this.labelFtpServer.Name = "labelFtpServer";
            this.labelFtpServer.Size = new System.Drawing.Size(63, 13);
            this.labelFtpServer.TabIndex = 106;
            this.labelFtpServer.Text = "FTP server:";
            // 
            // configureServer
            // 
            this.configureServer.Location = new System.Drawing.Point(443, 3);
            this.configureServer.Name = "configureServer";
            this.configureServer.Size = new System.Drawing.Size(75, 23);
            this.configureServer.TabIndex = 108;
            this.configureServer.Text = "Configure...";
            this.configureServer.UseVisualStyleBackColor = true;
            this.configureServer.Click += new System.EventHandler(this.OnConfigureFtp);
            // 
            // ftpHost
            // 
            this.ftpHost.Location = new System.Drawing.Point(164, 6);
            this.ftpHost.Name = "ftpHost";
            this.ftpHost.ReadOnly = true;
            this.ftpHost.Size = new System.Drawing.Size(273, 21);
            this.ftpHost.TabIndex = 107;
            // 
            // actionsControl
            // 
            this.actionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.actionsControl.Location = new System.Drawing.Point(22, 114);
            this.actionsControl.Name = "actionsControl";
            this.actionsControl.Size = new System.Drawing.Size(490, 162);
            this.actionsControl.TabIndex = 115;
            this.actionsControl.BrowseForSuccessFolder += new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceFolderBrowseEventHandler(this.OnBrowseForActionFolder);
            this.actionsControl.BrowseForErrorFolder += new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceFolderBrowseEventHandler(this.OnBrowseForActionFolder);
            this.actionsControl.SizeChanged += new System.EventHandler(this.OnActionsSizeChanged);
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = resources.GetString("infoTip1.Caption");
            this.infoTip1.Location = new System.Drawing.Point(479, 95);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 114;
            this.infoTip1.Title = "File Name Matching";
            // 
            // infotipWeightFormat
            // 
            this.infotipWeightFormat.Caption = resources.GetString("infotipWeightFormat.Caption");
            this.infotipWeightFormat.Location = new System.Drawing.Point(479, 69);
            this.infotipWeightFormat.Name = "infotipWeightFormat";
            this.infotipWeightFormat.Size = new System.Drawing.Size(12, 12);
            this.infotipWeightFormat.TabIndex = 113;
            this.infotipWeightFormat.Title = "File Name Matching";
            // 
            // importCantMatchPattern
            // 
            this.importCantMatchPattern.Enabled = false;
            this.importCantMatchPattern.Location = new System.Drawing.Point(327, 91);
            this.importCantMatchPattern.Name = "importCantMatchPattern";
            this.importCantMatchPattern.Size = new System.Drawing.Size(146, 21);
            this.importCantMatchPattern.TabIndex = 112;
            // 
            // importMustMatch
            // 
            this.importMustMatch.AutoSize = true;
            this.importMustMatch.Location = new System.Drawing.Point(69, 66);
            this.importMustMatch.Name = "importMustMatch";
            this.importMustMatch.Size = new System.Drawing.Size(250, 17);
            this.importMustMatch.TabIndex = 109;
            this.importMustMatch.Text = "Only import file names that match this pattern:";
            this.importMustMatch.UseVisualStyleBackColor = true;
            this.importMustMatch.CheckedChanged += new System.EventHandler(this.OnCheckedChangedMustMatch);
            // 
            // importCantMatch
            // 
            this.importCantMatch.AutoSize = true;
            this.importCantMatch.Location = new System.Drawing.Point(69, 93);
            this.importCantMatch.Name = "importCantMatch";
            this.importCantMatch.Size = new System.Drawing.Size(214, 17);
            this.importCantMatch.TabIndex = 111;
            this.importCantMatch.Text = "Skip file names that match this pattern:";
            this.importCantMatch.UseVisualStyleBackColor = true;
            this.importCantMatch.CheckedChanged += new System.EventHandler(this.OnCheckedChangedCantMatch);
            // 
            // importMustMatchPattern
            // 
            this.importMustMatchPattern.Enabled = false;
            this.importMustMatchPattern.Location = new System.Drawing.Point(327, 64);
            this.importMustMatchPattern.Name = "importMustMatchPattern";
            this.importMustMatchPattern.Size = new System.Drawing.Size(146, 21);
            this.importMustMatchPattern.TabIndex = 110;
            // 
            // GenericFileSourceFtpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.actionsControl);
            this.Controls.Add(this.infoTip1);
            this.Controls.Add(this.infotipWeightFormat);
            this.Controls.Add(this.importCantMatchPattern);
            this.Controls.Add(this.importMustMatch);
            this.Controls.Add(this.importCantMatch);
            this.Controls.Add(this.importMustMatchPattern);
            this.Controls.Add(this.configureServer);
            this.Controls.Add(this.ftpHost);
            this.Controls.Add(this.labelFtpServer);
            this.Controls.Add(this.browseIncoming);
            this.Controls.Add(this.labelFtpFolder);
            this.Controls.Add(this.ftpFolder);
            this.Controls.Add(this.pictureFtp);
            this.Name = "GenericFileSourceFtpControl";
            this.Size = new System.Drawing.Size(535, 319);
            ((System.ComponentModel.ISupportInitialize) (this.pictureFtp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.PathTextBox ftpFolder;
        private System.Windows.Forms.PictureBox pictureFtp;
        private System.Windows.Forms.Label labelFtpFolder;
        private System.Windows.Forms.Button browseIncoming;
        private System.Windows.Forms.Label labelFtpServer;
        private System.Windows.Forms.Button configureServer;
        private UI.Controls.PathTextBox ftpHost;
        private GenericFileSourceActionsSetupControl actionsControl;
        private UI.Controls.InfoTip infoTip1;
        private UI.Controls.InfoTip infotipWeightFormat;
        private System.Windows.Forms.TextBox importCantMatchPattern;
        private System.Windows.Forms.CheckBox importMustMatch;
        private System.Windows.Forms.CheckBox importCantMatch;
        private System.Windows.Forms.TextBox importMustMatchPattern;
    }
}
