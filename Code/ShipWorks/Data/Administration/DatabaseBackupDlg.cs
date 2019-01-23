using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI;
using Interapptive.Shared;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Connection;
using Interapptive.Shared.UI;
using System.Reactive;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Window for backing up the ShipWorks database.
    /// </summary>
    partial class DatabaseBackupDlg : Form
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DatabaseBackupDlg));
        private readonly TelemetricResult<Unit> telemetricResult;

        // The user that is performing the backup
        long userID;

        // Indicates if a backup was successfully completed
        bool backupCompleted = false;

        /// <summary>
        /// Constructor.  We take the user as input since the backup can be done as apart of an upgrade, in
        /// which it may be an old database, and thus no ShipWorksSession.User.
        /// </summary>
        public DatabaseBackupDlg(UserEntity user, TelemetricResult<Unit> telemetricResult)
        {
            InitializeComponent();

            this.userID = user.UserID;
            this.telemetricResult = telemetricResult;
        }

        /// <summary>
        /// Constructor.  We take the user as input since the backup can be done as apart of an upgrade, in
        /// which it may be an old database, and thus no ShipWorksSession.User.
        /// </summary>
        public DatabaseBackupDlg(long userID, TelemetricResult<Unit> telemetricResult)
        {
            InitializeComponent();

            this.userID = userID;
            this.telemetricResult = telemetricResult;
        }

        /// <summary>
        /// Indicates if a backup was successfully completed
        /// </summary>
        public bool BackupCompleted
        {
            get { return backupCompleted; }
        }

        /// <summary>
        /// Browse for the location of the backup file.
        /// </summary>
        private void OnBrowseBackupFile(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                backupFile.Text = saveFileDialog.FileName;
            }
        }

        /// <summary>
        /// Start the backup
        /// </summary>
        private void OnBackup(object sender, EventArgs e)
        {
            if (backupFile.Text.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please select the name of the backup file to create.");
                return;
            }

            ProgressProvider progressProvider = new ProgressProvider();

            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Backup ShipWorks";
            progressDlg.Description = "ShipWorks is creating a backup of your database.";
            progressDlg.Show(this);

            // Create the backup object
            ShipWorksBackup backup = new ShipWorksBackup(SqlSession.Current, userID, progressProvider);

            // Create the delegate to use to get it on another thread
            MethodInvoker<string, ShipWorksBackup> invoker = new MethodInvoker<string, ShipWorksBackup>(CreateBackup);

            // Initiate the backup
            invoker.BeginInvoke(backupFile.Text, backup, new AsyncCallback(OnBackupComplete), new object[] { invoker, progressDlg });
        }

        /// <summary>
        /// Create a backup
        /// </summary>
        private void CreateBackup(string fileName, ShipWorksBackup backup)
        {
            telemetricResult.RunTimedEvent("CreateBackup", () => backup.CreateBackup(fileName));
        }

        /// <summary>
        /// The backup is complete.
        /// </summary>
        private void OnBackupComplete(IAsyncResult asyncResult)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new AsyncCallback(OnBackupComplete), asyncResult);
                return;
            }

            // Extract our stateful data
            MethodInvoker<string, ShipWorksBackup> invoker = (MethodInvoker<string, ShipWorksBackup>) ((object[]) asyncResult.AsyncState)[0];
            ProgressDlg progressDlg = (ProgressDlg) ((object[]) asyncResult.AsyncState)[1];

            try
            {
                invoker.EndInvoke(asyncResult);

                // Close when the progress closes if it wasn't cancelled
                if (!progressDlg.ProgressProvider.CancelRequested)
                {
                    backupCompleted = true;
                    progressDlg.FormClosed += new FormClosedEventHandler(OnProgressSuccessClosed);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error during backup", ex);

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
            Close();
        }
    }
}