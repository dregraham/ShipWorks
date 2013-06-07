using System;
using ShipWorks.Actions.Scheduling;
using System.Threading;
using ShipWorks.Data.Connection;
using System.Timers;
using ShipWorks.Actions;
using ShipWorks.Data.Utility;
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
                if (null == scheduler)
                {
                    scheduler = new Scheduler();
                }

                return scheduler;
            }
            set
            {
                scheduler = value;
            }
        }


        protected override void OnStart(string[] args)
        {
            timestampTracker = new TimestampTracker();

            timer.Enabled = true;
            timer.Interval = 30000;
            timer.Elapsed += OnTimerInterval;

            canceller = new CancellationTokenSource();

            Scheduler.RunAsync(canceller.Token);
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
            if (timestampTracker.CheckForChange())
            {
                ActionProcessor.StartProcessing();
            }
        }
    }
}
