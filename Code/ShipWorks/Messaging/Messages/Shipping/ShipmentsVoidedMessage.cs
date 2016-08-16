using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Notify consumers that the shipments have been voided
    /// </summary>
    public struct ShipmentsVoidedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsVoidedMessage(object sender, IEnumerable<VoidShipmentResult> voidShipmentResults)
        {
            Sender = sender;
            VoidShipmentResults = voidShipmentResults.ToReadOnly();
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
        /// Read only shipment adapters that were voided
        /// </summary>
        public IEnumerable<VoidShipmentResult> VoidShipmentResults { get; }
    }
}
