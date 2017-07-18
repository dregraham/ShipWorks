using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.Groupon.Content
{
    /// <summary>
    /// Combination action that is specific to Groupon
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Groupon)]
    public class GrouponCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IGrouponOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(GrouponOrderSearchFields.OrderID,
                x => new GrouponOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    GrouponOrderID = x.GrouponOrderID
                });
        }
    }
}
