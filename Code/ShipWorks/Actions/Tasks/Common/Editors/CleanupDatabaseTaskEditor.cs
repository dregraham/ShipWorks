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

            numericStopAfterHours.Value = task.StopAfterHours;
            numericStopAfterHours.ValueChanged += OnStopAfterHoursValueChanged;
            numericStopAfterHours.Enabled = task.StopLongCleanups;

            numericCleanupDays.Value = task.CleanupAfterDays;
            numericCleanupDays.ValueChanged += OnCleanupDaysValueChanged;

            LoadPurgeTasks();
            audit.CheckedChanged += OnPurgeCheckChanged;
            downloads.CheckedChanged += OnPurgeCheckChanged;
            email.CheckedChanged += OnPurgeCheckChanged;
            labels.CheckedChanged += OnPurgeCheckChanged;
            printJobs.CheckedChanged += OnPurgeCheckChanged;
        }

        /// <summary>
        /// Called when [purge check changed].
        /// </summary>
        private void OnPurgeCheckChanged(object sender, EventArgs e)
        {
            task.Purges = new List<CleanupDatabaseType>();
            if (audit.Checked) task.Purges.Add(CleanupDatabaseType.Audit);
            if (downloads.Checked) task.Purges.Add(CleanupDatabaseType.Downloads);
            if (email.Checked) task.Purges.Add(CleanupDatabaseType.Email);
            if (labels.Checked) task.Purges.Add(CleanupDatabaseType.Labels);
            if (printJobs.Checked) task.Purges.Add(CleanupDatabaseType.PrintJobs);
        }

        /// <summary>
        /// Loads the purge tasks.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void LoadPurgeTasks()
        {
            if (task.Purges != null)
            {
                audit.Checked = task.Purges.Contains(CleanupDatabaseType.Audit);
                downloads.Checked = task.Purges.Contains(CleanupDatabaseType.Downloads);
                email.Checked = task.Purges.Contains(CleanupDatabaseType.Email);
                labels.Checked = task.Purges.Contains(CleanupDatabaseType.Labels);
                printJobs.Checked = task.Purges.Contains(CleanupDatabaseType.PrintJobs);
            }
        }

        /// <summary>
        /// Stop after minutes value has been changed
        /// </summary> 
        private void OnStopAfterHoursValueChanged(object sender, EventArgs eventArgs)
        {
            task.StopAfterHours = (int)numericStopAfterHours.Value;
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
            numericStopAfterHours.Enabled = checkboxStopLongCleanups.Checked;
        }
    }
}
