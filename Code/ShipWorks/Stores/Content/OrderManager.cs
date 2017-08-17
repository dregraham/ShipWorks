using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data;
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
        readonly ISqlAdapterFactory sqlAdapterFactory;
        readonly IDataProvider dataProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderManager(ISqlAdapterFactory sqlAdapterFactory, IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

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
        /// Get the first order in the specified predicate
        /// </summary>
        public async Task<OrderEntity> FetchFirstOrderAsync(IPredicate predicate, params IPrefetchPathElement2[] prefetchPaths)
        {
            QueryFactory factory = new QueryFactory();
            EntityQuery<OrderEntity> query = factory.Order.Where(predicate);

            foreach (IPrefetchPathElement2 path in prefetchPaths)
            {
                query = query.WithPath(path);
            }

            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                return await adapter.FetchFirstAsync(query).ConfigureAwait(false);
            }
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
            IEntityCollection2 entities = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
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

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        public Task<ShipmentEntity> GetLatestActiveShipmentAsync(long orderID) =>
            GetLatestActiveShipmentAsync(orderID, false);

        /// <summary>
        /// Returns the most recent, non-voided, processed shipment for the provided order
        /// </summary>
        public async Task<ShipmentEntity> GetLatestActiveShipmentAsync(long orderID, bool includeOrder)
        {
            var query = new QueryFactory().Shipment
                .Where(ShipmentFields.OrderID == orderID)
                .AndWhere(ShipmentFields.Processed == true)
                .AndWhere(ShipmentFields.Voided == false)
                .OrderBy(ShipmentFields.ProcessedDate.Descending());

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                var shipment = await sqlAdapter.FetchFirstAsync(query).ConfigureAwait(false);
                if (includeOrder)
                {
                    shipment.Order = await dataProvider.GetEntityAsync<OrderEntity>(orderID).ConfigureAwait(false);
                }
                return shipment;
            }
        }
    }
}
