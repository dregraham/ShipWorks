using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.ClickCartPro.Content
{
    /// <summary>
    /// Split action that is specific to ClickCartPro
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.ClickCartPro)]
    public class ClickCartProSplitOrderAction : IStoreSpecificSplitOrderAction
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

            ClickCartProOrderEntity order = (ClickCartProOrderEntity) splitOrder;
            ClickCartProOrderSearchEntity orderSearchEntity = order.ClickCartProOrderSearch.AddNew();

            orderSearchEntity.ClickCartProOrderID = order.ClickCartProOrderID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}