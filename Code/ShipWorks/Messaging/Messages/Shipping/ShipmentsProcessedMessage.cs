using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Notify consumers that one or more shipments have been processed
    /// </summary>
    public struct ShipmentsProcessedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsProcessedMessage(object sender, IEnumerable<ProcessShipmentResult> shipments)
        {
            Sender = sender;
            Shipments = shipments.ToReadOnly();
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
        /// Read only collection of shipments that should be processed
        /// </summary>
        public IEnumerable<ProcessShipmentResult> Shipments { get; }
    }
}
