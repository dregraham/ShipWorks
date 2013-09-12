using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using log4net;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.ExecutionMode.Initialization;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;
using ShipWorks.UI;
using ShipWorks.Users;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExecutionMode interface intended to be used when running ShipWorks with a UI.
    /// </summary>
    public class UserInterfaceExecutionMode : IExecutionMode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserInterfaceExecutionMode));
        private readonly IExecutionModeInitializer initializer;

        // Mutex used to indicate the application is alive. The installer uses this to know the app needs
        // shutdown before the installation can continue.
        Mutex appMutex;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode" /> class.
        /// </summary>
        public UserInterfaceExecutionMode()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode" /> class.
        /// </summary>
        /// <param name="initializer">The initializer.</param>
        public UserInterfaceExecutionMode(IExecutionModeInitializer initializer)
        {
            this.initializer = initializer ?? new UserInterfaceExecutionModeInitializer();
        }

        /// <summary>
        /// Indicates whether the UI is running for the current instance ID.
        /// </summary>
        public static bool IsProcessRunning
        {
            get { return Program.ExecutionMode is UserInterfaceExecutionMode || SingleInstance.IsAlreadyRunning; }
        }

        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserInteractive
        {
            get { return MainForm != null && MainForm.IsHandleCreated; }
        }

        /// <summary>
        /// Single instance of the running application
        /// </summary>
        public MainForm MainForm { get; private set; }

        /// <summary>
        /// Runs the ShipWorks UI.
        /// </summary>
        public void Execute()
        {
            // The installer uses this to know the app needs shutdown before the installation can continue.
            appMutex = new Mutex(false, "{AX70DA71-2A39-4f8c-8F97-7F5348493F57}");

            SingleInstance.Register();

            if (!InterapptiveOnly.MagicKeysDown && !InterapptiveOnly.AllowMultipleInstances)
            {
                // If the application is already running, open it now and exit.
                if (SingleInstance.ActivateRunningInstance())
                {
                    // User does not have the permissions to run multiple instances of ShipWorks
                    throw new MultipleExecutionModeInstancesException("An instance of ShipWorks is already running.");
                }
            }

            // Show the splash screen to give the user some visual feedback that ShipWorks is running
            // while initialization is being performed
            SplashScreen.ShowSplash();
            initializer.Initialize();

            // MainForm must be created after initialization to ensure licensing is in place
            MainForm = new MainForm();
            MainForm.Load += new EventHandler(OnMainFormLoaded);

            SplashScreen.Status = "Loading ShipWorks...";
            Application.Run(MainForm);
        }

        /// <summary>
        /// When the main form has loaded we wait for the splash minimum time to be up.
        /// </summary>
        private void OnMainFormLoaded(object sender, EventArgs e)
        {
            MainForm.Activate();
            SplashScreen.CloseSplash();

            // Start idle processing
            IdleWatcher.Initialize();

            log.InfoFormat("Application activated.");
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void HandleException(Exception exception)
        {
            string userEmail = string.Empty;
            if (UserSession.IsLoggedOn)
            {
                userEmail = UserSession.User.Email;
                UserSession.Logoff(false);
            }

            UserSession.Reset();

            if (ConnectionMonitor.HandleTerminatedConnection(exception))
            {
                log.Info("Terminating due to unrecoverable connection.", exception);
            }
            else
            {
                log.Fatal("Application Crashed", exception);

                // If the splash is shown, the crash window will close it.
                using (CrashWindow dlg = new CrashWindow(exception, true, userEmail))
                {
                    // Need to not set a parent here, in case we are on another thread.  Causes
                    // potential Invoke deadlock.
                    dlg.ShowDialog();
                }

            }
            try
            {
                // This forces windows to close.  If they try to save state or do other stupid things
                // while closing then they will throw an exception.
                Application.Exit();
            }
            catch (Exception termEx)
            {
                log.Error("Termination error", termEx);
            }

            // Application.Exit does not guaranteed that the windows close.  It only tries.  If an exception
            // gets thrown, or they set e.Cancel = true, they won't have closed.
            Application.ExitThread();
        }
    }
}
