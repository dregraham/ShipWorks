using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// A carrier is being configured for the first time
    /// </summary>
    public struct ConfiguringCarrierMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConfiguringCarrierMessage(object sender, ShipmentTypeCode shipmentTypeCode)
        {
            Sender = sender;
            ShipmentTypeCode = shipmentTypeCode;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Carrier that is being configured
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; }
    }
}
