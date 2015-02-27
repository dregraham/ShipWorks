using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using System.Threading;
using System.Net;
using System.IO;
using Interapptive.Shared.Net;
using System.Diagnostics;

namespace ShipWorks.ApplicationCore.Help
{
    /// <summary>
    /// Window for connecting to interapptive remote assistance
    /// </summary>
    public partial class RemoteAssistanceDlg : Form
    {
        // Url to try to activate the code
        static string activateUrl = "https://secure.logmeinrescue.com/Customer/Code.aspx";

        // Url to download the client
        static string downloadUrl = "https://secure.logmeinrescue.com/Customer/Entry.aspx";

        /// <summary>
        /// Constructor
        /// </summary>
        public RemoteAssistanceDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Connect to interapptive sharing
        /// </summary>
        private void OnConnect(object sender, EventArgs e)
        {
            if (pinBox.Text.Trim().Length != 6)
            {
                MessageHelper.ShowInformation(this, "Please enter the 6-digit PIN code provided by Interapptive.");
                return;
            }

            ProgressProvider progressProvider = new ProgressProvider();

            // Create the progress window
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Remote Assistance";
            progressDlg.Description = "Connecting to Interapptive support representative.";
            progressDlg.AutoCloseWhenComplete = true;
            progressDlg.Show(this);

            Dictionary<string, object> userState = new Dictionary<string, object>();
            userState["progressDlg"] = progressDlg;
            userState["pin"] = pinBox.Text;

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
            string pin = (string) state["pin"];

            ProgressProvider progressProvider = progressDlg.ProgressProvider;

            // Create the progress items
            ProgressItem progressConnect = progressProvider.ProgressItems.Add("Connect");
            ProgressItem progressLaunch = progressProvider.ProgressItems.Add("Initiate");

            // Start connection phase
            progressConnect.Detail = "Connecting to support...";
            progressConnect.Starting();

            Exception error = null;
            string clientExe = null;

            try
            {
                // Activate the PIN and get the resulting cookies
                CookieContainer cookies = ActivatePIN(pin);

                // Download the client
                clientExe = DownloadClient(pin, cookies, progressConnect);

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
                else
                {
                    throw;
                }
            }

            // Post the results back to the UI
            BeginInvoke(new MethodInvoker<ProgressDlg, Exception>(OnAsyncConnectComplete), progressDlg, error);
        }

        /// <summary>
        /// Luanch the support client
        /// </summary>
        private void LaunchClient(string clientExe)
        {
            FileInfo fileInfo = new FileInfo(clientExe);

            if (fileInfo.Length < 100000)
            {
                throw new InvalidOperationException("ShipWorks was unable to initiate the support session.\n\nPlease check that you entered the correct 6-digit PIN code.");
            }
            else
            {
                Stopwatch timer = Stopwatch.StartNew();

                // Launch the launcher process
                Process.Start(clientExe);

                // Try to wait for the assistance app to open
                while (true)
                {
                    foreach (Process process in Process.GetProcesses().Where(p => p.ProcessName.ToLower().Contains("rescue")))
                    {
                        // Once we find a rescue process that was started in the last 30 seconds with a valid main window title (window is open),
                        // then we can think that its ready.
                        if (!string.IsNullOrEmpty(process.MainWindowTitle) && process.StartTime > DateTime.Now.AddSeconds(-30))
                        {
                            return;
                        }
                    }

                    // Give up waiting for the assistance app to open
                    if (timer.Elapsed > TimeSpan.FromSeconds(30))
                    {
                        return;
                    }

                    Thread.Sleep(500);
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

                MessageHelper.ShowError(this, error.Message);
                progressDlg.Close();
            }
            else if (!progressDlg.ProgressProvider.CancelRequested)
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Activate the given PIN and return the cookies that were responded with
        /// </summary>
        private CookieContainer ActivatePIN(string pin)
        {
            CookieContainer cookies = new CookieContainer();

            // grab the cookies from the response            
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(activateUrl);
            request.CookieContainer = cookies;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            string postData = String.Format("Code={0}", pin);
            request.ContentLength = postData.Length;

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                writer.Write(postData);
            }

            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            response.Close();

            return cookies;
        }

        /// <summary>
        /// Download the remote assistance client
        /// </summary>
        private string DownloadClient(string pin, CookieContainer cookies, ProgressItem progress)
        {
            string clientExe = Path.Combine(DataPath.CreateUniqueTempPath(), "launcher.exe");

            // Create the webrequest for downloading
            HttpWebRequest downloadRequest = (HttpWebRequest) WebRequest.Create(downloadUrl);
            downloadRequest.Method = "POST";
            downloadRequest.ContentType = "application/x-www-form-urlencoded";
            downloadRequest.CookieContainer = cookies;

            string postData = string.Format("PrivateCode={0}&CompanyID=38200", pin);
            downloadRequest.ContentLength = postData.Length;
            downloadRequest.AllowWriteStreamBuffering = false;

            using (StreamWriter writer = new StreamWriter(downloadRequest.GetRequestStream(), System.Text.Encoding.ASCII))
            {
                writer.Write(postData);
            }

            // Get the web response
            using (FileStream targetStream = File.OpenWrite(clientExe))
            {
                using (WebResponse response = downloadRequest.GetResponse())
                {
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
