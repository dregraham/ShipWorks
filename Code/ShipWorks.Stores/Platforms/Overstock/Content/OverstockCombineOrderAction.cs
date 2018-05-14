using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.Actions;

namespace ShipWorks.Stores.Platforms.Overstock.Content
{
    /// <summary>
    /// Combination action that is specific to Overstock
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Overstock)]
    public class OverstockCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IOverstockOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(OverstockOrderSearchFields.OrderID,
                x => new OverstockOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    SalesChannelOrderNumber = x.SalesChannelOrderNumber,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}