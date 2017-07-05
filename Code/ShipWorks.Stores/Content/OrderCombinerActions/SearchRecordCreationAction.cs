using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content.OrderCombinerActions
{
    /// <summary>
    /// Create search records for the orders
    /// </summary>
    public class SearchRecordCreationAction : IOrderCombinerAction
    {
        /// <summary>
        /// Perform the action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<OrderSearchEntity> orderSearches = orders.Select(x => new OrderSearchEntity
            {
                OrderID = combinedOrder.OrderID,
                StoreID = x.StoreID,
                OrderNumber = x.OrderNumber,
                OrderNumberComplete = x.OrderNumberComplete
            });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
