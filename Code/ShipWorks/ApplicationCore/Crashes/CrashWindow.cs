using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// Window that is displayed when the application crashes.
    /// </summary>
    public partial class CrashWindow : Form
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(CrashWindow));

        Exception exception;
        bool guiThread;

        Task taskLogSizeCalculator;
        string logFileName;

        static volatile bool isApplicationCrashed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public CrashWindow(Exception ex, bool guiThread, string userEmail)
        {
            if (isApplicationCrashed)
            {
                string message = "CrashWindow has already been displayed.  Does not make sense to display twice.";
                log.Error(message);

                throw new InvalidOperationException(message);
            }

            InitializeComponent();

            isApplicationCrashed = true;

            this.exception = ex;
            this.guiThread = guiThread;

            ShowInTaskbar = !guiThread || (Application.OpenForms.Count == 1 && Application.OpenForms[0] is SplashScreen);
            TopMost = true;

            email.Text = userEmail;

            ActiveControl = userComments;

            // Dump the report to the log
            try
            {
                File.WriteAllText(
                    Path.Combine(LogSession.LogFolder, "crash.txt"),
                    CrashSubmitter.GetContent(exception, ""));
            }
            catch
            {
                // Nothing to do - we already crashed.
            }
        }

        /// <summary>
        /// Indicates if the Application has crashed.
        /// </summary>
        public static bool IsApplicationCrashed
        {
            get { return isApplicationCrashed; }
        }

        /// <summary>
        /// The window has been shown
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Activate();

            // The Splash may be going right now.  Make sure to close it first.
            SplashScreen.CloseSplash();

            // If the crash wasn't on the GUI thread, we need to hide the main app if its there
            if (!guiThread)
            {
                List<Form> formsToClose = new List<Form>();

                // We need a seperate list b\c the act of closing the form removes it from the OpenForms collection,
                // and you cant enumerate over a collection that changes during the enumeration.
                foreach (Form form in Application.OpenForms)
                {
                    formsToClose.Add(form);
                }

                foreach (Form form in formsToClose)
                {
                    if (form.InvokeRequired)
                    {
                        Form currentForm = form; // Access to foreach variables in closure may have different ehavior when compiled with different version of the compiler. Copy to Local Variable.
                        form.Invoke(new Action(() => currentForm.Visible = false));
                    }
                    else
                    {
                        form.Visible = false;    
                    }
                }
            }

            // Start figuring out how big the log submission would be
            taskLogSizeCalculator = Task.Factory.StartNew(() =>
                {
                    logFileName = CrashSubmitter.CreateCrashLogZip();

                    return new FileInfo(logFileName).Length;
                })
                .ContinueWith(
                    ant =>
                    {
                        if (TopLevelControl != null && TopLevelControl.Visible)
                        {
                            try
                            {
                                TopLevelControl.BeginInvoke((Action) delegate() { logSize.Text = string.Format("(about {0})", StringUtility.FormatByteCount(ant.Result)); });
                            }
                            catch
                            {
                                // Swallow - means we already got closed, no big deal
                            }
                        }
                    });
        }

        /// <summary>
        /// Add more description lines
        /// </summary>
        private void OnAddDescriptionLines(object sender, EventArgs e)
        {
            linkAddDescriptionLines.Visible = false;

            userComments.Multiline = true;
            userComments.Height *= 4;
            userComments.ScrollBars = ScrollBars.Both;

            int distanceMove = userComments.Bottom - panelBottom.Top;
            panelBottom.Top += distanceMove;
            Height += distanceMove;
        }

        /// <summary>
        /// Show the details of the crash report.
        /// </summary>
        private void OnViewReport(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (CrashReportDetails dlg = new CrashReportDetails(CrashSubmitter.GetContent(exception, userComments.Text)))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Send the report to interapptive
        /// </summary>
        private void OnSendReport(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string logFileToSubmit = null;

                if (includeLogFiles.Checked)
                {
                    taskLogSizeCalculator.Wait();
                    logFileToSubmit = logFileName;
                }

                CrashResponse response = CrashSubmitter.Submit(exception, email.Text, userComments.Text, logFileToSubmit);
                response.ShowDialog(this);
            
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                bool fallback = true;

                #if DEBUG
                    fallback = false;
                #endif

                if (InterapptiveOnly.IsInterapptiveUser)
                {
                    fallback = false;
                }

                // Fallback and submit to our script
                if (fallback)
                {
                    SubmitFallbackReport(ex);
                }
                else
                {
                    MessageBox.Show(this,
                        "An error occurred submitting the crash report:\n\n" + ex.Message,
                        "ShipWorks",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Submit a fallback report to the interapptive php script due to the given exception submitting to FogBugz
        /// </summary>
        private void SubmitFallbackReport(Exception fallbackEx)
        {
            HttpVariableRequestSubmitter post = new HttpVariableRequestSubmitter();
            post.Uri = new Uri("http://www.interapptive.com/shipworks/reports/bugzScoutFallback.php");
            post.Credentials = new NetworkCredential("shipworks", "report");

            post.Variables.Add(new HttpVariable("description", "Crash: " + CrashSubmitter.GetIdentifier(exception)));
            post.Variables.Add(new HttpVariable("extraInformation", CrashSubmitter.GetContent(exception, userComments.Text)));
            post.Variables.Add(new HttpVariable("userEmail", email.Text));
            post.Variables.Add(new HttpVariable("fallbackReason", fallbackEx.GetType().Name + ": " + fallbackEx.Message));

            try
            {
                using (IHttpResponseReader response = post.GetResponse())
                {
                    if (response.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                    {
                        throw new WebException("Could not connect to server.");
                    }
                }

                // Display the response
                MessageBox.Show(this,
                    "Thank you for helping us to improve ShipWorks.",
                    "ShipWorks",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    "Unable to send error report. \n\n" + 
                    fallbackEx.Message + "\n" +
                    ex.Message,
                    "ShipWorks",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Close the crash report window
        /// </summary>
        private void OnClose(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
        }
    }
}
