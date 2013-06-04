namespace ShipWorks.Data.Administration
{
    partial class DatabaseBackupDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseBackupDlg));
            this.sep = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.subtitle = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.labelBackupFile = new System.Windows.Forms.Label();
            this.browse = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.backup = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.backupFile = new ShipWorks.UI.Controls.PathTextBox();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).BeginInit();
            this.SuspendLayout();
            // 
            // sep
            // 
            this.sep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sep.Dock = System.Windows.Forms.DockStyle.Top;
            this.sep.Location = new System.Drawing.Point(0, 55);
            this.sep.Name = "sep";
            this.sep.Size = new System.Drawing.Size(495, 3);
            this.sep.TabIndex = 0;
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
            this.headerPanel.Size = new System.Drawing.Size(495, 55);
            this.headerPanel.TabIndex = 6;
            // 
            // headerImage
            // 
            this.headerImage.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.headerImage.Image = ((System.Drawing.Image) (resources.GetObject("headerImage.Image")));
            this.headerImage.Location = new System.Drawing.Point(433, 3);
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
            this.subtitle.Text = "Backup all of your ShipWorks data.";
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.title.Location = new System.Drawing.Point(20, 12);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(296, 14);
            this.title.TabIndex = 0;
            this.title.Text = "Backup ShipWorks";
            // 
            // labelBackupFile
            // 
            this.labelBackupFile.AutoSize = true;
            this.labelBackupFile.Location = new System.Drawing.Point(20, 73);
            this.labelBackupFile.Name = "labelBackupFile";
            this.labelBackupFile.Size = new System.Drawing.Size(236, 13);
            this.labelBackupFile.TabIndex = 1;
            this.labelBackupFile.Text = "Choose a name and location for the backup file:";
            // 
            // browse
            // 
            this.browse.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browse.Location = new System.Drawing.Point(408, 119);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(75, 23);
            this.browse.TabIndex = 3;
            this.browse.Text = "Browse...";
            this.browse.Click += new System.EventHandler(this.OnBrowseBackupFile);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "swb";
            this.saveFileDialog.Filter = "ShipWorks Backup (*.swb)|*.swb";
            this.saveFileDialog.Title = "ShipWorks Backup File";
            // 
            // backup
            // 
            this.backup.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backup.Location = new System.Drawing.Point(327, 164);
            this.backup.Name = "backup";
            this.backup.Size = new System.Drawing.Size(75, 23);
            this.backup.TabIndex = 4;
            this.backup.Text = "Backup";
            this.backup.UseVisualStyleBackColor = true;
            this.backup.Click += new System.EventHandler(this.OnBackup);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(408, 164);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // backupFile
            // 
            this.backupFile.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.backupFile.Location = new System.Drawing.Point(21, 89);
            this.backupFile.Name = "backupFile";
            this.backupFile.ReadOnly = true;
            this.backupFile.Size = new System.Drawing.Size(462, 21);
            this.backupFile.TabIndex = 2;
            // 
            // DatabaseBackupDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(495, 198);
            this.Controls.Add(this.backupFile);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.backup);
            this.Controls.Add(this.browse);
            this.Controls.Add(this.labelBackupFile);
            this.Controls.Add(this.sep);
            this.Controls.Add(this.headerPanel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseBackupDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Backup ShipWorks";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sep;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.PictureBox headerImage;
        private System.Windows.Forms.Label subtitle;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Label labelBackupFile;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button backup;
        private System.Windows.Forms.Button cancel;
        private ShipWorks.UI.Controls.PathTextBox backupFile;
    }
}