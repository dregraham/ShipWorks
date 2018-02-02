using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Yahoo.Content
{
    /// <summary>
    /// Split action that is specific to Yahoo
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Yahoo)]
    public class YahooSplitOrderAction : IStoreSpecificSplitOrderAction
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

            YahooOrderEntity order = (YahooOrderEntity) splitOrder;
            YahooOrderSearchEntity orderSearchEntity = order.YahooOrderSearch.AddNew();

            orderSearchEntity.YahooOrderID = order.YahooOrderID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}