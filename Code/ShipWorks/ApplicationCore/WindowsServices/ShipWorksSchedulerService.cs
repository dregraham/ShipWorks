using System;
using System.Diagnostics;
using System.IO;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Scheduling;
using System.Threading;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using System.Timers;
using ShipWorks.Actions;
using ShipWorks.Data.Utility;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Stores;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using Timer = System.Timers.Timer;

namespace ShipWorks.ApplicationCore.WindowsServices
{
    [System.ComponentModel.DesignerCategory("Component")]
    public partial class ShipWorksSchedulerService : ShipWorksServiceBase
    {
        IScheduler scheduler;
        CancellationTokenSource canceller;

        Timer timer = new Timer();
        TimestampTracker timestampTracker;

        public ShipWorksSchedulerService()
        {
            InitializeComponent();
            InitializeInstance();
        }


        public IScheduler Scheduler
        {
            get
            {
                return scheduler ?? (scheduler = new Scheduler());
            }
            set
            {
                scheduler = value;
            }
        }


        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control 
        /// Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to 
        /// take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            InitializeForApplication();

            timestampTracker = new TimestampTracker();

            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Elapsed += OnTimerInterval;

            canceller = new CancellationTokenSource();

            Scheduler.RunAsync(canceller.Token);
        }

        /// <summary>
        /// Initializes for application.
        /// </summary>
        private static void InitializeForApplication()
        {
            SqlSession.Initialize();
            LogSession.Initialize();
            
            DataProvider.InitializeForApplication();
            AuditProcessor.InitializeForApplication();

            UserSession.InitializeForCurrentDatabase();

            //if (!UserSession.Logon("shipworks", "shipworks", true))
            //{
            //    throw new Exception("A 'shipworks' account with password 'shipworks' needs to be created.");
            //}

            UserManager.InitializeForCurrentUser();
            UserSession.InitializeForCurrentUser();

            // Required for printing
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Close();
            timer.Dispose();

            canceller.Cancel();
        }

        /// <summary>
        /// If DB changes, make sure ActionProcessor is running.
        /// </summary>
        private void OnTimerInterval(object source, ElapsedEventArgs args)
        {
            // Switch to checking every 2 seconds
            timer.Interval = 2000;
            if (timestampTracker.CheckForChange())
            {
                ActionProcessor.StartProcessing();
            }
        }
    }
}
