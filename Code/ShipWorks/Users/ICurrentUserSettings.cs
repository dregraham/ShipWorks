using Interapptive.Shared.UI;

namespace ShipWorks.Users
{
    /// <summary>
    /// Settings for the current user
    /// </summary>
    public interface ICurrentUserSettings
    {
        /// <summary>
        /// Should the specified notification type be shown
        /// </summary>
        bool ShouldShowNotification(UserConditionalNotificationType notificationType);

        /// <summary>
        /// Stop showing the given notification for the user
        /// </summary>
        void StopShowingNotification(UserConditionalNotificationType notificationType);
    }
}
