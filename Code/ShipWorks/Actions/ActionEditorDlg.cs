﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Scheduling;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Data.Connection;
using ShipWorks.UI;
using ShipWorks.UI.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Triggers;
using ShipWorks.Actions.Triggers.Editors;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Templates;
using ShipWorks.Templates.Printing;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Window for editing the properties of an individual action
    /// </summary>
    public partial class ActionEditorDlg : Form
    {
        ActionEntity action;

        // A materialized representation of the action's trigger settings
        ActionTrigger trigger;

        // The trigger and tasks that were apart of the action when we first started editing it.  This is so we know what 
        // needs deleted when we go to save.
        ActionTrigger originalTrigger;
        List<ActionTask> originalTasks;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionEditorDlg(ActionEntity action)
        {
            InitializeComponent();

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.action = action;

            WindowStateSaver.Manage(this);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            UserSession.Security.DemandPermission(PermissionType.ManageActions);

            LoadStoresPanel();

            name.Text = action.Name;

            // Initialize the trigger combo
            EnumHelper.BindComboBox<ActionTriggerType>(triggerCombo);
            triggerCombo.SelectedValue = (ActionTriggerType) action.TriggerType;

            // Create the trigger settings instance and it's editor
            originalTrigger = ActionManager.LoadTrigger(action);
            trigger = originalTrigger;
            trigger.TriggeringEntityTypeChanged += new EventHandler(OnChangeTriggerEntityType);
            CreateTriggerEditor();

            // Start listening for changes to the trigger type
            triggerCombo.SelectedIndexChanged += new EventHandler(OnChangeTriggerType);

            // Create the menu for creating new tasks
            CreateAddTaskMenu();

            // Load the existing tasks for the action
            originalTasks = ActionManager.LoadTasks(action);
            foreach (ActionTask task in originalTasks)
            {
                AddTaskBubble(task);
            }

            // Hookup the "next" bubbles
            foreach (ActionTask task in originalTasks)
            {
                task.Entity.FlowSuccessBubble = ActiveBubbles.ElementAtOrDefault(-(task.Entity.FlowSuccess + 1));
                task.Entity.FlowSkippedBubble = ActiveBubbles.ElementAtOrDefault(-(task.Entity.FlowSkipped + 1));
                task.Entity.FlowErrorBubble = ActiveBubbles.ElementAtOrDefault(-(task.Entity.FlowError + 1));
            }

            UpdateTaskBubbles();

            // Load the basic settings
            enabled.Checked = action.Enabled;
            storeLimited.Checked = action.StoreLimited;
            panelStores.Enabled = action.StoreLimited;

            //Load the computer limited settings
            runOnOtherComputers.Checked = action.ComputerLimitedType != (int)ComputerLimitationType.TriggeringComputer;
            runOnOtherComputers.Enabled = trigger.TriggerType != ActionTriggerType.Scheduled;
            runOnSpecificComputers.Checked = action.ComputerLimitedType == (int)ComputerLimitationType.NamedList;
            runOnSpecificComputersList.SetSelectedComputers(action.ComputerLimitedList);

            // Check all the boxes for the stores its limited to
            foreach (long storeID in action.StoreLimitedList)
            {
                CheckBox storeBox = panelStores.Controls.OfType<CheckBox>().SingleOrDefault(c => (long)c.Tag == storeID);
                if (storeBox != null)
                {
                    storeBox.Checked = true;
                }
            }
        }

        /// <summary>
        /// Load the panel of store checkboxes
        /// </summary>
        private void LoadStoresPanel()
        {
            panelStores.Controls.Clear();

            Point location = new Point(3, 3);

            // Go through all the stores
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                CheckBox checkBox = new CheckBox();
                checkBox.AutoSize = true;
                checkBox.Location = location;
                checkBox.Text = store.StoreName;
                checkBox.Location = location;
                checkBox.Parent = panelStores;
                checkBox.Tag = store.StoreID;

                location.Y = checkBox.Bottom + 4;
            }

            panelStores.Height = location.Y + 4;
        }

        /// <summary>
        /// Create the context menu used for adding tasks
        /// </summary>
        private void CreateAddTaskMenu()
        {
            addTask.ContextMenuStrip = ActionTaskManager.CreateTasksMenu();

            HookAddTaskEvents(addTask.ContextMenuStrip.Items);
        }

        /// <summary>
        /// Hookup event listeners for each MenuItem in the menu
        /// </summary>
        private void HookAddTaskEvents(ToolStripItemCollection items)
        {
            foreach (ToolStripMenuItem menuItem in items.OfType<ToolStripMenuItem>())
            {
                if (menuItem.DropDownItems.Count == 0)
                {
                    menuItem.Click += new EventHandler(OnAddTask);
                }
                else
                {
                    HookAddTaskEvents(menuItem.DropDownItems);
                }
            }
        }

        /// <summary>
        /// The trigger type is changing
        /// </summary>
        private void OnChangeTriggerType(object sender, EventArgs e)
        {
            ActionTriggerType triggerType = (ActionTriggerType) triggerCombo.SelectedValue;

            // Create a new trigger of the selected type with the trigger's default settings
            trigger = ActionTriggerFactory.CreateTrigger(triggerType, null);
            trigger.TriggeringEntityTypeChanged += new EventHandler(OnChangeTriggerEntityType);

            CreateTriggerEditor();

            //Scheduled actions must be allowed to run on other computers
            if(triggerType == ActionTriggerType.Scheduled)
            {
                runOnOtherComputers.Checked = true;
                runOnOtherComputers.Enabled = false;
            }
            else
            {
                runOnOtherComputers.Checked = action.ComputerLimitedType != (int)ComputerLimitationType.TriggeringComputer;
                runOnOtherComputers.Enabled = true;
            }
        }

        /// <summary>
        /// The type of entity that causes the current trigger has changed
        /// </summary>
        void OnChangeTriggerEntityType(object sender, EventArgs e)
        {
            UpdateTaskBubbles();
        }

        /// <summary>
        /// Create the editor control for the current trigger instance
        /// </summary>
        private void CreateTriggerEditor()
        {
            ActionTriggerEditor editor = trigger.CreateEditor();

            editor.SizeChanged += OnActionTriggerEditorSizeChanged;

            panelTrigger.Height = editor.Height;

            editor.Dock = DockStyle.Fill;
            panelTrigger.Controls.Add(editor);

            if (panelTrigger.Controls.Count > 1)
            {
                ActionTriggerEditor oldEditor = (ActionTriggerEditor) panelTrigger.Controls[0];

                panelTrigger.Controls.Remove(oldEditor);
                oldEditor.Dispose();
            }

            UpdateTaskBubbles();
        }

        /// <summary>
        /// Updates the panel height when the action trigger editor changes it's height.
        /// </summary>
        void OnActionTriggerEditorSizeChanged(object sender, EventArgs e)
        {
            panelTrigger.Height = ((ActionTriggerEditor) sender).Height;
            UpdateTaskArea();
        }

        /// <summary>
        /// Layout and upate all the task bubbles to reflect their appropriate indexes
        /// </summary>
        private void UpdateTaskBubbles()
        {
            int y = panelTasks.AutoScrollPosition.Y;

            for (int i = 0; i < panelTasks.Controls.Count; i++)
            {
                ActionTaskBubble bubble = (ActionTaskBubble) panelTasks.Controls[i];

                bubble.Location = new Point(0, y);
                bubble.UpdateBubble(trigger);

                y = bubble.Bottom + 4;
            }

            UpdateTaskArea();
        }

        /// <summary>
        /// Update the UI positioning of the tasks area
        /// </summary>
        private void UpdateTaskArea()
        {
            labelTasksHeader.Top = panelTrigger.Bottom + 4;

            int panelTasksOldBottom = panelTasks.Bottom;
            panelTasks.Top = labelTasksHeader.Bottom + 4;
            panelTasks.Height = panelTasksOldBottom - panelTasks.Top;

            int tasksHeight = panelTasks.Controls.Count > 0 ? panelTasks.Controls.Cast<Control>().Max(c => c.Bottom) + 5 : 0;

            // See if the ideal height would push the Add button too far down
            if (panelTasks.Top + tasksHeight + 4 + addTask.Height + 10 > optionPageAction.Height)
            {
                tasksHeight = optionPageAction.Height - 4 - addTask.Height - 10 - panelTasks.Top + 2;
            }

            addTask.Top = panelTasks.Top + tasksHeight + 4;
        }

        /// <summary>
        /// The window is being resized
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            UpdateTaskArea();
        }

        /// <summary>
        /// Change whether limiting the action by store
        /// </summary>
        private void OnChangeLimitStores(object sender, EventArgs e)
        {
            panelStores.Enabled = storeLimited.Checked;
        }

        /// <summary>
        /// Generate the list of limited stores based on the UI
        /// </summary>
        private long[] GenerateStoreLimitedListFromUI()
        {
            List<long> list = new List<long>();

            foreach (CheckBox storeBox in panelStores.Controls.OfType<CheckBox>().Where(c => c.Checked))
            {
                list.Add((long)storeBox.Tag);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Add a new task to the action
        /// </summary>
        void OnAddTask(object sender, EventArgs e)
        {
            ActionTaskDescriptorBinding binding = (ActionTaskDescriptorBinding) ((ToolStripMenuItem) sender).Tag;

            AddTaskBubble(binding.CreateInstance());
        }

        /// <summary>
        /// Add a task bubble for editing the properties of the given task entity
        /// </summary>
        private void AddTaskBubble(ActionTask task)
        {
            // Create the bubble that will hold it
            ActionTaskBubble bubble = new ActionTaskBubble(task, ActiveBubbles);
            bubble.Width = panelTasks.DisplayRectangle.Width;
            bubble.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

            // Add the bubble to our task area
            panelTasks.Controls.Add(bubble);

            // Update the task bubble display
            UpdateTaskBubbles();

            // We need to know when the bubble changes size so we can adjust
            bubble.BubbleChanged += new EventHandler(OnBubbleChanged);
            bubble.MoveUp += new EventHandler(OnBubbleMoveUp);
            bubble.MoveDown += new EventHandler(OnBubbleMoveDown);
            bubble.Delete += new EventHandler(OnBubbleDelete);
        }

        /// <summary>
        /// The bubble has changed in some way
        /// </summary>
        void OnBubbleChanged(object sender, EventArgs e)
        {
            UpdateTaskBubbles();
        }

        /// <summary>
        /// A bubble is requesting to be moved up
        /// </summary>
        void OnBubbleMoveUp(object sender, EventArgs e)
        {
            ActionTaskBubble bubble = (ActionTaskBubble) sender;

            panelTasks.Controls.SetChildIndex(bubble, panelTasks.Controls.IndexOf(bubble) - 1);

            UpdateTaskBubbles();
        }

        /// <summary>
        /// A bubble is requesting to be moved down
        /// </summary>
        void OnBubbleMoveDown(object sender, EventArgs e)
        {
            ActionTaskBubble bubble = (ActionTaskBubble) sender;

            panelTasks.Controls.SetChildIndex(bubble, panelTasks.Controls.IndexOf(bubble) + 1);

            UpdateTaskBubbles();
        }

        /// <summary>
        /// A bubble is requesting to be deleted
        /// </summary>
        void OnBubbleDelete(object sender, EventArgs e)
        {
            ActionTaskBubble bubble = (ActionTaskBubble) sender;

            panelTasks.Controls.Remove(bubble);

            UpdateTaskBubbles();
        }

        /// <summary>
        /// The list of tasks that are currently being edited
        /// </summary>
        private IEnumerable<ActionTaskBubble> ActiveBubbles
        {
            get { return panelTasks.Controls.Cast<ActionTaskBubble>(); }
        }

        /// <summary>
        /// Write the correct flow option value to the task based on it's flow field setting.
        /// </summary>
        private void UpdateTaskFlowOption(ActionTask task, EntityField2 flowField, object bubble, List<ActionTask> allTasks)
        {
            int currentValue = (int) task.Entity.GetCurrentFieldValue(flowField.FieldIndex);

            if (!Enum.IsDefined(typeof(ActionTaskFlowOption), (ActionTaskFlowOption) currentValue) && bubble != null)
            {
                int referencedIndex = allTasks.IndexOf(((ActionTaskBubble) bubble).ActionTask);
                if (referencedIndex >= 0)
                {
                    task.Entity.SetNewFieldValue(flowField.FieldIndex, -(referencedIndex + 1));
                }
                else
                {
                    task.Entity.SetNewFieldValue(flowField.FieldIndex, (int) ActionTaskFlowOption.NextStep);
                }
            }
        }

        /// <summary>
        /// User is accepting changes to the action
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (name.Text.Trim().Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter a name for the action.");
                ActiveControl = name;
                return;
            }

            try
            {
                trigger.Validate();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                ActiveControl = panelTrigger;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                List<ActionTask> tasksToSave = ActiveBubbles.Select(b => b.ActionTask).ToList();
                List<ActionTask> tasksToDelete = originalTasks.Except(tasksToSave).ToList();

                // Update the flow order
                foreach (ActionTask task in tasksToSave)
                {
                    UpdateTaskFlowOption(task, ActionTaskFields.FlowSuccess, task.Entity.FlowSuccessBubble, tasksToSave);
                    UpdateTaskFlowOption(task, ActionTaskFields.FlowSkipped, task.Entity.FlowSkippedBubble, tasksToSave);
                    UpdateTaskFlowOption(task, ActionTaskFields.FlowError, task.Entity.FlowErrorBubble, tasksToSave);

                    task.Entity.StepIndex = tasksToSave.IndexOf(task);
                }

                if (!ValidateTaskFlow(tasksToSave))
                {
                    MessageHelper.ShowError(this, "The flow configured for the tasks could result in infinite looping.");
                    return;
                }

                // We have to ensure that for each task that uses a template to print, the template is configured for printing.  We only have a single PrintTask that we have to check this for.
                // If we ever add more tasks that print, we may consider making this more abstrat and having task have a method to return like "TemplatesToPrintWith";
                if (!TemplatePrinterSelectionDlg.EnsureConfigured(this, tasksToSave.OfType<IPrintWithTemplates>()))
                {
                    return;
                }

                ActionTriggerType actionTriggerType = (ActionTriggerType)triggerCombo.SelectedValue;
                
                // Check to see if there are any tasks that aren't allowed to be used in a scheduled action.
                List<ActionTask> invalidTasks = tasksToSave.Where(at => !ActionTaskManager.GetDescriptor(at.GetType()).AllowedForScheduledTask).ToList();
                if (actionTriggerType == ActionTriggerType.Scheduled && invalidTasks.Any())
                {
                    string invalidTasksMsg = string.Join<string>(", ", invalidTasks.Select<ActionTask, string>(t => ActionTaskManager.GetDescriptor(t.GetType()).BaseName));

                    MessageHelper.ShowError(this, string.Format("The task{0} '{1}' {2} not allowed for use with '{3}'", 
                        invalidTasks.Count == 1 ? "" : "s", 
                        invalidTasksMsg,
                        invalidTasks.Count == 1 ? "is" : "are", 
                        EnumHelper.GetDescription(actionTriggerType)));
                    return;
                }

                if (actionTriggerType != originalTrigger.TriggerType && originalTrigger.TriggerType == ActionTriggerType.Scheduled)
                {
                    try
                    {
                        // User changed the trigger from a scheduled trigger to another type of trigger, so we need to make sure
                        // the action is remvoed from the schedule
                        new Scheduler().UnscheduleAction(action);
                    }
                    catch (SchedulingException schedulingException)
                    {
                        MessageHelper.ShowError(this, string.Format("An error occurred trying to remove a scheduled action. {0}", schedulingException.Message));
                        return;
                    }
                }

                // Transacted since we affect multiple action tables
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // If the action is new, we have to save it once up front to get its PK.  Require's two trip's to save,
                    // but some of the SaveExtraState functions can require the ID... but then that can affect there XML Settings serialization,
                    // which requires the final save to the DB
                    if (action.IsNew)
                    {
                        adapter.SaveAndRefetch(action);
                    }

                    action.Name = name.Text.Trim();

                    // Save the basic options of the action
                    action.TriggerType = (int) triggerCombo.SelectedValue;
                    action.TaskSummary = ActionManager.GetTaskSummary(tasksToSave);
                    action.Enabled = enabled.Checked;
                    action.StoreLimited = storeLimited.Checked;
                    action.StoreLimitedList = GenerateStoreLimitedListFromUI();

                    //Save the computer limited settings
                    if (!runOnOtherComputers.Checked)
                    {
                        action.ComputerLimitedType = (int)ComputerLimitationType.TriggeringComputer;
                        action.ComputerLimitedList = new long[0];
                    }
                    else if (runOnAnyComputer.Checked)
                    {
                        action.ComputerLimitedType = (int)ComputerLimitationType.None;
                        action.ComputerLimitedList = new long[0];
                    }
                    else
                    {
                        action.ComputerLimitedType = (int)ComputerLimitationType.NamedList;
                        action.ComputerLimitedList = runOnSpecificComputersList.GetSelectedComputers().Select(x => x.ComputerID).ToArray();

                        if (!action.ComputerLimitedList.Any())
                        {
                            MessageHelper.ShowError(this, "At least one computer must be selected when choosing to run actions on specific computers.");
                            return;
                        }
                    }

                    // If we changed triggers, we need to notify the original that it is being deleted
                    if (trigger != originalTrigger)
                    {
                        originalTrigger.DeleteExtraState(action, adapter);
                    }

                    // Give the new trigger a chance to save its state
                    trigger.SaveExtraState(action, adapter);
                    action.TriggerSettings = trigger.GetXml();

                    // Save all the tasks
                    foreach (ActionTask task in tasksToSave)
                    {
                        task.Save(action, adapter);
                    }

                    // Delete all tasks that need deleting
                    foreach (ActionTask task in tasksToDelete)
                    {
                        task.Delete(adapter);
                    }

                    // We force at least the Enabled property to be saved everytime.  This is so if there were any edits made to the tasks,
                    // optimistic concurrecy can catch it just by checking at the Action level.  This does cause optimistic concurrency to give false
                    // positives in cases where no edits were made, but I think given the likelihood of it happening this is an OK way to handle it.  The
                    // alternative would be to handle it in detail at both the Action and AtionTask level.
                    action.Fields[(int) ActionFieldIndex.Enabled].IsChanged = true;
                    action.IsDirty = true;

                    // Save the action
                    ActionManager.SaveAction(action, adapter);

                    adapter.Commit();
                }

                DialogResult = DialogResult.OK;
            }
            catch (ActionConcurrencyException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                DialogResult = DialogResult.Abort;
            }
            catch (ActionSaveException ex)
            {
                MessageHelper.ShowMessage(this, ex.Message + "\n\nThe action has not been saved, and the editor will now close.");

                // At this point I'm taking the easy approach and just closing out of the window.  This is because it would be pretty
                // hard to make sure all the tasks roll back properly, due to the Save\Delete extra state stuff.  I also think that 
                // having this type of error while saving will be rare.  Its possible to make this able to stay open in the future,
                // if there was ever a compelling need.
                DialogResult = DialogResult.Abort;
            }
            catch (SqlForeignKeyException)
            {
                MessageHelper.ShowError(this, "The action has been deleted by another user.");
                DialogResult = DialogResult.Abort;
            }
            catch (SchedulingException schedulingException)
            {
                MessageHelper.ShowError(this, schedulingException.Message);
            }
        }

        /// <summary>
        /// Validate that the flow of tasks does not result in any infinite loops
        /// </summary>
        private bool ValidateTaskFlow(List<ActionTask> tasks)
        {
            // If there arent any tasks, there can't be a flow issue
            if (tasks.Count == 0)
            {
                return true;
            }

            // Get the entities in order
            List<ActionTaskEntity> steps = tasks.Select(t => t.Entity).OrderBy(e => e.StepIndex).ToList();

            // The list of steps that have been visited so far
            List<int> stepsSeen = new List<int> { 0 };

            return ValidateTaskFlow(steps[0], steps, stepsSeen);
        }

        /// <summary>
        /// Validate that the flow from the given step does not result in revisiting a step that has already been seen.
        /// </summary>
        private bool ValidateTaskFlow(ActionTaskEntity step, List<ActionTaskEntity> steps, List<int> stepsSeen)
        {
            int nextIndexSuccess = GetNextStepIndex(steps.IndexOf(step), steps, step.FlowSuccess);
            int nextIndexSkipped = step.FilterCondition ? GetNextStepIndex(steps.IndexOf(step), steps, step.FlowSkipped) : -1;
            int nextIndexError = GetNextStepIndex(steps.IndexOf(step), steps, step.FlowError);

            List<int> potentialIndexes = new List<int> { nextIndexSuccess, nextIndexSkipped, nextIndexError }.Where(i => i != -1).Distinct().ToList();

            // Now check each potential next step for its next steps
            foreach (int index in potentialIndexes)
            {
                if (stepsSeen.Contains(index))
                {
                    return false;
                }

                // Make a new copy each time we recurse down so we don't invalidate the list with old ones in it
                List<int> cloneSeen = new List<int>();
                cloneSeen.AddRange(stepsSeen);
                cloneSeen.Add(index);

                if (!ValidateTaskFlow(steps[index], steps, cloneSeen))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Get what would be the next step index
        /// </summary>
        private int GetNextStepIndex(int currentIndex, List<ActionTaskEntity> steps, int flowOption)
        {
            if (flowOption == (int) ActionTaskFlowOption.Quit || flowOption == (int) ActionTaskFlowOption.Suspend)
            {
                return -1;
            }

            if (flowOption == (int) ActionTaskFlowOption.NextStep)
            {
                if (currentIndex == steps.Count - 1)
                {
                    return -1;
                }
                else
                {
                    return currentIndex + 1;
                }
            }

            // The flow option is the negative of the next step to execute
            return -(flowOption + 1);
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            // If not closed successfully, rollback changes
            if (DialogResult != DialogResult.OK)
            {
                action.RollbackChanges();
            }
        }


        void OnRunOnOtherComputersChecked(object sender, EventArgs e)
        {
            computersPanel.Enabled = runOnOtherComputers.Checked;
        }

        void OnRunOnSpecificComputersChecked(object sender, EventArgs e)
        {
            runOnSpecificComputersList.Enabled = runOnSpecificComputers.Checked;

            if (runOnSpecificComputersList.Visible && runOnSpecificComputersList.Enabled && !runOnSpecificComputersList.GetSelectedComputers().Any())
                runOnSpecificComputersList.ShowPopup();
        }
    }
}
