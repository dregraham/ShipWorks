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

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoader(IShippingConfiguration shippingConfiguration, IShippingManager shippingManager, IFilterHelper filterHelper, 
                              IValidator<ShipmentEntity> addressValidator, IStoreManager storeManager, IStoreTypeManager storeTypeManager,
                              ICarrierShipmentAdapterFactory shipmentAdapterFactory)
        {
            this.shippingConfiguration = shippingConfiguration;
            this.shippingManager = shippingManager;
            this.filterHelper = filterHelper;
            this.addressValidator = addressValidator;
            this.storeManager = storeManager;
            this.storeTypeManager = storeTypeManager;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
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

                List<ShipmentEntity> shipments = shippingManager.GetShipments(orderID, createIfNone);
                ShipmentEntity firstShipment = shipments?.FirstOrDefault();

                if (firstShipment != null && shippingConfiguration.GetAddressValidation(firstShipment))
                {
                    addressValidator.ValidateAsync(firstShipment);
                }

                ShippingAddressEditStateType destinationAddressEditable = ShippingAddressEditStateType.Editable;
                OrderEntity order = firstShipment?.Order;

                if (order != null)
                {
                    order.Store = storeManager.GetStore(order.StoreID);

                    destinationAddressEditable = storeTypeManager.GetType(order.Store).ShippingAddressEditableState(firstShipment);
                }

                List<ICarrierShipmentAdapter> shipmentAdapters = new List<ICarrierShipmentAdapter>();
                shipments?.ForEach(s => shipmentAdapters.Add(shipmentAdapterFactory.Get(s)));

                return new OrderSelectionLoaded(order, shipmentAdapters, destinationAddressEditable);
            }
            catch (Exception ex)
            {
                return new OrderSelectionLoaded(ex);
            }
        }
    }
}
