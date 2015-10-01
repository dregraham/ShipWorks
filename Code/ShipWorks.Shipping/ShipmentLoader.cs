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

        private readonly IShippingPanelConfiguration shippingPanelConfiguration;
        private readonly IShippingManager shippingManager;
        private readonly IFilterHelper filterHelper;
        private readonly IShipmentTypeFactory shipmentTypeFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoader(IShippingPanelConfiguration shippingPanelConfiguration, IShippingManager shippingManager, IFilterHelper filterHelper, IShipmentTypeFactory shipmentTypeFactory)
        {
            this.shippingPanelConfiguration = shippingPanelConfiguration;
            this.shippingManager = shippingManager;
            this.filterHelper = filterHelper;
            this.shipmentTypeFactory = shipmentTypeFactory;
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        public ShippingPanelLoadedShipment Load(long orderID)
        {
            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = new ShippingPanelLoadedShipment();
            shipmentPanelLoadedShipment.OrderID = orderID;

            filterHelper.EnsureFiltersUpToDate(TimeSpan.FromSeconds(15));

            // Execute the work
            try
            {
                bool createIfNone = shippingPanelConfiguration.AutoCreateShipments && shippingPanelConfiguration.UserHasPermission(PermissionType.ShipmentsCreateEditProcess, orderID);

                List<ShipmentEntity> shipments = shippingManager.GetShipments(orderID, createIfNone);

                if (shipments.Count == 1)
                {
                    ShipmentEntity shipment = shipments.FirstOrDefault();

                    // Make sure the shipment type objects are fully loaded.
                    shippingManager.EnsureShipmentLoaded(shipment);

                    shipmentPanelLoadedShipment.RequestedShippingMode = shipment.Order.RequestedShipping;
                    shipmentPanelLoadedShipment.Shipment = shipment;
                    shipmentPanelLoadedShipment.Result = ShippingPanelLoadedShipmentResult.Success;
                    shipmentPanelLoadedShipment.ShipmentAdapter = shipmentTypeFactory.Get(shipment).GetShipmentAdapter(shipment);
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
