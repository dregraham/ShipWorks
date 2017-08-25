using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;
using ShipWorks.Stores.Platforms.Amazon.Mws;

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
            AmazonOrderEntity order = (AmazonOrderEntity) combinedOrder;

            order.IsPrime =
                orders.Where(o => o is AmazonOrderEntity).Cast<AmazonOrderEntity>().All(o => o.IsPrime == (int) AmazonMwsIsPrime.No) ? 
                    (int) AmazonMwsIsPrime.No : 
                    (int) AmazonMwsIsPrime.Unknown;

            order.FulfillmentChannel = 
                orders.Where(o => o is AmazonOrderEntity).Cast<AmazonOrderEntity>().All(o => o.FulfillmentChannel == (int) AmazonMwsFulfillmentChannel.MFN) ? 
                    (int) AmazonMwsFulfillmentChannel.MFN :
                    (int) AmazonMwsFulfillmentChannel.Unknown;

            var recordCreator = new SearchRecordMerger<IAmazonOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(AmazonOrderSearchFields.OrderID,
                x => new AmazonOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    AmazonOrderID = x.AmazonOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}