using System;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Settings;

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
        /// Should the specified notification type be shown
        /// </summary>
        bool ShouldShowNotification(UserConditionalNotificationType notificationType, DateTime date);

        /// <summary>
        /// Start showing the given notification for the user
        /// </summary>
        void StartShowingNotification(UserConditionalNotificationType notificationType);

        /// <summary>
        /// Stop showing the given notification for the user
        /// </summary>
        void StopShowingNotification(UserConditionalNotificationType notificationType);

        /// <summary>
        /// Stop showing the given notification for the user
        /// </summary>
        void StopShowingNotificationFor(UserConditionalNotificationType notificationType, TimeSpan waitTime);

        /// <summary>
        /// Gets the UIMode
        /// </summary>
        UIMode GetUIMode();

        /// <summary>
        /// Sets the UIMode
        /// </summary>
        void SetUIMode(UIMode mode);

        /// <summary>
        /// Gets the users SingleScanSettings
        /// </summary>
        SingleScanSettings? GetSingleScanSettings();
    }
}
