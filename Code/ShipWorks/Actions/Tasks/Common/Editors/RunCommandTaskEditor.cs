using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model;
using ShipWorks.Actions.Triggers;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks.Common.Enums;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class RunCommandTaskEditor : ActionTaskEditor
    {
        private readonly RunCommandTask task;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunCommandTaskEditor"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public RunCommandTaskEditor(RunCommandTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            InitializeComponent();

            // Load the cardinality box
            UpdateCardinalityBox(null);
            
            this.task = task;

            tokenizedExecuteCommand.Text = task.Command;
            shouldTimeout.Checked = task.ShouldStopCommandOnTimeout;
            timeoutInMinutes.Value = task.CommandTimeoutInMinutes;
            comboCardinality.SelectedValue = task.RunCardinality;
            
            tokenizedExecuteCommand.TextChanged += OnTokenizedExecuteCommandTextChanged;
        }

        /// <summary>
        /// Notifies the task that the input into it has been changed
        /// </summary>
        public override void NotifyTaskInputChanged(ActionTrigger trigger, ActionTaskInputSource inputSource, EntityType? inputType)
        {
            base.NotifyTaskInputChanged(trigger, inputSource, inputType);

            // We only show the cardinality box if there's actually an option
            if (inputSource == ActionTaskInputSource.FilterContents)
            {
                panelCardinality.Visible = true;

                UpdateCardinalityBox(inputType);
            }
            else
            {
                panelCardinality.Visible = false;
            }
        }
        /// <summary>
        /// Load the cardinality selection box
        /// </summary>
        private void UpdateCardinalityBox(EntityType? inputType)
        {
            comboCardinality.SelectedValueChanged -= OnChangeCardinality;

            object selected = comboCardinality.SelectedValue;

            comboCardinality.DataSource = null;
            comboCardinality.DisplayMember = "Key";
            comboCardinality.ValueMember = "Value";

            comboCardinality.DataSource = EnumHelper.GetEnumList<RunCommandCardinality>().Select(e => 
                new KeyValuePair<string, RunCommandCardinality>
                    (
                        string.Format(e.Description, ActionTaskBubble.GetTriggeringEntityDescription(inputType) ?? "entry"), 
                        e.Value)
                    ).ToList();

            if (selected != null)
            {
                comboCardinality.SelectedValue = selected;
            }
            else
            {
                comboCardinality.SelectedIndex = 0;
            }

            comboCardinality.SelectedValueChanged += OnChangeCardinality;
        }

        /// <summary>
        /// Changing the cardinality
        /// </summary>
        private void OnChangeCardinality(object sender, EventArgs e)
        {
            task.RunCardinality = (RunCommandCardinality) comboCardinality.SelectedValue;
        }

        /// <summary>
        /// Called when the timeout checkbox is checked/unchecked to update the timeout 
        /// property of the task and toggle the timeout values in minutes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnShouldTimeoutChecked(object sender, EventArgs e)
        {
            task.ShouldStopCommandOnTimeout = shouldTimeout.Checked;
            timeoutInMinutes.Enabled = shouldTimeout.Checked;
        }

        /// <summary>
        /// Called when [timeout value changed] to update the task's timeout property.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTimeoutValueChanged(object sender, EventArgs e)
        {
            task.CommandTimeoutInMinutes = (int)timeoutInMinutes.Value;
        }

        /// <summary>
        /// Called when [tokenized execute command text changed] to update the command property of the task.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnTokenizedExecuteCommandTextChanged(object sender, EventArgs e)
        {
            task.Command = tokenizedExecuteCommand.Text;
        }
    }
}
