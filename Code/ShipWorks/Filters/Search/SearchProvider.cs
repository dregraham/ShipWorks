using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using System.Threading;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Diagnostics;
using log4net;
using ShipWorks.Filters.Content.SqlGeneration;
using System.Data.SqlClient;
using ShipWorks.ApplicationCore;
using ShipWorks.SqlServer.Filters;
using System.Transactions;
using Interapptive.Shared;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.Interaction;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using ShipWorks.Users;
using ShipWorks.Data.Adapter.Custom;
using Interapptive.Shared.Data;
using ShipWorks.Data.Administration.Retry;

namespace ShipWorks.Filters.Search
{
    /// <summary>
    /// Used to manage searching and results
    /// </summary>
    public class SearchProvider : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SearchProvider));

        // Type of objects to search
        FilterTarget filterTarget;

        // The node we are using to search on
        volatile FilterNodeEntity searchNode;

        // The ID of the Search table that logs this search as active
        long searchID;

        // The status of the current search has changed
        public event EventHandler StatusChanged;

        // Multi-threaded status tracking
        volatile bool isSearching = false;
        volatile bool isCancelRequested = false;
        object searchFinishingLock = new object();

        // Event that signals that a previous search is complete
        ManualResetEvent searchComplete = new ManualResetEvent(true);

        // The command used to execute the search
        SqlCommand searchCmd;
        object searchCmdLock = new object();

        // Scheduling
        FilterDefinition scheduledDefinition = null;
        bool isScheduled = false;
        object scheduleLock = new object();

        // Used to keep the UI from changing database while we work in the background
        ApplicationBusyToken busyToken = null;

        // Need to keep the Search.Updated field current so other SearchProviders know we are not abandoned and don't delete us.  We
        // store this so we can cancel it later.
        Guid pingIdleWorkID;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchProvider(FilterTarget filterTarget)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired, "Designed to be called from the UI thread.");

            Stopwatch sw = Stopwatch.StartNew();

            this.filterTarget = filterTarget;

            CreateSearchResultsNode();

            pingIdleWorkID = IdleWatcher.RegisterDatabaseDependentWork("PingSearchTable", PingSearchTable, "maintaining search", TimeSpan.FromHours(2));

            log.DebugFormat("@@@@@@ Search Construct: {0}", sw.Elapsed.TotalSeconds);
        }

        /// <summary>
        /// Dispose of the resources required by search
        /// </summary>
        public void Dispose()
        {
            if (searchNode != null)
            {
                Cancel();

                CleanupSearch();

                // Cleanup the event
                searchComplete.Close();
            }
        }

        /// <summary>
        /// The filter node that contains the search results
        /// </summary>
        public FilterNodeEntity SearchResultsNode
        {
            get
            {
                if (searchNode == null)
                {
                    throw new ObjectDisposedException("SearchProvider");
                }

                return searchNode;
            }
        }

        /// <summary>
        /// The filter definition to search for.
        /// </summary>
        public void Search(FilterDefinition definition)
        {
            Debug.Assert(!Program.MainForm.InvokeRequired, "Designed to be called from the UI thread.");

            if (searchNode == null)
            {
                throw new ObjectDisposedException("SearchProvider");
            }

            // If a search is already scheduled, just replace the definition
            // its scheduled to search on.
            lock (scheduleLock)
            {
                if (isScheduled)
                {
                    log.DebugFormat("Search - Scheduler already running, replacing definition.");

                    scheduledDefinition = definition;
                    return;
                }
            }

            lock (searchFinishingLock)
            {
                // If its searching right now, schedule the next search
                if (isSearching)
                {
                    log.DebugFormat("Search - Already searching, starting scheduler.");

                    scheduledDefinition = definition;
                    isScheduled = true;
                    ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(ScheduleThread));

                    return;
                }
            }

            log.DebugFormat("Search - Starting fresh search.");

            // isSearching gets set to false before the event gets set.  Wait here, just in case we
            // are a little early.
            searchComplete.WaitOne();

            Debug.Assert(busyToken == null, "No background operation should be already existant.");

            // Keeps the database from changing while we are working
            busyToken = ApplicationBusyManager.OperationStarting("searching");

            // Start a search
            StartSearch(definition);

            // Search status has changed
            RaiseStatusChanged();
        }

        /// <summary>
        /// Start the search thread, to be searched with the given definition
        /// </summary>
        private void StartSearch(FilterDefinition definition)
        {
            isSearching = true;
            isCancelRequested = false;

            // Reset the event, so waiting works the next time around
            searchComplete.Reset();

            // Queue up the search
            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(SearchThread), definition);
        }

        /// <summary>
        /// Cancel any current work the search is doing.  Does not clear already found results.
        /// </summary>
        public void Cancel()
        {
            Debug.Assert(!Program.MainForm.InvokeRequired, "Designed to be called from the UI thread.");

            if (searchNode == null)
            {
                throw new ObjectDisposedException("SearchProvider");
            }

            log.DebugFormat("Search - Cancel requested by user.");

            // If a search is already scheduled, then the scheduler will have already
            // requested a cancel.  So just cancel the scheduler.
            lock (scheduleLock)
            {
                if (isScheduled)
                {
                    log.DebugFormat("Search - Canceling scheduler.");

                    isScheduled = false;
                }
            }

            CancelCurrentSearch();

            // It is possible that when search finished it thought something was still scheduled, and did
            // not clear the background operation
            isSearching = false;
            if (busyToken != null)
            {
                ApplicationBusyManager.OperationComplete(busyToken);
                busyToken = null;
            }

            RaiseStatusChanged();
        }

        /// <summary>
        /// Cancel the current outstanding search
        /// </summary>
        private void CancelCurrentSearch()
        {
            isCancelRequested = true;

            lock (searchCmdLock)
            {
                if (searchCmd != null)
                {
                    log.DebugFormat("Search - CancelCurrent - Canceling running SQL Command.");

                    searchCmd.Cancel();
                }
            }

            searchComplete.WaitOne();
        }

        /// <summary>
        /// The thread that is used to schedule the next search when we are already searching.
        /// </summary>
        private void ScheduleThread(object state)
        {
            log.DebugFormat("Search - Scheduler - Starting - Canceling current search.");

            CancelCurrentSearch();

            lock (scheduleLock)
            {
                // See if we are still scheduled
                if (isScheduled)
                {
                    isScheduled = false;

                    log.DebugFormat("Search - Scheduler - Starting search.");

                    StartSearch(scheduledDefinition);
                }
                else
                {
                    log.DebugFormat("Search - Scheduler - No longer scheduled.");
                }

                scheduledDefinition = null;
            }

            RaiseStatusChanged();
        }

        /// <summary>
        /// The thread where all the searching occurs
        /// </summary>
        private void SearchThread(object state)
        {
            log.DebugFormat("Search - Search thread starting.");

            try
            {
                if (!isCancelRequested)
                {
                    FilterDefinition definition = (FilterDefinition) state;
                    FilterNodeContentEntity nodeContent;

                    // Create a new filter node content entity
                    nodeContent = CreateSearchFilterNodeContentEntity(definition);

                    lock (searchCmdLock)
                    {
                        searchCmd = SqlCommandProvider.Create(null);
                        searchCmd.CommandText = nodeContent.InitialCalculation.Replace("<SwFilterNodeID />", searchNode.FilterNodeID.ToString());

                        // debugging delay
                        // searchCmd.CommandText = "WAITFOR DELAY '00:00:20'; " + searchCmd.CommandText;
                    }

                    // Execute the search in a SqlAdapterRetry
                    bool searchCompleted = false;
                    SqlAdapterRetry<SqlException> sqlAdapterRetry = new SqlAdapterRetry<SqlException>(5, -5, "SearchProvider.SearchThread ExecuteSearch.");
                    sqlAdapterRetry.ExecuteWithRetry(() =>
                    {
                        searchCompleted = ExecuteSearch();    
                    });

                    lock (searchCmdLock)
                    {
                        searchCmd = null;
                    }

                    if (searchCompleted)
                    {
                        log.DebugFormat("Search - Search thread - search completed, updating FilterNode with new count.");

                        // This gets the Node => Node count association loaded.  The Heartbeat of the MainForm does this too - but we do it now, so that
                        // when the SearchStatus event gets raised at the end of this thread, its ready to go with the MainGridControl updates the grid
                        // in response to the SearchStatus event.  Without this, it may be up to a second (heartbeat rate) before the grid is updated, 
                        // and might be out-of-sync with the status of the search bar.  And that doesn't look right.
                        FilterContentManager.CheckForChanges();
                    }
                }

                lock (searchFinishingLock)
                {
                    // If we are not scheduled for more, then mark the search and operation as done
                    if (!isScheduled)
                    {
                        isSearching = false;

                        ApplicationBusyManager.OperationComplete(busyToken);
                        busyToken = null;
                    }
                }
            }
            // We have to make sure the complete event gets set even if we are crashing, b\c when ShipWorks closes it waits on search to terminate, which waits on that even to be set
            finally
            {
                log.DebugFormat("Search - Search thread completing. (IsSearching {0}, IsCancelled {1})", isSearching, isCancelRequested);
                searchComplete.Set();
            }

            // This has to go after the set.  Otherwise if the UI handles the event, and does an Invoke,
            // we could deadlock if the UI is also wiating on a searchComplete.WaitOne().
            RaiseStatusChanged();
        }

        /// <summary>
        /// Execute the search
        /// </summary>
        /// <returns>True if the search completes, false otherwise.</returns>
        private bool ExecuteSearch()
        {
            bool searchCompleted = false;

            // Execute the search
            if (!isCancelRequested)
            {
                try
                {
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        bool gotLock = false;

                        // Wait in line with other update\initial counts
                        while (!isCancelRequested)
                        {
                            if (ActiveCalculationUtility.AcquireCalculatingLock(con, TimeSpan.FromSeconds(2)))
                            {
                                gotLock = true;
                                break;
                            }

                            log.InfoFormat("Search timed out waiting for calculation lock, will try again...");
                        }

                        try
                        {
                            if (!isCancelRequested)
                            {
                                Stopwatch sw = Stopwatch.StartNew();

                                searchCmd.Connection = con;
                                searchCmd.ExecuteNonQuery();
                                searchCompleted = true;

                                log.DebugFormat("@@@@@ Time to execute search query: {0}", sw.Elapsed.TotalSeconds);
                            }
                        }
                        finally
                        {
                            if (gotLock)
                            {
                                try
                                {
                                    ActiveCalculationUtility.ReleaseCalculatingLock(con);
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Search failed to release calculating lock.", ex);
                                }
                            }
                        }
                    }
                }
                    // Depending on the timing of the cancel, it could throw either SqlException or InvalidOperationException
                catch (SqlException)
                {
                    if (!isCancelRequested)
                    {
                        throw;
                    }
                }
                catch (InvalidOperationException)
                {
                    if (!isCancelRequested)
                    {
                        throw;
                    }
                }
            }

            return searchCompleted;
        }

        /// <summary>
        /// Create a new filter node content entity
        /// </summary>
        private FilterNodeContentEntity CreateSearchFilterNodeContentEntity(FilterDefinition definition)
        {
            FilterNodeContentEntity nodeContent;
            
            // Create a new count and initialize the filter node to do it.  Up to and through the first alpha we didn't apply the count
            // to the node until after the results were calculated.  This provided a very "smooth" search experience where you were always
            // taken directly from one result set to the next.  But, if the search takes a while, then you'd be viewing a result set from
            // the last search until it completed, and this felt pretty wrong.  If the code starting with making a clone is moved back to the area
            // that is "if (searchCompleted)", then the DeleteAbandonedFilterCounts needs to be updated to make sure to not somehow delete
            // the node count that would look abandoned while doing the initial search calculation.
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Create the FilterNodeContent row to hold the results
                nodeContent = CreateFilterNodeContent(definition);

                // Make a clone. Because we are on another thread.  The UI thread could be trying
                // to look at the node ID between setting and saving it if we dont.  It doesnt matter
                // that we have a clone.  The actual object does not matter, only the information.
                FilterNodeEntity clone = new FilterNodeEntity(searchNode.Fields.Clone());
                clone.IsNew = false;
                clone.IgnoreConcurrency = true;
                clone.FilterSequence = SearchManager.GetPlaceholder(filterTarget).FilterSequence;

                // Update the FilterNodeContentID of the node.  This is what points to the new search results.
                clone.FilterNodeContentID = nodeContent.FilterNodeContentID;

                try
                {
                    adapter.SaveEntity(clone, false, false);
                }
                catch (ORMConcurrencyException ex)
                {
                    // Even with the added ping, this was still happening as of 3.7.5.5512. We'll just create a new
                    // filter node and associate the content with it instead.
                    log.Debug("The Search must have been considered abandoned by another instance.  Creating a new filter node", ex);

                    clone.IsNew = true;

                    foreach (IEntityField2 field in clone.Fields)
                    {
                        field.IsChanged = true;
                    }

                    adapter.SaveEntity(clone, true, false);
                }

                // We didn't refetch, so we have to manually set the sync state
                clone.Fields.State = EntityState.Fetched;

                // This is the new search node
                searchNode = clone;

                adapter.Commit();
            }
            return nodeContent;
        }

        /// <summary>
        /// Create the node count that represents the results of the given search definition
        /// </summary>
        private FilterNodeContentEntity CreateFilterNodeContent(FilterDefinition definition)
        {
            FilterNodeContentEntity nodeContent = null;

            // Create the new count
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Create the new node count
                nodeContent = FilterContentManager.CreateNewFilterContent(new SearchSqlGenerator(definition), adapter);

                adapter.Commit();
            }

            return nodeContent;
        }

        /// <summary>
        /// Indicates that search is actively trying to find results
        /// </summary>
        public bool IsSearching
        {
            get { return isSearching; }
        }
        
        /// <summary>
        /// Raises the status changed event
        /// </summary>
        private void RaiseStatusChanged()
        {
            EventHandler handler = StatusChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Create the container rows that the search results will be materialized to.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void CreateSearchResultsNode()
        {
            // A null reference error was being thrown.  Discoverred by Crash Reports.
            // Let's figure out what is null....

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Create an initial empty count
                FilterNodeContentEntity content = new FilterNodeContentEntity();
                content.Status = (int) FilterCountStatus.RunningInitialCount;
                content.InitialCalculation = "";
                content.UpdateCalculation = "";
                content.Cost = 0;
                content.Count = 0;
                content.CountVersion = 0;
                content.ColumnMask = new byte[0];
                content.JoinMask = 0;

                // Create the node that will hold the results
                searchNode = new FilterNodeEntity();
                searchNode.ParentNode = null;
                searchNode.FilterNodeContent = content;
                searchNode.Created = DateTime.UtcNow;
                searchNode.Purpose = (int)FilterNodePurpose.Search;

                // We'll just point to and re-use the placeholder sequence.  We don't actually need the sequence\filter for
                // anything other than that some code references them for such things as determining FilterTarget and the filter
                // name.
                //
                // One of the biggies in doing this though is that we make sure not to do anything that would edit the properties
                // of the placeholder.

                FilterNodeEntity placeholder = SearchManager.GetPlaceholder(filterTarget);
                if (placeholder == null)
                {
                    throw new NullReferenceException("placeholder cannot be null.");
                }

                searchNode.FilterSequence = placeholder.FilterSequence;

                // Save the whole chain
                adapter.SaveAndRefetch(searchNode, true);

                // Record the fact that we are now searching
                SearchEntity search = new SearchEntity();
                search.Started = DateTime.UtcNow;
                search.Pinged = DateTime.UtcNow;
                search.FilterNodeID = searchNode.FilterNodeID;
                search.UserID = UserSession.User.UserID;
                search.ComputerID = UserSession.Computer.ComputerID;
                adapter.SaveEntity(search);

                // Save the ID
                IEntityField2 field = search.Fields[SearchFields.SearchID.FieldIndex];
                if (field == null)
                {
                    throw new NullReferenceException("field cannot be null.");
                }
                searchID = (long) field.CurrentValue;

                adapter.Commit();
            }

            log.InfoFormat("Search - Started search session {0}", searchID);

            // This gets the Node => Node count association loaded
            FilterContentManager.CheckForChanges();
        }

        /// <summary>
        /// Ping the Search table to other SearchProviders know we are still alive and don't try to delete us if the user lets search be open for a while
        /// </summary>
        private void PingSearchTable()
        {
            log.InfoFormat("Pinging search table with {0}", searchID);

            try
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    SearchEntity search = new SearchEntity(searchID);
                    search.IsNew = false;

                    search.Pinged = DateTime.UtcNow;

                    adapter.SaveEntity(search);
                }
            }
            catch (ORMConcurrencyException)
            {
                log.WarnFormat("Failed to ping search due to concurrency - Search must have eneded ({0})", searchID);
            }
        }

        /// <summary>
        /// Delete the node we created to hold the search results
        /// </summary>
        private void CleanupSearch()
        {
            EntityCollection<FilterNodeEntity> nodesUsingSequence = SearchManager.GetPlaceholder(filterTarget).FilterSequence.NodesUsingSequence;

            // Remove all the nodes we created from the placeholder sequence's list of references.  Since we clone the node on each
            // change, the list can end up with a log of duplicates.
            foreach (FilterNodeEntity node in nodesUsingSequence.Where(n => n.FilterNodeID == searchNode.FilterNodeID).ToList())
            {
                node.FilterSequence = null;
            }

            // Stop pinging the search table
            IdleWatcher.CancelRegistration(pingIdleWorkID);

            ThreadPool.QueueUserWorkItem(
                ExceptionMonitor.WrapWorkItem(AsyncCleanupSearch),
                new object[] { searchNode, ApplicationBusyManager.OperationStarting("cleaning up search") });

            searchNode = null;
        }

        /// <summary>
        /// Calculate the initial filter counts on a background thread
        /// </summary>
        private void AsyncCleanupSearch(object state)
        {
            log.InfoFormat("Search - Cleaning up session {0}.", searchID);

            FilterNodeEntity searchNode = (FilterNodeEntity) ((object[]) state)[0];
            ApplicationBusyToken token = (ApplicationBusyToken) ((object[]) state)[1];

            using (SqlAdapter adapter = new SqlAdapter())
            {
                // Delete our search filter node
                try
                {
                    adapter.DeleteEntity(new FilterNodeEntity(searchNode.FilterNodeID) { IgnoreConcurrency = true });
                }
                catch (ORMConcurrencyException ex)
                {
                    log.Warn(string.Format("FilterNode {0} looks like it was already deleted", searchNode.FilterNodeID), ex);
                }
                
                // Delete our search database entry
                adapter.DeleteEntitiesDirectly(typeof(SearchEntity), new RelationPredicateBucket(SearchFields.FilterNodeID == searchNode.FilterNodeID));

                //
                // Delete any search nodes that may have been abandoned due to a crash - do it for any over 24 hours old.  If the search is still actually open, ShipWorks
                // may crash - but that shouldn't happen since each SearchProvider updates the DateTime every 2 hours for as long as it is alive.
                //

                // Get a list of all search nodes that havnt been pinged in over 24 hours.
                SearchCollection abandonedSearch = SearchCollection.Fetch(adapter, SearchFields.Pinged < DateTime.UtcNow.AddDays(-1));

                // Delete all filter nodes that are in that abandoned list
                adapter.DeleteEntitiesDirectly(typeof(FilterNodeEntity),
                    new RelationPredicateBucket(
                        FilterNodeFields.FilterNodeID > 0 &
                        FilterNodeFields.Purpose == (int) FilterNodePurpose.Search &
                        FilterNodeFields.FilterNodeID == abandonedSearch.Select(s => s.FilterNodeID).ToArray()));

                // Now delete the abandoned search nodes too
                adapter.DeleteEntityCollection(abandonedSearch);
            }

            // Delete all abandoned counts.  This will delete all the ones we generated, plus any that were already lingering, but that should be fine.
            FilterContentManager.DeleteAbandonedFilterCounts();

            // Mark the background operation as complete
            ApplicationBusyManager.OperationComplete(token);
        }
    }
}
