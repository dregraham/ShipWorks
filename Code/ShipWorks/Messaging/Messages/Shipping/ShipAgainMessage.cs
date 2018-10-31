using System;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Settings;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Request that a shipment be shipped again
    /// </summary>
    public struct ShipAgainMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipAgainMessage(object sender, ShipmentEntity shipment, UIMode uiMode)
        {
            Sender = sender;
            Shipment = shipment;
            MessageId = Guid.NewGuid();
            UIMode = uiMode;
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
        /// Shipment to ship again
        /// </summary>
        public ShipmentEntity Shipment { get; }
        
        /// <summary>
        /// UI mode when message was sent
        /// </summary>
        public UIMode UIMode { get; }
    }
}
