using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.Walmart.Content
{
    /// <summary>
    /// Combination action that is specific to Walmart
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Walmart)]
    public class WalmartCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IWalmartOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(WalmartOrderSearchFields.OrderID,
                x => new WalmartOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    PurchaseOrderID = x.PurchaseOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}