using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using Interapptive.Shared;
using System.Text.RegularExpressions;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using Interapptive.Shared.Net;
using System.Net;
using log4net;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using ShipWorks.Users;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI;
using ShipWorks.ApplicationCore.MessageBoxes;
using Interapptive.Shared.Utility;
using ShipWorks.Common.Threading;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Templates.Emailing;
using ShipWorks.Email;
using Microsoft.Win32;
using System.Xml.Linq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Administration.SqlServerSetup;
using ShipWorks.Users.Security;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using ShipWorks.Data.Administration.UpdateFrom2x;
using ShipWorks.Data.Administration.UpdateFrom2x.Configuration;
using Interapptive.Shared.Win32;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Wizard that gets the database configured.
    /// </summary>
    public partial class DatabaseSetupWizard : WizardForm
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DatabaseSetupWizard));

        // The current state of the SQL Session data
        SqlSession sqlSession = new SqlSession();

        // Navigation helpers
        WizardPage pageAfterSqlLogin;
        WindowsInstallerDownloadPage pageFirstPrerequisite;

        // The sql installer and file downloader
        SqlServerInstaller sqlInstaller;
        WizardDownloadHelper sqlDownloader;

        // List of instances installed during the lifetime of the wizard
        List<string> installedInstances = new List<string>();

        // List of instances having their firewall opened during lifetime of wizard
        List<string> firewallOpenedInstances = new List<string>();

        // The SQL Instance that we have loaded data file location for
        string dataFileSqlInstance = null;

        // True if we have created a database that may need to be dropped
        // if the users cancels or goes back.
        bool pendingDatabaseCreated = false;
        string pendingDatabaseName = "";

        // Determines if the admin user has been created during this session.
        bool adminUserCreated = false;

        // Determines if the db has been succesfully restored 
        bool databaseRestored = false;

        // The user that will be used for restoring the db
        long userForRestore;

        /// <summary>
        /// Constructor.
        /// </summary>
        public DatabaseSetupWizard()
        {
            InitializeComponent();

            sqlInstaller = new SqlServerInstaller();
            sqlInstaller.InitializeForCurrentSqlSession();
            sqlInstaller.Exited += new EventHandler(OnInstallerSqlServerExited);

            sqlDownloader = new WizardDownloadHelper(this, 
                sqlInstaller.GetInstallerLocalFilePath(SqlServerInstallerPurpose.Install), 
                sqlInstaller.GetInstallerDownloadUri(SqlServerInstallerPurpose.Install));

            // Prepopulate the instance box with the current
            if (SqlSession.IsConfigured)
            {
                comboSqlServers.Text = SqlSession.Current.Configuration.ServerInstance;
            }

            StartSearchingSqlServers();

            // Remove the placeholder...
            int placeholderIndex = Pages.IndexOf(wizardPagePrerequisitePlaceholder);
            Pages.Remove(wizardPagePrerequisitePlaceholder);

            // Add the pages to detect and install .NET 3.5 SP1 if necessary
            DotNet35DownloadPage dotNet35Page = new DotNet35DownloadPage(
                wizardPageInstallSqlServer,
                StartupAction.OpenDatabaseSetup,
                () =>
                    {
                        return new XElement("Replay",
                            new XElement("InstanceName", instanceName.Text),
                            new XElement("Restore", radioRestoreDatabase.Checked));
                    });
            Pages.Insert(placeholderIndex, dotNet35Page);

            // Add the pages to detect and install windows installer if necessary
            WindowsInstallerDownloadPage windowsInstallerPage = new WindowsInstallerDownloadPage(
                dotNet35Page,
                StartupAction.OpenDatabaseSetup,
                () =>
                    {
                        return new XElement("Replay",
                            new XElement("InstanceName", instanceName.Text),
                            new XElement("Restore", radioRestoreDatabase.Checked));
                    });
            Pages.Insert(placeholderIndex, windowsInstallerPage);

            // The first prereq is Windows installer 4.5 which then chains to .net 3.5
            pageFirstPrerequisite = windowsInstallerPage;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            if (StartupController.StartupAction == StartupAction.OpenDatabaseSetup)
            {
                if (StartupController.StartupArgument.Name == "Upgrade2x")
                {
                    var setupControls = new List<Control>
                                {
                                    radioSetupNewDatabase, pictureBoxSetupNewDatabase, labelSetupNewDatabase,
                                    radioConnectRunningDatabase, pictureBoxConnectRunningDatabase, labelConnectRunningDatabase
                                };

                    var restoreControls = new List<Control>
                                {
                                    radioRestoreDatabase, pictureBoxRestoreDatabase, labelRestoreDatabase
                                };

                    int setupOffset = radioConnectRunningDatabase.Top - radioSetupNewDatabase.Top;
                    int restoreOffset = radioSetupNewDatabase.Top - radioRestoreDatabase.Top;

                    foreach (var control in setupControls)
                    {
                        control.Top += setupOffset;
                    }

                    foreach (var control in restoreControls)
                    {
                        control.Top += restoreOffset;
                    }

                    radioRestoreDatabase.Checked = true;
                    radioRestoreDatabase.Text = "Upgrade from a ShipWorks 2 Backup";
                    labelRestoreDatabase.Text = "Select this option to import ShipWorks 2 data without affecting ShipWorks 2.";

                    radioSetupNewDatabase.ForeColor = SystemColors.GrayText;
                    radioConnectRunningDatabase.ForeColor = SystemColors.GrayText;

                    instanceName.Text = "SHIPWORKS3";
                }
                else
                {
                    bool wasDoingRestore = (bool) StartupController.StartupArgument.Element("Restore");

                    if (wasDoingRestore)
                    {
                        radioRestoreDatabase.Checked = true;
                        MoveNext();

                        radioRestoreIntoNewDatabase.Checked = true;
                        MoveNext();
                    }
                    else
                    {
                        radioSetupNewDatabase.Checked = true;
                        MoveNext();
                    }

                    radioInstallSqlServer.Checked = true;
                    instanceName.Text = (string) StartupController.StartupArgument.Element("InstanceName");

                    // If we are here after rebooting from a successful install that needed a reboot, fastforward past the install page
                    var afterInstallSuccess = StartupController.StartupArgument.Element("AfterInstallSuccess");
                    if (afterInstallSuccess != null && (bool) afterInstallSuccess)
                    {
                        log.InfoFormat("Replaying SQL Install Success after reboot.");

                        installedInstances.Add(instanceName.Text);

                        // Reload all the SQL Session from last time
                        sqlSession.Configuration.ServerInstance = Environment.MachineName + "\\" + instanceName.Text;
                        sqlSession.Configuration.Username = "sa";
                        sqlSession.Configuration.Password = SecureText.Decrypt((string) afterInstallSuccess.Attribute("password"), "sa");
                        sqlSession.Configuration.WindowsAuth = false;

                        // Since we installed it, we can do this without asking.  We didn't do it right after install completed because
                        // a reboot was required (which is why we are here now)
                        using (SqlConnection con = sqlSession.OpenConnection())
                        {
                            SqlUtility.EnableClr(con);
                        }

                        MoveNext();
                    }
                }

                StartupController.ClearStartupAction();
            }

            if (SqlServerInstaller.IsSqlServer2012Supported)
            {
                panelSetupLegacy.Visible = false;
                radioSetupNewDatabase.Checked = false;
            }
            else
            {
                panelSetup2012.Visible = false;
                radioConnectToAnotherPC.Checked = false;
            }

            panelSetupLegacy.Location = panelSetup2012.Location;
        }

        #region Setup or Connect

        /// <summary>
        /// Stepping next from the first page
        /// </summary>
        private void OnStepNextSetupOrConnect(object sender, WizardStepEventArgs e)
        {
            if (radioSetupNewDatabase.Checked || radioNewDatabase.Checked)
            {
                e.NextPage = wizardPageChooseSqlServer;
            }

            else if (radioConnectRunningDatabase.Checked || radioConnectToAnotherPC.Checked)
            {
                e.NextPage = wizardPageSelectSqlServerInstance;
                pageAfterSqlLogin = wizardPageChooseDatabase;
            }

            else if (radioRestoreDatabase.Checked || radioRestoreBackup.Checked)
            {
                e.NextPage = wizardPageRestoreOption;
            }
        }

        /// <summary>
        /// Click on the labels for Another PC
        /// </summary>
        private void OnClickAnotherPcLabels(object sender, EventArgs e)
        {
            radioConnectToAnotherPC.Checked = true;
        }

        #endregion

        #region Restore Option

        /// <summary>
        /// Stepping into the page to choose to restore to the current database or a new database
        /// </summary>
        private void OnSteppingIntoRestoreOption(object sender, WizardSteppingIntoEventArgs e)
        {
            bool canConnectCurrent = SqlSession.IsConfigured && SqlSession.Current.CanConnect() && SqlSession.Current.IsSqlServer2008OrLater();

            // Can only choose current if there is a current
            radioRestoreIntoCurrent.Enabled = canConnectCurrent;

            if (!canConnectCurrent)
            {
                // Force new option
                radioRestoreIntoNewDatabase.Checked = true;
            }
        }
        
        /// <summary>
        /// Steppign next from the store option page.
        /// </summary>
        private void OnStepNextRestoreOption(object sender, WizardStepEventArgs e)
        {
            if (radioRestoreIntoCurrent.Checked)
            {
                // We are going to restore into the current, so we will be using the current settings
                sqlSession.Configuration.CopyFrom(SqlSession.Current.Configuration);

                e.NextPage = wizardPageRestoreLogin;
            }
            else
            {
                e.NextPage = wizardPageChooseSqlServer;
            }
        }

        #endregion

        #region New Or Existing SQL Server

        /// <summary>
        /// Stepping into the new or existing SQL Server page
        /// </summary>
        private void OnSteppingIntoNewOrExistingSqlServer(object sender, WizardSteppingIntoEventArgs e)
        {
            if (!e.FirstTime)
            {
                return;
            }

            // If the SQL Session is configured, then the first thing we'll list is the option to use the current SQL Server
            if (SqlSession.IsConfigured)
            {
                radioSqlServerCurrent.Checked = true;

                labelSqlServerCurrentName.Text = string.Format("({0})", SqlSession.Current.IsLocalDb() ? "Local only" : SqlSession.Current.ServerInstance);
            }
            // Otherwise, current isn't an option, and installing a new one is the first option
            else
            {
                radioSqlServerRunning.Text = "Choose a running instance of Microsoft SQL Server";

                panelSqlInstanceCurrent.Visible = false;
                radioInstallSqlServer.Checked = true;

                panelSqlInstanceInstall.Location = panelSqlInstanceCurrent.Location;
                panelSqlInstanceRunning.Top = panelSqlInstanceInstall.Bottom;
            }
        }

        /// <summary>
        /// User has chosen if they want to select an existing sql server or install a new one.
        /// </summary>
        private void OnStepNextNewOrExistingSqlServer(object sender, WizardStepEventArgs e)
        {
            if (radioInstallSqlServer.Checked)
            {
                instanceName.Text = instanceName.Text.Trim();

                // Save the instance
                sqlSession.ServerInstance = Environment.MachineName + "\\" + instanceName.Text;
                sqlSession.Username = "sa";
                sqlSession.Password = SqlInstanceUtility.ShipWorksSaPassword;
                sqlSession.RememberPassword = true;
                sqlSession.WindowsAuth = false;

                // If we've already installed this instance during this showing of the wizard (which means the user has just stepped back),
                // we can just skip right to the install step, which will then automatically skip to the next appropriate step.
                if (installedInstances.Contains(instanceName.Text))
                {
                    e.NextPage = wizardPageInstallSqlServer;
                }
                else
                {
                    if (!ValidateInstanceName(instanceName.Text))
                    {
                        e.NextPage = CurrentPage;
                    }
                    else
                    {
                        sqlSession.ServerInstance = Environment.MachineName + "\\" + instanceName.Text;
                        e.NextPage = pageFirstPrerequisite;
                    }
                }
            }
            else
            {

                if (radioSqlServerCurrent.Checked)
                {
                    sqlSession.ServerInstance = SqlSession.Current.ServerInstance;
                    sqlSession.Username = SqlSession.Current.Username;
                    sqlSession.Password = SqlSession.Current.Password;
                    sqlSession.WindowsAuth = SqlSession.Current.WindowsAuth;

                    e.NextPage = wizardPageDatabaseName;
                }
                else
                {
                    pageAfterSqlLogin = wizardPageDatabaseName;

                    e.NextPage = wizardPageSelectSqlServerInstance;
                }
            }
        }

        /// <summary>
        /// Validate the SQL Server instance name.
        /// </summary>
        private bool ValidateInstanceName(string name)
        {
            string error = "";

            if (string.IsNullOrEmpty(name))
            {
                error = "The instance name cannot be blank.";
            }

            if (name.ToLower() == "default" || name.ToLower() == SqlInstanceUtility.DefaultInstanceName.ToLower())
            {
                error = string.Format("Instance names cannot be 'Default' or '{0}'.", SqlInstanceUtility.DefaultInstanceName);
            }

            if (!Regex.Match(name, "^[a-zA-Z][0-9a-zA-Z$#_]*$").Success)
            {
                error = "The instance name must begin with a letter and cannot contain spaces or special characters.";
            }

            if (SqlInstanceUtility.IsSqlInstanceInstalled(name))
            {
                error = "The instance name '" + name + "' already exists on this computer.";
            }

            if (error.Length > 0)
            {
                MessageHelper.ShowError(this, error);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Changing whether they are going to install or select SQL Server
        /// </summary>
        private void OnChangeInstallSqlServerOption(object sender, EventArgs e)
        {
            if (!((RadioButton) sender).Checked)
            {
                return;
            }

            List<RadioButton> radios = new List<RadioButton>
                {
                    radioSqlServerRunning,
                    radioSqlServerCurrent,
                    radioInstallSqlServer
                };

            foreach (var radio in radios)
            {
                if (radio != sender)
                {
                    radio.Checked = false;
                }
            }

            instanceName.Enabled = radioInstallSqlServer.Checked;
        }

        /// <summary>
        /// Clicking one of the labels next to the radio
        /// </summary>
        private void OnClickSqlCurrentInstanceLabel(object sender, EventArgs e)
        {
            radioSqlServerCurrent.Checked = true;
        }

        /// <summary>
        /// Clicking one of the labels next to the radio
        /// </summary>
        private void OnClickSqlRunningInstanceLabel(object sender, EventArgs e)
        {
            radioSqlServerRunning.Checked = true;
        }

        /// <summary>
        /// Clicking one of the labels next to the radio
        /// </summary>
        private void OnClickSqlNewInstanceLabel(object sender, EventArgs e)
        {
            radioInstallSqlServer.Checked = true;
        }

        #endregion

        #region Select SQL Instance

        /// <summary>
        /// Start looking for all the installed SQL Servers
        /// </summary>
        private void StartSearchingSqlServers()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(OnFoundSqlServerInstances);
            worker.DoWork += new DoWorkEventHandler(SearchSqlServers);
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Calculate the size of the log files
        /// </summary>
        void SearchSqlServers(object sender, DoWorkEventArgs e)
        {
            string[] servers = SqlInstanceUtility.GetRunningSqlServers();

            BackgroundWorker worker = (BackgroundWorker) sender;
            worker.ReportProgress(0, servers);
        }

        /// <summary>
        /// Progress being reported while calculating file size
        /// </summary>
        void OnFoundSqlServerInstances(object sender, ProgressChangedEventArgs e)
        {
            // If we have closed, stop counting
            if (TopLevelControl == null || !TopLevelControl.Visible)
            {
                ((BackgroundWorker) sender).CancelAsync();
                return;
            }

            pictureServerSearching.Visible = false;
            labelServerSearching.Visible = false;

            string[] servers = (string[]) e.UserState;

            // Load the list with all servers found on the LAN
            foreach (string server in servers)
            {
                comboSqlServers.Items.Add(server);
            }

            // Auto-select first one if nothing is in there already
            if (comboSqlServers.Text.Length == 0 && comboSqlServers.Items.Count > 0)
            {
                comboSqlServers.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Stepping next from selecting the sql instance to connect to.
        /// </summary>
        private void OnStepNextSelectSqlInstance(object sender, WizardStepEventArgs e)
        {
            if (comboSqlServers.Text.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please select or enter the name of a SQL Server instance.");

                e.NextPage = CurrentPage;
                return;
            }

            // Save the instance
            sqlSession.Configuration.ServerInstance = comboSqlServers.Text;

            e.NextPage = wizardPageLoginSqlServer;
        }

        #endregion

        #region SQL Server Login

        /// <summary>
        /// Stepping next on the credentials page.
        /// </summary>
        private void OnStepNextSqlLogin(object sender, WizardStepEventArgs e)
        {
            // Set the credentials
            if (sqlServerAuth.Checked)
            {
                sqlSession.Configuration.Username = username.Text;
                sqlSession.Configuration.Password = password.Text;
                sqlSession.Configuration.WindowsAuth = false;
            }
            else
            {
                sqlSession.Configuration.WindowsAuth = true;
            }

            // Ensure the database name is cleared
            sqlSession.Configuration.DatabaseName = "";

            // Try to login
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Attempts to make a connection to the database
                sqlSession.TestConnection();

                if (!sqlSession.IsSqlServer2008OrLater())
                {
                    // If trying to create a new database in an existing sql instance
                    if (radioRestoreDatabase.Checked || (radioSetupNewDatabase.Checked && radioSqlServerCurrent.Checked))
                    {
                        MessageHelper.ShowError(this,
                            "ShipWorks requires SQL Server.\n\n" +
                            "The SQL Server instance you have selected is previous version that " +
                            "is not compatible with ShipWorks.\n\n" +
                            "You can install the ShipWorks database in a new SQL Server instance " +
                            "by returning to the beginning of this wizard.");

                        e.NextPage = CurrentPage;
                        return;
                    }
                }

                if (!sqlSession.CheckPermissions(SqlSessionPermissionSet.Setup, this))
                {
                    e.NextPage = CurrentPage;
                    return;
                }
                
                // If its not 08, we'll be upgrading it (and the CLR status) later
                if (sqlSession.IsSqlServer2008OrLater() && !sqlSession.IsClrEnabled())
                {
                    log.Info("CLR is not enabled on the server.");

                    using (NeedEnableClr dlg = new NeedEnableClr(sqlSession))
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                        {
                            e.NextPage = CurrentPage;
                            return;
                        }
                    }
                }

                // Cant restore to a remote server
                if (radioRestoreDatabase.Checked && !sqlSession.IsLocalServer())
                {
                    MessageHelper.ShowInformation(this, "A ShipWorks restore can only be done from the computer that is running SQL Server.");
                    e.NextPage = CurrentPage;
                    return;
                }

                // Worked
                e.NextPage = pageAfterSqlLogin;
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this,
                    "ShipWorks could not login to SQL Server '" + sqlSession.Configuration.ServerInstance + "' using " +
                    "the given login information.\n\n" +
                    "Detail: " + ex.Message);

                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Changing the login method on the credentials page
        /// </summary>
        private void OnChangeLoginMethod(object sender, System.EventArgs e)
        {
            username.Enabled = sqlServerAuth.Checked;
            password.Enabled = sqlServerAuth.Checked;
        }

        #endregion

        #region Choose Existing Database

        /// <summary>
        /// The choose database name page is being shown.
        /// </summary>
        private void OnShownChooseDatabase(object sender, EventArgs e)
        {
            string previousSelection = (string) databaseNames.SelectedItem;
            if (previousSelection == null && SqlSession.IsConfigured)
            {
                previousSelection = SqlSession.Current.Configuration.DatabaseName;
            }

            databaseNames.Items.Clear();
            
            try
            {
                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    SqlCommand cmd = SqlCommandProvider.Create(con);
                    cmd.CommandText = "select name from master..sysdatabases where name not in ('master', 'model', 'msdb', 'tempdb')";

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            databaseNames.Items.Add((string) reader["name"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this,
                    "There was an error trying to read the database names " +
                    "from SQL Server instance '" + sqlSession.Configuration.ServerInstance + "'.\n\n" +
                    "Detail: " + ex.Message);
            }

            if (databaseNames.Items.Count == 0)
            {
                databaseNames.Enabled = false;

                NextEnabled = false;
                databaseNames.Items.Add("No databases found.");
            }
            else
            {
                databaseNames.Enabled = true;

                databaseNames.SelectedItem = previousSelection;
                if (databaseNames.SelectedIndex < 0)
                {
                    databaseNames.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// User is choosing the database to which to connect.
        /// </summary>
        private void OnStepNextChooseDatabase(object sender, WizardStepEventArgs e)
        {
            string database = databaseNames.Text;

            try
            {
                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    // Now we have to make sure we have access to the database they selected
                    con.ChangeDatabase(database);

                    // Now see if its a valid ShipWorks database
                    SqlCommand cmd = SqlCommandProvider.Create(con);
                    cmd.CommandText = "GetSchemaVersion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    Version version = new Version((string) SqlCommandProvider.ExecuteScalar(cmd));

                    // We get this far, we must be ok
                    sqlSession.Configuration.DatabaseName = database;

                    // Complete
                    e.NextPage = wizardPageShipWorksAdmin;
                }
            }
            catch (SqlException ex)
            {
                log.Info(string.Format("Failed calling GetSchemaVersion for database '{0}'.", database), ex);

                MessageHelper.ShowError(this,
                    "The selected database is not a database created by ShipWorks, or is not " +
                    "a database that you have access to.\n\n" +
                    "ShipWorks can only connect to a valid ShipWorks database.  If you need " +
                    "to create a new database, you can do so by returning to the beginning of " +
                    "this wizard.");

                e.NextPage = CurrentPage;
                return;
            }
        }

        #endregion

        #region Install SQL Server

        /// <summary>
        /// Stepping into the install SQL Server page
        /// </summary>
        private void OnSteppingIntoInstallSqlServer(object sender, WizardSteppingIntoEventArgs e)
        {
            // If its installed now, we are ok to move on.
            if (SqlInstanceUtility.IsSqlInstanceInstalled(instanceName.Text))
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }
        }

        /// <summary>
        /// Install SQL Server
        /// </summary>
        private void OnStepNextInstallSqlServer(object sender, WizardStepEventArgs e)
        {
            // If it was installed, but needs a reboot, move to the last page (which will now be the reboot page)
            if (sqlInstaller.LastExitCode == SqlServerInstaller.ExitCodeSuccessRebootRequired)
            {
                e.NextPage = Pages[Pages.Count - 1];
                return;
            }

            // If its installed now, we are ok to move on.
            if (SqlInstanceUtility.IsSqlInstanceInstalled(instanceName.Text))
            {
                e.NextPage = wizardPageDatabaseName;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the installing message up and diable and the browsing buttons
            panelSqlServerInstallProgress.Location = panelSqlServerInstallReady.Location;
            panelSqlServerInstallProgress.Visible = true;
            panelSqlServerInstallReady.Visible = false;
            NextEnabled = false;
            BackEnabled = false;

            // Stay on this page
            e.NextPage = CurrentPage;

            try
            {
                sqlInstaller.InstallSqlServer(instanceName.Text, SqlInstanceUtility.ShipWorksSaPassword);

                progressPreparing.Value = 0;
                progressTimer.Start();
                progressTimer.Tag = null;
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1602 || ex.NativeErrorCode == 1603 || ex.NativeErrorCode == 1223)
                {
                    MessageHelper.ShowInformation(this, "You must click 'Yes' when asked to allow ShipWorks to make changes to your computer.");
                }
                else
                {
                    MessageHelper.ShowError(this, "An error occurred while installing SQL Server:\n\n" + ex.Message);
                }

                // Reset the gui
                panelSqlServerInstallProgress.Visible = false;
                panelSqlServerInstallReady.Visible = true;
                NextEnabled = true;
                BackEnabled = true;
            }
        }

        /// <summary>
        /// Simple UI timer we use to keep the progress of SQL Server install aproximated
        /// </summary>
        void OnInstallSqlServerProgressTimer(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(sqlInstaller.GetInstallerLocalFilePath(SqlServerInstallerPurpose.Install));
            long expectedFileSize = sqlInstaller.GetInstallerFileLength(SqlServerInstallerPurpose.Install);

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
                Stopwatch elapsed = progressTimer.Tag as Stopwatch;
                if (elapsed == null)
                {
                    elapsed = Stopwatch.StartNew();
                    progressTimer.Tag = elapsed;
                }

                // Download counts as 33%.  After that, installing counts all the way up to 100, and we progress it assuming it will take 5 minutes
                progressPreparing.Value = 33 + (int) ((elapsed.Elapsed.TotalSeconds / 300.0) * 0.66 * 100);

                log.InfoFormat("Installing - updated progress to {0}", progressPreparing.Value);
            }
        }

        /// <summary>
        /// The Sql Server installation has completed.
        /// </summary>
        private void OnInstallerSqlServerExited(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnInstallerSqlServerExited), new object[] { sender, e });
                return;
            }

            progressTimer.Stop();

            // If it was successful, we should now be able to connect.
            if (sqlInstaller.LastExitCode == 0 && SqlInstanceUtility.IsSqlInstanceInstalled(instanceName.Text))
            {
                sqlSession.Configuration.Username = "sa";
                sqlSession.Configuration.Password = SqlInstanceUtility.ShipWorksSaPassword;
                sqlSession.Configuration.RememberPassword = true;
                sqlSession.Configuration.WindowsAuth = false;
                sqlSession.Configuration.ServerInstance = Environment.MachineName + "\\" + instanceName.Text;

                installedInstances.Add(instanceName.Text);

                // Since we installed it, we can do this without asking
                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    SqlUtility.EnableClr(con);
                }

                MoveNext();
            }
            else if (sqlInstaller.LastExitCode == SqlServerInstaller.ExitCodeSuccessRebootRequired)
            {
                Pages.Add(new RebootRequiredPage(
                    "SQL Server", 
                    StartupAction.OpenDatabaseSetup,
                    () =>
                        {
                            return new XElement("Replay",
                                new XElement("InstanceName", instanceName.Text),
                                new XElement("Restore", radioRestoreDatabase.Checked),
                                new XElement("AfterInstallSuccess", new XAttribute("password", SecureText.Encrypt(sqlSession.Configuration.Password, "sa")), true));
                        }));

                MoveNext();
            }
            else
            {
                MessageHelper.ShowError(this,
                    "SQL Server was not installed.\n\n" + SqlServerInstaller.FormatReturnCode(sqlInstaller.LastExitCode));

                NextEnabled = true;
                BackEnabled = true;
            }

            // Reset the gui
            panelSqlServerInstallProgress.Visible = false;
            panelSqlServerInstallReady.Visible = true;
        }

        /// <summary>
        /// Cancell the installation of sql server
        /// </summary>
        private void OnCancellInstallSqlServer(object sender, CancelEventArgs e)
        {
            if (!NextEnabled)
            {
                MessageHelper.ShowMessage(this, "Please wait for the installation of SQL Server to complete.");
                e.Cancel = true;
            }
        }

        #endregion

        #region Create Database

        /// <summary>
        /// Stepping into the page for creating a shipworks database
        /// </summary>
        private void OnSteppingIntoCreateDatabase(object sender, WizardSteppingIntoEventArgs e)
        {
            // This will be true if we are stepping back into this page
            if (pendingDatabaseCreated)
            {
                DropPendingDatabase();
            }

            // See if we need to load up where the data files will go
            if (dataFileSqlInstance != sqlSession.ServerInstance)
            {
                linkChooseDataLocation.Visible = true;
                panelDataFiles.Visible = false;

                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    pathDataFiles.Text = SqlUtility.GetMasterDataFilePath(con);
                }

                dataFileSqlInstance = sqlSession.ServerInstance;
            }
        }

        /// <summary>
        /// User clicked next on the create a new database page
        /// </summary>
        private void OnStepNextCreateDatabase(object sender, WizardStepEventArgs e)
        {
            try
            {
                string name = databaseName.Text.Trim();

                // Validate the name
                if (!Regex.Match(name, "^[a-zA-Z][0-9a-zA-Z@$#_]*$").Success)
                {
                    MessageHelper.ShowMessage(this,
                        "The database name is invalid.\n\n" +
                        "The name must begin with a letter and cannot contain spaces\n" +
                        "or special characters.");

                    e.NextPage = CurrentPage;
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    SqlDatabaseCreator.CreateDatabase(name, pathDataFiles.Text, con);
                }

                sqlSession.Configuration.DatabaseName = name;

                pendingDatabaseCreated = true;
                pendingDatabaseName = sqlSession.Configuration.DatabaseName;

                // Restoring
                if (radioRestoreDatabase.Checked)
                {
                    e.NextPage = wizardPageRestoreLogin;
                }
                // Creating new
                else
                {
                    try
                    {
                        using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
                        {
                            SqlDatabaseCreator.CreateSchemaAndData();
                        }
                    }
                    // If something goes wrong, drop the db we just created
                    catch
                    {
                        DropPendingDatabase();

                        throw;
                    }

                    e.NextPage = wizardPageShipWorksAdmin;
                }
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
            catch (SqlScriptException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Drop the database that we created due to the use going back or cancelling
        /// the wizard.
        /// </summary>
        private void DropPendingDatabase()
        {
            if (!pendingDatabaseCreated)
            {
                return;
            }

            try
            {
                SqlDatabaseCreator.DropDatabase(sqlSession, pendingDatabaseName);

                sqlSession.Configuration.DatabaseName = "";
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this, "There was an error rolling back the created database.\n\n" + ex.Message);
            }

            pendingDatabaseCreated = false;
            pendingDatabaseName = "";
        }

        /// <summary>
        /// Clicking the link to choose the data file location
        /// </summary>
        private void OnChooseDataFileLocation(object sender, EventArgs e)
        {
            linkChooseDataLocation.Visible = false;
            panelDataFiles.Visible = true;
        }

        /// <summary>
        /// Browse for the database file location
        /// </summary>
        private void OnBrowseDatabaseLocation(object sender, EventArgs e)
        {
            databaseLocationBrowserDialog.SelectedPath = pathDataFiles.Text;

            if (databaseLocationBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                pathDataFiles.Text = databaseLocationBrowserDialog.SelectedPath;
            }
        }

        #endregion

        #region Restore Login

        /// <summary>
        /// Stepping into the page to login to be able to restore a ShipWorks backup.
        /// </summary>
        private void OnSteppingIntoRestoreLogin(object sender, WizardSteppingIntoEventArgs e)
        {
            userForRestore = -1;

            // If we are restoring over a new database, we don't need a user
            if (radioRestoreIntoNewDatabase.Checked)
            {
                e.Skip = true;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
            {
                Version installed;

                try
                {
                    installed = SqlSchemaUpdater.GetInstalledSchemaVersion();
                }
                catch (InvalidShipWorksDatabaseException)
                {
                    MessageHelper.ShowError(this,
                        "The current database is not a ShipWorks database.\n\n" +
                        "ShipWorks will not restore a backup into a non ShipWorks database.");

                    e.Skip = true;
                    e.SkipToPage = CurrentPage;
                    return;
                }

                // Below db version 1.2.0 (ShipWorks 2.4), there were no users.  So to be logged
                // in at all is to be an admin.
                if (installed < new Version(1, 2))
                {
                    log.Debug("Pre 2.4 database, no need for admin logon.");

                    e.Skip = true;
                }
                // See if we need to login 2.x style
                else if (installed < new Version(3, 0))
                {
                    if (!UserUtility.Has2xAdminUsers())
                    {
                        e.Skip = true;
                    }
                    else
                    {
                        string username;
                        string password;

                        // See if we can try to login automatically
                        if (UserSession.GetSavedUserCredentials(out username, out password))
                        {
                            log.Debug("2.x credentials found, attempting admin login.");

                            if (UserUtility.IsShipWorks2xAdmin(username, password))
                            {
                                e.Skip = true;
                            }
                        }
                    }
                }
                // Login using 3.0 schema
                else
                {
                    if (!UserUtility.HasAdminUsers())
                    {
                        e.Skip = true;
                    }
                    else
                    {
                        string username;
                        string password;
                        long userID = -1;

                        // See if we can try to login automatically
                        if (UserSession.GetSavedUserCredentials(out username, out password))
                        {
                            userID = UserUtility.GetShipWorksUserID(username, password);
                        }
                        else if (UserSession.IsLoggedOn)
                        {
                            userID = UserSession.User.UserID;
                        }

                        if (userID < 0)
                        {
                            return;
                        }

                        log.DebugFormat("3.x credentials found, UserID {0}", userID);

                        // Determine if the given user has rights to restore shipworks
                        if (SecurityContext.HasPermission(userID, PermissionType.DatabaseRestore))
                        {
                            userForRestore = userID;

                            e.Skip = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clicking next to login
        /// </summary>
        private void OnStepNextRestoreLogin(object sender, WizardStepEventArgs e)
        {
            restoreUsername.Text = restoreUsername.Text.Trim();

            Cursor.Current = Cursors.WaitCursor;

            // Verify a username
            if (restoreUsername.Text.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please enter a username.");
                e.NextPage = CurrentPage;
                return;
            }

            using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
            {
                Version installed = SqlSchemaUpdater.GetInstalledSchemaVersion();

                // See if we need to login 2.x style
                if (installed < new Version(3, 0))
                {
                    if (!UserUtility.IsShipWorks2xAdmin(restoreUsername.Text, restorePassword.Text))
                    {
                        MessageHelper.ShowMessage(this, "Incorrect username or password.");
                        e.NextPage = CurrentPage;
                        return;
                    }
                }
                // Login using 3.0 schema
                else
                {
                    long userID = UserUtility.GetShipWorksUserID(restoreUsername.Text, restorePassword.Text);

                    // Not a valid user
                    if (userID < 0)
                    {
                        MessageHelper.ShowMessage(this, "Incorrect username or password.");
                        e.NextPage = CurrentPage;
                        return;
                    }

                    // Determine if the given user has rights to upgrade shipworks
                    if (!SecurityContext.HasPermission(userID, PermissionType.DatabaseRestore))
                    {
                        MessageHelper.ShowMessage(this, "The user does not have permission to restore the database.");
                        e.NextPage = CurrentPage;
                        return;
                    }

                    userForRestore = userID;
                }
            }
        }

        #endregion

        #region Restore Backup

        /// <summary>
        /// The page to restore backup has been shown
        /// </summary>
        private void OnPageShownRestoreBackup(object sender, EventArgs e)
        {
            bool canBackup = sqlSession.IsLocalServer();

            labelCantRestore.Visible = !canBackup;

            groupInfo.Visible = canBackup;
            NextEnabled = canBackup;
            browseForBackupFile.Enabled = canBackup;
            backupFile.Enabled = canBackup;
        }

        /// <summary>
        /// Browse for the backup file to restore from
        /// </summary>
        private void OnBrowseBackupFile(object sender, EventArgs e)
        {
            if (openBackupFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                backupFile.Text = openBackupFileDialog.FileName;
            }
        }

        /// <summary>
        /// Stepping next on the restore backup page
        /// </summary>
        private void OnStepNextRestoreBackup(object sender, WizardStepEventArgs e)
        {
            if (databaseRestored)
            {
                e.NextPage = wizardPageComplete;
                return;
            }
            else
            {
                // Stay on this page
                e.NextPage = CurrentPage;

                if (backupFile.Text.Length == 0)
                {
                    MessageHelper.ShowMessage(this, "Please select a ShipWorks backup file.");
                    return;
                }

                ProgressDlg progressDlg = new ProgressDlg(new ProgressProvider());
                progressDlg.Title = "Restore ShipWorks";
                progressDlg.Description = "ShipWorks is restoring the backup of your database.";
                progressDlg.Show(this);

                // Create the backup object
                ShipWorksBackup backup = new ShipWorksBackup(sqlSession, userForRestore, progressDlg.ProgressProvider);

                // Create the delegate to use to get it on another thread
                MethodInvoker<string> invoker = new MethodInvoker<string>(backup.RestoreBackup);

                // Initiate the restore
                invoker.BeginInvoke(backupFile.Text, new AsyncCallback(OnRestoreComplete), new object[] { invoker, progressDlg });
            }
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

                // Close when the progress closes if it wasn't cancelled
                if (!progressDlg.ProgressProvider.CancelRequested)
                {
                    databaseRestored = true;
                    progressDlg.FormClosed += new FormClosedEventHandler(OnMoveNextAfterRestore);
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
                log.Error("Error during restore", ex);

                progressDlg.ProgressProvider.Terminate(ex);

                MessageHelper.ShowError(progressDlg, ex.Message);
                progressDlg.Close();
            }
        }

        /// <summary>
        /// Called when the progress window is closed after a successful restore.
        /// </summary>
        void OnMoveNextAfterRestore(object sender, FormClosedEventArgs e)
        {
            MoveNext();
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
                using (SqlSessionScope scope = new SqlSessionScope(sqlSession))
                {
                    // If its not the correct db version, then the upgrade wizard will take care of
                    // ensuring admin user.
                    //
                    // If any admin users already exist we can skip this.
                    //
                    e.Skip = !SqlSchemaUpdater.IsCorrectSchemaVersion() || UserUtility.HasAdminUsers();
                }
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
                    adminUserCreated = true;
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
        }

        #endregion

        #region Complete

        /// <summary>
        /// The "complete" page is being shown.
        /// </summary>
        private void OnShownComplete(object sender, EventArgs e)
        {
            // This is so we dont delete the pending db in the OnClose
            pendingDatabaseCreated = false;
            pendingDatabaseName = "";

            sqlSession.SaveAsCurrent();

            // We now have a new session
            if (SqlSchemaUpdater.IsCorrectSchemaVersion())
            {
                UserSession.InitializeForCurrentDatabase();
            }

            // If we created the admin user, go ahead and log that user in
            if (adminUserCreated)
            {
                UserSession.Logon(swUsername.Text.Trim(), swPassword.Text, true);
            }
        }

        /// <summary>
        /// Cleanup when closed.
        /// </summary>
        private void OnClosed(object sender, FormClosedEventArgs e)
        {
            if (pendingDatabaseCreated)
            {
                DropPendingDatabase();
            }
        }

        #endregion

    }
}

