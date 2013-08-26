using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class RunProgramTaskEditor : ActionTaskEditor
    {
        private readonly RunProgramTask task;

        /// <summary>
        /// Initializes a new instance of the <see cref="RunProgramTaskEditor"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public RunProgramTaskEditor(RunProgramTask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            InitializeComponent();
            
            this.task = task;
            tokenizedExecuteCommand.Text = task.Command;
            shouldTimeout.Checked = task.ShouldStopCommandOnTimeout;
            timeoutInMinutes.Value = task.CommandTimeoutInMinutes;
            
            tokenizedExecuteCommand.TextChanged += OnTokenizedExecuteCommandTextChanged;
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
