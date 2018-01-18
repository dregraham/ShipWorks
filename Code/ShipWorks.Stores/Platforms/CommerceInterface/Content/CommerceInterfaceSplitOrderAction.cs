using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.CommerceInterface.Content
{
    /// <summary>
    /// Split action that is specific to CommerceInterface
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.CommerceInterface)]
    public class CommerceInterfaceSplitOrderAction : IStoreSpecificSplitOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public void Perform(long originalOrderID, OrderEntity splitOrder)
        {
            if (splitOrder.IsManual)
            {
                return;
            }

            CommerceInterfaceOrderEntity order = (CommerceInterfaceOrderEntity) splitOrder;
            CommerceInterfaceOrderSearchEntity orderSearchEntity = order.CommerceInterfaceOrderSearch.AddNew();

            orderSearchEntity.CommerceInterfaceOrderNumber = order.CommerceInterfaceOrderNumber;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}