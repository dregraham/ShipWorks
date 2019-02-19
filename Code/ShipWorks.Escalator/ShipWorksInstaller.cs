﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Installs ShipWorks
    /// </summary>
    public class ShipWorksInstaller
    {
        private static ILog log = LogManager.GetLogger(typeof(ShipWorksInstaller));

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

            bool shouldRelaunch = KillShipWorks();
            return RunSetup(file, upgradeDatabase, shouldRelaunch);
        }

        /// <summary>
        /// Kill any instance of Shipworks running.
        /// </summary>
        /// <remarks>
        /// Returns bool if any of the processes were UI
        /// </remarks>
        private bool KillShipWorks()
        {
            bool shouldRelaunch = false;

            foreach (Process process in Process.GetProcessesByName("shipworks"))
            {
                // The process has a main window, so we should relaunch
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    shouldRelaunch = true;
                }

                process.Kill();
            }

            return shouldRelaunch;
        }

        /// <summary>
        /// Run ShipWorks setup
        /// </summary>
        private static Result RunSetup(InstallFile file, bool upgradeDatabase, bool relaunchShipWorks)
        {
            string upgradeDbParameter = upgradeDatabase ? "/upgradedb" : string.Empty;
            string relaunchParameter = relaunchShipWorks ? "/launchafterinstall" : string.Empty;
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = file.Path;
            start.Arguments = $"/VERYSILENT /DIR=\"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\" /log /FORCECLOSEAPPLICATIONS {upgradeDbParameter} {relaunchParameter}";

            int exitCode;

            log.Info("Starting Install Process");
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
