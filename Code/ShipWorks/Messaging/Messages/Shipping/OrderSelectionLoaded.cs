using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using System.Linq;
using Interapptive.Shared.Collections;

namespace ShipWorks.Core.Messaging.Messages.Shipping
{
    /// <summary>
    /// Class representing the loading of an order's shipment(s).
    /// </summary>
    public struct OrderSelectionLoaded
    {
        /// <summary>
        /// Constructor for success
        /// </summary>
        public OrderSelectionLoaded(OrderEntity order, IEnumerable<ShipmentEntity> shipments, bool destinationAddressEditable)
        {
            Order = order;
            Shipments = shipments.ToReadOnly();
            Exception = null;
            DestinationAddressEditable = destinationAddressEditable;
        }

        /// <summary>
        /// Constructor for errors
        /// </summary>
        public OrderSelectionLoaded(Exception ex)
        {
            Order = null;
            Shipments = Enumerable.Empty<ShipmentEntity>();
            Exception = ex;
            DestinationAddressEditable = false;
        }

        /// <summary>
        /// The shipments
        /// </summary>
        public IEnumerable<ShipmentEntity> Shipments { get; }
        
        /// <summary>
        /// Any exception that may have occured during loading.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Order that has been loaded
        /// </summary>
        public OrderEntity Order { get; }

        /// <summary>
        /// Returns whether the destination address is editable.
        /// </summary>
        public bool DestinationAddressEditable { get; }
    }
}
