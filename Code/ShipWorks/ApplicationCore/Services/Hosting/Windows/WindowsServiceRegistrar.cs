using log4net;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;


namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    public class WindowsServiceRegistrar : IServiceRegistrar
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsServiceRegistrar));

        public void RegisterAll()
        {
            log.Info("Registering all services as Windows services.");

            // The equivalent of running "installutil <path>", which invokes the MasterInstaller.
            ManagedInstallerClass.InstallHelper(new[] { "/LogFile=", Assembly.GetExecutingAssembly().Location });

            using (GetWindowsCredentialsDlg credentialsDialog = new GetWindowsCredentialsDlg())
            {
                credentialsDialog.ShowDialog();

                foreach (var service in ShipWorksServiceBase.GetAllServices())
                {
                    var controller = new WindowsServiceController(service);

                    if (credentialsDialog.DialogResult == DialogResult.OK)
                    {
                        controller.ChangeCredentials(credentialsDialog.Domain, credentialsDialog.UserName, credentialsDialog.Password);
                    }

                    using (ServiceController serviceController = controller.GetServiceController())
                    {
                        WindowsServiceRetryConfigurator.SetRecoveryOptions(serviceController);

                        if (controller.GetServiceStatus() == ServiceControllerStatus.Stopped)
                        {
                            controller.StartService();
                        }
                    }
                }
            }
        }

        public void UnregisterAll()
        {
            log.Info("Unregistering all Windows services.");

            foreach (var service in ShipWorksServiceBase.GetAllServices())
            {
                var controller = new WindowsServiceController(service);

                if (controller.IsServiceInstalled() &&
                    controller.GetServiceStatus() != ServiceControllerStatus.Stopped)
                {
                    controller.StopService();
                }
            }

            // The equivalent of running "installutil /u <path>".
            ManagedInstallerClass.InstallHelper(new[] { "/u", "/LogFile=", Assembly.GetExecutingAssembly().Location });
        }
    }
}
