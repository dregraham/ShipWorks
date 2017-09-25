using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.OrderMotion.Content
{
    /// <summary>
    /// Combination action that is specific to OrderMotion
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.OrderMotion)]
    public class OrderMotionCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IOrderMotionOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(OrderMotionOrderSearchFields.OrderID,
                x => new OrderMotionOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    OrderMotionShipmentID = x.OrderMotionShipmentID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}