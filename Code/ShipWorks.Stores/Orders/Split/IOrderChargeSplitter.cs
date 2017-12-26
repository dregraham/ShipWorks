using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Split charges among two orders
    /// </summary>
    public interface IOrderChargeSplitter
    {
        /// <summary>
        /// Split the charges
        /// </summary>
        void Split(IDictionary<long, decimal> newOrderChargeAmounts, OrderEntity originalOrder, OrderEntity splitOrder);
    }
}
