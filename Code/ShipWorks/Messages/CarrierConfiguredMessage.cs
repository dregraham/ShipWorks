using ShipWorks.Core.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messages
{
    /// <summary>
    /// A carrier has been configured for the first time
    /// </summary>
    public class CarrierConfiguredMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CarrierConfiguredMessage(object sender, ShipmentTypeCode shipmentTypeCode)
        {
            Sender = sender;
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Carrier that was configured
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }
    }
}