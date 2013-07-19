using System.Collections.Generic;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Services.Hosting.Windows;

namespace ShipWorks.ApplicationCore.Services.Installers
{
    /// <summary>
    /// Installs ShipWorks Service. Providing the command line argument /cmd:installservice when
    /// running ShipWorks from the command line will trigger this handler.
    /// </summary>
    public class ShipWorksCommandInstaller : ICommandLineCommandHandler
    {
        /// <summary>
        /// The name of the command handled by the handler.  Must be unique within the application
        /// </summary>
        public string CommandName
        {
            get
            {
                return "installservice";
            }
        }

        /// <summary>
        /// Execute the command with the given arguments.  If the arguments are not valid for the command,
        /// a CommandLineCommandException is thrown.
        /// </summary>
        /// <param name="args"></param>
        public void Execute(List<string> args)
        {
            using (var shipWorksSchedulerService = new ShipWorksSchedulerService())
            {
                var serviceManager = new WindowsServiceController(shipWorksSchedulerService);

                InstallService(serviceManager);

                if (serviceManager.GetServiceStatus() == ServiceControllerStatus.Stopped)
                {
                    serviceManager.StartService();
                }
            }
        }

        /// <summary>
        /// Installs the service and updates appropraite credentials.
        /// </summary>
        /// <param name="serviceManager">The service manager.</param>
        private static void InstallService(WindowsServiceController serviceManager)
        {
            if (!serviceManager.IsServiceInstalled())
            {
                // Install service
                ManagedInstallerClass.InstallHelper(new[]
                {
                    Assembly.GetExecutingAssembly().Location
                });
            }

            serviceManager.ChangeCredentials();
        }
    }
}