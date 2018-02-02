using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.LemonStand.Content
{
    /// <summary>
    /// Split action that is specific to ThreeDCart
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.LemonStand)]
    public class LemonStandSplitOrderAction : IStoreSpecificSplitOrderAction
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

            LemonStandOrderEntity order = (LemonStandOrderEntity) splitOrder;
            LemonStandOrderSearchEntity orderSearchEntity = order.LemonStandOrderSearch.AddNew();

            orderSearchEntity.LemonStandOrderID = order.LemonStandOrderID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}