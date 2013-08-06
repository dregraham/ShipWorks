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
            this.components = new System.ComponentModel.Container();
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
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericBackupCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelNotice
            // 
            this.labelNotice.AutoSize = true;
            this.labelNotice.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelNotice.Location = new System.Drawing.Point(96, 24);
            this.labelNotice.Name = "labelNotice";
            this.labelNotice.Size = new System.Drawing.Size(338, 13);
            this.labelNotice.TabIndex = 5;
            this.labelNotice.Text = "(The date and time of the backup will be added to the backup name.)";
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
            this.labelPath.Location = new System.Drawing.Point(3, 48);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(90, 13);
            this.labelPath.TabIndex = 7;
            this.labelPath.Text = "Store backups in:";
            this.labelPath.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Location = new System.Drawing.Point(19, 3);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(74, 13);
            this.labelPrefix.TabIndex = 10;
            this.labelPrefix.Text = "Backup name:";
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
            this.numericBackupCount.Location = new System.Drawing.Point(173, 71);
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
            this.lableBackups.Location = new System.Drawing.Point(225, 73);
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
            this.checkboxLimitBackupsRetained.Size = new System.Drawing.Size(74, 17);
            this.checkboxLimitBackupsRetained.TabIndex = 15;
            this.checkboxLimitBackupsRetained.Text = "Only keep";
            this.checkboxLimitBackupsRetained.UseVisualStyleBackColor = true;
            // 
            // labelCleanup
            // 
            this.labelCleanup.AutoSize = true;
            this.labelCleanup.Location = new System.Drawing.Point(35, 73);
            this.labelCleanup.Name = "labelCleanup";
            this.labelCleanup.Size = new System.Drawing.Size(58, 13);
            this.labelCleanup.TabIndex = 16;
            this.labelCleanup.Text = "Retention:";
            this.labelCleanup.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // BackupDatabaseTaskEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Controls.Add(this.labelCleanup);
            this.Controls.Add(this.checkboxLimitBackupsRetained);
            this.Controls.Add(this.lableBackups);
            this.Controls.Add(this.numericBackupCount);
            this.Controls.Add(this.textPrefix);
            this.Controls.Add(this.labelPrefix);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.backupPath);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.labelNotice);
            this.Name = "BackupDatabaseTaskEditor";
            this.Size = new System.Drawing.Size(456, 99);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.numericBackupCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.ErrorProvider errorProvider;

    }
}
