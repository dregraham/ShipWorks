﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order management
    /// </summary>
    public class OrderManager : IOrderManager
    {
        /// <summary>
        /// Get a populated order from a given shipment
        /// </summary>
        public void PopulateOrderDetails(ShipmentEntity shipment)
        {
            OrderUtility.PopulateOrderDetails(shipment);
        }

        /// <summary>
        /// Calculates the order total.
        /// </summary>
        public decimal CalculateOrderTotal(OrderEntity order)
        {
            return OrderUtility.CalculateTotal(order);
        }

        /// <summary>
        /// Get a populated order from a order ID
        /// </summary>
        public OrderEntity FetchOrder(long orderID)
        {
            return OrderUtility.FetchOrder(orderID);
        }

        /// <summary>
        /// Get order with the data specified in the prefetch path loaded
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        public OrderEntity LoadOrder(long orderId, IPrefetchPath2 prefetchPath)
        {
            return LoadOrders(new[] { orderId }, prefetchPath).FirstOrDefault();
        }

        /// <summary>
        /// Load an order async
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        public Task<OrderEntity> LoadOrderAsync(long orderId, ISqlAdapter sqlAdapter)
        {
            EntityQuery<OrderEntity> query = new QueryFactory().Order.Where(OrderFields.OrderID == orderId);
            return sqlAdapter.FetchFirstAsync(query);
        }

        /// <summary>
        /// Load orders async
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        public async Task<IEnumerable<OrderEntity>> LoadOrdersAsync(IEnumerable<long> orderIDs, ISqlAdapter sqlAdapter)
        {
            EntityQuery<OrderEntity> query = new QueryFactory().Order.Where(OrderFields.OrderID.In(orderIDs));
            IEntityCollection2 entities = await sqlAdapter.FetchQueryAsync(query);
            return entities.OfType<OrderEntity>();
        }

        /// <summary>
        /// Load an order async
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        public Task<OrderEntity> LoadOrdersAsync(long orderId, ISqlAdapter sqlAdapter)
        {
            EntityQuery<OrderEntity> query = new QueryFactory().Order.Where(OrderFields.OrderID == orderId);
            return sqlAdapter.FetchFirstAsync(query);
        }

        /// <summary>
        /// Get orders with the data specified in the prefetch path loaded
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        public IEnumerable<OrderEntity> LoadOrders(IEnumerable<long> orderIdList, IPrefetchPath2 prefetchPath)
        {
            MethodConditions.EnsureArgumentIsNotNull(prefetchPath, nameof(prefetchPath));

            OrderCollection orders = new OrderCollection();

            using (ISqlAdapter sqlAdapter = SqlAdapter.Create(false))
            {
                sqlAdapter.FetchEntityCollection(orders,
                    new RelationPredicateBucket(new FieldCompareRangePredicate(OrderFields.OrderID, null, orderIdList.ToArray())),
                    prefetchPath);
            }

            return orders;
        }

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        public ShipmentEntity GetLatestActiveShipment(long orderID) => OrderUtility.GetLatestActiveShipment(orderID);
    }
}
