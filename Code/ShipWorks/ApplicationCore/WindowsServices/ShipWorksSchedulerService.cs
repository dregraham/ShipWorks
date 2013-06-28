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
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using Timer = System.Timers.Timer;
using System.ComponentModel;

namespace ShipWorks.ApplicationCore.WindowsServices
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
        TimestampTracker timestampTracker;

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
            // Ask the base to process any OnStart needs.
            base.OnStart(args);

            timestampTracker = new TimestampTracker();

            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Elapsed += OnTimerInterval;

            canceller = new CancellationTokenSource();

            Scheduler.RunAsync(canceller.Token);
        }

        /// <summary>
        /// Process any OnStop needs for the service.
        /// </summary>
        protected override void OnStop()
        {
            // Ask the base to process any OnStop needs.
            base.OnStop();

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
