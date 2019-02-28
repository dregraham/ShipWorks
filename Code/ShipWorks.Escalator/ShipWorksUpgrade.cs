using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Upgrade ShipWorks
    /// </summary>
    [Component(SingleInstance = true)]
    public class ShipWorksUpgrade : IShipWorksUpgrade
    {
        private readonly ILog log;
        private readonly IUpdaterWebClient updaterWebClient;
        private readonly IShipWorksInstaller shipWorksInstaller;
        private readonly IAutoUpdateStatusProvider autoUpdateStatusProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksUpgrade(
            IUpdaterWebClient updaterWebClient,
            IShipWorksInstaller shipWorksInstaller,
            Func<Type, ILog> logFactory,
            IAutoUpdateStatusProvider autoUpdateStatusProvider)
        {
            this.updaterWebClient = updaterWebClient;
            this.shipWorksInstaller = shipWorksInstaller;
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
            log = logFactory(typeof(ShipWorksUpgrade));
        }

        /// <summary>
        /// Upgrade Shipworks to the requested version
        /// </summary>
        public async Task Upgrade(Version version)
        {
            try
            {
                log.InfoFormat("ShipWorksUpgrade attempting to upgrade to version {0}", version);

                ShipWorksRelease shipWorksRelease = await updaterWebClient.GetVersionToDownload(version).ConfigureAwait(false);

                if (shipWorksRelease == null)
                {
                    log.InfoFormat("Version {0} not found by webclient.", version);
                    return;
                }

                await Install(shipWorksRelease, false).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.Error("Error trying to upgrade by version", ex);
                throw;
            }
        }

        /// <summary>
        /// Download and install the next version of ShipWorks for the given customer if it is available
        /// </summary>
        public async Task Upgrade(string tangoCustomerId)
        {
            try
            {
                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                ShipWorksRelease shipWorksRelease = await updaterWebClient.GetVersionToDownload(tangoCustomerId, currentVersion).ConfigureAwait(false);
                if (shipWorksRelease == null)
                {
                    log.InfoFormat("New version not found for tango customer {0} running version {1}", tangoCustomerId, currentVersion);
                    return;
                }

                Version releaseVersion = Version.Parse(shipWorksRelease.ReleaseVersion);
                if (releaseVersion <= currentVersion)
                {
                    log.InfoFormat("No upgrade needed. ShipWorks client is on version {0} and version returned by tango was {1}.",
                         currentVersion,
                        shipWorksRelease.ReleaseVersion);
                    return;
                }

                log.InfoFormat("New Version {0} found. Attempting upgrade.", shipWorksRelease.ReleaseVersion);

                await Install(shipWorksRelease, true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.Error("Error trying to upgrade by customer id", ex);
                throw;
            }
        }

        /// <summary>
        /// Install new version of ShipWorks, optionally upgrading the database
        /// </summary>
        private async Task Install(ShipWorksRelease shipWorksRelease, bool upgradeDatabase)
        {
            if (IsInstallRunning(shipWorksRelease.DownloadUri))
            {
                log.ErrorFormat("The installer {0} is already running", shipWorksRelease.DownloadUrl);
            }
            else
            {
                log.InfoFormat("The installer {0} is not already running. Beginning Download...", shipWorksRelease.DownloadUrl);
                InstallFile newVersion = await updaterWebClient.Download(shipWorksRelease.DownloadUri, shipWorksRelease.Hash).ConfigureAwait(false);

                log.Info("Attempting to install new version");
                Result installationResult = shipWorksInstaller.Install(newVersion, upgradeDatabase);
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
