using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ShipWorks.Data.Model.TypedViewClasses;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// ShipWorks index that is currently disabled
    /// </summary>
    public class DisabledIndex
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DisabledIndex(IEnumerable<ShipWorksDisabledDefaultIndexRow> indexRows) :
            this(
                indexRows.FirstOrDefault().IndexName,
                indexRows.FirstOrDefault().TableName,
                indexRows.FirstOrDefault().EnableIndex,
                indexRows.Select(x => new IndexColumn(x.ColumnName, x.IsIncluded)))
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DisabledIndex(string name, string table, string enableIndexSql, IEnumerable<IndexColumn> columns)
        {
            Name = name;
            Table = table;
            EnableIndexSql = enableIndexSql;
            Columns = columns.ToImmutableList();
        }

        /// <summary>
        /// Name of the index
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Table to which this index applies
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// Sql statement that will enable this index
        /// </summary>
        public string EnableIndexSql { get; }

        /// <summary>
        /// List of columns in this index, in the correct order
        /// </summary>
        public IEnumerable<IndexColumn> Columns { get; }

        /// <summary>
        /// Get a list of disabled indexes from a typed view of index columns
        /// </summary>
        public static IEnumerable<DisabledIndex> FromView(ShipWorksDisabledDefaultIndexTypedView availableIndexes) =>
            availableIndexes
                .GroupBy(x => x.IndexName)
                .Select(x => new DisabledIndex(x))
                .ToList();
    }
}