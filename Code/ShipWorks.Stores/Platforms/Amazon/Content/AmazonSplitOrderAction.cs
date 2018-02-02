using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.Amazon.Content
{
    /// <summary>
    /// Split action that is specific to Amazon
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.Amazon)]
    public class AmazonSplitOrderAction : IStoreSpecificSplitOrderAction
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

            AmazonOrderEntity order = (AmazonOrderEntity) splitOrder;
            AmazonOrderSearchEntity orderSearchEntity = order.AmazonOrderSearch.AddNew();

            orderSearchEntity.AmazonOrderID = order.AmazonOrderID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}