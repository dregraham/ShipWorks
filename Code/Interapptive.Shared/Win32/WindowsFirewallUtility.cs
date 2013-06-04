using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Diagnostics;
using System.ComponentModel;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility class for helping with Windows Firewall
    /// </summary>
    public static class WindowsFirewallUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WindowsFirewallUtility));

        /// <summary>
        /// Enable the given pre-defined windows firewall service entry, such as 'FILEANDPRINT'
        /// </summary>
        public static void ExecuteSetServiceEnabled(string service)
        {
            ExecuteNetsh(string.Format("firewall set service {0} ENABLE", service));
        }

        /// <summary>
        /// Open Windows Firewall for the given exe, giving the exclusing the given name
        /// </summary>
        public static void ExecuteAddAllowedProgram(string exePath, string name)
        {
            if (!exePath.StartsWith("\""))
            {
                exePath = "\"" + exePath + "\"";
            }

            string args = string.Format("firewall add allowedprogram program = {0} name = \"{1}\" profile = ALL", exePath, name);

            ExecuteNetsh(args);
        }

        /// <summary>
        /// Execute 'netsh' with the specified arguments
        /// </summary>
        private static void ExecuteNetsh(string args)
        {
            log.Info("Running 'netsh " + args + "'");

            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo("netsh", args);
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new WindowsFirewallException(new Win32Exception(process.ExitCode));
                }
            }
            catch (Win32Exception ex)
            {
                throw new WindowsFirewallException(ex);
            }
        }
    }
}
