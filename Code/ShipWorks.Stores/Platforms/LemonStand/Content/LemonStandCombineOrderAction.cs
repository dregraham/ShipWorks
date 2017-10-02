using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.LemonStand.Content
{
    /// <summary>
    /// Combination action that is specific to LemonStand
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.LemonStand)]
    public class LemonStandCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<ILemonStandOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(LemonStandOrderSearchFields.OrderID,
                x => new LemonStandOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    LemonStandOrderID = x.LemonStandOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}