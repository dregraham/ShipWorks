using System;
using System.Reflection;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;

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
                string serviceName = ServiceName.Resolve();
                string parameter = string.Concat(args);

                switch (parameter)
                {
                    case "--install":
                        InstallService(serviceName);
                        break;

                    case "--uninstall":
                        UninstallService(serviceName);
                        break;

                    case "--stop":
                        ServiceController service = ServiceController.GetServices().SingleOrDefault(s => s.ServiceName == serviceName);
                        service?.Stop();
                        break;
                }
            }
            else
            {
                RunService();
            }
        }

        /// <summary>
        /// Uninstall the ShipWorks escalator service
        /// </summary>
        private static void UninstallService(string serviceName)
        {
            ServiceController service = ServiceController.GetServices().SingleOrDefault(s => s.ServiceName == serviceName);
            if (service != null)
            {
                service.Stop();
                ManagedInstallerClass.InstallHelper(new string[] { "/u", "/LogFile=", typeof(Program).Assembly.Location });
            }
        }

        /// <summary>
        /// Installs the service
        /// </summary>
        private static void InstallService(string serviceName)
        {
            ServiceController service = ServiceController.GetServices().SingleOrDefault(s => s.ServiceName == serviceName);
            if (service == null)
            {
                ManagedInstallerClass.InstallHelper(new string[] { $"/ServiceName={serviceName}", "/LogFile=", typeof(Program).Assembly.Location });
                SetRecoveryOptions(serviceName);
                service = new ServiceController(serviceName);
            }

            if (service.Status == ServiceControllerStatus.Stopped)
            {
                service.Start();
            }
        }

        /// <summary>
        /// Method to run the actual service
        /// </summary>
        private static void RunService()
        {
            ServiceBase.Run(new Escalator());
        }

        /// <summary>
        /// Set service to restart after 3 crashes
        /// </summary>
        static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails (the spacing in the below args looks weird but is correct)
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/60000", serviceName);

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
