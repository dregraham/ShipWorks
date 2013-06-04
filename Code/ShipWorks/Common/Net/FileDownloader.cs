using System;
using System.IO;
using System.Net;
using System.Threading;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;

namespace ShipWorks.Common.Net
{
    /// <summary>
    /// Utility class for download files from the internet.
    /// </summary>
    public class FileDownloader
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FileDownloader));

        // Keeps track of any thread that may currently be executing
        Thread currentThread;

        // Provides locking so that a Cancel cannot occur at bad times.
        object cancelLock = new Object();

        // Whether the current action has been cancelled
        bool cancelled;

        // The web request we are using to download
        WebRequest webRequest;

        // The target stream to which we will write the results of the request
        Stream targetStream;

        /// <summary>
        /// Event used for progress notifications
        /// </summary>
        public event EventHandler<FileDownloadEventArgs> DownloadProgress;

        /// <summary>
        /// Constructor.  Takes an already initialized WebRequest object and an initialzed stream object
        /// </summary>
        public FileDownloader(WebRequest webRequest, Stream targetStream)
        {
            if (webRequest == null)
            {
                throw new ArgumentNullException("webRequest");
            }

            if (targetStream == null)
            {
                throw new ArgumentNullException("targetStream");
            }

            this.webRequest = webRequest;
            this.targetStream = targetStream;
        }

        /// <summary>
        /// The webrequest that is being used to perform the download.
        /// </summary>
        public WebRequest WebRequest
        {
            get
            {
                return webRequest;
            }
        }

        /// <summary>
        /// The stream the download is being written to.
        /// </summary>
        public Stream TargetStream
        {
            get
            {
                return targetStream;
            }
        }

        /// <summary>
        /// Begin the download of the specified web request
        /// </summary>
        public void BeginDownload()
        {
            // Cannot already be downloading
            if (IsDownloading)
            {
                throw new InvalidOperationException("Download already in progress.");
            }

            log.InfoFormat("Begin download '{0}'", webRequest.RequestUri);

            cancelled = false;

            // Start looking for the data
            currentThread = new Thread(ExceptionMonitor.WrapThread(DownloadThread));
            currentThread.IsBackground = true;
            currentThread.Start();
        }

        /// <summary>
        /// The entry point for the download thread
        /// </summary>
        private void DownloadThread()
        {
            try
            {
                // Get the web response
                WebResponse response = webRequest.GetResponse();

                // Raise the initial progress with the content length
                OnDownloadProgress(0, response.ContentLength);

                // Copy the input stream to the output stream in 1K chunks
                byte[] byteBuffer = new byte[10240];
                int numBytes = 0;

                // Current number of bytes written so far
                int currentBytes = 0;

                // Get the response stream
                using (Stream respStream = response.GetResponseStream())
                {
                    // Keep writing the the target stream until the response stream is done
                    while ((numBytes = respStream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                    {
                        // Check for cancelled
                        if (IsCanceled)
                        {
                            log.Info("Download cancelled");

                            OnDownloadProgress(currentBytes, response.ContentLength, FileDownloadStatus.Canceled);
                            return;
                        }

                        // Write the data to the stream
                        targetStream.Write(byteBuffer, 0, numBytes);

                        // Update the current bytes
                        currentBytes += numBytes;

                        // Update the progress
                        OnDownloadProgress(currentBytes, response.ContentLength);
                    }
                }

                log.Info("Download complete.");

                // Raise the final Completed event
                OnDownloadProgress(currentBytes, response.ContentLength, FileDownloadStatus.Complete);
            }
            catch (Exception ex)
            {
                log.Error("Download error.", ex);

                OnDownloadProgress(ex.Message);
            }
            finally
            {
                currentThread = null;
            }
        }

        /// <summary>
        /// Raise the progress event
        /// </summary>
        public void OnDownloadProgress(string error)
        {
            if (DownloadProgress != null)
            {
                DownloadProgress(this, new FileDownloadEventArgs(FileDownloadStatus.Error, 0, 0, error));
            }
        }

        /// <summary>
        /// Raise the progress event
        /// </summary>
        public void OnDownloadProgress(long current, long total)
        {
            if (DownloadProgress != null)
            {
                DownloadProgress(this, new FileDownloadEventArgs(FileDownloadStatus.Downloading, current, total, null));
            }
        }

        /// <summary>
        /// Raise the progress event
        /// </summary>
        public void OnDownloadProgress(long current, long total, FileDownloadStatus status)
        {
            if (DownloadProgress != null)
            {
                DownloadProgress(this, new FileDownloadEventArgs(status, current, total, null));
            }
        }

        /// <summary>
        /// Cancel the current download, if any
        /// </summary>
        public void Cancel(bool wait)
        {
            if (IsDownloading)
            {
                lock (cancelLock)
                {
                    cancelled = true;
                }

                if (wait && currentThread != null)
                {
                    currentThread.Join();
                }
            }
        }

        /// <summary>
        /// Determines if we are currently downloading
        /// </summary>
        public bool IsDownloading
        {
            get
            {
                return currentThread != null && currentThread.IsAlive;
            }
        }

        /// <summary>
        /// Thread-safe access to the cancelled member
        /// </summary>
        public bool IsCanceled
        {
            get
            {
                lock (cancelLock)
                {
                    return cancelled;
                }
            }
        }
    }
}
