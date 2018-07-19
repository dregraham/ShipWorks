using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.TypedViewClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ShipWorks index that is currently disabled
    /// </summary>
    public class DisabledIndex
    {
        public DisabledIndex(IGrouping<int, ShipWorksDisabledDefaultIndexRow> indexRows)
        {
            //Columns = indexRows.Select(x => new IndexColumn(x.ColumnName, x.IsIncluded)).ToImmutableList();
        }

        public DisabledIndex(string enableIndexSql)
        {
            EnableIndexSql = enableIndexSql;
        }

        //public IEnumerable<IndexColumn> Columns { get; }

        public string EnableIndexSql { get; }

        public static IEnumerable<DisabledIndex> FromView(ShipWorksDisabledDefaultIndexTypedView availableIndexes) =>
            availableIndexes
                .GroupBy(x => x.IndexID)
                .Select(x => new DisabledIndex(x))
                .ToList();
    }
}