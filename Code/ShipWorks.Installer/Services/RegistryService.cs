using System;
using System.Linq;
using log4net;
using Microsoft.Win32;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for interacting with the registry
    /// </summary>
    public class RegistryService : IRegistryService
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public RegistryService(Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(RegistryService));
        }

        /// <summary>
        /// Get the last-used installation path if there is one, or the default path if not
        /// </summary>
        public string GetInstallPath()
        {
            log.Info("Getting install path");
            try
            {
                var shipWorksKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Interapptive\\ShipWorks");
                var lastInstanceId = (string) shipWorksKey.GetValue("LastInstalledInstanceID", null);

                if (lastInstanceId != null)
                {
                    log.Info($"Found previous instance ID: {lastInstanceId}");
                }

                var instancesKey = shipWorksKey.OpenSubKey("Instances");
                var previousInstallPaths = instancesKey.GetValueNames();

                var previousPath = previousInstallPaths.FirstOrDefault(x => ((string) instancesKey.GetValue(x, string.Empty)) == lastInstanceId);

                if (!string.IsNullOrWhiteSpace(previousPath))
                {
                    log.Info($"Got previous installation path: {previousPath}");
                    return previousPath;
                }
            }
            catch (Exception)
            {
                // Do nothing - we'll return the default path
            }

            log.Info($"Could not find previous installation path, using default");

            return "C:\\Program Files\\ShipWorks";
        }
    }
}
