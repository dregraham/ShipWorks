using System;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Interface used for generating SQL for order archiving
    /// </summary>
    public interface IOrderArchiveSqlGenerator
    {
        /// <summary>
        /// Generate and return SQL for copying a database
        /// </summary>
        string CopyDatabaseSql();

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        string ArchiveOrderDataSql(DateTime maxOrderDate);
    }
}
