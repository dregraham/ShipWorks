using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.Actions;

namespace ShipWorks.Stores.Platforms.Rakuten.Content
{
    /// <summary>
    /// Combination action that is specific to Rakuten
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Rakuten)]
    public class RakutenCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IRakutenOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(RakutenOrderSearchFields.OrderID,
                x => new RakutenOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    RakutenOrderID = x.RakutenOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}
