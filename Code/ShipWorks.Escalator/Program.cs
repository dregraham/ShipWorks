using System;
using System.Reflection;
using System.ServiceProcess;
using System.Configuration.Install;

namespace ShipWorks.Escalator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new string[] {"/ServiceName=ShipWorks.Escalator", Assembly.GetExecutingAssembly().Location });
                        var sc = new ServiceController("ShipWorksEscalator");
                        sc.Start();
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                }
            }
            else
            {
                RunService();
            }
        }

        private static void RunService()
        {
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                new Escalator()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
