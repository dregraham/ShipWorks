using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Content
{
    /// <summary>
    /// Split action that is specific to ChannelAdvisor
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorSplitOrderAction : IStoreSpecificSplitOrderAction
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

            ChannelAdvisorOrderEntity order = (ChannelAdvisorOrderEntity) splitOrder;
            ChannelAdvisorOrderSearchEntity orderSearchEntity = order.ChannelAdvisorOrderSearch.AddNew();

            orderSearchEntity.CustomOrderIdentifier = order.CustomOrderIdentifier;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}