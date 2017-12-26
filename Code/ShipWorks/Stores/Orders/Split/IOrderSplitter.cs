using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Split an order
    /// </summary>
    public interface IOrderSplitter
    {
        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        Task<IEnumerable<string>> Split(OrderEntity order, OrderSplitDefinition definition);
    }
}
