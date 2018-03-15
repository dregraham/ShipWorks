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
                            .WithSingleUserConnectionAsync(PerformArchive(cutoffDate, prepareProgress, archiveProgress))
                            .Do(_ => connectionManager.WithMultiUserConnection(RegenerateFilters(filterProgress)))
                            .Recover(_ => TerminateNonStartedTasks(new[] { archiveProgress, filterProgress }))
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
        private Unit TerminateNonStartedTasks(IProgressReporter[] progressReporters)
        {
            foreach (var progressReporter in progressReporters.Where(x => x.Status == ProgressItemStatus.Pending))
            {
                progressReporter.Terminate();
            }

            return Unit.Default;
        }

        /// <summary>
        /// Perform the archive
        /// </summary>
        private Func<DbConnection, Task<Unit>> PerformArchive(DateTime cutoffDate, IProgressReporter prepareProgress, IProgressReporter archiveProgress) =>
            (conn) =>
                ExecuteSqlAsync(prepareProgress, conn, "Creating Archive Database", sqlGenerator.CopyDatabaseSql())
                    .Bind(_ => ExecuteSqlAsync(archiveProgress, conn, "Archiving Order and Shipment data", sqlGenerator.ArchiveOrderDataSql(cutoffDate)));

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
