using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class CleanupDatabaseTaskEditor : ActionTaskEditor
    {
        private CleanupDatabaseTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="task">Task which should be edited</param>
        public CleanupDatabaseTaskEditor(CleanupDatabaseTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Initialize form
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            checkboxStopLongCleanups.Checked = task.StopLongCleanups;
            checkboxStopLongCleanups.CheckedChanged += OnStopLongCleanupsCheckedChanged;

            numericStopAfterMinutes.Value = task.StopAfterMinutes;
            numericStopAfterMinutes.ValueChanged += OnStopAfterMinutesValueChanged;
            numericStopAfterMinutes.Enabled = task.StopLongCleanups;

            numericCleanupDays.Value = task.CleanupAfterDays;
            numericCleanupDays.ValueChanged += OnCleanupDaysValueChanged;
        }

        /// <summary>
        /// Stop after minutes value has been changed
        /// </summary>
        private void OnStopAfterMinutesValueChanged(object sender, EventArgs eventArgs)
        {
            task.StopAfterMinutes = (int)numericStopAfterMinutes.Value;
        }

        /// <summary>
        /// Cleanup days value has been changed
        /// </summary>
        private void OnCleanupDaysValueChanged(object sender, EventArgs eventArgs)
        {
            task.CleanupAfterDays = (int)numericCleanupDays.Value;
        }

        /// <summary>
        /// Stop long cleanups checkbox value has been changed
        /// </summary>
        private void OnStopLongCleanupsCheckedChanged(object sender, EventArgs eventArgs)
        {
            task.StopLongCleanups = checkboxStopLongCleanups.Checked;
            numericStopAfterMinutes.Enabled = checkboxStopLongCleanups.Checked;
        }
    }
}
