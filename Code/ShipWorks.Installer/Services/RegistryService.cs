using System.Linq;
using Microsoft.Win32;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for interacting with the registry
    /// </summary>
    public class RegistryService : IRegistryService
    {
        /// <summary>
        /// Get the last-used installation path if there is one, or the default path if not
        /// </summary>
        public string GetInstallPath()
        {
            var shipWorksKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Interapptive\\ShipWorks");
            var lastInstanceId = (string) shipWorksKey.GetValue("LastInstalledInstanceID", null);
            var instancesKey = shipWorksKey.OpenSubKey("Instances");
            var previousInstallPaths = instancesKey.GetValueNames();

            return previousInstallPaths.FirstOrDefault(x => ((string) instancesKey.GetValue(x, null)) == lastInstanceId) ?? "C:\\Program Files\\ShipWorks";
        }
    }
}
