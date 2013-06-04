namespace ShipWorks.Data.Administration.SqlServerSetup
{
    partial class WindowsInstallerDownloadPage
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
            this.bytesWindowsInstaller = new System.Windows.Forms.Label();
            this.downloadWindowsInstaller = new System.Windows.Forms.Button();
            this.progressWindowsInstaller = new System.Windows.Forms.ProgressBar();
            this.labeDownloadWindowsInstallerInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bytesWindowsInstaller
            // 
            this.bytesWindowsInstaller.Location = new System.Drawing.Point(372, 72);
            this.bytesWindowsInstaller.Name = "bytesWindowsInstaller";
            this.bytesWindowsInstaller.Size = new System.Drawing.Size(145, 23);
            this.bytesWindowsInstaller.TabIndex = 19;
            this.bytesWindowsInstaller.Text = "(0 of 8052 KB)";
            this.bytesWindowsInstaller.Visible = false;
            // 
            // downloadWindowsInstaller
            // 
            this.downloadWindowsInstaller.Location = new System.Drawing.Point(44, 68);
            this.downloadWindowsInstaller.Name = "downloadWindowsInstaller";
            this.downloadWindowsInstaller.Size = new System.Drawing.Size(75, 23);
            this.downloadWindowsInstaller.TabIndex = 18;
            this.downloadWindowsInstaller.Text = "Download";
            this.downloadWindowsInstaller.Click += new System.EventHandler(this.OnDownload);
            // 
            // progressWindowsInstaller
            // 
            this.progressWindowsInstaller.Location = new System.Drawing.Point(124, 68);
            this.progressWindowsInstaller.Name = "progressWindowsInstaller";
            this.progressWindowsInstaller.Size = new System.Drawing.Size(246, 23);
            this.progressWindowsInstaller.TabIndex = 17;
            // 
            // labeDownloadWindowsInstallerInfo
            // 
            this.labeDownloadWindowsInstallerInfo.Location = new System.Drawing.Point(23, 6);
            this.labeDownloadWindowsInstallerInfo.Name = "labeDownloadWindowsInstallerInfo";
            this.labeDownloadWindowsInstallerInfo.Size = new System.Drawing.Size(508, 46);
            this.labeDownloadWindowsInstallerInfo.TabIndex = 16;
            this.labeDownloadWindowsInstallerInfo.Text = "Microsoft SQL Server 2008 requires Windows Installer 4.5, but it is not installed" +
                " on your computer.\r\n\r\nClick Download to download Windows Installer 4.5.";
            // 
            // WindowsInstallerDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bytesWindowsInstaller);
            this.Controls.Add(this.downloadWindowsInstaller);
            this.Controls.Add(this.progressWindowsInstaller);
            this.Controls.Add(this.labeDownloadWindowsInstallerInfo);
            this.Description = "ShipWorks needs to download Windows Installer 4.5.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "WindowsInstallerDownload";
            this.Size = new System.Drawing.Size(535, 148);
            this.Title = "Install Windows Installer 4.5";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnShown);
            this.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancelWindows);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label bytesWindowsInstaller;
        private System.Windows.Forms.Button downloadWindowsInstaller;
        private System.Windows.Forms.ProgressBar progressWindowsInstaller;
        private System.Windows.Forms.Label labeDownloadWindowsInstallerInfo;
    }
}
