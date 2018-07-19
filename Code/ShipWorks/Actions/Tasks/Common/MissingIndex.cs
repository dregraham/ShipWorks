using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ShipWorks.Data.Model.TypedViewClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Index that SQL Server thinks is missing from ShipWorks
    /// </summary>
    public class MissingIndex
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MissingIndex(IGrouping<int, ShipWorksMissingIndexRequestsRow> indexRows) :
            this(
                indexRows.FirstOrDefault().GroupHandle,
                indexRows.FirstOrDefault().TableName,
                indexRows.Select(x => new IndexColumn(x.ColumnName, x.ColumnUsage == "INCLUDE")))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MissingIndex(int groupHandle, string table, IEnumerable<IndexColumn> columns)
        {
            GroupHandle = groupHandle;
            Table = table;
            Columns = columns.ToImmutableList();
        }

        /// <summary>
        /// Group handle of the index
        /// </summary>
        public int GroupHandle { get; }

        /// <summary>
        /// Table to which the index applies
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// Columns in the index, in the correct order
        /// </summary>
        public IEnumerable<IndexColumn> Columns { get; }

        /// <summary>
        /// Create a list of missing indexes from a typed view
        /// </summary>
        public static IEnumerable<MissingIndex> FromView(ShipWorksMissingIndexRequestsTypedView missingIndexes) =>
            missingIndexes
                .GroupBy(x => x.GroupHandle)
                .Select(x => new MissingIndex(x))
                .ToList();
    }
}