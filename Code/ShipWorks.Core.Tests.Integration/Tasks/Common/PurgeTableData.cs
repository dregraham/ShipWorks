using System.Data;
using System.Linq;

namespace ShipWorks.Core.Tests.Integration.Tasks.Common
{
    /// <summary>
    /// Helper class for testing purges.
    /// </summary>
    public class PurgeTableData
    {
        public string TableName;
        public long InitialRowCount = 0;
        public long AfterPurgeRowCount = 0;
        public DataTable InitialTable = null;
        public DataTable AfterPurgeTable = null;

        public PurgeTableData(string tableName, DataTable initialTable, DataTable afterPurgeTable)
        {
            TableName = tableName;
            InitialTable = initialTable;
            AfterPurgeTable = afterPurgeTable;
            InitialRowCount = InitialTable?.Rows.Count ?? 0;
            AfterPurgeRowCount = AfterPurgeTable?.Rows.Count ?? 0;
        }

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
