using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.Jet.Content
{
    /// <summary>
    /// Combination action that is specific to Jet
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Jet)]
    public class JetCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IJetOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(JetOrderSearchFields.OrderID,
                x => new JetOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    MerchantOrderID = x.MerchantOrderId,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}
