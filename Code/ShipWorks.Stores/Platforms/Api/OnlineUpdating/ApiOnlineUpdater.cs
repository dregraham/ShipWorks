using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Warehouse.Orders;

namespace ShipWorks.Stores.Platforms.Api.OnlineUpdating
{
    /// <summary>
    /// Uploads shipment details to Api
    /// </summary>
    [Component]
    public class ApiOnlineUpdater : IApiOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ApiOnlineUpdater));
        private readonly IOrderManager orderManager;
        private readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly Func<IWarehouseOrderClient> createWarehouseOrderClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, Func<IWarehouseOrderClient> warehouseOrderClientCreator)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.createWarehouseOrderClient = createWarehouseOrderClient;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        public async Task UploadOrderShipmentDetails(IEnumerable<long> orderKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            var orders = await LoadOrders(orderKeys).ConfigureAwait(false);

            foreach (var order in orders)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(order.OrderID, false).ConfigureAwait(false);
                if (shipment == null)
                {
                    log.InfoFormat("There were no Processed and not Voided shipments to upload for OrderID {0}.", order.OrderID);
                }
                else
                {
                    shipment.Order = order;
                    shipments.Add(shipment);
                }
            }

            await UploadShipmentDetails(shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Load the orders for the given keys
        /// </summary>
        private async Task<IEnumerable<OrderEntity>> LoadOrders(IEnumerable<long> orderKeys)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await orderManager.LoadOrdersAsync(orderKeys, sqlAdapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(IEnumerable<long> shipmentKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            foreach (long shipmentID in shipmentKeys)
            {
                var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
                ShipmentEntity shipment = shipmentAdapter?.Shipment;

                if (shipment == null)
                {
                    log.InfoFormat("Not uploading shipment details, since the shipment {0} was deleted.", shipmentID);
                }
                else
                {
                    shipments.Add(shipment);
                }
            }

            await UploadShipmentDetails(shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(List<ShipmentEntity> shipments)
        {
            // upload the feed using the MWS API
            var client = createWarehouseOrderClient();

            foreach (var shipment in shipments)
            {
                var result = await client.NotifyShipped(shipment.Order.ChannelOrderID, shipment.TrackingNumber, ShippingManager.GetCarrierName(shipment.ShipmentTypeCode)).ConfigureAwait(false);
                result.OnFailure(ex => throw new ApiStoreException("Error uploading shipment details.", ex));
            }            
        }
    }
}