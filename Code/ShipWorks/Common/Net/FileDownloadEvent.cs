using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Common.Net
{
    /// <summary>
    /// EventArgs for the FileDownload event
    /// </summary>
    public class FileDownloadEventArgs : EventArgs
    {
        // Current bytes downloaded
        long currentBytes = 0;

        // Total bytes to download
        long totalBytes = 0;

        // If an error occurred, this will be non-null
        string errorMessage = null;

        // Indicates the current status of the download
        FileDownloadStatus status;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileDownloadEventArgs(FileDownloadStatus status, long currentBytes, long totalBytes, string error)
        {
            this.currentBytes = currentBytes;
            this.totalBytes = totalBytes;
            this.errorMessage = error;
            this.status = status;
        }

        /// <summary>
        /// Gets the status of the download
        /// </summary>
        public FileDownloadStatus Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// The current number of bytes that have already been downloaded.
        /// </summary>
        public long CurrentBytes
        {
            get
            {
                return currentBytes;
            }
        }

        /// <summary>
        /// The total number of bytes to be downloaded.
        /// </summary>
        public long TotalBytes
        {
            get
            {
                return totalBytes;
            }
        }

        /// <summary>
        /// If this is an error progress, then this returns the error message
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
        }
    }
}
