using System.Diagnostics;
using log4net;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility for executing netsh commands
    /// </summary>
    public static class NetshCommand
    {
        static readonly ILog log = LogManager.GetLogger(typeof(NetshCommand));

        /// <summary>
        /// Execute 'netsh' with the specified arguments
        /// </summary>
        public static int Execute(string args)
        {
            log.Info("Running 'netsh " + args + "'");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("netsh", args);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            log.Info(output);
            log.Info($"netsh command returned {process.ExitCode}");

            return process.ExitCode;
        }
    }
}
