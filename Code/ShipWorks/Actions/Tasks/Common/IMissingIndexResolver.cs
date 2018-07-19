using System.Collections.Generic;
using ShipWorks.Data.Model.TypedViewClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Resolve indexes that could be enabled based on a list of missing indexes
    /// </summary>
    public interface IMissingIndexResolver
    {
        /// <summary>
        /// Get a list of indexes to enable
        /// </summary>
        IEnumerable<DisabledIndex> GetIndexesToEnable(ShipWorksMissingIndexRequestsTypedView missingIndexes, ShipWorksDisabledDefaultIndexTypedView availableIndexes);
    }
}
