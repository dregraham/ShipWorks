using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Escalator
{
    class ShipWorksUpgrade
    {
        private static ILog log = LogManager.GetLogger(typeof(ShipWorksUpgrade));

        /// <summary>
        /// Upgrade Shipworks to the requested version
        /// </summary>
        public async Task Upgrade(Version version)
        {
            UpdaterWebClient updaterWebClient = new UpdaterWebClient();
            (Uri url, string sha) newVersionInfo = await updaterWebClient.GetVersionToDownload(version).ConfigureAwait(false);

            if (IsInstallRunning(newVersionInfo.url))
            {
                log.ErrorFormat("The installer {0} is already running", newVersionInfo.url);
            }
            else
            {
                await Install(updaterWebClient, newVersionInfo);
            }
        }

        /// <summary>
        /// Download and install new version of Shipworks
        /// </summary>
        private static async Task Install(UpdaterWebClient updaterWebClient, (Uri url, string sha) newVersionInfo)
        {
            log.InfoFormat("The installer {0} is not already running. Beginning Download...", newVersionInfo.url);
            InstallFile newVersion = await updaterWebClient.Download(newVersionInfo.url, newVersionInfo.sha).ConfigureAwait(false);

            log.Info("Attempting to install new version");
            Result installationResult = new ShipWorksInstaller().Install(newVersion);
            if (installationResult.Failure)
            {
                log.ErrorFormat("An error occured while installing the new version of ShipWorks: {0}", installationResult.Message);
            }
        }

        /// <summary>
        /// Detects that the installer is already running
        /// </summary>
        private static bool IsInstallRunning(Uri newVersion)
        {
            string fileName = Path.GetFileNameWithoutExtension(newVersion.LocalPath);
            return Process.GetProcessesByName(fileName).Any();
        }

    }
}
