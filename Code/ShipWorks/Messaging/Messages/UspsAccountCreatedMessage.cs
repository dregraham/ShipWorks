using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that notifies consumers that a Usps account was just created
    /// </summary>
    public struct UspsAccountCreatedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountCreatedMessage"/> class.
        /// </summary>
        public UspsAccountCreatedMessage(object sender, ShipmentTypeCode shipmentTypeCode)
        {
            Sender = sender;
            ShipmentTypeCode = shipmentTypeCode;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Gets the shipment type code that triggered the event.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }
    }
}
