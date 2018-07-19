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
        /// <summary>
        /// Constructor
        /// </summary>
        public DisabledIndex(IGrouping<int, ShipWorksDisabledDefaultIndexRow> indexRows)
        {
            TableName = indexRows.First().TableName;
            IndexName = indexRows.First().IndexName;
            EnableIndexSql = indexRows.First().EnableIndex;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DisabledIndex(string tableName, string indexName, string enableIndexSql)
        {
            EnableIndexSql = enableIndexSql;
            TableName = tableName;
            IndexName = indexName;
        }

        /// <summary>
        /// Table name
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// Index name
        /// </summary>
        public string IndexName { get; }

        /// <summary>
        /// SQL to enable the index
        /// </summary>
        public string EnableIndexSql { get; }

        /// <summary>
        /// Populate a list of DisabledIndex from a view
        /// </summary>
        public static IEnumerable<DisabledIndex> FromView(ShipWorksDisabledDefaultIndexTypedView availableIndexes) =>
            availableIndexes
                .GroupBy(x => x.IndexID)
                .Select(x => new DisabledIndex(x))
                .ToList();
    }
}