using System;
using System.Reflection;
using System.ServiceProcess;
using System.Configuration.Install;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// The Escalator program file
    /// </summary>
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
                        ManagedInstallerClass.InstallHelper(new string[] {$"/ServiceName={ServiceName.Resolve()}", Assembly.GetExecutingAssembly().Location });
                        var sc = new ServiceController(ServiceName.Resolve());
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

        /// <summary>
        /// Method to run the actual service
        /// </summary>
        private static void RunService()
        {
            ServiceBase.Run(new Escalator());
        }
    }
}
