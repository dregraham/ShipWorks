using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Split items among two orders
    /// </summary>
    public interface IOrderItemSplitter
    {
        /// <summary>
        /// Split the items
        /// </summary>
        void Split(IDictionary<long, double> newOrderItemQuantities, OrderEntity originalOrder, OrderEntity splitOrder);
    }
}
