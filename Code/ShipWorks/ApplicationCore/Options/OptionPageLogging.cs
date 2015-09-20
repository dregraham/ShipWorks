using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using System.IO;
using System.Linq;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.Utility;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.UI;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.ApplicationCore.Options
{
    /// <summary>
    /// Logging page of the Options window
    /// </summary>
    public partial class OptionPageLogging : OptionPageBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPageLogging()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Do initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            StartSizeCalculation();

            LogOptions options = LogSession.Options;
            logShipWorks.Checked = options.LogShipWorks;
            logApiCalls.Checked = options.LogServices;
            logRateCalls.Checked = options.LogRateCalls;

            SetMaxAgeSeletion(options.MaxLogAgeDays);

            linkLabelExtendedLogging.Visible = InterapptiveOnly.MagicKeysDown;

            panelContentOptions.Height = panelContentOptions.Controls.Cast<Control>().Where(c => c.Visible).Max(c => c.Bottom);
            panelOtherOptions.Top = panelContentOptions.Bottom;
            panelLogContent.Height = panelLogContent.Controls.Cast<Control>().Where(c => c.Visible).Max(c => c.Bottom);

            panelLogContent.Visible = !UserSession.IsLoggedOn || UserSession.Security.HasPermission(PermissionType.DatabaseSetup);
        }

        /// <summary>
        /// Apply the correct selection for the max age combo
        /// </summary>
        private void SetMaxAgeSeletion(int maxLogAgeDays)
        {
            if (maxLogAgeDays == 0)
            {
                logAge.SelectedIndex = 2;
            }
            else if (maxLogAgeDays == 30)
            {
                logAge.SelectedIndex = 1;
            }
            else
            {
                logAge.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Get the time-to-live for logs based on the combo selection
        /// </summary>
        private int GetMaxAgeSelection()
        {
            switch (logAge.SelectedIndex)
            {
                case 0:
                    return 7;
                case 1:
                    return 30;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Save the options
        /// </summary>
        public override void Save()
        {
            LogOptions options = new LogOptions();
            options.LogShipWorks = logShipWorks.Checked;
            options.LogServices = logApiCalls.Checked;
            options.LogRateCalls = logRateCalls.Checked;
            options.MaxLogAgeDays = GetMaxAgeSelection();

            LogSession.Configure(options);
        }

        /// <summary>
        /// Enable the extended (private) logging option
        /// </summary>
        private void OnEnableExtendedLogging(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LogSession.IsPrivateLoggingEncrypted = false;
            linkLabelExtendedLogging.Visible = false;
        }

        #region Log file size background calculation

        /// <summary>
        /// Start calculating the total size of the log files
        /// </summary>
        private void StartSizeCalculation()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(OnCalculateFileSizeProgress);
            worker.DoWork += new DoWorkEventHandler(CalculateFileSize);
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Calculate the size of the log files
        /// </summary>
        void CalculateFileSize(object sender, DoWorkEventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(DataPath.LogRoot);

            long size = 0;
            CalculateSizeRecursive(ref size, di, (BackgroundWorker) sender);
        }

        /// <summary>
        /// Calculate the size of every file and folder recursively
        /// </summary>
        private void CalculateSizeRecursive(ref long size, DirectoryInfo di, BackgroundWorker worker)
        {
            List<DirectoryInfo> childDirectories = new List<DirectoryInfo>();

            // Calculate the size of all files, and find all child directories
            foreach (FileSystemInfo fsi in di.GetFileSystemInfos())
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                FileInfo fileInfo = fsi as FileInfo;
                if (fileInfo != null)
                {
                    size += fileInfo.Length;
                }

                DirectoryInfo directoryInfo = fsi as DirectoryInfo;
                if (directoryInfo != null)
                {
                    childDirectories.Add(directoryInfo);
                }
            }

            // Report on the file size we found
            worker.ReportProgress(0, size);

            // Now recurse into child directories
            foreach (DirectoryInfo childDi in childDirectories)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                CalculateSizeRecursive(ref size, childDi, worker);
            }
        }

        /// <summary>
        /// Progress being reported while calculating file size
        /// </summary>
        void OnCalculateFileSizeProgress(object sender, ProgressChangedEventArgs e)
        {
            // If we have closed, stop counting
            if (TopLevelControl == null || !TopLevelControl.Visible)
            {
                ((BackgroundWorker) sender).CancelAsync();
                return;
            }

            logSize.Text = StringUtility.FormatByteCount((long) e.UserState);
        }

        #endregion

        /// <summary>
        /// Open the current log folder
        /// </summary>
        private void OnOpenLogFolder(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl(LogSession.LogFolder, this);
        }

        /// <summary>
        /// Open the log folder for all logs
        /// </summary>
        private void OnOpenAllLogs(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl(DataPath.LogRoot, this);
        }

        /// <summary>
        /// Save all logs to a zip file
        /// </summary>
        private void OnSaveLogsToZip(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Zip Files (*.zip)|*.zip";

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ZipWriter writer = new ZipWriter();
                    writer.Items.AddRange(ZipWriterFolderHelper.CreateFileItems(DataPath.LogRoot, false));

                    Cursor.Current = Cursors.WaitCursor;

                    try
                    {
                        writer.Save(dlg.FileName);
                    }
                    catch (IOException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        MessageHelper.ShowError(this, ex.Message);
                    }
                }
            }
        }
    }
}
