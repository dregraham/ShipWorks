using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private readonly IImmutableDictionary<long, ShippingAddressEditStateType> destinationAddressEditable;

        /// <summary>
        /// Constructor for success
        /// </summary>
        public LoadedOrderSelection(OrderEntity order, IEnumerable<ICarrierShipmentAdapter> shipmentAdapters, IEnumerable<KeyValuePair<long, ShippingAddressEditStateType>> destinationAddressEditable)
        {
            Order = order;
            ShipmentAdapters = shipmentAdapters.ToReadOnly();
            Exception = null;
            this.destinationAddressEditable = destinationAddressEditable.ToImmutableDictionary();
        }

        /// <summary>
        /// Constructor for errors
        /// </summary>
        /// <remarks>
        /// In order to get the OrderSelectionChangedHandler to match orders and allow loading to complete
        /// we need the order id to be passed along in the LoadedOrderSelection
        /// </remarks>
        public LoadedOrderSelection(Exception ex, OrderEntity order, IEnumerable<ICarrierShipmentAdapter> shipmentAdapters, IEnumerable<KeyValuePair<long, ShippingAddressEditStateType>> destinationAddressEditable)
        {
            Order = order;
            ShipmentAdapters = shipmentAdapters.ToReadOnly();
            Exception = ex;
            this.destinationAddressEditable = destinationAddressEditable.ToImmutableDictionary();
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
        /// Id of the order selection
        /// </summary>
        public long OrderID => Order?.OrderID ?? -1;

        /// <summary>
        /// Is the destination address editable for the given shipment ID
        /// </summary>
        public ShippingAddressEditStateType IsDestinationAddressEditableFor(long shipmentID) =>
            destinationAddressEditable.GetValueOrDefault(shipmentID, ShippingAddressEditStateType.Editable);

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

            return new LoadedOrderSelection(Order, shipmentAdapters, destinationAddressEditable);
        }
    }
}
