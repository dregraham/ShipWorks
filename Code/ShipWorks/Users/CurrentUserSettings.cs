using System;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public CurrentUserSettings(IUserSession userSession, IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
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
    }
}
