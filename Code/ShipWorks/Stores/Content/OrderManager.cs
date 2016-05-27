using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;

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
        /// Get an order with the data specified in the prefetch path loaded
        /// </summary>
        public OrderEntity LoadOrder(long orderId, IPrefetchPath2 prefetchPath)
        {
            MethodConditions.EnsureArgumentIsNotNull(prefetchPath, nameof(prefetchPath));

            OrderEntity order = null; 

            EntityType entityType = EntityUtility.GetEntityType(orderId);
            IEntityField2 pkField = EntityUtility.GetPrimaryKeyField(entityType);

            using (SqlAdapter sqlAdapter = SqlAdapter.Create(true))
            {
                EntityBase2 entity = (EntityBase2)sqlAdapter.FetchNewEntity(
                    GeneralEntityFactory.Create(entityType).GetEntityFactory(),
                    new RelationPredicateBucket(new FieldCompareValuePredicate(pkField, null, ComparisonOperator.Equal, orderId)),
                     prefetchPath);
                order = entity as OrderEntity;
            }

            return order;
        }
    }
}
