using System;
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
        protected MissingIndex()
        {

        }

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
        public MissingIndex(int groupHandle, string tableName, IEnumerable<IndexColumn> columns)
        {
            GroupHandle = groupHandle;
            TableName = tableName;
            Columns = columns.ToImmutableList();
        }

        /// <summary>
        /// Group handle of the index
        /// </summary>
        public int GroupHandle { get; }

        /// <summary>
        /// Table to which the index applies
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Columns in the index, in the correct order
        /// </summary>
        public IEnumerable<IndexColumn> Columns { get; }

        /// <summary>
        /// Find the existing index that best matching this missing index
        /// </summary>
        public virtual DisabledIndex FindBestMatchingIndex(IEnumerable<DisabledIndex> disabledIndexes) =>
            disabledIndexes
                .Where(x => TableName?.Equals(x.TableName, StringComparison.OrdinalIgnoreCase) ?? false)
                .Select(x => (Index: x, MatchingColumns: MatchingColumns(x.Columns), TotalColumns: x.Columns.Count()))
                .Where(x => x.MatchingColumns > 0)
                .OrderByDescending(x => x.MatchingColumns)
                .ThenBy(x => x.TotalColumns)
                .Select(x => x.Index)
                .FirstOrDefault();

        /// <summary>
        /// Match the columns of a disabled index to the columns of this missing index
        /// </summary>
        private int MatchingColumns(IEnumerable<IndexColumn> disabled) =>
            Columns
                .Zip(disabled, (m, d) => m.Name?.Equals(d.Name, StringComparison.OrdinalIgnoreCase) ?? false)
                .TakeWhile(x => x)
                .Count();

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