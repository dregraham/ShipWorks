namespace ShipWorks.Data.Administration
{
    partial class DatabaseRestoreDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseRestoreDlg));
            this.cancel = new System.Windows.Forms.Button();
            this.restore = new System.Windows.Forms.Button();
            this.browse = new System.Windows.Forms.Button();
            this.labelBackupFile = new System.Windows.Forms.Label();
            this.sep = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.subtitle = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.openBackupFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupInfo = new System.Windows.Forms.GroupBox();
            this.labelNote = new System.Windows.Forms.Label();
            this.labelNote1 = new System.Windows.Forms.Label();
            this.warningIcon = new System.Windows.Forms.PictureBox();
            this.labelNote2 = new System.Windows.Forms.Label();
            this.backupFile = new ShipWorks.UI.Controls.PathTextBox();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).BeginInit();
            this.groupInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(408, 247);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 7;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // restore
            // 
            this.restore.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.restore.Location = new System.Drawing.Point(327, 247);
            this.restore.Name = "restore";
            this.restore.Size = new System.Drawing.Size(75, 23);
            this.restore.TabIndex = 6;
            this.restore.Text = "Restore";
            this.restore.UseVisualStyleBackColor = true;
            this.restore.Click += new System.EventHandler(this.OnRestore);
            // 
            // browse
            // 
            this.browse.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browse.Location = new System.Drawing.Point(408, 118);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 4;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // labelBackupFile
            // 
            this.labelBackupFile.AutoSize = true;
            this.labelBackupFile.Location = new System.Drawing.Point(20, 72);
            this.labelBackupFile.Name = "labelBackupFile";
            this.labelBackupFile.Size = new System.Drawing.Size(242, 13);
            this.labelBackupFile.TabIndex = 2;
            this.labelBackupFile.Text = "Select the ShipWorks backup file to restore from:";
            // 
            // sep
            // 
            this.sep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sep.Dock = System.Windows.Forms.DockStyle.Top;
            this.sep.Location = new System.Drawing.Point(0, 55);
            this.sep.Name = "sep";
            this.sep.Size = new System.Drawing.Size(493, 3);
            this.sep.TabIndex = 1;
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.headerImage);
            this.headerPanel.Controls.Add(this.subtitle);
            this.headerPanel.Controls.Add(this.title);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(493, 55);
            this.headerPanel.TabIndex = 0;
            // 
            // headerImage
            // 
            this.headerImage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.headerImage.Image = ((System.Drawing.Image) (resources.GetObject("headerImage.Image")));
            this.headerImage.Location = new System.Drawing.Point(431, 3);
            this.headerImage.Name = "headerImage";
            this.headerImage.Size = new System.Drawing.Size(48, 48);
            this.headerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.headerImage.TabIndex = 5;
            this.headerImage.TabStop = false;
            // 
            // subtitle
            // 
            this.subtitle.Location = new System.Drawing.Point(40, 30);
            this.subtitle.Name = "subtitle";
            this.subtitle.Size = new System.Drawing.Size(338, 14);
            this.subtitle.TabIndex = 1;
            this.subtitle.Text = "Restore ShipWorks from a backup.";
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.title.Location = new System.Drawing.Point(20, 12);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(296, 14);
            this.title.TabIndex = 0;
            this.title.Text = "Restore Backup";
            // 
            // openBackupFileDialog
            // 
            this.openBackupFileDialog.DefaultExt = "swb";
            this.openBackupFileDialog.Filter = "ShipWorks Backup Files (*.swb)|*.swb";
            // 
            // groupInfo
            // 
            this.groupInfo.Controls.Add(this.labelNote);
            this.groupInfo.Controls.Add(this.labelNote1);
            this.groupInfo.Controls.Add(this.warningIcon);
            this.groupInfo.Controls.Add(this.labelNote2);
            this.groupInfo.Location = new System.Drawing.Point(21, 157);
            this.groupInfo.Name = "groupInfo";
            this.groupInfo.Size = new System.Drawing.Size(462, 70);
            this.groupInfo.TabIndex = 5;
            this.groupInfo.TabStop = false;
            this.groupInfo.Text = "Important Information";
            // 
            // labelNote
            // 
            this.labelNote.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelNote.Location = new System.Drawing.Point(34, 26);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(72, 22);
            this.labelNote.TabIndex = 0;
            this.labelNote.Text = "Important:";
            // 
            // labelNote1
            // 
            this.labelNote1.Location = new System.Drawing.Point(108, 26);
            this.labelNote1.Name = "labelNote1";
            this.labelNote1.Size = new System.Drawing.Size(352, 18);
            this.labelNote1.TabIndex = 1;
            this.labelNote1.Text = "- The data being restored will overwrite any existing data.";
            // 
            // warningIcon
            // 
            this.warningIcon.Image = ((System.Drawing.Image) (resources.GetObject("warningIcon.Image")));
            this.warningIcon.Location = new System.Drawing.Point(16, 24);
            this.warningIcon.Name = "warningIcon";
            this.warningIcon.Size = new System.Drawing.Size(16, 16);
            this.warningIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.warningIcon.TabIndex = 158;
            this.warningIcon.TabStop = false;
            // 
            // labelNote2
            // 
            this.labelNote2.Location = new System.Drawing.Point(108, 44);
            this.labelNote2.Name = "labelNote2";
            this.labelNote2.Size = new System.Drawing.Size(352, 18);
            this.labelNote2.TabIndex = 2;
            this.labelNote2.Text = "- Any other users currently using ShipWorks will be disconnected.";
            // 
            // backupFile
            // 
            this.backupFile.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backupFile.Location = new System.Drawing.Point(21, 88);
            this.backupFile.Name = "backupFile";
            this.backupFile.ReadOnly = true;
            this.backupFile.Size = new System.Drawing.Size(462, 21);
            this.backupFile.TabIndex = 3;
            // 
            // DatabaseRestoreDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(493, 282);
            this.Controls.Add(this.groupInfo);
            this.Controls.Add(this.backupFile);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.restore);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.labelBackupFile);
            this.Controls.Add(this.sep);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseRestoreDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Restore Backup";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).EndInit();
            this.groupInfo.ResumeLayout(false);
            this.groupInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.warningIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Controls.PathTextBox backupFile;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button restore;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.Label labelBackupFile;
        private System.Windows.Forms.Label sep;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.PictureBox headerImage;
        private System.Windows.Forms.Label subtitle;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.OpenFileDialog openBackupFileDialog;
        private System.Windows.Forms.GroupBox groupInfo;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Label labelNote1;
        private System.Windows.Forms.PictureBox warningIcon;
        private System.Windows.Forms.Label labelNote2;
    }
}