using System.Data;
using System.Linq;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    /// <summary>
    /// Helper class for testing purges.
    /// </summary>
    public class PurgeTableData
    {
        public PurgeTableData(string tableName, DataTable initialTable, DataTable afterPurgeTable)
        {
            TableName = tableName;
            InitialTable = initialTable;
            AfterPurgeTable = afterPurgeTable;
            InitialRowCount = initialTable?.Rows.Count ?? 0;
            AfterPurgeRowCount = AfterPurgeTable?.Rows.Count ?? 0;
        }

        public string TableName { get; }
        public long InitialRowCount { get; }
        public long AfterPurgeRowCount { get; private set; }
        public DataTable InitialTable { get; }
        public DataTable AfterPurgeTable { get; private set; }

        public void SetAfterPurgeTable(DataTable after)
        {
            if (after != null)
            {
                AfterPurgeTable = after;
                AfterPurgeRowCount = AfterPurgeTable.Rows.Count;
            }
        }

        public bool AreTablesEqual() => InitialTable.Rows.Cast<DataRow>()
                                            .Intersect(AfterPurgeTable.Rows.Cast<DataRow>(), DataRowComparer.Default).Count() == AfterPurgeRowCount;
    }
}
