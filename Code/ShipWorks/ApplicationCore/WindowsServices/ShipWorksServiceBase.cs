using System.ComponentModel;
using System.ServiceProcess;


namespace ShipWorks.ApplicationCore.WindowsServices
{
    [System.ComponentModel.DesignerCategory("")]
    public class ShipWorksServiceBase : ServiceBase
    {
        [Description("The ShipWorks service type that this service implements.")]
        public ShipWorksServiceType ServiceType { get; set; }


        [Browsable(false)]
        public string BaseServiceName { get; private set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public string ServiceName
        {
            get { return base.ServiceName; }
            private set { base.ServiceName = value; }
        }


        protected void InitializeInstance()
        {
            BaseServiceName = "ShipWorks" + ServiceType;
            ServiceName = BaseServiceName + "$" + ShipWorksSession.InstanceID.ToString("N");
        }
    }
}
