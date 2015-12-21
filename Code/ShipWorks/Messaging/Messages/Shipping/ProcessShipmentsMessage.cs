using System.Collections.Generic;
using System.Collections.ObjectModel;
using Interapptive.Shared.Collections;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Message that requests processing of shipments
    /// </summary>
    public class ProcessShipmentsMessage : IShipWorksMessage
    {
        private object sender;
        private ReadOnlyCollection<ShipmentEntity> shipments;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessShipmentsMessage(object sender, IEnumerable<ShipmentEntity> shipments)
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
        public IEnumerable<ShipmentEntity> Shipments => shipments;
    }
}
