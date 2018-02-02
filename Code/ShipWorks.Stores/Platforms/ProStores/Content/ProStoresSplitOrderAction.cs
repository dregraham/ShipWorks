using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.ProStores.Content
{
    /// <summary>
    /// Split action that is specific to ProStores
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.ProStores)]
    public class ProStoresSplitOrderAction : IStoreSpecificSplitOrderAction
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

            ProStoresOrderEntity order = (ProStoresOrderEntity) splitOrder;
            ProStoresOrderSearchEntity orderSearchEntity = order.ProStoresOrderSearch.AddNew();

            orderSearchEntity.ConfirmationNumber = order.ConfirmationNumber;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}