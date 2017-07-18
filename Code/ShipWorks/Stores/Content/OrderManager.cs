using System.Collections.Generic;
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
        readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderManager(ISqlAdapterFactory sqlAdapterFactory)
        {
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
        public OrderEntity LoadOrder(long orderId, IPrefetchPath2 prefetchPath)
        {
            return LoadOrders(new[] { orderId }, prefetchPath).FirstOrDefault();
        }

        /// <summary>
        /// Get orders with the data specified in the prefetch path loaded
        /// </summary>
        public IEnumerable<OrderEntity> LoadOrders(IEnumerable<long> orderIdList, IPrefetchPath2 prefetchPath)
        {
            MethodConditions.EnsureArgumentIsNotNull(prefetchPath, nameof(prefetchPath));

            OrderCollection orders = new OrderCollection();

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(false))
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
