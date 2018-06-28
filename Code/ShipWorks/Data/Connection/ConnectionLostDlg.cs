using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.Data.Model;
using ShipWorks.UI;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Window that is displayed when ShipWorks loses its connection to the database.
    /// </summary>
    public partial class ConnectionLostDlg : Form
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ConnectionLostDlg));

        int timeToConnect = 30;

        DbConnection connectionToOpen;
        bool closeOnSuccess = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConnectionLostDlg() : this(null)
        {

        }

        /// <summary>
        /// Constructor. The reconnection will be attempted on the specified connection.
        /// </summary>
        public ConnectionLostDlg(DbConnection connectionToOpen)
        {
            log.Info("Lost connection to the database.");

            InitializeComponent();

            this.connectionToOpen = connectionToOpen;

            if (this.connectionToOpen == null)
            {
                this.connectionToOpen = DataAccessAdapter.CreateConnection(SqlSession.Current.Configuration.GetConnectionString());
                this.closeOnSuccess = true;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, System.EventArgs e)
        {
            UpdateTimeToConnect();
        }

        /// <summary>
        /// Window is now visible.
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            AttemptReconnect(false);
        }

        /// <summary>
        /// Update the time to connect text.
        /// </summary>
        private void UpdateTimeToConnect()
        {
            label2.Text = string.Format("Reconnecting in {0} seconds.", timeToConnect);
        }

        /// <summary>
        /// Reconnection timer.
        /// </summary>
        private void OnTimer(object sender, System.EventArgs e)
        {
            timeToConnect--;

            UpdateTimeToConnect();

            if (timeToConnect == 0)
            {
                AttemptReconnect(false);
            }
        }

        /// <summary>
        /// User wants to manually attempt to reconnect to the database.
        /// </summary>
        private void OnReconnect(object sender, System.EventArgs e)
        {
            AttemptReconnect(true);
        }

        /// <summary>
        /// Attempt the reconnection
        /// </summary>
        private bool AttemptReconnect(bool showError)
        {
            log.Info("Attempting reconnection");

            label2.Text = "Attempting to reconnect...";
            Refresh();

            Cursor.Current = Cursors.WaitCursor;

            timer.Stop();

            try
            {
                if (connectionToOpen.State != ConnectionState.Open)
                {
                    connectionToOpen.Open();
                }

                ConnectionMonitor.ValidateOpenConnection(connectionToOpen);

                if (closeOnSuccess)
                {
                    connectionToOpen.Close();
                }

                // Close on success
                DialogResult = DialogResult.OK;

                return true;
            }
            catch (TransactionException ex)
            {
                log.Error("Reconnect failed", ex);
                DialogResult = DialogResult.Abort;
                return false;
            }
            catch (Exception ex) when(ConnectionMonitor.IsDbConnectionException(ex))
            {
                log.Error("Reconnect failed", ex);

                if (showError)
                {
                    MessageHelper.ShowError(this, "There was an error connecting: " + ex.Message);
                }

                timeToConnect = 31;
                timer.Start();

                return false;
            }
        }

        /// <summary>
        /// Attempts to reconnect the specified connection.
        /// </summary>
        public static ReconnectResult Reconnect(IDbConnection connection)
        {
            if (CrashDialog.IsApplicationCrashed)
            {
                Debug.Fail("Not attempting reconnect due to already crashed.");
                log.InfoFormat("Skipping reconnect due to crash.");
                return ReconnectResult.Canceled;
            }

            ReconnectResult connected = ReconnectResult.Failed;

            log.InfoFormat("About to show connection lost window (InvokeRequired = {0})", Program.MainForm.InvokeRequired.ToString());

            // There was an issue where the dialog would immediately see the connection as lost, which would cause this method to be called.
            // That would recurse until we ran out of window handles and crashed with a Win32 exception. Not showing the dialog if it is already
            // open should fix the symptom, but not the underlying cause of why the reconnect immediately failed.
            if (Program.ExecutionMode.IsUIDisplayed &&
                Application.OpenForms.OfType<ConnectionLostDlg>().None())
            {
                // We have to show the UI on the UI thread.  Otherwise behavior is a little undefined.  The only problem would be
                // if the UI thread is currently blocking waiting on a background operation that is waiting for the connection to come back.
                // But as far as I know we don't do this anywhere
                Program.MainForm.Invoke(new MethodInvoker(() =>
                    {
                        using (ConnectionLostDlg dlg = new ConnectionLostDlg((SqlConnection) connection))
                        {
                            DialogResult result = dlg.ShowDialog(DisplayHelper.GetActiveForm());

                            connected = result == DialogResult.OK ? ReconnectResult.Succeeded :
                                result == DialogResult.Cancel ? ReconnectResult.Canceled :
                                ReconnectResult.Failed;
                        }
                    }));
            }

            return connected;
        }

        /// <summary>
        /// Easter egg access to the call stack
        /// </summary>
        private void OnClickHeaderImage(object sender, EventArgs e)
        {
            if (InterapptiveOnly.MagicKeysDown)
            {
                MessageHelper.ShowMessage(this, new StackTrace().ToString());
            }
        }
    }
}
