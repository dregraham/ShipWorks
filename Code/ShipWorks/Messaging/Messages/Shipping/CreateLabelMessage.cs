using System;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a single label be created from the shipping view model
    /// </summary>
    public struct CreateLabelMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelMessage(object sender, long shipmentID)
        {
            Sender = sender;
            ShipmentID = shipmentID;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Id of the shipment to create a label for
        /// </summary>
        public long ShipmentID { get; }
    }
}
