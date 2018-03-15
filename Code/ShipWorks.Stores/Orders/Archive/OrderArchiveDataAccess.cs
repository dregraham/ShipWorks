using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Connection manager for order archiving
    /// </summary>
    [Component]
    public class OrderArchiveDataAccess : IOrderArchiveDataAccess
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderArchiveDataAccess));
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly int commandTimeout = int.MaxValue;
        private bool isRestore = false;

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
        /// Execute a function with a connection in multi user mode
        /// </summary>
        public void WithMultiUserConnection(Action<DbConnection> action)
        {
            using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
            {
                using (new ExistingConnectionScope())
                {
                    action(ExistingConnectionScope.ScopedConnection);
                }
            }
        }

        /// <summary>
        /// Execute a block of sql on the given transaction
        /// </summary>
        public async Task ExecuteSqlAsync(DbConnection connection, IProgressReporter progressReporter, string commandText)
        {
            using (HandleSqlInfo(connection, progressReporter))
            {
                var command = connection.CreateCommand();
                command.CommandTimeout = commandTimeout;

                progressReporter.Starting();
                progressReporter.PercentComplete = 0;
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);
                progressReporter.PercentComplete = 100;
                progressReporter.Completed();
            }
        }

        /// <summary>
        /// Get count of orders that will be archived
        /// </summary>
        public async Task<long> GetCountOfOrdersToArchive(DateTime archiveDate)
        {
            var queryFactory = new QueryFactory();
            var query = queryFactory.Order.Where(OrderFields.OrderDate < archiveDate.Date).Select(OrderFields.OrderID.CountBig());

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchScalarAsync<long>(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Handle connecting and disconnecting SqlInfo event handler
        /// </summary>
        private IDisposable HandleSqlInfo(DbConnection connection, IProgressReporter progressReporter)
        {
            if (connection is SqlConnection sqlConnection)
            {
                void infoHandler(object sender, SqlInfoMessageEventArgs e) => UpdateProgress(progressReporter, e.Message);

                sqlConnection.FireInfoMessageEventOnUserErrors = true;
                sqlConnection.InfoMessage += infoHandler;

                return Disposable.Create(() => sqlConnection.InfoMessage -= infoHandler);
            }

            return Disposable.Empty;
        }

        /// <summary>
        /// Handle SQL Info Messages and update progress as needed
        /// </summary>
        private void UpdateProgress(IProgressReporter progressReporter, string message)
        {
            log.Info($"OrderArchive: UpdateProgress SQL Message - {message}");

            message = message.Trim();

            if (message.IndexOf("percent processed", StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                if (int.TryParse(Regex.Match(message, "[0-9]{0,3}").Value, out int archiveDatabasePercentComplete))
                {
                    progressReporter.PercentComplete = (isRestore ? 50 : 0) + (archiveDatabasePercentComplete / 2);
                }
            }
            else if (message.IndexOf("BACKUP DATABASE successfully", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                isRestore = true;
            }
            else if (message.IndexOf("pages for database", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                     message.IndexOf("The backup set", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                     message.IndexOf("Processed ", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                     message.IndexOf("RESTORE DATABASE successfully", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                // Nothing to do for these, just continue.   
            }
            else
            {
                progressReporter.Detail = message;
            }
        }

        /// <summary>
        /// Create a SqlAdapter with the given connection
        /// </summary>
        public ISqlAdapter CreateSqlAdapter(DbConnection con) => sqlAdapterFactory.Create(con);
    }
}
