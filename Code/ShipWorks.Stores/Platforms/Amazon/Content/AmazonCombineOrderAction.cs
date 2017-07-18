using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.Amazon.Content
{
    /// <summary>
    /// Combination action that is specific to Amazon
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Amazon)]
    public class AmazonCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IAmazonOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(AmazonOrderSearchFields.OrderID,
                x => new AmazonOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    AmazonOrderID = x.AmazonOrderID
                });
        }
    }
}