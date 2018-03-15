using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Archive.Errors;
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
        private OrderArchiveException orderArchiveException = null;

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
            if (orderArchiveException != null)
            {
                if (progressReporter.CanCancel)
                {
                    progressReporter.Cancel();
                }

                progressReporter.Detail = orderArchiveException.Message;

                return;
            }

            using (HandleSqlInfo(connection, progressReporter))
            {
                var command = connection.CreateCommand();
                command.CommandTimeout = commandTimeout;

                progressReporter.Starting();
                progressReporter.PercentComplete = 0;
                command.CommandText = commandText;
                await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                if (progressReporter.Status == ProgressItemStatus.Running)
                {
                    progressReporter.PercentComplete = 100;
                    progressReporter.Completed();
                }
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
                void infoHandler(object sender, SqlInfoMessageEventArgs e) => UpdateProgress(progressReporter, e);

                sqlConnection.FireInfoMessageEventOnUserErrors = true;
                sqlConnection.InfoMessage += infoHandler;
                
                return Disposable.Create(() => sqlConnection.InfoMessage -= infoHandler);
            }

            return Disposable.Empty;
        }

        /// <summary>
        /// Handle SQL Info Messages and update progress as needed
        /// </summary>
        private void UpdateProgress(IProgressReporter progressReporter, SqlInfoMessageEventArgs sqlInfoMessageEvent)
        {
            if (orderArchiveException != null || progressReporter.Status != ProgressItemStatus.Running)
            {
                return;
            }

            string message = sqlInfoMessageEvent.Message.Trim();

            SqlError error = sqlInfoMessageEvent.Errors.Cast<SqlError>().FirstOrDefault(sqlError => !IgnoreSqlErrorMessage(sqlError.Message));

            if (error?.Message.IsNullOrWhiteSpace() == false)
            {
                message = error.Message;
                log.Info($"OrderArchive: UpdateProgress SQL ERROR Message - {message}");

                orderArchiveException = new OrderArchiveException(message);
                progressReporter.Failed(orderArchiveException);
                return;
            }

            message = message.Replace("OrderArchiveInfo:", string.Empty);

            log.Info($"OrderArchive: UpdateProgress SQL Message - {message}");

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
            else if (IgnoreSqlErrorMessage(message))
            {
                // Nothing to do for these, just continue.   
            }
            else
            {
                progressReporter.Detail = message;
            }
        }

        /// <summary>
        /// Checks a message to determine if it should be ignored.
        /// </summary>
        private static bool IgnoreSqlErrorMessage(string message)
        {
            string messageToCheck = message.Trim();

            return messageToCheck.StartsWith("OrderArchiveInfo:") ||
                   messageToCheck.IndexOf("pages for database", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   messageToCheck.IndexOf("The backup set", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                   messageToCheck.IndexOf("Processed ", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                   messageToCheck.IndexOf("RESTORE DATABASE successfully", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                   messageToCheck.IndexOf("BACKUP DATABASE successfully", StringComparison.InvariantCultureIgnoreCase) == 0 ||
                   messageToCheck.IndexOf("percent processed", StringComparison.InvariantCultureIgnoreCase) > 0 ||
                   messageToCheck.IndexOf("BatchTotal:", StringComparison.InvariantCultureIgnoreCase) > 0;
        }

        /// <summary>
        /// Create a SqlAdapter with the given connection
        /// </summary>
        public ISqlAdapter CreateSqlAdapter(DbConnection con) => sqlAdapterFactory.Create(con);
    }
}
