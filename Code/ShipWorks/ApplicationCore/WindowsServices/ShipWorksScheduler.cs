

namespace ShipWorks.ApplicationCore.WindowsServices
{
    [System.ComponentModel.DesignerCategory("Component")]
    partial class ShipWorksScheduler : ShipWorksServiceBase
    {
        public ShipWorksScheduler()
        {
            InitializeComponent();
            InitializeInstance();
        }


        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
