using System;
using System.Collections.Generic;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class PurgeDatabaseTaskEditor : ActionTaskEditor
    {
        private readonly PurgeDatabaseTask task;

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
            reclaimDiskSpaceCheckbox.Checked = task.ReclaimDiskSpace;

            timeoutPurgeCheckbox.Checked = task.CanTimeout;
            timeoutPurgeCheckbox.CheckedChanged += OnTimeoutPurgeCheckedChanged;

            timeoutInHours.Value = task.TimeoutInHours;
            timeoutInHours.ValueChanged += OnTimeoutInHoursValueChanged;
            timeoutInHours.Enabled = task.CanTimeout;

            retentionPeriodInDays.Value = task.RetentionPeriodInDays;
            retentionPeriodInDays.ValueChanged += OnRetentionPeriodInDaysValueChanged;

            emailHistory.Checked = task.PurgeEmailHistory;
            printJobHistory.Checked = task.PurgePrintJobHistory;

            emailHistory.CheckedChanged += (s, evt) => task.PurgeEmailHistory = emailHistory.Checked;
            printJobHistory.CheckedChanged += (s, evt) => task.PurgePrintJobHistory = printJobHistory.Checked;
            
            LoadPurgeTasks();
            audit.CheckedChanged += OnPurgeCheckChanged;
            email.CheckedChanged += OnPurgeCheckChanged;
            email.CheckedChanged += (s, evt) => emailHistory.Enabled = email.Checked;
            labels.CheckedChanged += OnPurgeCheckChanged;
            printJobs.CheckedChanged += OnPurgeCheckChanged;
            printJobs.CheckedChanged += (s, evt) => printJobHistory.Enabled = printJobs.Checked;
            orders.CheckedChanged += OnPurgeCheckChanged;

            emailHistory.Enabled = email.Checked;
            printJobHistory.Enabled = printJobs.Checked;
        }

        /// <summary>
        /// Called when [purge check changed].
        /// </summary>
        private void OnPurgeCheckChanged(object sender, EventArgs e)
        {
            task.Purges = new List<PurgeDatabaseType>();

            if (audit.Checked)
            {
                task.Purges.Add(PurgeDatabaseType.Audit);
            }

            if (email.Checked)
            {
                task.Purges.Add(PurgeDatabaseType.Email);
            }

            if (labels.Checked)
            {
                task.Purges.Add(PurgeDatabaseType.Labels);
            }

            if (printJobs.Checked)
            {
                task.Purges.Add(PurgeDatabaseType.PrintJobs);
            }

            if (orders.Checked)
            {
                task.Purges.Add(PurgeDatabaseType.Orders);
            }
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
                email.Checked = task.Purges.Contains(PurgeDatabaseType.Email);
                labels.Checked = task.Purges.Contains(PurgeDatabaseType.Labels);
                printJobs.Checked = task.Purges.Contains(PurgeDatabaseType.PrintJobs);
                orders.Checked = task.Purges.Contains(PurgeDatabaseType.Orders);
            }
        }

        /// <summary>
        /// Stop after minutes value has been changed
        /// </summary> 
        private void OnTimeoutInHoursValueChanged(object sender, EventArgs eventArgs)
        {
            task.TimeoutInHours = (int) timeoutInHours.Value;
        }

        /// <summary>
        /// Retention period value has been changed
        /// </summary>
        private void OnRetentionPeriodInDaysValueChanged(object sender, EventArgs eventArgs)
        {
            task.RetentionPeriodInDays = (int) retentionPeriodInDays.Value;
        }

        /// <summary>
        /// Stop long cleanups checkbox value has been changed
        /// </summary>
        private void OnTimeoutPurgeCheckedChanged(object sender, EventArgs eventArgs)
        {
            task.CanTimeout = timeoutPurgeCheckbox.Checked;
            timeoutInHours.Enabled = timeoutPurgeCheckbox.Checked;
        }

        /// <summary>
        /// Reclaim disk space has changed
        /// </summary>
        private void OnReclaimDiskSpaceCheckedChanged(object sender, EventArgs e)
        {
            task.ReclaimDiskSpace = reclaimDiskSpaceCheckbox.Checked;
        }
    }
}
