using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Content
{
    /// <summary>
    /// Combination action that is specific to ChannelAdvisor
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificOrderCombinerAction), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorOrderCombinerAction : IStoreSpecificOrderCombinerAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IEnumerable<ChannelAdvisorOrderSearchEntity> orderSearches = orders.Cast<IChannelAdvisorOrderEntity>()
                .Select(x => new ChannelAdvisorOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    CustomOrderIdentifier = x.CustomOrderIdentifier
                });

            return sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }
    }
}
