using System;
using System.Data.Common;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using Interapptive.Shared.Threading;
using ShipWorks.Data.Administration.Recovery;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Implementation of IFilterHelper to be able to DI and Mock classes that use FilterHelper
    /// </summary>
    public class FilterHelperWrapper : IFilterHelper
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterHelperWrapper(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Ensure filters are up to date
        /// </summary>
        public bool EnsureFiltersUpToDate(TimeSpan timeout) =>
            FilterHelper.EnsureFiltersUpToDate(timeout);

        /// <summary>
        /// Indicates if the given object is in the filter contents of the specified filter content id
        /// </summary>
        public bool IsObjectInFilterContent(long orderID, IRuleEntity rule)
        {
            if (rule == null)
            {
                return false;
            }

            long? filterContentID = FilterHelper.GetFilterNodeContentID(rule.FilterNodeID);

            return filterContentID != null &&
                FilterHelper.IsObjectInFilterContent(orderID, filterContentID.Value);
        }

        /// <summary>
        /// Regenerate all the filters
        /// </summary>
        public void RegenerateFilters(DbConnection con)
        {
            // Regenerate the filters
            using (var sqlAdapter = sqlAdapterFactory.Create(con))
            {
                FilterLayoutContext.Current.RegenerateAllFilters(sqlAdapter);
            }

            // Delete any filter counts we may have abandoned by regenerating
            FilterContentManager.DeleteAbandonedFilterCounts();

            // We can wipe any dirties and any current checkpoint - they don't matter since we have regenerated all filters anyway
            SqlUtility.TruncateTable("FilterNodeContentDirty", con);
            SqlUtility.TruncateTable("FilterNodeUpdateCheckpoint", con);
        }

        /// <summary>
        /// Calculate initial filter counts while doing a database upgrade.
        /// </summary>
        public void CalculateInitialFilterCounts(DbConnection connection, IProgressReporter progressFilterCounts, int initialPercentComplete)
        {
            if (progressFilterCounts.Status == ProgressItemStatus.Pending)
            {
                progressFilterCounts.Starting();
            }

            progressFilterCounts.Detail = "Calculating initial filter counts...";
            progressFilterCounts.PercentComplete = initialPercentComplete;

            // Create a new adapter
            using (SqlAdapter adapter = new SqlAdapter(connection))
            {
                adapter.KeepConnectionOpen = true;

                FilterCollection filters = new FilterCollection();
                adapter.FetchEntityCollection(filters, null);
                int totalFilters = filters.Count;

                // The calculation procedures bail out as soon as they hit a time threshold - but only at certain checkpoints.  So if
                // a single update calculation took 1 minute - then the command would take a full minute.  So we need to make sure and
                // give this plenty of time.
                adapter.CommandTimeOut = int.MaxValue;

                SqlAdapterRetry<SqlException> sqlAppResourceLockExceptionRetry =
                    new SqlAdapterRetry<SqlException>(5, -5, "ActionProcedures.CalculateInitialFilterCounts");

                int nodesUpdated = 1;
                int iterationNodesUpdated = 1;
                sqlAppResourceLockExceptionRetry.ExecuteWithRetry(() =>
                {
                    // Keep calculating until no nodes were updated
                    while (iterationNodesUpdated > 0)
                    {
                        ActionProcedures.CalculateInitialFilterCounts(ref iterationNodesUpdated, adapter);
                        nodesUpdated += iterationNodesUpdated;

                        if (iterationNodesUpdated == 0)
                        {
                            progressFilterCounts.PercentComplete = 100;
                        }
                        else
                        {
                            progressFilterCounts.PercentComplete = (int) (((decimal) nodesUpdated / totalFilters) * 100);
                        }
                    }
                });

                progressFilterCounts.Detail = "Done";
                progressFilterCounts.Completed();
            }
        }
    }
}
