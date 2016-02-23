using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using System.Diagnostics;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading;
using Divelements.SandGrid.Specialized;
using ShipWorks.Filters;
using ShipWorks.Properties;
using System.Reflection;
using System.Collections;
using Interapptive.Shared;
using System.Data.SqlClient;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.UI;
using ShipWorks.Users;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.Data.Grid.Columns.ValueProviders;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore.Crashes;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.Design;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Grid for displaying entity rows with support for paging, sorting, and filtering
    /// </summary>
    public partial class PagedEntityGrid : EntityGrid
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(PagedEntityGrid));

        IEntityGateway gateway;

        // Even if there would be more, we limit to showing this amount of rows in the grid
        public const int MaxVirtualRowCount = 100000;
        public const int MaxMultiSelectCount = 50;

        // The maximum amount of rows that can be pending a data load at any given time
        const int maxPendingRows = 50;

        // Event that signals when data has been populated
        ManualResetEvent pendingRowsExistEvent = new ManualResetEvent(false);

        // How long to wait before we show "Loading..."
        TimeSpan maxFetchWait = TimeSpan.FromSeconds(.1);

        // Rows that will need to have their data fetched
        LinkedList<DataPendingRow> rowsPendingDataFetch = new LinkedList<DataPendingRow>();

        // Long instead of bool for use with Interlocked
        long threadSafeIsDisposed = 0;

        // Row collection is being cleared
        bool isClearing = false;

        // This is non-null when there is data to be fetched, and keeps the connection from being messed with while we are fetching data.
        ApplicationBusyToken busyToken;

        // When we set the grid to "Loading..." while we are trying to determine the count, we replace the EmptyText.  This lets us know what to set it back to
        int waitingOnRowCountTimers = 0;
        string waitingOnRowCountText = "Loading...";
        string restoreEmptyText = null;
        Color restoreEmptyColor;

        // Deal with selection preservation, especially across large selections.  The virtual selection will differ from the actual grid selection
        // after a sort change, or a refresh of a grid after a data change.  Instead of trying to hunt down and reselect every row of the grid every
        // time that happens, we just maintain our own virtual selection.
        #region Big Explanation
        // It all is the ultimate result of this problem:
        //   Given a shitton of selected rows, with a filter with a ton of content, when a filter is known to have changed, know which rows are selected once the grid is reloaded.
        //   1) You select a whole bunch of stuff.  The grid knows exactly what rows are selected.  No problem here.  The grid knows what row indexes are selected, ShipWorks knows what data objects they correspond to.
        //   2) Someone somewhere changes a data object, which causes the filters to be re-figured within SQL Server.  ShipWorks detects this.  It has to reload the grid.   Some data objects may have been added, some removed, or they could have just been edited.
        //   3) Some of the data objects may now be gone that were selected.  Some that were selected may still be matching the filter, but due to the sort order, an what other stuff was added or removed, they could easily have a different row index in the grid.
        //   4) Due to the fact that data is paged, the row index where some previously selected data object is after reloading may be one that’s not paged in.  Therefore, ShipWorks has no idea what row indexes to tell the grid are selected.
        //   5) ShipWorks does know what data objects should be selected - it just has know way of knowing which rows indexes in the grid they are.
        //   6) Entire what, within the class, i call the “virtual selection”.  Everyone who deals with the grid from the outside sees the virtual selection, which is the data objects that are selected - not the actual grid rows that the grid thinks is selected.
        //   7) As you scroll through the grid, and rows are paged in, ShipWorks determines of the paged in row is one of the data objects in the virtual selection, and tells the grid to select that row index, so that when you are scrolling through, all the correct rows look like they are selected, as they get set to selected just-in-time.
        //   Enter the problem at hand:
        //   8) So there are 1000 items in the virtual selection.  10 of them are on screen, and the grid only knows that those 10 are selected.  You click on some random row that is not selected.  Which should clear the rest of the selection, which the grid does - but! only to the 10 it knows about.
        //   9) ShipWorks needs to know when this is happening, so it can clear the virtual selection as well.
        //   10) This is made worse by the fact that when the grid calls “Clear” on the selection collection, it’s not virtual.  But I can inject my custom ArrayList.  However, the grid optimizes when there are few selected items, and calls ArrayList.RemoveAt on each one, instead of ArrayList.Clear.  So to know if SelectedElementCollection.Clear is really being called, you have to walk the stack from RemoveAt.
        //   11) Once you know that SelectedElementCollection.Clear is being called, it could be called for one of two reasons.  a) To actually do a full clear like I gave an example of.  b) Sometimes to add element X to the section, it caches the selection, clears it, then adds back in cache + X.
        //   In case b) it WOULD have added back in the virtual selection had it known about it, so we have to detect whether its case a (and the virtual selection should be cleared) or b (and the virtual selection should stay)       
        #endregion

        HashSet<long> virtualSelection = new HashSet<long>();
        bool suspendSelectionProcessing = false;
        SelectionInterceptorArrayList selectionInterceptor;

        // The selection we expose to the outside world
        PagedEntityGridSelection selection;

        // Changes to the virtual selection since the last time there was a selection notification event
        HashSet<long> virtualSelectionAdded = new HashSet<long>();
        HashSet<long> virtualSelectionRemoved = new HashSet<long>();

        // We keep a pool or rows because instance creation is expensive.  And recreating 100,000+ rows every time a big filter is selected
        // sucked performance wise.
        GridRow[] gridRowPool = new GridRow[0];

        /// <summary>
        /// Raised when the rows from a gateway have completed loading into the grid.  This is only after the headers have been loaded - all of the invidual row content may not be loaded yet.
        /// </summary>
        public event EventHandler RowLoadingComplete;

        #region class PopulateState

        /// <summary>
        /// For passing state across threads
        /// </summary>
        class DataPendingRow
        {
            PagedEntityGridRow row;
            IEntityGateway gateway;
            EntityBase2 entity;
            bool loadDeffered;

            /// <summary>
            /// Constructor
            /// </summary>
            public DataPendingRow(GridRow row, IEntityGateway gateway)
            {
                this.row = (PagedEntityGridRow) row;
                this.gateway = gateway;
            }

            /// <summary>
            /// The row that needs populated
            /// </summary>
            public PagedEntityGridRow GridRow
            {
                get { return row; }
            }

            /// <summary>
            /// The gateway to use
            /// </summary>
            public IEntityGateway Gateway
            {
                get { return gateway; }
            }

            /// <summary>
            /// The entity that will populate the row data
            /// </summary>
            public EntityBase2 Entity
            {
                get { return entity; }
                set { entity = value; }
            }

            /// <summary>
            /// Indicates if the load was differed, perhaps because the row went off screen and it would have been wasteful to get it.
            /// </summary>
            public bool LoadDeffered
            {
                get { return loadDeffered; }
                set { loadDeffered = value; }
            }
        }

        #endregion

        #region Our Sneaky Custom "AOP" ArrayList's

        /// <summary>
        /// An array list that does nothing when asked to sort.
        /// </summary>
        class UnsortedArrayList : ArrayList
        {
            // All versions of Sort go through here.  Do nothing.
            public override void Sort(int index, int count, IComparer comparer)
            {
                
            }
        }

        /// <summary>
        /// ArrayList that helps us track what SandGrid is doing with the selected element collection, so we can
        /// keep our Virtual Selection in sync.
        /// </summary>
        class SelectionInterceptorArrayList : ArrayList
        {
            PagedEntityGrid owner;

            enum ClearAction
            {
                /// <summary>
                /// SandGrid is not doing a clear
                /// </summary>
                None,

                /// <summary>
                /// SandGrid is clearing the entire selection, meaning we would need
                /// to clear our Virtual Selection too.
                /// </summary>
                CompleteClear,

                /// <summary>
                /// SandGrid is doing a clear, but just as a faster way of updating the selection
                /// with some new items.  All items should not be cleared.  If it knew about our virtual
                /// selection, it would restore it.  But it doesnt.  So we can't clear it.
                /// </summary>
                PsuedoClear
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public SelectionInterceptorArrayList(PagedEntityGrid owner)
            {
                // Every time the SandGrid version changes we want to know so we can make sure the assumption we made in this
                // reflection cold still hold.  You can read the other comments below, and up and the member variable section of the
                // grid above... but basically
                //  1) It is assumed that the only method within SelectedElementCollection that calls its own Clear method is the psuedo-clear ("Revert") one.
                //  2) It is assumed that if the call to SelectedElementCollection.Clear is not called by that psuedo-clear method, its a full clear, and otherwise
                //     its not a full clear.
                if (typeof(SandGrid).Assembly.GetName().Version != new Version("2.2.4.1"))
                {
                    throw new InvalidOperationException("You need to check SandGrid to see that it hasn't changed to break this reflection stuff.");
                }

                this.owner = owner;
            }

            /// <summary>
            /// Determine what type of clear action SandGrid is currently doing.  We have to detect:
            /// 1) That SelectedElementCollection.Clear has been called.  We can't simply rely on the fact
            ///    that ArrayList.Clear is called or not, since SandGrid fucking optimizes the call to SEC.Clear by 
            ///    doing an individual remove of elements when the count is less than 20.  So in the RemoveAt function
            ///    it has to walk the stack to determine if the item is being removed do to a SEC.Clear, or because
            ///    its just individually being removed.
            /// 2) If SelectedElementCollection.Clear has been called, it could be for 2 reasons.  
            ///    a) The first is that the grid is intending that the entire selection by cleared.  This would be like if you 
            ///       clicked (without holding CTRL or SHIFT) on an unselected row.  All the other selection would 
            ///       be first totally cleared.  In this case our entire virtual selection obviously needs cleared as well.
            ///    b) The second is that the grid is adding a removing a single item from the selection, but does it by 
            ///       clearing the selection, and then re-adding everything that needs to still be selected.  When doing a drag
            ///       operation it basically caches the selection at the start, and as you drag keeps doing a clear followed by a 
            ///       re-add of the cached selection.  In this case the cached selection does not know about our virtual selection
            ///       and wouldn't get re-added automatically by the grid.  So we need to preserver our virtual selection in this case.
            ///       
            /// Timing Note: On my machine, it took about 0.0001 to execute this method.  Not too bad.
            /// 
            /// </summary>
            private ClearAction GetClearAction()
            {
                // We can skip the first 2 frames every time
                StackTrace trace = new StackTrace(2);

                // Look back 5 frames in the stack.  That should be enough to find what we need.
                for (int i = 0; i <= 5; i++)
                {
                    StackFrame frame = trace.GetFrame(i);
                    MethodBase method = frame.GetMethod();

                    // If we get to a method of our own before what we're looking for, it means SandGrid isn't the one calling it, and its not a clear
                    if (method.DeclaringType.Assembly == Assembly.GetExecutingAssembly())
                    {
                        return ClearAction.None;
                    }

                    if (method.DeclaringType == typeof(SelectedElementCollection))
                    {
                        if (frame.GetMethod().Name == "Clear")
                        {
                            MethodBase previousMethod = trace.GetFrame(i + 1).GetMethod();

                            // As of the version of SandGrid this was written for, the only method within SelectedElementColleciton 
                            // that calls Clear, is the one that is doing the psuedo-clear (described in 2b above).  So checking that the
                            // next method up in the stack comes from SelectedElementCollection will tell us if that is what's going on.
                            if (previousMethod.DeclaringType == typeof(SelectedElementCollection))
                            {
                                return ClearAction.PsuedoClear;
                            }
                            else
                            {
                                return ClearAction.CompleteClear;
                            }
                        }
                    }
                }

                return ClearAction.None;
            }

            /// <summary>
            /// The ArrayList is being cleared
            /// </summary>
            public override void Clear()
            {
                if (!owner.suspendSelectionProcessing)
                {
                    ClearAction clearAction = GetClearAction();

                    if (clearAction == ClearAction.None)
                    {
                        throw new InvalidOperationException("We can't be in Clear if no ClearAction is happening");
                    }
                    else
                    {
                        ProcessClearAction(clearAction);
                    }
                }

                base.Clear();
            }

            /// <summary>
            /// SandGrid is removing an element from the selection
            /// </summary>
            public override void RemoveAt(int index)
            {
                if (!owner.suspendSelectionProcessing)
                {
                    ClearAction clearAction = GetClearAction();

                    if (clearAction == ClearAction.None)
                    {
                        owner.ClearVirtualSelectionRow((GridRow) this[index]);
                    }
                    else
                    {
                        ProcessClearAction(clearAction);
                    }
                }

                base.RemoveAt(index);
            }

            /// <summary>
            /// Returns the count of elements
            /// </summary>
            public override int Count
            {
                get
                {
                    if (!owner.suspendSelectionProcessing)
                    {
                        // If the grid thinks we have zero items selected, then if its clearing, we would never know it was clearing,
                        // b\c it would think it had nothing to clear.  So we have to detect that case.
                        if (base.Count == 0)
                        {
                            ClearAction clearAction = GetClearAction();

                            if (clearAction != ClearAction.None)
                            {
                                ProcessClearAction(clearAction);
                            }
                        }
                    }

                    return base.Count;
                }
            }

            /// <summary>
            /// Process the given clear action.
            /// </summary>
            private void ProcessClearAction(ClearAction clearAction)
            {
                switch (clearAction)
                {
                    // A complete clear.  So the grid really wants there to be NO selection now.  Clear our virtual selection.
                    case ClearAction.CompleteClear:
                        owner.ClearVirtualSelectionAll(true);
                        break;

                    // A psuedo clear, which means the grid is just trying to clear, and then restore.  So what we'll do 
                    // is just clear from our virtual what we know to be selected now.  Then on our OnSelectionChanged handler
                    // what ends up being selected will all be handled back.
                    case ClearAction.PsuedoClear:
                        owner.ClearVirtualSelectionFromSandGridSelection();
                        break;

                    default:
                        throw new InvalidOperationException("Cannot process non-clearing clear action.");
                }
            }

            /// <summary>
            /// Remove a range of elements
            /// </summary>
            public override void RemoveRange(int index, int count)
            {
                throw new NotImplementedException("Based on the version of SandGrid this was written against, this does not get called.");
            }
        }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PagedEntityGrid()
        {
            InitializeComponent();

            // Have it create instances of our row type
            this.PrimaryGrid.NewRowType = typeof(PagedEntityGridRow);
        }

        /// <summary>
        /// When a check box is checked or unchecked, update the selection.
        /// </summary>
        protected override void OnAfterCheck(GridRowCheckEventArgs e)
        {
            e.Row.Selected = e.Row.Checked;
            base.OnAfterCheck(e);
        }

        /// <summary>
        /// Initializes the grid for use.  Must be called before anything else.
        /// </summary>
        public void InitializeGrid()
        {
            if (selectionInterceptor != null)
            {
                throw new InvalidOperationException("The grid has already been initialized.");
            }

            // Create the selection exposer
            selection = new PagedEntityGridSelection(this);

            // So I hate to use reflection.  But this is kind of a big deal.  When the user clicks a column the grid first
            // goes through all its sorting routines, and then tells us to sort.  Then we sort by using the database and the paging
            // mechanism.  So the grid sorting is a total waste of time, and with a lot of rows can take time (.5 seconds Ive seen) and
            // that's not cool.  So this replaces the ArrayList with our own that does a total no-op for sorting.
            System.Reflection.FieldInfo underlyingRowsListInfo = typeof(CollectionBase).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
            if (underlyingRowsListInfo == null)
            {
                // If this is null the framework likely got upgraded and its different now.
                throw new InvalidOperationException("Could not get private member 'list' from CollectionBase. WTF.");
            }

            // Replace the ArrayList with our own array list that doesnt bother to sort.
            underlyingRowsListInfo.SetValue(Rows, new UnsortedArrayList());

            // Again I hate to use reflection.  But this is necessary to make our magic selection preservation work between filter updates.  Since
            // the virtual selection is sometimes much bigger than what SandGrid thinks is selected, we have to track when SandGrid changes
            // selection stuff very closely so we can keep the virtual selection synced.
            System.Reflection.FieldInfo underlyingSelectionListInfo = typeof(ReadOnlyCollectionBase).GetField("list", BindingFlags.Instance | BindingFlags.NonPublic);
            if (underlyingSelectionListInfo == null)
            {
                // If this is null the framework likely got upgraded and its different now.
                throw new InvalidOperationException("Could not get private member 'list' ReadOnlyCollectionBase CollectionBase. WTF.");
            }

            // Replace the ArrayList with our own array list that tracks its changes
            selectionInterceptor = new SelectionInterceptorArrayList(this);
            underlyingSelectionListInfo.SetValue(base.SelectedElements, selectionInterceptor);

            Thread thread = new Thread(ExceptionMonitor.WrapThread(BackgroundPopulatePending));
            thread.IsBackground = true;
            thread.Name = "GridBackgroundFetch";
            thread.Start();
        }

        #region Row Content

        /// <summary>
        /// Configure the grid with the gateway from which it will get data
        /// </summary>
        public void OpenGateway(IEntityGateway newGateway)
        {
            SetGateway(newGateway, true);
        }

        /// <summary>
        /// Replace the current gateway with the specified gateway, but leave the rows and selection in place.  The rows simply get reset, and so will display
        /// the current data (ghosted) until the new data is loaded.  
        /// </summary>
        protected void UpdateGateway(IEntityGateway newGateway)
        {
            SetGateway(newGateway, false);
        }

        /// <summary>
        /// Set a new gateway for the grid, controlling if its a completely new one (clear) from the old one, or an update (just reset previous)
        /// rows from the previous.
        /// </summary>
        private void SetGateway(IEntityGateway newGateway, bool clear)
        {
            Debug.Assert(!InvokeRequired);

            if (selectionInterceptor == null)
            {
                throw new InvalidOperationException("The grid has not been initialized yet.");
            }

            // If there is a current gateway, we have t clear it out
            if (gateway != null)
            {
                gateway.Close();
            }

            // If clearing, go ahead and clear everything out of the selection (preserving the 'removed' entries)
            // so we'll know if to actually raise the 'SelectionChanged' event
            if (clear)
            {
                ClearVirtualSelectionAll(true);
            }

            // Mark the selection as now being dirty
            selection.OnOrderingChanged();

            // Clear the rows if necessary
            if (clear)
            {
                FastClearRows(false);
            }

            // Set the new gateway,
            gateway = newGateway;

            // Open the new gateway
            if (gateway != null)
            {
                newGateway.Open(GetSortForGateway());

                MonitorGatewayCountForCompletion(clear);
            }
            else
            {
                SetGridRowCount(0);
            }

            // Raise selection change notification, if necessary
            OnSelectionChanged(null);
        }

        /// <summary>
        /// Get the current gateway count and apply it, and then start monitoring for the count total to be completed
        /// </summary>
        private void MonitorGatewayCountForCompletion(bool wasCleared)
        {
            // Put into a local variable so lambda capturing kicks in below
            IEntityGateway currentGateway = gateway;

            // Get the current count
            PagedRowCount count = currentGateway.GetRowCount();

            // If we weren't complete cleared, and we are not done loading, set the initial count to what we last knew it to be.  Trying
            // to make that assumption to avoid flashing.  It will adjust itself correctly either way.
            int initialCount = (count.LoadingComplete || wasCleared) ? count.Count : Rows.Count;

            // Set the initial grid row count
            SetGridRowCount(initialCount);

            // See if we were able to load all the keys upfront
            if (count.LoadingComplete)
            {
                OnGatewayLoadingComplete(count.Count, wasCleared);
            }

            // If the count wasn't complete, we need to watch for it and keep increasing our row count
            else
            {
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = (int) TimeSpan.FromSeconds(0.1).TotalMilliseconds;

                if (waitingOnRowCountTimers++ == 0)
                {
                    restoreEmptyText = EmptyText;
                    restoreEmptyColor = EmptyTextForeColor;
                }

                // Setup the empty text
                EmptyText = WaitingOnRowCountText;
                EmptyTextForeColor = SystemColors.GrayText;

                // Setup the empty text restoration
                timer.Disposed += (object sender, EventArgs e) =>
                    {
                        if (--waitingOnRowCountTimers == 0)
                        {
                            EmptyText = restoreEmptyText;
                            EmptyTextForeColor = restoreEmptyColor;
                        }
                    };

                timer.Tick += (object sender, EventArgs e) =>
                    {
                        // If the gateway has changed since - then we have nothing to do now
                        if (this.gateway != currentGateway)
                        {
                            timer.Dispose();
                        }
                        else
                        {
                            PagedRowCount updatedCount = gateway.GetRowCount();
                            SetGridRowCount(updatedCount.Count);

                            if (updatedCount.LoadingComplete)
                            {
                                OnGatewayLoadingComplete(updatedCount.Count, wasCleared);

                                timer.Dispose();
                            }
                        }
                    };

                timer.Start();
            }
        }

        /// <summary>
        /// Called when the current gateway has been completely loaded. Indicates the number of rows loaded, and if the selection had been previously cleared
        /// </summary>
        protected virtual void OnGatewayLoadingComplete(int rows, bool wasCleared)
        {
            // If we weren't cleared, then the selection was preserved.  However this means there may now be things
            // selected that aren't actually in our key-set anymore.  Here we check this.
            if (!wasCleared)
            {
                CheckVirtualSelectionForRemoved();
            }

            if (RowLoadingComplete != null)
            {
                RowLoadingComplete(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Reloads the contents of the grid by resetting the underlying gateway.  The virtual selection is preserved, but 
        /// the removed tracker is checked.  So if you had changed the query of the gateway, all entities that matched the old gateway
        /// but not the new gateway will still be considered selected (unless they were deleted).
        /// </summary>
        public virtual void ReloadGridRows()
        {
            Debug.Assert(!InvokeRequired);

            IEntityGateway newGateway = (gateway != null) ? gateway.Clone() : null;

            UpdateGateway(newGateway);
        }

        /// <summary>
        /// Update the contents of the existing grid rows.  The current gateway paged\sort is maintained.
        /// </summary>
        public virtual void UpdateGridRows()
        {
            FastResetRows();
        }

        /// <summary>
        /// Create the initial and blank grid rows that will be used as placeholders for when the real data gets populated.
        /// </summary>
        protected void SetGridRowCount(int count)
        {
            // Cap it to our max amount
            count = (int) Math.Min(MaxVirtualRowCount, count);

            // We have to remove some rows.  
            if (Rows.Count > count)
            {
                // If there are less than threshold, then optimize by just removing each individual one that needs removed, without having to clear
                // the entire row collection.  This significantly reduces flicker.
                if (Rows.Count - count < 100)
                {
                    FastResetRows();

                    // Removing these rows, if they are selected, should not trigger the underlying entity to be removed
                    // from the virtual selection.
                    suspendSelectionProcessing = true;

                    for (int i = Rows.Count - 1; i >= count; i--)
                    {
                        Rows.RemoveAt(i);
                    }

                    // Resume selection processing
                    suspendSelectionProcessing = false;
                }
                else
                {
                    FastResetRows(true);
                }
            }
            // If we are adding rows, we don't have to clear, we are just adding. So we just do a reset
            else
            {
                FastResetRows();
            }

            if (Rows.Count < count)
            {
                // See if we have enough rows in our pool
                if (gridRowPool.Length < count)
                {
                    int firstEmptyIndex = gridRowPool.Length;

                    // Make the pool size big enough
                    Array.Resize(ref gridRowPool, count);

                    // Create the new rows
                    for (int i = firstEmptyIndex; i < count; i++)
                    {
                        PagedEntityGridRow row = (PagedEntityGridRow) Activator.CreateInstance(PrimaryGrid.NewRowType);
                        gridRowPool[i] = row;
                    }
                }

                // The new row array for the grid
                GridRow[] rows = new GridRow[count - Rows.Count];

                // Copy all the rows we'll need from the pool.  They'll already be in the reset state from the clear function.
                Array.Copy(gridRowPool, Rows.Count, rows, 0, rows.Length);

                // Add the rows into the grid collection
                Rows.AddRange(rows);
            }
        }

        /// <summary>
        /// Fastest way to reset what the grid feels is selected, and reset the row state of all rows.  The actual rows are left
        /// exactly how they were.  Only the rows currently in the grid are reset. This does not remove
        /// anything from the virtual selection.  Doing that is up to the caller and how the caller wants it to work.
        /// </summary>
        protected void FastResetRows(bool removeRowsFromGrid = false)
        {
            isClearing = true;

            // Clear all selected elements
            suspendSelectionProcessing = true;
            base.SelectedElements.Clear();
            suspendSelectionProcessing = false;

            // Clear the data state of all the rows
            foreach (PagedEntityGridRow row in Rows)
            {
                row.Reset();
            }

            // Remove all the rows from the grid
            if (removeRowsFromGrid)
            {
                Rows.Clear();
            }

            isClearing = false;
        }

        /// <summary>
        /// Fastest way to clear all the rows in the grid so there are no more rows. All pooled rows are reset.  This does not remove
        /// anything from the virtual selection.  Doing that is up to the caller and how the caller wants it to work.
        /// </summary>
        protected void FastClearRows(bool removeRowsFromGrid)
        {
            isClearing = true;

            // Clear all selected elements
            suspendSelectionProcessing = true;
            base.SelectedElements.Clear();
            suspendSelectionProcessing = false;

            // Rest every single row to make sure we are not hanging on to entity references
            foreach (PagedEntityGridRow row in gridRowPool)
            {
                row.Clear();
            }

            // Remove all the rows from the grid
            if (removeRowsFromGrid)
            {
                Rows.Clear();
            }

            isClearing = false;
        }

        #endregion

        #region Common

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (gateway != null)
                {
                    gateway.Close();
                    gateway = null;
                }

                lock (rowsPendingDataFetch)
                {
                    rowsPendingDataFetch.Clear();
                }

                FastClearRows(true);

                Interlocked.Increment(ref threadSafeIsDisposed);

                // This releases the background fetch thread
                pendingRowsExistEvent.Set();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Make sure errors bubble up.
        /// </summary>
        private void OnGridDataError(object sender, GridDataErrorEventArgs e)
        {
            log.ErrorFormat("GridDataError: {0} {1} {2}", e.Column, e.Operation, e.Exception.Message);
            e.ThrowException = true;
        }

        /// <summary>
        /// The configured gateway for providing entity data
        /// </summary>
        public IEntityGateway EntityGateway
        {
            get { return gateway; }
        }

        /// <summary>
        /// The text to display in an empty grid while waiting to final loaded row count value
        /// </summary>
        protected string WaitingOnRowCountText
        {
            get { return waitingOnRowCountText; }
            set { waitingOnRowCountText = value; }
        }

        /// <summary>
        /// A bit of a hack to improve the performance of Rows.Clear when there are a lot of records selected.  This may no longer be
        /// needed if SandGrid releases a fix.
        /// </summary>
        private bool IsClearing
        {
            get { return isClearing; }
        }

        /// <summary>
        /// A thread-safe version of IsDisposed
        /// </summary>
        protected bool ThreadSafeIsDisposed
        {
            get { return Interlocked.Read(ref threadSafeIsDisposed) > 0; }
        }

        #endregion

        #region Selection

        /// <summary>
        /// Overridden to issue warnings of its use
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("When using PagedEntityGrid, you should only be using SelectedKeys or SelectedHeaders.  This property may not be accurate.", true)]
        public new int SelectedElementCount
        {
            get { return base.SelectedElements.Count; }
        }

        /// <summary>
        /// Overridden to issue warnings of its use
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("When using PagedEntityGrid, you should only be using SelectedKeys or SelectedHeaders.  This property may not be accurate.", true)]
        public new SelectedElementCollection SelectedElements
        {
            get { return null; }
        }

        /// <summary>
        /// The current grid selection state
        /// </summary>
        public override IGridSelection Selection
        {
            get
            {
                return selection;
            }
        }

        /// <summary>
        /// Raised when the selection changes
        /// </summary>
        [NDependIgnoreLongMethod]
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            if (suspendSelectionProcessing)
            {
                return;
            }

            // Add in everything that we now know is selected
            foreach (GridRow row in base.SelectedElements)
            {
                long? entityID = GetRowEntityID(row);
                if (entityID != null)
                {
                    AddVirtualSelection(entityID.Value);
                }
            }

            // See if this number of items is restricted
            EditionRestrictionLevel restrictionLevel = EditionRestrictionLevel.None;
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                ILicense license = lifetimeScope.Resolve<ILicenseService>().GetLicenses().FirstOrDefault();

                if (license != null)
                {
                    restrictionLevel = license.CheckRestriction(EditionFeature.SelectionLimit, virtualSelection.Count);
                }

                if (restrictionLevel != EditionRestrictionLevel.None)
                {
                    int maxAllowed = virtualSelection.Count;

                    suspendSelectionProcessing = true;

                    foreach (
                        GridRow extraRow in
                            base.SelectedElements.Cast<PagedEntityGridRow>()
                                .Reverse()
                                .Where(r => r.EntityID != null && virtualSelectionAdded.Contains(r.EntityID.Value))
                                .Take(virtualSelection.Count - maxAllowed))
                    {
                        extraRow.Selected = false;
                        ClearVirtualSelectionRow(extraRow);
                    }

                    suspendSelectionProcessing = false;
                }

                if (virtualSelectionAdded.Count > 0 || virtualSelectionRemoved.Count > 0)
                {
                    virtualSelectionAdded.Clear();
                    virtualSelectionRemoved.Clear();

                    Stopwatch sw = Stopwatch.StartNew();

                    // Create a wait context so that at most, across everyone doing work (mostly panels) trying to fetch counts, we'll wait the specific given amount of time
                    PagedRowCount.StartWaitingForComplete(TimeSpan.FromSeconds(.05));

                    // Raise the event, which will trigger stuff
                    base.OnSelectionChanged(e);

                    log.DebugFormat("SELECTION CHNAGED {0}", sw.Elapsed.TotalSeconds);

                    Update();
                }

                if (restrictionLevel != EditionRestrictionLevel.None)
                {
                    license?.HandleRestriction(EditionFeature.SelectionLimit, virtualSelection.Count, this);
                }
            }
        }

        /// <summary>
        /// Add to the current virtual selection.  
        /// </summary>
        private void AddVirtualSelection(long entityID)
        {
            bool alreadyExists = virtualSelection.Contains(entityID);

            // Update the entity reference regardless
            virtualSelection.Add(entityID);

            // If the removed list has it, then this just undoes that
            if (virtualSelectionRemoved.Contains(entityID))
            {
                Debug.Assert(!alreadyExists);

                virtualSelectionRemoved.Remove(entityID);
            }
            // Otherwise, add it to the added list
            else
            {
                if (!alreadyExists)
                {
                    virtualSelectionAdded.Add(entityID);
                }
            }
        }

        /// <summary>
        /// Remove the given entity ID from the virtual selection
        /// </summary>
        private void RemoveVirtualSelection(long entityID)
        {
            bool existed = virtualSelection.Remove(entityID);

            if (virtualSelectionAdded.Contains(entityID))
            {
                Debug.Assert(existed);

                virtualSelectionAdded.Remove(entityID);
            }
            else
            {
                if (existed)
                {
                    virtualSelectionRemoved.Add(entityID);
                }
            }
        }

        /// <summary>
        /// Check for any entities that think they are selected but have actually now been removed from the filter match.
        /// </summary>
        public void CheckVirtualSelectionForRemoved()
        {
            if (virtualSelection.Count == 0)
            {
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();

            List<long> removedKeys = virtualSelection.Except(gateway.GetOrderedKeys()).ToList();

            // Remove all the selected keys that are no longer in the current gateway
            foreach (long removedKey in removedKeys)
            {
                RemoveVirtualSelection(removedKey);
            }

            log.DebugFormat("Removing virtual selection took: {0}", sw.Elapsed.TotalSeconds);

            // Raise the selection change event if any were removed.  We call the base b\c at this point there could still be some on-screen selected rows
            // in the Reset state, that are not in the virtual selection but havnt been deselected yet (b\c they havnt been loaded yet, so we don't know who they are).
            // If we called our override of OnSelectionChanged, we'd get the updated HeaderInfo for those selected rows, and add them back in the selection.
            // So like for deletes, we would accurately remove the deleted entity from our selection - but whatever entity takes its place when the rows scoot up
            // would become selected.
            if (virtualSelectionRemoved.Count > 0)
            {
                base.OnSelectionChanged(null);

                virtualSelectionRemoved.Clear();
            }
        }

        /// <summary>
        /// Clears the cached selection data.  If preserveRemoved is true, then the virtualSelectionRemoved collection will
        /// maintain the list of the items that are removed as apart of the clear.
        /// </summary>
        protected void ClearVirtualSelectionAll(bool preserveRemoved)
        {
            Debug.Assert(!InvokeRequired);

            // Clear any that were to be added
            virtualSelectionAdded.Clear();

            if (preserveRemoved)
            {
                // Add all the ones we are removing to the removed list.
                foreach (long removedKey in virtualSelection)
                {
                    virtualSelectionRemoved.Add(removedKey);
                }
            }
            else
            {
                virtualSelectionRemoved.Clear();
            }

            virtualSelection.Clear();
        }

        /// <summary>
        /// Remove all the rows currently known to be selected by SandGrid from our virtual selection.  Anything that still needs to be
        /// selected will get added back in the OnSelectionChanged method.
        /// </summary>
        private void ClearVirtualSelectionFromSandGridSelection()
        {
            // We remove all these from the virtual.  All the ones still selected
            // will be added right back in during OnSelectionChanged.
            foreach (GridRow row in base.SelectedElements)
            {
                ClearVirtualSelectionRow(row);
            }
        }

        /// <summary>
        /// Clear the specified row from our virtual selection
        /// </summary>
        private void ClearVirtualSelectionRow(GridRow row)
        {
            long? entityID = GetRowEntityID(row);

            if (entityID != null)
            {
                RemoveVirtualSelection(entityID.Value);
            }
        }

        /// <summary>
        /// Get the EntityID represented by the given row, unless the row count is currently configured incorrectly
        /// </summary>
        private long? GetRowEntityID(GridRow row)
        {
            PagedEntityGridRow pagedRow = (PagedEntityGridRow) row;
            long? entityID = pagedRow.EntityID;

            if (entityID == null)
            {
                entityID = gateway.GetKeyFromRow(row.Index);

                if (entityID != null)
                {
                    pagedRow.LoadRowEntityID(entityID.Value);
                }
            }

            return entityID;
        }

        #endregion

        #region Message Handling

        /// <summary>
        /// The sort has changed
        /// </summary>
        protected override void OnSortChanged(GridEventArgs e)
        {
            if (SuspendSortProcessing)
            {
                return;
            }

            // Clear what the grid thinks to be selected (virtual selection is preserved). No reason to maintain the selection, since after sorting
            // the selected row indexes will be different.
            suspendSelectionProcessing = true;
           
            base.SelectedElements.Clear();

            suspendSelectionProcessing = false;

            // Reload the grid rows given the new gateway
            ReloadGridRows();

            base.OnSortChanged(e);
        }

        /// <summary>
        /// Intercept CTRL-A
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool willSelectAll = false;

            if ((keyData == (Keys.Control | Keys.A)) && !this.EditorActive)
            {
                if (this.FocusedElement != null)
                {
                    willSelectAll = true;
                }
            }

            if (willSelectAll)
            {
                if (Rows.Count > 2000)
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region Sorting

        /// <summary>
        /// Initialize the gateway with the sort the grid is currently configured for
        /// </summary>
        protected virtual SortDefinition GetSortForGateway()
        {
            List<SortClause> sortClauses = null;
            RelationCollection sortRelations = null;

            if (SortColumn != null)
            {
                EntityGridColumn swColumn = SortColumn as EntityGridColumn;
                if (swColumn == null)
                {
                    throw new ArgumentException("column must be of type ShipWorksGridColumn");
                }

                SortOperator sortOperator = SortDirection == ListSortDirection.Ascending ? SortOperator.Ascending : SortOperator.Descending;

                sortClauses = swColumn.Definition.SortProvider.GetDatabaseSortClauses(sortOperator);
                sortRelations = swColumn.Definition.SortProvider.GetDatabaseSortRelations();
            }

            return new SortDefinition(sortClauses, sortRelations);
        }

        #endregion

        #region Virtual Row Population

        /// <summary>
        /// Takes the given row whose load has been differed and gives it a high priority to be loaded
        /// </summary>
        private void PromoteRowToLoading(PagedEntityGridRow gridRow)
        {
            Debug.Assert(gridRow.DataState == PagedDataState.None || gridRow.DataState == PagedDataState.Reset || gridRow.DataState == PagedDataState.LoadDeferred);
            Debug.Assert(!InvokeRequired);

            // If we are in a connection sensitive scope, we can't be fetching data in the background, b\c the connection
            // could be changed out from under us.
            if (ConnectionSensitiveScope.IsActive)
            {
                gridRow.DataState = PagedDataState.LoadDeferred;
                return;
            }

            // If our connection's trashed our we already crashed, no need to go get data
            if (ConnectionMonitor.Status != ConnectionMonitorStatus.Normal || CrashWindow.IsApplicationCrashed)
            {
                gridRow.DataState = PagedDataState.LoadDeferred;
                return;
            }

            TimeSpan timeout = maxFetchWait;

            // If there are any rows pending right now - don't fight for loading time with them, just set the timeout to zero
            // so if the entity exists, we load it immediately
            lock (rowsPendingDataFetch)
            {
                if (rowsPendingDataFetch.Count > 0)
                {
                    timeout = TimeSpan.Zero;
                }
            }

            // See if we can get the entity synchronously without waiting
            EntityBase2 entity = gateway.GetEntityFromRow(gridRow.Index, timeout);

            // If we got it, go ahead and load it
            if (entity != null)
            {
                PopulateRowFromEntity(gridRow, entity);
            }
            // Otherwise, add to the list of rows to fetch in the background
            else
            {
                AppendPendingRow(gridRow);
            }
        }

        /// <summary>
        /// Append a row to the pending list.  Assumes the list is already locked\synchronized in some way for multi threaded safety.
        /// </summary>
        private void AppendPendingRow(PagedEntityGridRow gridRow)
        {
            Debug.Assert(!InvokeRequired);

            lock (rowsPendingDataFetch)
            {
                // If there's already too many pending, defer one
                if (rowsPendingDataFetch.Count >= maxPendingRows)
                {
                    DataPendingRow defferedRow = rowsPendingDataFetch.First.Value;
                    rowsPendingDataFetch.RemoveFirst();

                    defferedRow.GridRow.DataState = PagedDataState.LoadDeferred;
                }

                gridRow.DataState = PagedDataState.Loading;
                rowsPendingDataFetch.AddLast(new DataPendingRow(gridRow, gateway));

                bool alreadySet = pendingRowsExistEvent.WaitOne(0, false);
                if (!alreadySet)
                {
                    Debug.Assert(busyToken == null);
                    busyToken = ApplicationBusyManager.OperationStarting("retrieving data");

                    pendingRowsExistEvent.Set();
                }
            }
        }

        /// <summary>
        /// Background thread for populating pending row data
        /// </summary>
        [NDependIgnoreLongMethod]
        private void BackgroundPopulatePending()
        {
            while (!ThreadSafeIsDisposed)
            {
                // We can't be fetching data, and pending rows have to exist
                pendingRowsExistEvent.WaitOne();

                log.Debug("Starting loop to process pending fetches.");

                DataPendingRow pendingRow;

                // We don't want to invoke the gui for each one, so we do a bunch at a time
                List<DataPendingRow> readyToPopulate = new List<DataPendingRow>();
                bool populatePending = false;

                // Makes sure we don't populate too often
                DateTime timeOfLastPopulate = DateTime.Now;

                do
                {
                    lock (rowsPendingDataFetch)
                    {
                        // If there are any rows pending, pop the top one off
                        if (rowsPendingDataFetch.Count > 0)
                        {
                            pendingRow = rowsPendingDataFetch.First.Value;
                            rowsPendingDataFetch.RemoveFirst();
                        }
                        // Otherwise we're done
                        else
                        {
                            pendingRow = null;
                            pendingRowsExistEvent.Reset();

                            // The only way this can be null right here is if we were released
                            // due to being disposed.
                            if (busyToken != null)
                            {
                                // Let the operation manager know we are done
                                ApplicationBusyManager.OperationComplete(busyToken);
                                busyToken = null;
                            }
                        }
                    }

                    // If we popped a row
                    if (pendingRow != null)
                    {
                        GridRow gridRow = pendingRow.GridRow;
                        InnerGrid grid = gridRow.Grid;

                        // If the row is still in the grid and visible
                        if (grid != null)
                        {
                            int vScroll = grid.SandGrid.VScrollOffset;
                            Rectangle visibleBounds = new Rectangle(gridRow.Bounds.Left, grid.SandGrid.ScrollableViewportBounds.Top + vScroll, gridRow.Bounds.Width, grid.SandGrid.ScrollableViewportBounds.Height);

                            // See if its visible in the grid
                            if (gridRow.Bounds.IntersectsWith(visibleBounds))
                            {
                                // Fetch its entity
                                pendingRow.Entity = pendingRow.Gateway.GetEntityFromRow(gridRow.Index, null);
                            }
                            else
                            {
                                pendingRow.LoadDeffered = true;
                            }

                            // And add it to the list of items to be repopulated on the gui thread
                            lock (readyToPopulate)
                            {
                                readyToPopulate.Add(pendingRow);
                            }
                        }
                    }

                    lock (readyToPopulate)
                    {
                        // If there are any items that are ready
                        if (readyToPopulate.Count > 0 && !populatePending)
                        {
                            if (!ThreadSafeIsDisposed && IsHandleCreated)
                            {
                                populatePending = true;

                                // Have to update the gui row on the gui thread.  Use the MainForm to avoid the race-condition that this grid
                                // went away since we checked Disposed and IsHandleCreated
                                Program.MainForm.BeginInvoke((MethodInvoker) delegate
                                    {
                                        lock (readyToPopulate)
                                        {
                                            foreach (DataPendingRow state in readyToPopulate)
                                            {
                                                // If the row is still in the grid, and the gateway we used is still the valid gateway, populate the row
                                                if (state.GridRow.Grid != null && state.Gateway == gateway)
                                                {
                                                    if (state.LoadDeffered)
                                                    {
                                                        state.GridRow.DataState = PagedDataState.LoadDeferred;
                                                    }
                                                    else
                                                    {
                                                        PopulateRowFromEntity(state.GridRow, state.Entity);
                                                    }
                                                }
                                            }

                                            readyToPopulate.Clear();
                                            populatePending = false;
                                        }
                                    });
                            }
                        }
                    }
                }
                while (pendingRow != null);

                log.Debug("Done processing pending fetches.");
            }

            log.Debug("Background thread terminating due to disposed.");

            pendingRowsExistEvent.Close();
        }

        /// <summary>
        /// Populate the row from the data from the entity
        /// </summary>
        private void PopulateRowFromEntity(PagedEntityGridRow gridRow, EntityBase2 entity)
        {
            Debug.Assert(!InvokeRequired);

            // If we didn't get the entity, that means the count is now bigger than the number of records there
            // really are.
            if (entity == null)
            {
                log.InfoFormat("Gateway returned null indicating entity has been deleted.");
                gridRow.DataState = PagedDataState.Removing;
                return;
            }

            long entityID = (long) entity.Fields.PrimaryKeyFields[0].CurrentValue;

            gridRow.LoadRowEntity(entity);

            suspendSelectionProcessing = true;

            gridRow.Selected = virtualSelection.Contains(entityID);
            
            suspendSelectionProcessing = false;
        }

        #endregion
    }
}
