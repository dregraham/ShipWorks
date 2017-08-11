using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading Buy.com shipment data
    /// </summary>
    [Component]
    public class DataAccess : IDataAccess
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShippingManager shippingManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataAccess(ISqlAdapterFactory sqlAdapterFactory, IShippingManager shippingManager, Func<Type, ILog> createLogger)
        {
            this.shippingManager = shippingManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get shipment data needed for uploading
        /// </summary>
        public async Task<IEnumerable<ShipmentUpload>> GetShipmentDataAsync(IEnumerable<long> orderKeys)
        {
            QueryFactory factory = new QueryFactory();

            IEnumerable<IShipmentEntity> shipments = await GetShipmentsAsync(orderKeys, factory).ConfigureAwait(false);
            IEnumerable<IOrderEntity> orders = await GetOrdersAsync(orderKeys, factory).ConfigureAwait(false);
            var ordersByOriginalOrderID = await GetCombinedOrdersAsync(orders, factory);
            var itemsByOrder = await GetItemsAsync(orders, factory);

            return orders.LeftJoin(shipments, x => x.OrderID, y => y.OrderID)
                .Where(x => x.Item2 != null)
                .Select(x => CreateShipmentUpload(x.Item1, x.Item2, ordersByOriginalOrderID, itemsByOrder));
        }

        /// <summary>
        /// Create shipment upload instance
        /// </summary>
        private ShipmentUpload CreateShipmentUpload(IOrderEntity order, IShipmentEntity shipment,
            IDictionary<long, IEnumerable<CombinedOrder>> ordersByOriginalOrderID,
            IDictionary<long, IEnumerable<IBuyDotComOrderItemEntity>> itemsByOrder)
        {
            var newOrders = ordersByOriginalOrderID.ContainsKey(order.OrderID) ?
                       ordersByOriginalOrderID[order.OrderID] :
                       new[] { new CombinedOrder(order.OrderNumber, order.IsManual, order.OrderID) };

            var shipmentOrders = newOrders.Select(x => CreateShipmentUploadOrders(x, itemsByOrder));

            return new ShipmentUpload(shipment, shipmentOrders);
        }

        /// <summary>
        /// Create a shipment upload orders instance
        /// </summary>
        private ShipmentUploadOrder CreateShipmentUploadOrders(CombinedOrder orderDetail, IDictionary<long, IEnumerable<IBuyDotComOrderItemEntity>> itemsByOrder)
        {
            var items = itemsByOrder.ContainsKey(orderDetail.OrderID) ?
                        itemsByOrder[orderDetail.OrderID] :
                        Enumerable.Empty<IBuyDotComOrderItemEntity>();

            return new ShipmentUploadOrder(orderDetail.OrderNumber, orderDetail.IsManual, items);
        }

        /// <summary>
        /// Combined order pieces
        /// </summary>
        private async Task<IDictionary<long, IEnumerable<CombinedOrder>>> GetCombinedOrdersAsync(IEnumerable<IOrderEntity> orders, QueryFactory factory)
        {
            var combinedOrderKeys = orders.Where(x => x.CombineSplitStatus == CombineSplitStatusType.Combined).Select(x => x.OrderID);
            if (combinedOrderKeys.None())
            {
                return new Dictionary<long, IEnumerable<CombinedOrder>>();
            }

            var query = factory.OrderSearch
                .Select(() => Tuple.Create(
                    OrderSearchFields.OrderID.ToValue<long>(),
                    OrderSearchFields.OrderNumber.ToValue<long>(),
                    OrderSearchFields.IsManual.ToValue<bool>(),
                    OrderSearchFields.OriginalOrderID.ToValue<long>()
                    ))
                .Where(OrderSearchFields.OrderID.In(combinedOrderKeys));

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                var orderPieces = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return orderPieces.GroupBy(x => x.Item1)
                    .ToDictionary(x => x.Key, x => x.Select(y => new CombinedOrder(y.Item2, y.Item3, y.Item4)));
            }
        }

        /// <summary>
        /// Get shipments
        /// </summary>
        private async Task<IEnumerable<IShipmentEntity>> GetShipmentsAsync(IEnumerable<long> orderKeys, QueryFactory factory)
        {
            var query = factory.Shipment.Where(ShipmentFields.OrderID.In(orderKeys));

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                var shipments = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return await EnsureShipmentsAreLoaded(shipments).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Ensure the shipments are loaded
        /// </summary>
        private async Task<IEnumerable<IShipmentEntity>> EnsureShipmentsAreLoaded(IEntityCollection2 shipments)
        {
            List<ShipmentEntity> loadedShipments = new List<ShipmentEntity>();

            foreach (var shipment in shipments.OfType<ShipmentEntity>())
            {
                try
                {
                    var loadedShipment = await shippingManager.EnsureShipmentLoadedAsync(shipment).ConfigureAwait(false);
                    loadedShipments.Add(loadedShipment);
                }
                catch (ObjectDeletedException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    continue;
                }
                catch (SqlForeignKeyException)
                {
                    log.InfoFormat("Not uploading tracking number since shipment {0} or related information has been deleted.", shipment.ShipmentID);
                    continue;
                }
            }

            return loadedShipments;
        }

        /// <summary>
        /// Get orders
        /// </summary>
        private async Task<IEnumerable<IOrderEntity>> GetOrdersAsync(IEnumerable<long> orderKeys, QueryFactory factory)
        {
            var query = factory.Order.Where(OrderFields.OrderID.In(orderKeys));

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                var orders = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return orders.OfType<IOrderEntity>();
            }
        }

        /// <summary>
        /// Get items for the given orders
        /// </summary>
        private async Task<IDictionary<long, IEnumerable<IBuyDotComOrderItemEntity>>> GetItemsAsync(IEnumerable<IOrderEntity> orders, QueryFactory factory)
        {
            var orderKeys = orders.Select(x => x.OrderID);
            var query = factory.BuyDotComOrderItem.Where(OrderItemFields.OrderID.In(orderKeys));

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                var items = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return items.OfType<IBuyDotComOrderItemEntity>()
                    .GroupBy(x => x.OriginalOrderID)
                    .ToDictionary(x => x.Key, x => x.OfType<IBuyDotComOrderItemEntity>());
            }
        }

        /// <summary>
        /// Combined order
        /// </summary>
        private class CombinedOrder
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public CombinedOrder(long orderNumber, bool isManual, long orderID)
            {
                OrderNumber = orderNumber;
                IsManual = isManual;
                OrderID = orderID;
            }

            /// <summary>
            /// Order number
            /// </summary>
            public long OrderNumber { get; }

            /// <summary>
            /// Is the order manual
            /// </summary>
            public bool IsManual { get; }

            /// <summary>
            /// Order ID
            /// </summary>
            public long OrderID { get; }
        }
    }
}
