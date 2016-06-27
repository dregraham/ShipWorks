using System;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Administration.UpdateFrom2x.Configuration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Window for restoring the ShipWorks database
    /// </summary>
    partial class DatabaseRestoreDlg : Form
    {
        // The user that is performing the backup
        UserEntity user;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseRestoreDlg(UserEntity user)
        {
            InitializeComponent();

            this.user = user;
        }

        /// <summary>
        /// Browse for the backup file
        /// </summary>
        private void OnBrowse(object sender, EventArgs e)
        {
            if (openBackupFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                backupFile.Text = openBackupFileDialog.FileName;
            }
        }

        /// <summary>
        /// Starts the restore
        /// </summary>
        private void OnRestore(object sender, EventArgs e)
        {
            if (backupFile.Text.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please select a ShipWorks backup file.");
                return;
            }

            Messenger.Current.Send(new WindowResettingMessage(this));

            ProgressProvider progressProvider = new ProgressProvider();

            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Restore ShipWorks";
            progressDlg.Description = "ShipWorks is restoring the backup of your database.";
            progressDlg.Show(this);

            // Create the backup object
            ShipWorksBackup backup = new ShipWorksBackup(SqlSession.Current, user, progressProvider);

            // Create the delegate to use to get it on another thread
            MethodInvoker<string> invoker = new MethodInvoker<string>(backup.RestoreBackup);

            // Initiate the restore
            invoker.BeginInvoke(backupFile.Text, new AsyncCallback(OnRestoreComplete), new object[] { invoker, progressDlg });
        }

        /// <summary>
        /// The restore is complete.
        /// </summary>
        private void OnRestoreComplete(IAsyncResult asyncResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncCallback(OnRestoreComplete), asyncResult);
                return;
            }

            // Extract our stateful data
            MethodInvoker<string> invoker = (MethodInvoker<string>) ((object[]) asyncResult.AsyncState)[0];
            ProgressDlg progressDlg = (ProgressDlg) ((object[]) asyncResult.AsyncState)[1];

            try
            {
                invoker.EndInvoke(asyncResult);

                // Close when the progress closes if it wasn't canceled
                if (!progressDlg.ProgressProvider.CancelRequested)
                {
                    progressDlg.FormClosed += new FormClosedEventHandler(OnProgressSuccessClosed);
                }

                // If this is an old backup file, the upgrade wizard will run, and it will get the default
                // place to get templates from this.
                ConfigurationMigrationState.ApplicationDataSource = new ShipWorks2xApplicationDataSource
                {
                    SourceType = ShipWorks2xApplicationDataSourceType.BackupFile,
                    Path = backupFile.Text
                };
            }
            catch (Exception ex)
            {
                progressDlg.ProgressProvider.Terminate(ex);

                MessageHelper.ShowError(progressDlg, ex.Message);
                progressDlg.Close();
            }
        }

        /// <summary>
        /// Called when the progress window is closed after a successful backup.
        /// </summary>
        void OnProgressSuccessClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}