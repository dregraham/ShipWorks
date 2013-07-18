using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Windows.Forms;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    public partial class BackupDatabaseTaskEditor : ActionTaskEditor
    {
        private BackupDatabaseTask task;

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
            backupPath.Validating += OnRequiredTextValidating;
            backupPath.Validated += OnTextValidated;

            textPrefix.Text = task.FilePrefix;
            textPrefix.TextChanged += OnPrefixTextChanged;
            textPrefix.Validating += OnRequiredTextValidating;
            textPrefix.Validating += OnTextPrefixValidating;
            textPrefix.Validated += OnTextValidated;

            numericBackupCount.Value = task.KeepNumberOfBackups;
            numericBackupCount.ValueChanged += OnBackupCountValueChanged;

            checkboxOnlyKeep.Checked = task.CleanOldBackups;
            checkboxOnlyKeep.CheckedChanged += OnOnlyKeepCheckChanged;
            numericBackupCount.Enabled = task.CleanOldBackups;
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
        /// The cleanup checkbox has changed
        /// </summary>
        private void OnOnlyKeepCheckChanged(object sender, EventArgs e)
        {
            task.CleanOldBackups = checkboxOnlyKeep.Checked;
            numericBackupCount.Enabled = checkboxOnlyKeep.Checked;
        }

        /// <summary>
        /// Validate that the text box has a value entered
        /// </summary>
        private void OnRequiredTextValidating(object sender, CancelEventArgs e)
        {
            TextBoxBase textBox = sender as TextBoxBase;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                errorProvider.SetError(textBox, "This value is required.");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Validate that the file prefix does not use any illegal characters
        /// </summary>
        private void OnTextPrefixValidating(object sender, CancelEventArgs e)
        {
            TextBoxBase textBox = sender as TextBoxBase;
            if (textBox != null && textPrefix.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                errorProvider.SetError(textBox, string.Format("Prefix cannot contain the following characters: {0}", 
                    Path.GetInvalidFileNameChars().Where(x => x > 31).Aggregate("", (x, y) => x + " " + y)));
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Validation has succeeded, so clear the errors
        /// </summary>
        private void OnTextValidated(object sender, EventArgs e)
        {
            TextBoxBase textBox = sender as TextBoxBase;
            if (textBox != null)
            {
                errorProvider.SetError(textBox, "");
            }
        }
    }
}
