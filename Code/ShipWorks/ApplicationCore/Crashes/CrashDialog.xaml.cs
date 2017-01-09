﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Win32;
using log4net;
using log4net.Appender;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
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
        readonly bool showSupportMessage;
        private const int showSupportMessageCutoff = 4;
        private const string reopenRegistryKey = "ReopenAfterCrash";
        readonly string crashContent;
        private readonly TaskCompletionSource<bool> waitForOkButton;

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

            exception = ex;
            this.guiThread = guiThread;
            string logName = Guid.NewGuid().ToString() + ".zip";

            // Dump the report to the log
            try
            {
                Telemetry.TrackException(exception, new Dictionary<string, string> {
                    { "Crash Log", logName },
                    { "ConnectionMonitorStatus", ConnectionMonitor.Status.ToString() },
                    { "TimeSinceReconnect", DateTime.Now.Subtract(ConnectionMonitor.LastReconnection).TotalSeconds.ToString() }
                });

                crashContent = CrashSubmitter.GetContent(exception, "");

                File.WriteAllText(
                    Path.Combine(LogSession.LogFolder, "crash.txt"),
                    crashContent);
            }
            catch
            {
                // Nothing to do - we already crashed.
            }

            waitForOkButton = new TaskCompletionSource<bool>();
            CreateLogTask = TaskEx.WhenAll(StartSubmissionTask(userEmail, logName), waitForOkButton.Task)
                .ContinueWith(ExitApplication);

            InitializeComponent();

            IsApplicationCrashed = true;

            ShowInTaskbar = !guiThread || (FormsApplication.OpenForms.Count == 1 && FormsApplication.OpenForms[0] is SplashScreen);

            showSupportMessage = recoveryCount > showSupportMessageCutoff;

            Loaded += OnLoaded;
        }

        /// <summary>
        /// Start the crash submission task
        /// </summary>
        private Task<Task> StartSubmissionTask(string userEmail, string logName)
        {
            return TaskEx.WhenAny(TaskEx.Run(() => SendReport(userEmail, logName)), TaskEx.Delay(TimeSpan.FromMinutes(10)));
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
            CrashDetailsDialog details = new CrashDetailsDialog(crashContent);
            details.Owner = this;
            details.ShowDialog();
        }

        /// <summary>
        /// Send the report to interapptive
        /// </summary>
        private void SendReport(string email, string logName)
        {
            try
            {
                StopLogging();

                string logFileToSubmit = CrashSubmitter.CreateCrashLogZip();
                CrashSubmitter.Submit(exception, email, logName, logFileToSubmit);
            }
            catch (Exception)
            {
                // We've already crashed, so just eat the exception if we fail to submit the crash report
            }
        }

        /// <summary>
        /// Stop the logging service so that we avoid trying to zip files that are changing
        /// </summary>
        private static void StopLogging()
        {
            log.Info($"Shutting down logger");
            foreach (IAppender appender in LogManager.GetAllRepositories().SelectMany(x => x.GetAppenders()))
            {
                appender.Close();
            }
            LogManager.Shutdown();
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

            waitForOkButton.SetResult(true);

            CloseForms();
            Close();
        }

        /// <summary>
        /// Exit the application
        /// </summary>
        private void ExitApplication(Task task)
        {
            log.Info("Exiting application...");

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

        /// <summary>
        /// Allow a window with no title bar to be moved
        /// </summary>
        private void OnWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
