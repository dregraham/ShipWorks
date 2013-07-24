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
    public partial class PurgeDatabaseTaskEditor : ActionTaskEditor
    {
        private PurgeDatabaseTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="task">Task which should be edited</param>
        public PurgeDatabaseTaskEditor(PurgeDatabaseTask task)
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
            task.Purges = new List<PurgeDatabaseType>();
            if (audit.Checked) task.Purges.Add(PurgeDatabaseType.Audit);
            if (downloads.Checked) task.Purges.Add(PurgeDatabaseType.Downloads);
            if (email.Checked) task.Purges.Add(PurgeDatabaseType.Email);
            if (labels.Checked) task.Purges.Add(PurgeDatabaseType.Labels);
            if (printJobs.Checked) task.Purges.Add(PurgeDatabaseType.PrintJobs);
        }

        /// <summary>
        /// Loads the purge tasks.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void LoadPurgeTasks()
        {
            if (task.Purges != null)
            {
                audit.Checked = task.Purges.Contains(PurgeDatabaseType.Audit);
                downloads.Checked = task.Purges.Contains(PurgeDatabaseType.Downloads);
                email.Checked = task.Purges.Contains(PurgeDatabaseType.Email);
                labels.Checked = task.Purges.Contains(PurgeDatabaseType.Labels);
                printJobs.Checked = task.Purges.Contains(PurgeDatabaseType.PrintJobs);
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
