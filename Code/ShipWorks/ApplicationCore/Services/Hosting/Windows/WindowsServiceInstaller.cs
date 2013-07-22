using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Security;
using System.ServiceProcess;
using System.Windows.Forms;


namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    [System.ComponentModel.DesignerCategory("")]
    class WindowsServiceInstaller : ServiceInstaller
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

            try
            {
                base.Install(stateSaver);
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is SecurityException)
                {
                    MessageBox.Show("Installer must be run with administrative privileges.");
                }

                throw;
            }

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
