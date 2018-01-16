using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.Content
{
    /// <summary>
    /// Split action that is specific to NetworkSolutions
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.NetworkSolutions)]
    public class NetworkSolutionsSplitOrderAction : IStoreSpecificSplitOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public void Perform(long originalOrderID, OrderEntity splitOrder)
        {
            NetworkSolutionsOrderEntity order = (NetworkSolutionsOrderEntity) splitOrder;
            NetworkSolutionsOrderSearchEntity orderSearchEntity = order.NetworkSolutionsOrderSearch.AddNew();

            orderSearchEntity.NetworkSolutionsOrderID = order.NetworkSolutionsOrderID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}