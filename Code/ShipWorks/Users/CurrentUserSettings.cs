using System;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Data.Connection;
using ShipWorks.Settings;
using ShipWorks.Shared.Users;

namespace ShipWorks.Users
{
    /// <summary>
    /// Settings for the current user
    /// </summary>
    [Component]
    public class CurrentUserSettings : ICurrentUserSettings
    {
        private readonly IUserSession userSession;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private UIMode? uiMode;

        /// <summary>
        /// Constructor
        /// </summary>
        public CurrentUserSettings(IUserSession userSession, IDateTimeProvider dateTimeProvider, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.userSession = userSession;
        }

        /// <summary>
        /// Should the specified notification type be shown
        /// </summary>
        public bool ShouldShowNotification(UserConditionalNotificationType notificationType)
        {
            return userSession.Settings?
                .DialogSettingsObject
                .NotificationDialogSettings
                .None(x => x.Type == notificationType) != false;
        }

        /// <summary>
        /// Should the specified notification type be shown
        /// </summary>
        public bool ShouldShowNotification(UserConditionalNotificationType notificationType, DateTime date)
        {
            var userSettings = userSession.Settings;

            if (userSettings == null)
            {
                return true;
            }

            if (userSettings.DialogSettingsObject
                .NotificationDialogSettings
                .None(x => x.Type == notificationType))
            {
                return true;
            }

            var notificationSettings = userSettings.DialogSettingsObject
                .NotificationDialogSettings
                .First(x => x.Type == notificationType);

            return notificationSettings.CanShowAfter.HasValue && date > notificationSettings.CanShowAfter;
        }
		
        /// <summary>
        /// Start showing the given notification for the user
        /// </summary>
        public void StartShowingNotification(UserConditionalNotificationType notificationType)
        {
            DialogSettings settings = userSession.Settings?.DialogSettingsObject;

            if (settings != null)
            {
                settings.DismissedNotifications = settings.DismissedNotifications.Except(new[] { notificationType }).ToArray();
                settings.NotificationDialogSettings = settings.NotificationDialogSettings.Where(s => s.Type != notificationType).ToArray();

                userSession.UpdateSettings(x => x.DialogSettingsObject = settings);
            }
        }

        /// <summary>
        /// Stop showing the given notification for the user
        /// </summary>
        public void StopShowingNotification(UserConditionalNotificationType notificationType)
        {
            DialogSettings settings = userSession.Settings?.DialogSettingsObject;

			if (settings == null)
			{
				return;
			}
			
            settings.NotificationDialogSettings = settings.NotificationDialogSettings
                .Where(x => x.Type != notificationType)
                .Append(new NotificationDialogSetting(notificationType))
                .OrderBy(x => x.Type)
                .ToArray();

            userSession.UpdateSettings(x => x.DialogSettingsObject = settings);
        }

        /// <summary>
        /// Stop showing the given notification for the user
        /// </summary>
        public void StopShowingNotificationFor(UserConditionalNotificationType notificationType, TimeSpan waitTime)
        {
            DialogSettings settings = userSession.Settings?.DialogSettingsObject;

            settings.NotificationDialogSettings = settings.NotificationDialogSettings
                .Where(x => x.Type != notificationType)
                .Append(new NotificationDialogSetting(notificationType, dateTimeProvider.UtcNow.Add(waitTime)))
                .OrderBy(x => x.Type)
                .ToArray();

            userSession.UpdateSettings(x => x.DialogSettingsObject = settings);
        }

        /// <summary>
        /// Returns the local UIMode of the current user
        /// </summary>
        public UIMode GetUIMode() => uiMode ?? userSession.Settings.UIMode;

        /// <summary>
        /// Sets the UIMode for the current user
        /// </summary>
        public void SetUIMode(UIMode uiMode)
        {
            this.uiMode = uiMode;
            userSession.User.Settings.UIMode = uiMode;

            // Save the settings
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                adapter.SaveAndRefetch(userSession.User.Settings);
            }
        }

        /// <summary>
        /// Returns the current user's single scan settings.
        /// </summary>
        /// <returns></returns>
        public SingleScanSettings? GetSingleScanSettings() => (SingleScanSettings?) userSession.Settings?.SingleScanSettings;
    }
}
