using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        public void Install(InstallFile file)
        {
            log.Info("Starting Install");
            if (!file.IsValid)
            {
                log.Info($"Install File {file.Path} has an invalid hash");
                throw new InvalidOperationException("Install file is invalid");
            }
            log.Info($"Install {file.Path} file validated");

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = file.Path;
            start.Arguments = $"/SILENT /DIR={Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}";

            int exitCode;

            log.Info("Starting Install Process");
            using (Process proc = Process.Start(start))
            {
                log.Info("Waiting for install to finish");
                proc.WaitForExit();
                log.Info($"Install process finished.");
                exitCode = proc.ExitCode;
                log.Info($"Install process exit code {exitCode}");
            }

            if (exitCode > 0)
            {
                throw new InvalidOperationException($"Update failed with exit code {exitCode}");
            }
        }
    }
}
