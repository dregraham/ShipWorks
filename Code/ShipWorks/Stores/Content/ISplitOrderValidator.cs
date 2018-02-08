using System.Collections.Generic;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order validation for splitting an order
    /// </summary>
    public interface ISplitOrderValidator
    {
        /// <summary>
        /// Validate the order selected for splitting
        /// </summary>
        Result Validate(IEnumerable<long> validate);
    }
}
