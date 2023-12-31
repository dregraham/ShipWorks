﻿using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Archive.Errors;
using ShipWorks.Users.Audit;
using static Interapptive.Shared.Utility.Functional;

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
        private readonly IOrderArchiveSqlGenerator orderArchiveSqlGenerator;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IAuditUtility auditUtility;
        private readonly int commandTimeout = int.MaxValue;
        private bool isRestore = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiveDataAccess(
            ISqlAdapterFactory sqlAdapterFactory,
            IOrderArchiveSqlGenerator orderArchiveSqlGenerator,
            IDateTimeProvider dateTimeProvider,
            IAuditUtility auditUtility)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderArchiveSqlGenerator = orderArchiveSqlGenerator;
            this.auditUtility = auditUtility;
        }

        /// <summary>
        /// Execute a function with a connection in multi user mode
        /// </summary>
        public async Task<T> WithMultiUserConnectionAsync<T>(Func<DbConnection, Task<T>> func)
        {
            using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
            {
                using (new ExistingConnectionScope())
                {
                    return await func(ExistingConnectionScope.ScopedConnection).ConfigureAwait(false);
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
        public Task<Unit> ExecuteSqlAsync(DbConnection connection, IProgressReporter progressReporter, string message, string commandText)
        {
            progressReporter.Detail = message;
            progressReporter.Starting();
            progressReporter.PercentComplete = 0;

            return Functional.UsingAsync(
                HandleSqlInfo(connection, progressReporter),
                _ => BuildCommand(connection, commandText)
                    .ExecuteNonQueryAsync()
                    .Bind(__ => progressReporter.Status == ProgressItemStatus.Running ?
                        Task.FromResult(CompleteProgress(progressReporter)) :
                        Task.FromException<Unit>(progressReporter is ProgressItem progressItem ? progressItem.Error : Error.Canceled)));
        }

        /// <summary>
        /// Build the command for execution
        /// </summary>
        private DbCommand BuildCommand(DbConnection connection, string commandText)
        {
            var command = connection.CreateCommand();
            command.CommandTimeout = commandTimeout;
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// Complete the given progress reporter
        /// </summary>
        private Unit CompleteProgress(IProgressReporter progressReporter)
        {
            progressReporter.PercentComplete = 100;
            progressReporter.Detail = "Done";
            progressReporter.Completed();
            return Unit.Default;
        }

        /// <summary>
        /// Get count of orders that will be archived
        /// </summary>
        public Task<long> GetCountOfOrdersToArchive(DateTime archiveDate)
        {
            var queryFactory = new QueryFactory();
            var query = queryFactory.Order.Where(OrderFields.OrderDate < archiveDate.Date).Select(OrderFields.OrderID.CountBig());

            return Functional.UsingAsync(sqlAdapterFactory.Create(), adapter => adapter.FetchScalarAsync<long>(query));
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
            if (progressReporter.Status != ProgressItemStatus.Running)
            {
                return;
            }

            string message = sqlInfoMessageEvent.Message.Trim();

            SqlError error = sqlInfoMessageEvent.Errors.Cast<SqlError>().FirstOrDefault(sqlError => !IgnoreSqlErrorMessage(sqlError.Message));

            if (error?.Message.IsNullOrWhiteSpace() == false)
            {
                message = error.Message;
                log.Info($"OrderArchive: UpdateProgress SQL ERROR Message - {message}");

                progressReporter.Failed(new OrderArchiveException(message));
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
                   messageToCheck.IndexOf("BatchTotal:", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   messageToCheck.IndexOf("Nonqualified", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   messageToCheck.IndexOf("The database name", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   messageToCheck.IndexOf("Changed database", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                   messageToCheck.IndexOf("Error executing xp_delete_file", StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        /// <summary>
        /// Create a SqlAdapter with the given connection
        /// </summary>
        public ISqlAdapter CreateSqlAdapter(DbConnection con) => sqlAdapterFactory.Create(con);

        /// <summary>
        /// Get the current database name (Not the archive db name)
        /// </summary>
        public string CurrentDatabaseName => SqlSession.Current.Configuration.DatabaseName;

        /// <summary>
        /// Enable archive triggers, making the database "readonly"
        /// </summary>
        public void EnableArchiveTriggers(DbConnection conn)
        {
            using (ISqlAdapter adapter = new SqlAdapter(conn))
            {
                string enableTriggerSqls = orderArchiveSqlGenerator.EnableArchiveTriggersSql(adapter);

                adapter.ExecuteSQL(enableTriggerSqls);
            }
        }

        /// <summary>
        /// Disable archive triggers, making the database "writable"
        /// </summary>
        public void DisableArchiveTriggers(DbConnection conn)
        {
            using (ISqlAdapter adapter = new SqlAdapter(conn))
            {
                string disableTriggerSqls = orderArchiveSqlGenerator.DisableArchiveTriggersSql(adapter);

                adapter.ExecuteSQL(disableTriggerSqls);
            }
        }

        /// <summary>
        /// Disable auto processing settings in archive databases.  (Auto download, auto create shipments, etc...)
        /// </summary>
        public void DisableAutoProcessingSettings(DbConnection conn)
        {
            using (ISqlAdapter adapter = new SqlAdapter(conn))
            {
                adapter.ExecuteSQL(orderArchiveSqlGenerator.DisableAutoProcessingSettingsSql(conn.Database));
            }
        }

        /// <summary>
        /// Get order counts for telemetry
        /// </summary>
        public Task<(long totalOrders, long purgedOrders)> GetOrderCountsForTelemetry(DateTime cutoffDate) =>
            GetCountOfOrdersToArchive(dateTimeProvider.Now.AddDays(1))
                .Bind(totalOrders => GetCountOfOrdersToArchive(cutoffDate).Map(purgedOrders => (totalOrders, purgedOrders)));

        /// <summary>
        /// Audit that an archive run failed
        /// </summary>
        public Task Audit(bool isManualArchive, bool success)
        {
            if (SqlSession.Current?.CanConnect() == true)
            {
                return UsingAsync(
                    sqlAdapterFactory.Create(),
                    adapter => auditUtility.AuditAsync(null, AuditActionType.Archive, CreateAuditReason(isManualArchive, success), adapter));
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Create the reason for auditing
        /// </summary>
        private AuditReason CreateAuditReason(bool isManualArchive, bool success) =>
            new AuditReason(AuditReasonType.Archive, $"{(isManualArchive ? "Manual" : "Automatic")} archive {(success ? "succeeded." : "failed.")}");
    }
}
