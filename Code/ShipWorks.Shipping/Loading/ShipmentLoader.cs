using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users.Security;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Loads a shipment for an order.  
    /// </summary>
    public class ShipmentLoader : IShipmentLoader
    {
        private readonly IShippingConfiguration shippingConfiguration;
        private readonly IShippingManager shippingManager;
        private readonly IFilterHelper filterHelper;
        private readonly IValidator<ShipmentEntity> addressValidator;
        private readonly IStoreManager storeManager;
        private readonly IStoreTypeManager storeTypeManager;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoader(IShippingConfiguration shippingConfiguration, IShippingManager shippingManager, IFilterHelper filterHelper, 
                              IValidator<ShipmentEntity> addressValidator, IStoreManager storeManager, IStoreTypeManager storeTypeManager,
                              ICarrierShipmentAdapterFactory shipmentAdapterFactory, IOrderManager orderManager)
        {
            this.shippingConfiguration = shippingConfiguration;
            this.shippingManager = shippingManager;
            this.filterHelper = filterHelper;
            this.addressValidator = addressValidator;
            this.storeManager = storeManager;
            this.storeTypeManager = storeTypeManager;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        public OrderSelectionLoaded Load(long orderID)
        {
            filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            // Execute the work
            try
            {
                bool createIfNone = shippingConfiguration.AutoCreateShipments && shippingConfiguration.UserHasPermission(PermissionType.ShipmentsCreateEditProcess, orderID);

                IEnumerable<ICarrierShipmentAdapter> shipments = shippingManager.GetShipments(orderID, createIfNone);
                ICarrierShipmentAdapter firstShipment = shipments.FirstOrDefault();

                if (firstShipment?.Shipment != null && shippingConfiguration.GetAddressValidation(firstShipment.Shipment))
                {
                    addressValidator.ValidateAsync(firstShipment.Shipment);
                }

                ShippingAddressEditStateType destinationAddressEditable = ShippingAddressEditStateType.Editable;
                OrderEntity order = firstShipment?.Shipment?.Order;

                if (order == null)
                {
                    order = orderManager.FetchOrder(orderID);
                }

                if (order != null && firstShipment != null)
                {
                    order.Store = storeManager.GetStore(order.StoreID);

                    destinationAddressEditable = storeTypeManager.GetType(order.Store).ShippingAddressEditableState(firstShipment?.Shipment);
                }
                
                return new OrderSelectionLoaded(order, shipments, destinationAddressEditable);
            }
            catch (Exception ex)
            {
                return new OrderSelectionLoaded(ex);
            }
        }
    }
}
