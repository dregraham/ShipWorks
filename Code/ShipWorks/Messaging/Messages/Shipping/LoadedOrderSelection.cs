using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Core.Messaging.Messages.Shipping
{
    /// <summary>
    /// Class representing the loading of an order's shipment(s).
    /// </summary>
    public struct LoadedOrderSelection : IOrderSelection
    {
        /// <summary>
        /// Constructor for success
        /// </summary>
        public LoadedOrderSelection(OrderEntity order, IEnumerable<ICarrierShipmentAdapter> shipmentAdapters, ShippingAddressEditStateType destinationAddressEditable)
        {
            Order = order;
            ShipmentAdapters = shipmentAdapters.ToReadOnly();
            Exception = null;
            DestinationAddressEditable = destinationAddressEditable;
        }

        /// <summary>
        /// Constructor for errors
        /// </summary>
        public LoadedOrderSelection(Exception ex)
        {
            Order = null;
            ShipmentAdapters = Enumerable.Empty<ICarrierShipmentAdapter>();
            Exception = ex;
            DestinationAddressEditable = ShippingAddressEditStateType.Editable;
        }

        /// <summary>
        /// The shipments
        /// </summary>
        public IEnumerable<ICarrierShipmentAdapter> ShipmentAdapters { get; }

        /// <summary>
        /// Any exception that may have occurred during loading.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Order that has been loaded
        /// </summary>
        public OrderEntity Order { get; }

        /// <summary>
        /// Returns the ShippingAddressEditStateType of the shipment.
        /// </summary>
        public ShippingAddressEditStateType DestinationAddressEditable { get; }

        /// <summary>
        /// Id of the order selection
        /// </summary>
        public long OrderID => Order?.OrderID ?? -1;
    }
}
