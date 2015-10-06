using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users.Security;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Configuration;
using ShipWorks.Shipping.Loading;

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

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoader(IShippingConfiguration shippingConfiguration, IShippingManager shippingManager, IFilterHelper filterHelper, IValidator<ShipmentEntity> addressValidator)
        {
            this.shippingConfiguration = shippingConfiguration;
            this.shippingManager = shippingManager;
            this.filterHelper = filterHelper;
            this.addressValidator = addressValidator;
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

                if (shipments?.Any() ?? false)
                {
                    addressValidator.ValidateAsync(shipments.FirstOrDefault());
                }

                OrderEntity order = shipments?.FirstOrDefault()?.Order;

                return new OrderSelectionLoaded(order, shipments);
            }
            catch (Exception ex)
            {
                return new OrderSelectionLoaded(ex);
            }
        }
    }
}
