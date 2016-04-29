using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Message that a shipment has changed.
    /// </summary>
    public class ShipmentChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedMessage(object sender, ICarrierShipmentAdapter shipment)
        {
            Sender = sender;
            ShipmentAdapter = shipment;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentChangedMessage(object sender, ICarrierShipmentAdapter shipment, string changedField)
        {
            Sender = sender;
            ShipmentAdapter = shipment;
            ChangedField = changedField;
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Shipment that has changed
        /// </summary>
        public ICarrierShipmentAdapter ShipmentAdapter { get; }

        /// <summary>
        /// The field that has changed
        /// </summary>
        public string ChangedField { get; }
    }
}
