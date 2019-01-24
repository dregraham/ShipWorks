using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SD.Tools.OrmProfiler.Interceptor;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data
{
    /// <summary>
    /// Extension methods for DbConnection
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Attempt to get the connection as a SqlConnection
        /// </summary>
        /// <remarks>This should be used instead of manually casting so that we can get a SqlConnection
        /// even if the profile is attached while debugging. In release mode, this should be no different
        /// than doing the cast manually</remarks>
        public static SqlConnection AsSqlConnection(this DbConnection connection)
        {
            SqlConnection sqlConn = connection as SqlConnection;

#if DEBUG
            if (sqlConn == null)
            {
                sqlConn = (connection as ProfilerDbConnection)?.WrappedConnection as SqlConnection;
            }
#endif

            return sqlConn;
        }

        /// <summary>
        /// Perform an action with a transaction
        /// </summary>
        public static Task WithTransactionAsync(this DbConnection connection, Func<ISqlAdapter, Task> operation, [CallerMemberName] string name = "") =>
            WithTransactionAsync(connection, async x => { await operation(x).ConfigureAwait(false); return Unit.Default; }, name);

        /// <summary>
        /// Perform an action with a transaction
        /// </summary>
        public static async Task<T> WithTransactionAsync<T>(this DbConnection connection, Func<ISqlAdapter, Task<T>> operation, [CallerMemberName] string name = "")
        {
            using (ISqlAdapter adapter = new SqlAdapter(connection))
            {
                await adapter.StartTransactionAsync(System.Data.IsolationLevel.ReadCommitted, name).ConfigureAwait(false);

                try
                {
                    return await operation(adapter).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    adapter.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Perform an action with a transaction
        /// </summary>
        public static void WithTransaction(this DbConnection connection, Action<ISqlAdapter> operation, [CallerMemberName] string name = "")
        {
            using (ISqlAdapter adapter = new SqlAdapter(connection))
            {
                adapter.StartTransaction(System.Data.IsolationLevel.ReadCommitted, name);

                try
                {
                    operation(adapter);
                }
                catch (Exception)
                {
                    adapter.Rollback();
                    throw;
                }
            }
        }
    }
}
