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
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Stores.Platforms.Amazon.OnlineUpdating
{
    /// <summary>
    /// Uploads shipment details to Amazon
    /// </summary>
    [Component]
    public class AmazonOnlineUpdater : IAmazonOnlineUpdater
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(AmazonOnlineUpdater));
        private readonly IOrderManager orderManager;
        private readonly IShippingManager shippingManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly Func<AmazonStoreEntity, IAmazonMwsClient> createMwsClient;
        private readonly IAmazonOrderSearchProvider orderSearchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonOnlineUpdater(IOrderManager orderManager, IShippingManager shippingManager,
            ISqlAdapterFactory sqlAdapterFactory, IAmazonOrderSearchProvider orderSearchProvider,
            Func<AmazonStoreEntity, IAmazonMwsClient> createMwsClient)
        {
            this.orderSearchProvider = orderSearchProvider;
            this.createMwsClient = createMwsClient;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Update the online status of the given order
        /// </summary>
        public async Task UploadOrderShipmentDetails(AmazonStoreEntity store, IEnumerable<long> orderKeys)
        {
            List<ShipmentEntity> shipments = new List<ShipmentEntity>();

            var orders = await LoadOrders(orderKeys).ConfigureAwait(false);

            foreach (var order in orders)
            {
                // upload tracking number for the most recent processed, not voided shipment
                ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(order.OrderID).ConfigureAwait(false);
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

            await UploadShipmentDetails(store, shipments).ConfigureAwait(false);
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
        public async Task UploadShipmentDetails(AmazonStoreEntity store, IEnumerable<long> shipmentKeys)
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

            await UploadShipmentDetails(store, shipments).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads shipment details for a particular shipment
        /// </summary>
        public async Task UploadShipmentDetails(AmazonStoreEntity store, List<ShipmentEntity> shipments)
        {
            // upload the feed using the MWS API
            using (IAmazonMwsClient client = createMwsClient(store))
            {
                var uploadDetails = await CreateUploadDetails(shipments).ConfigureAwait(false);
                await client.UploadShipmentDetails(uploadDetails).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create upload details for the list of shipments
        /// </summary>
        private async Task<List<AmazonOrderUploadDetail>> CreateUploadDetails(List<ShipmentEntity> shipments)
        {
            List<AmazonOrderUploadDetail> details = new List<AmazonOrderUploadDetail>();

            foreach (var shipment in shipments)
            {
                var orderDetails = await CreateUploadDetailForShipment(shipment, shipment.Order as AmazonOrderEntity).ConfigureAwait(false);
                details.AddRange(orderDetails.Where(x => x != null));
            }

            return details;
        }

        /// <summary>
        /// Create upload details for a given shipment
        /// </summary>
        private async Task<IEnumerable<AmazonOrderUploadDetail>> CreateUploadDetailForShipment(ShipmentEntity shipment, AmazonOrderEntity order)
        {
            if (order == null)
            {
                return Enumerable.Empty<AmazonOrderUploadDetail>();
            }

            var identifiers = await orderSearchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);
            return identifiers.Select(x => new AmazonOrderUploadDetail(shipment, x));
        }
    }
}