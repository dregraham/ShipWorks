using System.ComponentModel;

namespace Interapptive.Shared.Win32
{
    /// <summary>
    /// Utility class for helping with Windows Firewall
    /// </summary>
    public static class WindowsFirewallUtility
    {

        /// <summary>
        /// Enable the given pre-defined windows firewall service entry, such as 'FILEANDPRINT'
        /// </summary>
        public static void ExecuteSetServiceEnabled(string service)
        {
            NetshUtility.ExecuteNetsh(string.Format("firewall set service {0} ENABLE", service));
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

            try
            {
                int exitCode = NetshUtility.ExecuteNetsh(args);

                if (exitCode != 0)
                {
                    throw new WindowsFirewallException(new Win32Exception(exitCode));
                }
            }
            catch (Win32Exception ex)
            {
                throw new WindowsFirewallException(ex);
            }
        }
    }
}
