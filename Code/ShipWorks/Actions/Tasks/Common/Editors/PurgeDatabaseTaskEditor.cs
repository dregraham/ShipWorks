using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class PurgeDatabaseTaskEditor : ActionTaskEditor
    {
        private readonly PurgeDatabaseTask task;
        private IEnumerable<(CheckBox control, PurgeDatabaseType purgeType)> PurgeEditors;

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

            PurgeEditors = BuildPurgeEditors();
        }

        /// <summary>
        /// Build the purge editor list
        /// </summary>
        private IEnumerable<(CheckBox control, PurgeDatabaseType purgeType)> BuildPurgeEditors() =>
            new[]
            {
                (audit, PurgeDatabaseType.Audit),
                (email, PurgeDatabaseType.Email),
                (labels, PurgeDatabaseType.Labels),
                (printJobs, PurgeDatabaseType.PrintJobs),
                (downloadHistory, PurgeDatabaseType.Downloads),
                (orders, PurgeDatabaseType.Orders),
            };

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

            LoadPurgeTasks();
            PurgeEditors.ForEach(x => x.control.CheckedChanged += OnPurgeCheckChanged);

            SetupSubOption(email, emailHistory, task.PurgeEmailHistory, x => task.PurgeEmailHistory = x);
            SetupSubOption(printJobs, printJobHistory, task.PurgePrintJobHistory, x => task.PurgePrintJobHistory = x);
        }

        /// <summary>
        /// Setup a sub option
        /// </summary>
        private void SetupSubOption(CheckBox parent, CheckBox child, bool startChecked, Action<bool> updateChecked)
        {
            parent.CheckedChanged += (s, evt) => child.Enabled = parent.Checked;
            child.Checked = startChecked;
            child.Enabled = parent.Checked;
            child.CheckedChanged += (s, evt) => updateChecked(child.Checked);
        }

        /// <summary>
        /// Called when [purge check changed].
        /// </summary>
        private void OnPurgeCheckChanged(object sender, EventArgs e) =>
            task.Purges = PurgeEditors.Where(x => x.control.Checked).Select(x => x.purgeType).ToList();

        /// <summary>
        /// Loads the purge tasks.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void LoadPurgeTasks() =>
            PurgeEditors
                .ForEach(x => x.control.Checked = task.Purges?.Contains(x.purgeType) == true);

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
