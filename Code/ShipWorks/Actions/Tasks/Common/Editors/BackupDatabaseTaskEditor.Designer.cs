namespace ShipWorks.Actions.Tasks.Common.Editors
{
    partial class BackupDatabaseTaskEditor
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
            this.labelNotice = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.backupPath = new System.Windows.Forms.TextBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.textPrefix = new System.Windows.Forms.TextBox();
            this.numericBackupCount = new System.Windows.Forms.NumericUpDown();
            this.lableBackups = new System.Windows.Forms.Label();
            this.checkboxLimitBackupsRetained = new System.Windows.Forms.CheckBox();
            this.labelCleanup = new System.Windows.Forms.Label();
            this.editorPanel = new System.Windows.Forms.Panel();
            this.databaseConfigurationNotification = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.messagePanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericBackupCount)).BeginInit();
            this.editorPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.messagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelNotice
            // 
            this.labelNotice.AutoSize = true;
            this.labelNotice.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelNotice.Location = new System.Drawing.Point(96, 24);
            this.labelNotice.Name = "labelNotice";
            this.labelNotice.Size = new System.Drawing.Size(355, 13);
            this.labelNotice.TabIndex = 5;
            this.labelNotice.Text = "(The date and time of the backup will be added to the backup file name.)";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(352, 43);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 9;
            this.browse.Text = "Browse...";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // backupPath
            // 
            this.backupPath.Location = new System.Drawing.Point(99, 45);
            this.backupPath.Name = "backupPath";
            this.backupPath.ReadOnly = true;
            this.backupPath.Size = new System.Drawing.Size(247, 21);
            this.backupPath.TabIndex = 8;
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(18, 48);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(76, 13);
            this.labelPath.TabIndex = 7;
            this.labelPath.Text = "Backup folder:";
            this.labelPath.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Location = new System.Drawing.Point(3, 3);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(91, 13);
            this.labelPrefix.TabIndex = 10;
            this.labelPrefix.Text = "Backup file name:";
            this.labelPrefix.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textPrefix
            // 
            this.textPrefix.Location = new System.Drawing.Point(99, 0);
            this.textPrefix.Name = "textPrefix";
            this.textPrefix.Size = new System.Drawing.Size(243, 21);
            this.textPrefix.TabIndex = 11;
            // 
            // numericBackupCount
            // 
            this.numericBackupCount.Location = new System.Drawing.Point(212, 71);
            this.numericBackupCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericBackupCount.Name = "numericBackupCount";
            this.numericBackupCount.Size = new System.Drawing.Size(47, 21);
            this.numericBackupCount.TabIndex = 13;
            this.numericBackupCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lableBackups
            // 
            this.lableBackups.AutoSize = true;
            this.lableBackups.Location = new System.Drawing.Point(265, 73);
            this.lableBackups.Name = "lableBackups";
            this.lableBackups.Size = new System.Drawing.Size(102, 13);
            this.lableBackups.TabIndex = 14;
            this.lableBackups.Text = "successful backups.";
            // 
            // checkboxLimitBackupsRetained
            // 
            this.checkboxLimitBackupsRetained.AutoSize = true;
            this.checkboxLimitBackupsRetained.Location = new System.Drawing.Point(99, 72);
            this.checkboxLimitBackupsRetained.Name = "checkboxLimitBackupsRetained";
            this.checkboxLimitBackupsRetained.Size = new System.Drawing.Size(113, 17);
            this.checkboxLimitBackupsRetained.TabIndex = 15;
            this.checkboxLimitBackupsRetained.Text = "Only keep the last";
            this.checkboxLimitBackupsRetained.UseVisualStyleBackColor = true;
            // 
            // labelCleanup
            // 
            this.labelCleanup.AutoSize = true;
            this.labelCleanup.Location = new System.Drawing.Point(36, 73);
            this.labelCleanup.Name = "labelCleanup";
            this.labelCleanup.Size = new System.Drawing.Size(58, 13);
            this.labelCleanup.TabIndex = 16;
            this.labelCleanup.Text = "Retention:";
            this.labelCleanup.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // editorPanel
            // 
            this.editorPanel.Controls.Add(this.labelCleanup);
            this.editorPanel.Controls.Add(this.checkboxLimitBackupsRetained);
            this.editorPanel.Controls.Add(this.lableBackups);
            this.editorPanel.Controls.Add(this.numericBackupCount);
            this.editorPanel.Controls.Add(this.textPrefix);
            this.editorPanel.Controls.Add(this.labelPrefix);
            this.editorPanel.Controls.Add(this.browse);
            this.editorPanel.Controls.Add(this.backupPath);
            this.editorPanel.Controls.Add(this.labelPath);
            this.editorPanel.Controls.Add(this.labelNotice);
            this.editorPanel.Location = new System.Drawing.Point(0, 0);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(453, 96);
            this.editorPanel.TabIndex = 17;
            // 
            // databaseConfigurationNotification
            // 
            this.databaseConfigurationNotification.AutoSize = true;
            this.databaseConfigurationNotification.Location = new System.Drawing.Point(21, 4);
            this.databaseConfigurationNotification.Name = "databaseConfigurationNotification";
            this.databaseConfigurationNotification.Size = new System.Drawing.Size(408, 13);
            this.databaseConfigurationNotification.TabIndex = 19;
            this.databaseConfigurationNotification.Text = "The \'{0}\' task can only be configured on the computer running your database ({1})" +
    ".";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox1.Location = new System.Drawing.Point(1, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // messagePanel
            // 
            this.messagePanel.Controls.Add(this.databaseConfigurationNotification);
            this.messagePanel.Controls.Add(this.pictureBox1);
            this.messagePanel.Location = new System.Drawing.Point(0, 95);
            this.messagePanel.Name = "messagePanel";
            this.messagePanel.Size = new System.Drawing.Size(453, 21);
            this.messagePanel.TabIndex = 20;
            // 
            // BackupDatabaseTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.messagePanel);
            this.Controls.Add(this.editorPanel);
            this.Name = "BackupDatabaseTaskEditor";
            this.Size = new System.Drawing.Size(456, 120);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.numericBackupCount)).EndInit();
            this.editorPanel.ResumeLayout(false);
            this.editorPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.messagePanel.ResumeLayout(false);
            this.messagePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelNotice;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.TextBox backupPath;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.Label labelPrefix;
        private System.Windows.Forms.TextBox textPrefix;
        private System.Windows.Forms.NumericUpDown numericBackupCount;
        private System.Windows.Forms.Label lableBackups;
        private System.Windows.Forms.CheckBox checkboxLimitBackupsRetained;
        private System.Windows.Forms.Label labelCleanup;
        private System.Windows.Forms.Panel editorPanel;
        private System.Windows.Forms.Label databaseConfigurationNotification;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel messagePanel;

    }
}
