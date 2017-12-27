using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<IDictionary<long, string>> Split(OrderSplitDefinition definition);
    }
}
