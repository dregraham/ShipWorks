using System;
using System.Data.Common;
using System.Data.SqlClient;
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
        public static async Task WithTransactionAsync(this DbConnection connection, Func<DbTransaction, ISqlAdapter, Task> operation)
        {
            using (DbTransaction transaction = connection.BeginTransaction())
            {
                using (ISqlAdapter adapter = new SqlAdapter(connection, transaction))
                {
                    try
                    {
                        await operation(transaction, adapter).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Perform an action with a transaction
        /// </summary>
        public static void WithTransaction(this DbConnection connection, Action<DbTransaction, ISqlAdapter> operation)
        {
            using (DbTransaction transaction = connection.BeginTransaction())
            {
                using (ISqlAdapter adapter = new SqlAdapter(connection, transaction))
                {
                    try
                    {
                        operation(transaction, adapter);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
