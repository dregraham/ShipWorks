using System;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messaging.Messages.Dialogs
{
    /// <summary>
    /// Open the profile manager dialog
    /// </summary>
    public struct OpenProfileManagerDialogMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OpenProfileManagerDialogMessage(object sender)
        {
            Sender = sender;
            RestrictToShipmentType = null;
            OnComplete = null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OpenProfileManagerDialogMessage(object sender, ShipmentTypeCode restrictToShipmentType, Action onComplete)
        {
            Sender = sender;
            RestrictToShipmentType = restrictToShipmentType;
            OnComplete = onComplete;
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Restrict the dialog to the specified shipment type
        /// </summary>
        public ShipmentTypeCode? RestrictToShipmentType { get; }

        /// <summary>
        /// Action to perform when the dialog is closed
        /// </summary>
        public Action OnComplete { get; }
    }
}
