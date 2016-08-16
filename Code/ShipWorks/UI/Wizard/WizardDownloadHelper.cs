using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.UI.Wizard;
using System.Windows.Forms;
using System.IO;
using Interapptive.Shared.Net;
using System.Net;
using log4net;
using Interapptive.Shared;
using System.ComponentModel;
using ShipWorks.UI;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Common.Net;

namespace ShipWorks.UI.Wizard
{
    /// <summary>
    /// Utility class to factor out common Wizard download code
    /// </summary>
    class WizardDownloadHelper
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(WizardDownloadHelper));

        WizardForm wizard;

        string localPath;
        Uri remoteUri;

        FileDownloader fileDownloader;

        /// <summary>
        /// Constructor
        /// </summary>
        public WizardDownloadHelper(WizardForm wizard, string localPath, Uri remoteUri)
        {
            if (wizard == null)
            {
                throw new ArgumentNullException("wizard");
            }

            if (string.IsNullOrEmpty(localPath))
            {
                throw new ArgumentNullException("localPath");
            }

            if (remoteUri == null)
            {
                throw new ArgumentNullException("remoteUri");
            }

            this.wizard = wizard;
            this.localPath = localPath;
            this.remoteUri = remoteUri;
        }

        /// <summary>
        /// Start the process of downloading
        /// </summary>
        [NDependIgnoreLongMethod]
        public void Download(Button downloadButton, ProgressBar progress, Label bytes)
        {
            if (downloadButton == null)
            {
                throw new ArgumentNullException("downloadButton");
            }

            if (progress == null)
            {
                throw new ArgumentNullException("progress");
            }

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            log.InfoFormat("Initiating download for '{0}' => '{1}'", remoteUri, localPath);

            try
            {
                // The target stream for the downloader to write to
                FileStream fileTargetStream = new FileStream(localPath, FileMode.Create, FileAccess.Write);

                // Create the file downloader
                fileDownloader = new FileDownloader(
                    WebRequest.Create(remoteUri),
                    fileTargetStream);
            }
            catch (IOException ex)
            {
                log.Error(string.Format("Could not initialize download ({0})", Path.GetFileName(localPath)), ex);

                MessageHelper.ShowError(wizard, "ShipWorks encountered an error trying to download the file.\n\nDetails: " + ex.Message);
                return;
            }

            EventHandler<FileDownloadEventArgs> progressHandler =
                delegate(object sender, FileDownloadEventArgs e)
                {
                    FileStream fileTargetStream = (FileStream) fileDownloader.TargetStream;

                    // We are downloading
                    if (e.Status == FileDownloadStatus.Downloading)
                    {
                        bytes.Visible = true;
                        bytes.Text = string.Format("({0} of {1})", StringUtility.FormatByteCount(e.CurrentBytes), StringUtility.FormatByteCount(e.TotalBytes));

                        progress.Minimum = 0;
                        progress.Maximum = 100;
                        progress.Value = (int) Math.Min(Math.Max(100.0 * e.CurrentBytes / e.TotalBytes, 0), 100);
                    }

                    // If we are not downloading, then its time to cleanup
                    else
                    {
                        // Try to cleanup a little
                        fileTargetStream.Close();
                        fileTargetStream = null;
                        fileDownloader = null;

                        // If it was success, move on to the next page
                        if (e.Status == FileDownloadStatus.Complete)
                        {
                            wizard.MoveNext();
                        }

                        // If it was an error show the error message and reset the gui
                        if (e.Status == FileDownloadStatus.Error)
                        {
                            MessageHelper.ShowError(wizard, "An error occurred while downloading:\n\n" + e.ErrorMessage);
                        }

                        // If it was error or cancelled, delete any partial part of the file we may have downloaded
                        if (e.Status == FileDownloadStatus.Error || e.Status == FileDownloadStatus.Canceled)
                        {
                            try
                            {
                                if (File.Exists(localPath))
                                {
                                    File.Delete(localPath);
                                }
                            }
                            catch (IOException ex)
                            {
                                // If we can't delete it thats ok - we'll detect its invalid and delete it next time we start to download.
                                log.Error("Unable to delete file after failed download.", ex);
                            }

                            // Re-enable the gui
                            downloadButton.Enabled = true;
                            wizard.BackEnabled = true;

                            // Reset the progress stuff
                            bytes.Visible = false;
                            progress.Value = 0;
                        }
                    }
                };

            // Hookup the progress event
            fileDownloader.DownloadProgress +=
                delegate(object sender, FileDownloadEventArgs e)
                {
                    if (wizard.InvokeRequired)
                    {
                        wizard.BeginInvoke(progressHandler, new object[] { sender, e });
                    }
                    else
                    {
                        progressHandler(sender, e);
                    }
                };

            // Disable the download button and the browsing buttons
            downloadButton.Enabled = false;
            wizard.BackEnabled = false;

            wizard.Refresh();

            // Begin the download
            fileDownloader.BeginDownload();
        }

        /// <summary>
        /// User is trying to cancel downloading
        /// </summary>
        public void OnCancelDownload(object sender, CancelEventArgs e)
        {
            if (fileDownloader != null && fileDownloader.IsDownloading)
            {
                // Prevent the window from being closed. The cancel\close operation
                // will just server to cancel the download
                e.Cancel = true;
                fileDownloader.Cancel(true);
            }
        }
    }
}
