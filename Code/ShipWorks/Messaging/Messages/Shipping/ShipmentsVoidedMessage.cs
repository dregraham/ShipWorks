using System.Collections.Generic;
using System.Collections.ObjectModel;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Notify consumers that the shipments have been voided
    /// </summary>
    public class ShipmentsVoidedMessage : IShipWorksMessage
    {
        private readonly object sender;
        private ReadOnlyCollection<VoidShipmentResult> voidShipmentResults;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentsVoidedMessage(object sender, IEnumerable<VoidShipmentResult> voidShipmentResults)
        {
            this.sender = sender;
            this.voidShipmentResults = voidShipmentResults.ToReadOnly();
        }

        /// <summary>
        /// Sender of the message
        /// </summary>
        public object Sender => sender;

        /// <summary>
        /// Read only shipment adapters that were voided
        /// </summary>
        public IEnumerable<VoidShipmentResult> VoidShipmentResults => voidShipmentResults;
    }
}
