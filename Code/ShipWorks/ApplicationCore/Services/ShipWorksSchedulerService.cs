using System;
using System.IO;
using System.Threading;
using System.Timers;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Scheduling;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Enums;
using Timer = System.Threading.Timer;

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

        Timer timer;

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

            // Create the heartbeat
            heartbeat = new Heartbeat();

            // Start the timer
            timer = new Timer(OnTimerInterval, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

            canceller = new CancellationTokenSource();
            Scheduler.RunAsync(canceller.Token);

            AddressValidationQueue.PerformValidation(canceller.Token);
        }

        /// <summary>
        /// Stops the scheduler and the action processor timer.
        /// </summary>
        protected override void OnStopCore()
        {
            base.OnStopCore();

            // Kill the timer
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }

            // Communicate to the scheduler that we need to stop
            if (canceller != null)
            {
                canceller.Cancel();
                canceller = null;
            }
        }

        /// <summary>
        /// Called when the service is in the middle of running, and the SQL Session configuration changes mid-stream
        /// </summary>
        protected override void OnSqlConfigurationChanged()
        {
            // Cancel the current Scheduler
            if (canceller != null)
            {
                canceller.Cancel();
            }

            // Rerun it to ensure it loads the current SQL Session configuration
            canceller = new CancellationTokenSource();
            Scheduler.RunAsync(canceller.Token);

            AddressValidationQueue.PerformValidation(canceller.Token);
        }

        /// <summary>
        /// Callback for our heartbeat pacemaker
        /// </summary>
        private void OnTimerInterval(object state)
        {
            heartbeat.ForceHeartbeat(HeartbeatOptions.None);
        }
    }
}