using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        public string GetUninstallerPath(string installPath)
        {
            log.Info("Getting uninstaller path");
            try
            {
                string uninstallPath = null;

                var instancesKey = shipWorksRegistryKey.OpenSubKey("Instances");
                var previousInstallPaths = instancesKey.GetValueNames();
                var previousPath = previousInstallPaths.FirstOrDefault(x => CleanPath(x).Equals(CleanPath(installPath), StringComparison.OrdinalIgnoreCase));
                string instanceID = (string) instancesKey.GetValue(previousPath, null);

                if (instanceID != null)
                {
                    var uninstallKey = Registry.LocalMachine.OpenSubKey($"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\{instanceID}_is1");
                    uninstallPath = (string) uninstallKey.GetValue("UninstallString", null);

                    if (uninstallPath != null)
                    {
                        log.Info($"Got uninstaller path: {uninstallPath}");
                        return uninstallPath;
                    }
                }

                // Inno creates the uninstaller with a filename of "uninsXXX.exe"
                // where XXX is a number that gets incremented whenever a new
                // version of the installer is run in an existing installation directory
                for (int i = 0; i < 1000; i++)
                {
                    var possiblePath = $"{installPath}Uninstall\\unins{i:D3}.exe";
                    if (File.Exists(possiblePath))
                    {
                        uninstallPath = possiblePath;
                    }
                    else
                    {
                        break;
                    }
                }

                if (uninstallPath != null)
                {
                    log.Info($"Got uninstaller path: {uninstallPath}");
                    return uninstallPath;
                }
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

        /// <summary>
        /// Normalize a file path for comparison
        /// </summary>
        private string CleanPath(string path)
        {
            var pathWithBackslashes = path.Replace('/', '\\');
            var cleanPath = Regex.Replace(pathWithBackslashes, "\\+", "\\");
            return cleanPath.EndsWith('\\') ? cleanPath : cleanPath + "\\";
        }
    }
}
