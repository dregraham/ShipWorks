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
        private readonly IFileWriter fileWriter;
        private readonly IAutoUpdateStatusProvider autoUpdateStatusProvider;
        private readonly IShipWorksLauncher shipWorksLauncher;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksUpgrade(
            IUpdaterWebClient updaterWebClient,
            IShipWorksInstaller shipWorksInstaller,
			IFileWriter fileWriter,
            Func<Type, ILog> logFactory,
            IAutoUpdateStatusProvider autoUpdateStatusProvider,
            IShipWorksLauncher shipWorksLauncher)
        {
            this.updaterWebClient = updaterWebClient;
            this.shipWorksInstaller = shipWorksInstaller;
            this.fileWriter = fileWriter;
            this.autoUpdateStatusProvider = autoUpdateStatusProvider;
            this.shipWorksLauncher = shipWorksLauncher;
            log = logFactory(typeof(ShipWorksUpgrade));
        }

        /// <summary>
        /// Upgrade ShipWorks to the requested version
        /// </summary>
        public async Task Upgrade(Version version)
        {
            bool relaunchShipWorks = true;

            try
            {
                log.InfoFormat("ShipWorksUpgrade attempting to upgrade to version {0}", version);

                ShipWorksRelease shipWorksRelease = await updaterWebClient.GetVersionToDownload(version).ConfigureAwait(false);

                if (shipWorksRelease == null)
                {
                    log.InfoFormat("Version {0} not found by webclient.", version);
                }
                else
                {
                    Result installResult = await Install(shipWorksRelease, false).ConfigureAwait(false);
                    relaunchShipWorks = installResult.Failure;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error trying to upgrade by version", ex);
            }

            if (relaunchShipWorks)
            {
                autoUpdateStatusProvider.CloseSplashScreen();
                shipWorksLauncher.StartShipWorks();
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
            }
        }

        /// <summary>
        /// Install new version of ShipWorks, optionally upgrading the database
        /// </summary>
        private async Task<Result> Install(ShipWorksRelease shipWorksRelease, bool upgradeDatabase)
        {
            if (IsInstallRunning(shipWorksRelease.DownloadUri))
            {
                log.ErrorFormat("The installer {0} is already running", shipWorksRelease.DownloadUrl);
                return Result.FromSuccess();
            }
            else
            {
                fileWriter.WriteUpgradeDetailsToFile(Version.Parse(shipWorksRelease.ReleaseVersion));

                log.InfoFormat("The installer {0} is not already running. Beginning Download...", shipWorksRelease.DownloadUrl);
                InstallFile newVersion = await updaterWebClient.Download(shipWorksRelease.DownloadUri, shipWorksRelease.Hash).ConfigureAwait(false);

                log.Info("Attempting to install new version");
                Result installationResult = shipWorksInstaller.Install(newVersion, upgradeDatabase);
                if (installationResult.Failure)
                {
                    log.ErrorFormat("An error occurred while installing the new version of ShipWorks: {0}", installationResult.Message);
                }

                return installationResult;
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
