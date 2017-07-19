using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombineOrderActions;

namespace ShipWorks.Stores.Platforms.PayPal.Content
{/// <summary>
 /// Combination action that is specific to PayPal
 /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.PayPal)]
    public class PayPalCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<IPayPalOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(PayPalOrderSearchFields.OrderID,
                x => new PayPalOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    TransactionID = x.TransactionID
                });
        }
    }
}