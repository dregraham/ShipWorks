using System.Collections.Generic;
using System.Collections.ObjectModel;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Notify consumers that one or more shipments have been processed
    /// </summary>
    public class ShipmentsProcessedMessage : IShipWorksMessage
    {
        private object sender;
        private ReadOnlyCollection<ProcessShipmentResult> shipments;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsProcessedMessage(object sender, IEnumerable<ProcessShipmentResult> shipments)
        {
            this.sender = sender;
            this.shipments = shipments.ToReadOnly();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender => sender;

        /// <summary>
        /// Read only collection of shipments that should be processed
        /// </summary>
        public IEnumerable<ProcessShipmentResult> Shipments => shipments;
    }
}
