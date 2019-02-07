using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Installs ShipWorks
    /// </summary>
    public class ShipWorksInstaller
    {
        /// <summary>
        /// Installs ShipWorks
        /// </summary>
        public void Install(InstallFile file)
        {
            if (!file.IsValid)
            {
                throw new InvalidOperationException("Install file is invalid");
            }

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = file.Path;
            start.Arguments = $"/VERYSILENT /DIR={Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}";

            start.WindowStyle = ProcessWindowStyle.Hidden;
            start.CreateNoWindow = true;
            int exitCode;

            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();
                exitCode = proc.ExitCode;
            }

            if (exitCode > 0)
            {
                throw new InvalidOperationException($"Update failed with exit code {exitCode}");
            }
        }
    }
}
