using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using ShipWorks.Shared.Users;

namespace ShipWorks.Users
{
    /// <summary>
    /// Settings for the current user
    /// </summary>
    [Component]
    public class CurrentUserSettings : ICurrentUserSettings
    {
        readonly IUserSession userSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public CurrentUserSettings(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        /// <summary>
        /// Should the specified notification type be shown
        /// </summary>
        public bool ShouldShowNotification(UserConditionalNotificationType notificationType)
        {
            return userSession.Settings?.DialogSettingsObject
                .DismissedNotifications
                .None(x => x == notificationType) != false;
        }

        /// <summary>
        /// Stop showing the given notification for the user
        /// </summary>
        public void StopShowingNotification(UserConditionalNotificationType notificationType)
        {
            DialogSettings settings = userSession.Settings?.DialogSettingsObject;

            settings.DismissedNotifications = settings.DismissedNotifications
                .Concat(new[] { notificationType })
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            userSession.UpdateSettings(x => x.DialogSettingsObject = settings);
        }
    }
}
