using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;
using ShipWorks.Stores.Platforms.ChannelAdvisor.Enums;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.Content
{
    /// <summary>
    /// Combination action that is specific to ChannelAdvisor
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.ChannelAdvisor)]
    public class ChannelAdvisorCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            ChannelAdvisorOrderEntity order = (ChannelAdvisorOrderEntity) combinedOrder;

            order.IsPrime =
                orders.Where(o => o is ChannelAdvisorOrderEntity).Cast<ChannelAdvisorOrderEntity>().All(o => o.IsPrime == (int) ChannelAdvisorIsAmazonPrime.No) ? 
                    (int) ChannelAdvisorIsAmazonPrime.No :
                    (int) ChannelAdvisorIsAmazonPrime.Unknown;

            var recordCreator = new SearchRecordMerger<IChannelAdvisorOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(ChannelAdvisorOrderSearchFields.OrderID,
                x => new ChannelAdvisorOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    CustomOrderIdentifier = x.CustomOrderIdentifier,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}