using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users.Security;
using ShipWorks.Core.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.UI.ShippingPanel.Loading
{
    /// <summary>
    /// Loads a shipment for an order.  
    /// </summary>
    public class ShipmentLoader : IShipmentLoader
    {
        private readonly IShippingPanelConfiguration shippingPanelConfiguration;
        private readonly IShippingManager shippingManager;
        private readonly IFilterHelper filterHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoader(IShippingPanelConfiguration shippingPanelConfiguration, IShippingManager shippingManager, IFilterHelper filterHelper)
        {
            this.shippingPanelConfiguration = shippingPanelConfiguration;
            this.shippingManager = shippingManager;
            this.filterHelper = filterHelper;
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
                bool createIfNone = shippingPanelConfiguration.AutoCreateShipments && shippingPanelConfiguration.UserHasPermission(PermissionType.ShipmentsCreateEditProcess, orderID);

                List<ShipmentEntity> shipments = shippingManager.GetShipments(orderID, createIfNone);

                //TODO: Add the loaded order to the selection
                return new OrderSelectionLoaded(null, shipments);
            }
            catch (Exception ex)
            {
                return new OrderSelectionLoaded(ex);
            }
        }
    }
}
