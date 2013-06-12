using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using ShipWorks.ApplicationCore.Interaction;

namespace ShipWorks.ApplicationCore.WindowsServices.Installers
{
    /// <summary>
    /// Installs ShipWorks Service. Triggered command from command line (ShipWorks.exe /cmd:installservice
    /// </summary>
    class ShipWorksCommandInstaller : ICommandLineCommandHandler
    {
        /// <summary>
        /// The name of the command handled by the handler.  Must be unique within the application
        /// </summary>
        public string CommandName
        {
            get { return "installservice"; }
        }

        /// <summary>
        /// Execute the command with the given arguments.  If the arguments are not valid for the command,
        /// a CommandLineCommandException is thrown.
        /// </summary>
        /// <param name="args"></param>
        public void Execute(List<string> args)
        {
            InstallService();
        }

        /// <summary>
        /// Installs the service.
        /// </summary>
        public static void InstallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
            using (var shipWorksSchedulerService = new ShipWorksSchedulerService())
            {
                var serviceManager = new ShipWorksServiceManager(shipWorksSchedulerService);
                serviceManager.StartService();
            }
        }
    }
}
