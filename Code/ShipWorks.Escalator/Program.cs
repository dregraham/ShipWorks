﻿using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using Autofac;
using Interapptive.Shared.AutoUpdate;
using log4net;
using log4net.Config;
using ShipWorks.Escalator.ApplicationCore;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// The Escalator program file
    /// </summary>
    internal static class Program
    {
        private static ILog log;
        private static string serviceName;
        private static Guid instanceId;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            ContainerInitializer.Initialize();

            using (var scope = IoC.BeginLifetimeScope())
            {
                var serviceNameResolver = scope.Resolve<IServiceName>();
                serviceName = serviceNameResolver.Resolve();
                instanceId = serviceNameResolver.GetInstanceID();
            }

            string parameter = string.Concat(args);

            SetupLogging(parameter);

            // The service is calling itself via the installer, so we may have a parameter and
            // be not in UserInteractive mode. That is why we do the check at the default branch
            // of the switch statement.
            switch (parameter)
            {
                case "--launchshipworks":
                    new AutoUpdateStatusProvider().CloseSplashScreen();
                    new ShipWorksLauncher().StartShipWorks();
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
                case "--checknow":
                    // This is meant for debugging, and it will kick off the process of upgrading.  You'll want to set a few
                    // breakpoints so that you can set the upgrade in milliseconds to 1 and set the assembly version to 0.0.0.1,
                    // but that was simpler than trying to get everything set up just right to work normally.
                    using (var scope = IoC.BeginLifetimeScope())
                    {
                        scope.Resolve<EscalatorService>().Start();
                        Console.WriteLine("Checking for upgrade now...  Press any key to quit.");
                        Console.ReadLine();
                    }

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
            using (var scope = IoC.BeginLifetimeScope())
            {
                var serviceNameResolver = scope.Resolve<IServiceName>();
                GlobalContext.Properties["LogName"] = serviceNameResolver.GetLogFileName($"Escalator{parameter.Replace("--", " - ")}", "ShipWorks.Escalator.log");
            }

            XmlConfigurator.Configure();

            log = LogManager.GetLogger(typeof(Program));
            log.Info("Logging initialized");
        }

        /// <summary>
        /// Uninstall the ShipWorks escalator service
        /// </summary>
        private static void UninstallService(string serviceName)
        {
            log.InfoFormat("Uninstalling Service: {0}", serviceName);
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

            if (service?.Status == ServiceControllerStatus.Running)
            {
                service.Stop();
                log.Info("Service Stopped");
            }
            else
            {
                log.InfoFormat("Service \"{0}\" not running. No action taken", service?.DisplayName ?? "NoServiceFound");
            }
        }

        /// <summary>
        /// Installs the service
        /// </summary>
        private static void InstallService(string serviceName)
        {
            log.InfoFormat("InstallService({0}) called", serviceName);
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
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ServiceBase.Run(scope.Resolve<EscalatorService>());
            }
        }

        /// <summary>
        /// Set service to restart the service in 1 minute if it crashes
        /// </summary>
        private static void SetRecoveryOptions(string serviceName)
        {
            log.Info("Setting recovery options to restart the service in 1 minute if it crashes.");

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
                log.ErrorFormat("Recovery option failed with ExitCode {0}", exitCode);
                throw new InvalidOperationException();
            }

            log.Info("Recovery options set");
        }
    }
}
