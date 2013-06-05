using System.ComponentModel;
using System.ServiceProcess;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    [System.ComponentModel.DesignerCategory("")]
    class ShipWorksServiceBase : ServiceBase
    {
        [Browsable(false)]
        public string BaseServiceName { get; private set; }

        protected void InitializeInstance()
        {
            BaseServiceName = ServiceName;
            ServiceName += "$" + ShipWorksSession.InstanceID.ToString("N");
        }
    }
}
