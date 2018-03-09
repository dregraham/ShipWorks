using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Audit;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Connection manager for order archiving
    /// </summary>
    [Component]
    public class OrderArchiveDataAccess : IOrderArchiveDataAccess
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveDataAccess(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Execute a function with a connection in single user mode
        /// </summary>
        public async Task<T> WithSingleUserConnectionAsync<T>(Func<DbConnection, Task<T>> func)
        {
            using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
            {
                using (new SingleUserModeScope())
                {
                    using (new ExistingConnectionScope())
                    {
                        return await func(ExistingConnectionScope.ScopedConnection).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Execute a block of sql on the given transaction
        /// </summary>
        public async Task ExecuteSqlAsync(DbTransaction transaction, IProgressReporter progressReporter, string commandText)
        {
            using (HandleSqlInfo(transaction.Connection, progressReporter))
            {
                var command = transaction.Connection.CreateCommand();
                command.Transaction = transaction;

                progressReporter.Starting();
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                progressReporter.Completed();
            }
        }

        /// <summary>
        /// Handle connecting and disconnecting SqlInfo event handler
        /// </summary>
        private IDisposable HandleSqlInfo(DbConnection connection, IProgressReporter progressReporter)
        {
            if (connection is SqlConnection sqlConnection)
            {
                void infoHandler(object sender, SqlInfoMessageEventArgs e) => progressReporter.Detail = e.Message;

                sqlConnection.FireInfoMessageEventOnUserErrors = true;
                sqlConnection.InfoMessage += infoHandler;

                return Disposable.Create(() => sqlConnection.InfoMessage -= infoHandler);
            }

            return Disposable.Empty;
        }

        /// <summary>
        /// Create a SqlAdapter with the given connection
        /// </summary>
        public ISqlAdapter CreateSqlAdapter(DbConnection con) => sqlAdapterFactory.Create(con);
    }
}
