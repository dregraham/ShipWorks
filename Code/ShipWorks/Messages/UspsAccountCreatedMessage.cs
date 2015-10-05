using ShipWorks.Core.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messages
{
    /// <summary>
    /// Message that notifies consumers that a Usps account was just created
    /// </summary>
    public class UspsAccountCreatedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountCreatedMessage"/> class.
        /// </summary>
        public UspsAccountCreatedMessage(object sender, ShipmentTypeCode shipmentTypeCode)
        {
            Sender = sender;
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Gets the shipment type code that triggered the event.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; private set; }
    }
}
