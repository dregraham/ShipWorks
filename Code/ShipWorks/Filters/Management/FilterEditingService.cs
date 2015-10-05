using System;
using System.Collections.Generic;
using System.Text;
using Interapptive.Shared.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using System.ComponentModel;
using Interapptive.Shared;
using ShipWorks.Data;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.UI;
using ShipWorks.Filters.Controls;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.ApplicationCore.Appearance;
using Interapptive.Shared.UI;
using ShipWorks.Filters.Grid;
using ShipWorks.Messages;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Encapsulates the gui interaction and persistance logic required for filter editing and creation.
    /// </summary>
    public static class FilterEditingService
    {
        /// <summary>
        /// Create a new filter or folder
        /// </summary>
        public static FilterEditingResult NewFilter(
            bool isFolder, 
            FilterNodeEntity browserInitialParent, 
            FolderExpansionState browserInitialState, 
            IWin32Window parent,
            out FilterNodeEntity primaryCreatedNode)
        {
            primaryCreatedNode = null;

            using (AddFilterWizard wizard = new AddFilterWizard(isFolder, browserInitialState, browserInitialParent))
            {
                DialogResult result = wizard.ShowDialog(parent);

                if (result == DialogResult.OK)
                {
                    primaryCreatedNode = FindChild(wizard.ParentFilterNode, wizard.CreatedNodes);

                    FilterContentManager.CheckForChanges();

                    return FilterEditingResult.OK;
                }

                if (result == DialogResult.Cancel)
                {
                    return FilterEditingResult.Cancel;
                }

                return FilterEditingResult.Error;
            }
        }

        /// <summary>
        /// Edit the filter with gui controls, with gui having the specified parent, using the given layout for save logic.
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
            dlg.Saving += delegate(object sender, CancelEventArgs e)
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
