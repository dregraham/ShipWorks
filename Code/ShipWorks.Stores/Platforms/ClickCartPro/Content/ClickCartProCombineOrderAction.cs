using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.ClickCartPro.Content
{
    /// <summary>
    /// Combination action that is specific to ClickCartPro
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.ClickCartPro)]
    public class ClickCartProCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IClickCartProOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(ClickCartProOrderFields.OrderID,
                x => new ClickCartProOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    ClickCartProOrderID = x.ClickCartProOrderID,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}