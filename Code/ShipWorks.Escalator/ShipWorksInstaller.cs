using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
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
        private bool relaunchShipWorks;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksInstaller(Func<Type, ILog> logFactory, IServiceName serviceName, IAutoUpdateStatusProvider autoUpdateStatusProvider)
        {
            log = logFactory(GetType());
            this.serviceName = serviceName;
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
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

            KillShipWorks();
            return RunSetup(file, upgradeDatabase);
        }

        /// <summary>
        /// Kill any instance of Shipworks running.
        /// </summary>
        private void KillShipWorks()
        {
            foreach (Process process in Process.GetProcessesByName("shipworks"))
            {
                // The process has a main window, so we should relaunch
                if (IsRunningWithoutArguments(process))
                {
                    relaunchShipWorks = true;
                }

                process.Kill();
            }

            if (relaunchShipWorks)
            {
                // Show the splash screen to give users feedback that the update
                // is kicking off
                autoUpdateStatusProvider.ShowSplashScreen();
            }
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
