using System;
using System.Data.SqlClient;
using System.Reactive;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Filters;
using ShipWorks.Users;

namespace ShipWorks.Archiving
{
    /// <summary>
    /// Archive orders
    /// </summary>
    [Component]
    public class OrderArchiver : IOrderArchiver
    {
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
                using (var foo = messageHelper.ShowProgressDialog(
                    "Archive order and shipment data",
                    "ShipWorks is archiving your orders",
                    progressProvider,
                    TimeSpan.Zero))
                {
                    try
                    {
                        return await connectionManager.WithSingleUserConnectionAsync(async conn =>
                        {
                            // Backup/Restore cannot be done in a transaction.
                            await connectionManager.ExecuteSqlAsync(conn, prepareProgress, sqlGenerator.CopyDatabaseSql()).ConfigureAwait(false);

                            // The archive sql handles the transaction
                            await connectionManager.ExecuteSqlAsync(conn, archiveProgress, sqlGenerator.ArchiveOrderDataSql(cutoffDate)).ConfigureAwait(false);

                            filterProgress.Starting();
                            filterHelper.RegenerateFilters(conn);
                            filterProgress.Completed();

                            return Unit.Default;
                        }).ConfigureAwait(false);
                    }
                    catch (ORMException ex)
                    {
                        prepareProgress.Failed(ex);
                        archiveProgress.Failed(ex);
                        filterProgress.Failed(ex);
                        throw;
                    }
                    catch (SqlException ex)
                    {
                        prepareProgress.Failed(ex);
                        archiveProgress.Failed(ex);
                        filterProgress.Failed(ex);
                        throw;
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
