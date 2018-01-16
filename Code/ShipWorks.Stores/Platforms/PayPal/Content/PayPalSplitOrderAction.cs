using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split.Actions;

namespace ShipWorks.Stores.Platforms.PayPal.Content
{
    /// <summary>
    /// Split action that is specific to PayPal
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificSplitOrderAction), StoreTypeCode.PayPal)]
    public class PayPalSplitOrderAction : IStoreSpecificSplitOrderAction
    {
        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public void Perform(long originalOrderID, OrderEntity splitOrder)
        {
            PayPalOrderEntity order = (PayPalOrderEntity) splitOrder;
            PayPalOrderSearchEntity orderSearchEntity = order.PayPalOrderSearch.AddNew();

            orderSearchEntity.TransactionID = order.TransactionID;
            orderSearchEntity.OriginalOrderID = originalOrderID;
        }
    }
}