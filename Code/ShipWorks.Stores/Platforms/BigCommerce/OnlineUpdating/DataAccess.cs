using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Data access for the BigCommerce online updater
    /// </summary>
    [Component]
    public class DataAccess : IDataAccess
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IShippingManager shippingManager;
        private readonly IOrderManager orderManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataAccess(IShippingManager shippingManager, IOrderManager orderManager, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.orderManager = orderManager;
            this.shippingManager = shippingManager;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Get a unit of work
        /// </summary>
        public async Task Commit(IUnitOfWorkCore unitOfWork)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.CreateTransacted())
            {
                // Try to commit async, if possible
                UnitOfWork2 typedUnitOfWork = unitOfWork as UnitOfWork2;
                if (typedUnitOfWork == null)
                {
                    unitOfWork.Commit(adapter);
                }
                else
                {
                    await typedUnitOfWork.CommitAsync(adapter.AsDataAccessAdapter()).ConfigureAwait(false);
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Get the latest active shipment
        /// </summary>
        public Task<ShipmentEntity> GetLatestActiveShipmentAsync(long orderID) =>
            orderManager.GetLatestActiveShipmentAsync(orderID);

        /// <summary>
        /// Get order details for uploading
        /// </summary>
        public async Task<OnlineOrder> GetOrderDetailsAsync(long orderID)
        {
            QueryFactory factory = new QueryFactory();
            var query = factory.Order
                .Select(() => Tuple.Create(
                    new OnlineOrderDetails(
                        orderID,
                        OrderFields.IsManual.ToValue<bool>(),
                        OrderFields.OrderNumber.ToValue<long>(),
                        OrderFields.OrderNumberComplete.ToValue<string>()),
                    OrderFields.CombineSplitStatus.ToValue<CombineSplitStatusType>()))
                .Where(OrderFields.OrderID == orderID);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                var baseOrder = await sqlAdapter.FetchFirstAsync(query).ConfigureAwait(false);
                if (baseOrder == null)
                {
                    return null;
                }

                if (baseOrder.Item2 != CombineSplitStatusType.Combined)
                {
                    return new OnlineOrder(baseOrder.Item1);
                }

                var combinedOrders = await GetCombinedOrderDetailsAsync(orderID, sqlAdapter).ConfigureAwait(false);
                return new OnlineOrder(baseOrder.Item1, combinedOrders);
            }
        }

        /// <summary>
        /// Get order items
        /// </summary>
        public async Task<IDictionary<long, IEnumerable<IBigCommerceOrderItemEntity>>> GetOrderItemsAsync(long orderID)
        {
            QueryFactory factory = new QueryFactory();
            var query = factory.OrderItem
                .Where(OrderItemFields.OrderID == orderID);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                var orderItemCollection = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return orderItemCollection.OfType<IBigCommerceOrderItemEntity>()
                    .GroupBy(x => x.OriginalOrderID)
                    .ToDictionary(x => x.Key, x => x.OfType<IBigCommerceOrderItemEntity>());
            }
        }

        /// <summary>
        /// Get the overridden service used for a shipment
        /// </summary>
        public string GetOverriddenServiceUsed(ShipmentEntity shipment) =>
            shippingManager.GetOverriddenServiceUsed(shipment);

        /// <summary>
        /// Get a specified shipment
        /// </summary>
        public async Task<ShipmentEntity> GetShipmentAsync(long shipmentID)
        {
            var shipmentAdapter = await shippingManager.GetShipmentAsync(shipmentID).ConfigureAwait(false);
            return shipmentAdapter?.Shipment;
        }

        /// <summary>
        /// Commit a unit of work
        /// </summary>
        public IUnitOfWorkCore GetUnitOfWork() => new UnitOfWork2();

        /// <summary>
        /// Get combined order details
        /// </summary>
        private async Task<IEnumerable<OnlineOrderDetails>> GetCombinedOrderDetailsAsync(long orderID, ISqlAdapter sqlAdapter)
        {
            QueryFactory factory = new QueryFactory();
            var query = factory.OrderSearch
                .Select(() => new OnlineOrderDetails(
                        orderID,
                        OrderSearchFields.IsManual.ToValue<bool>(),
                        OrderSearchFields.OrderNumber.ToValue<long>(),
                        OrderSearchFields.OrderNumberComplete.ToValue<string>()))
                .Where(OrderSearchFields.OrderID == orderID);

            return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
        }
    }
}
