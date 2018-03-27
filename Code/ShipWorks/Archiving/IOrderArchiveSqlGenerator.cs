using System;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;

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
        string CopyDatabaseSql(string newDatabasename, DateTime selectedArchivalDate, string sourceDatabasename);

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        string ArchiveOrderDataSql(string databasename, DateTime maxOrderDate, OrderArchiverOrderDataComparisonType comparisonType);

        /// <summary>
        /// Enable archive triggers, making the database "readonly"
        /// </summary>
        string EnableArchiveTriggersSql(ISqlAdapter adapter);

        /// <summary>
        /// Disable archive triggers, making the database "writable"
        /// </summary>
        string DisableArchiveTriggersSql(ISqlAdapter adapter);
    }
}
