using System.Collections;
using System.ComponentModel;
using System.ServiceProcess;
using Microsoft.Win32;


namespace ShipWorks.ApplicationCore.WindowsServices.Installers
{
    [System.ComponentModel.DesignerCategory("")]
    class ShipWorksServiceInstaller : ServiceInstaller
    {
        [Browsable(false)]
        public string BaseServiceName { get; private set; }

        void InitializeInstance()
        {
            BaseServiceName = ServiceName;
            ServiceName += "$" + ShipWorksSession.InstanceID.ToString("N");
            DisplayName += " " + ShipWorksSession.InstanceID.ToString("B").ToUpper();
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

            serviceKey.SetValue("ImagePath", (string)serviceKey.GetValue("ImagePath") + " /service=" + BaseServiceName);
        }
    }
}
