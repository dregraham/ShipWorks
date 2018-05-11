using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Overstock.Content
{
    /// <summary>
    /// Split action that is specific to Overstock
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Overstock)]
    public class OverstockSplitOrderAction : IStoreSpecificSplitOrderAction
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

            OverstockOrderEntity order = (OverstockOrderEntity) splitOrder;
            OverstockOrderSearchEntity orderSearchEntity = order.OverstockOrderSearch.AddNew();

            orderSearchEntity.OverstockOrderID = order.OverstockOrderID;

            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}