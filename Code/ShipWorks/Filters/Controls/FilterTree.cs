using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Divelements.SandGrid;
using Interapptive.Shared.Messaging;
using ShipWorks.Properties;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Filters;
using Divelements.SandGrid.Rendering;
using System.Reflection;
using System.Diagnostics;
using Interapptive.Shared.Utility;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.UI.Utility;
using ShipWorks.Filters.Management;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.UI;
using System.Linq;
using Interapptive.Shared;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;
using ShipWorks.Data;
using ShipWorks.Editions;
using ShipWorks.Filters.Search;

namespace ShipWorks.Filters.Controls
{
    /// <summary>
    /// Control for displaying filters in a hierarchical tree display.
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class FilterTree : UserControl
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FilterTree));

        // Keeps track of which row a node belongs to
        Dictionary<FilterNodeEntity, FilterTreeGridRow> nodeOwnerMap = new Dictionary<FilterNodeEntity, FilterTreeGridRow>();

        // Used to make sure something is always selected. WhitespaceClickBehavior is set to None, but there is a bug (feature?) of the current grid that if you
        // collapse a parent folder when a child is selected, that child selection get's cleared.  If that gets fixed, we can remove 
        FilterTreeGridRow lastSelectedRow;

        // Indiciates if the "My Filters" is always shown, or only shown if it has contents
        bool alwaysShowMyFilters = false;

        // Indiciates if the tree will automatically try to retrieve count values for counts that
        // are in the calculating state.
        bool autoRefreshCalculatingCounts = false;

        // Indicates the search node to be displayed.
        FilterNodeEntity activeSearchNode = null;

        // Scope to show
        FilterScope filterScope = FilterScope.Any;

        // Raised when the selected node changes
        public event EventHandler SelectedFilterNodeChanged;

        // Raised during the rename of filters
        public event FilterNodeRenameEventHandler FilterRenaming;
        public event FilterNodeRenameEventHandler FilterRenamed;

        // Raised to indiciate the delete key is pressed
        public event EventHandler DeleteKeyPressed;

        // The state of the layout
        FilterLayoutContext layoutContext;
        public FilterTarget[] Targets { get; private set; }

        // The quick filter node, if any
        FilterNodeEntity quickFilterNode = null;
        bool quickFilterSelected = false;
        private readonly MessengerToken filterEditedToken;
        private readonly FilterControlDisplayManager quickFilterDisplayManager;

        // Event raised when a drag-drop operation has taken place
        public event FilterDragDropCompleteEventHandler DragDropComplete;

        // Context menu and items
        private ContextMenuStrip contextMenuFilterTree;
        private ToolStripMenuItem menuItemEditFilter;
        private ToolStripSeparator menuItemEditFilterSep;
        private ToolStripMenuItem menuItemNewFilter;
        private ToolStripMenuItem menuItemNewFolder;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripMenuItem menuItemOrganizeFilters;

        // Edition helper for UI updates
        private EditionGuiHelper editionGuiHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterTree()
        {
            InitializeComponent();

            InitializeContextMenu();

            sandGrid.PrimaryGrid.NewRowType = typeof(FilterTreeGridRow);

            ThemedBorderProvider themedBorder = new ThemedBorderProvider(this);

            quickFilterDisplayManager = new FilterControlDisplayManager(quickFilterName, quickFilterCount);

            UpdateQuickFilterDisplay();

            filterEditedToken = Messenger.Current.Handle<FilterNodeEditedMessage>(this, HandleFilterEdited);
        }

        /// <summary>
        /// Initialize the filter tree's context menu
        /// </summary>
        [NDependIgnoreLongMethod]
        private void InitializeContextMenu()
        {
            editionGuiHelper = new ShipWorks.Editions.EditionGuiHelper(this.components);
            contextMenuFilterTree = new System.Windows.Forms.ContextMenuStrip(components);
            menuItemEditFilter = new System.Windows.Forms.ToolStripMenuItem();
            menuItemEditFilterSep = new System.Windows.Forms.ToolStripSeparator();
            menuItemNewFilter = new System.Windows.Forms.ToolStripMenuItem();
            menuItemNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            menuItemOrganizeFilters = new System.Windows.Forms.ToolStripMenuItem();

            ContextMenuStrip = contextMenuFilterTree;

            contextMenuFilterTree.Font = new System.Drawing.Font("Segoe UI", 9F);
            contextMenuFilterTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                menuItemEditFilter,
                menuItemEditFilterSep,
                menuItemNewFilter,
                menuItemNewFolder,
                toolStripSeparator,
                menuItemOrganizeFilters});
            contextMenuFilterTree.Name = "contextMenuCustomerFilterTree";
            contextMenuFilterTree.Size = new System.Drawing.Size(152, 104);
            contextMenuFilterTree.Opening += new System.ComponentModel.CancelEventHandler(OnFilterTreeContextMenuOpening);

            // menuItemEditFilter
            menuItemEditFilter.Image = global::ShipWorks.Properties.Resources.edit16;
            menuItemEditFilter.Name = "menuItemEditFilter";
            menuItemEditFilter.Size = new System.Drawing.Size(151, 22);
            menuItemEditFilter.Text = "Edit";
            menuItemEditFilter.Click += new System.EventHandler(OnEditFilter);

            // menuItemEditFilterSep
            menuItemEditFilterSep.Name = "menuItemEditFilterSep";
            menuItemEditFilterSep.Size = new System.Drawing.Size(148, 6);
            
            // meuItemNewFilter
            menuItemNewFilter.Image = global::ShipWorks.Properties.Resources.filter_add;
            menuItemNewFilter.Name = "meuItemNewFilter";
            menuItemNewFilter.Size = new System.Drawing.Size(151, 22);
            menuItemNewFilter.Text = "New Filter";
            menuItemNewFilter.Click += new System.EventHandler(OnNewFilter);
             
            // menuItemNewFolder
            menuItemNewFolder.Image = global::ShipWorks.Properties.Resources.folderclosed_add;
            menuItemNewFolder.Name = "menuItemNewFolder";
            menuItemNewFolder.Size = new System.Drawing.Size(151, 22);
            menuItemNewFolder.Text = "New Folder";
            menuItemNewFolder.Click += new System.EventHandler(OnNewFolder);
            
            // toolStripSeparator1
            toolStripSeparator.Name = "toolStripSeparator1";
            toolStripSeparator.Size = new System.Drawing.Size(148, 6);
            
            // menuItemOrganizeFilters
            menuItemOrganizeFilters.Image = global::ShipWorks.Properties.Resources.funnel_properties_16;
            menuItemOrganizeFilters.Name = "menuItemOrganizeFilters";
            menuItemOrganizeFilters.Size = new System.Drawing.Size(151, 22);
            menuItemOrganizeFilters.Text = "Manage Filters";
            menuItemOrganizeFilters.Click += new System.EventHandler(OnManageFilters);

            editionGuiHelper.RegisterElement(menuItemNewFilter, EditionFeature.FilterLimit, () => FilterLayoutContext.Current == null ? 0 : FilterLayoutContext.Current.GetTopLevelFilterCount());
        }

        /// <summary>
        /// Gets the FilterLastActive for the current target type.  Defaults to Order value if more than one target is specified
        /// or if only one target and Customers is not it.
        /// </summary>
        private long FilterLastActive(UserSettingsEntity settings)
        {
            if (Targets.Count() == 1 && Targets.Contains(FilterTarget.Customers))
            {
                return settings.CustomerFilterLastActive;
            }

            return settings.OrderFilterLastActive;
        }
        
        /// <summary>
        /// Select the initial filter based on the given user settings
        /// </summary>
        public void SelectInitialFilter(UserSettingsEntity settings)
        {
            long initialID = 0;

            if (settings.FilterInitialUseLastActive)
            {
                initialID = FilterLastActive(settings);
            }
            else
            {
                initialID = settings.FilterInitialSpecified;
            }

            // Select it
            SelectedFilterNodeID = initialID;

            // If there is nothing selected, that doesn't exist anymore
            if (SelectedFilterNode == null)
            {
                SelectedFilterNodeID = BuiltinFilter.GetTopLevelKey(this.Targets.FirstOrDefault());
            }
        }

        /// <summary>
        /// Context menu for the filter tree is opening
        /// </summary>
        private void OnFilterTreeContextMenuOpening(object sender, CancelEventArgs e)
        {
            bool filtersPermission = UserSession.Security.HasPermission(PermissionType.ManageFilters);
            bool filterSelected = SelectedFilterNode != null;
            bool myFilter = filterSelected ? FilterHelper.IsMyFilter(SelectedFilterNode) : false;

            menuItemEditFilter.Enabled = filterSelected;
            menuItemEditFilter.Available = filtersPermission || myFilter;

            if (filterSelected && BuiltinFilter.IsSearchPlaceholderKey(SelectedFilterNode.FilterID))
            {
                menuItemEditFilter.Available = false;
            }

            menuItemEditFilterSep.Available = menuItemEditFilter.Available;
        }

        /// <summary>
        /// Edit the selected filter or folder, if any
        /// </summary>
        private void OnEditFilter(object sender, EventArgs e)
        {
            FilterEditingResult result = FilterEditingService.EditFilter(SelectedFilterNode, this);

            if (result == FilterEditingResult.OK)
            {
                UpdateFilter(SelectedFilterNode.Filter);
            }
        }

        /// <summary>
        /// Create a new filter, based on the current selection
        /// </summary>
        private void OnNewFilter(object sender, EventArgs e)
        {
            CreateFilter(false, (SelectedLocation != null) ? SelectedLocation.ParentNode : null);
        }

        /// <summary>
        /// Create a new filter folder, based on the current selection
        /// </summary>
        private void OnNewFolder(object sender, EventArgs e)
        {
            CreateFilter(true, (SelectedLocation != null) ? SelectedLocation.ParentNode : null);
        }

        /// <summary>
        /// Initiate the creation of a new filter or folder
        /// </summary>
        private void CreateFilter(bool isFolder, FilterNodeEntity parent)
        {
            // Creating a filter can create more than one node (if the parent is linked), but 
            // this one will be the one that should be selected
            FilterNodeEntity primaryNode;

            FilterEditingResult result = FilterEditingService.NewFilter(
                isFolder,
                parent,
                GetFolderState(),
                this,
                out primaryNode);

            if (result == FilterEditingResult.OK)
            {
                ReloadLayouts();
                SelectedFilterNode = primaryNode;

                // # of filters present can effect edition issues
                editionGuiHelper.UpdateUI();
            }
        }

        /// <summary>
        /// Open the filter organizer
        /// </summary>
        private void OnManageFilters(object sender, EventArgs e)
        {
            using (FilterOrganizerDlg dlg = new FilterOrganizerDlg(SelectedFilterNode, GetFolderState()))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Controls whether the filter tree is editable.  Items can be renamed, and dragged around.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool Editable
        {
            get
            {
                return sandGrid.MouseEditMode == MouseEditMode.DelayedSingleClick;
            }
            set
            {
                sandGrid.MouseEditMode = value ? MouseEditMode.DelayedSingleClick : MouseEditMode.None;
                sandGrid.AllowDrag = value;
            }
        }

        /// <summary>
        /// Indicates if the row under the mouse is always drawn hot.  It also changes how selection works, in that clicking the mouse and
        /// dragging it does not raise the selction change event until the mouse is released.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool HotTracking
        {
            get
            {
                return sandGrid.HotTracking;
            }
            set
            {
                sandGrid.HotTracking = value;
            }
        }

        /// <summary>
        /// Controls if only folders will be selectable.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool FoldersOnly
        {
            get
            {
                return sandGrid.SelectFoldersOnly;
            }
            set
            {
                sandGrid.SelectFoldersOnly = value;
            }
        }
        
        /// <summary>
        /// Controls if the user can choose a Quick Filter, or just the default standard filters
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool AllowQuickFilter
        {
            get
            {
                return panelQuickFilter.Visible;
            }
            set
            {
                panelQuickFilter.Visible = value;
            }
        }

        /// <summary>
        /// Controls the scope of filters that are displayed.  Does not take effect until the next time LoadLayouts is called.
        /// </summary>
        [DefaultValue(FilterScope.Any)]
        [Category("Behavior")]
        public FilterScope FilterScope
        {
            get
            {
                return filterScope;
            }
            set
            {
                filterScope = value;
            }
        }

        /// <summary>
        /// Indiciates if the "My Filters" is always shown, or only shown if it has contents
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool AlwaysShowMyFilters
        {
            get
            {
                return alwaysShowMyFilters;
            }
            set
            {
                alwaysShowMyFilters = value;
            }
        }

        /// <summary>
        /// Should disabled filters be hidden
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool HideDisabledFilters { get; set; }

        /// <summary>
        /// Indicates if the active search node - if any - that will be displayed
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FilterNodeEntity ActiveSearchNode
        {
            get
            {
                return activeSearchNode;
            }
            set
            {
                if (value != null && value.Purpose != (int) FilterNodePurpose.Search)
                {
                    throw new InvalidOperationException("Only search nodes can be set as ActiveSearchNode");
                }

                activeSearchNode = value;

                UpdateActiveSearchNodeDisplay();
            }
        }

        /// <summary>
        /// Indiciates if the tree will automatically try to retrieve count values for counts that
        /// are in the calculating state.  This will only look for updates as long as some counts
        /// are calculating.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool AutoRefreshCalculatingCounts
        {
            get { return autoRefreshCalculatingCounts; }
            set { autoRefreshCalculatingCounts = value; }
        }

        /// <summary>
        /// This property is here just to change the default value.
        /// </summary>
        [DefaultValue(BorderStyle.Fixed3D)]
        public new BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }
            set
            {
                base.BorderStyle = value;
            }
        }

        /// <summary>
        /// Get the size the tree would need to be to not show any scrollbars
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public Size IdealSize
        {
            get
            {
                // Seen cases where for some reason we arent' created yet, and row heights arent set
                IntPtr ensureCreated = Handle;

                List<SandGridTreeRow> rows = sandGrid.GetAllRows();

                int height = 50;

                foreach (GridRow row in rows)
                {
                    if (row.IsExpansionVisible())
                    {
                        if (row.Bounds.Bottom > height)
                        {
                            height = row.Bounds.Bottom;
                        }
                    }
                }

                // Account for scroll bar space;
                height += SystemInformation.HorizontalScrollBarHeight;

                // Let SandGrid figure out how wide it should be
                int width = gridColumnFilterNode.GetMaximumCellWidth(RowScope.AllRows, false);

                // Account for the fact that filters will draw there counts
                width += 75;

                if (AllowQuickFilter)
                {
                    height += panelQuickFilter.Height;

                    width = Math.Max(width, quickFilterChoose.Right + 10);
                }

                return new Size(width, height);
            }
        }

        /// <summary>
        /// Clears the tree of all content
        /// </summary>
        public void Clear()
        {
            sandGrid.Rows.Clear();
            nodeOwnerMap.Clear();
        }

        /// <summary>
        /// Update the counts of each node in the grid
        /// </summary>
        public void UpdateFilterCounts()
        {
            foreach (FilterTreeGridRow row in sandGrid.GetAllRows())
            {
                row.UpdateFilterCount();
            }

            UpdateQuickFilterDisplay();
        }

        /// <summary>
        /// Refresh the tree from the current state of the layouts
        /// </summary>
        public void ReloadLayouts()
        {
            // Save the selected node
            long selectedNodeID = SelectedFilterNodeID;

            // We want to maintain the folder state
            FolderExpansionState state = GetFolderState();

            // Don't listen to layout changes while we reload
            sandGrid.SelectionChanged -= new SelectionChangedEventHandler(OnGridSelectionChanged);

            // We have to reload the context
            layoutContext.Reload();

            // Do the reload
            LoadLayouts(Targets);

            // Reapply the state
            ApplyFolderState(state);

            // Try to reselect the node
            SelectedFilterNodeID = selectedNodeID;

            // Start listening for selection changes again
            sandGrid.SelectionChanged += new SelectionChangedEventHandler(OnGridSelectionChanged);

            // If its not available anymore, we wont have succesfully selected it
            if (SelectedFilterNodeID != selectedNodeID)
            {
                OnSelectedFilterNodeChanged();
            }
        }

        /// <summary>
        /// Load the layouts for the given targets from the specified context
        /// </summary>
        public void LoadLayouts(params FilterTarget[] targets)
        {
            Clear();
            quickFilterSelected = false;

            if (targets == null || targets.Length == 0)
            {
                throw new ArgumentException("Parameter 'targets' cannot be empty.");
            }

            // We will need this
            layoutContext = FilterLayoutContext.Current;
            this.Targets = targets;

            // Load the layout for each target
            foreach (FilterTarget target in targets)
            {
                // If its not a top-level target, it doesn't get shown in the tree.  It can however be used to create a quick filter
                if (!BuiltinFilter.HasTopLevelKey(target))
                {
                    continue;
                }

                if (filterScope == FilterScope.Any || filterScope == FilterScope.Everyone)
                {
                    FilterLayoutEntity sharedLayout = layoutContext.GetSharedLayout(target);
                    FilterNodeEntity filterNode = sharedLayout.FilterNode;

                    // Load the root node for the shared layout
                    FilterTreeGridRow rootRow = new FilterTreeGridRow(filterNode);
                    nodeOwnerMap[filterNode] = rootRow;
                    sandGrid.Rows.Add(rootRow);

                    // Load all its children
                    LoadChildren(filterNode, rootRow);
                }

                // Update display of the search node
                UpdateActiveSearchNodeDisplay();

                // Update whether the "My Filters" is being displayed
                UpdateMyLayoutAvailability(target);
            }

            // If there was local node, it has to match the new targets
            if (quickFilterNode != null && Array.IndexOf(targets, (FilterTarget) quickFilterNode.Filter.FilterTarget) < 0)
            {
                quickFilterNode = null;
            }

            // If auto refrshing, kick off the timer to check for calculating counts to complete.
            if (autoRefreshCalculatingCounts)
            {
                CheckForCalculatingCounts();
            }
        }

        /// <summary>
        /// Begin checking for counts that are calculating so that they may be refreshed.
        /// </summary>
        private void CheckForCalculatingCounts()
        {
            if (!autoRefreshCalculatingCounts)
            {
                throw new InvalidOperationException("AutoRefreshCalculatingCounts is not enabled.");
            }

            filterCountTimer.Start();
            OnRefreshFilterCountTimer(filterCountTimer, EventArgs.Empty);
        }

        /// <summary>
        /// Update the display of the ActiveSearchNode
        /// </summary>
        private void UpdateActiveSearchNodeDisplay()
        {
            if (layoutContext == null || Targets == null || Targets.Length == 0)
            {
                return;
            }

            FilterTreeGridRow existingSearchRow = sandGrid.Rows.Cast<FilterTreeGridRow>().FirstOrDefault(r => r.FilterNode.Purpose == (int) FilterNodePurpose.Search);
            bool removeExisting = false;

            // See if we need to show the active search node
            if (activeSearchNode != null)
            {
                // See if there is a search node
                if (existingSearchRow != null)
                {
                    // Its already there
                    if (existingSearchRow.FilterNode.FilterNodeID == activeSearchNode.FilterNodeID)
                    {
                        return;
                    }
                    else
                    {
                        removeExisting = true;
                    }
                }

                FilterTreeGridRow row = CreateRow(activeSearchNode);
                sandGrid.Rows.Insert(0, row);
            }
            else
            {
                removeExisting = true;
            }

            if (removeExisting && existingSearchRow != null)
            {
                nodeOwnerMap.Remove(existingSearchRow.FilterNode);

                // Without this our code to ensure we always have a selection would keep it selected
                if (lastSelectedRow == existingSearchRow)
                {
                    lastSelectedRow = null;
                }

                existingSearchRow.Remove();
            }
        }

        /// <summary>
        /// Update the presence of the My Filters for the given target
        /// </summary>
        private void UpdateMyLayoutAvailability(FilterTarget target)
        {
            if (filterScope != FilterScope.Any && filterScope != FilterScope.MyFilters)
            {
                return;
            }

            FilterLayoutEntity myLayout = layoutContext.GetMyLayout(target);
            FilterNodeEntity filterNode = myLayout.FilterNode;

            // Only show "My Filters" if there are any
            if (filterNode.ChildNodes.Count > 0 || filterScope == FilterScope.MyFilters || alwaysShowMyFilters)
            {
                // We are not already showing it
                if (!nodeOwnerMap.ContainsKey(filterNode))
                {
                    // Load the root node for the my layout
                    FilterTreeGridRow myRow = new FilterTreeGridRow(filterNode);
                    nodeOwnerMap[filterNode] = myRow;

                    if (sandGrid.Rows.Count > 0)
                    {
                        FilterTreeGridRow parentRow = sandGrid.Rows.Cast<FilterTreeGridRow>().Single(r =>
                            r.FilterNode.Filter.FilterTarget == (int) target &&
                            BuiltinFilter.IsTopLevelKey(r.FilterNode.FilterID) );

                        parentRow.NestedRows.Insert(0, myRow);
                    }
                    else
                    {
                        sandGrid.Rows.Add(myRow);
                    }

                    // Load all its children
                    LoadChildren(filterNode, myRow);
                }
            }
            else
            {
                // If its there now, we have to get rid of it
                FilterTreeGridRow row;
                if (nodeOwnerMap.TryGetValue(filterNode, out row))
                {
                    // Without this our code to ensure we always have a selection would keep it selected
                    if (lastSelectedRow == row)
                    {
                        lastSelectedRow = null;
                    }

                    nodeOwnerMap.Remove(filterNode);
                    row.Remove();
                }
            }
        }

        /// <summary>
        /// Load the child nodes of the given filter node
        /// </summary>
        private void LoadChildren(FilterNodeEntity filterNode, FilterTreeGridRow parentRow)
        {
            // Go through each of the children in order
            foreach (FilterNodeEntity childNode in filterNode.ChildNodes.Where(x => !HideDisabledFilters || x.Filter.State == (int) FilterState.Enabled))
            {
                FilterTreeGridRow row = CreateRow(childNode);
                parentRow.NestedRows.Add(row);

                LoadChildren(childNode, row);
            }

            // By default, everything is expanded
            parentRow.Expanded = true;
        }

        /// <summary>
        /// Gets \ sets the ID of the selected filter node
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(0)]
        public long SelectedFilterNodeID
        {
            get
            {
                FilterNodeEntity node = SelectedFilterNode;
                if (node == null)
                {
                    return 0;
                }

                return node.FilterNodeID;
            }
            set
            {
                foreach (KeyValuePair<FilterNodeEntity, FilterTreeGridRow> pair in nodeOwnerMap)
                {
                    if (pair.Key.FilterNodeID == value)
                    {
                        pair.Value.Selected = true;
                        EnsureNodeVisible(pair.Value.FilterNode);
                        return;
                    }
                }

                // See if we already have it as a loaded local node
                if (quickFilterNode != null && quickFilterNode.FilterNodeID == value)
                {
                    SelectedFilterNode = quickFilterNode;
                    return;
                }

                FilterNodeEntity potential = layoutContext.FindNode(value);
                if (potential != null && potential.Purpose == (int) FilterNodePurpose.Quick && Array.IndexOf(Targets, (FilterTarget) potential.Filter.FilterTarget) >= 0)
                {
                    SelectedFilterNode = potential;
                    return;
                }

                SelectedFilterNode = null;
            }
        }

        /// <summary>
        /// Returns the currently selected FilterNode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null)]
        public FilterNodeEntity SelectedFilterNode
        {
            get
            {
                if (quickFilterSelected)
                {
                    return quickFilterNode;
                }

                if (sandGrid.Rows.Count == 0)
                {
                    return null;
                }

                if (sandGrid.SelectedElements.Count == 1)
                {
                    FilterTreeGridRow row = sandGrid.SelectedElements[0] as FilterTreeGridRow;
                    return row.FilterNode;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    lastSelectedRow = null;

                    if (quickFilterSelected)
                    {
                        quickFilterSelected = false;
                        OnSelectedFilterNodeChanged();
                    }
                    else
                    {
                        sandGrid.SelectedElements.Clear();
                    }

                    return;
                }

                FilterTreeGridRow rowToSelect;
                if (nodeOwnerMap.TryGetValue(value, out rowToSelect))
                {
                    rowToSelect.Selected = true;
                    EnsureNodeVisible(rowToSelect.FilterNode);
                }
                else
                {
                    lastSelectedRow = null;

                    if (value.Purpose == (int) FilterNodePurpose.Quick)
                    {
                        // This quick filter is already selected
                        if (quickFilterSelected && quickFilterNode.FilterNodeID == value.FilterNodeID)
                        {
                            return;
                        }

                        quickFilterNode = value;
                        quickFilterSelected = true;

                        if (sandGrid.SelectedElements.Count > 0)
                        {
                            sandGrid.SelectedElements.Clear();
                        }
                        else
                        {
                            OnSelectedFilterNodeChanged();
                        }
                    }
                    else
                    {
                        sandGrid.SelectedElements.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Select the first (root) filter node in the tree
        /// </summary>
        public void SelectFirstNode()
        {
            if (sandGrid.Rows.Count > 0)
            {
                SelectedFilterNode = ((FilterTreeGridRow) sandGrid.Rows[0]).FilterNode;
            }
        }

        /// <summary>
        /// Get the location of the current selection
        /// </summary>
        [Browsable(false)]
        public FilterLocation SelectedLocation
        {
            get
            {
                if (SelectedFilterNode == null)
                {
                    return null;
                }

                FilterLocation location = new FilterLocation(GetSelectedParent(), GetSelectedPosition());
                return location;
            }
        }

        /// <summary>
        /// Get what is to be the parent of items that are moved \ inserted
        /// </summary>
        private FilterNodeEntity GetSelectedParent()
        {
            FilterNodeEntity filterNode = SelectedFilterNode;
            if (filterNode.Filter.IsFolder)
            {
                return filterNode;
            }
            else
            {
                return filterNode.ParentNode;
            }
        }

        /// <summary>
        /// Get the current position that items will be moved \ inserted
        /// </summary>
        private int GetSelectedPosition()
        {
            FilterNodeEntity filterNode = SelectedFilterNode;
            if (filterNode.Filter.IsFolder)
            {
                return filterNode.ChildNodes.Count;
            }
            else
            {
                return filterNode.FilterSequence.Position + 1;
            }
        }

        /// <summary>
        /// Gets \ sets the expand \ collapse state of all of the folders.
        /// </summary>
        public FolderExpansionState GetFolderState()
        {
            FolderExpansionState state = new FolderExpansionState();

            // Save the state of each node
            foreach (FilterTreeGridRow row in sandGrid.GetAllRows())
            {
                FilterNodeEntity node = row.FilterNode;

                // If its a folder that actually has children, save its state
                if (node.Filter.IsFolder && row.NestedRows.Count > 0)
                {
                    state.SetExpanded(node, row.Expanded);
                }
            }

            return state;
        }
       
        /// <summary>
        /// Apply the state to the folders currently in the treee
        /// </summary>
        public void ApplyFolderState(FolderExpansionState folderState)
        {
            // Update the state of each row
            foreach (FilterTreeGridRow row in sandGrid.GetAllRows())
            {
                row.Expanded = folderState.IsExpanded(row.FilterNode);
            }
        }

        /// <summary>
        /// Update the display text for all nodes that use the given filter
        /// </summary>
        public void UpdateFilterNames()
        {
            foreach (var pair in nodeOwnerMap)
            {
                GridCell gridCell = pair.Value.Cells[0];

                gridCell.Text = pair.Key.Filter.Name;
                gridCell.Image = FilterHelper.GetFilterImage(pair.Key);
            }

            if (autoRefreshCalculatingCounts)
            {
                CheckForCalculatingCounts();
            }
        }

        /// <summary>
        /// Update the display text for all nodes that use the given filter
        /// </summary>
        public void UpdateFilter(FilterEntity filter)
        {
            foreach (FilterNodeEntity node in FilterHelper.GetNodesUsingFilter(filter))
            {
                // They may be asking us to refresh node's we arent displaying (like someone else's My Filters) in which case we'll just
                // skip it as there's nothing to update.
                if (nodeOwnerMap.ContainsKey(node))
                {
                    FilterTreeGridRow gridRow = GetGridRow(node);

                    if (filter.State != (int) FilterState.Enabled && HideDisabledFilters)
                    {
                        if (gridRow.Selected)
                        {
                            SelectFirstNode();
                        }

                        gridRow.Remove();
                    }
                    else
                    {
                        GridCell gridCell = gridRow.Cells[0];
                        gridCell.Text = filter.Name;
                        gridCell.Image = FilterHelper.GetFilterImage(node);   
                    }
                }
            }

            if (autoRefreshCalculatingCounts)
            {
                CheckForCalculatingCounts();
            }
        }

        /// <summary>
        /// Begin a rename operation on the given filter
        /// </summary>
        public void BeginRename(FilterNodeEntity filterNode)
        {
            if (!Editable)
            {
                throw new InvalidOperationException("Cannot rename a filter.  The view is not editable.");
            }

            if (!sandGrid.EditorActive)
            {
                GetGridRow(filterNode).BeginEdit();
            }
        }

        /// <summary>
        /// Get the FilterTreeGridRow that represents the given FilterNode
        /// </summary>
        private FilterTreeGridRow GetGridRow(FilterNodeEntity node)
        {
            return nodeOwnerMap[node];
        }

        /// <summary>
        /// Create and initialize a new row for the given node
        /// </summary>
        private FilterTreeGridRow CreateRow(FilterNodeEntity node)
        {
            FilterTreeGridRow row = new FilterTreeGridRow(node);
            nodeOwnerMap[node] = row;

            return row;
        }

        /// <summary>
        /// The selected item in the grid has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If the selection was cleared, but there is a row selected we should preserve, then preserve it.
            if (sandGrid.SelectedElements.Count == 0 && lastSelectedRow != null && lastSelectedRow.Grid == sandGrid.PrimaryGrid)
            {
                lastSelectedRow.Selected = true;
            }
            else
            {
                if (sandGrid.SelectedElements.Count == 1)
                {
                    // If it's already know to be selected, don't raise the selected node change event
                    if (lastSelectedRow == sandGrid.SelectedElements[0])
                    {
                        return;
                    }

                    // Update the last selected row
                    lastSelectedRow = (FilterTreeGridRow) sandGrid.SelectedElements[0];

                    // If selecting an actual row, the quick filter is no longer selected
                    quickFilterSelected = false;
                }
                else
                {
                    lastSelectedRow = null;
                }

                OnSelectedFilterNodeChanged();
            }
        }

        /// <summary>
        /// Raise the SelectedFilterNodeChanged event
        /// </summary>
        private void OnSelectedFilterNodeChanged()
        {
            if (SelectedFilterNode != null && SelectedFilterNode.Purpose == (int) FilterNodePurpose.Quick)
            {
                quickFilterNode = SelectedFilterNode;
            }

            UpdateQuickFilterDisplay();

            EventHandler handler = SelectedFilterNodeChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// A node is about to be edited (renamed)
        /// </summary>
        private void OnBeforeEdit(object sender, GridBeforeEditEventArgs e)
        {
            FilterNodeEntity node = ((FilterTreeGridRow) e.Row).FilterNode;

            if (FilterRenaming != null)
            {
                FilterNodeRenameEventArgs args = new FilterNodeRenameEventArgs(node);
                FilterRenaming(this, args);

                e.Cancel = args.Cancel;
            }
        }

        /// <summary>
        /// A user is commiting there changes to an edited node (rename)
        /// </summary>
        private void OnAfterEdit(object sender, GridAfterEditEventArgs e)
        {
            FilterNodeEntity node = ((FilterTreeGridRow) e.Row).FilterNode;
            string proposed = e.Value.ToString().Trim();

            if (proposed == node.Filter.Name)
            {
                e.Cancel = true;
                return;
            }

            if (FilterRenamed != null)
            {
                FilterNodeRenameEventArgs args = new FilterNodeRenameEventArgs(node, proposed);
                FilterRenamed(this, args);

                e.Cancel = args.Cancel;
            }
        }

        /// <summary>
        /// The user let go
        /// </summary>
        private void OnGridRowDropped(object sender, GridRowDroppedEventArgs e)
        {
            FilterNodeEntity targetNode = ((FilterTreeGridRow) e.TargetRow).FilterNode;
            FilterNodeEntity sourceNode = ((FilterTreeGridRow) e.SourceRow).FilterNode;

            FilterLocation moveLocation = null;
            FilterLocation copyLocation = null;

            DropTargetState where = e.TargetRow.DragDropState;

            // Dropping inside a folder
            if (where == DropTargetState.DropInside)
            {
                // Can't drop inside oneself
                if (targetNode != sourceNode)
                {
                    // Cant move into a folder we are already in
                    if (targetNode != sourceNode.ParentNode)
                    {
                        // Drop it in the folder as the first child
                        moveLocation = new FilterLocation(targetNode, 0);
                    }

                    copyLocation = new FilterLocation(targetNode, 0);
                }
            }
            // Dropping above or below a sourceNode
            else
            {
                // Can always copy above or below
                copyLocation = new FilterLocation(
                        targetNode.ParentNode,
                        where == DropTargetState.DropAbove ? targetNode.FilterSequence.Position : targetNode.FilterSequence.Position + 1);

                // Can't move above or below one self
                if (targetNode != sourceNode)
                {
                    moveLocation = new FilterLocation(
                        targetNode.ParentNode,
                        where == DropTargetState.DropAbove ? targetNode.FilterSequence.Position : targetNode.FilterSequence.Position + 1);

                    // Special cases for siblings
                    if (targetNode.ParentNode == sourceNode.ParentNode)
                    {
                        // Not really moving
                        if (sourceNode.FilterSequence.Position == targetNode.FilterSequence.Position - 1 &&
                            where == DropTargetState.DropAbove)
                        {
                            moveLocation = null;
                        }

                        // Not really moving
                        if (sourceNode.FilterSequence.Position == targetNode.FilterSequence.Position + 1 &&
                            where == DropTargetState.DropBelow)
                        {
                            moveLocation = null;
                        }

                        // A sibling moving down
                        if (moveLocation != null && sourceNode.FilterSequence.Position < targetNode.FilterSequence.Position)
                        {
                            moveLocation.Position--;
                        }
                    }                
                }
            }

            if (DragDropComplete != null && (moveLocation != null || copyLocation != null))
            {
                FilterDragDropCompleteEventArgs args = new FilterDragDropCompleteEventArgs(sourceNode, moveLocation, copyLocation, e);
                DragDropComplete(this, args);
            }
        }

        /// <summary>
        /// Focus is leaving the control
        /// </summary>
        private void OnLeave(object sender, EventArgs e)
        {
            sandGrid.EndEdit(false, false);
        }

        /// <summary>
        /// User pressed a key
        /// </summary>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (Editable && DeleteKeyPressed != null)
                {
                    DeleteKeyPressed(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Ensure that the given node is visible in the tree
        /// </summary>
        public void EnsureNodeVisible(FilterNodeEntity node)
        {
            FilterTreeGridRow row;
            if (!nodeOwnerMap.TryGetValue(node, out row))
            {
                return;
            }

            FilterTreeGridRow originalRow = row;

            // Try to give it a little room if we can, by making a row a few below it visible
            for (int i = 0; i < 3; i++)
            {
                if (row.NextVisibleRow != null)
                {
                    row = (FilterTreeGridRow) row.NextVisibleRow;
                }
            }

            // First try to get the one a few below it visible, to make sure we are scrolled a little past it
            row.EnsureVisible();

            // But we don't want to scroll too far down, so now really ensure its visible.  IF it already is, it won't undo the
            // one we just did
            GridRow parent = originalRow.ParentRow;
            while (parent != null)
            {
                parent.Expanded = true;
                parent = parent.ParentRow;
            }

            originalRow.EnsureVisible();
        }

        /// <summary>
        /// Gets or sets the node that is currently being displayed as hot-tracked
        /// </summary>
        public FilterNodeEntity HotTrackNode
        {
            get
            {
                FilterTreeGridRow hotTrackRow = sandGrid.HotTrackRow as FilterTreeGridRow;

                if (hotTrackRow != null)
                {
                    return hotTrackRow.FilterNode;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    sandGrid.HotTrackRow = null;
                }
                else
                {
                    FilterTreeGridRow row;
                    if (nodeOwnerMap.TryGetValue(value, out row))
                    {
                        sandGrid.HotTrackRow = row;
                    }
                    else
                    {
                        sandGrid.HotTrackRow = null;
                    }
                }
            }
        }

        /// <summary>
        /// Timer use to check for the completion of filter counts that are in the process of calculating.
        /// </summary>
        private void OnRefreshFilterCountTimer(object sender, EventArgs e)
        {
            // Make sure our counts are up to date
            bool changesOrCalcuations = FilterContentManager.CheckForChanges();

            UpdateFilterCounts();

            // All we want to do is get latest counts so that we can stop the spinning
            if (!changesOrCalcuations)
            {
                filterCountTimer.Stop();
            }
        }

        /// <summary>
        /// Indicates if the tree is showing any nodes in the calculating state.
        /// </summary>
        public bool HasCalculatingNodes()
        {
            foreach (FilterTreeGridRow row in nodeOwnerMap.Values)
            {
                if (row.FilterCount != null && row.FilterCount.Status != FilterCountStatus.Ready)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update the quick filter display based on if a quick filter is currently selected
        /// </summary>
        private void UpdateQuickFilterDisplay()
        {
            // Update common visibility
            quickFilterPicture.Visible = quickFilterNode != null;
            quickFilterName.Visible = quickFilterNode != null;

            // See if its a quick filter
            if (quickFilterNode != null)
            {
                // Edit link is an edit link
                quickFilterEdit.Text = "Edit...";
                quickFilterEdit.Visible = UserSession.Security.HasPermission(PermissionType.ManageFilters);

                quickFilterChoose.Visible = UserSession.Security.HasPermission(PermissionType.ManageFilters);

                // Update the locations
                quickFilterName.Text = quickFilterNode.Filter.Name;

                quickFilterCount.Left = quickFilterName.Right - 3;
                quickFilterCalculating.Left = quickFilterCount.Left;

                FilterCount count = FilterContentManager.GetCount(quickFilterNode.FilterNodeID);
                if (count != null)
                {
                    quickFilterDisplayManager.ToggleDisplay(quickFilterNode.Filter.State == (int)FilterState.Enabled);
                    
                    if (count.Status == FilterCountStatus.Ready)
                    {
                        quickFilterCount.Visible = true;
                        quickFilterCount.Text = string.Format("({0:#,##0})", count.Count);
                        quickFilterEdit.Left = quickFilterCount.Right;

                        quickFilterCalculating.Visible = false;
                    }
                    else
                    {
                        quickFilterCalculating.Visible = true;
                        quickFilterEdit.Left = quickFilterCalculating.Right;

                        quickFilterCount.Visible = false;
                    }
                }
                else
                {
                    quickFilterCount.Visible = false;
                    quickFilterCalculating.Visible = false;
                    quickFilterEdit.Left = quickFilterName.Right;
                }

            }
            // Not a quick filter
            else
            {
                // The edit link becomes create, and it moves over to the left
                quickFilterEdit.Text = "Create...";
                quickFilterEdit.Left = quickFilterPicture.Left;

                quickFilterCalculating.Visible = false;
                quickFilterCount.Visible = false;
            }

            // Set clear based on edit location
            quickFilterChoose.Left = quickFilterEdit.Right;

            // Only show the divider between the quick filter and the main content if there is any main content
            quickFilterDivider.Visible = sandGrid.Rows.Count > 0;

            quickFilterBackground.Invalidate();
        }

        /// <summary>
        /// Edit the current quick filter, or create a new one
        /// </summary>
        private void OnQuickFilterEditCreate(object sender, EventArgs e)
        {
            if (!UserSession.Security.HasPermission(PermissionType.ManageFilters))
            {
                MessageHelper.ShowMessage(this, "You do not have permission to create or edit filters.");
                return;
            }

            if (quickFilterNode == null)
            {
                if (Targets.Length == 1)
                {
                    OnQuickFilterCreate(new ToolStripMenuItem { Tag = Targets[0] }, EventArgs.Empty);
                }
                else
                {
                    ContextMenuStrip targetMenu = new ContextMenuStrip();
                    foreach (FilterTarget target in Targets)
                    {
                        ToolStripMenuItem menuItem = new ToolStripMenuItem(EnumHelper.GetDescription(target).TrimEnd('s') + " Filter");
                        menuItem.Click += new EventHandler(OnQuickFilterCreate);
                        menuItem.Tag = target;
                        targetMenu.Items.Add(menuItem);
                    }

                    targetMenu.Show(quickFilterEdit.Parent.PointToScreen(new Point(quickFilterEdit.Left, quickFilterEdit.Bottom)));
                }
            }
            else
            {
                FilterEditingResult result = FilterEditingService.EditFilter(quickFilterNode, this);

                if (autoRefreshCalculatingCounts)
                {
                    CheckForCalculatingCounts();
                }

                if (result == FilterEditingResult.OK)
                {
                    // When a quick filter is edited, we treat it like the node was changed, so that consumers know they need to update\refresh.
                    SelectedFilterNode = quickFilterNode;
                }
                else
                {
                    UpdateQuickFilterDisplay();
                }
            }
        }

        /// <summary>
        /// Create a quick filter
        /// </summary>
        private void OnQuickFilterCreate(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem) sender;

            quickFilterNode = QuickFilterHelper.CreateQuickFilter((FilterTarget) menuItem.Tag);

            if (FilterEditingService.EditFilter(quickFilterNode, this) != FilterEditingResult.OK)
            {
                QuickFilterHelper.DeleteQuickFilter(quickFilterNode);

                quickFilterNode = null;
            }
            else
            {
                // Having this commented out means that as soon as you create a new quick filter it gets selected
                // and the drop-down closes.  I think that behavior feels better.
                //  if (!HotTracking)
                {
                    SelectedFilterNode = quickFilterNode;
                }
            }

            if (autoRefreshCalculatingCounts)
            {
                CheckForCalculatingCounts();
            }

            UpdateQuickFilterDisplay();
        }

        /// <summary>
        /// User wants to choose a different quick filter
        /// </summary>
        private void OnQuickFilterChoose(object sender, EventArgs e)
        {
            FilterTarget initialTarget = Targets[0];

            if (quickFilterNode != null)
            {
                initialTarget = (FilterTarget) quickFilterNode.Filter.FilterTarget;
            }

            using (QuickFilterChooserDlg dlg = new QuickFilterChooserDlg(Targets, initialTarget))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    if (dlg.ChosenFilterNode != null)
                    {
                        SelectedFilterNode = dlg.ChosenFilterNode;

                        if (autoRefreshCalculatingCounts)
                        {
                            CheckForCalculatingCounts();
                        }

                        UpdateQuickFilterDisplay();
                    }
                }
            }
        }

        /// <summary>
        /// If a Quick Filter had been created and is present, selected or not, clear it.
        /// </summary>
        public void ClearQuickFilter()
        {
            if (quickFilterSelected)
            {
                SelectedFilterNode = null;
            }

            quickFilterNode = null;

            UpdateQuickFilterDisplay();
        }

        /// <summary>
        /// Mouse is entering the Quick Filter area
        /// </summary>
        private void OnQuickFilterMouseEnter(object sender, EventArgs e)
        {
            if (HotTracking)
            {
                quickFilterBackground.Invalidate();
            }
        }

        /// <summary>
        /// Mouse is leaving the Quick Filter area
        /// </summary>
        private void OnQuickFilterMouseLeave(object sender, EventArgs e)
        {
            if (HotTracking)
            {
                quickFilterBackground.Invalidate();
            }
        }

        /// <summary>
        /// Enter
        /// </summary>
        protected override void OnEnter(EventArgs e)
        {
            quickFilterBackground.Invalidate();
        }

        /// <summary>
        /// Leave
        /// </summary>
        protected override void OnLeave(EventArgs e)
        {
            quickFilterBackground.Invalidate();
        }

        /// <summary>
        /// Paint the quick filter background
        /// </summary>
        private void OnQuickFilterPaintBackground(object sender, PaintEventArgs e)
        {
            Color color = Color.Transparent;

            // If we are HotTracking and over the quick filter area, then color it selected
            if (quickFilterBackground.ClientRectangle.Contains(quickFilterBackground.PointToClient(Cursor.Position)) && HotTracking)
            {
                color = SandGridThemedSelectionRenderer.GetSelectionColor(true);
            }

            if (quickFilterSelected)
            {
                color = SandGridThemedSelectionRenderer.GetSelectionColor(ContainsFocus);
            }

            if (color != Color.Transparent)
            {
                using (SolidBrush brush = new SolidBrush(color))
                {
                    e.Graphics.FillRectangle(brush, e.ClipRectangle);
                }
            }
        }

        /// <summary>
        /// MouseDown on the Quick Filter row
        /// </summary>
        private void OnQuickFilterMouseDown(object sender, MouseEventArgs e)
        {
            if (quickFilterNode != null && !HotTracking)
            {
                SelectedFilterNode = quickFilterNode;
            }
        }

        /// <summary>
        /// MouseUp on the quick filter row
        /// </summary>
        private void OnQuickFilterMouseUp(object sender, MouseEventArgs e)
        {
            if (quickFilterNode != null && HotTracking)
            {
                SelectedFilterNode = quickFilterNode;
            }
        }

        /// <summary>
        /// Handle any filter node changed messages
        /// </summary>
        private void HandleFilterEdited(FilterNodeEditedMessage message)
        {
            IEnumerable<FilterTreeGridRow> rows = sandGrid.FlatRows.OfType<FilterTreeGridRow>().Where(x => x.FilterNode.FilterNodeID == message.FilterNode.FilterNodeID);

            foreach (FilterTreeGridRow row in rows)
            {
                row.UpdateFilterNode(message.FilterNode);
            }

            // Update the quick filter node if it matches the edited filter
            if (quickFilterNode != null && quickFilterNode.FilterNodeID == message.FilterNode.FilterNodeID)
            {
                quickFilterNode = message.FilterNode;
                UpdateQuickFilterDisplay();
            }

            // Update the selected node if it matches the edited filter
            if (SelectedFilterNode != null && SelectedFilterNode.FilterNodeID == message.FilterNode.FilterNodeID)
            {
                SelectedFilterNode = message.FilterNode;
                OnSelectedFilterNodeChanged();
            }
        }

        /// <summary>
        /// Indiciates if the given node is currently available for selection in the tree.  This checks
        /// the currently displayed quick filter node, if any, and the the entire filter tree content.
        /// </summary>
        public bool IsFilterNodeAvailable(long filterNodeID)
        {
            if (quickFilterNode != null && quickFilterNode.FilterNodeID == filterNodeID)
            {
                return true;
            }

            if (nodeOwnerMap.Keys.Any(n => n.FilterNodeID == filterNodeID))
            {
                return true;
            }

            return false;
        }
    }
}
