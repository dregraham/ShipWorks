using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model;
using ShipWorks.Filters;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Actions.Tasks
{
    /// <summary>
    /// Window for managing the flow of a particular task in an action
    /// </summary>
    public partial class ActionTaskFlowDlg : Form
    {
        ActionTask task;
        ActionTrigger trigger;
        IEnumerable<ActionTaskBubble> allBubbles;

        static object flowOptionSameAsSuccess = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskFlowDlg(ActionTask task, ActionTrigger trigger, IEnumerable<ActionTaskBubble> allBubbles)
        {
            InitializeComponent();

            this.task = task;
            this.trigger = trigger;
            this.allBubbles = allBubbles;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            LoadConditionFilter();

            LoadFlowComboBox(whenSuccess, task.Entity.FlowSuccess, task.Entity.FlowSuccessBubble);
            LoadFlowComboBox(whenSkipped, task.Entity.FlowSkipped, task.Entity.FlowSkippedBubble);
            LoadFlowComboBox(whenError, task.Entity.FlowError, task.Entity.FlowErrorBubble);
        }

        /// <summary>
        /// Load the flow combo
        /// </summary>
        private void LoadFlowComboBox(ComboBox comboBox, long flowOption, object nextBubble)
        {
            List<KeyValuePair<string, object>> dataSource = new List<KeyValuePair<string, object>>();

            if (comboBox != whenSuccess)
            {
                dataSource.Add(new KeyValuePair<string, object>("Same as when success", flowOptionSameAsSuccess));
            }

            // Add the enum options
            dataSource.AddRange(new KeyValuePair<string, object>[] 
                {
                    new KeyValuePair<string, object>(EnumHelper.GetDescription(ActionTaskFlowOption.NextStep), ActionTaskFlowOption.NextStep),
                    new KeyValuePair<string, object>(EnumHelper.GetDescription(ActionTaskFlowOption.Quit), ActionTaskFlowOption.Quit)
                });

            if (comboBox == whenError)
            {
                dataSource.Add(new KeyValuePair<string, object>(EnumHelper.GetDescription(ActionTaskFlowOption.Suspend), ActionTaskFlowOption.Suspend));
            }

            // Add the other step options
            dataSource.AddRange(allBubbles.Select((b, i) =>
                {
                    return new KeyValuePair<string, object>(string.Format("Go to step {0}: {1}", i + 1, ActionTaskManager.GetBinding(b.ActionTask).FullName), b);
                }).Where(p => p.Value != allBubbles.SingleOrDefault(b => b.ActionTask == task)));

            comboBox.DisplayMember = "Key";
            comboBox.ValueMember = "Value";
            comboBox.DataSource = dataSource;

            if (comboBox != whenSuccess && flowOption == task.Entity.FlowSuccess)
            {
                comboBox.SelectedValue = flowOptionSameAsSuccess;
            }
            else
            {
                // See if its the enum value
                if (Enum.IsDefined(typeof(ActionTaskFlowOption), (ActionTaskFlowOption) flowOption))
                {
                    comboBox.SelectedValue = (ActionTaskFlowOption) flowOption;
                }
                // Otherwise it must be another task entity
                else
                {
                    if (allBubbles.Any(b => b == nextBubble))
                    {
                        comboBox.SelectedValue = nextBubble;
                    }
                    else
                    {
                        comboBox.SelectedValue = ActionTaskFlowOption.NextStep;
                    }
                }
            }
        }

        /// <summary>
        /// Save the option selected in the given combobox to the specified field
        /// </summary>
        private ActionTaskBubble SaveFlowOption(ComboBox comboBox, EntityField2 flowField)
        {
            if (comboBox.SelectedValue is ActionTaskFlowOption)
            {
                task.Entity.SetNewFieldValue(flowField.FieldIndex, (int) (ActionTaskFlowOption) comboBox.SelectedValue);

                return null;
            }
            else if (comboBox.SelectedValue == flowOptionSameAsSuccess)
            {
                return SaveFlowOption(whenSuccess, flowField);
            }
            else
            {
                ActionTaskBubble bubble = (ActionTaskBubble) comboBox.SelectedValue;
                task.Entity.SetNewFieldValue(flowField.FieldIndex, -(allBubbles.ToList().IndexOf(bubble) + 1));

                return bubble;
            }
        }

        /// <summary>
        /// Load the filter condition options
        /// </summary>
        private void LoadConditionFilter()
        {
            // Load the correct filters for the entity type
            if (trigger.TriggeringEntityType != null)
            {
                restrictFilterCombo.LoadLayouts(GetTriggeringEntityConditionFilterTarget(trigger.TriggeringEntityType.Value));
                restrictFilterCombo.SelectedFilterNodeID = task.Entity.FilterConditionNodeID;

                if (restrictFilterCombo.SelectedFilterNode == null)
                {
                    restrictFilterCombo.SelectFirstNode();
                }

                restrictFilter.Text = string.Format("Only if the {0} is in", GetTriggeringEntityDescription(trigger.TriggeringEntityType));
                restrictFilterCombo.Left = restrictFilter.Right;

                restrictFilter.Checked = task.Entity.FilterCondition;
            }
            else
            {
                restrictFilter.Enabled = false;
            }
        }

        /// <summary>
        /// Changing if the filter condition is enabled
        /// </summary>
        private void OnConditionEnabledChanged(object sender, EventArgs e)
        {
            restrictFilterCombo.Enabled = restrictFilter.Checked;
        }
         
        /// <summary>
        /// Get the FilterTarget to use for the run condition based on the triggering entity type
        /// </summary>
        private FilterTarget GetTriggeringEntityConditionFilterTarget(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.CustomerEntity: return FilterTarget.Customers;
                case EntityType.OrderEntity: return FilterTarget.Orders;
                case EntityType.ShipmentEntity: return FilterTarget.Shipments;
                case EntityType.OrderItemEntity: return FilterTarget.Items;
            }

            throw new ArgumentException("Invalid value passed.", "entityType");
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
        /// Confirming changes
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            ActionTaskEntity entity = task.Entity;

            // Alert the user if they have chosen to use a disabled filter
            if (restrictFilter.Checked &&
                entity.FilterConditionNodeID != restrictFilterCombo.SelectedFilterNodeID &&
                restrictFilterCombo.SelectedFilterNode != null && 
                restrictFilterCombo.SelectedFilterNode.Filter.State != (byte)FilterState.Enabled)
            {
                DialogResult result = MessageHelper.ShowQuestion(this, MessageBoxIcon.Warning, MessageBoxButtons.YesNo,
                        "The selected filter has been disabled.\n\nDo you want to use this filter anyway?");
                if (result == DialogResult.No)
                {
                    return;
                }
            }

            entity.FilterCondition = restrictFilter.Checked;
            entity.FilterConditionNodeID = restrictFilterCombo.SelectedFilterNodeID;

            entity.FlowSuccessBubble = SaveFlowOption(whenSuccess, ActionTaskFields.FlowSuccess);
            entity.FlowSkippedBubble = SaveFlowOption(whenSkipped, ActionTaskFields.FlowSkipped);
            entity.FlowErrorBubble = SaveFlowOption(whenError, ActionTaskFields.FlowError);

            DialogResult = DialogResult.OK;
        }

    }
}
