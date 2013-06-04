using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Event handler for the DownloadComplete event
    /// </summary>
    public delegate void DownloadCompleteEventHandler(object sender, DownloadCompleteEventArgs e);

    /// <summary>
    /// EventArgs for the DownloadComplete event
    /// </summary>
    public class DownloadCompleteEventArgs : EventArgs
    {
        bool showDashboardError;
        bool isProgressCurrent;

        /// <summary>
        /// Constructor
        /// </summary>
        public DownloadCompleteEventArgs(bool showDashboardError, bool isProgressCurrent)
        {
            this.showDashboardError = showDashboardError;
            this.isProgressCurrent = isProgressCurrent;
        }

        /// <summary>
        /// Indicates if any errors occurred during download that should be shown as a dashboard message
        /// </summary>
        public bool ShowDashboardError
        {
            get { return showDashboardError; }
        }

        /// <summary>
        /// This is only valid of the progress display is currently visible. It indicates if the progress display is showing the progress
        /// items for the current download.  It's possible that the progress window is showing progress items from a previous download,
        /// and that an auto-download triggered this download, and thus it's progress is not the current one shown.
        /// </summary>
        public bool IsProgressCurrent
        {
            get { return isProgressCurrent; }
        }
    }
}
