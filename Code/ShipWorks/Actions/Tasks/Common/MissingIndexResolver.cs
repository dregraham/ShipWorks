using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Resolve indexes that could be enabled based on a list of missing indexes
    /// </summary>
    [Component]
    public class MissingIndexResolver : IMissingIndexResolver
    {
        /// <summary>
        /// Get a list of disabled indexes that should be enabled
        /// </summary>
        public IEnumerable<DisabledIndex> GetIndexesToEnable(IEnumerable<MissingIndex> missingIndexes, IEnumerable<DisabledIndex> disabledIndexes) =>
            missingIndexes
                .Select(x => x.FindBestMatchingIndex(disabledIndexes))
                .Where(x => x != null)
                .Distinct();
    }
}
