using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Data.Connection;
using ShipWorks.UI.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Triggers;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using System.Diagnostics;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// The control that contains a single action task editor (and kind of looks like a bubble)
    /// </summary>
    [ToolboxItem(false)]
    public partial class ActionTaskBubble : UserControl
    {
        ActionTask task;
        ActionTrigger trigger;

        IEnumerable<ActionTaskBubble> allBubbles;

        public event ActionTaskChangingEventHandler ActionTaskChanging;
        public event EventHandler BubbleChanged;
        public event EventHandler MoveUp;
        public event EventHandler MoveDown;
        public event EventHandler Delete;

        // This is so we can return settings when switching between task types
        Dictionary<ActionTaskDescriptorBinding, ActionTask> taskHistoryMap = new Dictionary<ActionTaskDescriptorBinding, ActionTask>();

        // The menus for choosing whatits inputs will be
        ContextMenuStrip inputSourceMenu;
        private bool usesDisabledFilter = false;
        private readonly long initialInputFilterNodeId;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskBubble(ActionTask task, IEnumerable<ActionTaskBubble> allBubbles)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
            taskHistoryMap[ActionTaskManager.GetBinding(task)] = task;

            this.allBubbles = allBubbles;

            toolStrip.Renderer = new NoBorderToolStripRenderer();

            // Load the ComboBox with the available task options
            taskTypes.DropDownSandPopupMenu = ActionTaskManager.CreateTasksMenu();
            taskTypes.SelectedMenuObject = ActionTaskManager.GetBinding(task);
            taskTypes.DisplayValueProvider = (object tag) => { return ((ActionTaskDescriptorBinding) tag).FullName; };

            // Create the editor for the task
            CreateTaskEditor();

            // Start listening for changes
            taskTypes.SelectedMenuObjectChanged += new EventHandler(OnChangeTaskType);

            // The filter selection for input can be anything, and get's loaded one time
            inputSourceFilter.LoadLayouts(FilterTarget.Orders, FilterTarget.Customers, FilterTarget.Shipments, FilterTarget.Items);
            inputSourceFilter.SelectedFilterNodeID = task.Entity.InputFilterNodeID;
            
            // As it changes, update the task
            inputSourceFilter.SelectedFilterNodeChanged += new EventHandler(OnInputSourceFilterChanged);

            initialInputFilterNodeId = task.Entity.InputFilterNodeID;
        }

        /// <summary>
        /// The ActionTask row the bubble is displaying information for
        /// </summary>
        public ActionTask ActionTask
        {
            get { return task; }
        }

        /// <summary>
        /// Specifies whether this bubble is using a disabled filter
        /// </summary>
        public bool UsesDisabledFilter
        {
            get
            {
                return ActionTask.Entity.InputSource == (int) ActionTaskInputSource.FilterContents && usesDisabledFilter;
            }
        }

        /// <summary>
        /// Create the editor for editing the task
        /// </summary>
        private void CreateTaskEditor()
        {
            ActionTaskEditor editor = task.CreateEditor();
            
            // We have to ensure the contorl is created, to make sure its OnLoad is called, which is where it may change
            // its height.
            IntPtr handle = editor.Handle;

            panelTaskSettings.Controls.Add(editor);
            panelTaskSettings.Height = editor.Height;

            editor.Width = panelTaskSettings.Width;
            editor.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            editor.SizeChanged += new EventHandler(OnEditorSizeChanged);

            if (panelTaskSettings.Controls.Count > 1)
            {
                ActionTaskEditor oldEditor = (ActionTaskEditor) panelTaskSettings.Controls[0];
                oldEditor.SizeChanged -= this.OnEditorSizeChanged;

                panelTaskSettings.Controls.Remove(oldEditor);
                oldEditor.Dispose();
            }
        }

        /// <summary>
        /// The size of the editor area in a bubble has changed
        /// </summary>
        void OnEditorSizeChanged(object sender, EventArgs e)
        {
            Control editor = (Control) sender;
            Debug.Assert(editor.Parent == panelTaskSettings);

            if (panelTaskSettings.Height != editor.Height)
            {
                panelTaskSettings.Height = editor.Height;

                RaiseBubbleChanged();                
            }
        }

        /// <summary>
        /// Raise the bubble changed event, which lets the parent know our size may have changed
        /// </summary>
        private void RaiseBubbleChanged()
        {
            // This lets the parent editor to know to relayout the stuff below us
            if (BubbleChanged != null)
            {
                BubbleChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raise the action task changing event args, which lets the parent know that the selected task has changed
        /// </summary>
        /// <param name="previousTask">The task that was selcted before it was changed</param>
        /// <param name="newTask">The task that is currently selected</param>
        /// <returns></returns>
        private bool RaiseActionTaskChanging(ActionTask previousTask, ActionTask newTask)
        {
            if (ActionTaskChanging != null)
            {
                ActionTaskChangingEventArgs args = new ActionTaskChangingEventArgs(previousTask, newTask);
                ActionTaskChanging(this, args);
                return !args.Cancel;
            }

            return true;
        }

        /// <summary>
        /// Update the layout and positioning of everything based settings and trigger
        /// </summary>
        private void UpdateControlLayout()
        {
            panelInputSource.Visible = task.InputRequirement != ActionTaskInputRequirement.None;
            panelTaskSettings.Top = panelInputSource.Visible ? panelInputSource.Bottom : panelInputSource.Top;

            flowInfoControl.UpdateInfoDisplay(task, trigger, allBubbles);
            flowInfoControl.Top = panelTaskSettings.Bottom;

            kryptonGroup.Height = flowInfoControl.Bottom + 6;
            Height = kryptonGroup.Bottom + 2;
        }

        /// <summary>
        /// Change the type of task selected
        /// </summary>
        void OnChangeTaskType(object sender, EventArgs e)
        {
            ActionTask oldTask = task;

            // The binding of the new task 
            ActionTaskDescriptorBinding binding = (ActionTaskDescriptorBinding) taskTypes.SelectedMenuObject;

            // See if we have cached settings for it, if not create them
            if (!taskHistoryMap.TryGetValue(binding, out task))
            {
                task = binding.CreateInstance();
                taskHistoryMap[binding] = task;
            }

            // Raise the task changing event
            if (!RaiseActionTaskChanging(oldTask, task))
            {
                // Reset the selected task if any event handlers canceled the change
                taskTypes.SelectedMenuObject = ActionTaskManager.GetBinding(oldTask);
                return;
            }

            // Copy over the flow settings of the old one, so it looks seemless
            CopyFlowSettings(oldTask.Entity, task.Entity);

            CreateTaskEditor();
            RaiseBubbleChanged();
        }

        /// <summary>
        /// Copy the flow settings from the source to the target task
        /// </summary>
        private void CopyFlowSettings(ActionTaskEntity source, ActionTaskEntity target)
        {
            target.FilterCondition = source.FilterCondition;
            target.FilterConditionNodeID = source.FilterConditionNodeID;

            target.FlowSuccess = source.FlowSuccess;
            target.FlowSuccessBubble = source.FlowSuccessBubble;

            target.FlowSkipped = source.FlowSkipped;
            target.FlowSkippedBubble = source.FlowSkippedBubble;

            target.FlowError = source.FlowError;
            target.FlowErrorBubble = source.FlowErrorBubble;
        }

        /// <summary>
        /// Update the bubble based on its position relative to the other task bubbles
        /// </summary>
        public void UpdateBubble(ActionTrigger trigger)
        {
            this.trigger = trigger;

            int index = Parent.Controls.IndexOf(this);
            int total = Parent.Controls.Count;

            taskIndex.Text = string.Format("{0}.", index + 1);

            moveUp.Enabled = index > 0;
            moveDown.Enabled = index < total - 1;

            UpdateInputSourceOptions(trigger);

            ActionTaskEditor taskEditor = (ActionTaskEditor) panelTaskSettings.Controls[0];
            taskEditor.NotifyTaskInputChanged(trigger, (ActionTaskInputSource) task.Entity.InputSource, GetEffectiveInputEntityType());

            // If the task has a filter condition, we have to make sure its still valid
            if (task.Entity.FilterCondition)
            {
                // If the task had a filter condition - but now there is no triggering entity type, we have to remove the condition
                if (trigger.TriggeringEntityType == null)
                {
                    task.Entity.FilterCondition = false;
                    task.Entity.FilterConditionNodeID = 0;
                }
                else
                {
                    FilterNodeEntity conditionNode = FilterLayoutContext.Current.FindNode(task.Entity.FilterConditionNodeID);
                    if (conditionNode != null && FilterHelper.GetEntityType((FilterTarget) conditionNode.Filter.FilterTarget) != trigger.TriggeringEntityType)
                    {
                        task.Entity.FilterConditionNodeID = 0;
                    }
                }
            }

            UpdateControlLayout();
        }

        /// <summary>
        /// Get the effective input entity type based on the configured trigger and input selection options
        /// </summary>
        private EntityType? GetEffectiveInputEntityType()
        {
            if (task.Entity.InputSource == (int)ActionTaskInputSource.Nothing)
            {
                return null;
            }

            if (task.Entity.InputSource == (int)ActionTaskInputSource.TriggeringRecord)
            {
                return trigger.TriggeringEntityType;
            }

            if (task.Entity.InputSource == (int) ActionTaskInputSource.Selection)
            {
                return trigger.SelectionEntityType;
            }

            FilterNodeEntity filterNode = FilterLayoutContext.Current.FindNode(task.Entity.InputFilterNodeID);
            if (filterNode == null)
            {
                return null;
            }

            return FilterHelper.GetEntityType((FilterTarget)filterNode.Filter.FilterTarget);
        }

        /// <summary>
        /// Move the bubble up
        /// </summary>
        private void OnMoveUp(object sender, EventArgs e)
        {
            if (MoveUp != null)
            {
                MoveUp(this, EventArgs.Empty);
            }

            // Not sure how i feel about how this felt...
            Cursor.Position = moveUp.Owner.PointToScreen(new Point(moveUp.Bounds.Left + moveUp.Bounds.Width / 2, moveUp.Bounds.Top + moveUp.Bounds.Height / 2));
        }

        /// <summary>
        /// Move the bubble down
        /// </summary>
        private void OnMoveDown(object sender, EventArgs e)
        {
            if (MoveDown != null)
            {
                MoveDown(this, EventArgs.Empty);
            }

            // Not sure how i feel about how this felt...
            Cursor.Position = moveDown.Owner.PointToScreen(new Point(moveDown.Bounds.Left + moveDown.Bounds.Width / 2, moveDown.Bounds.Top + moveDown.Bounds.Height / 2));
        }

        /// <summary>
        /// Delete the bubble
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (Delete != null)
            {
                Delete(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Update the menu options for the data source
        /// </summary>
        private void UpdateInputSourceOptions(ActionTrigger trigger)
        {
            inputSourceMenu = new ContextMenuStrip();

            if (trigger.TriggeringEntityType != null)
            {
                AddInputSourceMenuItem(ActionTaskInputSource.TriggeringRecord, GetInputTriggeringRecordOption(trigger.TriggeringEntityType.Value, task.InputEntityType, false));
            }

            // If this trigger supports using the current selection as the input source...
            if (trigger.SelectionEntityType != null)
            {
                AddInputSourceMenuItem(ActionTaskInputSource.Selection, GetInputTriggeringRecordOption(trigger.SelectionEntityType.Value, task.InputEntityType, true, "selected"));
            }

            if (task.InputRequirement != ActionTaskInputRequirement.None)
            {
                AddInputSourceMenuItem(ActionTaskInputSource.FilterContents, GetInputFilterOption(task.InputEntityType));
            }

            if (task.InputRequirement == ActionTaskInputRequirement.Optional || task.InputRequirement == ActionTaskInputRequirement.None)
            {
                AddInputSourceMenuItem(ActionTaskInputSource.Nothing, "No Input");
            }
            
            labelInput.Text = task.InputLabel;
            inputSourceLink.Left = labelInput.Right;

            SelectInputSource((ActionTaskInputSource) task.Entity.InputSource);
        }

        /// <summary>
        /// Adds an input source to the itemSourceMenu
        /// </summary>
        /// <param name="inputSource">Type of input source</param>
        /// <param name="label">Label to display on the menu for this item</param>
        private void AddInputSourceMenuItem(ActionTaskInputSource inputSource, string label)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(label);
            item.Click += OnChangeInputSource;
            item.Tag = inputSource;
            inputSourceMenu.Items.Add(item);  
        }
        
        /// <summary>
        /// Select the given input source as the current one for the task
        /// </summary>
        private void SelectInputSource(ActionTaskInputSource inputSource)
        {
            // See if its a valid data source
            bool isValid = inputSourceMenu.Items.Cast<ToolStripMenuItem>().Any(i => ((ActionTaskInputSource) i.Tag) == inputSource);

            // If its not currently valid, default to the first one that is available
            if (!isValid)
            {
                inputSource = (ActionTaskInputSource) inputSourceMenu.Items[0].Tag;
            }

            // Update the task
            ActionTask.Entity.InputSource = (int) inputSource;

            // Update the displayed text
            inputSourceLink.Text = inputSourceMenu.Items.Cast<ToolStripMenuItem>().Single(i => ((ActionTaskInputSource) i.Tag) == inputSource).Text.Replace("...", "");

            // Update positioning of the filter
            inputSourceFilter.Visible = (inputSource == ActionTaskInputSource.FilterContents);
            inputSourceFilter.Left = inputSourceLink.Right;
            UpdateInputSourceFilterComboSize();

            ActionTaskEditor taskEditor = (ActionTaskEditor) panelTaskSettings.Controls[0];
            taskEditor.NotifyTaskInputChanged(trigger, (ActionTaskInputSource) task.Entity.InputSource, GetEffectiveInputEntityType());
        }

        /// <summary>
        /// Update the size of the datasource filter combo
        /// </summary>
        private void UpdateInputSourceFilterComboSize()
        {
            inputSourceFilter.Width = Math.Min(250, panelInputSource.Width - inputSourceFilter.Left);
        }

        /// <summary>
        /// Clicking the link to choose the data source
        /// </summary>
        private void OnClickInputSourceLink(object sender, EventArgs e)
        {
            inputSourceMenu.Show(inputSourceLink.Parent.PointToScreen(new Point(inputSourceLink.Left, inputSourceLink.Bottom)));
        }

        /// <summary>
        /// User has selected a different data source
        /// </summary>
        private void OnChangeInputSource(object sender, EventArgs e)
        {
            SelectInputSource((ActionTaskInputSource) ((ToolStripMenuItem) sender).Tag);
        }

        /// <summary>
        /// The filter contents to use as the data source has changed
        /// </summary>
        void OnInputSourceFilterChanged(object sender, EventArgs e)
        {
            if (inputSourceFilter.SelectedFilterNode != null)
            {
                task.Entity.InputFilterNodeID = inputSourceFilter.SelectedFilterNode.FilterNodeID;
                usesDisabledFilter = inputSourceFilter.SelectedFilterNode.Filter.State != (byte) FilterState.Enabled &&
                    inputSourceFilter.SelectedFilterNodeID != initialInputFilterNodeId;
            }
            else
            {
                task.Entity.InputFilterNodeID = 0;
                usesDisabledFilter = false;
            }

            ActionTaskEditor taskEditor = (ActionTaskEditor) panelTaskSettings.Controls[0];
            taskEditor.NotifyTaskInputChanged(trigger, (ActionTaskInputSource) task.Entity.InputSource, GetEffectiveInputEntityType());
        }

        /// <summary>
        /// Get the descriptive text to use for the given entity type in the data source menu
        /// </summary>
        public static string GetTriggeringEntityDescription(EntityType? entityType)
        {
            if (entityType == null)
            {
                return null;
            }

            switch (entityType.Value)
            {
                case EntityType.CustomerEntity: return "customer";
                case EntityType.OrderEntity: return "order";
                case EntityType.ShipmentEntity: return "shipment";
                case EntityType.OrderItemEntity: return "item";
            }

            throw new InvalidOperationException(string.Format("Unhandled EntityType {0}", entityType));
        }

        /// <summary>
        /// Get the label to use for the Input Label when choosing the triggering record option.
        /// </summary>
        private static string GetInputTriggeringRecordOption(EntityType triggering, EntityType? target, bool supportsPlural, string adjective = "")
        {
            string label = "The " + adjective + (!string.IsNullOrWhiteSpace(adjective) ? " " : "") + GetTriggeringEntityDescription(triggering);

            // The target entity is already what we want.  So just "The order" (or if plural) "The orders"
            if (target == null || target == triggering)
            {
                return label + (supportsPlural ? "s" : "");
            }

            // So this would say like "The shipment's" if plural isn't supported, or The shipments' if it is.
            label += (supportsPlural ? "s' " : "'s ");

            RelationCollection relations = EntityUtility.FindRelationChain(triggering, target.Value);
            EntityType currentEntity = triggering;

            // Build the text that represents how we get from the triggering entity to the target
            for (int i = 0; i < relations.Count; i++)
            {
                EntityRelation relation = (EntityRelation) relations[i];
                EntityType startEntity = EntityTypeProvider.GetEntityType(relation.StartEntityIsPkSide ? relation.GetPKEntityFieldCore(0).ContainingObjectName : relation.GetFKEntityFieldCore(0).ContainingObjectName);
                EntityType endEntity = EntityTypeProvider.GetEntityType(relation.StartEntityIsPkSide ? relation.GetFKEntityFieldCore(0).ContainingObjectName : relation.GetPKEntityFieldCore(0).ContainingObjectName);

                Debug.Assert(startEntity == currentEntity);
                currentEntity = endEntity;

                // Now add on the ending object type
                label += GetTriggeringEntityDescription(endEntity);

                // Determine if the end is multiple compared to a single start
                bool plural = relation.TypeOfRelation == RelationType.ManyToMany || relation.TypeOfRelation == RelationType.OneToMany;

                // Make the label plural
                if (plural)
                {
                    label += "s";
                }

                // If there is still more coming, we have to make it possessive
                if (i < relations.Count - 1)
                {
                    label += plural ? "' " : "'s ";
                }
            }

            return label;
        }

        /// <summary>
        /// Get the label to use for the Input Label when choosing the filter option
        /// </summary>
        private static string GetInputFilterOption(EntityType? target)
        {
            if (target == null)
            {
                return "Everything in...";
            }

            return string.Format("The {0}'s of everything in...", GetTriggeringEntityDescription(target));
        }

        /// <summary>
        /// The bubble is being resized
        /// </summary>
        private void OnResizeBubble(object sender, EventArgs e)
        {
            UpdateInputSourceFilterComboSize();
        }

        /// <summary>
        /// Edit the flow options for the task
        /// </summary>
        private void OnEditFlow(object sender, EventArgs e)
        {
            using (ActionTaskFlowDlg dlg = new ActionTaskFlowDlg(task, trigger, allBubbles))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    RaiseBubbleChanged();
                }
            }
        }

        /// <summary>
        /// Validates the task editor
        /// </summary>
        /// <param name="errors">Collection of errors to which new errors will be added</param>
        public void ValidateTask(ICollection<TaskValidationError> errors)
        {
            foreach (ActionTaskEditor editor in panelTaskSettings.Controls.OfType<ActionTaskEditor>())
            {
                editor.ValidateTask(errors);
            }
        }
    }
}
