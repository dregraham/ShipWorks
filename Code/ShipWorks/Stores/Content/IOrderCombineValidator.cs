using System.Collections.Generic;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order validation for combining orders
    /// </summary>
    public interface IOrderCombineValidator
    {
        /// <summary>
        /// Validate the orders selected for combining
        /// </summary>
        Result Validate(IEnumerable<long> validate);
    }
}
