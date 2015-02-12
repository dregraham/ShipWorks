using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore.MessageBoxes;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Grid;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Filters.Management
{
    /// <summary>
    /// Window for editing a filter
    /// </summary>
    public partial class FilterEditorDlg : Form
    {
        FilterNodeEntity filterNode;
        FilterEntity filter;

        IEntityFields2 originalFields;

        bool gridChanges = false;

        /// <summary>
        /// Raised when the user hits OK and the filter needs to be saved
        /// </summary>
        public event CancelEventHandler Saving;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterEditorDlg(FilterNodeEntity filterNode)
        {
            if (filterNode == null)
            {
                throw new ArgumentNullException("filterNode");
            }

            InitializeComponent();
            WindowStateSaver.Manage(this);

            this.filterNode = filterNode;
            this.filter = filterNode.Filter;
            
            // We will use these to restore the filter if the user cancels
            this.originalFields = filter.Fields.Clone();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            filterName.Text = filter.Name;

            appliesTo.Text = EnumHelper.GetDescription((FilterTarget) filter.FilterTarget);
            appliesToImage.Image = FilterHelper.GetFilterImage((FilterTarget) filter.FilterTarget);
            enabled.Checked = filterNode.Filter.State == (int)FilterState.Enabled;
            
            // Verify its OK for this user to edit this filter
            if (!FilterHelper.IsMyFilter(filterNode))
            {
                UserSession.Security.DemandPermission(PermissionType.ManageFilters);
            }

            // Load condition editor
            conditionControl.LoadFilter(filter);

            // Folders
            if (filter.IsFolder)
            {
                this.Text = this.Text.Replace("Filter", "Folder");
                labelFilterName.Text = labelFilterName.Text.Replace("Filter", "Folder");

                // One of our builtin folders
                if (FilterHelper.IsBuiltin(filter))
                {
                    filterName.Enabled = false;
                    tabControl.TabPages.Remove(tabPageCondition);
                }

                labelEnabled.Enabled = false;
                enabled.Enabled = false;
            }

            // Quick Filter's never have content displayed in a grid
            if (filterNode.Purpose != (int) FilterNodePurpose.Quick)
            {
                // Load the column editor
                gridColumnsPersonal.LoadSettings(FilterNodeColumnManager.GetUserSettings(filterNode));
                gridColumnsDefault.LoadSettings(FilterNodeColumnManager.GetDefaultSettings(filterNode));

                columnSettingList.SelectedIndex = 0;
            }
            else
            {
                tabControl.TabPages.Remove(tabPageGridColumns);
            }
        }

        /// <summary>
        /// The filter that is being edited
        /// </summary>
        public FilterEntity Filter
        {
            get
            {
                return filter;
            }
        }

        /// <summary>
        /// Change the type of grid columns being edited
        /// </summary>
        private void OnChangeGridColumnType(object sender, EventArgs e)
        {
            panelPersonalColumns.Visible = columnSettingList.SelectedIndex == 0;
            panelDefaultColumns.Visible = columnSettingList.SelectedIndex != 0;
        }

        /// <summary>
        /// Overwrite the default settings with the current My Settings
        /// </summary>
        private void OnCopyColumnsFromMySettings(object sender, EventArgs e)
        {
            if (MessageHelper.ShowQuestion(this, 
                "This will overwrite the default column settings with your\n" +
                "personal column settings.\n\n" +
                "Continue and overwrite?") == DialogResult.OK)
            {
                FilterNodeColumnSettings personalSettings = gridColumnsPersonal.Settings;
                FilterNodeColumnSettings defaultSettings = gridColumnsDefault.Settings;

                defaultSettings.CopyFrom(personalSettings);
                gridColumnsDefault.LoadSettings(defaultSettings);

                gridChanges = true;
            }
        }

        /// <summary>
        /// Reset My Settings to whatever the default settings are
        /// </summary>
        private void OnResetMySettingsToDefault(object sender, EventArgs e)
        {
            if (MessageHelper.ShowQuestion(this,
                "This will overwrite your personal column settings with the\n" +
                "default column settings.\n\n" +
                "Continue and overwrite?") == DialogResult.OK)
            {
                FilterNodeColumnSettings personalSettings = gridColumnsPersonal.Settings;
                FilterNodeColumnSettings defaultSettings = gridColumnsDefault.Settings;

                personalSettings.CopyFrom(defaultSettings);
                gridColumnsPersonal.LoadSettings(personalSettings);

                gridChanges = true;
            }
        }

        /// <summary>
        /// Save the filter
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (filterName.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "You must specify a name.");
                return;
            }

            // Only show the confirmation if the state is disabled and there are references to the filter
            if (IsFilterBeingDisabled && new FilterNodeReferenceRepository().Find(filterNode).Any())
            {
                using (DisableLinkedFilterDlg dlg = new DisableLinkedFilterDlg(filterNode))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }
            }

            filter.Name = filterName.Text.Trim();
            filterNode.Filter.State = enabled.Checked ? (byte)FilterState.Enabled : (byte)FilterState.Disabled;
            
            // Save the condition
            if (!conditionControl.SaveDefinitionToFilter())
            {
                MessageHelper.ShowInformation(this, "Some of the values entered in the condition are not valid.");

                return;
            }

            if (Saving != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                Saving(this, args);

                // Cancel the closing of the window
                if (args.Cancel)
                {
                    return;
                }
            }

            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Returns whether the current filter is being disabled
        /// </summary>
        private bool IsFilterBeingDisabled
        {
            get
            {
                return filterNode.Filter.State == (int)FilterState.Enabled && !enabled.Checked;
            }
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // If its not closing because of an OK, see if they'd be losing unsaved changes
            if (DialogResult != DialogResult.OK && !filter.IsNew)
            {
                bool isDirty = false;

                // First we need to save the fields to the filter so we can know if its dirty
                filter.Name = filterName.Text.Trim();

                // If the condition is now invalid, we know its dirty, b\c it had to have been valid upon entry
                if (!conditionControl.SaveDefinitionToFilter())
                {
                    isDirty = true;
                }

                // If the filter content itself is dirty,
                if (filter.IsDirty)
                {
                    isDirty = true;
                }

                if (isDirty || gridChanges)
                {
                    using (UnsavedChangesDlg dlg = new UnsavedChangesDlg())
                    {
                        dlg.Message = "The filter has unsaved changes.";

                        DialogResult result = dlg.ShowDialog(this);

                        if (result == DialogResult.Cancel)
                        {
                            e.Cancel = true;
                            return;
                        }

                        if (result == DialogResult.Yes)
                        {
                            e.Cancel = true;
                            BeginInvoke(new Action<object, EventArgs>(OnOK), null, EventArgs.Empty);
                            return;
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// The form is being closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            // If its not closing because of an OK, rollback the fields
            if (DialogResult != DialogResult.OK)
            {
                filter.Fields = originalFields;

                // Quick filter's don't have columns
                if (filterNode.Purpose != (int) FilterNodePurpose.Quick)
                {
                    // Rollback changes to the layouts
                    gridColumnsDefault.Settings.CancelChanges();
                    gridColumnsPersonal.Settings.CancelChanges();
                }
            }
        }
    }
}