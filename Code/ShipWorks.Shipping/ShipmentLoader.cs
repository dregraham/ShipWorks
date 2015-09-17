using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Loads a shipment for an order.  
    /// </summary>
    public class ShipmentLoader : IShipmentLoader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipmentLoader));

        private IShippingPanelConfigurator shippingPanelConfigurator;
        private IShippingManager shippingManager;
        private IFilterHelper filterHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoader(IShippingPanelConfigurator shippingPanelConfigurator, IShippingManager shippingManager, IFilterHelper filterHelper)
        {
            this.shippingPanelConfigurator = shippingPanelConfigurator;
            this.shippingManager = shippingManager;
            this.filterHelper = filterHelper;
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        public ShippingPanelLoadedShipment Load(long orderID)
        {
            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = new ShippingPanelLoadedShipment();
            ShipmentEntity shipment = null;

            filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            // Execute the work
            try
            {
                bool createIfNone = shippingPanelConfigurator.AutoCreateShipments && shippingPanelConfigurator.UserHasPermission(PermissionType.ShipmentsCreateEditProcess, orderID);

                List<ShipmentEntity> shipments = shippingManager.GetShipments(orderID, createIfNone);

                if (shipments.Count == 1)
                {
                    shipment = shipments.FirstOrDefault();

                    // Make sure the shipment type objects are fully loaded.
                    shippingManager.EnsureShipmentLoaded(shipment);

                    shipmentPanelLoadedShipment.Shipment = shipment;
                    shipmentPanelLoadedShipment.Result = ShippingPanelLoadedShipmentResult.Success;
                }
                else if (shipments.Count > 1)
                {
                    shipmentPanelLoadedShipment.Result = ShippingPanelLoadedShipmentResult.Multiple;
                }
                else
                {
                    shipmentPanelLoadedShipment.Result = ShippingPanelLoadedShipmentResult.NotCreated;
                }
            }
            catch (Exception ex)
            {
                shipmentPanelLoadedShipment.Result = ShippingPanelLoadedShipmentResult.Error;
                shipmentPanelLoadedShipment.Exception = ex;
            }

            return shipmentPanelLoadedShipment;
        }
    }
}
