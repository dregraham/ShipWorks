using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Escalator
{
    public class ShipWorksInstaller
    {

        public void Install(InstallFile file)
        {
            if (!file.IsValid)
            {
                throw new InvalidOperationException("Install file is invalid");
            }

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = file.Path;
            start.Arguments = "/VERYSILENT";
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
