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
using System.Linq;
using System.Xml.Linq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Administration.SqlServerSetup;
using ShipWorks.Users.Security;
using Interapptive.Shared.Data;
using Interapptive.Shared.UI;
using ShipWorks.Data.Administration.UpdateFrom2x;
using ShipWorks.Data.Administration.UpdateFrom2x.Configuration;
using Interapptive.Shared.Win32;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Properties;
using Divelements.SandGrid;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Controls;
using ShipWorks.ApplicationCore.Setup;
using ShipWorks.Stores.Management;
using ShipWorks.Users.Logon;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Wizard that gets the database configured.
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class DetailedDatabaseSetupWizard : WizardForm
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DetailedDatabaseSetupWizard));

        // The current state of the SQL Session data
        SqlSession sqlSession = new SqlSession();

        // The session and SQL instance that we are currently testing in the background for succesfully connect
        SqlSession connectionSession = null;

        // Navigation helpers
        WindowsInstallerDownloadPage pageFirstPrerequisite;

        // The sql installers
        SqlServerInstaller sqlInstaller;
        SqlServerInstaller localDbUpgrader;

        // List of instances installed during the lifetime of the wizard
        List<string> installedInstances = new List<string>();

        // The SQL Instance that we have loaded data file location for
        string dataFileSqlInstance = null;

        // Indicates if the SQL login page is also choosing a database (or just selecting a server)
        bool sqlInstanceChooseDatabase = false;

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

        // If we upgrade a LocalDB to a full SQL Server instance, this stores the name of the instance that was created
        string localDbUpgradedInstance;

        // Allows consumers to control what the wizard allows the user to do
        SetupMode setupMode = SetupMode.Default;

        // What we display for LocalDB instead of the actual connection string
        const string localDbDisplayName = "(Local Only)";

        // To mark our special comboBox items
        object serverSearchAgain = new object();
        object serverSearching = new object();

        // The user setup wizard page
        private ICustomerLicenseActivation tangoUserControlHost;

        /// <summary>
        /// Let's consumers control what the user is allowed to do in the wizard
        /// </summary>
        public enum SetupMode
        {
            Default,
            EnableRemoteConnections
        }

        /// <summary>
        /// The option the user chooses for the first page of the wizard
        /// </summary>
        enum ChooseWiselyOption
        {
            Connect,
            Create,
            Restore
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DetailedDatabaseSetupWizard()
            : this(SetupMode.Default)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        [NDependIgnoreLongMethod]
        public DetailedDatabaseSetupWizard(SetupMode setupMode)
        {
            InitializeComponent();

            this.setupMode = setupMode;

            sqlInstaller = new SqlServerInstaller();
            sqlInstaller.InitializeForCurrentSqlSession();
            sqlInstaller.Exited += new EventHandler(OnInstallerSqlServerExited);

            localDbUpgrader = new SqlServerInstaller();
            localDbUpgrader.InitializeForCurrentSqlSession();
            localDbUpgrader.Exited += new EventHandler(OnUpgradeLocalDbExited);

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
                        new XElement("Restore", (ChooseWisely == ChooseWiselyOption.Restore)));
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
                        new XElement("Restore", (ChooseWisely == ChooseWiselyOption.Restore)));
                });
            Pages.Insert(placeholderIndex, windowsInstallerPage);

            // The first prereq is Windows installer 4.5 which then chains to .net 3.5
            pageFirstPrerequisite = windowsInstallerPage;

            // Event not available in the designer
            comboSqlServers.MouseWheel += new MouseEventHandler(OnSqlServerMouseWheel);

            // Resolve the user control
            tangoUserControlHost = IoC.UnsafeGlobalLifetimeScope.Resolve<ICustomerLicenseActivation>();
            tangoUserControlHost.StepNext += OnStepNextShipWorksAdmin;
            tangoUserControlHost.SteppingInto += OnSteppingIntoShipWorksAdmin;

            // Replace the user wizard page with the new tango user wizard page
            Pages.Insert(Pages.Count - 1, (WizardPage) tangoUserControlHost);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnLoad(object sender, EventArgs e)
        {
            gridDatabses.Rows.Clear();

            StartSearchingSqlServers();

            if (StartupController.StartupAction == StartupAction.OpenDatabaseSetup)
            {
                var afterLocalDbUpgrade = StartupController.StartupArgument.Element("LocalDbUpgrade");
                if (afterLocalDbUpgrade != null && (bool)afterLocalDbUpgrade)
                {
                    Pages.Remove(wizardPageChooseWisely2008);
                    Pages.Remove(wizardPageChooseWisely2012);

                    // We removed the first pages, which the wizard handles after OnLoad finishes - but it hasn't finished (we are still in it), so we have
                    // to ensure the first page ourself, or MoveNext won't work
                    SetCurrent(0);

                    radioLocalDbEnableRemote.Checked = true;

                    MoveNext();
                }
                else
                {
                    Pages.Remove(wizardPageChooseWisely2012);
                    Pages.Remove(wizardPageManageLocalDb);

                    // We removed the first pages, which the wizard handles after OnLoad finishes - but it hasn't finished (we are still in it), so we have
                    // to ensure the first page ourself, or MoveNext won't work
                    SetCurrent(0);

                    if (StartupController.StartupArgument.Name == "Upgrade2x")
                    {
                        var setupControls = new List<Control>
                                    {
                                        radioChooseCreate2008, pictureBoxSetupNewDatabase, labelSetupNewDatabase,
                                        radioChooseConnect2008, pictureBoxConnectRunningDatabase, labelConnectRunningDatabase
                                    };

                        var restoreControls = new List<Control>
                                    {
                                        radioChooseRestore2008, pictureBoxRestoreDatabase, labelRestoreDatabase
                                    };

                        int setupOffset = radioChooseConnect2008.Top - radioChooseCreate2008.Top;
                        int restoreOffset = radioChooseCreate2008.Top - radioChooseRestore2008.Top;

                        foreach (var control in setupControls)
                        {
                            control.Top += setupOffset;
                        }

                        foreach (var control in restoreControls)
                        {
                            control.Top += restoreOffset;
                        }

                        radioChooseRestore2008.Checked = true;
                        radioChooseRestore2008.Text = "Upgrade from a ShipWorks 2 Backup";
                        labelRestoreDatabase.Text = "Select this option to import ShipWorks 2 data without affecting ShipWorks 2.";

                        radioChooseCreate2008.ForeColor = SystemColors.GrayText;
                        radioChooseConnect2008.ForeColor = SystemColors.GrayText;

                        instanceName.Text = "SHIPWORKS3";
                    }
                    else
                    {
                        bool wasDoingRestore = (bool)StartupController.StartupArgument.Element("Restore");

                        if (wasDoingRestore)
                        {
                            radioChooseRestore2008.Checked = true;
                            MoveNext();

                            radioRestoreIntoNewDatabase.Checked = true;
                            MoveNext();
                        }
                        else
                        {
                            radioChooseCreate2008.Checked = true;
                            MoveNext();
                        }

                        radioInstallSqlServer.Checked = true;
                        instanceName.Text = (string)StartupController.StartupArgument.Element("InstanceName");

                        // If we are here after rebooting from a successful install that needed a reboot, fastforward past the install page
                        var afterInstallSuccess = StartupController.StartupArgument.Element("AfterInstallSuccess");
                        if (afterInstallSuccess != null && (bool)afterInstallSuccess)
                        {
                            log.InfoFormat("Replaying SQL Install Success after reboot.");

                            installedInstances.Add(instanceName.Text);

                            // Reload all the SQL Session from last time
                            sqlSession.Configuration.ServerInstance = Environment.MachineName + "\\" + instanceName.Text;
                            sqlSession.Configuration.Username = "sa";
                            sqlSession.Configuration.Password = SqlInstanceUtility.ShipWorksSaPassword;
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
                }

                StartupController.ClearStartupAction();
            }
            else if (setupMode == SetupMode.EnableRemoteConnections)
            {
                // We just have to remove the first three pages so the only option is to do the LocalDb upgrade
                Pages.Remove(wizardPageChooseWisely2012);
                Pages.Remove(wizardPageChooseWisely2008);
                Pages.Remove(wizardPageManageLocalDb);
            }
            else
            {
                // Setup which first page the user will see
                if (SqlServerInstaller.IsSqlServer2012Supported)
                {
                    Pages.Remove(wizardPageChooseWisely2008);

                    if (SqlSession.IsConfigured && SqlSession.Current.Configuration.IsLocalDb())
                    {
                        Pages.Remove(wizardPageChooseWisely2012);
                    }
                    else
                    {
                        Pages.Remove(wizardPageManageLocalDb);
                    }
                }
                else
                {
                    Pages.Remove(wizardPageChooseWisely2012);
                    Pages.Remove(wizardPageManageLocalDb);
                }
            }
        }

        #region Setup or Connect

        /// <summary>
        /// The option the user has choosen on the first page of the wizard.  The reason for all this mess is we use two different layouts, for the same options, depending
        /// on if the user's machine supports SQL 2012
        /// </summary>
        private ChooseWiselyOption ChooseWisely
        {
            get
            {
                if (Pages.Contains(wizardPageChooseWisely2012))
                {
                    if (radioChooseConnect2012.Checked) return ChooseWiselyOption.Connect;
                    if (radioChooseCreate2012.Checked) return ChooseWiselyOption.Create;
                    return ChooseWiselyOption.Restore;
                }
                else if (Pages.Contains(wizardPageChooseWisely2008))
                {
                    if (radioChooseConnect2008.Checked) return ChooseWiselyOption.Connect;
                    if (radioChooseCreate2008.Checked) return ChooseWiselyOption.Create;
                    return ChooseWiselyOption.Restore;
                }
                else
                {
                    if (radioRestoreBackupLocalDb.Checked) return ChooseWiselyOption.Restore;
                    return ChooseWiselyOption.Connect;
                }
            }
        }

        /// <summary>
        /// Stepping next from the first page
        /// </summary>
        private void OnStepNextSetupOrConnect(object sender, WizardStepEventArgs e)
        {
            switch (ChooseWisely)
            {
                case ChooseWiselyOption.Connect:
                    {
                        sqlInstanceChooseDatabase = true;
                        e.NextPage = wizardPageSelectSqlServerInstance;
                    }
                    break;

                case ChooseWiselyOption.Create:
                    {
                        e.NextPage = wizardPageChooseSqlServer;
                    }
                    break;

                case ChooseWiselyOption.Restore:
                    {
                        e.NextPage = wizardPageRestoreOption;
                    }
                    break;
            }
        }

        /// <summary>
        /// Show advanced options on the choose wisely page
        /// </summary>
        private void OnChooseWiselyAdvancedOptions(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabelAdvancedOptions.Visible = false;

            radioChooseCreate2012.Visible = true;
            radioChooseCreate2012.Top = linkLabelAdvancedOptions.Top;

            radioChooseRestore2012.Visible = true;
            radioChooseRestore2012.Top = radioChooseCreate2012.Top + 26;
        }

        /// <summary>
        /// Open the help link for learning how to enable remote connections
        /// </summary>
        private void OnLinkLearnEnableRemoteConnections(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/shipworks/help/EnableRemoteConnections.html", this);
        }

        #endregion

        #region Manage Local DB

        /// <summary>
        /// Stepping next from the manage local db page
        /// </summary>
        private void OnStepNextManageLocalDb(object sender, WizardStepEventArgs e)
        {
            if (radioLocalDbConnect.Checked)
            {
                sqlInstanceChooseDatabase = true;
                e.NextPage = wizardPageSelectSqlServerInstance;
            }
            else if (radioRestoreBackupLocalDb.Checked)
            {
                e.NextPage = wizardPageRestoreOption;
            }
            else
            {
                e.NextPage = wizardPageUpgradeLocalDb;
            }
        }

        /// <summary>
        /// Show advanced options on the choose wisely page
        /// </summary>
        private void OnManageLocalDbAdvancedOptions(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkAdvancedOptionsLocalDb.Visible = false;

            radioRestoreBackupLocalDb.Visible = true;
            radioRestoreBackupLocalDb.Top = linkAdvancedOptionsLocalDb.Top;
        }

        #endregion

        #region Upgrade Local DB

        /// <summary>
        /// Upgrade the Local DB in order to enable remote connections
        /// </summary>
        private void OnStepNextUpgradeLocalDb(object sender, WizardStepEventArgs e)
        {
            // If it was installed, but needs a reboot, move to the last page (which will now be the reboot page)
            if (localDbUpgrader.LastExitCode == SqlServerInstaller.ExitCodeSuccessRebootRequired || SqlServerInstaller.IsErrorCodeRebootRequiredBeforeInstall(localDbUpgrader.LastExitCode))
            {
                e.NextPage = Pages[Pages.Count - 1];
                return;
            }

            // If its installed now, we are ok to move on.
            if (localDbUpgradedInstance != null && SqlInstanceUtility.IsSqlInstanceInstalled(localDbUpgradedInstance))
            {
                labelSetupComplete.Text = "Remote connections are now supported.";

                e.NextPage = wizardPageComplete;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Bring the upgrading message up and diable and the browsing buttons
            panelUpgradeLocalDb.Location = panelUpgradeLocalDbReady.Location;
            panelUpgradeLocalDb.Visible = true;
            panelUpgradeLocalDbReady.Visible = false;
            NextEnabled = false;
            BackEnabled = false;

            // Stay on this page
            e.NextPage = CurrentPage;

            try
            {
                localDbUpgradedInstance = localDbUpgrader.UpgradeLocalDb();

                progressUpdgradeLocalDb.Value = 0;
                progressLocalDbTimer.Start();
                progressLocalDbTimer.Tag = null;
            }
            catch (Win32Exception ex)
            {
                if (ex.NativeErrorCode == 1602 || ex.NativeErrorCode == 1603 || ex.NativeErrorCode == 1223)
                {
                    MessageHelper.ShowInformation(this, "You must click 'Yes' when asked to allow ShipWorks to make changes to your computer.");
                }
                else
                {
                    MessageHelper.ShowError(this, "An error occurred while enabling support for remote connections:\n\n" + ex.Message);
                }

                // Reset the gui
                panelUpgradeLocalDb.Visible = false;
                panelUpgradeLocalDbReady.Visible = true;
                NextEnabled = true;
                BackEnabled = true;
            }
        }

        /// <summary>
        /// Simple UI timer we use to keep the progress of SQL Server install aproximated
        /// </summary>
        void OnUpgradeLocalDbProgressTimer(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(localDbUpgrader.GetInstallerLocalFilePath(SqlServerInstallerPurpose.Install));
            long expectedFileSize = localDbUpgrader.GetInstallerFileLength(SqlServerInstallerPurpose.Install);

            // Not downloaded at all
            if (!fileInfo.Exists)
            {
                progressPreparing.Value = 0;
            }
            // In the process of downloading
            else if (fileInfo.Length < expectedFileSize)
            {
                progressUpdgradeLocalDb.Value = (int)(33.0 * (double)fileInfo.Length / expectedFileSize);
            }
            // Fully downloaded
            else
            {
                Stopwatch elapsed = progressLocalDbTimer.Tag as Stopwatch;
                if (elapsed == null)
                {
                    elapsed = Stopwatch.StartNew();
                    progressLocalDbTimer.Tag = elapsed;
                }

                // Download counts as 33%.  After that, installing counts all the way up to 100, and we progress it assuming it will take 5 minutes
                progressUpdgradeLocalDb.Value = Math.Min(98, 33 + (int)((elapsed.Elapsed.TotalSeconds / 300.0) * 0.66 * 100));
            }
        }

        /// <summary>
        /// The upgrade local db background process has completed.
        /// </summary>
        private void OnUpgradeLocalDbExited(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler(OnUpgradeLocalDbExited), new object[] { sender, e });
                return;
            }

            progressLocalDbTimer.Stop();

            // If it was successful, we should now be able to connect.
            if (localDbUpgrader.LastExitCode == 0 && SqlInstanceUtility.IsSqlInstanceInstalled(localDbUpgradedInstance))
            {
                sqlSession.Configuration.Username = "sa";
                sqlSession.Configuration.Password = SqlInstanceUtility.ShipWorksSaPassword;
                sqlSession.Configuration.WindowsAuth = false;
                sqlSession.Configuration.ServerInstance = Environment.MachineName + "\\" + localDbUpgradedInstance;
                sqlSession.Configuration.DatabaseName = ShipWorksDatabaseUtility.AutomaticDatabaseName;

                installedInstances.Add(localDbUpgradedInstance);

                // Since we installed it, we can do this without asking
                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    SqlUtility.EnableClr(con);
                }

                MoveNext();
            }
            else if (localDbUpgrader.LastExitCode == SqlServerInstaller.ExitCodeSuccessRebootRequired || SqlServerInstaller.IsErrorCodeRebootRequiredBeforeInstall(localDbUpgrader.LastExitCode))
            {
                Pages.Add(new RebootRequiredPage(
                    "some prerequisites",
                    StartupAction.OpenDatabaseSetup,
                    () =>
                    {
                        return new XElement("Replay",
                            new XElement("InstanceName", localDbUpgradedInstance),
                            new XElement("LocalDbUpgrade", true));
                    }));

                MoveNext();
            }
            else
            {
                MessageHelper.ShowError(this,
                    "Support for remote connections could not be enabled.\n\n" + SqlServerInstaller.FormatReturnCode(localDbUpgrader.LastExitCode));

                NextEnabled = true;
                BackEnabled = true;
            }

            // Reset the gui
            panelUpgradeLocalDb.Visible = false;
            panelUpgradeLocalDbReady.Visible = true;
        }

        /// <summary>
        /// User trying to cancel the upgrade of Local DB
        /// </summary>
        private void OnCancellUpgradeLocalDb(object sender, CancelEventArgs e)
        {
            if (!NextEnabled)
            {
                MessageHelper.ShowMessage(this, "Please wait for ShipWorks to finish enabling support for remote connections.");
                e.Cancel = true;
            }
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
            else
            {
                // If it's local DB, then we can't allow the option to restore into a new database
                if (SqlSession.Current.Configuration.IsLocalDb())
                {
                    radioRestoreIntoCurrent.Checked = true;

                    // And we just automatically move on
                    e.Skip = true;
                    e.RaiseStepEventWhenSkipping = true;
                }
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
                radioSqlServerCurrent.Text = "Use the current instance of Microsoft SQL Server";

                labelSqlServerCurrentName.Text = string.Format("({0})", SqlSession.Current.Configuration.IsLocalDb() ? "Local only" : SqlSession.Current.Configuration.ServerInstance);
            }
            // Or if the ShipWorks instance has been installed at some point, default to that
            else if (SqlInstanceUtility.IsSqlInstanceInstalled("SHIPWORKS"))
            {
                radioSqlServerCurrent.Checked = true;
                radioSqlServerCurrent.Text = "Use the installed instance of Microsoft SQL Server";

                labelSqlServerCurrentName.Text = string.Format("({0}\\{1})", Environment.MachineName, "SHIPWORKS");
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
        [NDependIgnoreLongMethod]
        private void OnStepNextNewOrExistingSqlServer(object sender, WizardStepEventArgs e)
        {
            if (radioInstallSqlServer.Checked)
            {
                instanceName.Text = instanceName.Text.Trim();

                // Save the instance
                sqlSession.Configuration.ServerInstance = Environment.MachineName + "\\" + instanceName.Text;
                sqlSession.Configuration.Username = "sa";
                sqlSession.Configuration.Password = SqlInstanceUtility.ShipWorksSaPassword;
                sqlSession.Configuration.WindowsAuth = false;

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
                        sqlSession.Configuration.ServerInstance = Environment.MachineName + "\\" + instanceName.Text;
                        e.NextPage = pageFirstPrerequisite;
                    }
                }
            }
            else
            {

                if (radioSqlServerCurrent.Checked)
                {
                    SqlSessionConfiguration newConfiguration;

                    // If it was configured, we are using the current
                    if (SqlSession.IsConfigured)
                    {
                        newConfiguration = SqlSession.Current.Configuration;
                    }
                    // Otherwise we are defaulting to "SHIPWORKS"
                    else
                    {
                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;

                            newConfiguration = SqlInstanceUtility.DetermineCredentials(Environment.MachineName + "\\SHIPWORKS");
                        }
                        catch (SqlException ex)
                        {
                            MessageHelper.ShowError(this, "ShipWorks could not connect to the selected instance.\n\n" + ex.Message);

                            e.NextPage = CurrentPage;
                            return;
                        }
                    }

                    // newConfiguration can be null if none of the credentials we tried were able to log in
                    if (newConfiguration == null)
                    {
                        MessageHelper.ShowError(this, "ShipWorks could not connect to the selected instance.");

                        e.NextPage = CurrentPage;
                        return;
                    }

                    sqlSession.Configuration.ServerInstance = newConfiguration.ServerInstance;
                    sqlSession.Configuration.Username = newConfiguration.Username;
                    sqlSession.Configuration.Password = newConfiguration.Password;
                    sqlSession.Configuration.WindowsAuth = newConfiguration.WindowsAuth;

                    e.NextPage = wizardPageDatabaseName;
                }
                else
                {
                    sqlInstanceChooseDatabase = false;
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
            if (!((RadioButton)sender).Checked)
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
            comboSqlServers.Items.Clear();
            comboSqlServers.Items.Add(new ImageComboBoxItem("Searching...", serverSearching, Resources.indiciator_green) { Selectable = false });

            // Force the list to refresh, in case it's already dropped down
            comboSqlServers.RefreshItemList();

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

            BackgroundWorker worker = (BackgroundWorker)sender;
            worker.ReportProgress(0, servers);
        }

        /// <summary>
        /// Progress being reported while calculating file size
        /// </summary>
        void OnFoundSqlServerInstances(object sender, ProgressChangedEventArgs e)
        {
            // If we have closed, don't go on
            if (TopLevelControl == null || !TopLevelControl.Visible)
            {
                ((BackgroundWorker)sender).CancelAsync();
                return;
            }

            panelSqlInstanceHelp.Top = panelSearchSqlServers.Top;
            panelSqlInstanceHelp.Visible = true;
            panelSearchSqlServers.Visible = false;

            comboSqlServers.Items.Clear();
            comboSqlServers.Items.Add(new ImageComboBoxItem("Refresh...", serverSearchAgain, Resources.arrows_green_static) { CloseOnSelect = false });

            string[] servers = (string[])e.UserState;

            if (SqlInstanceUtility.IsLocalDbInstalled())
            {
                comboSqlServers.Items.Add(new ImageComboBoxItem(localDbDisplayName, Resources.server));
            }

            // Load the list with all servers found on the LAN
            foreach (string server in servers)
            {
                comboSqlServers.Items.Add(new ImageComboBoxItem(server, string.Compare(SqlInstanceUtility.ExtractServerName(server), Environment.MachineName, true) == 0 ? Resources.server : Resources.server_client2));
            }

            // Force the list to refresh, in case it's already dropped down
            comboSqlServers.RefreshItemList();

            // Only automatically select the first SQL instance if we are on the SQL Instance page right now.  If we aren't, we don't want to be changing out
            // the selected instance from under the user while they are on a different page.
            if (CurrentPage == wizardPageSelectSqlServerInstance)
            {
                SelectFirstSqlInstance();
            }
        }

        /// <summary>
        /// Automatically select the first SQL instance if none is currently selected
        /// </summary>
        private void SelectFirstSqlInstance()
        {
            // Auto-select first one if nothing is in there already.  We use index 1, because index 0 is always Refresh
            if (comboSqlServers.Text.Length == 0 && comboSqlServers.Items.Count > 1)
            {
                comboSqlServers.SelectedIndex = 1;
            }
        }

        /// <summary>
        /// Stepping into the SQL instance window
        /// </summary>
        private void OnSteppingIntoSelectSqlInstance(object sender, WizardSteppingIntoEventArgs e)
        {
            labelDatabaseSelect.Text = sqlInstanceChooseDatabase ? "Select your ShipWorks database" : "Connection Check";
            gridDatabses.Visible = sqlInstanceChooseDatabase;

            if (e.FirstTime)
            {
                // Prepopulate the instance box with the current
                if (SqlSession.IsConfigured)
                {
                    comboSqlServers.Text = SqlSession.Current.Configuration.IsLocalDb() ?
                        localDbDisplayName :
                        SqlSession.Current.Configuration.ServerInstance;

                    // Don't show the "Searching..." next to the ComboBox if we are also showing that we are trying to connect to a selected instance
                    panelSearchSqlServers.Visible = false;
                }
                else
                {
                    SelectFirstSqlInstance();
                }

                // This function handles a selected instance or blank
                ConnectToSelectedServerInstance();
            }
            else
            {
                SelectFirstSqlInstance();
            }
        }

        /// <summary>
        /// Wheel scrolling on the SQL Server combo
        /// </summary>
        private void OnSqlServerMouseWheel(object sender, MouseEventArgs e)
        {
            // If the combo isn't dropped down, don't scroll, as that pisses off users
            if (!comboSqlServers.DroppedDown)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        /// <summary>
        /// Leaving focus from the SQL instance combo
        /// </summary>
        private void OnLeaveSqlInstance(object sender, EventArgs e)
        {
            ConnectToSelectedServerInstance();
        }

        /// <summary>
        /// Custom command key procesing for hitting enter in the combo box
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (comboSqlServers.Focused && keyData == Keys.Return)
            {
                ConnectToSelectedServerInstance();
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        /// <summary>
        /// The selected SQL Server instance has changed
        /// </summary>
        private void OnChangeSelectedInstance(object sender, EventArgs e)
        {
            ImageComboBoxItem selected = comboSqlServers.SelectedItem as ImageComboBoxItem;
            if (selected != null)
            {
                // The only time there'd be a value is in our special marker items that shouldn't be selected
                if (selected.Value != null)
                {
                    // Kick off a search again
                    if (selected.Value == serverSearchAgain)
                    {
                        StartSearchingSqlServers();
                    }

                    comboSqlServers.SelectedIndexChanged -= OnChangeSelectedInstance;
                    comboSqlServers.SelectedIndex = -1;
                    comboSqlServers.SelectedIndexChanged += OnChangeSelectedInstance;

                    return;
                }
                else
                {
                    // Update the text in the ComboBox
                    comboSqlServers.Text = selected.Text;
                }
            }

            ConnectToSelectedServerInstance();
        }

        /// <summary>
        /// Connect to the currently selected SQL Server instance
        /// </summary>
        [NDependIgnoreLongMethod]
        private void ConnectToSelectedServerInstance()
        {
            string selectedInstance = (comboSqlServers.Text == localDbDisplayName) ? SqlInstanceUtility.LocalDbServerInstance : comboSqlServers.Text;

            // If we are already conected to or connecting to this exact session, then forget it.
            if (connectionSession != null)
            {
                // Same server
                if (connectionSession.Configuration.ServerInstance == selectedInstance)
                {
                    // Same auth (windows) so we can get out
                    if (sqlSession.Configuration.WindowsAuth && connectionSession.Configuration.WindowsAuth)
                    {
                        return;
                    }

                    // Same auth (password) so we can get out
                    if (!sqlSession.Configuration.WindowsAuth && !connectionSession.Configuration.WindowsAuth &&
                        connectionSession.Configuration.Username == sqlSession.Configuration.Username &&
                        connectionSession.Configuration.Password == sqlSession.Configuration.Password)
                    {
                        return;
                    }
                }
            }

            // Create a copy that we can mess around with without affecting the UI session
            connectionSession = new SqlSession(sqlSession);
            connectionSession.Configuration.ServerInstance = selectedInstance;

            // If there is no selected instance name, we just clear everything out. And get out.
            if (string.IsNullOrWhiteSpace(selectedInstance))
            {
                gridDatabses.Rows.Clear();
                gridDatabses.EmptyText = "";

                panelSelectedInstance.Visible = false;

                pictureSqlConnection.Visible = false;
                labelSqlConnection.Visible = false;
                linkSqlInstanceAccount.Visible = false;

                return;
            }
            else
            {
                panelSelectedInstance.Visible = true;
            }

            // Clear the database list
            gridDatabses.Rows.Clear();
            gridDatabses.EmptyText = "";
            gridDatabses.Visible = false;

            pictureSqlConnection.Image = Resources.arrows_greengray;
            pictureSqlConnection.Visible = true;

            labelSqlConnection.Text = string.Format("Connecting to '{0}'...", comboSqlServers.Text);
            labelSqlConnection.Visible = true;

            linkSqlInstanceAccount.Visible = false;

            // Create another variable for closure purposes
            SqlSession backgroundSession = connectionSession;
            SqlSessionConfiguration firstTryConfiguration = !string.IsNullOrEmpty(sqlSession.Configuration.ServerInstance) ? sqlSession.Configuration : (SqlSession.IsConfigured ? SqlSession.Current.Configuration : null);

            // Start the background task to try to log in and figure out the background databases...
            var task = Task.Factory.StartNew(() =>
            {
                SqlSessionConfiguration configuration = SqlInstanceUtility.DetermineCredentials(backgroundSession.Configuration.ServerInstance, firstTryConfiguration);

                if (configuration != null)
                {
                    using (SqlConnection con = new SqlSession(configuration).OpenConnection())
                    {
                        return Tuple.Create(configuration, ShipWorksDatabaseUtility.GetDatabaseDetails(con));
                    }
                }
                else
                {
                    return null;
                }
            });

            task.ContinueWith(t =>
            {
                // If the background session we know about isn't the same as the current connection session, then the user has changed
                // it in the meantime on us and we discard the results
                if (backgroundSession != connectionSession)
                {
                    return;
                }

                // Whether it worked or not, this is now the SQL Instance this session is configured for.  This is especially important to set now in case
                // the user opens the window to try to change the credentials.
                sqlSession.Configuration.ServerInstance = selectedInstance;

                string instanceDisplay = (selectedInstance == SqlInstanceUtility.LocalDbServerInstance) ? localDbDisplayName : selectedInstance;

                // Null indicates error
                if (t.Result == null)
                {
                    pictureSqlConnection.Image = Resources.warning16;
                    labelSqlConnection.Text = string.Format("Could not connect to '{0}'", instanceDisplay);
                    linkSqlInstanceAccount.Text = "Try changing the account";

                    gridDatabses.EmptyText = "";
                    gridDatabses.Visible = false;
                }
                else
                {
                    SqlSessionConfiguration configuration = t.Result.Item1;
                    List<SqlDatabaseDetail> databases = t.Result.Item2;

                    pictureSqlConnection.Image = Resources.check16;
                    labelSqlConnection.Text = string.Format("Connected to '{0}' using {1} account.", instanceDisplay, configuration.WindowsAuth ? "your Windows" : string.Format("the '{0}'", configuration.Username));
                    linkSqlInstanceAccount.Text = "Change";

                    gridDatabses.EmptyText = "No databases were found.";
                    gridDatabses.Visible = sqlInstanceChooseDatabase;

                    // Save the credentials
                    sqlSession.Configuration.Username = configuration.Username;
                    sqlSession.Configuration.Password = configuration.Password;
                    sqlSession.Configuration.WindowsAuth = configuration.WindowsAuth;

                    // We also have to save them to our connectionSession, so we can properly track when\if it changes
                    connectionSession.Configuration.Username = configuration.Username;
                    connectionSession.Configuration.Password = configuration.Password;
                    connectionSession.Configuration.WindowsAuth = configuration.WindowsAuth;

                    LoadDatabaseList(databases, configuration);
                }

                linkSqlInstanceAccount.Left = labelSqlConnection.Right;
                linkSqlInstanceAccount.Visible = !backgroundSession.Configuration.IsLocalDb();

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Load the database grid
        /// </summary>
        [NDependIgnoreLongMethod]
        private void LoadDatabaseList(List<SqlDatabaseDetail> databases, SqlSessionConfiguration configuration)
        {
            gridDatabses.Rows.Clear();

            // Add a row for each database
            foreach (SqlDatabaseDetail database in databases.OrderBy(d => d.Name))
            {
                string status;
                string activity = "";
                string order = "";

                bool isCurrent = SqlSession.IsConfigured &&
                    SqlSession.Current.Configuration.ServerInstance == configuration.ServerInstance &&
                    SqlSession.Current.Configuration.DatabaseName == database.Name;

                // If it's the database we are currently connected to
                if (isCurrent)
                {
                    status = "(Active)";
                }
                // A ShipWorks 3x database get's it's status based on schema being older, newer, or same
                else if (database.Status == SqlDatabaseStatus.ShipWorks)
                {
                    if (database.SchemaVersion > SqlSchemaUpdater.GetRequiredSchemaVersion())
                    {
                        status = "Newer";
                    }
                    else if (database.SchemaVersion < SqlSchemaUpdater.GetRequiredSchemaVersion())
                    {
                        status = "Out of Date";
                    }
                    else
                    {
                        status = "Ready";
                    }
                }
                // ShipWorks 2
                else if (database.Status == SqlDatabaseStatus.ShipWorks2x)
                {
                    status = "ShipWorks 2";
                }
                // Not a ShipWorks database
                else if (database.Status == SqlDatabaseStatus.NonShipWorks)
                {
                    status = "Non-ShipWorks";
                }
                // Couldn't connect for some reason
                else
                {
                    status = "Couldn't Connect";
                }

                // See if we have info on the last logged in user
                if (!string.IsNullOrWhiteSpace(database.LastUsedBy))
                {
                    activity = string.Format("{0}, on {1}", database.LastUsedBy, database.LastUsedOn.ToLocalTime().ToString("MM/dd/yy"));
                }

                // See if we have info on the most recently created order
                if (!string.IsNullOrWhiteSpace(database.LastOrderNumber))
                {
                    order = string.Format("{0}, from {1}", database.LastOrderNumber, database.LastOrderDate.ToLocalTime().ToString("MM/dd/yy"));
                }

                string name = database.Name;

                // If it's local db, we special case the naming
                if (configuration.IsLocalDb())
                {
                    // If this is the default database for this instance of ShipWorks, then just call it default
                    if (database.Name == ShipWorksDatabaseUtility.LocalDbDatabaseName)
                    {
                        name = "(Default)";
                    }
                    // If it's a LocalDB, but for another instance of ShipWorks, we don't even show it
                    else if (ShipWorksDatabaseUtility.IsLocalDbDatabaseName(database.Name))
                    {
                        name = null;
                    }
                }

                // Only create the row if it wasn't nulled out on purpose to skip showing it
                if (name != null)
                {
                    gridDatabses.Rows.Add(new GridRow(new string[] { name, status, activity, order }) { Tag = database });

                    if (isCurrent)
                    {
                        gridDatabses.Rows[gridDatabses.Rows.Count - 1].Selected = true;
                    }
                }
            }
        }

        /// <summary>
        /// User wants to change the SQL Server account to use
        /// </summary>
        private void OnChangeSqlInstanceAccount(object sender, EventArgs e)
        {
            using (SqlCredentialsDlg dlg = new SqlCredentialsDlg(sqlSession))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ConnectToSelectedServerInstance();
                }
            }
        }

        /// <summary>
        /// Stepping next from selecting the sql instance to connect to.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNextSelectSqlInstance(object sender, WizardStepEventArgs e)
        {
            if (comboSqlServers.Text.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Please select or enter the name of a SQL Server instance.");

                e.NextPage = CurrentPage;
                return;
            }

            // Save the instance
            sqlSession.Configuration.ServerInstance = (comboSqlServers.Text == localDbDisplayName) ? SqlInstanceUtility.LocalDbServerInstance : comboSqlServers.Text;

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
                    if (ChooseWisely == ChooseWiselyOption.Restore || ChooseWisely == ChooseWiselyOption.Create)
                    {
                        MessageHelper.ShowError(this,
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
                if (ChooseWisely == ChooseWiselyOption.Restore && !sqlSession.IsLocalServer())
                {
                    MessageHelper.ShowInformation(this, "A ShipWorks restore can only be done from the computer that is running SQL Server.");
                    e.NextPage = CurrentPage;
                    return;
                }
            }
            catch (SqlException ex)
            {
                MessageHelper.ShowError(this,
                    "ShipWorks could not login to SQL Server '" + sqlSession.Configuration.ServerInstance + "' using " +
                    "the given login information.\n\n" +
                    "Detail: " + ex.Message);

                e.NextPage = CurrentPage;
                return;
            }

            // Connected to SQL Server, now see if we need to validate the database
            if (sqlInstanceChooseDatabase)
            {
                if (gridDatabses.SelectedElements.Count == 0)
                {
                    MessageHelper.ShowInformation(this, "Please select a database before continuing.");
                    e.NextPage = CurrentPage;

                    return;
                }

                string database = ((SqlDatabaseDetail)gridDatabses.SelectedElements[0].Tag).Name;

                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    SqlDatabaseDetail detail = ShipWorksDatabaseUtility.GetDatabaseDetail(database, con);

                    if (detail.Status == SqlDatabaseStatus.ShipWorks || detail.Status == SqlDatabaseStatus.ShipWorks2x)
                    {
                        // As long as it's a ShipWorks database, we can upgrade it later if it's out of date
                        sqlSession.Configuration.DatabaseName = database;

                        // Complete
                        e.NextPage = (WizardPage)tangoUserControlHost;
                    }
                    else
                    {
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
            }
            else
            {
                e.NextPage = wizardPageDatabaseName;
            }
        }

        /// <summary>
        /// Open the help link to troubleshoot connecting to a database
        /// </summary>
        private void OnLinkSqlTroubleshooting(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/shipworks/help/DatabaseTroubleshooting.html", this);
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
                progressInstallTimer.Start();
                progressInstallTimer.Tag = null;
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
                progressPreparing.Value = (int)(33.0 * (double)fileInfo.Length / expectedFileSize);
            }
            // Fully downloaded
            else
            {
                Stopwatch elapsed = progressInstallTimer.Tag as Stopwatch;
                if (elapsed == null)
                {
                    elapsed = Stopwatch.StartNew();
                    progressInstallTimer.Tag = elapsed;
                }

                // Download counts as 33%.  After that, installing counts all the way up to 100, and we progress it assuming it will take 5 minutes
                progressPreparing.Value = Math.Min(98, 33 + (int)((elapsed.Elapsed.TotalSeconds / 300.0) * 0.66 * 100));
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

            progressInstallTimer.Stop();

            // If it was successful, we should now be able to connect.
            if (sqlInstaller.LastExitCode == 0 && SqlInstanceUtility.IsSqlInstanceInstalled(instanceName.Text))
            {
                sqlSession.Configuration.Username = "sa";
                sqlSession.Configuration.Password = SqlInstanceUtility.ShipWorksSaPassword;
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
                            new XElement("Restore", (ChooseWisely == ChooseWiselyOption.Restore)),
                            new XElement("AfterInstallSuccess", true));
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

        #region Database Name

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

            // See if the server had changed...
            if (dataFileSqlInstance != sqlSession.Configuration.ServerInstance)
            {
                Cursor.Current = Cursors.WaitCursor;

                linkChooseDataLocation.Visible = true;
                panelDataFiles.Visible = false;

                panelDatabaseGivenName.Visible = true;
                panelDatabaseChooseName.Visible = false;

                using (SqlConnection con = sqlSession.OpenConnection())
                {
                    givenDatabaseName.Text = ShipWorksDatabaseUtility.GetFirstAvailableDatabaseName(con);
                    databaseName.Text = givenDatabaseName.Text;

                    linkEditGivenDatabaseName.Left = givenDatabaseName.Right + 1;

                    pathDataFiles.Text = SqlUtility.GetMasterDataFilePath(con);
                }

                dataFileSqlInstance = sqlSession.Configuration.ServerInstance;
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
                    ShipWorksDatabaseUtility.CreateDatabase(name, pathDataFiles.Text, con);
                }

                sqlSession.Configuration.DatabaseName = name;

                pendingDatabaseCreated = true;
                pendingDatabaseName = sqlSession.Configuration.DatabaseName;

                // Restoring
                if (ChooseWisely == ChooseWiselyOption.Restore)
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
                            ShipWorksDatabaseUtility.CreateSchemaAndData();
                        }
                    }
                    // If something goes wrong, drop the db we just created
                    catch
                    {
                        DropPendingDatabase();

                        throw;
                    }

                    e.NextPage = (WizardPage)tangoUserControlHost;
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
                ShipWorksDatabaseUtility.DropDatabase(sqlSession, pendingDatabaseName);

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
        /// User wants to edit the database name we assigned
        /// </summary>
        private void OnLinkEditDatabaseName(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelDatabaseChooseName.Top = panelDatabaseGivenName.Top;
            panelDatabaseGivenName.Visible = false;
            panelDatabaseChooseName.Visible = true;
        }

        /// <summary>
        /// Clicking the link to choose the data file location
        /// </summary>
        private void OnChooseDataFileLocation(object sender, EventArgs e)
        {
            if (!sqlSession.IsLocalServer())
            {
                MessageHelper.ShowError(this,
                    "You can only choose the location of the data files from the same computer that is running the database.\n\n" +
                    "(Your database is on '" + sqlSession.GetServerMachineName() + "')");

                return;
            }

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
        [NDependIgnoreLongMethod]
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
            MethodInvoker<string> invoker = (MethodInvoker<string>)((object[])asyncResult.AsyncState)[0];
            ProgressDlg progressDlg = (ProgressDlg)((object[])asyncResult.AsyncState)[1];

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
            // Default to not moving on
            WizardPage nextPage = e.NextPage;
            e.NextPage = CurrentPage;

            GenericResult<ICustomerLicense> result;

            using (new SqlSessionScope(sqlSession))
            {
                result = tangoUserControlHost.Save();
            }

            if (result.Success)
            {
                //Now we can move on
                adminUserCreated = true;
                e.NextPage = nextPage;
            }
            else
            {
                MessageHelper.ShowMessage(this, result.Message);
            }
        }

        #endregion

        #region Complete

        /// <summary>
        /// The "complete" page is being shown.
        /// </summary>
        private void OnSteppingIntoComplete(object sender, WizardSteppingIntoEventArgs e)
        {
            // This is so we dont delete the pending db in the OnClose
            pendingDatabaseCreated = false;
            pendingDatabaseName = "";

            sqlSession.SaveAsCurrent();

            // If we created this database, then seamlessly continue this wizard into the add store wizard
            if (ChooseWisely == ChooseWiselyOption.Create)
            {
                AddStoreWizard.ContinueAfterCreateDatabase(this, tangoUserControlHost.ViewModel.Email, tangoUserControlHost.ViewModel.DecryptedPassword);
            }
            else
            {
                // We now have a new session
                if (SqlSchemaUpdater.IsCorrectSchemaVersion())
                {
                    UserSession.InitializeForCurrentDatabase();
                }

                // If we created the admin user, go ahead and log that user in
                if (adminUserCreated)
                {
                    IUserService userService = IoC.UnsafeGlobalLifetimeScope.Resolve<IUserService>();

                    EnumResult<UserServiceLogonResultType> logonResult = userService.Logon(new LogonCredentials(tangoUserControlHost.ViewModel.Email, tangoUserControlHost.ViewModel.DecryptedPassword, true));

                    if (logonResult.Value != UserServiceLogonResultType.Success)
                    {
                        MessageHelper.ShowError(this, logonResult.Message);
                    }
                }
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

