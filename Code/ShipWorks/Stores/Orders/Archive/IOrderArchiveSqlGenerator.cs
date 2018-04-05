using System;
using ShipWorks.Data.Connection;

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
        string CopyDatabaseSql(string newDatabasename, DateTime selectedArchivalDate, string sourceDatabasename);

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        string ArchiveOrderDataSql(string databasename, DateTime maxOrderDate, OrderArchiverOrderDataComparisonType comparisonType);

        /// <summary>
        /// Enable archive triggers, making the database "read only"
        /// </summary>
        string EnableArchiveTriggersSql(ISqlAdapter adapter);

        /// <summary>
        /// Disable archive triggers, making the database "writable"
        /// </summary>
        string DisableArchiveTriggersSql(ISqlAdapter adapter);

        /// <summary>
        /// Generate SQL for disabling auto processing settings.  (Auto download, auto create shipments, etc...)
        /// </summary>
        string DisableAutoProcessingSettingsSql();
    }
}
