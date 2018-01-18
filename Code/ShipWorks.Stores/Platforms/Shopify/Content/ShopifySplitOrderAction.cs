using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Shopify.Content
{
    /// <summary>
    /// Split action that is specific to Shopify
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Shopify)]
    public class ShopifySplitOrderAction : IStoreSpecificSplitOrderAction
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

            ShopifyOrderEntity order = (ShopifyOrderEntity) splitOrder;
            ShopifyOrderSearchEntity orderSearchEntity = order.ShopifyOrderSearch.AddNew();

            orderSearchEntity.ShopifyOrderID = order.ShopifyOrderID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}