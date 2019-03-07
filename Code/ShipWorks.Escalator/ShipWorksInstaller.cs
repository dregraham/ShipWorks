﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Threading;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Installs ShipWorks
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipWorksInstaller : IShipWorksInstaller
    {
        private readonly ILog log;
        private readonly IServiceName serviceName;
        private readonly IAutoUpdateStatusProvider autoUpdateStatusProvider;
        private readonly Func<string, IShipWorksCommunicationBridge> communicationBridgeFactory;
        private bool relaunchShipWorks;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksInstaller(
            Func<Type, ILog> logFactory,
            IServiceName serviceName,
            IAutoUpdateStatusProvider autoUpdateStatusProvider,
            Func<string, IShipWorksCommunicationBridge> communicationBridgeFactory)
        {
            log = logFactory(GetType());
            this.serviceName = serviceName;
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
            this.communicationBridgeFactory = communicationBridgeFactory;
        }

        /// <summary>
        /// Installs ShipWorks
        /// </summary>
        public Result Install(InstallFile file, bool upgradeDatabase, bool killShipWorksUI)
        {
            log.Info("Starting Install");
            if (!file.IsValid())
            {
                log.InfoFormat("Install File {0} has an invalid hash", file.Path);
                return Result.FromError("Install file is invalid");
            }
            log.InfoFormat("Install {0} file validated", file.Path);

            if (killShipWorksUI)
            {
                KillShipWorks();
            }

            WaitForShipWorksUIToExit();

            return RunSetup(file, upgradeDatabase);
        }

        /// <summary>
        /// Wait for the UI to exit
        /// </summary>
        private void WaitForShipWorksUIToExit()
        {
            while (Process.GetProcessesByName("shipworks").Where(p => IsRunningWithoutArguments(p)).Any())
            {
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Kill any instance of Shipworks UI running.
        /// </summary>
        /// <remarks>callback to invoke when shipworks is dead</remarks>
        private void KillShipWorks()
        {
            if (Process.GetProcessesByName("shipworks").Where(p => IsRunningWithoutArguments(p)).Any())
            {
                relaunchShipWorks = true;

                // show the splash screen and patiently wait to see if shipworks closes
                ShowSplashScreenAndAttemptToCloseShipWorks(30);

                // at 60 seconds if shipworks has not closed kill it
                var killShipWorksTimer = new System.Timers.Timer(60000);
                killShipWorksTimer.Elapsed += (a, b) =>
                {
                    foreach (Process process in Process.GetProcessesByName("shipworks").Where(p => IsRunningWithoutArguments(p)))
                    {
                        log.Info($"Killing ShipWorks process.");
                        process.Kill();
                    }
                };
                killShipWorksTimer.Start();
            }
        }

        /// <summary>
        /// Show the splash screen with a countdown, after the countdown attempt to close shipworks
        /// </summary>
        private void ShowSplashScreenAndAttemptToCloseShipWorks(int countDownInSeconds)
        {
            // Show the splash screen to give users feedback that the update
            // is kicking off
            autoUpdateStatusProvider.ShowSplashScreen(serviceName.GetInstanceID().ToString("B"));

            int timeRemaining = countDownInSeconds;
            var countDownTimer = new System.Timers.Timer(1000);
            countDownTimer.Elapsed += (a, b) =>
            {
                if (timeRemaining < 0)
                {
                    // once the countdown has elapsed stop the timer, update the status on the splash
                    // screen and then ask shipworks to close
                    countDownTimer.Stop();
                    autoUpdateStatusProvider.UpdateStatus("Installing Update");
                    log.Info($"Asking ShipWorks to close.");
                    communicationBridgeFactory("AutoUpdateStart").SendMessage("CloseShipWorks");
                }
                else
                {
                    autoUpdateStatusProvider.UpdateStatus($"ShipWorks will automatically close in {timeRemaining} seconds.");
                    log.Info($"ShipWorks will automatically close in {timeRemaining} seconds.");
                    timeRemaining -= 1;
                }
            };

            countDownTimer.Start();
        }

        /// <summary>
        /// If SW is running without arguments, it is open
        /// </summary>
        private bool IsRunningWithoutArguments(Process process)
        {
            string commandLine = GetCommandLine(process);

            return commandLine.Trim().EndsWith("shipworks.exe\"", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the command line that started the process
        /// </summary>
        private static string GetCommandLine(Process process)
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + process.Id))
            using (ManagementObjectCollection objects = searcher.Get())
            {
                return objects.Cast<ManagementBaseObject>().SingleOrDefault()?["CommandLine"]?.ToString();
            }
        }

        /// <summary>
        /// Run ShipWorks setup
        /// </summary>
        private Result RunSetup(InstallFile file, bool upgradeDatabase)
        {
            string upgradeDbParameter = upgradeDatabase ? "/upgradedb" : string.Empty;
            string relaunchParameter = relaunchShipWorks ? "/launchafterinstall" : string.Empty;
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = file.Path;
            string logFileName = serviceName.GetLogFileName("ShipWorks Installer", "install.log");
            Directory.CreateDirectory(Path.GetDirectoryName(logFileName));
            start.Arguments = $"/VERYSILENT /DIR=\"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\" /LOG=\"{logFileName}\" /FORCECLOSEAPPLICATIONS {upgradeDbParameter} {relaunchParameter}";
            log.InfoFormat("Command to run [{0} {1}]", start.FileName, start.Arguments);

            int exitCode;

            log.Info("Starting Install Process");
            autoUpdateStatusProvider.UpdateStatus("Installing Update");
            using (Process proc = Process.Start(start))
            {
                log.Info("Waiting for install to finish");
                proc.WaitForExit();
                log.Info("Install process finished.");
                exitCode = proc.ExitCode;
                log.InfoFormat("Install process exit code {0}", exitCode);
            }

            return exitCode > 0 ?
                Result.FromError($"Update failed with exit code {exitCode}") :
                Result.FromSuccess();
        }
    }
}
