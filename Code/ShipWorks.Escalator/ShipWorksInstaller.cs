using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Threading;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Collections;
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
        public Result Install(InstallFile file, bool upgradeDatabase)
        {
            log.Info("Starting Install");
            if (!file.IsValid())
            {
                log.InfoFormat("Install File {0} has an invalid hash", file.Path);
                return Result.FromError("Install file is invalid");
            }
            log.InfoFormat("Install {0} file validated", file.Path);

            if (upgradeDatabase)
            {
                Result databaseLock = GetUpgradeLock();

                if (databaseLock.Failure)
                {
                    log.Error(databaseLock.Message);
                    return databaseLock;
                }
            }

            KillShipWorks();

            return RunSetup(file, upgradeDatabase);
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

                // update the status to say we are installing the update, secretly we will give the UI another 30 seconds to exit
                autoUpdateStatusProvider.UpdateStatus("Installing Update");

                // at this point shipworks was asked nicely to close, if it didnt close because there is a dialog open
                // or its in the middle of doing something we are going to wait another 30 seconds before killing it
                int countDown = 30;
                while (Process.GetProcessesByName("shipworks").Where(p => IsRunningWithoutArguments(p)).Any() && countDown > 0)
                {
                    Thread.Sleep(1000);
                    countDown -= 1;
                }
            }

            // now that we have given the UI 60 seconds to close we are going to kill all shipworks processes
            foreach (Process process in Process.GetProcessesByName("shipworks"))
            {
                log.Info($"Killing ShipWorks process.");
                process?.Kill();
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

            for (int secondsLeft = countDownInSeconds; secondsLeft > 0; secondsLeft--)
            {
                Thread.Sleep(1000);

                // Check to see if the UI has been closed, if so skip to the end
                if (Process.GetProcessesByName("shipworks").Where(p => IsRunningWithoutArguments(p)).None())
                {
                    log.Info("It looks like ShipWorks has been closed, skipping the rest of the countdown.");
                    return;
                }

                string message = $"The application will automatically close in {secondsLeft} seconds.";
                autoUpdateStatusProvider.UpdateStatus(message);
                log.Info(message);
            }

            log.Info($"Asking ShipWorks to close.");
            communicationBridgeFactory($"{serviceName.GetInstanceID().ToString()}_AutoUpdateStart").SendMessage("CloseShipWorks");
        }

        /// <summary>
        /// If SW is running without arguments, it is open in ui mode
        /// </summary>
        private bool IsRunningWithoutArguments(Process process)
        {
            string commandLine = GetCommandLine(process);

            return commandLine?.Trim().EndsWith("shipworks.exe\"", StringComparison.InvariantCultureIgnoreCase) ?? false;
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
        /// Get a database lock
        /// </summary>
        private Result GetUpgradeLock()
        {
            string process = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\\swc.exe";
            string arg = "/command=getupdatelock";

            int exitCode;
            log.InfoFormat("Executing {0} {1}", process, arg);

            using (Process proc = Process.Start(process, arg))
            {
                proc.WaitForExit();
                exitCode = proc.ExitCode;
            }

            return exitCode > 0 ?
                Result.FromError($"Could not obtain database lock with exit code {exitCode}") :
                Result.FromSuccess();
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
