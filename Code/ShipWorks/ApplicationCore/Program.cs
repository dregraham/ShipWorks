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
using Interapptive.Shared.Win32;
using System.Diagnostics;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Services;
using NDesk.Options;

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
                var uiMode = ExecutionMode as UserInterfaceExecutionMode;
                if (uiMode == null)
                {
                    throw new InvalidOperationException("MainForm is only available in UI execution mode.");
                }

                return uiMode.MainForm;
            }
        }

        /// <summary>
        /// Gets the execution mode for this instance.  (with UI, command line, etc.)
        /// </summary>
        public static ExecutionMode ExecutionMode 
        { 
            get; 
            private set; 
        }

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
            // These come first regardless of ExecutionMode. Even the ServiceExecutionMode uses UI to prompt for credentials.
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            // Has to be done right away.  MessageBoxes can be shown before any ExecutionMode - and can also be shown in any ExecutionMode.  For example, ServiceExecutionMode displays
            // errors when trying to use Windows credentials when installing the service.
            MessageHelper.Initialize("ShipWorks");

            SetupUnhandledExceptionHandling();

            try
            {
                ShipWorksCommandLine commandLine = null;

                try
                {
                    // Determine any command line actions or options
                    commandLine = ShipWorksCommandLine.Parse(Environment.GetCommandLineArgs());
                }
                // No matter what, even if the command line throws, we still initialize logging so that we can log the command line error.
                finally
                {
                    // Load the per-install and per machine identifiers
                    ShipWorksSession.Initialize(commandLine);
                }

                // Load the execution mode, which is command-line dependant
                ExecutionMode = new ExecutionModeFactory(commandLine).Create();

                if (!CheckSystemRequirements())
                {
                    return;
                }

                ExecutionMode.Execute();

                // Log total connections made
                log.InfoFormat("Total connections: {0}", ConnectionMonitor.TotalConnectionCount);
            }
            catch (FileNotFoundException ex)
            {
                string message = "ShipWorks requires " + ex.FileName + ", but the file could not be found.";

                ExecutionMode.ShowTerminationMessage(new InstallationProblemDlg(message), message);
            }
            catch (InstallationException ex)
            {
                ExecutionMode.ShowTerminationMessage(new InstallationProblemDlg(ex.Message), ex.Message);
            }
            catch (ShipWorksServiceException ex)
            {
                ExecutionMode.ShowTerminationMessage(null, ex.Message);
            }
            catch (UserInterfaceAlreadyOpenException ex)
            {
                // We just want to log and eat this exception since it is only being thrown if multiple instances
                // of the ShipWorks UI are opened.
                log.Warn(ex);
            }
            catch (CommandLineCommandArgumentException ex)
            {
                log.Error(ex.Message);
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
                ExecutionMode.ShowTerminationMessage(new NeedWindowsXPSP2(), "Service Pack 2 is required when running Windows XP.");

                return false;
            }

            // Have to have MDAC 2.8
            if (MyComputer.MdacVersion < new Version(2, 80))
            {
                ExecutionMode.ShowTerminationMessage(new NeedMdac28(), "MDAC 2.8 is required.");

                return false;
            }

            // Right now we only support en-US due to string parsing and sql generation issues
            if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
            {
                ExecutionMode.ShowTerminationMessage(new EnglishCultureRequiredDlg(), "en-US culture is required");

                return false;
            }

            // See if a reboot is required
            if (StartupController.CheckRebootRequired())
            {
                ExecutionMode.ShowTerminationMessage(null, "Your computer must be restarted before running ShipWorks.");

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

            // Handle exceptions from GUI threads.
            Application.ThreadException += new ThreadExceptionEventHandler(OnApplicationException);

            // Make sure app exceptions route to the "ThreadException" event
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
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

            string userEmail = string.Empty;
            if (UserSession.IsLoggedOn)
            {
                userEmail = UserSession.User.Email;
                UserSession.Logoff(false);
            }

            UserSession.Reset();

            // If executionMode exists, use it's HandleException method.  Otherwise we'll use the default.
            if (ExecutionMode != null)
            {
                ExecutionMode.HandleException(ex, guiThread, userEmail);
            }
            else
            {
                log.Fatal("Application crashed, and no ExecutionMode to handle exception.", ex);
            }
        }
    }
}
