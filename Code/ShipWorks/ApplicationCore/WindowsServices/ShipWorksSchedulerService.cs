using System;
using ShipWorks.Actions.Scheduling;
using System.Threading;
using ShipWorks.Data.Connection;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    [System.ComponentModel.DesignerCategory("Component")]
    public partial class ShipWorksSchedulerService : ShipWorksServiceBase
    {
        IScheduler scheduler;
        CancellationTokenSource canceller;

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
            canceller = new CancellationTokenSource();

            Scheduler.RunAsync(canceller.Token);
        }

        protected override void OnStop()
        {
            canceller.Cancel();
        }
    }
}
