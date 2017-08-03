using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.CombineOrderActions
{
    /// <summary>
    /// Create search records for the orders
    /// </summary>
    public class SearchRecordCreationAction : ICombineOrderAction
    {
        /// <summary>
        /// Perform the action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, long survivingOrderID, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(OrderSearchFields.OrderID,
                x => new OrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    StoreID = x.StoreID,
                    OrderNumber = x.OrderNumber,
                    OrderNumberComplete = x.OrderNumberComplete,
                    IsManual = x.IsManual
                });
        }
    }
}
