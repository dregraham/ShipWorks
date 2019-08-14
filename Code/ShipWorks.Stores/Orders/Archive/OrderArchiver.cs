﻿using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Data;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Actions;
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
        private bool manualArchive = true;

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
        public Task<IResult> Archive(DateTime cutoffDate, bool isManualArchive)
        {
            manualArchive = isManualArchive;

            return Functional.UsingAsync(
                new TrackedDurationEvent("Orders.Archiving"),
                async evt =>
                {
                    var (totalOrderCount, ordersToPurgeCount) = await orderArchiveDataAccess.GetOrderCountsForTelemetry(cutoffDate);

                    if (ordersToPurgeCount == 0)
                    {
                        AddTelemetryProperties(cutoffDate, evt, totalOrderCount, ordersToPurgeCount, OrderArchiveResult.Succeeded);
                        return Result.FromSuccess();
                    }

                    return await ArchiveAsync(cutoffDate, evt)
                        .Do(result =>
                        {
                            AddTelemetryProperties(cutoffDate, evt, totalOrderCount, ordersToPurgeCount, result.Value);
                        })
                        .Map(result => (IResult) result);
                });
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        /// <param name="cutoffDate">Date before which orders will be archived</param>
        /// <returns>Task of Unit, where Unit is just a placeholder to let us treat this method
        /// as a Func instead of an Action for easier composition.</returns>
        public async Task<GenericResult<OrderArchiveResult>> ArchiveAsync(DateTime cutoffDate, TrackedDurationEvent trackedDurationEvent)
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
                        Exception exception = null;

                        await orderArchiveDataAccess
                            .WithMultiUserConnectionAsync(connection =>
                                PerformArchive(connection, cutoffDate, prepareProgress, archiveProgress, trackedDurationEvent))
                            .Bind(ContinueArchive(cutoffDate, trackedDurationEvent, filterProgress, syncProgress))
                            .Recover(ex =>
                            {
                                exception = ex;
                                return TerminateNonStartedTasks(ex, new[] {prepareProgress, archiveProgress, syncProgress, filterProgress});
                            })
                            .Bind(_ => progressProvider.Terminated)
                            .ConfigureAwait(true);

                        // Make sure we can connect before trying to audit
                        if (SqlSession.Current.CanConnect())
                        {
                            await orderArchiveDataAccess.Audit(manualArchive, !progressProvider.HasErrors).ConfigureAwait(false);
                        }

                        return OrderArchiveResult.Succeeded;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while archiving.", ex);
				// On failure, we want the archive task to leave the action queue, so always return success.
                return OrderArchiveResult.Succeeded;
            }
            finally
            {
                try
                {
                    userLoginWorkflow.Logon(loggedInUser);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
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
            try
            {
                int retentionPeriodInDays = DateTime.UtcNow.Subtract(cutoffDate).Days;

                trackedDurationEvent.AddProperty("Orders.Archiving.Result", EnumHelper.GetApiValue(result));
                trackedDurationEvent.AddProperty("Orders.Archiving.Type", manualArchive ? "Manual" : "Automatic");
                trackedDurationEvent.AddProperty("Orders.Archiving.RetentionPeriodInDays", retentionPeriodInDays.ToString());
                trackedDurationEvent.AddProperty("Orders.Archiving.OrdersArchived", ordersToPurgeCount.ToString());
                trackedDurationEvent.AddProperty("Orders.Archiving.OrdersRetained", (totalOrderCount - ordersToPurgeCount).ToString());
                trackedDurationEvent.AddProperty("Orders.Archiving.ArchiveDb.SizeInMB", (SqlDiskUsage.GetDatabaseSpaceUsed(archiveDatabaseName) / Telemetry.Megabyte).ToString("#0.0"));
                trackedDurationEvent.AddProperty("Orders.Archiving.TransactionalDb.SizeInMB", (SqlDiskUsage.GetDatabaseSpaceUsed(currentDatabaseName) / Telemetry.Megabyte).ToString("#0.0"));
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
            log.Error("Archive failed", ex);

            // Fail any progress reporters that are running
            foreach (var progressReporter in progressReporters.Where(x => x.Status == ProgressItemStatus.Running))
            {
                progressReporter.Failed(ex);
            }

            // Now if there are no failed progress reporters, that means we were between progress reporters
            // when the exception occurred.  So, we'll grab the first pending one, start it, and immediate fail
            // it so that the UI has something to show the exception.
            if (progressReporters.None(pr => pr.Status == ProgressItemStatus.Failure))
            {
                var progressReporter = progressReporters.First(x => x.Status == ProgressItemStatus.Pending);
                if (progressReporter != null)
                {
                    progressReporter.Starting();
                    progressReporter.Failed(ex);
                }
            }

            // Now terminate any remaining pending ones.
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

            string copyDatabaseSql = sqlGenerator.CopyDatabaseSql(archiveDatabaseName, cutoffDate, currentDatabaseName) +
                                     Environment.NewLine +
                                     sqlGenerator.DisableAutoProcessingSettingsSql(archiveDatabaseName);

            return ExecuteSqlAsync(prepareProgress, conn, "Creating Archive Database", copyDatabaseSql,
                        (timeInSeconds) => trackedDurationEvent.AddProperty("Orders.Archiving.CreateArchive.DurationInSecond", timeInSeconds.ToString()))
                    .Bind(_ => ExecuteSqlAsync(archiveProgress, conn, "Archiving Order and Shipment data", currentDbArchiveSql,
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
