using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.Sears.Content
{
    /// <summary>
    /// Combination action that is specific to Sears
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Sears)]
    public class SearsCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<ISearsOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(SearsOrderSearchFields.OrderID,
                x => new SearsOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    PoNumber = x.PoNumber,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}