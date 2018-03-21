using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Class used for generating SQL for order archiving
    /// </summary>
    [Component]
    public class OrderArchiveSqlGenerator : IOrderArchiveSqlGenerator
    {
        /// <summary>
        /// Generate and return SQL for copying a database
        /// </summary>
        public string CopyDatabaseSql(string newDatabasename)
        {
            return ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.CopyDatabase.sql")
                .Replace("%destinationDatabaseName%", newDatabasename);
        }

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        public string ArchiveOrderDataSql(string databasename, DateTime maxOrderDate, OrderArchiverOrderDataComparisonType comparisonType)
        {
            string sqlToFormat = ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.ArchiveOrderData.sql");

            return sqlToFormat.Replace("%databaseName%", databasename)
                .Replace("%orderDate%", maxOrderDate.Date.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("%orderDateComparer%", comparisonType == OrderArchiverOrderDataComparisonType.LessThan ? "<" : ">=");
        }
    }
}
