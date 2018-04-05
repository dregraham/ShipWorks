using System;
using System.Data.Common;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Connection;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Connection manager for order archiving
    /// </summary>
    public interface IOrderArchiveDataAccess
    {
        /// <summary>
        /// Execute a function with a connection in multi user mode
        /// </summary>
        Task<T> WithMultiUserConnectionAsync<T>(Func<DbConnection, Task<T>> func);

        /// <summary>
        /// Execute an action with a connection in multi user mode
        /// </summary>
        void WithMultiUserConnection(Action<DbConnection> action);

        /// <summary>
        /// Create a SqlAdapter with the given connection
        /// </summary>
        ISqlAdapter CreateSqlAdapter(DbConnection con);

        /// <summary>
        /// Execute a block of sql on the given DbConnection
        /// </summary>
        Task<Unit> ExecuteSqlAsync(DbConnection con, IProgressReporter prepareProgress, string message, string commandText);

        /// <summary>
        /// Get count of orders that will be archived
        /// </summary>
        Task<long> GetCountOfOrdersToArchive(DateTime archiveDate);

        /// <summary>
        /// Get the current database name (Not the archive db name)
        /// </summary>
        string CurrentDatabaseName { get; }

        /// <summary>
        /// Enable archive triggers, making the database "read only"
        /// </summary>
        void EnableArchiveTriggers(DbConnection conn);

        /// <summary>
        /// Disable archive triggers, making the database "writable"
        /// </summary>
        void DisableArchiveTriggers(DbConnection conn);

        /// <summary>
        /// Disable auto processing settings in archive databases.  (Auto download, auto create shipments, etc...)
        /// </summary>
        void DisableAutoProcessingSettings(DbConnection conn);

        /// <summary>
        /// Get order counts for telemetry
        /// </summary>
        Task<(long totalOrders, long purgedOrders)> GetOrderCountsForTelemetry(DateTime cutoffDate);
    }
}
