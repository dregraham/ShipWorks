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
        private readonly RegistryKey shipWorksRegistryKey = null;
        private const string defaultInstallPath = "C:\\Program Files\\ShipWorks";

        /// <summary>
        /// Constructor
        /// </summary>
        public RegistryService(Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(RegistryService));

            try
            {
                log.Info("Getting ShipWorks registry key");
                shipWorksRegistryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Interapptive\\ShipWorks");
            }
            catch (Exception)
            {
                log.Info("Could not find ShipWorks registry key");
            }

        }

        /// <summary>
        /// Get the last-used installation path if there is one, or the default path if not
        /// </summary>
        public string GetInstallPath()
        {
            log.Info("Getting install path");

            try
            {
                var lastInstanceID = GetLastInstanceID();

                var instancesKey = shipWorksRegistryKey.OpenSubKey("Instances");
                var previousInstallPaths = instancesKey.GetValueNames();

                var previousPath = previousInstallPaths.FirstOrDefault(x => ((string) instancesKey.GetValue(x, string.Empty)) == lastInstanceID);

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
            return defaultInstallPath;
        }

        /// <summary>
        /// Get the path to the INNO uninstaller
        /// </summary>
        public string GetUninstallerPath()
        {
            log.Info("Getting uninstaller path");
            try
            {
                var lastInstanceID = GetLastInstanceID();
                var uninstallKey = Registry.LocalMachine.OpenSubKey($"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{lastInstanceID}_is1");
                var path = (string) uninstallKey.GetValue("UninstallString", null);

                if (path != null)
                {
                    log.Info($"Got uninstaller path: {path}");
                }

                return path;
            }
            catch (Exception)
            {
                // Do nothing - we couldn't find the uninstaller path
            }

            log.Info("Could not find uninstaller path");
            return null;
        }

        /// <summary>
        /// Get the ID of the most recently installed ShipWorks instance
        /// </summary>
        /// <returns></returns>
        private string GetLastInstanceID()
        {
            var lastInstanceID = (string) shipWorksRegistryKey.GetValue("LastInstalledInstanceID", null);

            if (lastInstanceID != null)
            {
                log.Info($"Found previous instance ID: {lastInstanceID}");
            }

            return lastInstanceID;
        }
    }
}
