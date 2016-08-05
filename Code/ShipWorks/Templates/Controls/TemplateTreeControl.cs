using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Divelements.SandGrid;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Management;
using ShipWorks.UI.Controls.SandGrid;
using ShipWorks.UI.Utility;

namespace ShipWorks.Templates.Controls
{
    /// <summary>
    /// Control for displaying and organizing templates
    /// </summary>
    public partial class TemplateTreeControl : UserControl
    {
        // Raised when the selected node changes
        public event TemplateNodeChangedEventHandler SelectedTemplateNodeChanged;

        /// <summary>
        /// Raised when the user lets go of something while dragging in a valid location
        /// </summary>
        public event TemplateDragDropCompleteEventHandler DragDropComplete;

        /// <summary>
        /// Raised after a node finishes the renaming process
        /// </summary>
        public event TemplateNodeRenameEventHandler TemplateNodeRenamed;

        // Raised to indiciate the delete key has been pressed
        public event EventHandler DeleteKeyPressed;

        // Show a root folder for all the top-level folder's parent
        bool showFoldersRoot = false;

        // Shows the Snippets folder (or not)
        TemplateTreeSnippetDisplayType snippetDisplay = TemplateTreeSnippetDisplayType.Hidden;

        // Used so we can give the OldNode to the TemplateNodeChanged event
        TemplateTreeNode lastSelectedNode;

        // What the tree control was loaded from
        TemplateTree templateTree;

