using Interapptive.Shared;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.MessageBoxes;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ShipWorks
{
    static class Program
    {
        // Indicates if the application is shutting down due to an exception
        static bool isTerminating = false;

        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(Program));

        /// <summary>
        /// Single instance of the running application
        /// </summary>
        public static MainForm MainForm
        {
            get
            {
                var ui = ExecutionMode as UserInterfaceExecutionMode;
                if (ui == null)
                    throw new InvalidOperationException("MainForm is only available in UI execution mode.");

                return ui.MainForm;
            }
        }

        /// <summary>
        /// Gets the execution mode for this instance.  (with UI, command line, etc.)
        /// </summary>
        public static IExecutionMode ExecutionMode { get; private set; }

        /// <summary>
        /// Gets the path to the ShipWorks executable.
        /// </summary>
        public static string AppFileName
        {
            get { return Assembly.GetExecutingAssembly().Location; }
        }

        /// <summary>
        /// Gets the folder path containing the ShipWorks executable.
        /// </summary>
        public static string AppLocation
        {
            get { return Path.GetDirectoryName(AppFileName); }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Message boxes can currently be shown before the execution mode is initialized, so
            // let's initialize this here until all remaining UI is moved out of the startup path.
            MessageHelper.Initialize("ShipWorks");

            SetupUnhandledExceptionHandling();

            try
            {
                // Determine any command line actions or options
                ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(Environment.GetCommandLineArgs());

                // Load the per-install and per machine identifiers
                ShipWorksSession.Initialize(commandLine);
                ExecutionMode = new ExecutionModeFactory(commandLine).Create();

                if (!CheckSystemRequirements())
                {
                    return;
                }

                try
                {
                    ExecutionMode.Execute();
                }
                catch (MultipleExecutionModeInstancesException exception)
                {
                    // We just want to log and eat this exception since it is only being thrown if multiple instances
                    // of the ShipWorks UI are opened.
                    log.Warn(exception);
                }
                catch (ExecutionModeConfigurationException ex)
                {
                    log.Fatal(ex);
                    Console.Error.WriteLine(ex.Message);
                    Environment.ExitCode = -1;
                    return;
                }

                // Log total connections made
                log.InfoFormat("Total connections: {0}", ConnectionMonitor.TotalConnectionCount);
            }
            catch (FileNotFoundException ex)
            {
                if (Environment.UserInteractive)
                {
                    using (InstallationProblemDlg dlg = new InstallationProblemDlg("ShipWorks requires " + ex.FileName + ", but the file could not be found."))
                    {
                        dlg.ShowDialog();
                    }
                }
                else
                    HandleUnhandledException(ex, false);
            }
            catch (InstallationException ex)
            {
                if (Environment.UserInteractive)
                {
                    using (InstallationProblemDlg dlg = new InstallationProblemDlg(ex.Message))
                    {
                        dlg.ShowDialog();
                    }
                }
                else
                {
                    HandleUnhandledException(ex, false);
                }
            }
            catch (Exception ex)
            {
                HandleUnhandledException(ex, false);
            }

            Environment.Exit(Environment.ExitCode);
        }       

        /// <summary>
        /// Check that all system requirements for running are met
        /// </summary>
        private static bool CheckSystemRequirements()
        {
            // Verify native, which won't be caught by CLR since its not .net
            if (!File.Exists(DataPath.NativeDll))
            {
                log.Error("ShipWorks.Native.dll could not be found.");
                throw new FileNotFoundException("Could not find ShipWorks.Native.dll.", "ShipWorks.Native.dll");
            }

            // If we are on XP, we have to have SP2
            if (MyComputer.IsWindowsXP && MyComputer.ServicePack <= 1)
            {
                log.Error("Service Pack 2 is required when running Windows XP.");

                if (Environment.UserInteractive)
                {
                    using (NeedWindowsXPSP2 dlg = new NeedWindowsXPSP2())
                    {
                        dlg.ShowDialog();
                    }
                }

                return false;
            }

            // Have to have MDAC 2.8
            if (MyComputer.MdacVersion < new Version(2, 80))
            {
                log.Error("MDAC 2.8 is required.");
                if (Environment.UserInteractive)
                {
                    using (NeedMdac28 dlg = new NeedMdac28())
                    {
                        dlg.ShowDialog();
                    }
                }

                return false;
            }

            // Right now we only support en-US due to string parsing and sql generation issues
            if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
            {
                log.Error("en-US culture is required");
                if (Environment.UserInteractive)
                {
                    using (EnglishCultureRequiredDlg dlg = new EnglishCultureRequiredDlg())
                    {
                        dlg.ShowDialog();
                    }
                }

                return false;
            }

            // See if a reboot is required
            if (StartupController.CheckRebootRequired())
            {
                log.Info("A reboot is required before running ShipWorks.");
                if (Environment.UserInteractive)
                {
                    MessageHelper.ShowInformation(null, "Your computer must be restarted before running ShipWorks.");
                }

                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Sets up the application exception handling policy
        /// </summary>
        private static void SetupUnhandledExceptionHandling()
        {
            // Handle non-gui thread exceptions.  These should never happen if we do things right.
            AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler(OnAppDomainException);

            if (Environment.UserInteractive)
            {
                // Handle exceptions from GUI threads.
                Application.ThreadException += new ThreadExceptionEventHandler(OnApplicationException);

                // Make sure app exceptions route to the "ThreadException" event
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            }
        }
        
        /// <summary>
        /// An unhandled exception occurred in the AppDomain, outside of a GUI thread.
        /// </summary>
        private static void OnAppDomainException(object sender, System.UnhandledExceptionEventArgs e)
        {
            HandleUnhandledException((Exception) e.ExceptionObject, false);
        }

        /// <summary>
        /// An unhandled exception occurred in a GUI thread.
        /// </summary>
        private static void OnApplicationException(object sender, ThreadExceptionEventArgs e)
        {
            HandleUnhandledException(e.Exception, true);
        }

        /// <summary>
        /// Handles an unhandled exception.
        /// </summary>
        private static void HandleUnhandledException(Exception ex, bool guiThread)
        {
            // No executionMode, so default to the original exception handling.
            if (isTerminating)
            {
                log.Error("Exception received while already terminating.", ex);
                return;
            }

            isTerminating = true;

            // If executionMode exists, use it's HandleException method.  Otherwise we'll use the default.
            if (ExecutionMode != null)
            {
                ExecutionMode.HandleException(ex);
            }
            else
            {
                DefaultHandleUnhandledException(ex, guiThread);
            }
        }

        /// <summary>
        /// Handles an unhandled exception.
        /// </summary>
        private static void DefaultHandleUnhandledException(Exception ex, bool guiThread)
        {
            string userEmail = "";
            if (UserSession.IsLoggedOn)
            {
                userEmail = UserSession.User.Email;

                UserSession.Logoff(false);
            }

            UserSession.Reset();

            if (ConnectionMonitor.HandleTerminatedConnection(ex))
            {
                log.Info("Terminating due to unrecoverable connection.", ex);
            }
            else
            {
                log.Fatal("Application Crashed", ex);

                if (Environment.UserInteractive)
                {
                    // If the splash is shown, the crash window will close it.
                    using (CrashWindow dlg = new CrashWindow(ex, guiThread, userEmail))
                    {
                        // Need to not set a parent here, in case we are on another thread.  Causes
                        // potential Invoke deadlock.
                        dlg.ShowDialog();
                    }
                }
            }

            if (Environment.UserInteractive)
            {
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
}
