using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Windows.Forms;


namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    public class WindowsServiceRegistrar : IServiceRegistrar
    {
        public void RegisterAll()
        {
            // The equivalent of running "installutil <path>", which invokes the MasterInstaller.
            ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });

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

                    if (controller.GetServiceStatus() == ServiceControllerStatus.Stopped)
                    {
                        controller.StartService();
                    }
                }
            }
        }

        public void UnregisterAll()
        {
            foreach (var service in ShipWorksServiceBase.GetAllServices())
            {
                var controller = new WindowsServiceController(service);

                if (controller.GetServiceStatus() != ServiceControllerStatus.Stopped)
                {
                    controller.StopService();
                }
            }

            // The equivalent of running "installutil /u <path>".
            ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetExecutingAssembly().Location });
        }
    }
}
