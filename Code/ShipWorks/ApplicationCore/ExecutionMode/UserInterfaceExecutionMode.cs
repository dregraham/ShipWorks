using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using log4net;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Connection;
using ShipWorks.UI;
using ShipWorks.Users;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using System.IO;
using ShipWorks.UI.Controls;
using ActiproSoftware.SyntaxEditor;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Stores;
using Autofac;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping;
using System.Linq;
using ShipWorks.Messaging.Messages;
using TD.SandDock;
using System.Reactive.Linq;

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
        Mutex appMutex;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceExecutionMode" /> class.
        /// </summary>
        public UserInterfaceExecutionMode()
        {

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
        public override bool IsUISupported
        {
            get { return true; }
        }

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
        public override void Execute()
        {
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

            //var builder = new ContainerBuilder();
            //builder.RegisterInstance(MainForm)
            //    .As<Control>()
            //    .ExternallyOwned();
            //builder.Update((IContainer)IoC.UnsafeGlobalLifetimeScope);

            SplashScreen.Status = "Loading ShipWorks...";
            Application.Run(MainForm);
        }
        
        /// <summary>
        /// Overridden to provide custom UI initialization
        /// </summary>
        protected override void Initialize()
        {
            // First do base/common initialization
            base.Initialize();

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
            IdleWatcher.RegisterDatabaseIndependentWork("Refresh Nudges", RefreshNudgeThread, TimeSpan.FromHours(8));

            // Register some idle cleanup work.
            DataResourceManager.RegisterResourceCacheCleanup();
            DataPath.RegisterTempFolderCleanup();
            LogSession.RegisterLogCleanup();

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                lifetimeScope.Resolve<IMessenger>()
                    .OfType<OpenShippingDialogMessage>()
                    .Subscribe(HandleOpenShippingDialogMessage);
            }
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

            log.InfoFormat("Application activated.");
        }

        /// <summary>
        /// Intended to be used as an opportunity to handle any unhandled exceptions that bubble up from
        /// the stack. This would be where things like showing a crash report dialog, would take place
        /// just before the app terminates.
        /// </summary>
        /// <param name="exception">The exception that has bubbled up the entire stack.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void HandleException(Exception exception, bool guiThread, string userEmail)
        {
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

                // If the splash is shown, the crash window will close it.
                using (CrashWindow dlg = new CrashWindow(exception, guiThread, userEmail))
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

        /// <summary>
        /// Handle the open shipping dialog message
        /// </summary>
        /// <param name="obj"></param>
        private void HandleOpenShippingDialogMessage(OpenShippingDialogMessage message)
        {
            if (MainForm.InvokeRequired)
            {
                MainForm.Invoke((Action)(() => OpenShippingDialog(message)));
            }
            else
            {
                OpenShippingDialog(message);
            }
        }

        /// <summary>
        /// Open the shipping dialog
        /// </summary>
        private void OpenShippingDialog(OpenShippingDialogMessage message)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope(ConfigureShippingDialogDependencies))
            {
                // Show the shipping window.  
                ShippingDlg dlg = lifetimeScope.Resolve<ShippingDlg>(new TypedParameter(typeof(OpenShippingDialogMessage), message));
                
                dlg.ShowDialog(Program.MainForm);
            }
        }
		
        /// <summary>
        /// Configure extra dependencies for the shipping dialog
        /// </summary>
        private void ConfigureShippingDialogDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<ShippingDlg>()
                .AsSelf()
                .As<Control>()
                .SingleInstance();
        }
    }
}
