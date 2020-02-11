using System.Diagnostics;
using log4net;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility for executing netsh commands
    /// </summary>
    public static class NetshCommand
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsFirewallUtility));

        /// <summary>
        /// Execute 'netsh' with the specified arguments
        /// </summary>
        public static int Execute(string args)
        {
            log.Info("Running 'netsh " + args + "'");

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo("netsh", args);
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();

            log.Info($"netsh command returned {process.ExitCode}");

            return process.ExitCode;
        }
    }
}
