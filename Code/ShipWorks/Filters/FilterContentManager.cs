using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using System.Diagnostics;
using System.Threading;
using ShipWorks.Data.Adapter;
using log4net;
using System.Data.SqlClient;
using System.Data;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.SqlServer.General;
using ShipWorks.Users;
using ShipWorks.Filters.Content.SqlGeneration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.Interaction;
using Interapptive.Shared.Utility;
using System.Windows.Forms;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Utility;
using Interapptive.Shared.Data;
using Interapptive.Shared.Collections;

namespace ShipWorks.Filters
{
    /// <summary>
    /// Helper class for dealing with filter counts
    /// </summary>
    public static class FilterContentManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FilterContentManager));

        // A cache of all the filter counts for each node
        static Dictionary<long, FilterCount> countCache = new Dictionary<long,FilterCount>();

        // The max timestamp value we currently have loaded
        static long maxTimestamp = 0;

        // Event that lets us sync threads and know about when calculation is happening
        static ManualResetEvent calculatingEvent = new ManualResetEvent(true);

        // So we know when to recalc date filters when the day changes
        static DateTime dateFiltersLastUpdated;

        // Used to take lock so only one thread can check for changes at a time
        static object checkChangesLock = new object();

        /// <summary>
        /// Completely reload the count cache
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            maxTimestamp = 0;
            countCache.Clear();

            // Get the day when the date-based filters were lasted updated
            dateFiltersLastUpdated = SystemData.Fetch().DateFiltersLastUpdate.Date;

            CheckForChanges();
        }

        /// <summary>
        /// Get the current count for the given node
        /// </summary>
        public static FilterCount GetCount(long filterNodeID)
        {
            lock (countCache)
            {
                FilterCount count = null;
                countCache.TryGetValue(filterNodeID, out count);

                return count;
            }
        }

        /// <summary>
        /// Refresh our our count cache from what has changed since last time.  If any counts are in need of calcuations - initial or update - the calculations are kicked off.
        /// Returns true if any changes were loaded or any calculations were initiated.
        /// </summary>
        public static bool CheckForChanges()
        {
            lock (checkChangesLock)
            {
                // Get the latest counts
                bool changesOrCalculations = GetLatestCounts();

                // Get all the counts still being calculated
                List<FilterCount> initialNeeded = GetInitialCountsNeeded();

                // If there are any that need there filters initial calculations done...
                if (initialNeeded.Count > 0)
                {
                    // Remove all the counts that have been abandoned, we don't care if they are still calculating
                    CheckForAbandonedFilterCounts(initialNeeded);

                    // Get the updated list now that some abandoned ones may have been removed
                    initialNeeded = GetInitialCountsNeeded();

                    // If any are in the initial state, make sure sql server is working on them
                    if (initialNeeded.Count > 0)
                    {
                        CalculateInitialCounts(TimeSpan.FromSeconds(.1));

                        // Get the latest again in case they finished already
                        GetLatestCounts();

                        changesOrCalculations = true;
                    }
                }

                // If there are any update counts required, make sure they are being worked on
                if (IsUpdateCountsNeeded())
                {
                    // Don't wait for them to complete
                    CalculateUpdateCounts();

                    changesOrCalculations = true;
                }

                return changesOrCalculations;
            }
        }

        /// <summary>
        /// Get the latest filter counts from the database
        /// </summary>
        private static bool GetLatestCounts()
        {
            List<FilterCount> filterCountList = null;

            try
            {
                filterCountList = GetFilterCountList();
            }
            catch (SqlException ex)
            {
                // If we couldn't get the counts, don't worry too much about it since the next heartbeat will run it again
                log.Warn("Could not get latest filter counts", ex);
            }

            // No need to do anything else if there aren't any counts
            if (filterCountList == null || !filterCountList.Any())
            {
                return false;
            }

            lock (countCache)
            {
                foreach (FilterCount filterCount in filterCountList)
                {
                    // Cache this under the node ID
                    countCache[filterCount.FilterNodeID] = filterCount;
                }
            }

            // Save the new max
            maxTimestamp = filterCountList.Select(x => x.RowVersion).Concat(new[] { maxTimestamp }).Max();

            return true;
        }

        /// <summary>
        /// Get a list of the latest filter counts from the database
        /// </summary>
        private static List<FilterCount> GetFilterCountList()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = @"
                    SELECT n.FilterNodeID, c.FilterNodeContentID, n.Purpose, c.Status, c.Count, c.CountVersion, CAST(c.RowVersion as bigint) AS 'RowVersion', c.Cost
                        FROM FilterNode n WITH (NOLOCK) INNER JOIN FilterNodeContent c WITH (NOLOCK) ON n.FilterNodeContentID = c.FilterNodeContentID
                        WHERE c.RowVersion > @MaxRowVersion";
                    cmd.Parameters.AddWithValue("@MaxRowVersion", maxTimestamp);

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                    {
                        List<FilterCount> filterCountList = new List<FilterCount>();

                        while (reader.Read())
                        {
                            filterCountList.Add(new FilterCount
                                (
                                    (long)reader["FilterNodeID"],
                                    (long)reader["FilterNodeContentID"],
                                    (FilterNodePurpose)Convert.ToInt32(reader["Purpose"]),
                                    (FilterCountStatus)Convert.ToInt32(reader["Status"]),
                                    (int)reader["Count"],
                                    (long)reader["CountVersion"],
		                            (long) reader["RowVersion"],
		                            (int) reader["cost"]
                                )
                            );
                        }

                        return filterCountList;
                    }   
                }
            }
        }

        /// <summary>
        /// From the counts in the given list, remove all the ones that have been abandoned
        /// </summary>
        private static void CheckForAbandonedFilterCounts(List<FilterCount> counts)
        {
            if (counts.Count == 0)
            {
                return;
            }

            List<long> okList = new List<long>();

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // Select all the counts that are in the count list and not abandoned
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = string.Format(@"
                    SELECT FilterNodeContentID
                    FROM FilterNodeContent
                    WHERE FilterNodeContentID IN (SELECT FilterNodeContentID FROM FilterNode) AND FilterNodeContentID IN ({0})", ArrayUtility.FormatCommaSeparatedList(counts.Select(c => c.FilterNodeContentID).ToArray()));

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        okList.Add(reader.GetInt64(0));
                    }
                }
            }

            lock (countCache)
            {
                foreach (FilterCount count in counts.ToList())
                {
                    // If its no longer a valid count referenced by a node, remove it from our cache - and from the original list
                    if (!okList.Contains(count.FilterNodeContentID))
                    {
                        log.InfoFormat("Removing abandoned count {0} from cache", count.FilterNodeContentID);

                        // The cache is keyed by the node ID - not the count id
                        countCache.Remove(count.FilterNodeID);

                        counts.Remove(count);
                    }
                }
            }
        }

        /// <summary>
        /// Get a list of all counts that need to have their initial counts calculated
        /// </summary>
        private static List<FilterCount> GetInitialCountsNeeded()
        {
            lock (countCache)
            {
                // See if any of them need to have initial calculations.  Search does it's own, so we don't consider those.
                return countCache.Values.Where(count =>
                    {
                        return
                            (count.Status == FilterCountStatus.NeedsInitialCount || count.Status == FilterCountStatus.RunningInitialCount) &&
                             count.Purpose != FilterNodePurpose.Search;
                    }).ToList();
            }
        }

        /// <summary>
        /// Indicates if calculating update counts is currently needed.  This is determined by if there are any rows in the FilterNodeContentDirty table
        /// </summary>
        private static bool IsUpdateCountsNeeded()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                return (SqlCommandProvider.ExecuteScalar(con, "SELECT TOP(1) ObjectID FROM FilterNodeContentDirty WITH (NOLOCK)") != null);
            }
        }

        /// <summary>
        /// Calculate the update counts of all nodes in the ready state.  If it does not complete
        /// within the time specified, the method returns before the count is complete.
        /// </summary>
        public static void CalculateUpdateCounts()
        {
            InitiateCalculation(TimeSpan.Zero, false);
        }

        /// <summary>
        /// Calculate the initial counts of all nodes in the initial state.  If it does not complete
        /// within the time specified, the method returns before the count is complete.
        /// </summary>
        private static void CalculateInitialCounts(TimeSpan wait)
        {
            InitiateCalculation(wait, true);
        }

        /// <summary>
        /// Initiate a calculation for either an initial count or update count
        /// </summary>
        private static void InitiateCalculation(TimeSpan wait, bool initial)
        {
            // Since we need to register an operation with the ApplicationBusyManager, we've got to start our work from the UI thread.
            if (Program.ExecutionMode.IsUIDisplayed && Program.MainForm.InvokeRequired)
            {
                Program.MainForm.BeginInvoke((MethodInvoker) delegate { InitiateCalculation(wait, initial); });
                return;
            }

            // If we've entered a connection sensitive scope since we were BeginInvoke'd, then this will just have to wait for later
            if (ConnectionSensitiveScope.IsActive)
            {
                return;
            }
            // Already calculating
            if (!calculatingEvent.WaitOne(TimeSpan.Zero, false))
            {
                return;
            }

            ApplicationBusyToken operationToken;
            if (!ApplicationBusyManager.TryOperationStarting("updating filters", out operationToken))
            {
                return;
            }

            // Ensure we are starting over
            calculatingEvent.Reset();

            // Queue the work to a background thread
            ThreadPool.QueueUserWorkItem(
                ExceptionMonitor.WrapWorkItem(InitiateCalculationThread), 
                new object[] { initial, operationToken });

            // Wait for it to finish.  It's ok if it doesnt.
            calculatingEvent.WaitOne(wait, false);
        }

        /// <summary>
        /// Calculate the initial filter counts on a background thread
        /// </summary>
        private static void InitiateCalculationThread(object state)
        {
            bool initial = (bool) ((object[]) state)[0];
            ApplicationBusyToken token = (ApplicationBusyToken) ((object[]) state)[1];

            // Get a connection that will not timeout
            using (SqlConnection noTimeoutSqlConnection = SqlSession.Current.OpenConnection(0))
            {
                // Create a new connection
                using (SqlAdapter adapter = new SqlAdapter(noTimeoutSqlConnection))
                {
                    // adapter.LogInfoMessages = true;

                    // The calculation procedures bail out as soon as they hit a time threshold - but only at certain checkpoints.  So if
                    // a single update calculation took 1 minute - then the command would take a full minute.  So we need to make sure and
                    // give this plenty of time.
                    adapter.CommandTimeOut = 0;

                    if (adapter.InSystemTransaction)
                    {
                        // If there is no way around this, then wrap it in a TransactionScope with Suppress.
                        throw new InvalidOperationException("This cannot be within a transaction.");
                    }

                    log.DebugFormat("Begin {0} filter counts", initial ? "initial" : "update");

                    SqlAdapterRetry<SqlException> sqlAppResourceLockExceptionRetry =
                        new SqlAdapterRetry<SqlException>(5, -5, "FilterContentManager.InitiateCalculationThread");

                    if (initial)
                    {
                        sqlAppResourceLockExceptionRetry.ExecuteWithRetry(() => 
                            ActionProcedures.CalculateInitialFilterCounts(adapter)
                            );
                    }
                    else
                    {
                        sqlAppResourceLockExceptionRetry.ExecuteWithRetry(() => 
                            ActionProcedures.CalculateUpdateFilterCounts(adapter)
                            );
                    }

                    log.DebugFormat("Complete {0} filter counts", initial ? "initial" : "update");
                }
            }

            calculatingEvent.Set();

            ApplicationBusyManager.OperationComplete(token);
        }

        /// <summary>
        /// Delete any filter counts that have been abandoned from the database
        /// </summary>
        public static void DeleteAbandonedFilterCounts()
        {
            log.InfoFormat("Deleting abandoned filter counts....");
            
            // Get a connection that will not timeout
            using (SqlConnection noTimeoutSqlConnection = SqlSession.Current.OpenConnection(0))
            {
                // Create a new connection
                using (SqlAdapter adapter = new SqlAdapter(noTimeoutSqlConnection))
                {
                    // This needs to run as long as it has work to do
                    adapter.CommandTimeOut = 0;

                    try
                    {
                        ActionProcedures.DeleteAbandonedFilterCounts(adapter);
                    }
                    catch (SqlException ex)
                    {
                        // The constraint exception happens rarely, but we don't want it to crash ShipWorks since we can just 
                        // try to delete the filter counts again later.
                        if (UtilityFunctions.IsConstraintException(ex) || UtilityFunctions.IsDeadlockException(ex))
                        {
                            log.Warn("Could not delete abandoned filter counts", ex);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check to see if any date filters need regenerated due to the date changing, and regenerate if necessary.
        /// </summary>
        public static void CheckRelativeDateFilters()
        {
            DateTime serverDate = SqlSession.Current.GetLocalDate().Date;

            // See if we have to update date filters
            if (dateFiltersLastUpdated != serverDate)
            {
                try
                {
                    // Make sure we have the most up-to-date date they were updated
                    SystemData.CheckForChangesNeeded();
                    dateFiltersLastUpdated = SystemData.Fetch().DateFiltersLastUpdate.Date;
                }
                catch (ORMEntityOutOfSyncException ex)
                {
                    // Fix for FogBugz #267990: Since we just wait until next run when the filter layout context is dirty,
                    // we should be able to do the same thing here
                    log.Warn("SystemData out of sync while checking relative date filters", ex);
                    return;
                }
            }

            // See if we have to update date filters
            if (dateFiltersLastUpdated != serverDate)
            {
                // Can't do it while the layout is dirty - we might miss one.  The next time around, it will have been updated, and
                // we'll probably do it then.
                if (FilterLayoutContext.Current.IsLayoutDirty())
                {
                    return;
                }

                log.InfoFormat("Regenerating date filters ({0}, {1})", dateFiltersLastUpdated, serverDate);

                try
                {
                    using (SqlAppResourceLock dateLock = new SqlAppResourceLock("UpdateDateFilters"))
                    {
                        ExistingConnectionScope.ExecuteWithAdapter(adapter =>
                        {
                            // Set the new date to the current date
                            SystemDataEntity systemData = new SystemDataEntity(true) { IsNew = false };
                            systemData.DateFiltersLastUpdate = serverDate;
                            adapter.SaveEntity(systemData);

                            // Regenerate any date filters based on relative dates
                            FilterLayoutContext.Current.RegenerateDateFilters(adapter);

                            adapter.Commit();

                            // Update our local value
                            dateFiltersLastUpdated = serverDate;
                        });
                    }
                }
                catch (SqlAppResourceLockException)
                {
                    log.InfoFormat("Could not get lock to update filters - they must be working on somewhere else.");
                }
            }
        }

        /// <summary>
        /// Prepares the given node for having its full counts taken.
        /// </summary>
        public static FilterNodeContentEntity CreateNewFilterContent(IFilterContentSqlGenerator sqlProvider, SqlAdapter adapter)
        {
            if (sqlProvider == null)
            {
                throw new ArgumentNullException("sqlProvider");
            }

            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            FilterNodeContentEntity content = new FilterNodeContentEntity();
            content.Cost = 0;
            content.Count = 0;
            content.Status = (int) FilterCountStatus.NeedsInitialCount;
            content.CountVersion = 0;
            content.ColumnMask = new byte[0];
            content.JoinMask = 0;

            // We need the count ID before we can fill these in
            content.InitialCalculation = "";
            content.UpdateCalculation = "";

            // Save it, creates the ID
            adapter.SaveEntity(content, false, false);
            content.Fields.State = EntityState.Fetched;

            FilterSqlResult filterSql = sqlProvider.GenerateSql(content.FilterNodeContentID);

            // Update the calculations
            content.InitialCalculation = filterSql.InitialSql;
            content.UpdateCalculation = filterSql.UpdateSql;
            content.ColumnMask = filterSql.ColumnMask;
            content.JoinMask = filterSql.JoinMask;

            // Save it again
            adapter.SaveEntity(content, false, false);
            content.Fields.State = EntityState.Fetched;

            return content;
        }
    }
}
