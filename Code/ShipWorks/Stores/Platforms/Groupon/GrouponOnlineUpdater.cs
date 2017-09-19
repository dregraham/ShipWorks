using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Groupon.DTO;

namespace ShipWorks.Stores.Platforms.Groupon
{
    /// <summary>
    /// Uploads shipment details to Groupon
    /// </summary>
    [Component]
    public class GrouponOnlineUpdater : IGrouponOnlineUpdater
    {
        // Logger
        private readonly ILog log;
        private readonly IGrouponWebClient webClient;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IOrderManager orderManager;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public GrouponOnlineUpdater(IGrouponWebClient webClient, IOrderManager orderManager,
            IShippingManager shippingManager, ISqlAdapterFactory sqlAdapterFactory, Func<Type, ILog> createLogger)
        {
            this.shippingManager = shippingManager;
            this.orderManager = orderManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.webClient = webClient;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Push the shipment details to the store.
        /// </summary>
        public async Task UpdateShipmentDetails(IGrouponStoreEntity store, IEnumerable<long> orderKeys)
        {
            List<GrouponTracking> trackingList = new List<GrouponTracking>();

            var orders = await LoadOrders(orderKeys).ConfigureAwait(false);

            foreach (IOrderEntity order in orders)
            {
                ShipmentEntity shipment = await orderManager.GetLatestActiveShipmentAsync(order.OrderID).ConfigureAwait(false);

                // Check to see if shipment exists
                if (shipment == null)
                {
                    log.InfoFormat("Not uploading orderid {0} has no items.", order.OrderID);
                    continue;
                }

                trackingList.AddRange(await GetGrouponTracking(order, shipment).ConfigureAwait(false));
            }

            await PerformUpload(store, trackingList).ConfigureAwait(false);
        }

        /// <summary>
        /// Push the online status for an shipment.
        /// </summary>
        public async Task UpdateShipmentDetails(IGrouponStoreEntity store, IOrderEntity order, ShipmentEntity shipment)
        {
            List<GrouponTracking> trackingList = await GetGrouponTracking(order, shipment).ConfigureAwait(false);

            await PerformUpload(store, trackingList).ConfigureAwait(false);
        }

        /// <summary>
        /// Load the given orders
        /// </summary>
        private async Task<IEnumerable<IOrderEntity>> LoadOrders(IEnumerable<long> orderKeys)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                return await orderManager.LoadOrdersAsync(orderKeys, adapter).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the tracking info to send to groupon
        /// </summary>
        /// <remarks>
        /// We're not checking whether the order is manual because it's possible a real order was combined into a manual
        /// order. In that case, we'd still have legit items to upload. So just try to get GrouponOrderItems for the
        /// order because manually created orders will only have generic OrderItems.</remarks>
        private async Task<List<GrouponTracking>> GetGrouponTracking(IOrderEntity order, ShipmentEntity shipment)
        {
            var loadedShipment = await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
            var orderItems = await FetchOrderItems(order.OrderID).ConfigureAwait(false);

            return orderItems.Where(x => !string.IsNullOrEmpty(x.GrouponLineItemID))
                .Select(x => CreateGrouponTracking(x, loadedShipment))
                .ToList();
        }

        /// <summary>
        /// Create a groupon tracking object
        /// </summary>
        private GrouponTracking CreateGrouponTracking(IGrouponOrderItemEntity item, IShipmentEntity shipment)
        {
            string trackingNumber = shipment.TrackingNumber;
            string carrier = GrouponCarrier.GetCarrierCode(shipment);
            Int64 CILineItemID = Convert.ToInt64(item.GrouponLineItemID);

            return new GrouponTracking(carrier, CILineItemID, trackingNumber);
        }

        /// <summary>
        /// Fetch the order items for the given order
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        private async Task<IEnumerable<IGrouponOrderItemEntity>> FetchOrderItems(long orderID)
        {
            var query = new QueryFactory().GrouponOrderItem
                .Where(OrderItemFields.OrderID == orderID);

            // Fetch the order items
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                var items = await adapter.FetchQueryAsync(query).ConfigureAwait(false);
                return items.OfType<IGrouponOrderItemEntity>();
            }
        }

        /// <summary>
        /// Perform the upload
        /// </summary>
        private async Task PerformUpload(IGrouponStoreEntity store, List<GrouponTracking> trackingList)
        {
            if (trackingList.Any())
            {
                await webClient.UploadShipmentDetails(store, trackingList).ConfigureAwait(false);
            }
        }
    }
}