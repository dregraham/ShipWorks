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
                        StopService(service);
                        break;
#if DEBUG
                    case "--debugupdate":
                        Escalator.ProcessMessage("6.2.2.2").Wait();
                        break;
#endif
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
                StopService(service);
                ManagedInstallerClass.InstallHelper(new string[] { "/u", "/LogFile=", typeof(Program).Assembly.Location });
            }
        }
        
        /// <summary>
        /// Stops the service exists and is currently running
        /// </summary>
        private static void StopService(ServiceController service)
        {
            if (service != null && service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
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
