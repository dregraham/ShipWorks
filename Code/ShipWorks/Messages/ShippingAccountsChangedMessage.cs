using Interapptive.Shared.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messages
{
    /// <summary>
    /// Message that shipping accounts have changed for a carrier.
    /// </summary>
    public class ShippingAccountsChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingAccountsChangedMessage(object sender, ShipmentTypeCode shipmentTypeCode)
        {
            Sender = sender;
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Shipment type that had accounts change
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }
    }
}
