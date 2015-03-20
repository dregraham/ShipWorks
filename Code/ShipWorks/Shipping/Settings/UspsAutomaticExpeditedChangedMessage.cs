using Interapptive.Shared.Messaging;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Message that notifies consumers that a Usps account was just converted to discount
    /// </summary>
    public class UspsAutomaticExpeditedChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsAccountCreatedMessage"/> class.
        /// </summary>
        public UspsAutomaticExpeditedChangedMessage(object sender, ShipmentTypeCode shipmentTypeCode)
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