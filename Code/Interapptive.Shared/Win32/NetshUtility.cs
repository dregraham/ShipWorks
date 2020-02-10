using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility for executing netsh commands
    /// </summary>
    public static class NetshUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsFirewallUtility));

        /// <summary>
        /// Execute 'netsh' with the specified arguments
        /// </summary>
        public static int ExecuteNetsh(string args)
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
