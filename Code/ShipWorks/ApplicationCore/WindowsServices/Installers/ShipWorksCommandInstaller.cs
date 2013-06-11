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
    class ShipWorksCommandInstaller : ICommandLineCommandHandler
    {
        public string CommandName
        {
            get { return "installservice"; }
        }

        public void Execute(List<string> args)
        {
            InstallService();
        }

        public static void InstallService()
        {

            ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });

            ServiceManager serviceManager = new ServiceManager(new ShipWorksSchedulerService());
            serviceManager.StartService();
        }
    }
}
