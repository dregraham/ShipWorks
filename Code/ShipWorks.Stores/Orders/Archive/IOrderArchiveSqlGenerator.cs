using System;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Interface used for generating SQL for order archiving
    /// </summary>
    public interface IOrderArchiveSqlGenerator
    {
        /// <summary>
        /// Generate and return SQL for copying a database
        /// </summary>
        string CopyDatabaseSql(string newDatabasename);

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        string ArchiveOrderDataSql(string databasename, DateTime maxOrderDate, OrderArchiverOrderDataComparisonType comparisonType);
    }
}
