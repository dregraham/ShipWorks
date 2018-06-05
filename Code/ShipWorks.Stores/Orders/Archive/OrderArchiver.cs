using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Data;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Users;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Archive orders
    /// </summary>
    [Component]
    public class OrderArchiver : IOrderArchiver
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderArchiver));
        private readonly IAsyncMessageHelper messageHelper;
        private readonly IOrderArchiveDataAccess orderArchiveDataAccess;
        private readonly IFilterHelper filterHelper;
        private readonly IUserLoginWorkflow userLoginWorkflow;
        private readonly IOrderArchiveSqlGenerator sqlGenerator;
        private readonly string archiveDatabaseName;
        private readonly string currentDatabaseName;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiver(
            IAsyncMessageHelper messageHelper,
            IOrderArchiveDataAccess orderArchiveDataAccess,
            IFilterHelper filterHelper,
            IUserLoginWorkflow userLoginWorkflow,
            IOrderArchiveSqlGenerator sqlGenerator)
        {
            this.sqlGenerator = sqlGenerator;
            this.userLoginWorkflow = userLoginWorkflow;
            this.filterHelper = filterHelper;
            this.orderArchiveDataAccess = orderArchiveDataAccess;
            this.messageHelper = messageHelper;

            currentDatabaseName = this.orderArchiveDataAccess.CurrentDatabaseName;
            archiveDatabaseName = $"{currentDatabaseName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
        }

        /// <summary>
        /// Split an order based on the definition
        /// </summary>
        public Task<Unit> Archive(DateTime cutoffDate)
        {
            return Functional.UsingAsync(
                new TrackedDurationEvent("Orders.Archiving"),
                async evt =>
                {
                    var (totalOrderCount, ordersToPurgeCount) = await orderArchiveDataAccess.GetOrderCountsForTelemetry(cutoffDate);

                    if (ordersToPurgeCount == 0)
                    {
                        AddTelemetryProperties(cutoffDate, evt, totalOrderCount, ordersToPurgeCount, OrderArchiveResult.Succeeded);
                        return await Task.FromResult(Unit.Default);
                    }

                    return await ArchiveAsync(cutoffDate, evt)
                        .Do(result => AddTelemetryProperties(cutoffDate, evt, totalOrderCount, ordersToPurgeCount, result))
                        .Map(_ => Unit.Default);
                });
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        /// <param name="cutoffDate">Date before which orders will be archived</param>
        /// <returns>Task of Unit, where Unit is just a placeholder to let us treat this method
        /// as a Func instead of an Action for easier composition.</returns>
        public async Task<OrderArchiveResult> ArchiveAsync(DateTime cutoffDate, TrackedDurationEvent trackedDurationEvent)
        {
            UserEntity loggedInUser = userLoginWorkflow.CurrentUser;

            if (!userLoginWorkflow.Logoff(clearRememberMe: false))
            {
                return OrderArchiveResult.Cancelled;
            }

            IProgressProvider progressProvider = messageHelper.CreateProgressProvider();

            IProgressReporter prepareProgress = progressProvider.AddItem("Preparing archive");
            prepareProgress.CanCancel = false;
            IProgressReporter archiveProgress = progressProvider.AddItem("Archiving orders");
            archiveProgress.CanCancel = false;
            IProgressReporter filterProgress = progressProvider.AddItem("Regenerating filters");
            filterProgress.CanCancel = false;
            IProgressReporter syncProgress = progressProvider.AddItem("Syncing orders");
            syncProgress.CanCancel = false;

            try
            {
                using (var progress = await messageHelper.ShowProgressDialog(
                    "Archive order and shipment data",
                    "ShipWorks is archiving your orders",
                    progressProvider,
                    TimeSpan.Zero).ConfigureAwait(true))
                {
                    using (new LoggedStopwatch(log, "OrderArchive: Archive orders - "))
                    {
                        await orderArchiveDataAccess
                            .WithMultiUserConnectionAsync(connection =>
                                PerformArchive(connection, cutoffDate, prepareProgress, archiveProgress, trackedDurationEvent))
                            .Bind(ContinueArchive(cutoffDate, trackedDurationEvent, filterProgress, syncProgress))
                            .Recover(ex => TerminateNonStartedTasks(ex, new[] { prepareProgress, archiveProgress, syncProgress, filterProgress }))
                            .Bind(_ => progressProvider.Terminated)
                            .ConfigureAwait(true);

                        return progressProvider.HasErrors ? OrderArchiveResult.Failed : OrderArchiveResult.Succeeded;
                    }
                }
            }
            finally
            {
                userLoginWorkflow.Logon(loggedInUser);
            }
        }

        /// <summary>
        /// Continue the archive process
        /// </summary>
        private Func<Unit, Task<Unit>> ContinueArchive(DateTime cutoffDate, TrackedDurationEvent trackedDurationEvent, IProgressReporter filterProgress, IProgressReporter syncProgress)
        {
            return _ => orderArchiveDataAccess.WithMultiUserConnectionAsync(connection =>
            {
                RegenerateFilters(connection, filterProgress);
                return TrimArchive(connection, cutoffDate, syncProgress, trackedDurationEvent);
            });
        }

        /// <summary>
        /// Add telemetry properties
        /// </summary>
        private void AddTelemetryProperties(DateTime cutoffDate, TrackedDurationEvent trackedDurationEvent, long totalOrderCount,
            long ordersToPurgeCount, OrderArchiveResult result)
        {
            var megabyte = (1024 * 1024);

            try
            {
                int retentionPeriodInDays = DateTime.UtcNow.Subtract(cutoffDate).Days;

                trackedDurationEvent.AddProperty("Orders.Archiving.Result", EnumHelper.GetApiValue(result));
                trackedDurationEvent.AddProperty("Orders.Archiving.Type", "Manual");
                trackedDurationEvent.AddProperty("Orders.Archiving.RetentionPeriodInDays", retentionPeriodInDays.ToString());
                trackedDurationEvent.AddProperty("Orders.Archiving.OrdersArchived", ordersToPurgeCount.ToString());
                trackedDurationEvent.AddProperty("Orders.Archiving.OrdersRetained", (totalOrderCount - ordersToPurgeCount).ToString());
                trackedDurationEvent.AddProperty("Orders.Archiving.ArchiveDb.SizeInMB", (SqlDiskUsage.GetDatabaseSpaceUsed(archiveDatabaseName) / megabyte).ToString("#0.0"));
                trackedDurationEvent.AddProperty("Orders.Archiving.TransactionalDb.SizeInMB", (SqlDiskUsage.GetDatabaseSpaceUsed(currentDatabaseName) / megabyte).ToString("#0.0"));
            }
            catch
            {
                // Just continue...we don't want to stop the combine if telemetry has an issue.
            }
        }

        /// <summary>
        /// Terminate all non-started tasks
        /// </summary>
        private Unit TerminateNonStartedTasks(Exception ex, IProgressReporter[] progressReporters)
        {
            foreach (var progressReporter in progressReporters.Where(x => x.Status == ProgressItemStatus.Running))
            {
                progressReporter.Failed(ex);
            }

            foreach (var progressReporter in progressReporters.Where(x => x.Status == ProgressItemStatus.Pending))
            {
                progressReporter.Terminate();
            }

            return Unit.Default;
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        private Task<Unit> PerformArchive(DbConnection conn, DateTime cutoffDate, IProgressReporter prepareProgress, IProgressReporter archiveProgress, TrackedDurationEvent trackedDurationEvent)
        {
            string archivingDbName = SqlUtility.GetArchivingDatabasename(currentDatabaseName);
            string currentDbArchiveSql = sqlGenerator.ArchiveOrderDataSql(archivingDbName, cutoffDate, OrderArchiverOrderDataComparisonType.LessThan);
            currentDbArchiveSql += $"{Environment.NewLine}ALTER DATABASE [{archivingDbName}] MODIFY NAME = [{currentDatabaseName}]";

            return ExecuteSqlAsync(prepareProgress, conn, "Creating Archive Database",
                    sqlGenerator.CopyDatabaseSql(archiveDatabaseName, cutoffDate, currentDatabaseName),
                    (timeInSeconds) => trackedDurationEvent.AddProperty("Orders.Archiving.CreateArchive.DurationInSecond", timeInSeconds.ToString()))
                .Bind(_ => ExecuteSqlAsync(archiveProgress, conn, "Archiving Order and Shipment data",
                    currentDbArchiveSql,
                    (timeInSeconds) => trackedDurationEvent.AddProperty("Orders.Archiving.Purge.DurationInSeconds", timeInSeconds.ToString())));
        }

        /// <summary>
        /// Trim the archive
        /// </summary>
        private Task<Unit> TrimArchive(DbConnection conn, DateTime cutoffDate, IProgressReporter syncProgress, TrackedDurationEvent trackedDurationEvent)
        {
            string archiveDbArchiveSql =
                sqlGenerator.ArchiveOrderDataSql(archiveDatabaseName, cutoffDate, OrderArchiverOrderDataComparisonType.GreaterThanOrEqual) +
                Environment.NewLine +
                sqlGenerator.DisableAutoProcessingSettingsSql() +
                Environment.NewLine +
                sqlGenerator.EnableArchiveTriggersSql(new SqlAdapter(conn));

            return ExecuteSqlAsync(syncProgress, conn, "Syncing Order and Shipment data",
                archiveDbArchiveSql,
                (timeInSeconds) => trackedDurationEvent.AddProperty("Orders.Archiving.Synch.DurationInSeconds", timeInSeconds.ToString()));
        }

        /// <summary>
        /// Execute the given sql
        /// </summary>
        private Task<Unit> ExecuteSqlAsync(IProgressReporter progressItem, DbConnection conn, string message, string sql, Action<long> addTelemetry)
        {
            return Functional.UsingAsync(
                new LoggedStopwatch(log, $"OrderArchive: {message} - "),
                async _ =>
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    Unit result = await orderArchiveDataAccess.ExecuteSqlAsync(conn, progressItem, message, sql);
                    stopwatch.Stop();

                    addTelemetry((long) stopwatch.Elapsed.TotalSeconds);

                    return result;
                });
        }

        /// <summary>
        /// Regenerate filters
        /// </summary>
        private void RegenerateFilters(DbConnection conn, IProgressReporter filterProgress)
        {
            using (new LoggedStopwatch(log, "OrderArchive: Regenerate filters - "))
            {
                filterProgress.Starting();
                filterProgress.Detail = "Regenerating filters...";
                filterProgress.PercentComplete = 5;

                filterHelper.RegenerateFilters(conn);

                filterHelper.CalculateInitialFilterCounts(conn, filterProgress, 10);

                filterProgress.PercentComplete = 100;
                filterProgress.Detail = "Done.";
            }
        }
    }
}
