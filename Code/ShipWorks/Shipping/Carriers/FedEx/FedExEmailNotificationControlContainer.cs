using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Notification control type map
    /// </summary>
    internal struct FedExEmailNotificationControlContainer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExEmailNotificationControlContainer(CheckBox control, FedExEmailNotificationType notificationType)
        {
            Control = control;
            NotificationType = notificationType;
        }

        /// <summary>
        /// Check box
        /// </summary>
        public CheckBox Control { get; }

        /// <summary>
        /// Notification type
        /// </summary>
        public FedExEmailNotificationType NotificationType { get; }
    }
}
