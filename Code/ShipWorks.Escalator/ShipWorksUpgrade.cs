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
    /// <summary>
    /// Upgrade ShipWorks
    /// </summary>
    class ShipWorksUpgrade
    {
        private static ILog log = LogManager.GetLogger(typeof(ShipWorksUpgrade));
        private static UpdaterWebClient updaterWebClient;

        public ShipWorksUpgrade()
        {
            updaterWebClient = new UpdaterWebClient();
        }

        /// <summary>
        /// Upgrade Shipworks to the requested version
        /// </summary>
        public async Task Upgrade(Version version)
        {
            log.InfoFormat("ShipWorksUpgrade attempting to upgrade to version {0}", version);

            ShipWorksRelease shipWorksRelease = await updaterWebClient.GetVersionToDownload(version).ConfigureAwait(false);

            if (shipWorksRelease == null)
            {
                log.InfoFormat("Version {0} not found by webclient.", version);
                return;
            }

            await Install(shipWorksRelease, false);
        }

        public async Task Upgrade(Version shipworksVersion, string tangoCustomerId)
        {
            ShipWorksRelease shipWorksRelease = await updaterWebClient.GetVersionToDownload(tangoCustomerId).ConfigureAwait(false);
            if (shipWorksRelease == null)
            {
                log.InfoFormat("Version not found for tango customer {0}", tangoCustomerId);
                return;
            }

            if (shipWorksRelease.Version <= shipworksVersion)
            {
                log.InfoFormat("No upgrade needed. ShipWorks client is on version {0} and version returned by tango was {1}.",
                    shipworksVersion,
                    shipWorksRelease.Version);
                return;
            }

            log.InfoFormat("New Version {0} found. Attempting upgrade.");

            await Install(shipWorksRelease, true);
        }

        /// <summary>
        /// Download and install new version of Shipworks
        /// </summary>
        private static async Task Install(ShipWorksRelease shipWorksRelease, bool upgradeDatabase)
        {
            if (IsInstallRunning(shipWorksRelease.DownloadUrl))
            {
                log.ErrorFormat("The installer {0} is already running", shipWorksRelease.DownloadUrl);
            }
            else
            {
                log.InfoFormat("The installer {0} is not already running. Beginning Download...", shipWorksRelease.DownloadUrl);
                InstallFile newVersion = await updaterWebClient.Download(shipWorksRelease.DownloadUrl, shipWorksRelease.Hash).ConfigureAwait(false);

                log.Info("Attempting to install new version");
                Result installationResult = new ShipWorksInstaller().Install(newVersion, upgradeDatabase);
                if (installationResult.Failure)
                {
                    log.ErrorFormat("An error occured while installing the new version of ShipWorks: {0}", installationResult.Message);
                }
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
