using ShipWorks.Core.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// A carrier is being configured for the first time
    /// </summary>
    public class ConfiguringCarrierMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConfiguringCarrierMessage(object sender, ShipmentTypeCode shipmentTypeCode)
        {
            Sender = sender;
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Carrier that is being configured
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }
    }
}
