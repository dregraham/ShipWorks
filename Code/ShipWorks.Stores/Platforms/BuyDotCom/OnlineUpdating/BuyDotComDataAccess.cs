﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Extensions;
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
    public class BuyDotComDataAccess : IBuyDotComDataAccess
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShippingManager shippingManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComDataAccess(ISqlAdapterFactory sqlAdapterFactory, IShippingManager shippingManager, Func<Type, ILog> createLogger)
        {
            this.shippingManager = shippingManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Get shipment data needed for uploading
        /// </summary>
        public Task<IEnumerable<BuyDotComShipmentUpload>> GetShipmentDataByShipmentAsync(IEnumerable<long> shipmentKeys) =>
            GetShipmentDataAsync(ShipmentFields.ShipmentID.In(shipmentKeys));

        /// <summary>
        /// Get shipment data needed for uploading
        /// </summary>
        public Task<IEnumerable<BuyDotComShipmentUpload>> GetShipmentDataByOrderAsync(IEnumerable<long> orderKeys) =>
            GetShipmentDataAsync(ShipmentFields.OrderID.In(orderKeys));

        /// <summary>
        /// Get shipment data needed for uploading
        /// </summary>
        private async Task<IEnumerable<BuyDotComShipmentUpload>> GetShipmentDataAsync(IPredicate shipmentPredicate)
        {
            QueryFactory factory = new QueryFactory();

            IEnumerable<IShipmentEntity> shipments = await GetShipmentsAsync(shipmentPredicate, factory).ConfigureAwait(false);
            IEnumerable<IOrderEntity> orders = await GetOrdersAsync(shipments.Select(x => x.OrderID), factory).ConfigureAwait(false);
            var orderSearchRecordsByOrderID = await GetOrderSearchRecords(orders, factory);
            var itemsByOrder = await GetItemsAsync(orders, factory);

            return orders.LeftJoin(shipments, x => x.OrderID, y => y.OrderID)
                .Where(x => x.Item2 != null)
                .Select(x => CreateShipmentUpload(x.Item1, x.Item2, orderSearchRecordsByOrderID, itemsByOrder));
        }

        /// <summary>
        /// Create shipment upload instance
        /// </summary>
        private BuyDotComShipmentUpload CreateShipmentUpload(IOrderEntity order, IShipmentEntity shipment,
            IDictionary<long, IEnumerable<CombinedOrder>> orderSearchRecordsByOrderID,
            IDictionary<long, IEnumerable<IBuyDotComOrderItemEntity>> itemsByOrder)
        {
            var newOrders = orderSearchRecordsByOrderID.ContainsKey(order.OrderID) ?
                       orderSearchRecordsByOrderID[order.OrderID] :
                       new[] { new CombinedOrder(order.OrderNumberComplete, order.IsManual, order.OrderID, order.OrderID) };

            var shipmentOrders = newOrders.Distinct(x => x.OriginalOrderID).Select(x => CreateShipmentUploadOrders(x, itemsByOrder));

            return new BuyDotComShipmentUpload(shipment, shipmentOrders);
        }

        /// <summary>
        /// Create a shipment upload orders instance
        /// </summary>
        private BuyDotComShipmentUploadOrder CreateShipmentUploadOrders(CombinedOrder orderDetail, IDictionary<long, IEnumerable<IBuyDotComOrderItemEntity>> itemsByOrder)
        {
            var items = itemsByOrder.ContainsKey(orderDetail.OriginalOrderID) ?
                        itemsByOrder[orderDetail.OriginalOrderID].Where(x => x.OrderID == orderDetail.OrderID) :
                        Enumerable.Empty<IBuyDotComOrderItemEntity>();

            return new BuyDotComShipmentUploadOrder(orderDetail.OrderNumberComplete, orderDetail.IsManual, items);
        }

        /// <summary>
        /// Combined order pieces
        /// </summary>
        private async Task<IDictionary<long, IEnumerable<CombinedOrder>>> GetOrderSearchRecords(IEnumerable<IOrderEntity> orders, QueryFactory factory)
        {
            var combinedOrderKeys = orders.Where(x => x.CombineSplitStatus.IsEither()).Select(x => x.OrderID);
            if (combinedOrderKeys.None())
            {
                return new Dictionary<long, IEnumerable<CombinedOrder>>();
            }

            var query = factory.OrderSearch
                .Select(() => new CombinedOrder(
                    OrderSearchFields.OrderNumberComplete.ToValue<string>(),
                    OrderSearchFields.IsManual.ToValue<bool>(),
                    OrderSearchFields.OrderID.ToValue<long>(),
                    OrderSearchFields.OriginalOrderID.ToValue<long>()
                    ))
                .Where(OrderSearchFields.OrderID.In(combinedOrderKeys));

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                var orderPieces = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return orderPieces.GroupBy(x => x.OrderID)
                    .ToDictionary(x => x.Key, x => x.Select(y => y));
            }
        }

        /// <summary>
        /// Get shipments
        /// </summary>
        private async Task<IEnumerable<IShipmentEntity>> GetShipmentsAsync(IPredicate shipmentPredicate, QueryFactory factory)
        {
            var query = factory.Shipment.Where(shipmentPredicate);

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
            public CombinedOrder(string orderNumberComplete, bool isManual, long orderID, long originalOrderID)
            {
                OrderNumberComplete = orderNumberComplete;
                IsManual = isManual;
                OrderID = orderID;
                OriginalOrderID = originalOrderID;
            }

            /// <summary>
            /// Order number
            /// </summary>
            public string OrderNumberComplete { get; }

            /// <summary>
            /// Is the order manual
            /// </summary>
            public bool IsManual { get; }

            /// <summary>
            /// Order ID
            /// </summary>
            public long OrderID { get; }

            /// <summary>
            /// Original Order ID
            /// </summary>
            public long OriginalOrderID { get; }
        }
    }
}
