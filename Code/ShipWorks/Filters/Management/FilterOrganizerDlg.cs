using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ShipWorks.Users;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Properties;
using ShipWorks.UI.Controls;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared;
using System.Data.SqlClient;
using ShipWorks.Data;
using ShipWorks.Filters.Management;
using ShipWorks.UI;
using System.Transactions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Diagnostics;
using ShipWorks.Filters.Controls;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.MessageBoxes;
using System.Linq;
using ShipWorks.Data.Connection;
using ShipWorks.Users.Security;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Window for managing filters
    /// </summary>
    public partial class FilterOrganizerDlg : Form
    {
        FilterLayoutContext layoutContext;

        // Maintains folder state so it remains consistant as you select what applies to
        Dictionary<FilterTarget, FolderExpansionState> folderState = new Dictionary<FilterTarget, FolderExpansionState>();

        // Saved state
        long lastSelectedNodeIDFallback = 0;
        long lastSelectedNodeID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterOrganizerDlg(FilterNodeEntity selectedNode, FolderExpansionState initialState)
        {
            InitializeComponent();

            //filterTreeOrders.HideDisabledFilters = !showDisabledFilters.Checked;
            //filterTreeCustomers.HideDisabledFilters = !showDisabledFilters.Checked;
            
            WindowStateSaver.Manage(this);

            folderState[FilterTarget.Orders] = initialState;
            folderState[FilterTarget.Customers] = initialState;

            // If no selected node was requested, just use the root order node
            if (selectedNode == null)
            {
                selectedNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
            }

            this.lastSelectedNodeID = selectedNode.FilterNodeID;
            tabControl.SelectedIndex = (selectedNode.Filter.FilterTarget == (int) FilterTarget.Orders) ? 0 : 1;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            // We have to push a new FilterLayoutContext, since we are going to be editing and moving and deleting.  If 
            // we deleted stuff from the main FilterLayoutContext that the MainForm has reference to, then as it does its
            // background stuff it could fail pretty badly.  Pusing a new scope essentially makes managing the filters as if
            // it was being done by a totally seperate ShipWorks process - which the MainForm of course handles just fine.
            FilterLayoutContext.PushScope();
            layoutContext = FilterLayoutContext.Current;

            // Edition-managed UI
            editionGuiHelper.RegisterElement(newFilter, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(copy, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(createLink, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(menuItemCopy, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(menuItemCreateLink, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(menuItemContextCopy, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(menuItemLink, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(menuItemNewFilter, Editions.EditionFeature.FilterLimit, () => layoutContext.GetTopLevelFilterCount());

            ActiveFilterTree.SelectedFilterNodeChanged -= this.OnChangeSelectedFilterNode;

            LoadOrganizeFor(filterTreeOrders, FilterTarget.Orders);
            LoadOrganizeFor(filterTreeCustomers, FilterTarget.Customers);

            // Try to select the requested ID
            ActiveFilterTree.SelectedFilterNodeID = lastSelectedNodeID;

            // But if we cant, then use the primary from the same type
            if (ActiveFilterTree.SelectedFilterNode == null)
            {
                ActiveFilterTree.SelectFirstNode();
            }

            ActiveFilterTree.SelectedFilterNodeChanged += this.OnChangeSelectedFilterNode;

            UpdateButtonState();
        }

        /// <summary>
        /// The selected filter node has changed
        /// </summary>
        private void OnChangeSelectedFilterNode(object sender, EventArgs e)
        {
            if (sender == ActiveFilterTree)
            {
                UpdateButtonState();
            }
        }

        /// <summary>
        /// Return the filter tree the user is working with
        /// </summary>
        private FilterTree ActiveFilterTree
        {
            get
            {
                return tabControl.SelectedIndex == 0 ?
                    filterTreeOrders : filterTreeCustomers;
            }
        }

        /// <summary>
        /// The filter target the user is currently working with
        /// </summary>
        private FilterTarget ActiveFilterTarget
        {
            get
            {
                return tabControl.SelectedIndex == 0 ?
                    FilterTarget.Orders : FilterTarget.Customers;
            }
        }

        /// <summary>
        /// Selected tab has changed
        /// </summary>
        private void OnTabChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Update the state of the buttons based on the selection
        /// </summary>
        private void UpdateButtonState()
        {
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            rename.Enabled = FilterHelper.CanRename(selected);

            moveUp.Enabled = layoutContext.CanMoveUp(selected);
            moveDown.Enabled = layoutContext.CanMoveDown(selected);

            bool isBuiltin = FilterHelper.IsBuiltin(selected);

            copy.Enabled = !isBuiltin && !selected.Filter.IsFolder;
            createLink.Enabled = !isBuiltin;
            moveIntoFolder.Enabled = !isBuiltin;
            delete.Enabled = !isBuiltin;
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            menuItemContextMove.Enabled = moveIntoFolder.Enabled;
            menuItemCreateLink.Enabled = createLink.Enabled;
            menuItemContextCopy.Enabled = copy.Enabled;
            deleteToolStripMenuItem.Enabled = delete.Enabled;
        }

        /// <summary>
        /// Load the tree to be organized by the given target
        /// </summary>
        private void LoadOrganizeFor(FilterTree filterTree, FilterTarget target)
        {
            Cursor.Current = Cursors.WaitCursor;

            FilterContentManager.CheckForChanges();

            // Limit to my filters if permissions not enabled
            if (!UserSession.Security.HasPermission(PermissionType.ManageFilters))
            {
                filterTree.FilterScope = FilterScope.MyFilters;
            }

            filterTree.LoadLayouts(target);
            filterTree.ApplyFolderState(folderState[target]);
            filterTree.SelectFirstNode();
        }

        /// <summary>
        /// Save the current state of the folders and selection
        /// </summary>
        private void SaveSelectionState()
        {
            folderState[ActiveFilterTarget] = ActiveFilterTree.GetFolderState();
            lastSelectedNodeID = ActiveFilterTree.SelectedFilterNodeID;

            // What to fallback on if this node is deleted
            lastSelectedNodeIDFallback = 0;
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            // First try to fallback on the following sibling
            if (selected.ParentNode != null)
            {
                if (selected.ParentNode.ChildNodes.Count > selected.FilterSequence.Position + 1)
                {
                    lastSelectedNodeIDFallback = selected.ParentNode.ChildNodes[selected.FilterSequence.Position + 1].FilterNodeID;
                }

                // Then try the previous sibling
                else if (selected.FilterSequence.Position != 0)
                {
                    lastSelectedNodeIDFallback = selected.ParentNode.ChildNodes[selected.FilterSequence.Position - 1].FilterNodeID;
                }

                // If that doesnt work, then go to the parent
                else
                {
                    lastSelectedNodeIDFallback = selected.ParentNode.FilterNodeID;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
        }

        /// <summary>
        /// Completely reload the layout context and the tree
        /// </summary>
        private void Reload(bool reloadContext)
        {
            if (reloadContext)
            {
                Cursor.Current = Cursors.WaitCursor;

                // Reload the context
                layoutContext.Reload();
            }

            ActiveFilterTree.SelectedFilterNodeChanged -= new EventHandler(this.OnChangeSelectedFilterNode);

            // Load the tree
            LoadOrganizeFor(ActiveFilterTree, ActiveFilterTarget);

            // Try to select the last
            ActiveFilterTree.SelectedFilterNodeID = lastSelectedNodeID;

            // If that didn't work, try the fallback
            if (ActiveFilterTree.SelectedFilterNode == null)
            {
                ActiveFilterTree.SelectedFilterNodeID = lastSelectedNodeIDFallback;
            }

            // If that didn't work, select the root
            if (ActiveFilterTree.SelectedFilterNode == null)
            {
                ActiveFilterTree.SelectedFilterNode = layoutContext.GetSharedLayout(ActiveFilterTarget).FilterNode;
            }

            ActiveFilterTree.SelectedFilterNodeChanged += new EventHandler(this.OnChangeSelectedFilterNode);
            OnChangeSelectedFilterNode(ActiveFilterTree, EventArgs.Empty);

            // Edition stuff depends on filter layout
            editionGuiHelper.UpdateUI();
        }

        /// <summary>
        /// Create a new filter
        /// </summary>
        private void OnNewFilter(object sender, EventArgs e)
        {
            CreateFilter(false);
        }

        /// <summary>
        /// Create a new folder
        /// </summary>
        private void OnNewFolder(object sender, EventArgs e)
        {
            CreateFilter(true);
        }

        /// <summary>
        /// Initiate the creation of a new filter or folder
        /// </summary>
        private void CreateFilter(bool isFolder)
        {
            SaveSelectionState();

            // Creating a filter can create more than one node (if the parent is linked), but 
            // this one will be the one that should be selected
            FilterNodeEntity primaryNode;

            FilterEditingResult result = FilterEditingService.NewFilter(
                isFolder,
                ActiveFilterTree.SelectedLocation.ParentNode,
                ActiveFilterTree.GetFolderState(),
                this,
                out primaryNode);

            if (result == FilterEditingResult.OK)
            {
                Reload(false);
                ActiveFilterTree.SelectedFilterNode = primaryNode;
            }

            if (result == FilterEditingResult.Error)
            {
                Reload(true);
            }
        }

        /// <summary>
        /// Open the currently selected filter\folder for editing
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            SaveSelectionState();

            FilterEditingResult result = FilterEditingService.EditFilter(selected, this);

            if (result == FilterEditingResult.OK)
            {
                // It has changed, update the tree
                ActiveFilterTree.UpdateFilter(selected.Filter);
            }

            if (result == FilterEditingResult.Error)
            {
                Reload(true);
            }
        }

        /// <summary>
        /// Start a rename operation on the selected node
        /// </summary>
        private void OnRename(object sender, EventArgs e)
        {
            ActiveFilterTree.BeginRename(ActiveFilterTree.SelectedFilterNode);
        }

        /// <summary>
        /// A filter is about to be renamed
        /// </summary>
        private void OnBeforeRename(object sender, FilterNodeRenameEventArgs e)
        {
            e.Cancel = !FilterHelper.CanRename(e.FilterNode);
        }

        /// <summary>
        /// A filter has been renamed
        /// </summary>
        private void OnAfterRename(object sender, FilterNodeRenameEventArgs e)
        {
            FilterEntity filter = e.FilterNode.Filter;
            filter.Name = e.Proposed;

            SaveSelectionState();

            try
            {
                // Save the filter
                layoutContext.SaveFilter(filter);

                // The only thing that could have changed is the name
                ActiveFilterTree.UpdateFilter(filter);
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Move the selected filter up among its siblings
        /// </summary>
        private void OnMoveUp(object sender, EventArgs e)
        {
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            SaveSelectionState();

            try
            {
                layoutContext.MoveUp(selected);
                Reload(false);

                UpdateButtonState();
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Move the selected filter down among its sibilngs
        /// </summary>
        private void OnMoveDown(object sender, EventArgs e)
        {
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            SaveSelectionState();

            try
            {
                layoutContext.MoveDown(selected);
                Reload(false);

                UpdateButtonState();
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// The sort menu is opening
        /// </summary>
        private void OnSortMenuOpening(object sender, CancelEventArgs e)
        {
            FilterNodeEntity folder = ActiveFilterTree.SelectedLocation.ParentNode;
            toolStripMenuItemSortSelected.Text = string.Format("Contents of '{0}'", folder.Filter.Name);
        }

        /// <summary>
        /// Sort just the contents of the selected folder
        /// </summary>
        private void OnSortSelectedFolder(object sender, EventArgs e)
        {
            Sort(ActiveFilterTree.SelectedLocation.ParentNode);
        }

        /// <summary>
        /// Sort all the filters and folders
        /// </summary>
        private void OnSortAll(object sender, EventArgs e)
        {
            Sort(layoutContext.GetSharedLayout(ActiveFilterTarget).FilterNode);
        }

        /// <summary>
        /// Sort the contents of the given folder
        /// </summary>
        private void Sort(FilterNodeEntity folder)
        {
            SaveSelectionState();

            try
            {
                layoutContext.Sort(folder.Filter);

                // The My Filters is logically under the shared layout
                if (folder == layoutContext.GetSharedLayout(ActiveFilterTarget).FilterNode)
                {
                    layoutContext.Sort(layoutContext.GetMyLayout(ActiveFilterTarget).FilterNode.Filter);
                }

                Reload(false);
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Move the selected item into a chosen folder
        /// </summary>
        private void OnMoveIntoFolder(object sender, EventArgs e)
        {
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            using (ChooseFilterLocationDlg dlg = new ChooseFilterLocationDlg("Move", selected, ActiveFilterTree.GetFolderState()))
            {
                dlg.SelectedFolder = selected.ParentNode;
                dlg.Saving += new CancelEventHandler(OnMovingFilter);

                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// A user has confirmed where he wants to move the filter
        /// </summary>
        void OnMovingFilter(object sender, CancelEventArgs e)
        {
            ChooseFilterLocationDlg locationDlg = (ChooseFilterLocationDlg) sender;
            FilterNodeEntity selected = locationDlg.SubjectNode;

            // See if they did anything
            if (locationDlg.SelectedFolder == selected.ParentNode)
            {
                // In this case we don't set e.Cancel to false, b\c we do want the dlg to close.
                return;
            }

            if (!ConfirmMoveFilterNode(selected, locationDlg))
            {
                e.Cancel = true;
                return;
            }

            SaveSelectionState();

            try
            {

                List<FilterNodeEntity> nodes = layoutContext.Move(selected, locationDlg.SelectedFolder, 0);

                FilterContentManager.CheckForChanges();

                Reload(false);
                ActiveFilterTree.SelectedFilterNode = FindChild(locationDlg.SelectedFolder, nodes);
            }
            catch (FilterInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(locationDlg, ex.Message);
                e.Cancel = true;
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(locationDlg, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Confirm with the user that moving the given node is ok
        /// </summary>
        private bool ConfirmMoveFilterNode(FilterNodeEntity node, IWin32Window parent)
        {
            Cursor.Current = Cursors.WaitCursor;

            // After we move this node, all the other soft-links will have been automatically deleted
            List<FilterNodeEntity> softLinksToDelete = FilterHelper.GetNodesUsingSequence(node.FilterSequence).Where(n => n != node).ToList();

            List<string> reasons = ObjectReferenceManager.GetReferenceReasons(softLinksToDelete.Select(n => n.FilterNodeID).ToList());
            if (reasons.Count > 0)
            {
                using (DeleteMoveDlg dlg = new DeleteMoveDlg(reasons, node.Filter.IsFolder))
                {
                    if (dlg.ShowDialog(parent) == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Create a link between the selected item to some other location
        /// </summary>
        private void OnCreateLink(object sender, EventArgs e)
        {
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            using (ChooseFilterLocationDlg dlg = new ChooseFilterLocationDlg("Link", selected, ActiveFilterTree.GetFolderState()))
            {
                dlg.SelectedFolder = selected.ParentNode;
                dlg.Saving += new CancelEventHandler(OnLinkingFilter);

                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// User has confirmed where a filter should be linked
        /// </summary>
        void OnLinkingFilter(object sender, CancelEventArgs e)
        {
            ChooseFilterLocationDlg dlg = (ChooseFilterLocationDlg) sender;
            FilterNodeEntity selected = dlg.SubjectNode;

            SaveSelectionState();

            try
            {
                List<FilterNodeEntity> nodes = layoutContext.AddLink(selected, dlg.SelectedFolder, 0);

                Reload(false);
                ActiveFilterTree.SelectedFilterNode = FindChild(dlg.SelectedFolder, nodes);
            }
            catch (FilterInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(dlg, ex.Message);

                e.Cancel = true;
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(dlg, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Delete the selected node
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (!delete.Enabled)
            {
                return;
            }

            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;
            FilterEntity filter = selected.Filter;

            // If this turns false, we are only deleting the particular selected link
            bool deleteAllInstances = true;

            // The easy case
            if (!filter.IsFolder)
            {
                // If its not hard linked, we can just delete it
                if (!FilterHelper.IsFilterHardLinked(filter))
                {
                    using (DeleteObjectReferenceDlg dlg = new DeleteObjectReferenceDlg(
                        string.Format("Delete the filter '{0}'?", filter.Name), 
                        FilterHelper.GetNodesUsingFilter(filter).Select(n => n.FilterNodeID).ToList()))
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                    }
                }
                // If its hard linked, we have to know if they want to delete all its instances, or just the selected
                else
                {
                    using (DeleteHardLinkedFilterDlg dlg = new DeleteHardLinkedFilterDlg(selected))
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }

                        deleteAllInstances = dlg.DeleteAllLinks;
                    }
                }
            }

            // Folders
            else
            {
                // See if its hard linked
                if (FilterHelper.IsFilterHardLinked(filter))
                {
                    using (DeleteHardLinkedFolderDlg dlg = new DeleteHardLinkedFolderDlg(selected))
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }

                        deleteAllInstances = dlg.DeleteAllLinks;
                    }
                }
                else
                {
                    using (DeleteFolderDlg dlg = new DeleteFolderDlg(filter))
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                    }
                }
            }

            SaveSelectionState();

            try
            {
                // Do the delete
                if (deleteAllInstances)
                {
                    layoutContext.DeleteFilter(filter);
                }
                else
                {
                    layoutContext.DeleteLink(selected);
                }

                FilterContentManager.CheckForChanges();

                // Easist way is to just rebuild the tree completely
                Reload(false);
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// The user has dropped a node somewhere
        /// </summary>
        private void OnDragDropComplete(object sender, FilterDragDropCompleteEventArgs e)
        {
            e.AutoClearDropIndicator = false;

            contextMenuDragDrop.Tag = e;

            menuItemMove.Enabled = e.MoveLocation != null;

            menuItemLink.Enabled = e.CopyLocation != null;
            menuItemCopy.Enabled = e.CopyLocation != null;
            menuItemCopy.Visible = !e.FilterNode.Filter.IsFolder;

            contextMenuDragDrop.Show(Cursor.Position);
        }

        /// <summary>
        /// The drag-drop menu has closed
        /// </summary>
        private void OnDragDropMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            FilterDragDropCompleteEventArgs args = (FilterDragDropCompleteEventArgs) strip.Tag;

            args.ClearDropIndicator();
        }

        /// <summary>
        /// Out of the given list of nodes, determine which one is a child of the given parent
        /// </summary>
        private FilterNodeEntity FindChild(FilterNodeEntity parent, List<FilterNodeEntity> nodes)
        {
            foreach (FilterNodeEntity node in nodes)
            {
                if (node.ParentNode == parent)
                {
                    return node;
                }
            }

            return null;
        }

        /// <summary>
        /// Finish a drag & drop move operation
        /// </summary>
        private void OnDragDropMove(object sender, EventArgs e)
        {
            FilterDragDropCompleteEventArgs dropInfo = (FilterDragDropCompleteEventArgs) ((ToolStripMenuItem) sender).Owner.Tag;

            if (!ConfirmMoveFilterNode(dropInfo.FilterNode, this))
            {
                return;
            }

            SaveSelectionState();

            try
            {
                List<FilterNodeEntity> nodes = layoutContext.Move(dropInfo.FilterNode, dropInfo.MoveLocation.ParentNode, dropInfo.MoveLocation.Position);

                FilterContentManager.CheckForChanges();

                Reload(false);
                ActiveFilterTree.SelectedFilterNode = FindChild(dropInfo.MoveLocation.ParentNode, nodes);
            }
            catch (FilterInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Finish a drag & drop link operation
        /// </summary>
        private void OnDragDropLink(object sender, EventArgs e)
        {
            FilterDragDropCompleteEventArgs dropInfo = (FilterDragDropCompleteEventArgs) ((ToolStripMenuItem) sender).Owner.Tag;

            SaveSelectionState();

            try
            {
                List<FilterNodeEntity> nodes = layoutContext.AddLink(dropInfo.FilterNode, dropInfo.CopyLocation.ParentNode, dropInfo.CopyLocation.Position);

                Reload(false);
                ActiveFilterTree.SelectedFilterNode = FindChild(dropInfo.CopyLocation.ParentNode, nodes);
            }
            catch (FilterInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Finish a drag & drop copy operation
        /// </summary>
        private void OnDragDropCopy(object sender, EventArgs e)
        {
            FilterDragDropCompleteEventArgs dropInfo = (FilterDragDropCompleteEventArgs) ((ToolStripMenuItem) sender).Owner.Tag;

            SaveSelectionState();

            try
            {
                List<FilterNodeEntity> nodes = layoutContext.Copy(dropInfo.FilterNode.Filter, dropInfo.CopyLocation.ParentNode, dropInfo.CopyLocation.Position);

                FilterContentManager.CheckForChanges();

                Reload(false);
                ActiveFilterTree.SelectedFilterNode = FindChild(dropInfo.CopyLocation.ParentNode, nodes);
            }
            catch (FilterInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload(true);
            }
        }

        /// <summary>
        /// Create a copy of the selection
        /// </summary>
        private void OnCopy(object sender, EventArgs e)
        {
            FilterNodeEntity selected = ActiveFilterTree.SelectedFilterNode;

            using (ChooseFilterLocationDlg dlg = new ChooseFilterLocationDlg("Copy", selected, ActiveFilterTree.GetFolderState()))
            {
                dlg.SelectedFolder = selected.ParentNode;
                dlg.Saving += new CancelEventHandler(OnCopyingFilter);

                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// A user has confirmed where he wants to move the filter
        /// </summary>
        void OnCopyingFilter(object sender, CancelEventArgs e)
        {
            ChooseFilterLocationDlg dlg = (ChooseFilterLocationDlg) sender;
            FilterNodeEntity selected = dlg.SubjectNode;

            SaveSelectionState();

            try
            {
                List<FilterNodeEntity> nodes = layoutContext.Copy(selected.Filter, dlg.SelectedFolder, 0);

                FilterContentManager.CheckForChanges();

                Reload(false);
                ActiveFilterTree.SelectedFilterNode = FindChild(dlg.SelectedFolder, nodes);
            }
            catch (FilterInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(dlg, ex.Message);

                e.Cancel = true;
            }
            catch (FilterException ex)
            {
                MessageHelper.ShowError(dlg, ex.Message);

                Reload(true);
            }
        }
        
        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            // Pop the scope we pushed in the constructor.  For an explanation of why we do this see the Push call.
            FilterLayoutContext.PopScope();
        }

        /// <summary>
        /// Show or hide filters
        /// </summary>
        private void OnShowDisabledFiltersCheckedChanged(object sender, EventArgs e)
        {
            ReloadFilterTree(filterTreeCustomers);
            ReloadFilterTree(filterTreeOrders);
        }

        /// <summary>
        /// Reload the specified filter tree
        /// </summary>
        private void ReloadFilterTree(FilterTree filterTree)
        {
            if (!showDisabledFilters.Checked &&
                filterTree.SelectedFilterNode != null &&
                filterTree.SelectedFilterNode.Filter.State != (int)FilterState.Enabled)
            {
                filterTree.SelectFirstNode();    
            }

            filterTree.HideDisabledFilters = !showDisabledFilters.Checked;
            filterTree.ReloadLayouts();
        }
    }
}