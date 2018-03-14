using System;
using System.Data.Common;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Connection manager for order archiving
    /// </summary>
    public interface IOrderArchiveDataAccess
    {
        /// <summary>
        /// Execute a function with a connection in single user mode
        /// </summary>
        Task<T> WithSingleUserConnectionAsync<T>(Func<DbConnection, Task<T>> func);

        /// <summary>
        /// Execute an action with a connection in multi user mode
        /// </summary>
        void WithMultiUserConnectionAsync(Action<DbConnection> action);

        /// <summary>
        /// Create a SqlAdapter with the given connection
        /// </summary>
        ISqlAdapter CreateSqlAdapter(DbConnection con);

        /// <summary>
        /// Execute a block of sql on the given DbConnection
        /// </summary>
        Task ExecuteSqlAsync(DbConnection con, IProgressReporter prepareProgress, string commandText);

        /// <summary>
        /// Get count of orders that will be archived
        /// </summary>
        Task<long> GetCountOfOrdersToArchive(DateTime archiveDate);
    }
}
