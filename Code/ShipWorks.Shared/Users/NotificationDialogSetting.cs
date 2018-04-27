using System;
using Interapptive.Shared.UI;

namespace ShipWorks.Shared.Users
{
    /// <summary>
    /// Settings for a notification dialog
    /// </summary>
    public struct NotificationDialogSetting
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NotificationDialogSetting(UserConditionalNotificationType type)
        {
            Type = type;
            CanShowAfter = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NotificationDialogSetting(UserConditionalNotificationType type, DateTime canShowAfter)
        {
            Type = type;
            CanShowAfter = canShowAfter;
        }

        /// <summary>
        /// Type of dialog
        /// </summary>
        public UserConditionalNotificationType Type { get; set; }

        /// <summary>
        /// The dialog can be shown after the given date
        /// </summary>
        public DateTime? CanShowAfter { get; set; }

        /// <summary>
        /// Implicitly convert from a notification type to a NotificationDialogSetting
        /// </summary>
        public static implicit operator NotificationDialogSetting(UserConditionalNotificationType notificationType) =>
            new NotificationDialogSetting(notificationType);
    }
}