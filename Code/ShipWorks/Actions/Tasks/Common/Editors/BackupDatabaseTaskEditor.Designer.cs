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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelNotice = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.backupPath = new System.Windows.Forms.TextBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.textPrefix = new System.Windows.Forms.TextBox();
            this.numericBackupCount = new System.Windows.Forms.NumericUpDown();
            this.lableBackups = new System.Windows.Forms.Label();
            this.checkboxOnlyKeep = new System.Windows.Forms.CheckBox();
            this.labelCleanup = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBackupCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox1.Location = new System.Drawing.Point(26, 122);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // labelNotice
            // 
            this.labelNotice.Location = new System.Drawing.Point(45, 122);
            this.labelNotice.Name = "labelNotice";
            this.labelNotice.Size = new System.Drawing.Size(318, 16);
            this.labelNotice.TabIndex = 5;
            this.labelNotice.Text = "The date and time of the backup will be appended to the prefix.\r\n";
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(236, 58);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 9;
            this.browse.Text = "Browse...";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // backupPath
            // 
            this.backupPath.Location = new System.Drawing.Point(64, 35);
            this.backupPath.Name = "backupPath";
            this.backupPath.ReadOnly = true;
            this.backupPath.Size = new System.Drawing.Size(247, 21);
            this.backupPath.TabIndex = 8;
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(25, 38);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(33, 13);
            this.labelPath.TabIndex = 7;
            this.labelPath.Text = "Path:";
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Location = new System.Drawing.Point(25, 11);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(39, 13);
            this.labelPrefix.TabIndex = 10;
            this.labelPrefix.Text = "Prefix:";
            // 
            // textPrefix
            // 
            this.textPrefix.Location = new System.Drawing.Point(70, 8);
            this.textPrefix.Name = "textPrefix";
            this.textPrefix.Size = new System.Drawing.Size(141, 21);
            this.textPrefix.TabIndex = 11;
            // 
            // numericBackupCount
            // 
            this.numericBackupCount.Location = new System.Drawing.Point(157, 86);
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
            this.lableBackups.Location = new System.Drawing.Point(209, 88);
            this.lableBackups.Name = "lableBackups";
            this.lableBackups.Size = new System.Drawing.Size(102, 13);
            this.lableBackups.TabIndex = 14;
            this.lableBackups.Text = "successful backups.";
            // 
            // checkboxOnlyKeep
            // 
            this.checkboxOnlyKeep.AutoSize = true;
            this.checkboxOnlyKeep.Location = new System.Drawing.Point(81, 87);
            this.checkboxOnlyKeep.Name = "checkboxOnlyKeep";
            this.checkboxOnlyKeep.Size = new System.Drawing.Size(74, 17);
            this.checkboxOnlyKeep.TabIndex = 15;
            this.checkboxOnlyKeep.Text = "Only keep";
            this.checkboxOnlyKeep.UseVisualStyleBackColor = true;
            // 
            // labelCleanup
            // 
            this.labelCleanup.AutoSize = true;
            this.labelCleanup.Location = new System.Drawing.Point(25, 88);
            this.labelCleanup.Name = "labelCleanup";
            this.labelCleanup.Size = new System.Drawing.Size(50, 13);
            this.labelCleanup.TabIndex = 16;
            this.labelCleanup.Text = "Cleanup:";
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
            this.Controls.Add(this.checkboxOnlyKeep);
            this.Controls.Add(this.lableBackups);
            this.Controls.Add(this.numericBackupCount);
            this.Controls.Add(this.textPrefix);
            this.Controls.Add(this.labelPrefix);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.backupPath);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.labelNotice);
            this.Name = "BackupDatabaseTaskEditor";
            this.Size = new System.Drawing.Size(366, 148);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericBackupCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelNotice;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.TextBox backupPath;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label labelPrefix;
        private System.Windows.Forms.TextBox textPrefix;
        private System.Windows.Forms.NumericUpDown numericBackupCount;
        private System.Windows.Forms.Label lableBackups;
        private System.Windows.Forms.CheckBox checkboxOnlyKeep;
        private System.Windows.Forms.Label labelCleanup;
        private System.Windows.Forms.ErrorProvider errorProvider;

    }
}
