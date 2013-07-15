using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using ActiproSoftware.SyntaxEditor;
using Interapptive.Shared;
using Interapptive.Shared.Data;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using log4net;
using NDesk.Options;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.MessageBoxes;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Editions;
using ShipWorks.Filters;
using ShipWorks.Filters.Management;
using ShipWorks.UI;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.ApplicationCore.WindowsServices;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks
{
    static class Program
    {
        // Mutex used to indicate the application is alive. The installer uses this to know the app needs
        // shutdown before the installation can continue.
        static Mutex appMutex;

        // The single main form of the application
        static MainForm mainForm;

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
                if (mainForm == null)
                {
                    // Updated to lazy load the main form to work with the execution mode factory
                    // due to the order that third party license activation/dependencies and other
                    // ShipWorks initialization processes need to be executed in
                    mainForm = new MainForm();
                }
                
                return mainForm;
            }
        }

        /// <summary>
        /// Indicates if the application is running with a UI.  If not, then it must be being ran from the command-line without a UI, or the 
        /// UI is not yet shown.
        /// </summary>
        public static bool IsUserInteractive
        {
            get { return mainForm != null && mainForm.IsHandleCreated; }
        }

        /// <summary>
        /// Gets the folder path containing the ShipWorks executable.
        /// </summary>
        public static string AppLocation
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();
            }

            SetupUnhandledExceptionHandling();

            try
            {
                // The installer uses this to know the app needs shutdown before the installation can continue.
                appMutex = new Mutex(false, "{AX70DA71-2A39-4f8c-8F97-7F5348493F57}");

                // Determine any command line actions or options
                ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(Environment.GetCommandLineArgs());

                // Load the per-install and per machine identifiers
                ShipWorksSession.Initialize(commandLine);

                // Make sure all our data paths have been created and logging initialized
                DataPath.Initialize();
                LogSession.Initialize();

                if (Environment.UserInteractive)
                {
                    // Setup MessageHelper
                    MessageHelper.Initialize("ShipWorks");
                }

                if (!CheckSystemRequirements())
                {
                    return;
                }

                
                ExecutionModeFactory factory = new ExecutionModeFactory(commandLine);

                // TODO: Remove the RunService and RunCommand blocks above as execution modes and factory is ready
                if (!Environment.UserInteractive)
                {
                    RunService(commandLine.ProgramOptions);
                }
                else if (commandLine.IsCommandSpecified)
                {
                    RunCommand(commandLine.CommandName, commandLine.CommandOptions);
                }
                else
                {
                    try
                    {
                        IExecutionMode executionMode = factory.Create();
                        executionMode.Execute();
                    }
                    catch (MultipleExecutionModeInstancesException exception)
                    {
                        // We just want to log and eat this exception since it is only being thrown if multiple instances
                        // of the ShipWorks UI are opened.
                        log.Warn(exception);
                    }
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
                    HandleUnhandledException(ex, false);
            }
            catch (Exception ex)
            {
                HandleUnhandledException(ex, false);
            }
        }

        /// <summary>
        /// Runs ShipWorks as a service
        /// </summary>
        private static void RunService(List<string> options)
        {
            log.Info("Running as a service.");

            CommonInitialization();

            string serviceName = null;

            var optionSet = new OptionSet {
                { "service=", v =>  serviceName = v  }
            };

            optionSet.Parse(options);

            if (null == serviceName)
            {
                log.Error("Service name was not specified.");
                Environment.ExitCode = -1;
                return;
            }

            ShipWorksServiceType serviceType;
            if(!Enum.TryParse<ShipWorksServiceType>(serviceName, out serviceType))
            {
                log.ErrorFormat("'{0}' is not a valid service name.", serviceName);
                Environment.ExitCode = -1;
                return;
            }

            log.InfoFormat("Starting the '{0}' service.", serviceName);

            ServiceBase.Run(ShipWorksServiceBase.GetService(serviceType));
        }

        /// <summary>
        /// Run the given command with its specified options
        /// </summary>
        private static void RunCommand(string command, List<string> options)
        {
            log.InfoFormat("Running command '{0}'", command);

            // Common initialization
            CommonInitialization();

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && !type.IsInterface && typeof(ICommandLineCommandHandler).IsAssignableFrom(type));
            var handlers = types.Select(Activator.CreateInstance).Cast<ICommandLineCommandHandler>().ToList();

            var duplicate = handlers.GroupBy(h => h.CommandName).Where(g => g.Count() > 1).FirstOrDefault();
            if (duplicate != null)
            {
                throw new InvalidOperationException(string.Format("More than one command line handler with command name '{0}' was found.", duplicate.Key));
            }

            ICommandLineCommandHandler handler = handlers.SingleOrDefault(h => h.CommandName == command);
            if (handler == null)
            {
                string error = string.Format("'{0}' is not a valid command line command.", command);

                log.ErrorFormat(error);
                Console.Error.WriteLine(error);

                Environment.ExitCode = -1;

                return;
            }

            try
            {
                handler.Execute(options);
            }
            catch (CommandLineCommandArgumentException ex)
            {
                log.Error(ex.Message, ex);
                Console.Error.WriteLine(ex.Message);

                Environment.ExitCode = -1;
            }
        }

        /// <summary>
        /// Do initialization common to command line or UI.  It's here instead of upfront so that if its UI the splash can already be shown.
        /// </summary>
        private static void CommonInitialization()
        {
            MyComputer.LogEnvironmentProperties();

            // Looking for all types in this assembly that have the LLBLGen DependcyInjection attribute
            DependencyInjectionDiscoveryInformation.ConfigInformation = new DependencyInjectionConfigInformation();
            DependencyInjectionDiscoveryInformation.ConfigInformation.AddAssembly(Assembly.GetExecutingAssembly());

            // SSL certificate policy
            ServicePointManager.ServerCertificateValidationCallback = WebHelper.TrustAllCertificatePolicy;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

            // Override the default of 30 seconds.  We are seeing a lot of timeout crashes in the alpha that I think are due
            // to people's machines just not being able to handle the load, and 30 seconds just wasn't enough.
            SqlCommandProvider.DefaultTimeout = TimeSpan.FromSeconds(Debugger.IsAttached ? 300 : 120);

            // Do initial edition initialization
            EditionManager.Initialize();
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
            if (isTerminating)
            {
                log.Error("Exception recieved while already terminating.", ex);
                return;
            }

            isTerminating = true;

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

                // Application.Exit does not gaurnteed that the windows close.  It only tries.  If an exception
                // gets thrown, or they set e.Cancel = true, they won't have closed.
                Application.ExitThread();
            }
        }
    }
}
