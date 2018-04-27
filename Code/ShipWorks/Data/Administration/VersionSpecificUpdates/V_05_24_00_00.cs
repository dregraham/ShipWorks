using System;
using System.Linq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Shared.Users;
using ShipWorks.Users;

namespace ShipWorks.Data.Administration.VersionSpecificUpdates
{
    /// <summary>
    /// Migrates dialog settings
    /// </summary>
    /// <seealso cref="ShipWorks.Data.Administration.VersionSpecificUpdates.IVersionSpecificUpdate" />
    public class V_05_24_00_00 : IVersionSpecificUpdate
    {
        private readonly IUserSession userSession;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public V_05_24_00_00(IUserSession userSession, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.userSession = userSession;
        }

        /// <summary>
        /// To which version does this update apply
        /// </summary>
        public Version AppliesTo => new Version(5, 24, 0, 0);

        /// <summary>
        /// Always run just in case it has never been run before.
        /// </summary>
        public bool AlwaysRun => true;

        /// <summary>
        /// Updates ODBC store maps - Points order number to order number complete
        /// </summary>
        public void Update()
        {
            var queryFactory = new QueryFactory();

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                var userSettings = sqlAdapter.FetchQuery(queryFactory.UserSettings)
                    .OfType<UserSettingsEntity>()
                    .Where(x => x.DialogSettings != null)
                    .Select(UpdateDialogSettings)
                    .ToEntityCollection();

                sqlAdapter.SaveEntityCollection(userSettings);
            }
        }

        /// <summary>
        /// Update the dialog settings for a user settings entity
        /// </summary>
        public UserSettingsEntity UpdateDialogSettings(UserSettingsEntity userSettings)
        {
            DialogSettings settings = userSettings.DialogSettingsObject;

            settings.NotificationDialogSettings = settings.DismissedNotifications
                .Except(settings.NotificationDialogSettings.Select(x => x.Type))
                .Select(x => new NotificationDialogSetting(x))
                .Concat(settings.NotificationDialogSettings)
                .OrderBy(x => x.Type)
                .ToArray();

            settings.DismissedNotifications = null;

            userSettings.DialogSettingsObject = settings;

            return userSettings;
        }
    }
}
