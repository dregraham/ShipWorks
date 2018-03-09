using System;
using System.Data.Common;
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
        /// Execute a function with a connection in single user mode
        /// </summary>
        Task<T> WithSingleUserConnectionAsync<T>(Func<DbConnection, Task<T>> func);

        /// <summary>
        /// Create a SqlAdapter with the given connection
        /// </summary>
        ISqlAdapter CreateSqlAdapter(DbConnection con);

        /// <summary>
        /// Execute a block of sql on the given transaction
        /// </summary>
        Task ExecuteSqlAsync(DbTransaction transaction, IProgressReporter prepareProgress, string commandText);
    }
}
