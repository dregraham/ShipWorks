using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a return shipment be created
    /// </summary>
    public struct CreateReturnShipmentMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreateReturnShipmentMessage(object sender, ShipmentEntity shipment)
        {
            Sender = sender;
            Shipment = shipment;
            MessageId = Guid.NewGuid();
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
        /// Id of the shipment to create a return shipment for
        /// </summary>
        public ShipmentEntity Shipment { get; }
    }
}
