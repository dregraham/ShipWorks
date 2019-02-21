using System;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing.TangoRequests;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Check whether the release notes dashboard item should be displayed
    /// </summary>
    [Component]
    public class ReleaseNotesChecker : IReleaseNotesChecker
    {
        private readonly IUserSession userSession;
        private readonly IDashboardManager dashboardManager;
        private readonly ITangoGetReleaseByVersionRequest versionRequest;
        private readonly ISqlSchemaUpdater sqlSchemaUpdater;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReleaseNotesChecker(
            IUserSession userSession,
            IDashboardManager dashboardManager,
            ISqlSchemaUpdater sqlSchemaUpdater,
            ITangoGetReleaseByVersionRequest versionRequest)
        {
            this.sqlSchemaUpdater = sqlSchemaUpdater;
            this.versionRequest = versionRequest;
            this.userSession = userSession;
            this.dashboardManager = dashboardManager;
        }

        /// <summary>
        /// Show the release notes dashboard item, if necessary
        /// </summary>
        public Task<Unit> ShowReleaseNotesIfNecessary(ISynchronizeInvoke invoker, IUserEntity user) =>
            Task.Run(() =>
            {
                var buildVersion = sqlSchemaUpdater.GetBuildVersion();
                if (user.Settings.LastReleaseNotesSeenVersion < buildVersion)
                {
                    userSession.UpdateSettings(x => x.LastReleaseNotesSeenVersion = buildVersion);

                    versionRequest.GetReleaseInfo(buildVersion)
                        .Do(x => invoker.Invoke((Action<string>) ShowDashboardItem, new[] { x.ReleaseNotes }));
                }

                return Unit.Default;
            });

        /// <summary>
        /// Show the dashboard item
        /// </summary>
        private void ShowDashboardItem(string releaseNotes) =>
            dashboardManager.ShowLocalMessage("ReleaseNotes",
                DashboardMessageImageType.LightBulb,
                "ShipWorks was updated",
                "ShipWorks was recently updated. See the release notes for more information",
                new DashboardActionUrl("[link]View Release Notes[/link]", releaseNotes));
    }
}
