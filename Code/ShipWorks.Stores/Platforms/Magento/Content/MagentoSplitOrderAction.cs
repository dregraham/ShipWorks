using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Magento.Content
{
    /// <summary>
    /// Split action that is specific to Magento
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Magento)]
    public class MagentoSplitOrderAction : IStoreSpecificSplitOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public void Perform(long originalOrderID, OrderEntity splitOrder)
        {
            MagentoOrderEntity order = (MagentoOrderEntity) splitOrder;
            MagentoOrderSearchEntity orderSearchEntity = order.MagentoOrderSearch.AddNew();

            orderSearchEntity.MagentoOrderID = order.MagentoOrderID;

            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}