using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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
        private string errorMessage = string.Empty;

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
            this.Validating += OnBackupDatabaseTaskEditorValidating;
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
        /// Validate the user entered data
        /// </summary>
        void OnBackupDatabaseTaskEditorValidating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textPrefix.Text))
            {
                errorMessage = string.Format("Please enter a backup file name.{0}", Environment.NewLine);
                e.Cancel = true;
            }

            if (textPrefix.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                errorMessage += string.Format("The backup file name cannot contain the following characters: {0}{1}",
                    Path.GetInvalidFileNameChars().Where(x => x > 31).Aggregate("", (x, y) => x + " " + y), Environment.NewLine);

                e.Cancel = true;
            }

            if (string.IsNullOrWhiteSpace(backupPath.Text))
            {
                errorMessage += string.Format("Please enter a backup folder.{0}", Environment.NewLine);
                e.Cancel = true;
            }

            // If there was an error, show the user 
            if (e.Cancel)
            {
                MessageHelper.ShowError(this, errorMessage);
            }

            errorMessage = string.Empty;
        }
    }
}
