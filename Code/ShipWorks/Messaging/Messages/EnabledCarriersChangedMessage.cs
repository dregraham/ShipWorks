using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using ShipWorks.Shipping;

namespace ShipWorks.Messaging.Messages
{
    /// <summary>
    /// Indicate that enabled carriers have changed
    /// </summary>
    public class EnabledCarriersChangedMessage : IShipWorksMessage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EnabledCarriersChangedMessage(object sender, IEnumerable<ShipmentTypeCode> typesAdded, IEnumerable<ShipmentTypeCode> typesRemoved)
        {
            Sender = sender;
            Added = typesAdded.ToReadOnly();
            Removed = typesRemoved.ToReadOnly();
            MessageId = Guid.NewGuid();
        }

        /// <summary>
        /// Originator of the message
        /// </summary>
        public object Sender { get; private set; }

        /// <summary>
        /// Id of the message used for tracking purposes
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Shipment types that were added
        /// </summary>
        public IEnumerable<ShipmentTypeCode> Added { get; private set; }

        /// <summary>
        /// Shipment types that were removed
        /// </summary>
        public IEnumerable<ShipmentTypeCode> Removed { get; private set; }
    }
}
