using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

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
        Task<GenericResult<long>> Combine(long survivingOrderID, IEnumerable<IOrderEntity> orders,
            string newOrderNumber, IProgressReporter progressReporter);
    }
}
