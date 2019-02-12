using System;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.IO;
using log4net;
using log4net.Config;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// The Escalator program file
    /// </summary>
    static class Program
    {
        static ILog log;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            string serviceName = ServiceName.Resolve();
            string parameter = string.Concat(args);

            SetupLogging(parameter);

            // The service is calling itself via the installer, so we may have a parameter and
            // be not in UserInteractive mode. That is why we do the check at the default branch
            // of the switch statement.
            switch (parameter)
            {
                case "--launchshipworks":
                    ShipWorksLauncher.StartShipWorks();
                    break;

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

                case "--log":
                    log.Error("test message");
                    break;
#endif
                default:
                    if (!Environment.UserInteractive)
                    {
                        RunService();
                    }
                    break;
            }
        }

        /// <summary>
        /// Configure log4net
        /// </summary>
        private static void SetupLogging(string parameter)
        {            
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string logFolder = $"{DateTime.Now.ToString(DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss"))} - Escalator{parameter.Replace("--"," - ")}";

            string logName = Path.Combine(appData,
                "Interapptive\\ShipWorks\\Instances",
                ServiceName.GetInstanceID().ToString("B"),
                "Log",
                logFolder,
                "ShipWorks.Escelator.log");

            GlobalContext.Properties["LogName"] = logName;
             
            XmlConfigurator.Configure();

            log = LogManager.GetLogger(typeof(Program));
            log.Info("Logging initialized");
        }

        /// <summary>
        /// Uninstall the ShipWorks escalator service
        /// </summary>
        private static void UninstallService(string serviceName)
        {
            log.Info($"Uninstalling Service: {serviceName}");
            ServiceController service = ServiceController.GetServices().SingleOrDefault(s => s.ServiceName == serviceName);

            if (service != null)
            {
                log.Info("Service found.");
                StopService(service);

                log.Info("Uninstalling service");
                ManagedInstallerClass.InstallHelper(new string[] { "/u", "/LogFile=", typeof(Program).Assembly.Location });
            }
        }
        
        /// <summary>
        /// Stops the service exists and is currently running
        /// </summary>
        private static void StopService(ServiceController service)
        {
            log.Info("Stopping service");

            if (service != null && service.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                log.Info("Service Stopped");
            }
            else
            {
                log.Info($"Service \"{service?.DisplayName ?? "NoServiceFound"}\" not running. No action taken");
            }
        }

        /// <summary>
        /// Installs the service
        /// </summary>
        private static void InstallService(string serviceName)
        {
            log.Info($"InstallService({serviceName} called");
            ServiceController service = ServiceController.GetServices().SingleOrDefault(s => s.ServiceName == serviceName);
            if (service == null)
            {
                log.Info("Service not previously installed. Installing.");
                ManagedInstallerClass.InstallHelper(new string[] { $"/ServiceName={serviceName}", "/LogFile=", typeof(Program).Assembly.Location });
                log.Info("Service installed");
                SetRecoveryOptions(serviceName);
                service = new ServiceController(serviceName);
            }
            else
            {
                log.Info("Service already installed");
            }

            if (service.Status == ServiceControllerStatus.Stopped)
            {
                log.Info("Starting Service");
                service.Start();
                log.Info("Service started");
            }
        }

        /// <summary>
        /// Method to run the actual service
        /// </summary>
        private static void RunService()
        {
            log.Info("Starting as a service");
            ServiceBase.Run(new Escalator());
        }

        /// <summary>
        /// Set service to restart after 3 crashes
        /// </summary>
        static void SetRecoveryOptions(string serviceName)
        {
            log.Info("Setting recovery options");

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
                log.Error($"Recovery option failed with ExitCode {exitCode}");
                throw new InvalidOperationException();
            }

            log.Info("Recovery options set");
        }
    }
}
