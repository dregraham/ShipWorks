using System;
using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Resolve indexes that could be enabled based on a list of missing indexes
    /// </summary>
    [Component]
    public class MissingIndexResolver : IMissingIndexResolver
    {
        public IEnumerable<DisabledIndex> GetIndexesToEnable(IEnumerable<MissingIndex> missingIndexes, IEnumerable<DisabledIndex> disabledIndexes)
        {
            //missingIndexes
            //    .Select(x => FindBestExistingIndex(x, disabledIndexes))

            throw new NotImplementedException();
        }

        //private object FindBestExistingIndex(MissingIndex missing, IEnumerable<DisabledIndex> disabledIndexes) =>
        //    disabledIndexes
        //        .Select(x => (x, MatchingColumns(x, missing), x.Count()))
        //        .OrderByDescending(x => x.Item2)
        //        .ThenBy(x => x.Item3);

        //public int MatchingColumns(IEnumerable<DisabledIndex> index, IEnumerable<MissingIndex> suggested)
        //{
        //    return suggested
        //        .Zip(index, (s, i) => s.Column == i.Column)
        //        .TakeWhile(x => x)
        //        .Count();
        //}
    }
}
