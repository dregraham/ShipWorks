using Interapptive.Shared.UI;

namespace ShipWorks.Users
{
    /// <summary>
    /// Notification that can be dismissed by a user
    /// </summary>
    public interface IUserConditionalNotification
    {
        /// <summary>
        /// Show a conditional notification, if necessary
        /// </summary>
        void Show(IMessageHelper messageHelper, string title, string message, UserConditionalNotificationType notificationType);
    }
}
