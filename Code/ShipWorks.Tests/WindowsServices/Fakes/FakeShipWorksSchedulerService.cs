using ShipWorks.ApplicationCore.WindowsServices;
using ShipWorks.Actions.Scheduling;


namespace ShipWorks.ApplicationCore.WindowsServices.Tests
{
    [System.ComponentModel.DesignerCategory("")]
    public class FakeShipWorksSchedulerService : ShipWorksSchedulerService
    {
        public FakeShipWorksSchedulerService(IScheduler scheduler)
        {
            base.Scheduler = scheduler;
        }


        new public void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        new public void OnStop()
        {
            base.OnStop();
        }
    }
}
