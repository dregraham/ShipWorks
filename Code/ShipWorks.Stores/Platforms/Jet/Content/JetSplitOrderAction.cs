using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Jet.Content
{
    /// <summary>
    /// Split action that is specific to Jet
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Jet)]
    public class JetSplitOrderAction : IStoreSpecificSplitOrderAction
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

            JetOrderEntity order = (JetOrderEntity) splitOrder;
            JetOrderSearchEntity orderSearchEntity = order.JetOrderSearch.AddNew();

            orderSearchEntity.MerchantOrderID = order.MerchantOrderId;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}