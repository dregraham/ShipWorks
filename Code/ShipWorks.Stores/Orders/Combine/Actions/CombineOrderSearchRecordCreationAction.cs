using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Orders.Combine.Actions
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
            var recordCreator = new SearchRecordMerger<IOrderEntity>(combinedOrder, orders, sqlAdapter, includeManualOrders: true);

            return recordCreator.Perform(OrderSearchFields.OrderID,
                x => new OrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    StoreID = x.StoreID,
                    OrderNumber = x.OrderNumber,
                    OrderNumberComplete = GetOriginalOrderNumberComplete(x, sqlAdapter),
                    IsManual = x.IsManual,
                    OriginalOrderID = x.OrderID
                });
        }

        /// <summary>
        /// Get appropriate order number complete.  
        /// For example, split an order, then combine it, the order's OrderNumberComplete could have the postfix
        /// that was added during split which is not valid for uploading.  We need to go to the order's search entries
        /// to get the original order number.
        /// </summary>
        private string GetOriginalOrderNumberComplete(IOrderEntity order, ISqlAdapter sqlAdapter)
        {
            string originalOrderNumberComplete = order.OrderNumberComplete;

            if (order.CombineSplitStatus.IsSplit())
            {
                IOrderSearchEntity orderSearchEntity = null;
                if (order.OrderSearch.None())
                {
                    OrderSearchCollection orderSearchCollection = new OrderSearchCollection();
                    sqlAdapter.FetchEntityCollection(orderSearchCollection,
                        new RelationPredicateBucket(OrderSearchFields.OrderID == order.OrderID));

                    orderSearchEntity = orderSearchCollection.FirstOrDefault();
                }
                else
                {
                    orderSearchEntity = order.OrderSearch.FirstOrDefault();
                }

                if (orderSearchEntity != null)
                {
                    originalOrderNumberComplete = orderSearchEntity.OrderNumberComplete;
                }
            }

            return originalOrderNumberComplete;
        }
    }
}
