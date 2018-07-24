using System.Collections.Generic;

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
        IEnumerable<DisabledIndex> GetIndexesToEnable(IEnumerable<MissingIndex> missingIndexes, IEnumerable<DisabledIndex> disabledIndexes);
    }
}
