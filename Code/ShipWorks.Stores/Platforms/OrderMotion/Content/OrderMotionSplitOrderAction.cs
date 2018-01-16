using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.OrderMotion.Content
{
    /// <summary>
    /// Split action that is specific to OrderMotion
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.OrderMotion)]
    public class OrderMotionOrderAction : IStoreSpecificSplitOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public void Perform(long originalOrderID, OrderEntity splitOrder)
        {
            OrderMotionOrderEntity order = (OrderMotionOrderEntity) splitOrder;
            OrderMotionOrderSearchEntity orderSearchEntity = order.OrderMotionOrderSearch.AddNew();

            orderSearchEntity.OrderID = order.OrderID;
            orderSearchEntity.OrderMotionShipmentID = order.OrderMotionShipmentID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}