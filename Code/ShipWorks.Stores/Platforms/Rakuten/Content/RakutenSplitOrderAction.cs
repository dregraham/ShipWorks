using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Rakuten.Content
{
    /// <summary>
    /// Split action that is specific to Rakuten
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Rakuten)]
    public class RakutenSplitOrderAction : IStoreSpecificSplitOrderAction
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

            RakutenOrderEntity order = (RakutenOrderEntity) splitOrder;
            RakutenOrderSearchEntity orderSearchEntity = order.RakutenOrderSearch.AddNew();

            orderSearchEntity.OriginalOrderID = originalOrderID;
            orderSearchEntity.RakutenPackageID = order.RakutenPackageID;
        }
    }
}