        // Indicates if the "live" TemplateManager.TemplateTree was loaded or not
        bool liveTreeLoaded = true;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateTreeControl()
        {
            InitializeComponent();

            ThemedBorderProvider themedBorder = new ThemedBorderProvider(this);
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
        /// Controls if only folders will be displayed.
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
        /// Controls if a root folder will be displayed that the top-level folders will be contained in.  Does not take effect until the next time LoadTemplates is called.
        /// </summary>
        [DefaultValue(false)]
        [Category("Behavior")]
        public bool ShowFoldersRoot
        {
            get
            {
                return showFoldersRoot;
            }
            set
            {
                showFoldersRoot = value;
            }
        }

        /// <summary>
        /// Controls if the snippets folder is displayed
        /// </summary>
        [DefaultValue(TemplateTreeSnippetDisplayType.Hidden)]
        [Category("Behavior")]
        public TemplateTreeSnippetDisplayType SnippetDisplay
        {
            get { return snippetDisplay; }
            set { snippetDisplay = value; }
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

        [DefaultValue(false)]
        [Category("Behavior")]
        public bool ShowManageTemplates
        {
            get
            {
                return panelManageTemplates.Visible;
            }
            set
            {
                if (value && !liveTreeLoaded)
                {
                    // Can't b\c after the Template Manager window closes, there would be no way to load the updated tree changes
                    throw new InvalidOperationException("Cannot allow managing templates when a live tree is not loaded.");
                }

                panelManageTemplates.Visible = value;
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

                // Account fo the "Manage Templates" section
                height += this.panelManageTemplates.Height;

                // Account for scroll bar space;
                height += SystemInformation.HorizontalScrollBarHeight;

                // Let SandGrid figure out how wide it should be
                int width = this.gridColumnTemplate.GetMaximumCellWidth(RowScope.AllRows, false);

                // Account for the fact that filters will draw there counts
                width += 50;

                return new Size(width, height);
            }
        }

        /// <summary>
        /// The tree that was used to load the control
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemplateTree TemplateTree
        {
            get { return templateTree; }
        }

        /// <summary>
        /// Returns the currently selected TemplateTreeNode
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemplateTreeNode SelectedTemplateTreeNode
        {
            get
            {
                if (sandGrid.Rows.Count == 0)
                {
                    return null;
                }

                if (sandGrid.SelectedElements.Count == 1)
                {
                    TemplateTreeGridRow row = sandGrid.SelectedElements[0] as TemplateTreeGridRow;
                    return row.TemplateTreeNode;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    sandGrid.SelectedElements.Clear();
                    return;
                }

                if (value == TemplateTreeNode.RootNode)
                {
                    if (showFoldersRoot)
                    {
                        sandGrid.Rows[0].Selected = true;
                    }

                    return;
                }

                TemplateTreeGridRow rowToSelect = FindRow(value.ID);
                if (rowToSelect != null)
                {
                    rowToSelect.Selected = true;
                    EnsureNodeVisible(rowToSelect.TemplateTreeNode);
                }
                else
                {
                    sandGrid.SelectedElements.Clear();
                }
            }
        }

        /// <summary>
        /// Returns the database ID of the currently selected template or folder
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long SelectedID
        {
            get
            {
                TemplateTreeNode selected = SelectedTemplateTreeNode;

                if (selected == null)
                {
                    return 0;
                }

                if (selected.IsRoot)
                {
                    return -1;
                }

                return selected.ID;
            }
            set
            {
                if (value == 0)
                {
                    SelectedTemplateTreeNode = null;
                }

                else if (value == -1)
                {
                    SelectedTemplateTreeNode = TemplateTreeNode.RootNode;
                }

                else
                {
                    TemplateTreeGridRow rowToSelect = FindRow(value);
                    if (rowToSelect != null)
                    {
                        SelectedTemplateTreeNode = rowToSelect.TemplateTreeNode;
                    }
                    else
                    {
                        SelectedTemplateTreeNode = null;
                    }
                }
            }
        }

        /// <summary>
        /// Load all the templates into the template tree.  The live TemplateManager.Tree is used.
        /// </summary>
        public void LoadTemplates()
        {
            LoadTemplates(TemplateManager.Tree);
        }

        /// <summary>
        /// Load all the templates into the template tree using the specified TemplateTree
        /// </summary>
        public void LoadTemplates(TemplateTree templateTree)
        {
            if (templateTree == null)
            {
                throw new ArgumentNullException("templateTree");
            }

            this.templateTree = templateTree;

            liveTreeLoaded = (templateTree == TemplateManager.Tree);

            if (ShowManageTemplates && !liveTreeLoaded)
            {
                // Can't b\c after the Template Manager window closes, there would be no way to load the updated tree changes
                throw new InvalidOperationException("Cannot load templtes from a non live tree when managing templates is enabled.");
            }

            Clear();

            GridRowCollection rows = null;

            // Add a root for all folders
            if (showFoldersRoot)
            {
                TemplateTreeGridRow rootRow = new TemplateTreeGridRow(TemplateTreeNode.RootNode);
                sandGrid.Rows.Add(rootRow);

                rows = rootRow.NestedRows;
            }
            // All folders are the top-level
            else
            {
                rows = sandGrid.Rows;
            }

            if (snippetDisplay != TemplateTreeSnippetDisplayType.OnlySnippets)
            {
                // Load all the top-level folders
                foreach (TemplateFolderEntity folder in templateTree.RootFolders)
                {
                    LoadFolder(folder, rows);
                }
            }
            else
            {
                TemplateFolderEntity snippetsFolder = templateTree.GetFolder(TemplateBuiltinFolders.SnippetsFolderID);
                if (snippetsFolder != null)
                {
                    LoadFolder(snippetsFolder, rows);
                }
            }

            // The root always starts expanded
            if (showFoldersRoot)
            {
                sandGrid.Rows[0].Expanded = true;
            }
        }

        /// <summary>
        /// Clears the tree of all content
        /// </summary>
        public void Clear()
        {
            sandGrid.Rows.Clear();
        }

        /// <summary>
        /// The selected item in the grid has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseSelectedTemplateNodeChanged();

            lastSelectedNode = SelectedTemplateTreeNode;
        }

        /// <summary>
        /// Raise the SelectedTemplateNodeChanged event
        /// </summary>
        private void RaiseSelectedTemplateNodeChanged()
        {
            TemplateNodeChangedEventHandler handler = SelectedTemplateNodeChanged;
            if (handler != null)
            {
                handler(this, new TemplateNodeChangedEventArgs(lastSelectedNode, SelectedTemplateTreeNode));
            }
        }

        /// <summary>
        /// Load the folder and its contents into the tree
        /// </summary>
        private void LoadFolder(TemplateFolderEntity folder, GridRowCollection parent)
        {
            if (folder.IsSnippetsFolder && SnippetDisplay == TemplateTreeSnippetDisplayType.Hidden)
            {
                return;
            }

            TemplateTreeGridRow folderRow = new TemplateTreeGridRow(new TemplateTreeNode(folder));
            parent.Add(folderRow);

            // Add all child folders
            foreach (TemplateFolderEntity childFolder in folder.ChildFolders)
            {
                LoadFolder(childFolder, folderRow.NestedRows);
            }

            // Add all the templates
            foreach (TemplateEntity template in folder.Templates)
            {
                TemplateTreeGridRow templateRow = new TemplateTreeGridRow(new TemplateTreeNode(template));
                folderRow.NestedRows.Add(templateRow);
            }
        }

        /// <summary>
        /// Apply the state to the folders currently in the tree
        /// </summary>
        public void ApplyFolderState(FolderExpansionState folderState)
        {
            ApplyFolderState(folderState, true);
        }

        /// <summary>
        /// Apply the state to the folders currently in the treee
        /// </summary>
        public void ApplyFolderState(FolderExpansionState folderState, bool defaultExpanded)
        {
            foreach (TemplateTreeGridRow row in sandGrid.GetAllRows())
            {
                // ID would be null for our fake root folder
                if (row.IsFolder && !row.TemplateTreeNode.IsRoot)
                {
                    row.Expanded = folderState.IsExpanded(row.TemplateTreeNode.Entity,
                        defaultExpanded && !row.TemplateTreeNode.Folder.IsBuiltin);
                }
            }
        }

        /// <summary>
        /// Gets \ sets the expand \ collapse state of all of the folders.
        /// </summary>
        public FolderExpansionState GetFolderState()
        {
            FolderExpansionState state = new FolderExpansionState();

            foreach (TemplateTreeGridRow row in sandGrid.GetAllRows())
            {
                // If its a folder that actually has children, save its state
                if (row.IsFolder)
                {
                    if (row.NestedRows.Count > 0)
                    {
                        state.SetExpanded(row.TemplateTreeNode.Entity, row.Expanded);
                    }
                }
            }

            return state;
        }

        /// <summary>
        /// Ensure that the row that represents the specified node is visible
        /// </summary>
        public void EnsureNodeVisible(TemplateTreeNode treeNode)
        {
            if (treeNode == null)
            {
                throw new ArgumentNullException("treeNode");
            }

            TemplateTreeGridRow row = FindRow(treeNode.ID);

            if (row != null)
            {
                GridRow parent = row.ParentRow;
                while (parent != null)
                {
                    parent.Expanded = true;
                    parent = parent.ParentRow;
                }

                row.EnsureVisible();
            }
        }

        /// <summary>
        /// Gets or sets the node that is currently being displayed as hot-tracked
        /// </summary>
        public TemplateTreeNode HotTrackNode
        {
            get
            {
                TemplateTreeGridRow hotTrackRow = sandGrid.HotTrackRow as TemplateTreeGridRow;

                if (hotTrackRow != null)
                {
                    return hotTrackRow.TemplateTreeNode;
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
                    sandGrid.HotTrackRow = FindRow(value.ID);
                }
            }
        }

        /// <summary>
        /// Find the grid row that represents the folder or template with the given database id
        /// </summary>
        private TemplateTreeGridRow FindRow(long id)
        {
            foreach (TemplateTreeGridRow row in sandGrid.GetAllRows())
            {
                if (row.TemplateTreeNode.ID == id)
                {
                    return row;
                }
            }

            return null;
        }

        /// <summary>
        /// Begin a rename operation on the given filter
        /// </summary>
        public void BeginRename()
        {
            if (!Editable)
            {
                throw new InvalidOperationException("Cannot rename a template.  The view is not editable.");
            }

            if (sandGrid.SelectedElements.Count > 0)
            {
                ((GridRow) sandGrid.SelectedElements[0]).BeginEdit();
            }
        }

        /// <summary>
        /// Editing process is beginning
        /// </summary>
        private void OnBeforeEdit(object sender, GridBeforeEditEventArgs e)
        {
            TemplateTreeGridRow row = (TemplateTreeGridRow) e.Row;
            if (row.TemplateTreeNode.IsFolder && row.TemplateTreeNode.Folder.IsBuiltin)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// A user is commiting there changes to an edited node (rename)
        /// </summary>
        private void OnAfterEdit(object sender, GridAfterEditEventArgs e)
        {
            TemplateTreeGridRow row = (TemplateTreeGridRow) e.Row;
            string proposed = e.Value.ToString().Trim();

            if (proposed == row.TemplateTreeNode.Name)
            {
                e.Cancel = true;
                return;
            }

            if (TemplateNodeRenamed != null)
            {
                TemplateNodeRenameEventArgs args = new TemplateNodeRenameEventArgs(row.TemplateTreeNode, proposed);
                TemplateNodeRenamed(this, args);

                e.Cancel = args.Cancel;
            }

            if (!e.Cancel)
            {
                GridRowCollection rows;

                if (row.IsFolder)
                {
                    rows = sandGrid.Rows;
                }
                else
                {
                    rows = row.ParentRow.NestedRows;
                }

                rows.SetSort(new GridColumn[] { sandGrid.Columns[0] }, new ListSortDirection[] { ListSortDirection.Ascending });
            }
        }

        /// <summary>
        /// Get the closest relative node.  First look for a following sibiling. If not found, look for a previous sibling.  If not found, go to the parent.
        /// </summary>
        public TemplateTreeNode GetClosestRelative(TemplateTreeNode treeNode)
        {
            if (treeNode == null)
            {
                return null;
            }

            TemplateTreeGridRow row = FindRow(treeNode.ID);

            GridRowCollection parentRows = row.ParentRow != null ? row.ParentRow.NestedRows : sandGrid.Rows;

            // First try to fallback on the following sibling
            if (parentRows.Count > row.Index + 1)
            {
                return ((TemplateTreeGridRow) parentRows[row.Index + 1]).TemplateTreeNode;
            }

            // Then try the previous sibling
            else if (row.Index != 0)
            {
                return ((TemplateTreeGridRow) parentRows[row.Index - 1]).TemplateTreeNode;
            }

            // If that doesnt work, then go to the parent
            else if (row.ParentRow != null)
            {
                return ((TemplateTreeGridRow) row.ParentRow).TemplateTreeNode;
            }
            else
            {
                return null;
            }
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
        /// A row has been dropped
        /// </summary>
        private void OnGridRowDropped(object sender, GridRowDroppedEventArgs e)
        {
            TemplateTreeNode draggedNode = ((TemplateTreeGridRow) e.SourceRow).TemplateTreeNode;
            TemplateTreeNode targetNode = ((TemplateTreeGridRow) e.TargetRow).TemplateTreeNode;

            DropTargetState where = e.TargetRow.DragDropState;

            TemplateFolderEntity targetFolder = null;

            // If dropping inside, the target must be a folder
            if (where == DropTargetState.DropInside)
            {
                targetFolder = targetNode.Folder;
            }

            else
            {
                targetFolder = targetNode.ParentFolder;
            }

            if (DragDropComplete != null)
            {
                TemplateDragDropCompleteEventArgs args = new TemplateDragDropCompleteEventArgs(draggedNode, targetFolder, e);
                DragDropComplete(this, args);
            }
        }

        /// <summary>
        /// Open the template manager
        /// </summary>
        private void OnManageTemplates(object sender, EventArgs e)
        {
            using (TemplateManagerDlg dlg = new TemplateManagerDlg())
            {
                dlg.ShowDialog(this);
            }

            FolderExpansionState state = GetFolderState();

            LoadTemplates();

            ApplyFolderState(state);
        }
    }
}
