using log4net;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;
using System;

namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    /// <summary>
    /// Encapsulates registering and unregistring of ShipWorks services as official windows services
    /// </summary>
    public class WindowsServiceRegistrar : IServiceRegistrar
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceRegistrar));

        /// <summary>
        /// Register (load\install) ShipWorks services into the Windows Service Control Manager
        /// </summary>
        public void RegisterAll()
        {
            log.Info("Registering all services as Windows services.");

            // The equivalent of running "installutil <path>", which invokes the MasterInstaller.
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { "/LogFile=", Program.AppFileName });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.InnerException is ShipWorksServiceException)
                {
                    throw new ShipWorksServiceException(ex.InnerException.Message, ex);
                }

                throw;
            }

            // First off, show the credentials window one time up front
            using (WindowsServiceCredentialsDlg credentialsDialog = new WindowsServiceCredentialsDlg())
            {
                credentialsDialog.ShowDialog();

                // Go through each ShipWorks service
                foreach (var service in ShipWorksServiceManager.GetAllServices())
                {
                    var controller = new WindowsServiceController(service);

                    // See if they specified a windows user account, or just want to use Local System
                    if (credentialsDialog.UseWindowsUserAccount)
                    {
                        controller.ChangeCredentials(credentialsDialog.Domain, credentialsDialog.UserName, credentialsDialog.Password);
                    }

                    // Prepare our recovery options
                    using (ServiceController serviceController = controller.GetServiceController())
                    {
                        WindowsServiceConfiguration.SetRecoveryOptions(serviceController);
                    }

                    // Start the service right away
                    if (controller.GetServiceStatus() == ServiceControllerStatus.Stopped)
                    {
                        controller.StartService();
                    }
                }
            }
        }

        /// <summary>
        /// Unregister (uninstall) all Windows services supported by ShipWorks
        /// </summary>
        public void UnregisterAll()
        {
            log.Info("Unregistering all Windows services.");

            bool anyServiceInstalled = false;

            // First stop each ShipWorks windows service that is running
            foreach (var service in ShipWorksServiceManager.GetAllServices())
            {
                var controller = new WindowsServiceController(service);

                if (controller.IsServiceInstalled())
                {
                    anyServiceInstalled = true;

                    if (controller.GetServiceStatus() != ServiceControllerStatus.Stopped)
                    {
                        controller.StopService();
                    }
                }
            }

            // The equivalent of running "installutil /u <path>".  We only have to do this if there were services actually installed
            if (anyServiceInstalled)
            {
                try
                {
                    ManagedInstallerClass.InstallHelper(new[] { "/u", "/LogFile=", Program.AppFileName });
                }
                catch (InstallException ex)
                {
                    if (ex.InnerException is ShipWorksServiceException)
                    {
                        throw new ShipWorksServiceException(ex.InnerException.Message, ex);
                    }

                    throw;
                }
            }
        }
    }
}
