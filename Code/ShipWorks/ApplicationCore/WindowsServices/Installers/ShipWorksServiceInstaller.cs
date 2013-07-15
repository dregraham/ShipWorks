using Interapptive.Shared.Utility;
using Microsoft.Win32;
using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;


namespace ShipWorks.ApplicationCore.WindowsServices.Installers
{
    [System.ComponentModel.DesignerCategory("")]
    class ShipWorksServiceInstaller : ServiceInstaller
    {
        [Description("The ShipWorks service type that this service implements.")]
        public ShipWorksServiceType ServiceType { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public string ServiceName
        {
            get { return base.ServiceName; }
            private set { base.ServiceName = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public string DisplayName
        {
            get { return base.DisplayName; }
            private set { base.DisplayName = value; }
        }

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        private void InitializeInstance()
        {
            ServiceName = ShipWorksServiceBase.GetServiceName(ServiceType);
            DisplayName = ShipWorksServiceBase.GetDisplayName(ServiceType);
        }


        public override void Install(IDictionary stateSaver)
        {
            InitializeInstance();
            base.Install(stateSaver);
        }

        public override void Uninstall(IDictionary savedState)
        {
            InitializeInstance();
            base.Uninstall(savedState);
        }


        protected override void OnCommitting(IDictionary savedState)
        {
            base.OnCommitting(savedState);

            var serviceKey = Registry.LocalMachine.OpenSubKey(
                @"System\CurrentControlSet\Services\" + ServiceName, true
            );

            serviceKey.SetValue("ImagePath", (string)serviceKey.GetValue("ImagePath") + " /service=" + ServiceType);
        }
    }
}
