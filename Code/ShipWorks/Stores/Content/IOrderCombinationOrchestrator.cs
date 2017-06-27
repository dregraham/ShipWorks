using System.Collections.Generic;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Orchestrate the combination of orders
    /// </summary>
    public interface IOrderCombinationOrchestrator
    {
        /// <summary>
        /// Combine the list of orders into a single order
        /// </summary>
        /// <returns>
        /// The value of the result is the ID of the created order
        /// </returns>
        GenericResult<long> Combine(IEnumerable<long> orderIDs);
    }
}
