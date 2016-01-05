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
using Autofac;
using Autofac.Core;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Email;
using ShipWorks.Users;
using ShipWorks.ApplicationCore.Setup;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Wizard for setting up ShipWorks
    /// </summary>
    public partial class SimpleDatabaseSetupWizard : WizardForm
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SimpleDatabaseSetupWizard));

        // The sql installer
        SqlServerInstaller sqlInstaller;

        // The current state of the SQL Session data
        SqlSession sqlSession = new SqlSession();

        // Indicates if this session of the wizard has created the database
        bool createdDatabase = false;

        // How we need to elevate
        ElevatedPreparationType preparationType = ElevatedPreparationType.None;

        // The user setup wizard page
        private readonly ICustomerLicenseActivation tangoUserControlHost;

        /// <summary>
        /// What we need to do from elevation
        /// </summary>
        enum ElevatedPreparationType
        {
            /// <summary>
            /// Nothing to do from elevation - but we may still need to create an actual database
            /// </summary>
            None,

            /// <summary>
            /// Install LocalDb
            /// </summary>
            InstallLocalDb,

            /// <summary>
            /// LocalDb has been upgraded to the full version of SQL Server at some point, but we still need to assign a database name for this ShipWorks session
            /// </summary>
            AssignFullDatabaseName
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SimpleDatabaseSetupWizard()
        {
            InitializeComponent();

            sqlInstaller = new SqlServerInstaller();
            sqlInstaller.InitializeForCurrentSqlSession();
            sqlInstaller.Exited += OnPrepareAutomaticDatabaseExited;

            // Resolve the user control
            tangoUserControlHost = IoC.UnsafeGlobalLifetimeScope.Resolve<ICustomerLicenseActivation>();
            tangoUserControlHost.StepNext += OnStepNextCreateUsername;

            // Replace the user wizard page with the new tango user wizard page
            Pages.Insert(Pages.Count - 1, (WizardPage)tangoUserControlHost);
        }

        /// <summary>
        /// Stepping into the Welcome page
        /// </summary>
        private void OnSteppingIntoWelcome(object sender, WizardSteppingIntoEventArgs e)
        {
            // If we're stepping back into this, clean up anything we've done so far
            DropPendingDatabase();

            // Determine what we need to do from the elevation
            preparationType = DetermineElevationPreparationType();

            // See if we are going to require elevation to do what we need to do
            startFromScratch.ShowShield = (preparationType != ElevatedPreparationType.None);

            // User has to click one of our buttons
            NextVisible = false;
        }

        /// <summary>
        /// Same as moving next
        /// </summary>
        private void OnStartFromScratch(object sender, EventArgs e)
        {
            MoveNext();
        }

        /// <summary>
        /// User wants to run the detailed database setup
        /// </summary>
        private void OnOpenDetailedSetup(object sender, EventArgs e)
        {
            using (DetailedDatabaseSetupWizard dlg = new DetailedDatabaseSetupWizard())
            {
                BeginInvoke(new MethodInvoker(Hide));

                DialogResult = dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            // If next requires elevation
            if (preparationType != ElevatedPreparationType.None)
            {
                try
                {
                    if (preparationType == ElevatedPreparationType.InstallLocalDb)
                    {
                        sqlInstaller.InstallLocalDb();
                    }
                    else
                    {
                        sqlInstaller.AssignAutomaticDatabaseName();
                    }
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
            // If it doesn't require elevation, SQL Server is ready to go - but we aren't sure if the ShipWorks database actually exists or not
            else
            {
                Cursor.Current = Cursors.WaitCursor;

                // If we can actually connect to the ShipWorks database, then there is nothing more for this wizard to do
                if (CanConnectToAutomaticDatabase())
                {
                    sqlSession.Configuration.ServerInstance = SqlInstanceUtility.AutomaticServerInstance;
                    sqlSession.Configuration.DatabaseName = ShipWorksDatabaseUtility.AutomaticDatabaseName;
                    sqlSession.SaveAsCurrent();

                    e.NextPage = wizardPageFinishExisting;
                }
            }

            // Reset next visibility
            if (e.NextPage != CurrentPage)
            {
                NextVisible = true;
            }
        }

        /// <summary>
        /// Stepping into the prepare automatic database page
        /// </summary>
        private void OnSteppingIntoPrepareAutomaticDatabase(object sender, WizardSteppingIntoEventArgs e)
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

            // If we are going to be installing LocalDB in the background, start the timer that will monitor that progress.
            if (preparationType == ElevatedPreparationType.InstallLocalDb)
            {
                localDbProgressTimer.Start();
            }
            // Otherwise, we just need to go ahead and create the database right now
            else if (preparationType == ElevatedPreparationType.None)
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
            }
            // Fully downloaded
            else
            {
                // Download counts as 33%, the installation phase increments by 1 each 1/10th second (it installs that fast)
                progressPreparing.Value = Math.Max(33, Math.Min(progressPreparing.Value + 1, 90));
            }
        }

        /// <summary>
        /// The elevated preparations are coplete
        /// </summary>
        private void OnPrepareAutomaticDatabaseExited(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnPrepareAutomaticDatabaseExited), new object[] { sender, e });
                return;
            }

            localDbProgressTimer.Stop();

            // If it was successful, we should now be able to connect.
            if (sqlInstaller.LastExitCode == 0)
            {
                MethodInvoker invoker = new MethodInvoker(CreateDatabase);
                invoker.BeginInvoke(CreateDatabaseComplete, invoker);
            }
            else
            {
                MessageHelper.ShowError(this,
                    "ShipWorks was unable to prepare your computer.\n\n" + SqlServerInstaller.FormatReturnCode(sqlInstaller.LastExitCode));

                MoveBack();
            }
        }

        /// <summary>
        /// Stepping next from the create username page
        /// </summary>
        private void OnStepNextCreateUsername(object sender, WizardStepEventArgs e)
        {
            GenericResult<ICustomerLicense> result;

            using (new SqlSessionScope(sqlSession))
            {
                result = tangoUserControlHost.Save();
            }

            if (result.Success == false)
            {
                MessageHelper.ShowMessage(this, result.Message);
                e.NextPage = (WizardPage)tangoUserControlHost;
                return;
            }

            // Save and commit the database creation
            createdDatabase = false;
            sqlSession.SaveAsCurrent();

            // Now we propel them right into our add store wizard
            AddStoreWizard.ContinueAfterCreateDatabase(this, tangoUserControlHost.ViewModel.Username, tangoUserControlHost.ViewModel.DecryptedPassword);
        }

        /// <summary>
        /// Create the ShipWorks database within automatic server instance
        /// </summary>
        private void CreateDatabase()
        {
            string automaticInstance = SqlInstanceUtility.AutomaticServerInstance;

            SqlSessionConfiguration configuration = SqlInstanceUtility.DetermineCredentials(automaticInstance);
            if (configuration == null)
            {
                throw new SqlScriptException(string.Format("Could not connect to '{0}' with the expected credentials.", automaticInstance));
            }

            sqlSession.Configuration.CopyFrom(configuration);
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
                    ShipWorksDatabaseUtility.CreateDatabase(ShipWorksDatabaseUtility.AutomaticDatabaseName, con);
                    sqlSession.Configuration.DatabaseName = ShipWorksDatabaseUtility.AutomaticDatabaseName;

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
        /// Stepping next from the preparation page
        /// </summary>
        private void OnStepNextPrepareAutomaticDatabase(object sender, WizardStepEventArgs e)
        {
            // Database should have been created if we were on that page
            if (!createdDatabase)
            {
                throw new InvalidOperationException("We shouldn't be moving next from this page if we didn't create a database - we should have skipped it");
            }
        }

        /// <summary>
        /// Determines what, if anything, we need to do from background elevation
        /// </summary>
        private ElevatedPreparationType DetermineElevationPreparationType()
        {
            // If the instance we are connecting to is LocalDb, then we just need to know if we can connect to LocalDb at all.  Even if we have to create
            // a database, we do that without elevation.
            if (SqlInstanceUtility.AutomaticServerInstance == SqlInstanceUtility.LocalDbServerInstance)
            {
                return CanConnectToLocalDb("master") ? ElevatedPreparationType.None : ElevatedPreparationType.InstallLocalDb;
            }
            else
            {
                string database = ShipWorksDatabaseUtility.AutomaticDatabaseName;

                // If the automatic name doesn't exist yet, then we need elevation to assign it, as that goes into HKLM
                return (database == null) ? ElevatedPreparationType.AssignFullDatabaseName : ElevatedPreparationType.None;
            }
        }

        /// <summary>
        /// Indicates if we can fully connect to the automatic database
        /// </summary>
        private bool CanConnectToAutomaticDatabase()
        {
            SqlSessionConfiguration configuration = SqlInstanceUtility.DetermineCredentials(SqlInstanceUtility.AutomaticServerInstance);
            if (configuration == null)
            {
                return false;
            }

            SqlSession session = new SqlSession(configuration);
            session.Configuration.DatabaseName = ShipWorksDatabaseUtility.AutomaticDatabaseName;

            return session.CanConnect();
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
        /// Drop the database that we created due to the use going back or cancelling the wizard.
        /// </summary>
        private void DropPendingDatabase()
        {
            if (!createdDatabase)
            {
                return;
            }

            try
            {
                ShipWorksDatabaseUtility.DropDatabase(sqlSession, ShipWorksDatabaseUtility.AutomaticDatabaseName);

            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this, "There was an error rolling back the created database.\n\n" + ex.Message);
            }

            sqlSession.Configuration.DatabaseName = "";
            createdDatabase = false;
        }

        /// <summary>
        /// Cancel the install \ preparation of the automatic sql server database
        /// </summary>
        private void OnCancelPrepareAutomaticDatabase(object sender, CancelEventArgs e)
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
