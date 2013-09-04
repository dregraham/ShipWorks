using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Task editor for backup database task.
    /// </summary>
    public partial class BackupDatabaseTaskEditor : ActionTaskEditor
    {
        private readonly BackupDatabaseTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public BackupDatabaseTaskEditor(BackupDatabaseTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            //this.Validating += OnBackupDatabaseTaskEditorValidating;
            backupPath.Text = task.BackupDirectory;

            textPrefix.Text = task.FilePrefix;
            textPrefix.TextChanged += OnPrefixTextChanged;

            numericBackupCount.Value = task.KeepNumberOfBackups;
            numericBackupCount.ValueChanged += OnBackupCountValueChanged;

            checkboxLimitBackupsRetained.Checked = task.LimitNumberOfBackupsRetained;
            checkboxLimitBackupsRetained.CheckedChanged += OnLimitBackupsRetainedCheckChanged;
            numericBackupCount.Enabled = task.LimitNumberOfBackupsRetained;

            // Editing is not allowed unless the user is on the database server
            Enabled = SqlSession.Current.IsLocalServer();
        }

        /// <summary>
        /// Browse for the directory in which to save backups
        /// </summary>
        private void OnBrowse(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    backupPath.Text = task.BackupDirectory = dlg.SelectedPath;
                }
            }
        }

        /// <summary>
        /// The number of backups to keep has changed
        /// </summary>
        private void OnBackupCountValueChanged(object sender, EventArgs e)
        {
            task.KeepNumberOfBackups = (int) numericBackupCount.Value;
        }

        /// <summary>
        /// The file prefix has changed
        /// </summary>
        private void OnPrefixTextChanged(object sender, EventArgs e)
        {
            task.FilePrefix = textPrefix.Text;
        }

        /// <summary>
        /// Called when [limit backups retained check changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnLimitBackupsRetainedCheckChanged(object sender, EventArgs e)
        {
            task.LimitNumberOfBackupsRetained = checkboxLimitBackupsRetained.Checked;
            numericBackupCount.Enabled = checkboxLimitBackupsRetained.Checked;
        }

        /// <summary>
        /// Performs validation outside of the Windows Forms flow to make dealing with navigation easier
        /// </summary>
        /// <param name="errors">Collection of errors to which new errors will be added</param>
        public override void ValidateTask(ICollection<TaskValidationError> errors)
        {
            ActionTaskDescriptor descriptor = new ActionTaskDescriptor(task.GetType());
            TaskValidationError error = new TaskValidationError(string.Format("The '{0}' task is missing some information.", descriptor.BaseName));

            if (string.IsNullOrWhiteSpace(textPrefix.Text))
            {
                error.Details.Add("Please enter a backup file name.");
            }

            if (textPrefix.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                error.Details.Add(string.Format("The backup file name cannot contain the following characters: {0}",
                    Path.GetInvalidFileNameChars().Where(x => x > 31).Combine(" ")));
            }

            if (string.IsNullOrWhiteSpace(backupPath.Text))
            {
                error.Details.Add("Please enter a backup folder.");
            }

            // Add the error to the main errors collection if there are any validation errors
            if (error.Details.Any())
            {
                if (SqlSession.Current.IsLocalServer())
                {
                    errors.Add(error);  
                }
                else
                {
                    errors.Add(new TaskValidationError(string.Format(CultureInfo.InvariantCulture, 
                        "The '{0}' task can only be configured on the computer running your database ({1}).", 
                        descriptor.BaseName,
                        SqlSession.Current.GetServerMachineName())));
                }
            }
        }
    }
}
