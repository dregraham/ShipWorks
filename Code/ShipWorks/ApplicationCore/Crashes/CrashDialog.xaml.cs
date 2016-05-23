using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Interapptive.Shared.Net;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using Form = System.Windows.Forms.Form;
using FormsApplication = System.Windows.Forms.Application;

namespace ShipWorks.ApplicationCore.Crashes
{
    /// <summary>
    /// Interaction logic for CrashDialog.xaml
    /// </summary>
    public partial class CrashDialog : Window, System.Windows.Forms.IWin32Window
    {
        static readonly ILog log = LogManager.GetLogger(typeof(CrashDialog));
        RegistryHelper registry = new RegistryHelper(@"Software\Interapptive\ShipWorks\Options");

        Exception exception;
        bool guiThread;
        string email;
        readonly bool showSupportMessage;
        private const int showSupportMessageCutoff = 4;
        private const string reopenRegistryKey = "ReopenAfterCrash";

        /// <summary>
        /// Constructor
        /// </summary>
        public CrashDialog(Exception ex, bool guiThread, string userEmail, int recoveryCount)
        {
            if (IsApplicationCrashed)
            {
                string message = "CrashWindow has already been displayed.  Does not make sense to display twice.";
                log.Error(message);

                throw new InvalidOperationException(message);
            }

            CreateLogTask = TaskEx.Run(() => SendReport());

            InitializeComponent();

            IsApplicationCrashed = true;

            this.exception = ex;
            this.guiThread = guiThread;

            ShowInTaskbar = !guiThread || (FormsApplication.OpenForms.Count == 1 && FormsApplication.OpenForms[0] is SplashScreen);

            email = userEmail;
            showSupportMessage = recoveryCount > showSupportMessageCutoff;

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

            Loaded += OnLoaded;
        }

        /// <summary>
        /// Create log task
        /// </summary>
        public Task CreateLogTask { get; }

        /// <summary>
        /// Indicates if the Application has crashed.
        /// </summary>
        public static bool IsApplicationCrashed { get; private set; }

        /// <summary>
        /// Get a handle to the current window
        /// </summary>
        public IntPtr Handle => new WindowInteropHelper(this).Handle;

        /// <summary>
        /// The window was loaded
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // The Splash may be going right now.  Make sure to close it first.
            SplashScreen.CloseSplash();

            ContactMessage.Visibility = showSupportMessage ? Visibility.Visible : Visibility.Collapsed;
            ContactMessage.IsEnabled = showSupportMessage;

            bool reopen = true;
            bool.TryParse(registry.GetValue(reopenRegistryKey, bool.TrueString), out reopen);
            ReopenShipWorks.IsChecked = reopen;

            // If the crash wasn't on the GUI thread, we need to hide the main app if its there
            if (!guiThread)
            {
                CloseForms();
            }
        }

        /// <summary>
        /// Close all open forms
        /// </summary>
        private static void CloseForms()
        {
            // We need a seperate list b\c the act of closing the form removes it from the OpenForms collection,
            // and you cant enumerate over a collection that changes during the enumeration.
            List<Form> formsToClose = FormsApplication.OpenForms.OfType<Form>().ToList();

            foreach (Form form in formsToClose)
            {
                if (form.InvokeRequired)
                {
                    // Access to foreach variables in closure may have different behavior when compiled with
                    // different version of the compiler. Copy to Local Variable.
                    Form currentForm = form;
                    form.Invoke(new Action(() => currentForm.Visible = false));
                }
                else
                {
                    form.Visible = false;
                }
            }
        }

        /// <summary>
        /// Show the details of the crash report.
        /// </summary>
        private void OnMoreInformationLinkClick(object sender, RoutedEventArgs e)
        {
            CrashDetailsDialog details = new CrashDetailsDialog(CrashSubmitter.GetContent(exception, string.Empty));
            details.Owner = this;
            details.ShowDialog();
        }

        /// <summary>
        /// Send the report to interapptive
        /// </summary>
        private void SendReport()
        {
            try
            {
                string logFileToSubmit = CrashSubmitter.CreateCrashLogZip();
                CrashSubmitter.Submit(exception, email, string.Empty, logFileToSubmit);
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
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
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
            post.Variables.Add(new HttpVariable("userEmail", email));
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
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,
                    "Unable to send error report. \n\n" +
                    fallbackEx.Message + "\n" +
                    ex.Message,
                    "ShipWorks",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Close the crash report window
        /// </summary>
        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            IsEnabled = false;

            DialogResult = ReopenShipWorks.IsChecked;
            registry.SetValue(reopenRegistryKey, ReopenShipWorks.IsChecked);

            TaskEx.WhenAny(CreateLogTask, TaskEx.Delay(TimeSpan.FromMinutes(10)))
                .ContinueWith(ExitApplication);

            CloseForms();
            Close();
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        private void ExitApplication(Task<Task> task)
        {
            try
            {
                // Application.Exit does not guaranteed that the windows close.  It only tries.  If an exception
                // gets thrown, or they set e.Cancel = true, they won't have closed.
                FormsApplication.Exit();

                // This forces windows to close.  If they try to save state or do other stupid things
                // while closing then they will throw an exception.
                FormsApplication.Exit();
            }
            catch (Exception termEx)
            {
                log.Error("Termination error", termEx);
            }
        }

        /// <summary>
        /// Support link was clicked
        /// </summary>
        private void OnSupportClick(object sender, RoutedEventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/support/tickets/new", this);
        }
    }
}
