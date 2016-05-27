using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messaging.Messages
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
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Source of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Carrier that was configured
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }
    }
}