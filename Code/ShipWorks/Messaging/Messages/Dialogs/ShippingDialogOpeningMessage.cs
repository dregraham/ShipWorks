using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.Dialogs
{
    /// <summary>
    /// The shipping dialog is opening
    /// </summary>
    public struct ShippingDialogOpeningMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingDialogOpeningMessage(object sender)
        {
            MessageId = Guid.NewGuid();
            Sender = sender;
        }

        /// <summary>
        /// Id of the message used for tracking
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }
    }
}
