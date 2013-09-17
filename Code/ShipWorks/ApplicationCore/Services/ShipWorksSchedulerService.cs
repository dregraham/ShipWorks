using System;
using System.IO;
using System.Threading;
using System.Timers;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Scheduling;
using ShipWorks.ApplicationCore.Enums;
using Timer = System.Timers.Timer;

namespace ShipWorks.ApplicationCore.Services
{
    /// <summary>
    /// Implementation for the ShipWorks Scheduler, so that the UI does not need to be running to process actions
    /// on a scheduled bases.  Also hosts the Scheduler process for dispatching Jobs. 
    /// </summary>
    [System.ComponentModel.DesignerCategory("Component")]
    public partial class ShipWorksSchedulerService : ShipWorksServiceBase
    {
        IScheduler scheduler;
        CancellationTokenSource canceller;

        readonly Timer timer = new Timer();

        Heartbeat heartbeat;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ShipWorksSchedulerService()
        {
            InitializeComponent();
            InitializeInstance();
        }

        /// <summary>
        /// IScheduler for this ShipWorksSchedulerService instance.
        /// </summary>
        public IScheduler Scheduler
        {
            get { return scheduler ?? (scheduler = new Scheduler()); }
            set { scheduler = value; }
        }

        /// <summary>
        /// Starts up the scheduler and the action processor timer.
        /// </summary>
        protected override void OnStartCore()
        {
            base.OnStartCore();

            // Required for printing
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));

            heartbeat = new Heartbeat();

            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Elapsed += OnTimerInterval;

            canceller = new CancellationTokenSource();
            Scheduler.RunAsync(canceller.Token);
        }

        /// <summary>
        /// Stops the scheduler and the action processor timer.
        /// </summary>
        protected override void OnStopCore()
        {
            base.OnStopCore();

            timer.Stop();
            timer.Close();
            timer.Dispose();

            // Communicate to the scheduler that we need to stop
            canceller.Cancel();
        }

        /// <summary>
        /// Called when the service is in the middle of running, and the SQL Session configuration changes mid-stream
        /// </summary>
        protected override void OnSqlConfigurationChanged()
        {
            // Cancel the current Scheduler
            canceller.Cancel();

            // Rerun it to ensure it loads the current SQL Session configuration
            canceller = new CancellationTokenSource();
            Scheduler.RunAsync(canceller.Token);
        }

        /// <summary>
        /// If DB changes, make sure ActionProcessor is running.
        /// </summary>
        private void OnTimerInterval(object source, ElapsedEventArgs args)
        {
            // Switch to checking every 15 seconds
            timer.Interval = (int)TimeSpan.FromSeconds(15).TotalMilliseconds;

            heartbeat.ForceHeartbeat(HeartbeatOptions.None);
        }
    }
}