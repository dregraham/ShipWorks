using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Api
{
    /// <summary>
    /// Represents the API Order Repository
    /// </summary>
    public interface IApiOrderRepository
    {
        /// <summary>
        /// Get orders with the given orderNumber or OrderID
        /// </summary>
        IEnumerable<OrderEntity> GetOrders(string orderNumber);
    }
}
