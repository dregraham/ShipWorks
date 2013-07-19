using System.Collections.Generic;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using ShipWorks.ApplicationCore.Interaction;

namespace ShipWorks.ApplicationCore.Services.Installers
{
    /// <summary>
    /// Uninstalls the ShipWorks Service. Providing the command line argument /cmd:uninstallservice when 
    /// running ShipWorks from the command line will trigger this handler.
    /// </summary>
    public class ShipWorksCommandUninstaller : ICommandLineCommandHandler
    {
        /// <summary>
        /// The name of the command handled by the handler.  Must be unique within the application
        /// </summary>
        public string CommandName
        {
            get { return "uninstallservice"; }
        }

        /// <summary>
        /// Execute the command with the given arguments.  If the arguments are not valid for the command,
        /// a CommandLineCommandException is thrown.
        /// </summary>
        public void Execute(List<string> args)
        {
            using (ShipWorksSchedulerService schedulerService = new ShipWorksSchedulerService())
            {
                ShipWorksServiceManager serviceManager = new ShipWorksServiceManager(schedulerService);
                if (serviceManager.IsServiceInstalled())
                {
                    if (serviceManager.GetServiceStatus() != ServiceControllerStatus.Stopped)
                    {
                        serviceManager.StopService();
                    }

                    // The equivalent of running installutil /u [path]/ShipWorks.exe
                    ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                }
            }
        }
    }
}
