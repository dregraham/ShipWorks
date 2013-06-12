using System.Collections.Generic;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using ShipWorks.ApplicationCore.Interaction;

namespace ShipWorks.ApplicationCore.WindowsServices.Installers
{
    /// <summary>
    /// Installs ShipWorks Service. Triggered command from command line (ShipWorks.exe /cmd:installservice
    /// </summary>
    internal class ShipWorksCommandInstaller : ICommandLineCommandHandler
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
                var serviceManager = new ShipWorksServiceManager(shipWorksSchedulerService);

                InstallService(serviceManager);

                serviceManager.ChangeCredentials();

                if (serviceManager.GetServiceStatus() == ServiceControllerStatus.Stopped)
                {
                    serviceManager.StartService();
                }
            }
        }

        /// <summary>
        /// Installs the service.
        /// </summary>
        /// <param name="serviceManager">The service manager.</param>
        private static void InstallService(ShipWorksServiceManager serviceManager)
        {
            if (!serviceManager.IsServiceInstalled())
            {
                // Install service
                ManagedInstallerClass.InstallHelper(new[]
                {
                    Assembly.GetExecutingAssembly().Location
                });
            }
        }
    }
}