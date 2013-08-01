using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Connection;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
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
            backupPath.Text = task.BackupDirectory;
            backupPath.Validating += OnBackupPathValidating;
            backupPath.Validated += OnBackupPathValidated;

            textPrefix.Text = task.FilePrefix;
            textPrefix.TextChanged += OnPrefixTextChanged;
            textPrefix.Validating += OnTextPrefixValidating;
            textPrefix.Validated += OnTextPrefixValidated;

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
        /// Validate that the file prefix does not use any illegal characters
        /// </summary>
        private void OnTextPrefixValidating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textPrefix.Text))
            {
                errorProvider.SetError(textPrefix, "This value is required.");
                e.Cancel = true;
            }

            if (textPrefix.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                errorProvider.SetError(textPrefix, string.Format("Backup name cannot contain the following characters: {0}", 
                    Path.GetInvalidFileNameChars().Where(x => x > 31).Aggregate("", (x, y) => x + " " + y)));

                e.Cancel = true;
            }
        }

        /// <summary>
        /// Clear the validation error for the prefix text box
        /// </summary>
        private void OnTextPrefixValidated(object sender, EventArgs e)
        {
            errorProvider.SetError(textPrefix, "");
        }

        /// <summary>
        /// Validate that the text box has a value entered
        /// </summary>
        private void OnBackupPathValidating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(backupPath.Text))
            {
                errorProvider.SetError(browse, "This value is required.");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Clear the validation error for the backup path text box
        /// </summary>
        private void OnBackupPathValidated(object sender, EventArgs e)
        {
            // Use the browse button as the error control so that the validation
            // UI shows up next to it instead of over it
            errorProvider.SetError(browse, "");
        }
    }
}
