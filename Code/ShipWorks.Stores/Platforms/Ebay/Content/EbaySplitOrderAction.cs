using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Ebay.Content
{
    /// <summary>
    /// Split action that is specific to Ebay
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Ebay)]
    public class EbaySplitOrderAction : IStoreSpecificSplitOrderAction
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

            EbayOrderEntity order = (EbayOrderEntity) splitOrder;
            EbayOrderSearchEntity orderSearchEntity = order.EbayOrderSearch.AddNew();

            orderSearchEntity.EbayOrderID = order.EbayOrderID;
            orderSearchEntity.EbayBuyerID = order.EbayBuyerID;
            orderSearchEntity.SellingManagerRecord = order.SellingManagerRecord;

            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}