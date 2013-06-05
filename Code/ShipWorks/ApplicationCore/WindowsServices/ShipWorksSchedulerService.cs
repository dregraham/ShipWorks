using System;
using ShipWorks.Actions.Scheduling;
using System.Threading;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    [System.ComponentModel.DesignerCategory("Component")]
    public partial class ShipWorksSchedulerService : ShipWorksServiceBase
    {
        readonly IScheduler scheduler;
        CancellationTokenSource canceller;

        public ShipWorksSchedulerService()
            : this(new Scheduler()) { }

        public ShipWorksSchedulerService(IScheduler scheduler)
        {
            if (null == scheduler)
                throw new ArgumentNullException("scheduler");
            this.scheduler = scheduler;

            InitializeComponent();
            InitializeInstance();
        }


        protected override void OnStart(string[] args)
        {
            canceller = new CancellationTokenSource();

            scheduler.RunAsync(canceller.Token);
        }

        protected override void OnStop()
        {
            canceller.Cancel();
        }
    }
}
