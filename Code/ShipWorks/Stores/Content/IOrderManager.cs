﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order management
    /// </summary>
    public interface IOrderManager
    {
        /// <summary>
        /// Get a populated order from a given shipment
        /// </summary>
        void PopulateOrderDetails(ShipmentEntity shipment);

        /// <summary>
        /// Get a populated order from a given order
        /// </summary>
        void PopulateOrderDetails(OrderEntity order);

        /// <summary>
        /// Calculates the order total.
        /// </summary>
        decimal CalculateOrderTotal(OrderEntity order);

        /// <summary>
        /// Get a populated order from a order ID
        /// </summary>
        OrderEntity FetchOrder(long orderID);

        /// <summary>
        /// Load an order async
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        Task<OrderEntity> LoadOrderAsync(long orderId, ISqlAdapter sqlAdapter);

        /// <summary>
        /// Load an order async using given prefetch path
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        Task<OrderEntity> LoadOrderAsync(long orderId, ISqlAdapter sqlAdapter, IEnumerable<IPrefetchPathElement2> prefetchPaths);

        /// <summary>
        /// Load orders async
        /// </summary>
        /// <remarks>
        /// This method bypasses the entity cache
        /// </remarks>
        Task<IEnumerable<OrderEntity>> LoadOrdersAsync(IEnumerable<long> orderIDs, ISqlAdapter sqlAdapter);

        /// <summary>
        /// Load the specified order using the given prefetch path
        /// </summary>
        OrderEntity LoadOrder(long orderID, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        ShipmentEntity GetLatestActiveShipment(long orderID, bool includeReturns);

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        Task<ShipmentEntity> GetLatestActiveShipmentAsync(long orderID, bool includeReturns);

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        Task<ShipmentEntity> GetLatestActiveShipmentAsync(long orderID, bool includeOrder, bool includeReturns);

        /// <summary>
        /// Load the specified orders using the given prefetch path
        /// </summary>
        IEnumerable<OrderEntity> LoadOrders(IEnumerable<long> orderIdList, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Get items from an order
        /// </summary>
        IEnumerable<IOrderItemEntity> GetItems(IOrderEntity order);
    }
}
