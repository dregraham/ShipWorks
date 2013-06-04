using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility functions for dealing with Windows OS
    /// </summary>
    public static class WindowsUtility
    {
        static DateTime lastBootTime = DateTime.MinValue;

        /// <summary>
        /// Exit Windows using the given method.  Applications are given a chance to cancel or save data.
        /// </summary>
        public static void ExitWindows(WindowsExitType exitType)
        {
            ExitWindows(exitType, false);
        }

        /// <summary>
        /// Exit Windows using the given method.  If force is true applications are not given a chance
        /// to respond or save data.
        /// </summary>
        public static void ExitWindows(WindowsExitType exitType, bool force)
        {
            ManagementClass os = new ManagementClass("Win32_OperatingSystem");
            os.Get();

            // Enables required security
            os.Scope.Options.EnablePrivileges = true; 

            ManagementBaseObject inParams = os.GetMethodParameters("Win32Shutdown");
            inParams["Flags"] = (((int)exitType) + (force ? 4 : 0)).ToString();
            inParams["Reserved"] = "0";

            foreach (ManagementObject mo in os.GetInstances())
            {
                mo.InvokeMethod("Win32Shutdown", inParams, null);
            }
        }

        /// <summary>
        /// Get the date and time in UTC that the computer was last booted.
        /// </summary>
        public static DateTime LastBootTime
        {
            get
            {
                if (lastBootTime == DateTime.MinValue)
                {
                    SelectQuery query = new SelectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem WHERE Primary='true'");

                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        lastBootTime = ManagementDateTimeConverter.ToDateTime(mo.Properties["LastBootUpTime"].Value.ToString()).ToUniversalTime();
                        break;
                    }
                }

                return lastBootTime;
            }
        }
    }
}
