using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Split order details among two orders.  For example, order items or order charges
    /// </summary>
    public interface IOrderDetailSplitter
    {
        /// <summary>
        /// Split the details
        /// </summary>
        void Split(OrderSplitDefinition orderSplitDefinition, OrderEntity originalOrder, OrderEntity splitOrder);
    }
}
