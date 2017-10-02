using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Orchestrate the combination of orders
    /// </summary>
    public interface ICombineOrderOrchestrator
    {
        /// <summary>
        /// Combine the list of orders into a single order
        /// </summary>
        /// <returns>
        /// The value of the result is the ID of the created order
        /// </returns>
        Task<GenericResult<long>> Combine(IEnumerable<long> orderIDs);
    }
}