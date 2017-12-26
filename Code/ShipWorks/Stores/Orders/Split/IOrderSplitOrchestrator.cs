using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Orchestrate the steps necessary to split an order
    /// </summary>
    public interface IOrderSplitOrchestrator
    {
        /// <summary>
        /// Split an order
        /// </summary>
        Task<IEnumerable<long>> Split(long orderID);
    }
}
