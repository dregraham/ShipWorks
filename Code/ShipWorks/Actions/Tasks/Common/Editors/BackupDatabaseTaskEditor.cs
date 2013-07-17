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
            backupPath.Text = task.BackupPath;

            textPrefix.Text = task.FilePrefix;
            textPrefix.TextChanged += OnPrefixTextChanged;

            numericBackupCount.Value = task.KeepNumberOfBackups;
            numericBackupCount.ValueChanged += OnBackupCountValueChanged;
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
                    backupPath.Text = task.BackupPath = dlg.SelectedPath;
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
    }
}
