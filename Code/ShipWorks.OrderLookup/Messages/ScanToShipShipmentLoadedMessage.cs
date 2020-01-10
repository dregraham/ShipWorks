using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.Messages
{
    /// <summary>
    /// Message sent when a shipment is loaded into scan to ship
    /// </summary>
    public class ScanToShipShipmentLoadedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipShipmentLoadedMessage(object sender, IShipmentEntity shipment)
        {
            Sender = sender;
            MessageId = Guid.NewGuid();
            Shipment = shipment.AsReadOnly();
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
        /// The shipment that was loaded
        /// </summary>
        public IShipmentEntity Shipment { get; }
    }
}
