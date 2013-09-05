using Interapptive.Shared.UI;
using ShipWorks.Actions;
using ShipWorks.Actions.Scheduling;
using ShipWorks.ApplicationCore.Enums;
using ShipWorks.Data.Utility;
using System.IO;
using System.Threading;
using System.Timers;
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

        private Heartbeat _heartbeat;

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
        /// Starts up the scheduler and the action processor timer.
        /// </summary>
        protected override void OnStartCore()
        {
            base.OnStartCore();

            // Required for printing
            WindowStateSaver.Initialize(Path.Combine(DataPath.WindowsUserSettings, "windows.xml"));

            _heartbeat = new Heartbeat();

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

            canceller.Cancel();
        }

        /// <summary>
        /// If DB changes, make sure ActionProcessor is running.
        /// </summary>
        private void OnTimerInterval(object source, ElapsedEventArgs args)
        {
            // Switch to checking every 2 seconds
            timer.Interval = 2000;
            
            _heartbeat.DoHeartbeat(HeartbeatOptions.None);
        }
    }
}
