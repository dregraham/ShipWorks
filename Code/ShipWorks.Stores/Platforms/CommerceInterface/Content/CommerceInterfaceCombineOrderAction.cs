using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.OrderCombinerActions;

namespace ShipWorks.Stores.Platforms.CommerceInterface.Content
{
    /// <summary>
    /// Combination action that is specific to CommerceInterface
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            var recordCreator = new SearchRecordMerger<ICommerceInterfaceOrderEntity>(combinedOrder, orders, sqlAdapter);

            return recordCreator.Perform(CommerceInterfaceOrderSearchFields.OrderID,
                x => new CommerceInterfaceOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    CommerceInterfaceOrderNumber = x.CommerceInterfaceOrderNumber
                });
        }
    }
}