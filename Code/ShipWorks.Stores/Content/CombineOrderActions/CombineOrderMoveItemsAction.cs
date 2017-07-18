using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.OrderCombinerActions
{
    /// <summary>
    /// Move items from a list of orders to the combined order
    /// </summary>
    public class CombineOrderMoveItemsAction : ICombineOrderAction
    {
        /// <summary>
        /// Perform the combination of items
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IRelationPredicateBucket itemsBucket = new RelationPredicateBucket(OrderItemFields.OrderID.In(orders.Select(x => x.OrderID)));
            return sqlAdapter.UpdateEntitiesDirectlyAsync(new OrderItemEntity { OrderID = combinedOrder.OrderID }, itemsBucket);
        }
    }
}