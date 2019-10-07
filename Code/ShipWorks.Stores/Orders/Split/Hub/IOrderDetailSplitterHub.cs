using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;

namespace ShipWorks.Stores.Orders.Split.Hub
{
    /// <summary>
    /// Split order details.
    /// </summary>
    [Service]
    public interface IOrderDetailSplitterHub
    {
        /// <summary>
        /// Split the details
        /// </summary>
        void Split(OrderSplitDefinition orderSplitDefinition, OrderEntity originalOrder);
    }
}
