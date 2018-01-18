using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Content
{
    /// <summary>
    /// Split action that is specific to ThreeDCart
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.ThreeDCart)]
    public class ThreeDCartSplitOrderAction : IStoreSpecificSplitOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public void Perform(long originalOrderID, OrderEntity splitOrder)
        {
            if (splitOrder.IsManual)
            {
                return;
            }

            ThreeDCartOrderEntity order = (ThreeDCartOrderEntity) splitOrder;
            ThreeDCartOrderSearchEntity orderSearchEntity = order.ThreeDCartOrderSearch.AddNew();

            orderSearchEntity.ThreeDCartOrderID = order.ThreeDCartOrderID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}