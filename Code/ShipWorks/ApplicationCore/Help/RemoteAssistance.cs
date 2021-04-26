using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Threading;
using ShipWorks.Common.Threading;


namespace ShipWorks.ApplicationCore.Help
{
    public class RemoteAssistance
    {
        // Url to download the client
        private static Uri downloadUrl = new Uri("https://d17kmd0va0f0mp.cloudfront.net/sos/SplashtopSOS.exe");

        /// <summary>
        /// Create progress dialog
        /// </summary>
        public void InitiateRemoteAssistance(IWin32Window parent)
        {
            ProgressProvider progressProvider = new ProgressProvider();

            // Create the progress window
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Remote Assistance";
            progressDlg.Description = "Connecting to ShipWorks support representative.";
            progressDlg.AutoCloseWhenComplete = true;
            progressDlg.Show(parent);

            Dictionary<string, object> userState = new Dictionary<string, object>();
            userState["progressDlg"] = progressDlg;

            // Kick off the activation
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(AsyncConnect), userState);
        }

        /// <summary>
        /// Connect to interapptive
        /// </summary>
        private void AsyncConnect(object userState)
        {
            var state = (Dictionary<string, object>) userState;
            ProgressDlg progressDlg = (ProgressDlg) state["progressDlg"];

            IProgressProvider progressProvider = progressDlg.ProgressProvider;

            // Create the progress items
            IProgressReporter progressConnect = progressProvider.AddItem("Connect");
            IProgressReporter progressLaunch = progressProvider.AddItem("Initiate");

            // Start connection phase
            progressConnect.Detail = "Connecting to support...";
            progressConnect.Starting();

            Exception error = null;
            string clientExe = null;

            try
            {
                // Download the client
                clientExe = DownloadClient(progressConnect);

                // Done with connection phase
                progressConnect.Completed();

                // Launch the support session if the download completed
                if (clientExe != null && !progressProvider.CancelRequested)
                {
                    progressLaunch.CanCancel = false;
                    progressLaunch.Starting();
                    progressLaunch.Detail = "Starting support session...";

                    LaunchClient(clientExe);

                    progressLaunch.Completed();
                }
            }
            catch (InvalidOperationException ex)
            {
                error = ex;
            }
            catch (UnauthorizedAccessException ex)
            {
                error = ex;
            }
            catch (Exception ex)
            {
                if (WebHelper.IsWebException(ex))
                {
                    error = ex;
                }
                // Check by message because a Win32Exception could be caused by multiple things
                else if (ex.Message.Contains("canceled"))
                {
                    error = ex;
                }
                else
                {
                    throw;
                }
            }

            // Close the progress dialog on the correct thread
            progressDlg.BeginInvoke(new MethodInvoker<ProgressDlg, Exception>(OnAsyncConnectComplete), progressDlg, error);
        }

        /// <summary>
        /// Launches the client.
        /// </summary>
        /// <param name="clientExe">The client executable.</param>
        /// <exception cref="System.InvalidOperationException">ShipWorks was unable to initiate the support session.</exception>
        private void LaunchClient(string clientExe)
        {
            FileInfo fileInfo = new FileInfo(clientExe);

            if (fileInfo.Length < 100000)
            {
                throw new InvalidOperationException("ShipWorks was unable to initiate the support session.");
            }
            else
            {
                // Launch the launcher process
                using (Process client = new Process())
                {
                    client.StartInfo.FileName = clientExe;
                    client.StartInfo.Verb = "runas";
                    if (!client.Start())
                    {
                        throw new InvalidOperationException("ShipWorks was unable to initiate the support session.");
                    }
                }
            }
        }

        /// <summary>
        /// The connection is complete
        /// </summary>
        private void OnAsyncConnectComplete(ProgressDlg progressDlg, Exception error)
        {
            if (error != null)
            {
                progressDlg.ProgressProvider.Terminate(error);
            }
            else
            {
                progressDlg.Close();
            }
        }

        /// <summary>
        /// Download the remote assistance client
        /// </summary>
        private string DownloadClient(IProgressReporter progress)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string clientExe = Path.Combine(appData, "Interapptive\\ShipWorks\\RemoteAssistanceLauncher\\launcher.exe");

            // Create the directory
            Directory.CreateDirectory(Path.GetDirectoryName(clientExe));

            // Create the webrequest for downloading
            HttpWebRequest downloadRequest = (HttpWebRequest) WebRequest.Create(downloadUrl);
            downloadRequest.AllowWriteStreamBuffering = false;

            // Get the web response
            using (FileStream targetStream = File.OpenWrite(clientExe))
            {
                using (WebResponse response = downloadRequest.GetResponse())
                {
                    // Erase the existing file
                    targetStream.SetLength(0);

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
                            if (progress.IsCancelRequested)
                            {
                                return null;
                            }

                            // Write the data to the stream
                            targetStream.Write(byteBuffer, 0, numBytes);

                            // Update the current bytes
                            currentBytes += numBytes;

                            // Update the progress
                            progress.PercentComplete = (100 * currentBytes) / (int) response.ContentLength;
                        }
                    }
                }
            }
            progress.Detail = "Done";
            return clientExe;
        }
    }
}
