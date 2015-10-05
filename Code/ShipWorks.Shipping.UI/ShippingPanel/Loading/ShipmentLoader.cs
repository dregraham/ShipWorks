using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users.Security;

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
            OrderSelectionLoaded orderSelectionLoaded = new OrderSelectionLoaded();
            ShipmentEntity shipment = null;

            filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            // Execute the work
            try
            {
                bool createIfNone = shippingPanelConfiguration.AutoCreateShipments && shippingPanelConfiguration.UserHasPermission(PermissionType.ShipmentsCreateEditProcess, orderID);

                List<ShipmentEntity> shipments = shippingManager.GetShipments(orderID, createIfNone);

                if (shipments.Count == 1)
                {
                    shipment = shipments.FirstOrDefault();

                    // Make sure the shipment type objects are fully loaded.
                    shippingManager.EnsureShipmentLoaded(shipment);

                    orderSelectionLoaded.Shipments = shipments;
                    orderSelectionLoaded.Result = ShippingPanelLoadedShipmentResult.Success;
                }
                else if (shipments.Count > 1)
                {
                    orderSelectionLoaded.Result = ShippingPanelLoadedShipmentResult.Multiple;
                }
                else
                {
                    orderSelectionLoaded.Result = ShippingPanelLoadedShipmentResult.NotCreated;
                }
            }
            catch (Exception ex)
            {
                orderSelectionLoaded.Result = ShippingPanelLoadedShipmentResult.Error;
                orderSelectionLoaded.Exception = ex;
            }

            return orderSelectionLoaded;
        }
    }
}
