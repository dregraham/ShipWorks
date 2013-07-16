using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.Data.Connection;
using ShipWorks.Editions;
using ShipWorks.Users;
using log4net;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data;
using ShipWorks.Filters;
using ShipWorks.Filters.Management;
using ShipWorks.UI;
using ShipWorks.UI.Controls;
using ShipWorks.ApplicationCore.ExecutionMode.Initialization;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExectionMode interface intended to be used when running ShipWorks with a UI.
    /// </summary>
    public class UserInterfaceExecutionMode : IExecutionMode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserInterfaceExecutionMode));
        private readonly IExecutionModeInitializer initializer;
        private readonly ShipWorksCommandLine commandLine;

        private bool isTerminating;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode" /> class.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        public UserInterfaceExecutionMode(ShipWorksCommandLine commandLine)
            : this(commandLine, new UserInterfaceExecutionModeInitializer())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode" /> class.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <param name="initializer">The initializer.</param>
        public UserInterfaceExecutionMode(ShipWorksCommandLine commandLine, IExecutionModeInitializer initializer)
        {
            isTerminating = false;

            this.initializer = initializer;
            this.commandLine = commandLine;
        }

        /// <summary>
        /// Gets or sets the command line.
        /// </summary>
        /// <value>The command line.</value>
        public ShipWorksCommandLine CommandLine
        {
            get { return commandLine; }
        }

        /// <summary>
        /// Determines whether the exection mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserInteractive()
        {
            return true;
        }

        /// <summary>
        /// Runs the ShipWorks UI.
        /// </summary>
        public void Execute()
        {
            SingleInstance.Register(ShipWorksSession.InstanceID);

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
            initializer.Initialize(this);

            Program.MainForm.Load += new EventHandler(OnMainFormLoaded);

            SplashScreen.Status = "Loading ShipWorks...";
            Application.Run(Program.MainForm);
        }

        /// <summary>
        /// When the main form has loaded we wait for the splash minimum time to be up.
        /// </summary>
        private void OnMainFormLoaded(object sender, EventArgs e)
        {
            Program.MainForm.Activate();
            SplashScreen.CloseSplash();

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
            if (isTerminating)
            {
                log.Error("Exception recieved while already terminating.", exception);
                return;
            }

            isTerminating = true;

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

            // Application.Exit does not gaurnteed that the windows close.  It only tries.  If an exception
            // gets thrown, or they set e.Cancel = true, they won't have closed.
            Application.ExitThread();
        }
    }
}
