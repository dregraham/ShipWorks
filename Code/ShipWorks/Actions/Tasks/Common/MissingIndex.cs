using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.TypedViewClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    public class MissingIndex
    {
        private IGrouping<int, ShipWorksMissingIndexRequestsRow> x;

        public MissingIndex(IGrouping<int, ShipWorksMissingIndexRequestsRow> x)
        {
            this.x = x;
        }

        public static IEnumerable<MissingIndex> FromView(ShipWorksMissingIndexRequestsTypedView missingIndexes) =>
            missingIndexes
                .GroupBy(x => x.GroupHandle)
                .Select(x => new MissingIndex(x))
                .ToList();
    }
}