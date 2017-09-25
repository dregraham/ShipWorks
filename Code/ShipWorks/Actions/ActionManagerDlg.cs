using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Autofac;
using Divelements.SandGrid;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Scheduling;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Properties;
using ShipWorks.Stores;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Main ShipWorks window for managing actions
    /// </summary>
    public partial class ActionManagerDlg : Form
    {
        Font fontStrikeout;
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionManagerDlg(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.Size);
        }

        /// <summary>
        /// Load the current set of actions into the grid
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageActions);

            fontStrikeout = new Font(sandGrid.Font, FontStyle.Strikeout);

            editionGuiHelper.RegisterElement(newAction, EditionFeature.ActionLimit, () => sandGrid.Rows.Count);

            LoadActionGrid();
        }

        /// <summary>
        /// Update the state of the buttons based on the selection
        /// </summary>
        private void UpdateButtonState()
        {
            edit.Enabled = sandGrid.SelectedElements.Count == 1;
            rename.Enabled = sandGrid.SelectedElements.Count == 1;
            delete.Enabled = sandGrid.SelectedElements.Count == 1;

            editionGuiHelper.UpdateUI();
        }

        /// <summary>
        /// Load the actions into the grid
        /// </summary>
        private void LoadActionGrid()
        {
            Cursor.Current = Cursors.WaitCursor;

            sandGrid.Rows.Clear();

            foreach (ActionEntity action in ActionManager.Actions)
            {
                // Don't show internal actinos to the user
                if (action.InternalOwner != null)
                {
                    continue;
                }

                // Don't load actions that are just for stores that are still in the middle of the setup wizard or have been deleted
                if (action.StoreLimitedSingleStoreID != null)
                {
                    StoreEntity store = StoreManager.GetStore(action.StoreLimitedSingleStoreID.Value);
                    if (store == null || !store.SetupComplete)
                    {
                        continue;
                    }
                }

                GridRow gridRow = sandGrid.NewRow();

                // Tag and add it
                gridRow.Tag = action;
                gridRow.Checked = action.Enabled;
                sandGrid.Rows.Add(gridRow);

                // Apply the name
                gridRow.SetCellValue(gridColumnName, action.Name);

                // Apply the trigger
                gridRow.SetCellValue(gridColumnRunWhen, EnumHelper.GetDescription((ActionTriggerType) action.TriggerType));

                // Apply the tasks text
                gridRow.SetCellValue(gridColumnTasks, GetTaskDisplayNames(action.TaskSummary));

                // Update the display state
                UpdateEnabledDisplay(gridRow);
            }

            UpdateButtonState();
        }

        /// <summary>
        /// Get the list of display names from the list of identifiers
        /// </summary>
        private string GetTaskDisplayNames(string taskSummary)
        {
            StringBuilder sb = new StringBuilder();

            var matches = taskSummary.Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Regex.Match(x, @"([^:]+)([:](\d+))?"))
                .Where(x => x.Success);

            foreach (Match match in matches)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                try
                {
                    ActionTaskDescriptor descriptor = ActionTaskManager.GetDescriptor(match.Groups[1].Value);

                    // If we don't have type binding data (pre 3.5), then just use the base name
                    if (match.Groups.Count <= 2 || !match.Groups[2].Success)
                    {
                        sb.Append(descriptor.BaseName);
                    }
                    else
                    {
                        ActionTask instance = descriptor.CreateInstance(lifetimeScope);

                        if (instance is StoreTypeTaskBase)
                        {
                            sb.Append(new ActionTaskDescriptorBinding(descriptor, (StoreTypeCode) int.Parse(match.Groups[3].Value)).FullName);
                        }
                        else if (instance is StoreInstanceTaskBase)
                        {
                            sb.Append(new ActionTaskDescriptorBinding(descriptor, long.Parse(match.Groups[3].Value)).FullName);
                        }
                        else
                        {
                            sb.Append(descriptor.BaseName);
                        }
                    }
                }
                catch (NotFoundException)
                {
                    // If the descriptor isn't found it's because it was truncated in the database due to the length of TaskSummary getting too long
                    sb.Append("...");
                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reload the action manager, reload the list, and select the given action ID
        /// </summary>
        private void ReloadActions(long actionToSelect)
        {
            // Update the list in the manager
            ActionManager.CheckForChangesNeeded();

            // Reload our list
            LoadActionGrid();

            // Select the one we just created
            foreach (GridRow row in sandGrid.Rows)
            {
                if (((ActionEntity) row.Tag).ActionID == actionToSelect)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Update the GridRow UI display for the given action based on it's enabled state
        /// </summary>
        private void UpdateEnabledDisplay(GridRow gridRow)
        {
            ActionEntity action = (ActionEntity) gridRow.Tag;
            bool enabled = action.Enabled;

            Color color = enabled ? sandGrid.ForeColor : Color.DimGray;
            Font font = enabled ? sandGrid.Font : fontStrikeout;
            Image image = enabled ? Resources.gear_run16 : Resources.gear_stop_16;

            // Apply the font
            gridRow.Font = font;

            // Apply the color
            foreach (GridCell cell in gridRow.Cells)
            {
                cell.ForeColor = color;
            }

            // Apply the image
            gridRow.Cells[0].Image = image;
        }

        /// <summary>
        /// Create a new action
        /// </summary>
        private void OnNewAction(object sender, EventArgs e)
        {
            ActionEntity action = new ActionEntity();
            action.Name = "New Action";
            action.Enabled = true;

            action.ComputerLimitedType = (int) ComputerLimitedType.TriggeringComputer;
            action.InternalComputerLimitedList = string.Empty;

            action.StoreLimited = false;
            action.StoreLimitedList = new long[0];
            action.TriggerType = (int) ActionTriggerType.OrderDownloaded;
            action.TriggerSettings = "<Settings />";
            action.TaskSummary = "";

            using (ActionEditorDlg dlg = new ActionEditorDlg(lifetimeScope, action))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ReloadActions(action.ActionID);
                }
            }
        }

        /// <summary>
        /// Begin the rename operation on the selected store
        /// </summary>
        private void OnRename(object sender, EventArgs e)
        {
            ((GridRow) sandGrid.SelectedElements[0]).BeginEdit();
        }

        /// <summary>
        /// Rename operation is finalizing
        /// </summary>
        private void OnAfterRename(object sender, GridAfterEditEventArgs e)
        {
            ActionEntity action = (ActionEntity) e.Row.Tag;

            string proposed = e.Value.ToString().Trim();

            if (proposed == action.Name || proposed.Length == 0)
            {
                e.Cancel = true;
                return;
            }

            try
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    action.Name = proposed;
                    ActionManager.SaveAction(action, adapter);
                }
            }
            catch (ActionConcurrencyException ex)
            {
                e.Cancel = true;
                action.RollbackChanges();

                BeginInvoke((MethodInvoker) delegate
                    {
                        MessageHelper.ShowError(this, ex.Message);

                        ReloadActions(-1);
                    }
                );
            }
        }

        /// <summary>
        /// A checkbox in the grid changed
        /// </summary>
        private void OnCheckChanged(object sender, GridRowCheckEventArgs e)
        {
            ActionEntity action = (ActionEntity) e.Row.Tag;

            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    action.Enabled = e.Row.Checked;

                    // Some trigger's state depend on the enabledness of the action
                    ActionTrigger trigger = ActionManager.LoadTrigger(action);
                    trigger.SaveExtraState(action, adapter);

                    ActionManager.SaveAction(action, adapter);

                    adapter.Commit();
                }

                UpdateEnabledDisplay(e.Row);
            }
            catch (SchedulingException ex)
            {
                action.RollbackChanges();
                MessageHelper.ShowError(this, ex.Message);

                BeginInvoke((MethodInvoker) delegate { e.Row.Checked = !e.Row.Checked; });
            }
            catch (ActionConcurrencyException ex)
            {
                action.RollbackChanges();
                MessageHelper.ShowError(this, ex.Message);

                BeginInvoke((MethodInvoker) delegate { ReloadActions(-1); });
            }
        }

        /// <summary>
        /// The selected row in the grid has changed
        /// </summary>
        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonState();
        }

        /// <summary>
        /// Edit the selected action
        /// </summary>
        private void OnEdit(object sender, EventArgs e)
        {
            ActionEntity action = (ActionEntity) sandGrid.SelectedElements[0].Tag;

            EditAction(action);
        }

        /// <summary>
        /// A grid row has been activated (Double-clicked)
        /// </summary>
        private void OnGridRowActivated(object sender, GridRowEventArgs e)
        {
            EditAction((ActionEntity) e.Row.Tag);
        }

        /// <summary>
        /// Edit the given action
        /// </summary>
        private void EditAction(ActionEntity action)
        {
            using (ActionEditorDlg dlg = new ActionEditorDlg(lifetimeScope, action))
            {
                dlg.ShowDialog(this);

                ReloadActions(action.ActionID);
            }
        }

        /// <summary>
        /// Delete the selected action
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            ActionEntity action = (ActionEntity) sandGrid.SelectedElements[0].Tag;

            // By default we are just asking if they want to delete
            string question = string.Format("Delete the action '{0}'?", action.Name);

            if (MessageHelper.ShowQuestion(this, question) != DialogResult.OK)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            ActionManager.DeleteAction(action);

            ReloadActions(-1);
        }

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

                if (fontStrikeout != null)
                {
                    fontStrikeout.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
