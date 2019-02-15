using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Grid;
using ShipWorks.Messaging.Messages;
using ShipWorks.UI;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Encapsulates the GUI interaction and persistence logic required for filter editing and creation.
    /// </summary>
    public static class FilterEditingService
    {
        /// <summary>
        /// Create a new filter with the given definition
        /// </summary>
        public static (FilterEditingResult result, FilterNodeEntity createdNode) NewFilter(
                IWin32Window parent,
                FilterDefinition defaultFilterDefinition) =>
            NewFilter(false, null, null, parent, defaultFilterDefinition);

        /// <summary>
        /// Create a new filter or folder
        /// </summary>
        public static (FilterEditingResult result, FilterNodeEntity createdNode) NewFilter(
                bool isFolder,
                FilterNodeEntity browserInitialParent,
                FolderExpansionState browserInitialState,
                IWin32Window parent) =>
            NewFilter(isFolder, browserInitialParent, browserInitialState, parent, null);

        /// <summary>
        /// Create a new filter or folder
        /// </summary>
        public static (FilterEditingResult result, FilterNodeEntity createdNode) NewFilter(
            bool isFolder,
            FilterNodeEntity browserInitialParent,
            FolderExpansionState browserInitialState,
            IWin32Window parent,
            FilterDefinition defaultFilterDefinition)
        {
            using (AddFilterWizard wizard = new AddFilterWizard(isFolder, browserInitialState, browserInitialParent))
            {
                wizard.DefaultFilterDefinition = defaultFilterDefinition;
                DialogResult result = wizard.ShowDialog(parent);

                if (result == DialogResult.OK)
                {
                    var primaryCreatedNode = FindChild(wizard.ParentFilterNode, wizard.CreatedNodes);

                    FilterContentManager.CheckForChanges();

                    return (FilterEditingResult.OK, primaryCreatedNode);
                }

                if (result == DialogResult.Cancel)
                {
                    return (FilterEditingResult.Cancel, null);
                }

                return (FilterEditingResult.Error, null);
            }
        }

        /// <summary>
        /// Edit the filter with GUI controls, with GUI having the specified parent, using the given layout for save logic.
        /// </summary>
        public static FilterEditingResult EditFilter(FilterNodeEntity filterNode, Control parent)
        {
            if (filterNode == null)
            {
                throw new ArgumentNullException("filterNode");
            }

            FilterEditingResult result = FilterEditingResult.Cancel;

            FilterEntity filter = filterNode.Filter;

            FilterEditorDlg dlg = new FilterEditorDlg(filterNode);
            dlg.Saving += delegate (object sender, CancelEventArgs e)
            {
                try
                {
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        // Save the filter
                        FilterLayoutContext.Current.SaveFilter(filter, adapter);

                        // Quick filter's should never be displayed as grid content
                        if (filterNode.Purpose != (int) FilterNodePurpose.Quick)
                        {
                            // Get the grid layouts
                            FilterNodeColumnSettings userSettings = FilterNodeColumnManager.GetUserSettings(filterNode);
                            FilterNodeColumnSettings derfaultSettings = FilterNodeColumnManager.GetDefaultSettings(filterNode);

                            userSettings.Save(adapter);
                            derfaultSettings.Save(adapter);
                        }

                        adapter.Commit();
                    }

                    FilterContentManager.CheckForChanges();

                    result = FilterEditingResult.OK;

                    Messenger.Current.Send(new FilterNodeEditedMessage(parent, filterNode));
                }
                catch (FilterException ex)
                {
                    MessageHelper.ShowError(dlg, ex.Message);

                    e.Cancel = true;
                    dlg.DialogResult = DialogResult.Cancel;

                    result = FilterEditingResult.Error;
                }
            };

            // This was added due to the "Create\Edit" links for editing the Quick Filter that you can get
            // to through the FilterComboBox.  If the parent is ultimately a PopupWindow, then the modal dlg
            // opens _under_ the main app.  Bad.
            PopupWindow popup = parent.TopLevelControl as PopupWindow;
            if (popup != null)
            {
                popup.TopMost = false;
            }

            dlg.ShowDialog(parent);
            dlg.Dispose();

            if (popup != null)
            {
                popup.TopMost = true;
            }

            return result;
        }

        /// <summary>
        /// Out of the given list of nodes, determine which one is a child of the given parent
        /// </summary>
        private static FilterNodeEntity FindChild(FilterNodeEntity parent, List<FilterNodeEntity> nodes)
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

    }
}
