using System;
using System.ComponentModel;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Check whether the release notes dashboard item should be displayed
    /// </summary>
    [Component]
    public class UpgradeResultsChecker : IUpgradeResultsChecker
    {
        private readonly IUserSession userSession;
        private readonly IDashboardManager dashboardManager;
        private readonly ILog log;
        private readonly ISqlSchemaUpdater sqlSchemaUpdater;
        private readonly IConfigurationData configurationData;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpgradeResultsChecker(
            IUserSession userSession,
            IDashboardManager dashboardManager,
            ISqlSchemaUpdater sqlSchemaUpdater,
            IConfigurationData configurationData,
            Func<Type, ILog> createLogger)
        {
            this.configurationData = configurationData;
            this.sqlSchemaUpdater = sqlSchemaUpdater;
            this.userSession = userSession;
            this.dashboardManager = dashboardManager;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Show the release notes dashboard item, if necessary
        /// </summary>
        public Task<Unit> ShowUpgradeNotificationIfNecessary(ISynchronizeInvoke invoker, IUserEntity user) =>
            Task.Run(() =>
            {
                var buildVersion = sqlSchemaUpdater.GetBuildVersion();
                var upgradingToVersion = GetUpgradingToVersion();
                var lastSeenVersion = user.Settings.LastReleaseNotesSeenVersion;

                if (upgradingToVersion > buildVersion)
                {
                    var nextUpdateWindow = configurationData.GetNextUpdateWindow(DateTime.Now);
                    invoker.Invoke((Action<DateTime>) ShowUpgradeError, new object[] { nextUpdateWindow });
                }
                else if (buildVersion > lastSeenVersion)
                {
                    userSession.UpdateSettings(x => x.LastReleaseNotesSeenVersion = buildVersion);

                    invoker.Invoke((Action) ShowDashboardItem, null);
                }

                return Unit.Default;
            });

        /// <summary>
        /// Get the version that we're upgrading to, if available
        /// </summary>
        /// <returns></returns>
        private Version GetUpgradingToVersion()
        {
            var upgradingToVersion = new Version();

            try
            {
                var upgradeDetailsPath = Path.Combine(DataPath.InstanceRoot, "UpgradeDetails.xml");
                if (File.Exists(upgradeDetailsPath))
                {
                    var serializedDetails = File.ReadAllText(upgradeDetailsPath);
                    var details = SerializationUtility.DeserializeFromXml<UpgradeDetails>(serializedDetails);
                    upgradingToVersion = details.UpgradingToVersion;

                    File.Delete(upgradeDetailsPath);
                }
            }
            catch (Exception ex)
            {
                // If there is any exception during this process, just continue on since we're just deciding whether
                // to show a notification
                log.Error("Error while getting upgrade details", ex);
            }

            return upgradingToVersion;
        }

        /// <summary>
        /// Show the dashboard item
        /// </summary>
        private void ShowDashboardItem() =>
            dashboardManager.ShowLocalMessage("ReleaseNotes",
                DashboardMessageImageType.LightBulb,
                "ShipWorks updated successfully",
                string.Empty);

        /// <summary>
        /// Show the dashboard item
        /// </summary>
        private void ShowUpgradeError(DateTime updateWindow) =>
            dashboardManager.ShowLocalMessage("UpgradeError",
                DashboardMessageImageType.Error,
                "ShipWorks was unable to update.",
                $"Next update attempt: {updateWindow.ToString("MMMM d")} at {updateWindow.ToString("h:00 tt")}");
    }
}
