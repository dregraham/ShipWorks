namespace ShipWorks.Stores.Platforms.GenericFile.Sources.Disk
{
    partial class GenericFileSourceDiskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenericFileSourceDiskControl));
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.infotipWeightFormat = new ShipWorks.UI.Controls.InfoTip();
            this.folderPath = new ShipWorks.UI.Controls.PathTextBox();
            this.importCantMatchPattern = new System.Windows.Forms.TextBox();
            this.importMustMatch = new System.Windows.Forms.CheckBox();
            this.importCantMatch = new System.Windows.Forms.CheckBox();
            this.folderBrowse = new System.Windows.Forms.Button();
            this.pictureFolder = new System.Windows.Forms.PictureBox();
            this.importMustMatchPattern = new System.Windows.Forms.TextBox();
            this.actionsControl = new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceActionsSetupControl();
            ((System.ComponentModel.ISupportInitialize) (this.pictureFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = resources.GetString("infoTip1.Caption");
            this.infoTip1.Location = new System.Drawing.Point(475, 68);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 97;
            this.infoTip1.Title = "File Name Matching";
            // 
            // infotipWeightFormat
            // 
            this.infotipWeightFormat.Caption = resources.GetString("infotipWeightFormat.Caption");
            this.infotipWeightFormat.Location = new System.Drawing.Point(475, 42);
            this.infotipWeightFormat.Name = "infotipWeightFormat";
            this.infotipWeightFormat.Size = new System.Drawing.Size(12, 12);
            this.infotipWeightFormat.TabIndex = 96;
            this.infotipWeightFormat.Title = "File Name Matching";
            // 
            // folderPath
            // 
            this.folderPath.Location = new System.Drawing.Point(61, 6);
            this.folderPath.Name = "folderPath";
            this.folderPath.ReadOnly = true;
            this.folderPath.Size = new System.Drawing.Size(295, 21);
            this.folderPath.TabIndex = 86;
            // 
            // importCantMatchPattern
            // 
            this.importCantMatchPattern.Enabled = false;
            this.importCantMatchPattern.Location = new System.Drawing.Point(323, 64);
            this.importCantMatchPattern.Name = "importCantMatchPattern";
            this.importCantMatchPattern.Size = new System.Drawing.Size(146, 21);
            this.importCantMatchPattern.TabIndex = 91;
            // 
            // importMustMatch
            // 
            this.importMustMatch.AutoSize = true;
            this.importMustMatch.Location = new System.Drawing.Point(65, 39);
            this.importMustMatch.Name = "importMustMatch";
            this.importMustMatch.Size = new System.Drawing.Size(250, 17);
            this.importMustMatch.TabIndex = 88;
            this.importMustMatch.Text = "Only import file names that match this pattern:";
            this.importMustMatch.UseVisualStyleBackColor = true;
            this.importMustMatch.CheckedChanged += new System.EventHandler(this.OnCheckedChangedMustMatch);
            // 
            // importCantMatch
            // 
            this.importCantMatch.AutoSize = true;
            this.importCantMatch.Location = new System.Drawing.Point(65, 66);
            this.importCantMatch.Name = "importCantMatch";
            this.importCantMatch.Size = new System.Drawing.Size(214, 17);
            this.importCantMatch.TabIndex = 90;
            this.importCantMatch.Text = "Skip file names that match this pattern:";
            this.importCantMatch.UseVisualStyleBackColor = true;
            this.importCantMatch.CheckedChanged += new System.EventHandler(this.OnCheckedChangedCantMatch);
            // 
            // folderBrowse
            // 
            this.folderBrowse.Location = new System.Drawing.Point(362, 4);
            this.folderBrowse.Name = "folderBrowse";
            this.folderBrowse.Size = new System.Drawing.Size(75, 23);
            this.folderBrowse.TabIndex = 87;
            this.folderBrowse.Text = "Browse...";
            this.folderBrowse.UseVisualStyleBackColor = true;
            this.folderBrowse.Click += new System.EventHandler(this.OnBrowseImportFolder);
            // 
            // pictureFolder
            // 
            this.pictureFolder.Image = global::ShipWorks.Properties.Resources.folder_document;
            this.pictureFolder.Location = new System.Drawing.Point(23, 4);
            this.pictureFolder.Name = "pictureFolder";
            this.pictureFolder.Size = new System.Drawing.Size(32, 32);
            this.pictureFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureFolder.TabIndex = 85;
            this.pictureFolder.TabStop = false;
            // 
            // importMustMatchPattern
            // 
            this.importMustMatchPattern.Enabled = false;
            this.importMustMatchPattern.Location = new System.Drawing.Point(323, 37);
            this.importMustMatchPattern.Name = "importMustMatchPattern";
            this.importMustMatchPattern.Size = new System.Drawing.Size(146, 21);
            this.importMustMatchPattern.TabIndex = 89;
            // 
            // actionsControl
            // 
            this.actionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.actionsControl.Location = new System.Drawing.Point(18, 87);
            this.actionsControl.Name = "actionsControl";
            this.actionsControl.Size = new System.Drawing.Size(490, 162);
            this.actionsControl.TabIndex = 98;
            this.actionsControl.BrowseForSuccessFolder += new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceFolderBrowseEventHandler(this.OnBrowseForSuccessFolder);
            this.actionsControl.BrowseForErrorFolder += new ShipWorks.Stores.Platforms.GenericFile.Sources.GenericFileSourceFolderBrowseEventHandler(this.OnBrowseForErrorFolder);
            this.actionsControl.SizeChanged += new System.EventHandler(this.OnActionsSizeChanged);
            // 
            // GenericFileSourceDiskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.actionsControl);
            this.Controls.Add(this.infoTip1);
            this.Controls.Add(this.infotipWeightFormat);
            this.Controls.Add(this.folderPath);
            this.Controls.Add(this.importCantMatchPattern);
            this.Controls.Add(this.importMustMatch);
            this.Controls.Add(this.importCantMatch);
            this.Controls.Add(this.folderBrowse);
            this.Controls.Add(this.pictureFolder);
            this.Controls.Add(this.importMustMatchPattern);
            this.Name = "GenericFileSourceDiskControl";
            this.Size = new System.Drawing.Size(511, 259);
            ((System.ComponentModel.ISupportInitialize) (this.pictureFolder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UI.Controls.PathTextBox folderPath;
        private System.Windows.Forms.TextBox importCantMatchPattern;
        private System.Windows.Forms.CheckBox importMustMatch;
        private System.Windows.Forms.CheckBox importCantMatch;
        private System.Windows.Forms.Button folderBrowse;
        private System.Windows.Forms.PictureBox pictureFolder;
        private System.Windows.Forms.TextBox importMustMatchPattern;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private UI.Controls.InfoTip infotipWeightFormat;
        private UI.Controls.InfoTip infoTip1;
        private GenericFileSourceActionsSetupControl actionsControl;
    }
}
