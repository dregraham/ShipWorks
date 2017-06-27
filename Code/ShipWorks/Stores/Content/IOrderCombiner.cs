using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Combine orders
    /// </summary>
    public interface IOrderCombiner
    {
        /// <summary>
        /// Combine the list of orders into a single order
        /// </summary>
        GenericResult<long> Combine(long survivingOrderID, IEnumerable<OrderEntity> orders, string newOrderNumber);
    }
}
