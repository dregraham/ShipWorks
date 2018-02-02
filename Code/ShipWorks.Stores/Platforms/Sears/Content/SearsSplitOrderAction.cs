using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Sears.Content
{
    /// <summary>
    /// Split action that is specific to Sears
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Sears)]
    public class SearsSplitOrderAction : IStoreSpecificSplitOrderAction
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

            SearsOrderEntity order = (SearsOrderEntity) splitOrder;
            SearsOrderSearchEntity orderSearchEntity = order.SearsOrderSearch.AddNew();

            orderSearchEntity.PoNumber = order.PoNumber;

            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}