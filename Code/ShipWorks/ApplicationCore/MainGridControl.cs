using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Forms;
using Autofac;
using ComponentFactory.Krypton.Toolkit;
using Divelements.SandGrid;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Data;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Grid;
using ShipWorks.Filters.Search;
using ShipWorks.Properties;
using ShipWorks.UI.Controls.Design;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// The primary grid control which contains the search header and the grid
    /// </summary>
    public partial class MainGridControl : UserControl
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MainGridControl));

        // Active target.  Or last target, if there is no active node

        // The grids
        readonly Dictionary<FilterTarget, FilterEntityGrid> entityGrids =
            new Dictionary<FilterTarget, FilterEntityGrid>();

        // Text that is displayed when a search is in progress
        readonly string searchingText;

        /// <summary>
        /// Raised when the selection in the grid changes.  Does not get raised when the selection changes due to changing the current filter.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Raised when the grid sort changes.  Does not get raised when the selection changes due to changing the current filter.
        /// </summary>
        public event EventHandler SortChanged;

        /// <summary>
        /// Raised when the delete key is pressed
        /// </summary>
        public event EventHandler DeleteKeyPressed;

        /// <summary>
        /// Raised when a row is activated (double-clicked, or Enter is pressed)
        /// </summary>
        public event GridRowEventHandler RowActivated;

        /// <summary>
        /// Raised when the IsSearchActive property is changed
        /// </summary>
        public event EventHandler SearchActiveChanged;

        /// <summary>
        /// Raised when the search query changes
        /// </summary>
        public event EventHandler SearchQueryChanged;

        // Indicates if the user is currently searching
        SearchProvider searchProvider;

        // Indicates if advanced search has been initiated during the current search round
        bool initiatedAdvanced;

        // Cached list of selected store keys.  So if it's asked for more than once and the selection hasn't changed,
        // we don't have to refigure it out

        // Observable subscriptions
        private IDisposable subscriptions;

        List<long> selectedStoreKeys;

        // Keeps track of the latest search being via barcode or not
        private bool isBarcodeSearch = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);

            searchingText = kryptonHeader.Values.Description;
        }

        /// <summary>
        /// Loading
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            // For some reason hiding\showing ShipWorks in the task tray causes this to be called multiple times.  So
            // we make sure we perform the actions only once.
            if (entityGrids.Count == 0)
            {
                AdvancedSearchVisible = false;

                searchBox.GotFocus += OnSearchBoxFocusChange;
                searchBox.LostFocus += OnSearchBoxFocusChange;
            }

            // Register any IMainGridControlPipelines
            subscriptions = new CompositeDisposable(
                IoC.UnsafeGlobalLifetimeScope.Resolve<IEnumerable<IMainGridControlPipeline>>().Select(p => p.Register(this)));
        }

        /// <summary>
        /// Action for adding Search box Text change event
        /// </summary>
        public Action<EventHandler> SearchTextChangedAdd => h =>
        {
            searchBox.TextChanged += h;
        };

        /// <summary>
        /// Action for removing Search box Text change event
        /// </summary>
        public Action<EventHandler> SearchTextChangedRemove => h =>
        {
            searchBox.TextChanged -= h;
        };

        /// <summary>
        /// Action for adding filter editor definition edited event
        /// </summary>
        public Action<EventHandler> FilterEditorDefinitionEditedAdd => h =>
        {
            filterEditor.DefinitionEdited += h;
        };

        /// <summary>
        /// Action for removing filter editor definition edited event
        /// </summary>
        public Action<EventHandler> FilterEditorDefinitionEditedRemove => h =>
        {
            filterEditor.DefinitionEdited -= h;
        };

        /// <summary>
        /// Initialization
        /// </summary>
        public void InitializeForTarget(FilterTarget filterTarget,
            ContextMenuStrip orderMenu,
            ToolStripMenuItem copyMenu)
        {
            if (entityGrids.ContainsKey(filterTarget))
            {
                throw new InvalidOperationException("Already initialized for target {0}.  Use Reset first if you need to reinitialize.");
            }

            entityGrids[filterTarget] = CreateGrid(filterTarget);
            entityGrids[filterTarget].ContextMenuStrip = orderMenu;

            if (copyMenu != null)
            {
                copyMenu.DropDownItems.Clear();
                copyMenu.DropDownItems.AddRange(entityGrids[filterTarget].CreateCopyMenuItems(true));
            }

            ApplyDisplaySettings();

            if (filterTarget == ActiveFilterTarget)
            {
                SetupForTarget(filterTarget);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                subscriptions?.Dispose();

                Reset();

                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Apply the current user's display settings to the grid.
        /// </summary>
        public void ApplyDisplaySettings()
        {
            foreach (KeyValuePair<FilterTarget, FilterEntityGrid> pair in entityGrids)
            {
                pair.Value.Renderer = AppearanceHelper.CreateSandGridRenderer(pair.Key);
            }
        }

        /// <summary>
        /// The grid that is currently displayed
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private FilterEntityGrid ActiveGrid
        {
            get
            {
                FilterEntityGrid grid;
                if (entityGrids.TryGetValue(ActiveFilterTarget, out grid))
                {
                    return grid;
                }

                return null;
            }
        }

        /// <summary>
        /// Sets the current filter used
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FilterNodeEntity ActiveFilterNode
        {
            get
            {
                if (DesignModeDetector.IsDesignerHosted())
                {
                    return null;
                }

                return ActiveGrid.ActiveFilterNode;
            }
            set
            {
                if (value != null)
                {
                    FilterTarget newTarget = (FilterTarget) value.Filter.FilterTarget;
                    if (newTarget != ActiveFilterTarget)
                    {
                        ActiveFilterTarget = newTarget;

                        SetupForTarget(ActiveFilterTarget);
                    }
                }

                if (IsSearchActive)
                {
                    EndSearch();
                }
                else if (AdvancedSearchVisible)
                {
                    // Advanced search isn't active, its just visible with no Filter Definition in it. (If there was, the IsSearchActive test would have been true)
                    // Close the Advanced Search, which will coincide with the functionality that ends the search if it was active
                    AdvancedSearchVisible = false;
                }

                if (ActiveGrid != null)
                {
                    ActiveGrid.ActiveFilterNode = value;

                    UpdateHeaderContent();
                }
                else
                {
                    if (value != null)
                    {
                        throw new InvalidOperationException(
                            "Cannot update ActiveFilterNode when there is no active grid.");
                    }
                }
            }
        }

        /// <summary>
        /// The FilterTarget the grid control is configured to display
        /// </summary>
        [Browsable(false)]
        public FilterTarget ActiveFilterTarget { get; private set; } = FilterTarget.Orders;

        /// <summary>
        /// The DetailViewSettings currently in use in the active grid.
        /// </summary>
        [Browsable(false)]
        public DetailViewSettings ActiveDetailViewSettings => ActiveGrid?.DetailViewSettings;

        /// <summary>
        /// The total number of rows being displayed in the grid
        /// </summary>
        [Browsable(false)]
        public int TotalCount
        {
            get { return ActiveGrid?.Rows.Count ?? 0; }
        }

        /// <summary>
        /// The current grid selection
        /// </summary>
        [Browsable(false)]
        public IGridSelection Selection
        {
            get { return ActiveGrid?.Selection ?? new StaticGridSelection(); }
        }

        /// <summary>
        /// A list of distinct store keys for selected orders.  Only valid if there are orders selected.
        /// </summary>
        public List<long> SelectedStoreKeys
        {
            get
            {
                if (ActiveFilterTarget != FilterTarget.Orders)
                {
                    throw new InvalidOperationException("This property is only valid for order selection.");
                }

                if (selectedStoreKeys == null)
                {
                    selectedStoreKeys =
                        ActiveGrid?.Selection.Keys.Select(orderID => DataProvider.GetOrderHeader(orderID).StoreID)
                            .Distinct()
                            .ToList() ?? new List<long>();
                }

                return selectedStoreKeys;
            }
        }

        /// <summary>
        /// Synchronize the grids virtual row count with the current filter node count
        /// </summary>
        public void UpdateFiltering()
        {
            ActiveGrid.UpdateGridRows();

            AutoSelectSingleRow();

            UpdateHeaderContent();
        }

        /// <summary>
            // Auto select the row when doing a filter search and there is only 1 result
        /// </summary>
        private void AutoSelectSingleRow()
        {
            try
            {
                if (ActiveFilterNode?.Purpose == (int) FilterNodePurpose.Search && ActiveGrid?.Selection.Count == 0)
                {
                    bool autoPrintOn = UserSession.User.Settings.SingleScanSettings == (int) SingleScanSettings.AutoPrint;
                    long activeFilterNodeContentId = ActiveFilterNode.FilterNodeContentID;

                    if (autoPrintOn && isBarcodeSearch)
                    {
                        long? orderId = FilterContentManager.GetMostRecentOrderID(activeFilterNodeContentId);
                        if (orderId.HasValue)
                        {
                            bool entityInGrid = ActiveGrid.Rows?.Cast<PagedEntityGrid.PagedEntityGridRow>()
                                                    .Any(row => ActiveGrid.GetRowEntityID(row) == orderId) ?? false;
                            if (entityInGrid)
                            {
                                ActiveGrid.SelectRows(new[] {orderId.Value});
                            }
                        }
                    }
                    else
                    {
                        GridRow firstRow = ActiveGrid.Rows?.Cast<GridRow>().FirstOrDefault();
                        if (firstRow != null)
                        {
                            firstRow.Selected = true;
                        }
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                log.Error("AutoSelectSingleRow: Exception thrown when attempting to auto-select", ex);
            }
        }

        /// <summary>
        /// Forces the grid to reload its data, even if there is no change to the current filter detected.
        /// </summary>
        public void ReloadFiltering()
        {
            // Added this check when MainForm started calling this in OnActivated - which can happen all the time, and definitely when there isn't even an ActiveGrid
            if (ActiveGrid != null)
            {
                // This forces the data to be reloaded, even if the filtering has not changed.  This can help to show updated
                // data before the counts have been retaken after a change.
                ActiveGrid.ReloadGridRows();

                UpdateHeaderContent();
            }
        }

        /// <summary>
        /// Clear the current contents of the grid, and do not save any changed column settings.
        /// </summary>
        public void Reset()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(Reset));
                return;
            }

            ActiveGrid?.Clear();

            if (IsSearchActive)
            {
                EndSearch();
            }
            else
            {
                AdvancedSearchVisible = false;
                UpdateSearchBox();
            }

            selectedStoreKeys = null;

            foreach (KeyValuePair<FilterTarget, FilterEntityGrid> pair in entityGrids)
            {
                FilterEntityGrid grid = pair.Value;

                gridPanel.Controls.Remove(grid);
                grid.Dispose();
            }

            entityGrids.Clear();

            ActiveControl = gridPanel;
            gridPanel.Focus();
        }

        /// <summary>
        /// Reload the grid columns with the latest from the GridLayout
        /// </summary>
        public void ReloadGridColumns()
        {
            if (ActiveGrid.ActiveFilterNode != null)
            {
                ActiveGrid.ReloadColumns();
            }
        }

        /// <summary>
        /// Save the current state of the grid columns
        /// </summary>
        public void SaveGridColumnState()
        {
            if (ActiveGrid.ActiveFilterNode != null)
            {
                ActiveGrid.SaveColumns();
            }
        }

        /// <summary>
        /// Indicates if the grid is currently displaying search results
        /// </summary>
        [Browsable(false)]
        public bool IsSearchActive => searchProvider != null;

        /// <summary>
        /// Cancel the search mode.  IsSearchActive must be true, or this will throw an exception.
        /// </summary>
        public void CancelSearch()
        {
            if (IsSearchActive)
            {
                EndSearch();
            }
        }

        /// <summary>
        /// Create a grid for the given filter target
        /// </summary>
        private FilterEntityGrid CreateGrid(FilterTarget target)
        {
            FilterEntityGrid grid = new FilterEntityGrid(target);
            grid.BorderStyle = BorderStyle.None;

            grid.CheckBoxes = true;
            grid.Visible = false;
            grid.Dock = DockStyle.Fill;
            grid.Parent = gridPanel;
            grid.StretchPrimaryGrid = false;

            grid.ColumnsReordered += OnColumnsReordered;
            grid.SelectionChanged += OnGridSelectionChanged;
            grid.SortChanged += OnGridSortChanged;
            grid.KeyDown += OnGridKeyDown;

            grid.RowActivated += OnGridRowActivated;

            return grid;
        }

        /// <summary>
        /// Someone reordered the grid columns
        /// </summary>
        private void OnColumnsReordered(object sender, EventArgs e)
        {
            FilterEntityGrid grid = sender as FilterEntityGrid;
            if (grid != null)
            {
                grid.SaveColumns();
                grid.ReloadColumns();
            }
        }

        /// <summary>
        /// Setup the grid display based on the given target
        /// </summary>
        private void SetupForTarget(FilterTarget target)
        {
            entityGrids[target].Visible = true;

            // Clear and hide all the non active grids
            foreach (KeyValuePair<FilterTarget, FilterEntityGrid> pair in entityGrids)
            {
                if (pair.Key != target)
                {
                    pair.Value.Visible = false;
                    pair.Value.ActiveFilterNode = null;
                }
            }
        }

        /// <summary>
        /// Update the header control
        /// </summary>
        private void UpdateHeaderContent()
        {
            if (IsSearchActive)
            {
                kryptonHeader.Values.Heading = string.Format("Search {0}", EnumHelper.GetDescription(ActiveFilterTarget));
                kryptonHeader.Values.Image = Resources.view;

                kryptonHeader.Values.Description = searchProvider.IsSearching ? searchingText : "";
                pictureSearchHourglass.Visible = searchProvider.IsSearching;

                if (searchProvider.IsSearching)
                {
                    ActiveGrid.OverrideEmptyText = "Searching...";
                }
                else
                {
                    // I don't like that we are getting BasicSearchText here because it may not be the text we searched.
                    // If the user scans an orderid barcode, we put the order number in the search box. That should
                    // still be ok because we are really only using this text to see if there is something in it.
                    string basicSearchText = GetBasicSearchText();
                    if (!AdvancedSearchResultsActive && basicSearchText.Length == 0)
                    {
                        // Has to be space... empty is considered no override at all
                        ActiveGrid.OverrideEmptyText = " ";
                    }
                    else if (AdvancedSearchResultsActive && GetSearchDefinition(basicSearchText) == null)
                    {
                        ActiveGrid.OverrideEmptyText = "Some of the values entered in the search condition are not valid.";
                    }
                    else
                    {
                        ActiveGrid.OverrideEmptyText = string.Format("No matching {0} were found.", EnumHelper.GetDescription(ActiveFilterTarget).ToLowerInvariant());
                    }
                }
            }
            else
            {
                kryptonHeader.Values.Description = "";
                pictureSearchHourglass.Visible = false;

                ActiveGrid.OverrideEmptyText = "";

                if (ActiveGrid.ActiveFilterNode == null)
                {
                    kryptonHeader.Values.Heading = "";
                }
                else
                {
                    kryptonHeader.Values.Heading = ActiveGrid.ActiveFilterNode.Filter.Name;
                }

                kryptonHeader.Values.Image = FilterHelper.GetFilterImage(ActiveFilterTarget);
            }

            UpdateHeaderColors();
            UpdateSearchBox();

            selectedStoreKeys = null;
        }

        /// <summary>
        /// Update the coloring of the header background.  Used to help indicate if search is active.
        /// </summary>
        private void UpdateHeaderColors()
        {
            List<PaletteBack> backgrounds = new List<PaletteBack> { kryptonHeader.StateCommon.Back, kryptonGroup.StateCommon.Back, kryptonHeaderSearchContainer.StateCommon.Back };

            // Any time the user is even thinking about searching, its orange
            if (IsSearchActive || searchBox.TextBox.Focused || AdvancedSearchVisible)
            {
                backgrounds.ForEach(b => { b.Color1 = Color.FromArgb(255, 255, 220); b.Color2 = Color.FromArgb(247, 192, 92); });
            }
            else
            {
                backgrounds.ForEach(b => { b.Color1 = Color.Empty; b.Color2 = Color.Empty; });
            }
        }

        /// <summary>
        /// Update the search box control to show the correct state
        /// </summary>
        private void UpdateSearchBox()
        {
            buttonEndSearch.Enabled = IsSearchActive ? ButtonEnabled.True : ButtonEnabled.False;

            if (IsSearchActive)
            {
                buttonEndSearch.Image = searchProvider.IsSearching ? Resources.stop_small : Resources.clear_small;
                searchBox.WaterText = AdvancedSearchResultsActive ? "Search these results" : "";
            }
            else
            {
                searchBox.WaterText = string.Format("Search All {0}", EnumHelper.GetDescription(ActiveFilterTarget));
            }
        }

        /// <summary>
        /// The selection changed in the grid.
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PagedEntityGrid grid = (PagedEntityGrid) sender;
            if (!grid.Visible)
            {
                log.InfoFormat("Grid selection changed while not visible. ({0}, {1})", grid.Rows.Count, grid.Selection.Count);
                return;
            }

            RaiseSelectionChanged();
        }

        /// <summary>
        /// Raise the event to notify that the selection and content of the grid has changed
        /// </summary>
        private void RaiseSelectionChanged()
        {
            selectedStoreKeys = null;

            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The sort changed in the grid
        /// </summary>
        void OnGridSortChanged(object sender, GridEventArgs e)
        {
            PagedEntityGrid grid = (PagedEntityGrid) sender;
            if (!grid.Visible)
            {
                log.InfoFormat("Grid selection changed while not visible. ({0}, {1})", grid.Rows.Count, grid.Selection.Count);
                return;
            }

            RaiseSortChanged();
        }

        /// <summary>
        /// Raise the event to notify listeners that the sort has changed
        /// </summary>
        private void RaiseSortChanged()
        {
            SortChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Key was pressed in the active grid
        /// </summary>
        void OnGridKeyDown(object sender, KeyEventArgs e)
        {
            // This used to be a bitwise comparison, but the MSDN docs discourage that: http://msdn.microsoft.com/en-us/library/system.windows.forms.keys(v=vs.110).aspx
            // We had a situation where a customer's volume keys were triggering the delete, and the period key would cause it as well.
            if (e.KeyCode == Keys.Delete)
            {
                DeleteKeyPressed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Row was double-clicked or hit enter on
        /// </summary>
        void OnGridRowActivated(object sender, GridRowEventArgs e)
        {
            RowActivated?.Invoke(this, e);
        }

        /// <summary>
        /// Toggle advanced search
        /// </summary>
        private void OnAdvancedSearch(object sender, EventArgs e)
        {
            bool wasActive = AdvancedSearchResultsActive;

            // Need to initialize the editor before its shown
            if (!AdvancedSearchVisible && !initiatedAdvanced)
            {
                filterEditor.LoadDefinition(new FilterDefinition(ActiveFilterTarget));
            }

            AdvancedSearchVisible = !AdvancedSearchVisible;

            if (AdvancedSearchVisible && IsSearchActive && initiatedAdvanced)
            {
                filterEditor.Focus();
            }

            // Update the search with the current definition
            if (IsSearchActive && (wasActive != AdvancedSearchResultsActive))
            {
                isBarcodeSearch = false;
                searchProvider.Search(GetSearchDefinition(GetBasicSearchText()));
                RaiseSearchQueryChanged();
            }
        }

        /// <summary>
        /// Perform a barcode search
        /// </summary>
        public void PerformBarcodeSearch(string barcode)
        {
            barcode = barcode?.Trim();

            if (barcode.IsNullOrWhiteSpace())
            {
                return;
            }

            // Mark that the search is coming from a barcode scan
            isBarcodeSearch = true;

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ISingleScanOrderShortcut singleScanOrderShortcut = scope.Resolve<ISingleScanOrderShortcut>();
                searchBox.SetTextWithoutTextChangedEvent(singleScanOrderShortcut.GetDisplayText(barcode));

                using (TrackedDurationEvent singleScanSearchTrackedDurationEvent =
                        new TrackedDurationEvent("SingleScan.Search"))
                {
                    singleScanSearchTrackedDurationEvent.AddProperty("SingleScan.Search.IsOrderShorcut",
                        singleScanOrderShortcut.AppliesTo(barcode) ? "Yes" : "No");
                    singleScanSearchTrackedDurationEvent.AddProperty("SingleScan.Search.AdvancedSearch",
                        AdvancedSearchVisible || AdvancedSearchResultsActive ? "Yes" : "No");
                    singleScanSearchTrackedDurationEvent.AddProperty("SingleScan.Search.Barcode", barcode);

                    PerformSearch(barcode);
                }
            }
        }

        /// <summary>
        /// Performs the search.
        /// </summary>
        public void PerformManualSearch()
        {
            // Mark that the search is not coming from a barcode scan
            isBarcodeSearch = false;
            PerformSearch(GetBasicSearchText());
        }

        /// <summary>
        /// Perform the search using the provided text.
        /// </summary>
        private void PerformSearch(string searchText)
        {
            if (AdvancedSearchVisible)
            {
                initiatedAdvanced = true;
            }

            if (searchText.Length > 0 || AdvancedSearchVisible || AdvancedSearchResultsActive)
            {
                if (!IsSearchActive)
                {
                    StartSearch();
                }

                // Update the search with the current definition
                searchProvider.Search(GetSearchDefinition(searchText));

                RaiseSearchQueryChanged();
            }
            else
            {
                if (IsSearchActive)
                {
                    EndSearch();
                }
            }
        }
        
        /// <summary>
        /// Raise the search query changed event
        /// </summary>
        private void RaiseSearchQueryChanged()
        {
            UpdateHeaderContent();

            SearchQueryChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Start the search process
        /// </summary>
        private void StartSearch()
        {
            if (IsSearchActive)
            {
                throw new InvalidOperationException("Already searching.");
            }

            // Create a new search provider
            searchProvider = new SearchProvider(ActiveFilterTarget);
            searchProvider.StatusChanged += OnSearchStatusChanged;

            // Start the search
            ActiveGrid.ActiveFilterNode = searchProvider.SearchResultsNode;

            // Update the search icon to a clear icon
            UpdateHeaderContent();

            // Raise the search changed event
            RaiseSearchActiveChanged();
        }

        /// <summary>
        /// Get the FilterDefinition that defines what the user is currently searching for
        /// </summary>
        private FilterDefinition GetSearchDefinition(string searchText)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                SearchDefinitionProviderFactory definitionProviderFactory = scope.Resolve<SearchDefinitionProviderFactory>();

                ISearchDefinitionProvider definitionProvider;
                if (AdvancedSearchResultsActive && filterEditor.SaveDefinition())
                {
                    definitionProvider = definitionProviderFactory.Create(ActiveFilterTarget, filterEditor.FilterDefinition, isBarcodeSearch);
                }
                else
                {
                    definitionProvider = definitionProviderFactory.Create(ActiveFilterTarget, isBarcodeSearch);
                }

                return definitionProvider.GetDefinition(searchText);
            }
        }

        /// <summary>
        /// The status of the current search operation has changed
        /// </summary>
        void OnSearchStatusChanged(object sender, EventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                try
                {
                    Invoke(new EventHandler(OnSearchStatusChanged), sender, e);
                }
                catch (ObjectDisposedException)
                {
                    // If the user is typing as they close the containing window,
                    // it's possible that we pass the check above and then get disposed
                    log.Debug("Search was trying to access a disposed main grid control");
                }

                return;
            }

            // If null then EndSearch was called in the middle of the event being raised. Rare race-condition, but not a big
            // deal, we just ignore the event.
            if (searchProvider == null)
            {
                return;
            }

            ActiveGrid.ActiveFilterNode = searchProvider.SearchResultsNode;
            UpdateHeaderContent();
            RaiseSelectionChanged();
        }

        /// <summary>
        /// The button to end the active search has been clicked
        /// </summary>
        private void OnEndSearch(object sender, EventArgs e)
        {
            EndSearch();
        }

        /// <summary>
        /// Handle search cancellation via the Escape key
        /// </summary>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                if (IsSearchActive)
                {
                    EndSearch();
                }
                else if (searchBox.ContainsFocus)
                {
                    ActiveGrid.Focus();
                }

                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// End the current search and put the grid back to its normal state
        /// </summary>
        private void EndSearch()
        {
            if (!IsSearchActive)
            {
                throw new InvalidOperationException("Not Searching.");
            }

            ActiveGrid.ActiveFilterNode = null;

            searchProvider.StatusChanged -= OnSearchStatusChanged;
            searchProvider.Cancel();
            searchProvider.Dispose();
            searchProvider = null;

            AdvancedSearchVisible = false;
            initiatedAdvanced = false;

            searchBox.Text = "";

            UpdateHeaderContent();

            RaiseSearchActiveChanged();
        }

        /// <summary>
        /// Raise the searching changed event
        /// </summary>
        private void RaiseSearchActiveChanged()
        {
            SearchActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Indicate if search results from the advanced search are what is currently displayed in the grid.  This could be true even
        /// when AdvancedSearchVisible is false if they had done an advanced search and then just collapsed it.
        /// </summary>
        private bool AdvancedSearchResultsActive => IsSearchActive && initiatedAdvanced;

        /// <summary>
        /// Get the normalized text of the basic search box.
        /// </summary>
        private string GetBasicSearchText()
        {
            return searchBox.Text.Trim().Trim(',');
        }

        /// <summary>
        /// Gets \ sets whether Advanced Search is visible.
        /// </summary>
        private bool AdvancedSearchVisible
        {
            get
            {
                return filterEditor.Visible;
            }
            set
            {
                filterEditor.Visible = value;
                borderAdvanced.Visible = value;

                buttonAdvancedSearch.Type = value ? PaletteButtonSpecStyle.ArrowUp : PaletteButtonSpecStyle.ArrowDown;

                UpdateHeaderColors();
            }
        }

        /// <summary>
        /// As the filter definition changes, its required height will change.  We adjust to fit.
        /// </summary>
        private void OnAdvancedSearchRequiredHeightChanged(object sender, EventArgs e)
        {
            filterEditor.Height = Math.Min(filterEditor.RequiredHeight, Height / 2) + 4;
        }

        /// <summary>
        /// Called when focus enters or leaves the basic search box
        /// </summary>
        private void OnSearchBoxFocusChange(object sender, EventArgs e)
        {
            UpdateHeaderColors();
        }

        /// <summary>
        /// Ensure that search is active and use the specified definition as the advanced search criteria.
        /// </summary>
        public void LoadSearchCriteria(FilterDefinition definition)
        {
            AdvancedSearchVisible = true;
            filterEditor.LoadDefinition(definition);
        }
    }
}
