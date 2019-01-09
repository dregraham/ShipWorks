﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using ActiproSoftware.SyntaxEditor;
using CefSharp;
using CefSharp.WinForms;
using Interapptive.Shared.Data;
using Interapptive.Shared.IO.Hardware.Scales;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using NDesk.Options;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Stores;
using ShipWorks.UI;
using ShipWorks.UI.Controls;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// An implementation of the IExecutionMode interface intended to be used when running ShipWorks with a UI.
    /// </summary>
    public class UserInterfaceExecutionMode : ExecutionMode
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserInterfaceExecutionMode));

        // Mutex used to indicate the application is alive. The installer uses this to know the app needs
        // shutdown before the installation can continue.
        private Mutex appMutex;
        private int recoveryCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode" /> class.
        /// </summary>
        public UserInterfaceExecutionMode(IEnumerable<string> options)
        {
            options = options ?? new string[0];

            // Need to extract the arguments
            OptionSet optionSet = new OptionSet
                {
                    { "recovery=", v => int.TryParse(v, out recoveryCount) }
                };
            optionSet.Parse(options);
        }

        /// <summary>
        /// Name of this execution mode (User interface, command line, service)
        /// </summary>
        public override string Name
        {
            get
            {
                return "UserInterfaceExecutionMode";
            }
        }

        /// <summary>
        /// Indicates whether the UI is running for the current instance ID.
        /// </summary>
        public static bool IsProcessRunning
        {
            get { return Program.ExecutionMode is UserInterfaceExecutionMode || SingleInstance.IsAlreadyRunning; }
        }

        /// <summary>
        /// Indicates if this execution mode supports displaying a UI, whether or not one is currently displayed or not
        /// </summary>
        public override bool IsUISupported => true;

        /// <summary>
        /// Determines whether the execution mode supports interacting with the user.
        /// </summary>
        /// <returns>
        ///   <c>true</c> ShipWorks is running in a mode that interacts with the user; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsUIDisplayed
        {
            get { return MainForm != null && MainForm.IsHandleCreated; }
        }

        /// <summary>
        /// Single instance of the running application
        /// </summary>
        public MainForm MainForm
        {
            get;
            private set;
        }

        /// <summary>
        /// Runs the ShipWorks UI.
        /// </summary>
        public override Task Execute()
        {
            AppDomain.CurrentDomain.AssemblyResolve += Resolver;

            InitializeChromium();

            // The installer uses this to know the app needs shutdown before the installation can continue.
            appMutex = new Mutex(false, "{AX70DA71-2A39-4f8c-8F97-7F5348493F57}");

            // Make sure overwritten generated code has not changed
            RewriteScanFormMessageAttribute.CheckNecessaryCodeIsInPlace();

            SingleInstance.Register();

            if (!InterapptiveOnly.MagicKeysDown && !InterapptiveOnly.AllowMultipleInstances)
            {
                // If the application is already running, open it now and exit.
                if (SingleInstance.ActivateRunningInstance())
                {
                    // User does not have the permissions to run multiple instances of ShipWorks
                    throw new UserInterfaceAlreadyOpenException("An instance of ShipWorks is already running.");
                }
            }

            // Show the splash screen to give the user some visual feedback that ShipWorks is running
            // while initialization is being performed
            SplashScreen.ShowSplash();

            // Check for illegal cross thread calls in any mode - not just when the debugger is attached, which is the default
            Control.CheckForIllegalCrossThreadCalls = true;

            // Initialize everything
            Initialize();

            // MainForm must be created after initialization to ensure licensing is in place
            MainForm = new MainForm();
            MainForm.Load += new EventHandler(OnMainFormLoaded);

            SplashScreen.Status = "Loading ShipWorks...";
            Application.Run(MainForm);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Overridden to provide custom UI initialization
        /// </summary>
        protected override void Initialize()
        {
            // First do base/common initialization
            base.Initialize();

            // This kicks off scale detection when SW starts instead of waiting for the the scale to initialize the first time
            // it is used which can take upwards to 1 minute.
            ScaleReader.Initialize();

            // Initialize window state
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));
            CollapsibleGroupControl.Initialize(Path.Combine(DataPath.WindowsUserSettings, "collapsiblegroups.xml"));

            // For Divelements licensing
            Divelements.SandGrid.SandGridBase.ActivateProduct("120|iTixOUJcBvFZeCMW0Zqf8dEUqM0=");
            Divelements.SandRibbon.Ribbon.ActivateProduct("120|wmbyvY12rhj+YHC5nTIyBO33bjE=");
            TD.SandDock.SandDockManager.ActivateProduct("120|cez0Ci0UI1owSCvXUNrMCcZQWik=");

            // For syntax editor
            SemanticParserService.Start();

            // Register nudge refreshing
            IdleWatcher.RegisterDatabaseDependentWork("Refresh Nudges", RefreshNudgeThread, "Refreshing Nudges", TimeSpan.FromHours(8));

            // Register some idle cleanup work.
            DataResourceManager.RegisterResourceCacheCleanup();
            DataPath.RegisterTempFolderCleanup();
            LogSession.RegisterLogCleanup();
        }

        /// <summary>
        /// Runs on a schedule to refresh nudges.
        /// </summary>
        private static void RefreshNudgeThread()
        {
            if (UserSession.IsLoggedOn)
            {
                // The user has to be logged on in order to get the stores
                IEnumerable<StoreEntity> stores = StoreManager.GetAllStores();
                NudgeManager.Refresh(stores);
            }
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

            Telemetry.SetUserInterfaceView(EnumHelper.GetDescription(MainForm.UIMode));
            log.InfoFormat("Application activated.");
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override async Task HandleException(Exception exception, bool guiThread, string userEmail)
        {
            bool shouldReopen = false;
            Task sendReportTask = Task.CompletedTask;

            if (ConnectionMonitor.HandleTerminatedConnection(exception))
            {
                log.Info("Terminating due to unrecoverable connection.", exception);
            }
            else
            {
                log.Fatal("Application Crashed", exception);

                if (SqlSession.IsConfigured)
                {
                    log.Fatal(SqlUtility.GetRunningSqlCommands(SqlSession.Current.Configuration.GetConnectionString()));
                }

                CrashDialog crashDialog = new CrashDialog(exception, guiThread, userEmail, recoveryCount);

                try
                {
                    new WindowInteropHelper(crashDialog).Owner = Program.MainForm?.Handle ?? IntPtr.Zero;
                }
                catch (ObjectDisposedException)
                {
                    // If the main form was disposed, we can't get its handle.
                    // That's ok, just open the dialog with no owner
                    crashDialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                }

                sendReportTask = crashDialog.CreateLogTask;
                shouldReopen = crashDialog.ShowDialog().GetValueOrDefault();
            }

            if (shouldReopen)
            {
                ReopenShipWorks();
            }

            await sendReportTask;
        }

        /// <summary>
        /// Reopen ShipWorks
        /// </summary>
        private void ReopenShipWorks()
        {
            // We want to restart using the same arguments as before, except incrementing the value of the recovery attempts
            // to let the new process know it is being started as an attempt to recover from a crash
            List<string> commandArgs = Environment.GetCommandLineArgs().Where(s => !s.Contains("recovery")).ToList();
            commandArgs.Add($"/recovery={recoveryCount + 1}");

            ProcessStartInfo restartInfo = new ProcessStartInfo
            {
                FileName = commandArgs[0],
                Arguments = string.Join(" ", commandArgs.Skip(1))
            };

            SingleInstance.Unregister();

            Process.Start(restartInfo);
            log.Info("Restart succeeded.");
        }

        /// <summary>
        /// Load the app after configuring Chromium
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeChromium()
        {
            var settings = new CefSettings();

            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        /// <summary>
        /// Resolve CEF assemblies using correct bitness
        /// </summary>
        private static Assembly Resolver(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("CefSharp"))
            {
                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";
                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                       Environment.Is64BitProcess ? "x64" : "x86",
                                                       assemblyName);

                if (File.Exists(archSpecificPath))
                {
                    return Assembly.LoadFile(archSpecificPath);
                }
            }

            return null;
        }
    }
}
