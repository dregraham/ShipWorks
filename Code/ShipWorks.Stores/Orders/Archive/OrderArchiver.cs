using System;
using System.Data.Common;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Archiving;
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
        private readonly IOrderArchiveDataAccess connectionManager;
        private readonly IFilterHelper filterHelper;
        private readonly IUserSession userSession;
        private readonly IOrderArchiveSqlGenerator sqlGenerator;
        private readonly string archiveDatabaseName;
        private readonly string currentDatabaseName;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderArchiver(
            IAsyncMessageHelper messageHelper,
            IOrderArchiveDataAccess connectionManager,
            IFilterHelper filterHelper,
            IUserSession userSession,
            IOrderArchiveSqlGenerator sqlGenerator)
        {
            this.sqlGenerator = sqlGenerator;
            this.userSession = userSession;
            this.filterHelper = filterHelper;
            this.connectionManager = connectionManager;
            this.messageHelper = messageHelper;

            currentDatabaseName = connectionManager.CurrentDatabaseName;
            archiveDatabaseName = $"{currentDatabaseName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}";
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        /// <param name="cutoffDate">Date before which orders will be archived</param>
        /// <returns>Task of Unit, where Unit is just a placeholder to let us treat this method
        /// as a Func instead of an Action for easier composition.</returns>
        public async Task<Unit> Archive(DateTime cutoffDate)
        {
            UserEntity loggedInUser = userSession.User;

            userSession.Logoff(clearRememberMe: false);

            IProgressProvider progressProvider = messageHelper.CreateProgressProvider();

            IProgressReporter prepareProgress = progressProvider.AddItem("Preparing archive");
            prepareProgress.CanCancel = false;
            IProgressReporter archiveProgress = progressProvider.AddItem("Archiving orders");
            archiveProgress.CanCancel = false;
            IProgressReporter syncProgress = progressProvider.AddItem("Syncing orders");
            syncProgress.CanCancel = false;
            IProgressReporter filterProgress = progressProvider.AddItem("Regenerating filters");
            filterProgress.CanCancel = false;

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
                        await connectionManager
                            .WithSingleUserConnectionAsync(PerformArchive(cutoffDate, prepareProgress, archiveProgress, syncProgress))
                            .Do(_ => connectionManager.WithMultiUserConnection(RegenerateFilters(filterProgress)))
                            .Recover(ex => TerminateNonStartedTasks(ex, new[] { prepareProgress, archiveProgress, syncProgress, filterProgress }))
                            .Bind(_ => progressProvider.Terminated)
                            .ConfigureAwait(false);

                        return Unit.Default;
                    }
                }
            }
            finally
            {
                userSession.Logon(loggedInUser);
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
        private Func<DbConnection, Task<Unit>> PerformArchive(DateTime cutoffDate, IProgressReporter prepareProgress, IProgressReporter archiveProgress, IProgressReporter syncProgress)
        {
            async Task<Unit> Func(DbConnection conn)
            {
                string currentDbArchiveSql = sqlGenerator.ArchiveOrderDataSql(currentDatabaseName, cutoffDate, OrderArchiverOrderDataComparisonType.LessThan);
                string archiveDbArchiveSql = string.Format("{0}{1}{2}{3}{4}", 
                    sqlGenerator.ArchiveOrderDataSql(archiveDatabaseName, cutoffDate, OrderArchiverOrderDataComparisonType.GreaterThanOrEqual), 
                    Environment.NewLine,
                    sqlGenerator.DisableAutoProcessingSettingsSql(), 
                    Environment.NewLine,
                    sqlGenerator.EnableArchiveTriggersSql(new SqlAdapter(conn)));

                return await ExecuteSqlAsync(prepareProgress, conn, "Creating Archive Database", sqlGenerator.CopyDatabaseSql(archiveDatabaseName, cutoffDate, currentDatabaseName))
                    .Bind(_ => ExecuteSqlAsync(archiveProgress, conn, "Archiving Order and Shipment data", currentDbArchiveSql))
                    .Bind(_ => ExecuteSqlAsync(syncProgress, conn, "Syncing Order and Shipment data", archiveDbArchiveSql));
            }

            return Func;
        }

        /// <summary>
        /// Execute the given sql
        /// </summary>
        private Task<Unit> ExecuteSqlAsync(IProgressReporter progressItem, DbConnection conn, string message, string sql) =>
            Functional.UsingAsync(
                new LoggedStopwatch(log, $"OrderArchive: {message} - "),
                _ => connectionManager.ExecuteSqlAsync(conn, progressItem, message, sql));

        /// <summary>
        /// Regenerate filters
        /// </summary>
        private Action<DbConnection> RegenerateFilters(IProgressReporter filterProgress) =>
            (conn) =>
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
            };
    }
}
