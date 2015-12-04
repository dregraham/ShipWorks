using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.UI;
using ShipWorks.Users;
using Microsoft.Win32;
using ShipWorks.Templates.Controls;
using System.Windows.Forms.VisualStyles;
using Interapptive.Shared;
using ShipWorks.UI.Controls;
using ShipWorks.Filters;
using ShipWorks.UI.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.MessageBoxes;
using ShipWorks.Users.Security;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;
using ShipWorks.Properties;

namespace ShipWorks.Templates.Management
{
    /// <summary>
    /// Window for managing templates
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class TemplateManagerDlg : Form
    {
        FolderExpansionState folderState;
        
        // Saved selection state
        long lastSelectedFallback;
        long lastSelectedID;

        SizeGripRenderer sizeGripRenderer;

        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateManagerDlg()
        {
            InitializeComponent();

            WindowStateSaver windowSaver = new WindowStateSaver(this);
            windowSaver.ManageSplitter(splitContainer);

            templatePreview.Initialize();

            sizeGripRenderer = new SizeGripRenderer(this);
            ThemedBorderProvider themedBorder = new ThemedBorderProvider(panelPreview);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            panelEditControls.Visible = UserSession.Security.HasPermission(PermissionType.ManageTemplates);
            if (!panelEditControls.Visible)
            {
                treeControl.ContextMenuStrip = null;
                splitContainer.Width = panelEditControls.Right - splitContainer.Left;
            }

            folderState = new FolderExpansionState(UserSession.User.Settings.TemplateExpandedFolders);
            lastSelectedID = UserSession.User.Settings.TemplateLastSelected;

            Reload();
        }

        /// <summary>
        /// Load the template tree, apply initial selection, and folder expands
        /// </summary>
        private void LoadTemplateTree()
        {
            treeControl.LoadTemplates(TemplateManager.Tree.CreateEditableClone());
            treeControl.ApplyFolderState(folderState);
        }

        /// <summary>
        /// Save the selection stae of the tree, so it can be restored on a reload
        /// </summary>
        private void SaveSelectionState()
        {
            folderState = treeControl.GetFolderState();

            // Set the last selected
            lastSelectedID = treeControl.SelectedID;

            TemplateTreeNode relative = treeControl.GetClosestRelative(treeControl.SelectedTemplateTreeNode);
            lastSelectedFallback = (relative != null) ? relative.ID : 0;

            Cursor.Current = Cursors.WaitCursor;
        }

        /// <summary>
        /// Reload the template tree.  If completeReload is true, instead of checking for changes, the tempalte manager completely 
        /// reloads all templates and folders.
        /// </summary>
        private void Reload()
        {
            Cursor.Current = Cursors.WaitCursor;

            TemplateManager.CheckForChangesNeeded();

            treeControl.SelectedTemplateNodeChanged -= new TemplateNodeChangedEventHandler(OnSelectedTemplateNodeChanged);

            LoadTemplateTree();

            // Try to select the last
            treeControl.SelectedID = lastSelectedID;

            // If that didn't work, try the fallback
            if (treeControl.SelectedTemplateTreeNode == null)
            {
                treeControl.SelectedID = lastSelectedFallback;
            }

            treeControl.SelectedTemplateNodeChanged += new TemplateNodeChangedEventHandler(OnSelectedTemplateNodeChanged);

            UpdateButtonState();
            UpdatePreview();
        }

        /// <summary>
        /// The selected template\folder has changed
        /// </summary>
        private void OnSelectedTemplateNodeChanged(object sender, TemplateNodeChangedEventArgs e)
        {
            SavePreviewSettings();

            UpdateButtonState();
            UpdatePreview();

            treeControl.Focus();
        }

        /// <summary>
        /// Update the template preview display
        /// </summary>
        private void UpdatePreview()
        {
            if (treeControl.SelectedTemplateTreeNode == null || treeControl.SelectedTemplateTreeNode.IsFolder)
            {
                templatePreview.ClearPreview();
            }
            else
            {
                templatePreview.LoadPreview(treeControl.SelectedTemplateTreeNode.Template);
            }
        }

        /// <summary>
        /// Update the UI state of the buttons
        /// </summary>
        private void UpdateButtonState()
        {
            bool hasSelection = treeControl.SelectedTemplateTreeNode != null;

            // We don't allow any editing for builtin folders.  For UI purposes that's the same as just not having anything selected
            if (hasSelection && treeControl.SelectedTemplateTreeNode.IsFolder && treeControl.SelectedTemplateTreeNode.Folder.IsBuiltin)
            {
                hasSelection = false;
            }

            edit.Enabled = hasSelection && !treeControl.SelectedTemplateTreeNode.IsFolder;
            rename.Enabled = hasSelection;
            delete.Enabled = hasSelection;

            newTemplate.Enabled = treeControl.TemplateTree.RootFolders.Count > 0;
            newFolder.Enabled = true;
            copy.Enabled = hasSelection;

            if (treeControl.SelectedTemplateTreeNode != null && treeControl.SelectedTemplateTreeNode.IsSnippetNode)
            {
                newTemplate.Text = "New Snippet";
                newTemplate.Image = Resources.template_snippet_add;
            }
            else
            {
                newTemplate.Text = "New Template";
                newTemplate.Image = Resources.template_add;
            }

            moveIntoFolder.Enabled = hasSelection;
        }

        /// <summary>
        /// User clicked to rename the selected item
        /// </summary>
        private void OnRename(object sender, EventArgs e)
        {
            treeControl.BeginRename();
        }

        /// <summary>
        /// A template or folder is being renamed
        /// </summary>
        private void OnAfterRename(object sender, TemplateNodeRenameEventArgs e)
        {
            e.TemplateTreeNode.Name = e.Proposed;

            SaveSelectionState();

            string errorMessage = null;

            try
            {
                if (e.TemplateTreeNode.IsFolder)
                {
                    TemplateEditingService.SaveFolder(e.TemplateTreeNode.Folder);
                }
                else
                {
                    TemplateEditingService.SaveTemplate(e.TemplateTreeNode.Template);
                }
            }
            catch (TemplateException ex)
            {
                errorMessage = ex.Message;
            }

            // Changing names could affect imports, so we do need to reload.  The grid doesnt like
            // a reload happening in the middle of its callback to AfterRename, so we need to BeginInvoke this.
            BeginInvoke(new MethodInvoker(() =>
                {
                    Reload();

                    if (errorMessage != null)
                    {
                        MessageHelper.ShowError(this, errorMessage);
                    }
                }));
        }

        /// <summary>
        /// Move the selected template or folder into another folder
        /// </summary>
        private void OnMoveIntoFolder(object sender, EventArgs e)
        {
            TemplateTreeNode selected = treeControl.SelectedTemplateTreeNode;

            using (ChooseTemplateLocationDlg dlg = new ChooseTemplateLocationDlg("Move", selected, treeControl.GetFolderState()))
            {
                dlg.SelectedFolder = selected.ParentFolder;
                dlg.Saving += new CancelEventHandler(OnMovingTemplate);

                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// A user has confirmed where he wants to move the template
        /// </summary>
        void OnMovingTemplate(object sender, CancelEventArgs e)
        {
            ChooseTemplateLocationDlg dlg = (ChooseTemplateLocationDlg) sender;
            TemplateTreeNode selected = dlg.SubjectNode;

            try
            {
                MoveTemplate(selected, dlg.SelectedFolder);
            }
            catch (TemplateInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(dlg, ex.Message);

                e.Cancel = true;
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(dlg, ex.Message);
                Reload();
            }
        }

        /// <summary>
        /// Move the given node to the specified target folder.  No exeptions are caught.
        /// </summary>
        private void MoveTemplate(TemplateTreeNode selected, TemplateFolderEntity targetFolder)
        {
            // See if they did anything
            if (targetFolder == selected.ParentFolder)
            {
                return;
            }

            if (selected.IsFolder && targetFolder == selected.Folder)
            {
                return;
            }

            SaveSelectionState();

            TemplateEditingService.MoveToFolder(selected, targetFolder);

            // Reload and reselect
            Reload();
            treeControl.SelectedTemplateTreeNode = selected;
        }

        /// <summary>
        /// Create a new template
        /// </summary>
        private void OnNewTemplate(object sender, EventArgs e)
        {
            SaveSelectionState();

            TemplateTreeNode selected = treeControl.SelectedTemplateTreeNode;
            TemplateFolderEntity initialFolder = null;

            if (selected != null)
            {
                initialFolder = selected.IsFolder ? selected.Folder : selected.ParentFolder;
            }

            // Creating a snippet
            if (initialFolder != null && initialFolder.IsSnippetsOrDescendantFolder)
            {
                using (AddSnippetDlg dlg = new AddSnippetDlg(initialFolder, treeControl.GetFolderState()))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Reload();
                        treeControl.SelectedID = dlg.Template.TemplateID;
                    }
                    else
                    {
                        Reload();
                    }
                }
            }
            // Creating a template
            else
            {
                if (initialFolder == null)
                {
                    initialFolder = treeControl.TemplateTree.RootFolders[0];
                }

                using (AddTemplateWizard dlg = new AddTemplateWizard(initialFolder, treeControl.GetFolderState()))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        Reload();
                        treeControl.SelectedID = dlg.Template.TemplateID;
                    }
                    else
                    {
                        Reload();
                    }
                }
            }
        }

        /// <summary>
        /// Create a new template folder
        /// </summary>
        private void OnNewFolder(object sender, EventArgs e)
        {
            SaveSelectionState();

            string defaultName = "New Folder";

            using (AddTemplateFolderDlg dlg = new AddTemplateFolderDlg(defaultName, treeControl.GetFolderState()))
            {
                TemplateTreeNode selected = treeControl.SelectedTemplateTreeNode;

                if (selected != null)
                {
                    dlg.SelectedFolder = selected.IsFolder ? selected.Folder : selected.ParentFolder;
                }

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        TemplateFolderEntity folder = new TemplateFolderEntity();
                        folder.Name = dlg.FolderName;
                        folder.ParentFolder = dlg.SelectedFolder;
                        folder.TemplateTree = treeControl.TemplateTree;

                        TemplateEditingService.SaveFolder(folder);

                        // Reload and reselect
                        Reload();
                        treeControl.SelectedTemplateTreeNode = new TemplateTreeNode(folder);
                    }
                    catch (TemplateException ex)
                    {
                        MessageHelper.ShowError(dlg, ex.Message);

                        Reload();
                    }
                }
            }
        }

        /// <summary>
        /// Open the editor for the selected template
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            SaveSelectionState();
            SavePreviewSettings();

            using (TemplateEditorDlg dlg = new TemplateEditorDlg(treeControl.SelectedTemplateTreeNode.Template))
            {
                dlg.ShowDialog(this);
            }

            Reload();
        }

        /// <summary>
        /// Delete the selected template or folder
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            TemplateTreeNode selected = treeControl.SelectedTemplateTreeNode;
            string message = selected.IsFolder ?
                string.Format("Delete folder '{0}' and all of its contents?", selected.Name) :
                string.Format("Delete template '{0}'?", selected.Name);

            using (DeleteObjectReferenceDlg dlg = new DeleteObjectReferenceDlg(message, GetTemplateOrDescendants(selected)))
            {
                if (dlg.ShowDialog(this) == DialogResult.Cancel)
                {
                    return;
                }
            }

            SaveSelectionState();

            try
            {
                if (selected.IsFolder)
                {
                    TemplateEditingService.DeleteFolder(selected.Folder);
                }
                else
                {
                    TemplateEditingService.DeleteTemplate(selected.Template);
                }
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }

            Reload();
        }

        /// <summary>
        /// Get the ID of of the template of the node represents a template, or the ID of every descendant template
        /// if the node represents a folder.
        /// </summary>
        private List<long> GetTemplateOrDescendants(TemplateTreeNode node)
        {
            List<long> keys = new List<long>();

            if (node.IsFolder)
            {
                foreach (TemplateFolderEntity folder in node.Folder.ChildFolders)
                {
                    keys.AddRange(GetTemplateOrDescendants(new TemplateTreeNode(folder)));
                }

                foreach (TemplateEntity template in node.Folder.Templates)
                {
                    keys.Add(template.TemplateID);
                }
            }
            else
            {
                keys.Add(node.ID);
            }

            return keys;
        }

        /// <summary>
        /// Copy the selected template or folder
        /// </summary>
        private void OnCopy(object sender, EventArgs e)
        {
            TemplateTreeNode selected = treeControl.SelectedTemplateTreeNode;

            using (ChooseTemplateLocationDlg dlg = new ChooseTemplateLocationDlg("Copy", selected, treeControl.GetFolderState()))
            {
                dlg.SelectedFolder = selected.ParentFolder;
                dlg.Saving += new CancelEventHandler(OnCopyingTemplate);

                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Copy the template or folder
        /// </summary>
        void OnCopyingTemplate(object sender, CancelEventArgs e)
        {
            ChooseTemplateLocationDlg dlg = (ChooseTemplateLocationDlg) sender;
            TemplateTreeNode selected = dlg.SubjectNode;

            try
            {
                CopyTemplate(selected, dlg.SelectedFolder);
            }
            catch (TemplateInvalidLocationException ex)
            {
                MessageHelper.ShowInformation(dlg, ex.Message);

                e.Cancel = true;
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(dlg, ex.Message);

                Reload();
            }
        }

        /// <summary>
        /// Copy the given template node into the specified target folder.  No exceptions are caught.
        /// </summary>
        private void CopyTemplate(TemplateTreeNode selected, TemplateFolderEntity targetFolder)
        {
            SaveSelectionState();

            TemplateTreeNode copy = TemplateEditingService.CopyToFolder(selected, targetFolder);

            // Reload and reselect
            Reload();
            treeControl.SelectedTemplateTreeNode = copy;
        }

        /// <summary>
        /// The context menu is opening
        /// </summary>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            TemplateTreeNode selectedNode = treeControl.SelectedTemplateTreeNode;

            menuItemEdit.Enabled = edit.Enabled;
            menuItemNewFolder.Enabled = newFolder.Enabled;
            menuItemNewTemplate.Enabled = newTemplate.Enabled;
            menuItemNewSnippet.Enabled = newTemplate.Enabled;
            menuItemContextMove.Enabled = moveIntoFolder.Enabled;
            menuItemContextCopy.Enabled = copy.Enabled;
            menuItemContextRename.Enabled = rename.Enabled;
            deleteToolStripMenuItem.Enabled = delete.Enabled;

            // Reset them all to true - we'll turn them off as we check more things...
            foreach (ToolStripItem item in contextMenuTree.Items)
            {
                item.Available = true;
            }

            menuItemNewSnippet.Available = false;

            if (selectedNode != null)
            {
                // For the builtin folders (System, Snippets) dont show anything but "New" on them - they cant be moved, copied, or edited
                if (selectedNode.IsFolder && selectedNode.Folder.IsBuiltin)
                {
                    foreach (ToolStripItem item in contextMenuTree.Items)
                    {
                        if (item != menuItemNewFolder &&
                            item != menuItemNewTemplate &&
                            item != menuItemNewSnippet)
                        {
                            item.Available = false;
                        }
                    }
                }

                // For snippets, you don't create templates, you create snippets
                if (selectedNode.IsSnippetNode)
                {
                    menuItemNewSnippet.Available = true;
                    menuItemNewTemplate.Available = false;
                }
                else
                {
                    menuItemNewSnippet.Available = false;
                }
            }
        }

        /// <summary>
        /// A drag-drop operation completed
        /// </summary>
        private void OnDragDropComplete(object sender, TemplateDragDropCompleteEventArgs e)
        {
            e.AutoClearDropIndicator = false;

            contextMenuDragDrop.Tag = e;
            contextMenuDragDrop.Show(Cursor.Position);
        }

        /// <summary>
        /// The drag-drop menu has closed
        /// </summary>
        private void OnDragDropMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ContextMenuStrip strip = (ContextMenuStrip) sender;
            TemplateDragDropCompleteEventArgs args = (TemplateDragDropCompleteEventArgs) strip.Tag;

            args.ClearDropIndicator();
        }

        /// <summary>
        /// Execute the drag & drop move operation
        /// </summary>
        private void OnDragDropMove(object sender, EventArgs e)
        {
            TemplateDragDropCompleteEventArgs dropInfo = (TemplateDragDropCompleteEventArgs) ((ToolStripMenuItem) sender).Owner.Tag;

            try
            {
                MoveTemplate(dropInfo.DraggedNode, dropInfo.TargetFolder);
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload();
            }
        }

        /// <summary>
        /// Execute the drag & drop copy operation
        /// </summary>
        private void OnDragDropCopy(object sender, EventArgs e)
        {
            TemplateDragDropCompleteEventArgs dropInfo = (TemplateDragDropCompleteEventArgs) ((ToolStripMenuItem) sender).Owner.Tag;

            try
            {
                CopyTemplate(dropInfo.DraggedNode, dropInfo.TargetFolder);
            }
            catch (TemplateException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                Reload();
            }
        }

        /// <summary>
        /// Save the current preview settings to the template
        /// </summary>
        private void SavePreviewSettings()
        {
            if (templatePreview.ActiveTemplate != null)
            {
                templatePreview.SaveSettingsToTemplate();
                TemplateEditingService.SaveTemplate(templatePreview.ActiveTemplate);
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SavePreviewSettings();
            templatePreview.ClearPreview();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                UserSettingsEntity settings = UserSession.User.Settings;
                settings.TemplateExpandedFolders = treeControl.GetFolderState().GetState();
                settings.TemplateLastSelected = treeControl.SelectedID;

                adapter.SaveAndRefetch(UserSession.User.Settings);
            }
        }
    }
}