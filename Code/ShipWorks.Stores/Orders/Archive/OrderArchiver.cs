using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
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
                using (var progressDialog = await messageHelper.ShowProgressDialog(
                    "Archive order and shipment data",
                    "ShipWorks is archiving your orders",
                    progressProvider,
                    TimeSpan.Zero).ConfigureAwait(true))
                {
                    using (new LoggedStopwatch(log, "OrderArchive: Archive orders - "))
                    {
                        await connectionManager.WithSingleUserConnectionAsync(async conn =>
                        {
                            // Backup/Restore cannot be done in a transaction.
                            using (new LoggedStopwatch(log, "OrderArchive: Create archive database - "))
                            {
                                prepareProgress.Detail = "Creating Archive Database";
                                await connectionManager.ExecuteSqlAsync(conn, prepareProgress, sqlGenerator.CopyDatabaseSql()).ConfigureAwait(false);
                                prepareProgress.Detail = "Done";
                            }

                            // The archive sql handles the transaction
                            using (new LoggedStopwatch(log, "OrderArchive: Archive order and shipment data - "))
                            {
                                archiveProgress.Detail = "Archiving Order and Shipment data";
                                await connectionManager.ExecuteSqlAsync(conn, archiveProgress, sqlGenerator.ArchiveOrderDataSql(cutoffDate)).ConfigureAwait(false);
                                archiveProgress.Detail = "Done";
                            }

                            return Unit.Default;
                        }).ConfigureAwait(false);

                        // We have to regenerate filters outside of a single user connection, otherwise they all get abandoned.
                        using (new LoggedStopwatch(log, "OrderArchive: Regenerate filters - "))
                        {
                            filterProgress.Starting();
                            filterProgress.Detail = "Regenerating.";

                            using (DbConnection con = SqlSession.Current.OpenConnection())
                            {
                                filterHelper.RegenerateFilters(con);
                            }

                            filterProgress.Detail = "Done.";
                            filterProgress.Completed();
                        }

                        return Unit.Default;
                    }
                }
            }
            finally
            {
                userSession.LogonLastUser();
            }
        }
    }
}
