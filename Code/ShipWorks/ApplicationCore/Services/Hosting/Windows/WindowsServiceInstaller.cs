using Interapptive.Shared.Utility;
using Microsoft.Win32;
using System;
using System.Collections;
using System.ComponentModel;
using System.Security;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Configuration.Install;

namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// Our ServiceInstaller for a ShipWorks service, used by MasterInstaller via InstallUtil.exe
    /// </summary>
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
            ServiceName = ShipWorksServiceManager.GetServiceName(ServiceType);
            DisplayName = ShipWorksServiceManager.GetDisplayName(ServiceType);
        }

        /// <summary>
        /// Indirectly called by InstallUtil to install the service
        /// </summary>
        public override void Install(IDictionary stateSaver)
        {
            InitializeInstance();

            try
            {
                base.Install(stateSaver);
            }
            catch (SecurityException ex)
            {
                throw new ShipWorksServiceException("ShipWorks is not running with the appropriate permissions to install or uninstall services.", ex);
            }
        }

        /// <summary>
        /// Indirectly called by InstallUtil to uninstall the service
        /// </summary>
        public override void Uninstall(IDictionary savedState)
        {
            InitializeInstance();

            try
            {
                base.Uninstall(savedState);
            }
            catch (Win32Exception ex)
            {
                // 1060 => ERROR_SERVICE_DOES_NOT_EXIST
                if (ex.NativeErrorCode != 1060)
                {
                    throw new ShipWorksServiceException("An error occurred while uninstalling the service: " + ex.Message, ex);
                }

                throw;
            }
            catch (InstallException ex)
            {
                throw new ShipWorksServiceException("ShipWorks is not running with the appropriate permissions to install or uninstall services.", ex);
            }
        }

        /// <summary>
        /// Service save is committing
        /// </summary>
        protected override void OnCommitting(IDictionary savedState)
        {
            base.OnCommitting(savedState);

            using (var serviceKey = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Services\" + ServiceName, true))
            {
                serviceKey.SetValue("ImagePath", (string)serviceKey.GetValue("ImagePath") + " /service=" + EnumHelper.GetApiValue(ServiceType));
            }
        }
    }
}
