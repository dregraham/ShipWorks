using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using System.Reflection;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Loads a shipment for an order.  
    /// </summary>
    public class ShipmentLoader : ILoader<ShippingPanelLoadedShipment, OrderEntity>
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
        /// Load the shipment asychronously.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<ShippingPanelLoadedShipment> LoadAsync(OrderEntity order)
        {
            if (order == null)
            {
                throw new ArgumentNullException("order");
            }

            ShippingPanelLoadedShipment shipmentPanelLoadedShipment = LoadShipments(order.OrderID);

            return shipmentPanelLoadedShipment;
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        private ShippingPanelLoadedShipment LoadShipments(long orderID)
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
