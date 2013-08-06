namespace ShipWorks.Data.Administration.SqlServerSetup
{
    partial class DotNet35DownloadPage
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
            this.bytes = new System.Windows.Forms.Label();
            this.download = new System.Windows.Forms.Button();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.labeDownloadInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bytes
            // 
            this.bytes.Location = new System.Drawing.Point(372, 75);
            this.bytes.Name = "bytes";
            this.bytes.Size = new System.Drawing.Size(145, 23);
            this.bytes.TabIndex = 23;
            this.bytes.Text = "(0 of 2908 KB)";
            this.bytes.Visible = false;
            // 
            // download
            // 
            this.download.Location = new System.Drawing.Point(44, 71);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(75, 23);
            this.download.TabIndex = 22;
            this.download.Text = "Download";
            this.download.Click += new System.EventHandler(this.OnDownload);
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(124, 71);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(246, 23);
            this.progress.TabIndex = 21;
            // 
            // labeDownloadInfo
            // 
            this.labeDownloadInfo.Location = new System.Drawing.Point(23, 6);
            this.labeDownloadInfo.Name = "labeDownloadInfo";
            this.labeDownloadInfo.Size = new System.Drawing.Size(508, 59);
            this.labeDownloadInfo.TabIndex = 20;
            this.labeDownloadInfo.Text = "Microsoft SQL Server requires some additional .NET Framework components, but" +
                " they are not installed on your computer.\r\n\r\nClick Download to download the requ" +
                "ired components.";
            // 
            // DotNet35DownloadPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.bytes);
            this.Controls.Add(this.download);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.labeDownloadInfo);
            this.Description = "ShipWorks needs to download .NET Framework SQL Components";
            this.Name = "DotNet35DownloadPage";
            this.Size = new System.Drawing.Size(536, 138);
            this.Title = "Install .NET Framework SQL Components";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnShown);
            this.Cancelling += new System.ComponentModel.CancelEventHandler(this.OnCancelWindows);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label bytes;
        private System.Windows.Forms.Button download;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label labeDownloadInfo;
    }
}
