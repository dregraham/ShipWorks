using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;

namespace ShipWorks.Stores.Orders.Split.Local
{
    /// <summary>
    /// Split order details among two orders.  For example, order items or order charges
    /// </summary>
    [Service]
    public interface IOrderDetailSplitterLocal
    {
        /// <summary>
        /// Split the details
        /// </summary>
        void Split(OrderSplitDefinition orderSplitDefinition, OrderEntity originalOrder, OrderEntity splitOrder);
    }
}
