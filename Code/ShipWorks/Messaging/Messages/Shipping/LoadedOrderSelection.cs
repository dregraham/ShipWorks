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
        /// <remarks>
        /// In order to get the OrderSelectionChangedHandler to match orders and allow loading to complete
        /// we need the order id to be passed along in the LoadedOrderSelection
        /// </remarks>
        public LoadedOrderSelection(Exception ex, OrderEntity order, IEnumerable<ICarrierShipmentAdapter> shipmentAdapters, ShippingAddressEditStateType destinationAddressEditable)
        {
            Order = order;
            ShipmentAdapters = shipmentAdapters.ToReadOnly();
            Exception = ex;
            DestinationAddressEditable = destinationAddressEditable;
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

        /// <summary>
        /// Create a new LoadedOrderSelection with the given updated shipment
        /// </summary>
        public LoadedOrderSelection CreateSelectionWithUpdatedShipment(ICarrierShipmentAdapter shipmentAdapter)
        {
            if (ShipmentAdapters?.Any() != true)
            {
                return this;
            }

            IEnumerable<ICarrierShipmentAdapter> shipmentAdapters = ShipmentAdapters
                .Where(sa => sa?.Shipment?.ShipmentID != shipmentAdapter?.Shipment?.ShipmentID)
                .Concat(new[] { shipmentAdapter })
                .ToList();

            return new LoadedOrderSelection(Order, shipmentAdapters, DestinationAddressEditable);
        }
    }
}
