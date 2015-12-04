using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;
using log4net;
using System.Data.SqlClient;
using Interapptive.Shared;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Templates.Emailing;
using ShipWorks.Email;
using ShipWorks.Data.Administration.SqlServerSetup;
using ShipWorks.ApplicationCore.Interaction;
using System.Xml.Linq;
using ShipWorks.Users.Security;
using ShipWorks.Users.Audit;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Administration.UpdateFrom2x.Database;
using System.Reflection;
using ShipWorks.Data.Administration.UpdateFrom2x;
using System.Linq;
using ShipWorks.Data.Administration.UpdateFrom2x.Configuration;
using ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration;
using ShipWorks.Stores;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.ShippingPages;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.PostMigration.StorePages;
using System.IO;
using Interapptive.Shared.Win32;
using System.Diagnostics;
using ShipWorks.Users.Logon;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Wizard for walking a user throught upgrading the database
    /// </summary>
    [NDependIgnoreLongTypes]
    partial class DatabaseUpdateWizard : WizardForm
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DatabaseUpdateWizard));

        // Version of the database installed
        Version installed;

        // If its a 3.x database, this is the user we are running the upgrade as.
        // Note: We can't use the UserEntity, b\c if we are upgrading, it could be different
        // now than it is in the db schema.
        long userID3x;
        string userName3x;

        // For 2x admins, we log them on under the new way after upgrade
        string logonAfterUsername;
        string logonAfterPassword;

        SqlServerInstaller sqlInstaller;
        WizardDownloadHelper sqlDownloader;

        // Indicates if a backup has been completed
        bool backupCompleted = false;

        // Indicates if the firewall has been opened'
        bool showFirewallPage = false;
        bool firewallOpened = false;

        // Handles V2 to V3 data migration
        MigrationController v2migrationController;

        /// <summary>
        /// Open the upgrade window and returns true if the wizard completed with an OK result.
        /// </summary>
        public static bool Run(IWin32Window owner)
        {
            if (SqlSession.Current.DetermineMissingPermissions(SqlSessionPermissionSet.Upgrade).Count > 0)
            {
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    MessageHelper.ShowError(owner,
                        string.Format(
                            "ShipWorks needs to update your ShipWorks database to work with this version of ShipWorks.\n\n" +
                            "However, the SQL Server user '{0}' does not have the required SQL Server permissions to upgrade the database.", SqlUtility.GetUsername(con)));
                }

                return false;
            }

            using (DatabaseUpdateWizard dlg = new DatabaseUpdateWizard())
            {
                return dlg.ShowDialog(owner) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private DatabaseUpdateWizard()
        {
            InitializeComponent();

            installed = SqlServerInstaller.IsMsdeMigrationInProgress ? new Version(0,0,0,0) : SqlSchemaUpdater.GetInstalledSchemaVersion();
            showFirewallPage = installed < new Version(3, 0);

            sqlInstaller = new SqlServerInstaller();
            sqlInstaller.InitializeForCurrentSqlSession();
            sqlInstaller.Exited += new EventHandler(OnUpgraderSqlServerExited);

            sqlDownloader = new WizardDownloadHelper(this, 
                sqlInstaller.GetInstallerLocalFilePath(SqlServerInstallerPurpose.Upgrade), 
                sqlInstaller.GetInstallerDownloadUri(SqlServerInstallerPurpose.Upgrade));

            // Remove the placeholder...
            int placeholderIndex = Pages.IndexOf(wizardPagePrerequisitePlaceholder);
            Pages.Remove(wizardPagePrerequisitePlaceholder);

            if (!SqlServerInstaller.IsMsdeMigrationInProgress)
            {
                // If its not the current sql version, then sql is going to be upgraded, which means we need some prereqs
                if (!SqlSession.Current.IsSqlServer2008OrLater())
                {
                    // Add the pages to detect and install .NET 3.5 SP1 if necessary
                    DotNet35DownloadPage dotNet35Page = new DotNet35DownloadPage(
                        wizardPageLogin,
                        StartupAction.ContinueDatabaseUpgrade,
                        () => { return new XElement("Empty"); });
                    Pages.Insert(placeholderIndex, dotNet35Page);

                    // Add the pages to detect and install windows installer if necessary
                    WindowsInstallerDownloadPage windowsInstallerPage = new WindowsInstallerDownloadPage(
                        dotNet35Page,
                        StartupAction.ContinueDatabaseUpgrade,
                        () => { return new XElement("Empty"); });
                    Pages.Insert(placeholderIndex, windowsInstallerPage);
                }

                if (installed.Major < 3)
                {
                    v2migrationController = MigrationController.CreateV2ToV3Controller(installed);

                    log.InfoFormat("Created V2 migration controller: {0}", v2migrationController.MigrationState);
                }

                // Add in the post v2 migration pages if the v2 migration is still in progress
                if (v2migrationController != null || MigrationController.IsMigrationInProgress())
                {
                    InsertV2PostMigrationPages();
                }
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            noSingleUserMode.Visible = InterapptiveOnly.IsInterapptiveUser && Debugger.IsAttached;

            if (StartupController.StartupAction == StartupAction.ContinueDatabaseUpgrade)
            {
                if (!SqlServerInstaller.IsMsdeMigrationInProgress && StartupController.StartupArgument.Name == "AfterInstallSuccess")
                {
                    log.InfoFormat("Replaying upgrade SQL Server after install success needed reboot.");

                    // Since we installed it, we can do this without asking
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        SqlUtility.SetSql2008CompatibilityLevel(con);
                        SqlUtility.EnableClr(con);
                    }

                    showFirewallPage = true;

                    // To have gotten to the point where they installed SQL Server, they would have had to have already
                    // done a successful backup
                    MarkBackupCompleted();
                }

                StartupController.ClearStartupAction();
            }
        }

        #region Upgrade Info

        /// <summary>
        /// Stepping into the upgrade info page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnSteppingIntoUpgradeInfo(object sender, WizardSteppingIntoEventArgs e)
        {
            if (SqlServerInstaller.IsMsdeMigrationInProgress)
            {
                e.Skip = true;
                return;
            }

            // If its 08, we move right on to the db upgrade
            if (SqlSession.Current.IsSqlServer2008OrLater())
            {
                e.Skip = true;
                return;
            }

            // If its not local we cant do anything
            if (!SqlSession.Current.IsLocalServer())
            {
                NextEnabled = false;
                labelCantUpgrade.BringToFront();
                return;
            }

            // We can upgrade, show some info
            panelCanUpgrade.BringToFront();

            labelDatabaseList.Text = "";

            // Show them a list of databases that will be affected
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("select name from master..sysdatabases where name not in ('master', 'model', 'msdb', 'tempdb')", con);
                DataTable namesTable = new DataTable();
                dataAdapter.Fill(namesTable);

                List<string> databases = namesTable.Rows.Cast<DataRow>().Select(r => (string) r[0]).ToList();

                // Go through each datababase
                foreach (string name in databases.ToList())
                {
                    // Could have already been removed from the list if its an archive
                    if (!databases.Contains(name))
                    {
                        continue;
                    }

                    con.ChangeDatabase(name);

                    List<string> archives = ShipWorks2xArchiveUtility.GetArchiveDatabaseNames(con);
                    if (archives.Count > 0)
                    {
                        databases[databases.IndexOf(name)] = string.Format("{0} (with {1} archives)", name, archives.Count);

                        // Don't include the archive names in our database list we show to the user
                        archives.ForEach(a => databases.Remove(a));
                    }
                }

                foreach (string name in databases)
                {
                    labelDatabaseList.Text += string.Format("{0}\r\n", name);
                }
            }
        }

        #endregion

        #region Login

        /// <summary>
        /// Stepping into the login page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnSteppingIntoLogin(object sender, WizardSteppingIntoEventArgs e)
        {
            if (SqlServerInstaller.IsMsdeMigrationInProgress)
            {
                e.Skip = true;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            const string loggedInNoRights = "You are currently logged on to ShipWorks as '{0}', a user that does not have rights to update the ShipWorks database.";
            const string loggedInNeedAdmin = "Please log on to ShipWorks as an administrator to update the ShipWorks database.";
            const string loggedInNeedRights = "Please log on to ShipWorks as a user that has rights to update the ShipWorks database.";

            // Below db version 1.2.0 (ShipWorks 2.4), there were no users.  So to be logged
            // in at all is to be an admin.
            if (installed < new Version(1, 2))
            {
                log.Debug("Pre 2.4 database, no need for admin logon.");

                e.Skip = true;
                return;
            }
            // See if we need to login 2.x style
            else if (installed < new Version(3, 0))
            {
                // If there are no admin users, it would be impossible to go on
                if (!UserUtility.Has2xAdminUsers())
                {
                    e.Skip = true;
                    return;
                }

                string username;
                string password;

                // See if we can try to login automatically
                if (UserSession.GetSavedUserCredentials(out username, out password))
                {
                    log.Debug("2.x credentials found, attempting admin login.");

                    if (UserUtility.IsShipWorks2xAdmin(username, password))
                    {
                        logonAfterUsername = username;
                        logonAfterPassword = password;

                        e.Skip = true;
                        return;
                    }
                }

                labelNeedUpgradeRights.Text = loggedInNeedAdmin;
            }
            // Login using 3.0 schema
            else
            {
                // If there are no admin users, it would be impossible to go on
                if (!UserUtility.HasAdminUsers())
                {
                    e.Skip = true;
                    return;
                }

                string username;
                string password;

                // See if we can try to login automatically
                if (UserSession.GetSavedUserCredentials(out username, out password))
                {
                    long userID = UserUtility.GetShipWorksUserID(username, password);

                    // Not a valid user
                    if (userID < 0)
                    {
                        labelNeedUpgradeRights.Text = loggedInNeedRights;
                        return;
                    }

                    log.DebugFormat("3.x credentials found, UserID {0}", userID);

                    // Determine if the given user has rights to upgrade shipworks
                    if (SecurityContext.HasPermission(userID, PermissionType.DatabaseSetup))
                    {
                        // If they also have the rights to backup, just move on
                        if (SecurityContext.HasPermission(userID, PermissionType.DatabaseBackup))
                        {
                            userID3x = userID;
                            userName3x = username;

                            logonAfterUsername = username;
                            logonAfterPassword = password;

                            e.Skip = true;
                            return;
                        }
                        // If they can't backup, they have to have the opportunity to login as someone who can
                        else
                        {
                            labelNeedUpgradeRights.Text = loggedInNeedRights;
                            return;
                        }
                    }
                    else
                    {
                        labelNeedUpgradeRights.Text = string.Format(loggedInNoRights, username);
                        return;
                    }
                }

                labelNeedUpgradeRights.Text = loggedInNeedRights;
            }
        }

        /// <summary>
        /// Clicking the forgot username link
        /// </summary>
        private void OnForgotUsername(object sender, EventArgs e)
        {
            using (ForgotUsernameDlg dlg = new ForgotUsernameDlg())
            {
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// Clicking the forgot password link
        /// </summary>
        private void OnForgotPassword(object sender, EventArgs e)
        {
            using (ForgotPasswordDlg dlg = new ForgotPasswordDlg())
            {
                dlg.ShowDialog();
            }
        }

        /// <summary>
        /// Stepping next on the login page, need to try to login.
        /// </summary>
        private void OnStepNextLogin(object sender, WizardStepEventArgs e)
        {
            username.Text = username.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            // Verify a username
            if (username.Text.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter a username.");
                e.NextPage = CurrentPage;
                return;
            }

            // See if we need to login 2.x style
            if (installed < new Version(3, 0))
            {
                if (!UserUtility.IsShipWorks2xAdmin(username.Text, password.Text))
                {
                    MessageHelper.ShowMessage(this, "Incorrect username or password.");
                    e.NextPage = CurrentPage;
                    return;
                }
                else
                {
                    logonAfterUsername = username.Text;
                    logonAfterPassword = password.Text;
                }
            }
            // Login using 3.0 schema
            else
            {
                long userID = UserUtility.GetShipWorksUserID(username.Text, password.Text);

                // Not a valid user
                if (userID < 0)
                {
                    MessageHelper.ShowMessage(this, "Incorrect username or password.");
                    e.NextPage = CurrentPage;
                    return;
                }

                // Determine if the given user has rights to upgrade shipworks
                if (!SecurityContext.HasPermission(userID, PermissionType.DatabaseSetup))
                {
                    MessageHelper.ShowMessage(this, "The user does not have permission to update the database.");
                    e.NextPage = CurrentPage;
                    return;
                }

                userID3x = userID;
                userName3x = username.Text;

                logonAfterUsername = username.Text;
                logonAfterPassword = password.Text;
            }
        }

        #endregion

        #region Backup

        /// <summary>
        /// Stepping into the backup page
        /// </summary>
        private void OnSteppingIntoBackup(object sender, WizardSteppingIntoEventArgs e)
        {
            if (SqlServerInstaller.IsMsdeMigrationInProgress)
            {
                e.Skip = true;
                return;
            }

            // If a migration is already in progress we don't want to allow them to backup a partially updatd database
            if ((v2migrationController != null && v2migrationController.MigrationState != MigrationState.NotStarted) ||
                (v2migrationController == null && MigrationController.IsMigrationInProgress()))
            {
                e.Skip = true;
                return;
            }

            const string backupText = "User '{0}' does not have permissions to create a backup.";

            // Make sure we are on the right machine
            if (!SqlSession.Current.IsLocalServer())
            {
                labelBackupNoPermission.Text = "A ShipWorks backup can only be made from the computer running SQL Server.";
                labelBackupNoPermission.Visible = true;
                backup.Enabled = false;
            }
            // Make sure user is allowed to backup
            else if (userID3x > 0 && !SecurityContext.HasPermission(userID3x, PermissionType.DatabaseBackup))
            {
                labelBackupNoPermission.Text = string.Format(backupText, userName3x);
                labelBackupNoPermission.Visible = true;
                backup.Enabled = false;
            }
            // Skip if running an internal version - internal debug versions are 0.0.0.0
            else if (installed < new Version(3, 0) && !InterapptiveOnly.MagicKeysDown && Assembly.GetExecutingAssembly().GetName().Version.Major == 3)
            {
                labelBackupInfo.Text = "You must create a backup before updating your ShipWorks 2 database.  We don't expect anything to go wrong, but it doesn't hurt to be safe.";
                labelBackupNoPermission.Visible = false;

                NextEnabled = backupCompleted;
            }
            else
            {
                labelBackupNoPermission.Visible = false;
                backup.Enabled = true;
            }
        }

        /// <summary>
        /// Open the backup window
        /// </summary>
        private void OnBackup(object sender, EventArgs e)
        {
            using (DatabaseBackupDlg dlg = new DatabaseBackupDlg(userID3x))
            {
                dlg.ShowDialog(this);

                if (dlg.BackupCompleted)
                {
                    MarkBackupCompleted();

                    // We require a backup for sw2 upgrades - so enable it if the backup completed
                    if (installed < new Version(3, 0))
                    {
                        NextEnabled = true;
                    }
                }
            }
        }

        /// <summary>
        /// Mark the backup flag and UI as completed
        /// </summary>
        private void MarkBackupCompleted()
        {
            backupCompleted = true;

            pictureBackupComplete.Visible = true;
            labelBackupComplete.Visible = true;
        }

        #endregion

        #region Download SQL Server

        /// <summary>
        /// Stepping into the download page for SQL Server
        /// </summary>
        private void OnSteppingIntoDownloadSqlServer(object sender, WizardSteppingIntoEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // If the installer is already available, or we don't need to download, just skip over the download.
            if (sqlInstaller.IsInstallerLocalFileValid(SqlServerInstallerPurpose.Upgrade) || (!SqlServerInstaller.IsMsdeMigrationInProgress && SqlSession.Current.IsSqlServer2008OrLater()))
            {
                e.Skip = true;
                return;
            }

            downloadSqlServer.Enabled = true;
            progressSqlServer.Value = 0;
            bytesSqlServer.Text = "";

            // Cant click next, have to download
            NextEnabled = false;
        }

        /// <summary>
        /// Download the sql server install from the interapptive server
        /// </summary>
        private void OnDownloadSqlServer(object sender, EventArgs e)
        {
            sqlDownloader.Download(downloadSqlServer, progressSqlServer, bytesSqlServer);
        }

        /// <summary>
        /// Cancel the current download
        /// </summary>
        private void OnCancelDownload(object sender, CancelEventArgs e)
        {
            sqlDownloader.OnCancelDownload(sender, e);
        }

        #endregion

        #region Upgrade SQL Server

        /// <summary>
        /// Stepping into the upgrade SQL Server page
        /// </summary>
        private void OnSteppingIntoUpgradeSqlServer(object sender, WizardSteppingIntoEventArgs e)
        {
            // If its already upgraded, move on
            if (!SqlServerInstaller.IsMsdeMigrationInProgress && SqlSession.Current.IsSqlServer2008OrLater())
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }
        }

        /// <summary>
        /// Upgrade SQL Server
        /// </summary>
        private void OnStepNextUpgradeSqlServer(object sender, WizardStepEventArgs e)
        {
            // If it was installed, but needs a reboot, move to the last page (which will now be the reboot page)
            if (sqlInstaller.LastExitCode == SqlServerInstaller.ExitCodeSuccessRebootRequired)
            {
                e.NextPage = Pages[Pages.Count - 1];
                return;
            }

            // If its upgraded now, we are ok to move on.
            if (!SqlServerInstaller.IsMsdeMigrationInProgress && SqlSession.Current.IsSqlServer2008OrLater())
            {
                e.NextPage = wizardPageWindowsFirewall;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the upgrading message up and dislabe and the browsing buttons
            panelUpgradingSqlServer.BringToFront();
            NextEnabled = false;
            BackEnabled = false;

            // Stay on this page
            e.NextPage = CurrentPage;

            try
            {
                sqlInstaller.UpgradeSqlServer();
            }
            catch (Win32Exception ex)
            {
                MessageHelper.ShowError(this, "An error occurred while upgrading SQL Server:\n\n" + ex.Message);

                // Reset the gui
                panelUpgradeSqlServer.BringToFront();
                NextEnabled = true;
                BackEnabled = true;
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this, "An error occurred while upgrading SQL Server:\n\n" + ex.Message);

                // Reset the gui
                panelUpgradeSqlServer.BringToFront();
                NextEnabled = true;
                BackEnabled = true;
            }
        }

        /// <summary>
        /// The Sql Server upgrade has completed.
        /// </summary>
        private void OnUpgraderSqlServerExited(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnUpgraderSqlServerExited), new object[] { sender, e });
                return;
            }

            log.InfoFormat("SQL Server upgrade exited {0}", sqlInstaller.LastExitCode);

            // If it was successful, we should now be able to connect.
            if (sqlInstaller.LastExitCode == 0 && SqlSession.Current.IsSqlServer2008OrLater())
            {
                // Since we installed it, we can do this without asking
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    SqlUtility.SetSql2008CompatibilityLevel(con);
                    SqlUtility.EnableClr(con);
                }

                MoveNext();
            }
            else if (sqlInstaller.LastExitCode == SqlServerInstaller.ExitCodeSuccessRebootRequired)
            {
                Pages.Add(new RebootRequiredPage(
                    "SQL Server",
                    StartupAction.ContinueDatabaseUpgrade,
                    () => { return new XElement("AfterInstallSuccess"); }));

                MoveNext();
            }
            else
            {
                MessageHelper.ShowError(this,
                    "SQL Server was not upgraded.\n\n" + SqlServerInstaller.FormatReturnCode(sqlInstaller.LastExitCode));

                // Reset the gui
                panelUpgradeSqlServer.BringToFront();
                NextEnabled = true;
                BackEnabled = true;
            }
        }

        /// <summary>
        /// Cancel the upgrade of sql server
        /// </summary>
        private void OnCancellUpgradeSqlServer(object sender, CancelEventArgs e)
        {
            if (!NextEnabled)
            {
                MessageHelper.ShowMessage(this, "Please wait for the upgrade of SQL Server to complete.");
                e.Cancel = true;
            }
        }

        #endregion

        #region Windows Firewall

        /// <summary>
        /// Stepping into the windows firewall page
        /// </summary>
        private void OnSteppingIntoWindowsFirewall(object sender, WizardSteppingIntoEventArgs e)
        {
            // If we didnt upgrade the db, dont show the firewall page
            if (!showFirewallPage)
            {
                e.Skip = true;
                return;
            }

            e.Skip = !MyComputer.HasWindowsFirewall || firewallOpened;

            if (!e.Skip)
            {
                // These may have been set the other way by updating windows firewall from another sql instance install.  Not likely
                // to do to sql installs in one wizard, but possible.
                updateWindowsFirewall.Enabled = true;
                firewallUpdatedPicture.Visible = false;
                firewallUpdatedLabel.Visible = false;
            }
        }

        /// <summary>
        /// Update the windows firewall to work with ShipWorks
        /// </summary>
        private void OnUpdateWindowsFirewall(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SqlWindowsFirewallUtility.OpenWindowsFirewall();

                firewallOpened = true;

                updateWindowsFirewall.Enabled = false;
                firewallUpdatedPicture.Visible = true;
                firewallUpdatedLabel.Visible = true;

                UseWaitCursor = false;
            }
            catch (WindowsFirewallException ex)
            {
                UseWaitCursor = false;
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        #endregion

        #region Upgrade Database

        /// <summary>
        /// Stepping into the upgrade db page
        /// </summary>
        private void OnSteppingIntoUpgrade(object sender, WizardSteppingIntoEventArgs e)
        {
            if (SqlSchemaUpdater.IsCorrectSchemaVersion() && Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.PostMigrationPreparation))
            {
                e.Skip = true;
            }
            else
            {
                panelUpgradingDatabase.Visible = false;
            }
        }

        /// <summary>
        /// Begin the db upgrade process
        /// </summary>
        private void OnStepNextUpgrade(object sender, WizardStepEventArgs e)
        {
            if (SqlSchemaUpdater.IsCorrectSchemaVersion() && Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.PostMigrationPreparation))
            {
                // Let it flow to whatever the next page is.  May be the Admin User page, or may be a PostV2 migration page.
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the upgrading message up and dislabe and the browsing buttons
            panelUpgradingDatabase.Visible = true;
            panelUpgradingDatabase.BringToFront();
            NextEnabled = false;
            BackEnabled = false;
            CanCancel = false;

            // Stay on this page
            e.NextPage = CurrentPage;

            // Create the progress provider and window
            ProgressProvider progressProvider = new ProgressProvider();
            ProgressDlg progressDlg = new ProgressDlg(progressProvider);
            progressDlg.Title = "Update Database";
            progressDlg.Description = "ShipWorks is updating your database.";
            progressDlg.Show(this);

            // Used for async invoke
            MethodInvoker<ProgressProvider> invoker = new MethodInvoker<ProgressProvider>(AsyncUpdateDatabase);

            // Pass along user state
            Dictionary<string, object> userState = new Dictionary<string, object>();
            userState["invoker"] = invoker;
            userState["progressDlg"] = progressDlg;

            // Kick off the async upgrade process
            invoker.BeginInvoke(progressDlg.ProgressProvider, new AsyncCallback(OnAsyncUpdateComplete), userState);
        }

        /// <summary>
        /// Method meant to be called from an asycn invoker to update the database in the background
        /// </summary>
        private void AsyncUpdateDatabase(ProgressProvider progressProvider)
        {
            // First see if we need to get migrated from 2x -> 3x
            if (v2migrationController != null &&
                (v2migrationController.MigrationState == MigrationState.NotStarted ||
                 v2migrationController.MigrationState == MigrationState.ResumeRequired))
            {
                v2migrationController.Initialize();

                bool completed = v2migrationController.Execute(progressProvider);

                if (!completed)
                {
                    throw new OperationCanceledException();
                }
            }

            // Update to the latest v3 schema
            SqlSchemaUpdater.UpdateDatabase(progressProvider, noSingleUserMode.Checked);

            // After a 2x migration, there are a few steps that need to be performed once the v3 schema is totally current
            if (!Post2xMigrationUtility.IsStepComplete(Post2xMigrationStep.PostMigrationPreparation))
            {
                Post2xMigrationPreparation.PrepareForFinalStepsAfter3xSchemaUpdate(progressProvider);
            }
        }

        /// <summary>
        /// Update to the latest ShipWorks3x schema is complete
        /// </summary>
        private void OnAsyncUpdateComplete(IAsyncResult result)
        {
            if (InvokeRequired)
            {
                Invoke(new AsyncCallback(OnAsyncUpdateComplete), result);
                return;
            }

            Dictionary<string, object> userState = (Dictionary<string, object>) result.AsyncState;
            MethodInvoker<ProgressProvider> invoker = (MethodInvoker<ProgressProvider>) userState["invoker"];
            ProgressDlg progressDlg = (ProgressDlg) userState["progressDlg"];

            try
            {
                invoker.EndInvoke(result);

                if (progressDlg.Visible)
                {
                    // If the dialog is visible, wait until it closes before moving on
                    progressDlg.FormClosed += (object sender, FormClosedEventArgs e) => MoveNext();
                }
                else
                {
                    // Since the dialog is not visible, just move on now
                    log.Debug("Progress dialog was already closed.");
                    MoveNext();
                }
            }
            catch (OperationCanceledException)
            {
                progressDlg.ProgressProvider.Cancel();
                progressDlg.CloseForced();

                panelUpgradingDatabase.Visible = false;
                NextEnabled = true;
                BackEnabled = true;
                CanCancel = true;
            }
            catch (Exception ex)
            {
                if (ex is SqlScriptException || ex is SqlException || ex is MigrationException)
                {
                    log.ErrorFormat("An error occurred during upgrade.", ex);
                    progressDlg.ProgressProvider.Terminate(ex);

                    MessageHelper.ShowError(progressDlg, string.Format("An error occurred: {0}", ex.Message));
                    progressDlg.CloseForced();

                    panelUpgradingDatabase.Visible = false;
                    NextEnabled = true;
                    BackEnabled = true;
                    CanCancel = true;
                }
                else
                {
                    throw;
                }
            }         
        }

        /// <summary>
        /// Insert the WizardPages required for performing post v2 migration steps, after the 3x schema is completely up to date with whatever
        ///  the current schema is.
        /// </summary>
        private void InsertV2PostMigrationPages()
        {
            List<WizardPage> postPages = new List<WizardPage>
                {
                    new ImportTemplatesWizardPage(),
                    new ImportCertificatesWizardPage(),
                    new NetworkSolutionsLicenseWizardPage(),
                    new YahooEmailWizardPage(),
                    new AmazonWizardPage(),
                    new ChannelAdvisorAccountKeyWizardPage(),
                    new FedExWizardPage(),
                    new UpsShipperWizardPage(),
                    new WorldShipWizardPage(),
                    new UspsWizardPage(),
                    new EndiciaWizardPage(),
                    new Express1WizardPage(),
                    new OtherShipperWizardPage(),
                    new UserSecurityInfoWizardPage(),
                    new FinalTasksWizardPage()
                };

            // Add the pages in order right after the "Create Admin User" page
            foreach (var page in postPages.Reverse<WizardPage>())
            {
                Pages.Insert(Pages.IndexOf(wizardPageShipWorksAdmin) + 1, page);
            }
        }

        #endregion

        #region ShipWorks Administrator

        /// <summary>
        /// Stepping into the page to create a ShipWorks admin user
        /// </summary>
        private void OnSteppingIntoShipWorksAdmin(object sender, WizardSteppingIntoEventArgs e)
        {
            try
            {
                // If any admin users already exist we can skip this.
                e.Skip = UserUtility.HasAdminUsers();
                e.RaiseStepEventWhenSkipping = true;
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowMessage(this, ex.Message);
            }
        }

        /// <summary>
        /// Time to create the ShipWorks administrator
        /// </summary>
        private void OnStepNextShipWorksAdmin(object sender, WizardStepEventArgs e)
        {
            // By the time we get here we are all updated to the 3x schema, and are ready to ensure the session is initialized.
            UserSession.InitializeForCurrentDatabase();

            // If skipping then that means we can used the user they already picked \ were logged in as
            if (e.Skipping)
            {
                string rememberedUsername;
                string rememberedPassword;

                bool remember = UserSession.GetSavedUserCredentials(out rememberedUsername, out rememberedPassword) && rememberedUsername == logonAfterUsername;

                UserSession.Logon(logonAfterUsername, logonAfterPassword, remember);
                userID3x = UserSession.User.UserID;
            }
            // If not skipping then create the new admin user
            else
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
                    UserUtility.CreateUser(username, swEmail.Text, swPassword.Text, true);

                    // Go ahead and log them on
                    UserSession.Logon(username, swPassword.Text, swAutomaticLogon.Checked);

                    // This is now the user we are using
                    userID3x = UserSession.User.UserID;
                    userName3x = username;

                    // Now we can move on
                    e.NextPage = nextPage;
                }
                catch (SqlException ex)
                {
                    MessageHelper.ShowMessage(this, ex.Message);
                    return;
                }
                catch (DuplicateNameException ex)
                {
                    MessageHelper.ShowMessage(this, ex.Message);
                    return;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            // Either way before leaving this page we need the environment setup
            UserSession.InitializeForCurrentSession();
        }

        #endregion

        #region Complete

        /// <summary>
        /// Stepping into the complete page
        /// </summary>
        private void OnSteppingIntoComplete(object sender, WizardSteppingIntoEventArgs e)
        {
            AuditUtility.Audit(AuditActionType.UpgradeDatabase, userID3x);
        }

        #endregion
    }
}
