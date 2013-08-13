using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using System.Diagnostics;
using ShipWorks.Data.Administration.SqlServerSetup;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using ShipWorks.Data.Connection;
using System.IO;
using log4net;
using ShipWorks.Email;
using ShipWorks.Users;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Wizard for setting up ShipWorks
    /// </summary>
    public partial class ShipWorksSetupWizard : WizardForm
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksSetupWizard));

        // The sql installer
        SqlServerInstaller sqlInstaller;

        // The current state of the SQL Session data
        SqlSession sqlSession = new SqlSession();

        // Indicates if this session of the wizard has created the database
        bool createdDatabase = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWorksSetupWizard()
        {
            InitializeComponent();

            sqlInstaller = new SqlServerInstaller();
            sqlInstaller.InitializeForCurrentSqlSession();
            sqlInstaller.Exited += new EventHandler(OnInstallerLocalDbExited);
        }

        /// <summary>
        /// User wants to run the detailed database setup
        /// </summary>
        private void OnLinkDetailedSetup(object sender, EventArgs e)
        {
            using (DatabaseSetupWizard dlg = new DatabaseSetupWizard())
            {
                BeginInvoke(new MethodInvoker(Hide));

                DialogResult = dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Stepping into the Welcome page
        /// </summary>
        private void OnSteppingIntoWelcome(object sender, WizardSteppingIntoEventArgs e)
        {
            // If we're stepping back into this, clean up anything we've done so far
            DropPendingDatabase();

            // As long as we can connect to master, we know we don't need to actually install LocalDB, which means we don't need to elevate
            wizardPageWelcome.NextRequiresElevation = !CanConnectToLocalDb("master");
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            // If next requires elevation, it means we need to install localdb
            if (wizardPageWelcome.NextRequiresElevation)
            {
                try
                {
                    sqlInstaller.InstallLocalDb();
                }
                catch (Win32Exception ex)
                {
                    if (ex.NativeErrorCode == 1602 || ex.NativeErrorCode == 1603 || ex.NativeErrorCode == 1223)
                    {
                        MessageHelper.ShowInformation(this, "You must click 'Yes' when asked to allow ShipWorks to make changes to your computer.");
                    }
                    else
                    {
                        MessageHelper.ShowError(this, "An error occurred while trying to initiate the setup:\n\n" + ex.Message);
                    }

                    e.NextPage = CurrentPage;
                }
            }
            // If it doesn't require elevation, it's already installed - but we aren't sure if the ShipWorks database actually exists or not
            else
            {
                Cursor.Current = Cursors.WaitCursor;

                // If we can actually connect to the ShipWorks database, then there is nothing more for this wizard to do
                if (CanConnectToLocalDb(ShipWorksDatabaseUtility.LocalDbDatabaseName))
                {
                    sqlSession.Configuration.ServerInstance = SqlInstanceUtility.LocalDbServerInstance;
                    sqlSession.Configuration.DatabaseName = ShipWorksDatabaseUtility.LocalDbDatabaseName;
                    sqlSession.SaveAsCurrent();

                    e.NextPage = wizardPageFinishExisting;
                }
            }
        }

        /// <summary>
        /// Stepping into the install SQL Server page
        /// </summary>
        private void OnSteppingIntoInstallLocalDb(object sender, WizardSteppingIntoEventArgs e)
        {
            // We shouldn't get here stepping forward, since the StepNext of welcome would have advanced
            // to the finish, but we could get here while stepping back.
            if (createdDatabase)
            {
                e.Skip = true;
                return;
            }

            NextEnabled = false;
            BackEnabled = false;

            progressPreparing.Value = 0;

            // If elevation is required, we are going to be installing LocalDB in the background, so start the timer that will monitor that progress.
            if (wizardPageWelcome.NextRequiresElevation)
            {
                progressTimer.Start();
            }
            // Otherwise, we just need to go ahead and create the database right now
            else
            {
                MethodInvoker invoker = new MethodInvoker(CreateDatabase);
                invoker.BeginInvoke(CreateDatabaseComplete, invoker);
            }
        }

        /// <summary>
        /// Simple UI timer we use to keep the progress of LocalDb install aproximated
        /// </summary>
        void OnLocalDbProgressTimer(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(sqlInstaller.GetInstallerLocalFilePath(SqlServerInstallerPurpose.LocalDb));
            long expectedFileSize = sqlInstaller.GetInstallerFileLength(SqlServerInstallerPurpose.LocalDb);

            // Not downloaded at all
            if (!fileInfo.Exists)
            {
                progressPreparing.Value = 0;
            }
            // In the process of downloading
            else if (fileInfo.Length < expectedFileSize)
            {
                progressPreparing.Value = (int) (33.0 * (double) fileInfo.Length / expectedFileSize);

                log.InfoFormat("Updated value to {0}.  ({1} of {2})", progressPreparing.Value, fileInfo.Length, expectedFileSize);
            }
            // Fully downloaded
            else
            {
                // Download counts as 33%, the installation phase increments by 1 each 1/10th second (it installs that fast)
                progressPreparing.Value = Math.Max(33, Math.Min(progressPreparing.Value + 1, 90));

                log.InfoFormat("Installing - updated progress to {0}", progressPreparing.Value);
            }
        }

        /// <summary>
        /// The Sql Server installation has completed.
        /// </summary>
        private void OnInstallerLocalDbExited(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnInstallerLocalDbExited), new object[] { sender, e });
                return;
            }

            progressTimer.Stop();

            // If it was successful, we should now be able to connect.
            if (sqlInstaller.LastExitCode == 0 && SqlInstanceUtility.IsLocalDbInstalled())
            {
                MethodInvoker invoker = new MethodInvoker(CreateDatabase);
                invoker.BeginInvoke(CreateDatabaseComplete, invoker);
            }
            else
            {
                MessageHelper.ShowError(this,
                    "ShipWorks was unable to install the database on your computer.\n\n" + SqlServerInstaller.FormatReturnCode(sqlInstaller.LastExitCode));

                MoveBack();
            }
        }

        /// <summary>
        /// Create the ShipWorks database within LocalDB
        /// </summary>
        private void CreateDatabase()
        {
            sqlSession.Configuration.ServerInstance = SqlInstanceUtility.LocalDbServerInstance;
            sqlSession.Configuration.DatabaseName = "";

            try
            {
                Invoke((MethodInvoker) delegate { progressPreparing.Value = 90; });

                // Since we installed it, we can do this without asking
                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    SqlUtility.EnableClr(con);
                }

                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    ShipWorksDatabaseUtility.CreateDatabase(ShipWorksDatabaseUtility.LocalDbDatabaseName, con);
                    sqlSession.Configuration.DatabaseName = ShipWorksDatabaseUtility.LocalDbDatabaseName;

                    createdDatabase = true;
                }

                // Without this the next connection didn't always work...
                SqlConnection.ClearAllPools();

                Invoke((MethodInvoker) delegate { progressPreparing.Value = 98; });

                using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
                {
                    ShipWorksDatabaseUtility.CreateSchemaAndData();
                }

                Invoke((MethodInvoker) delegate { progressPreparing.Value = 100; });
            }
            // If something goes wrong, drop the db we just created
            catch
            {
                DropPendingDatabase();

                throw;
            }
        }

        /// <summary>
        /// The database creation is complete
        /// </summary>
        private void CreateDatabaseComplete(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new AsyncCallback(CreateDatabaseComplete), result);
                return;
            }

            try
            {
                MethodInvoker invoker = (MethodInvoker) result.AsyncState;
                invoker.EndInvoke(result);

                MoveNext();
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                MoveBack();
            }
            catch (SqlScriptException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                MoveBack();
            }
        }

        /// <summary>
        /// Stepping next from the install LocalDb page
        /// </summary>
        private void OnStepNextInstallLocalDb(object sender, WizardStepEventArgs e)
        {
            // LocalDB should now be fully installed and ready to go
            if (!createdDatabase)
            {
                throw new InvalidOperationException("We shouldn't be moving next from this page if we weren't the ones to install it.");
            }
        }

        /// <summary>
        /// Stepping next from the create username page
        /// </summary>
        private void OnStepNextCreateUsername(object sender, WizardStepEventArgs e)
        {
            string username = swUsername.Text.Trim();

            // Default to not moving on
            WizardPage nextPage = e.NextPage;
            e.NextPage = CurrentPage;

            if (username.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter a username.");
                return;
            }

            if (!EmailUtility.IsValidEmailAddress(swEmail.Text))
            {
                MessageHelper.ShowMessage(this, "Please enter a valid email address.");
                return;
            }

            if (swPassword.Text != swPasswordAgain.Text)
            {
                MessageHelper.ShowMessage(this, "The passwords you typed do not match.");
                return;
            }

            try
            {
                using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
                {
                    UserUtility.CreateUser(username, swEmail.Text, swPassword.Text, true);
                }

                // Now we can move on
                e.NextPage = nextPage;
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowMessage(this, ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                MessageHelper.ShowMessage(this, ex.Message);
            }

            // Save and commit the database creation
            createdDatabase = false;
            sqlSession.SaveAsCurrent();

            // Initialize the session
            UserSession.InitializeForCurrentDatabase();

            // Logon the user
            UserSession.Logon(username, swPassword.Text, true);

            // We're done - the AddStoreWizard should open next
            e.NextPage = CurrentPage;
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Indicates if the LocalDB is installed and able to connect to the given database
        /// </summary>
        public static bool CanConnectToLocalDb(string database)
        {
            if (SqlInstanceUtility.IsLocalDbInstalled())
            {
                SqlSession session = new SqlSession();

                session.Configuration.ServerInstance = SqlInstanceUtility.LocalDbServerInstance;
                session.Configuration.DatabaseName = database;

                return session.CanConnect();
            }

            return false;
        }

        /// <summary>
        /// Drop the database that we created due to the use going back or cancelling
        /// the wizard.
        /// </summary>
        private void DropPendingDatabase()
        {
            if (!createdDatabase)
            {
                return;
            }

            try
            {
                ShipWorksDatabaseUtility.DropDatabase(sqlSession, ShipWorksDatabaseUtility.LocalDbDatabaseName);

            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this, "There was an error rolling back the created database.\n\n" + ex.Message);
            }

            sqlSession.Configuration.DatabaseName = "";
            createdDatabase = false;
        }

        /// <summary>
        /// Cancell the installation of sql server
        /// </summary>
        private void OnCancellInstallLocalDb(object sender, CancelEventArgs e)
        {
            if (!NextEnabled)
            {
                MessageHelper.ShowMessage(this, "Please wait for the preparations to complete.");
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Cleanup when closed.
        /// </summary>
        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            DropPendingDatabase();
        }
    }
}
