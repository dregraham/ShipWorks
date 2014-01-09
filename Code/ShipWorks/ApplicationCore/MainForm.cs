using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Divelements.SandRibbon;
using ShipWorks.ApplicationCore.Enums;
using ShipWorks.ApplicationCore.Services;
using ShipWorks.Data.Administration.Versioning;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.MessageBoxes;
using ShipWorks.ApplicationCore.Options;
using Interapptive.Shared;
using ShipWorks.Common.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Filters;
using ShipWorks.Filters.Management;
using ShipWorks.Properties;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Templates;
using ShipWorks.Templates.Management;
using ShipWorks.Templates.Media;
using ShipWorks.UI;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.Users;
using TD.SandDock;
using RibbonShortcut = Divelements.SandRibbon.Shortcut;
using SandMenuItem = Divelements.SandRibbon.MenuItem;
using SandMenu = Divelements.SandRibbon.Menu;
using SandLabel = Divelements.SandRibbon.Label;
using SandButton = Divelements.SandRibbon.Button;
using SandComboBox = Divelements.SandRibbon.ComboBox;
using ShipWorks.Shipping;
using System.Threading;
using ShipWorks.Templates.Controls;
using System.Linq;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.UI.Utility;
using System.Reflection;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Stores.Platforms;
using ShipWorks.Filters.Controls;
using ShipWorks.Shipping.Editing;
using ShipWorks.Data.Grid;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Filters.Search;
using ShipWorks.Stores.Content;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Saving;
using System.IO;
using ShipWorks.Templates.Emailing;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Email.Outlook;
using ShipWorks.Users.Logon;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Dashboard.Content;
using Divelements.SandGrid;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users.Security;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Users.Audit;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.Settings.Printing;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using Interapptive.Shared.Win32;
using Interapptive.Shared.UI;
using System.Xml.Linq;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Stores.Management;
using ShipWorks.Templates.Distribution;
using Interapptive.Shared.Net;
using ShipWorks.Filters.Grid;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Data.Administration.SqlServerSetup;
using ShipWorks.Data.Administration.UpdateFrom2x;
using ShipWorks.Data.Administration.UpdateFrom2x.Configuration;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Administration.UpdateFrom2x.Database;
using ShipWorks.Editions;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.FileTransfer;
using ShipWorks.ApplicationCore.Help;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore.Setup;

namespace ShipWorks
{
    /// <summary>
    /// Main window of the application.
    /// </summary>
    public partial class MainForm : RibbonForm
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

        // Indicates if the background async login was a success.  This is to make sure we don't keep going on the UI thread 
        // if it failed.
        bool logonAsyncLoadSuccess = false;

        // We have to remember these so that we can restore them after blanking the UI
        List<RibbonTab> ribbonTabs = new List<RibbonTab>();

        // Used to manage the UI state of the online update commands
        OnlineUpdateCommandProvider onlineUpdateCommandProvider = new OnlineUpdateCommandProvider();

        // Used to keep ShipWorks "pumping" looking for data changes
        UIHeartbeat heartBeat;

        // The FilterNode to restore if search is canceled
        long searchRestoreFilterNodeID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Create the heartbeat
            heartBeat = new UIHeartbeat(this);

            // Persist size\position of the window
            WindowStateSaver wss = new WindowStateSaver(this, WindowStateSaverOptions.FullState, "MainForm");
        }

        #region Initialization \ Shutdown

        /// <summary>
        /// Form is loading, this is before its visible
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            log.Info("Loading main application window.");

            DataProvider.InitializeForApplication();
            DataProvider.EntityChangeDetected += new EventHandler(OnEntityChangeDetected);

            // Listen for modal windows to open and close
            Application.EnterThreadModal += new EventHandler(OnEnterThreadModal);
            Application.LeaveThreadModal += new EventHandler(OnLeaveThreadModal);

            // Listen for transitions into a connection sensitive scope
            ConnectionSensitiveScope.Acquiring += new EventHandler(OnAcquiringConnectionSensitiveScope);

            // We want to know right when actions are dispatched so we can run them right away, and we want to know when they are ran
            // so we can update the display right away.
            ActionRunner.ActionStepRan += new EventHandler(OnActionRan);

            InitializeDetailViewControls();
            InitializePanels();

            // We have to save these so they can be restored when the UI goes from blank to normal
            foreach (RibbonTab tab in ribbon.Tabs)
            {
                ribbonTabs.Add(tab);
            }

            // Load the options for what panels can be shown
            menuShowPanels.Items.Clear();
            foreach (DockControl dockControl in sandDockManager.GetDockControls().OrderBy(d => d.Text))
            {
                SandMenuItem menuItem = new SandMenuItem(dockControl.Text);
                menuItem.Image = dockControl.TabImage;
                menuItem.Tag = dockControl;
                menuItem.Activate += new EventHandler(OnShowPanel);
                menuShowPanels.Items.Add(menuItem);
            }

            // Prepare app level initialization
            DashboardManager.InitializeForApplication(dashboardArea);
            AuditProcessor.InitializeForApplication();

            // We need to know what the download is doing
            DownloadManager.DownloadStarting += new EventHandler(OnDownloadStarting);
            DownloadManager.DownloadComplete += new DownloadCompleteEventHandler(OnDownloadComplete);

            // We need to know what the emailer is doing
            EmailCommunicator.EmailStarting += new EventHandler(OnEmailStarting);
            EmailCommunicator.EmailCommunicationComplete += new EmailCommunicationCompleteEventHandler(OnEmailComplete);

            // Initialize the grid menu provider
            gridMenuLayoutProvider.Initialize(gridControl);

            // Initialize ribbon security
            ribbonSecurityProvider.AddAdditionalCondition(buttonUpdateOnline, () => OnlineUpdateCommandProvider.HasOnlineUpdateCommands());
            ribbonSecurityProvider.AddAdditionalCondition(buttonFedExClose, () => FedExAccountManager.Accounts.Count > 0);
            ribbonSecurityProvider.AddAdditionalCondition(buttonEndiciaSCAN, () => (EndiciaAccountManager.EndiciaAccounts.Count + EndiciaAccountManager.Express1Accounts.Count + StampsAccountManager.StampsAccounts.Count + StampsAccountManager.Express1Accounts.Count) > 0);
            ribbonSecurityProvider.AddAdditionalCondition(buttonFirewall, () => (SqlSession.IsConfigured && !SqlSession.Current.Configuration.IsLocalDb()));
            ribbonSecurityProvider.AddAdditionalCondition(buttonChangeConnection, () => (SqlSession.IsConfigured && !SqlSession.Current.Configuration.IsLocalDb()));

            // Prepare stuff that needs prepare for dealing with UI changes for Editions
            PrepareEditionManagedUI();

            // Start off with a blank looking UI
            ShowBlankUI();

            // Load default display settings
            ShipWorksDisplay.LoadDefault();

            ApplyDisplaySettings();
        }

        /// <summary>
        /// Main form has been made visible
        /// </summary>
        private void OnShown(object sender, EventArgs e)
        {
            // If we were just upgraded, we need to move the old sqlsession to where it needs to be
            if (!ShipWorks2xConfigurationMigrator.MigrateIfRequired(this))
            {
                Close();
                return;
            }

            // Its visible, but possibly not completely drawn
            Refresh();

            // Initialize the last saved session
            SqlSession.Initialize();

            // If the action is to open the DB setup, we can do that now - no need to logon first.
            if (StartupController.StartupAction == StartupAction.OpenDatabaseSetup)
            {
                using (DetailedDatabaseSetupWizard dlg = new DetailedDatabaseSetupWizard())
                {
                    dlg.ShowDialog(this);
                }
            }

            // Check if we have configured a connection to an instance of SQL Server
            else if (!SqlSession.IsConfigured)
            {
                log.InfoFormat("SqlSession not configured; showing welcome.");

                // If they don't complete the database configuration, we get out.  It's possible that at this point SqlSession is actually configured,
                // but they just didn't completely make it through the other stuff like store, carrier, etc. setup.
                if (!OpenDatabaseConfiguration())
                {
                    return;
                }
            }

            // If its still not setup, dont go on
            if (!SqlSession.IsConfigured)
            {
                return;
            }

            // Initiate the logon sequence
            InitiateLogon();
        }

        /// <summary>
        /// Open the appropriate database configuration window based on the current state of the system
        /// </summary>
        private bool OpenDatabaseConfiguration()
        {
            // If we aren't configured at all
            if (!SqlSession.IsConfigured)
            {
                // If we aren't configured and 2012 is supported, open the fast track setup wizard
                if (SqlServerInstaller.IsSqlServer2012Supported)
                {
                    using (SimpleDatabaseSetupWizard wizard = new SimpleDatabaseSetupWizard())
                    {
                        return wizard.ShowDialog(this) == DialogResult.OK;
                    }
                }
                else
                {
                    using (DetailedDatabaseSetupWizard wizard = new DetailedDatabaseSetupWizard())
                    {
                        return wizard.ShowDialog(this) == DialogResult.OK;
                    }
                }
            }
            // Otherwise, we use our normal database setup wizard
            else
            {
                using (DatabaseDetailsDlg dlg = new DatabaseDetailsDlg())
                {
                    dlg.ShowDialog(this);

                    return dlg.DatabaseConfigurationChanged;
                }
            }
        }

        /// <summary>
        /// The window is about to close
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // Make sure we are not in a failure state
            if (ConnectionMonitor.Status != ConnectionMonitorStatus.Normal)
            {
                return;
            }

            // The ribbon is disabled when the window is loading.  Can't close it during that time.
            if (!ribbon.Enabled)
            {
                e.Cancel = true;
                return;
            }

            if (CrashWindow.IsApplicationCrashed)
            {
                return;
            }

            using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("close ShipWorks", this))
            {
                if (!scope.Acquired)
                {
                    e.Cancel = true;
                    return;
                }

                heartBeat.Stop();

                if (UserSession.IsLoggedOn)
                {
                    InitiateLogoff(false);
                }

                UserSession.Reset();
            }
        }

        #endregion

        #region Session

        /// <summary>
        /// Initiate the process of logging on to the system
        /// </summary>
        private void InitiateLogon()
        {
            if (!SqlSession.IsConfigured)
            {
                throw new InvalidOperationException("Cannot initiate log on without a valid SqlSession.");
            }

            ShowBlankUI();

            if (LogonToSqlServer())
            {
                LogonToShipWorks();
            }
            else
            {
                UserSession.Reset();
            }
        }

        /// <summary>
        /// Initiate the process of logging off the system.
        /// </summary>
        private void InitiateLogoff(bool clearRememberMe)
        {
            // Don't need a scope if we're already in one
            using (ConnectionSensitiveScope scope = (ConnectionSensitiveScope.IsActive ? null : new ConnectionSensitiveScope("log off", this)))
            {
                if (scope != null && !scope.Acquired)
                {
                    return;
                }

                SaveCurrentUserSettings();

                UserSession.Logoff(clearRememberMe);
            }

            // Can't do anything when logged off
            ShowBlankUI();
        }

        /// <summary>
        /// Open the window for logging on to SQL Server
        /// </summary>
        private bool LogonToSqlServer()
        {
            // If we are here b\c MSDE was uinstalled, but SQL 08 isn't ready yet, we need to force the user back into the Database Upgrade window when they come back.
            // If we didn't do that, then they wouldnt be able to get back in b\c normally it requires a successfully connection (which they can't have now, b\c MSDE is
            // uinstalled).
            if (SqlServerInstaller.IsMsdeMigrationInProgress)
            {
                log.InfoFormat("Forcing Database Upgrade window open to to MSDE migration file existing.");

                using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("update the database", this))
                {
                    if (!scope.Acquired)
                    {
                        return false;
                    }

                    if (!DatabaseUpdateWizard.Run(this))
                    {
                        return false;
                    }
                }
            }

            bool canConnect = SqlSession.Current.CanConnect();

            // If we couldn't connect, see if it's b\c another ShipWorks is upgrading or restoring
            if (!canConnect)
            {
                try
                {
                    SqlSession master = new SqlSession(SqlSession.Current);
                    master.Configuration.DatabaseName = "master";

                    using (SqlConnection testConnection = new SqlConnection(master.Configuration.GetConnectionString()))
                    {
                        testConnection.Open();

                        if (SqlUtility.IsSingleUser(testConnection, SqlSession.Current.Configuration.DatabaseName))
                        {
                            using (SingleUserModeDlg dlg = new SingleUserModeDlg())
                            {
                                dlg.ShowDialog(this);
                            }

                            return false;
                        }
                    }
                }
                catch (Exception textEx)
                {
                    log.Error("Could not login to master to try to check for SINGLE_USER mode", textEx);
                }
            }

            if (!canConnect || SqlSession.Current.DetermineMissingPermissions(SqlSessionPermissionSet.Standard).Count > 0)
            {
                using (DatabaseLogonDlg dlg = new DatabaseLogonDlg())
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return false;
                    }
                }
            }

            // In case we bombed while in single user mode, make sure we are always back to multi.  I guess this could screw you
            // if you were an admin who was purposely trying to put it in single mode and keep it there... but why would you do that?
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                try
                {
                    SqlUtility.SetMultiUser(con);
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("permission"))
                    {
                        // If they don't have permissions, then just carry on - its not a deal breaker
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Check that the database version is current
            if (!CheckDatabaseVersion())
            {
                return false;
            }

            // Check that CLR is enabled on the server
            if (!SqlSession.Current.IsClrEnabled())
            {
                using (NeedEnableClr dlg = new NeedEnableClr(SqlSession.Current))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return false;
                    }
                }
            }

            log.InfoFormat("Logon to SQL Server: Success");

            // Reset the heartbeat
            heartBeat.Reset();

            // Now we know we have a connection to a current database.  Initialize a new session.
            UserSession.InitializeForCurrentDatabase();

            // Has to be at least one admin user in the system
            if (!UserUtility.HasAdminUsers())
            {
                log.Error("No administrator users exist.");

                MessageHelper.ShowError(this, "No administrator users exist.  Please select 'Setup Database' from the main application menu.");
                return false;
            }

            // If WorldShip has been configured start monitoring for shipments to import
            if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.UpsWorldShip))
            {
                WorldShipImportMonitor.Start();
            }

            return true;
        }

        /// <summary>
        /// Log on to ShipWorks as a ShipWorks user.
        /// </summary>
        private void LogonToShipWorks()
        {
            UserManager.InitializeForCurrentUser();

            // May already be logged on
            if (!UserSession.IsLoggedOn)
            {
                if (!UserSession.LogonLastUser())
                {
                    using (LogonDlg dlg = new LogonDlg())
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                    }
                }
            }

            log.InfoFormat("Logon to ShipWorks: Success");

            // Load the display
            LoadCurrentUserDisplaySettings();

            // Do as much of the data load as possible in a background thread
            try
            {
                ManualResetEvent dataLoadedEvent = new ManualResetEvent(false);
                ribbon.Enabled = false;

                ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(LogonToShipWorksAsyncLoad, "loading"), dataLoadedEvent);

                // This would normally be bad news, but we disabled the UI, so it effectively just keeps the painting responsive.
                while (!dataLoadedEvent.WaitOne(TimeSpan.FromSeconds(.1), false))
                {
                    Application.DoEvents();
                    Cursor.Current = Cursors.AppStarting;
                }

                dataLoadedEvent.Close();
            }
            finally
            {
                ribbon.Enabled = true;
            }

            // This means the app is going to crash...
            if (!logonAsyncLoadSuccess)
            {
                return;
            }
            
            // If there are no stores, we need to make sure one is added before continuing
            if (StoreManager.GetDatabaseStoreCount() == 0)
            {
                if (!AddStoreWizard.RunWizard(this))
                {
                    UserSession.Logoff(false);
                    UserSession.Reset();

                    ShowBlankUI();

                    return;
                }
            }

            ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(LogonToShipWorksAsyncGetLicenseStatus, "checking license status"));

            UserEntity user = UserSession.User;

            // Initialize the grid
            gridControl.InitializeForTarget(FilterTarget.Orders, contextMenuOrderGrid, contextOrderCopy);
            gridControl.InitializeForTarget(FilterTarget.Customers, contextMenuCustomerGrid, contextCustomerCopy);

            filterTree.LoadLayouts(FilterTarget.Orders, FilterTarget.Customers);
            filterTree.ApplyFolderState(new FolderExpansionState(user.Settings.FilterExpandedFolders));

            // Update the custom actions UI.  Has to come before applying the layout, so the QAT can pickup the buttons
            UpdateCustomButtonsActionsUI();

            // We can now show the normal UI
            ApplyCurrentUserLayout();

            // Select the active filter
            SelectInitialFilter(user.Settings);

            log.InfoFormat("UI shown");

            // Start the dashboard.  Has to be before updating store depending UI - as that affects dashboard display.
            DashboardManager.OpenDashboard();

            // Get all new\edited templates are installed
            BuiltinTemplates.UpdateTemplates(this);

            // Update all UI items that are related to the current stores
            UpdateStoreDependentUI();

            // Update the Detail View UI
            UpdateDetailViewSettingsUI();

            // Check the disk usage of SQL server to warn the user if they are about out
            CheckDatabaseDiskUsage();

            // Start the heartbeat
            heartBeat.Start();

            // Start auto downloading immediately
            DownloadManager.StartAutoDownloadIfNeeded(true);

            // Then, if we are downloading any stores for the very very first time, auto-show the progress
            if (StoreManager.GetLastDownloadTimes().Any(pair => pair.Value == null && DownloadManager.IsDownloading(pair.Key)))
            {
                ShowDownloadProgress();
            }
        }

        /// <summary>
        /// Check the disk usage of SQL server to warn the user if they are about out
        /// </summary>
        private void CheckDatabaseDiskUsage()
        {
            double gigsBeforeWarn = 2;

            long spaceRemaining = SqlDiskUsage.SpaceRemaining;
            if (spaceRemaining != -1 && spaceRemaining < gigsBeforeWarn * 1024 * 1024 * 1024)
            {
                int gbLimit = SqlDiskUsage.SizeLimitGB;

                DashboardManager.ShowLocalMessage("DatabaseSize",
                    DashboardMessageImageType.Warning,
                    "Database Size",
                    string.Format("You are using {0} of your {1} GB database size limit.", StringUtility.FormatByteCount(SqlDiskUsage.TotalUsage), gbLimit),
                    new DashboardActionMethod("[link]Help[/link]", () =>
                    {
                        MessageHelper.ShowInformation(this,
                            string.Format(
                                "The Express edition of Microsoft SQL Server has a {0} GB database size limit.\n\n" +
                                "Please call us at 1-800-95-APPTIVE to discuss potential solutions.",
                                gbLimit));
                    }));
            }
        }

        /// <summary>
        /// Load initial data when logging in in a background thread.  Its not perfect, but it does keep it a bit more responsive
        /// </summary>
        private void LogonToShipWorksAsyncLoad(object state)
        {
            ManualResetEvent loadedEvent = (ManualResetEvent) state;
            logonAsyncLoadSuccess = false;

            try
            {
                UserSession.InitializeForCurrentSession();

                logonAsyncLoadSuccess = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed in logon async load.", ex);

                throw;
            }
            finally
            {
                loadedEvent.Set();
            }
        }

        /// <summary>
        /// When we logon, we asynchronously get the license status of each license and update edition information.
        /// </summary>
        private void LogonToShipWorksAsyncGetLicenseStatus(object state)
        {
            bool editionChanged = false;

            // Update our edition for each store.  Eventually this will also be where we log with tango the sw version being used and maybe other things
            foreach (StoreEntity store in StoreManager.GetAllStores())
            {
                try
                {
                    ShipWorksLicense license = new ShipWorksLicense(store.License);
                    if (!license.IsTrial)
                    {
                        LicenseAccountDetail accountDetail = TangoWebClient.GetLicenseStatus(store.License, store);

                        if (accountDetail.ActivationState == LicenseActivationState.Active ||
                            accountDetail.ActivationState == LicenseActivationState.ActiveElsewhere ||
                            accountDetail.ActivationState == LicenseActivationState.ActiveNowhere)
                        {
                            editionChanged |= EditionManager.UpdateStoreEdition(store, accountDetail.Edition);
                        }
                    }
                    else
                    {
                        TrialDetail trialDetail = TangoWebClient.GetTrial(store);

                        editionChanged |= EditionManager.UpdateStoreEdition(store, trialDetail.Edition);
                    }
                }
                catch (ShipWorksLicenseException ex)
                {
                    log.Warn(string.Format("Could not check status of license {0}", store.License), ex);
                }
                catch (TangoException ex)
                {
                    log.Warn(string.Format("Could not check status of license {0}", store.License), ex);
                }
            }

            if (editionChanged)
            {
                ForceHeartbeat();
            }
        }

        /// <summary>
        /// Check that the version of the connected database is what we need.
        /// </summary>
        private bool CheckDatabaseVersion()
        {
            SchemaVersion databaseSchemaVersion;
            SchemaVersion softwareSchemaVersion = (new SchemaVersionManager()).GetRequiredSchemaVersion();

            try
            {
                databaseSchemaVersion = SqlSchemaUpdater.GetDatabaseSchemaVersion();
            }
            catch (InvalidShipWorksDatabaseException ex)
            {
                log.Error("CheckDatabaseVersion failed on GetInstalledDbVersion.", ex);

                MessageHelper.ShowError(this, "The database is not a valid ShipWorks database.");
                return false;
            }

            log.InfoFormat("CheckDatabaseVersion: Installed: {0}, Required {1}", databaseSchemaVersion, softwareSchemaVersion);

            // See if it needs upgraded
            SchemaVersionComparisonResult softwareSchemaComparedToDatabaseSchema = databaseSchemaVersion.Compare(softwareSchemaVersion);

            if (softwareSchemaComparedToDatabaseSchema==SchemaVersionComparisonResult.Unknown)
            {
                log.Error("Upgrade path not found.");

                MessageHelper.ShowError(this, "Cannot upgrade database.");
                return false;
            }

            if (softwareSchemaComparedToDatabaseSchema == SchemaVersionComparisonResult.Newer || !SqlSession.Current.IsSqlServer2008OrLater() || MigrationController.IsMigrationInProgress())
            {
                using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("update the database", this))
                {
                    if (!scope.Acquired)
                    {
                        return false;
                    }

                    if (DatabaseUpdateWizard.Run(this))
                    {
                        // If the upgrade went OK, we still need to check that the current user has adequate permissions to work in the restored datbase
                        if (!SqlSession.Current.CheckPermissions(SqlSessionPermissionSet.Standard, this))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            
            // See if its too new
            if (softwareSchemaComparedToDatabaseSchema == SchemaVersionComparisonResult.Older)
            {
                MessageHelper.ShowMessage(this,
                    "The version of your ShipWorks database is newer than this version\n" +
                    "of ShipWorks.\n\n" +
                    "You will need to upgrade ShipWorks before you can continue.");

                return false;
            }

            return true;
        }
        
        #endregion

        #region GUI \ Layout

        /// <summary>
        /// Show the UI for when there is no connection or stores
        /// </summary>
        private void ShowBlankUI()
        {
            // Stop the update heartbeat
            heartBeat.Stop();

            ApplicationText = "";

            // Hide all dock windows.  Hide them first so they don't attempt to save when the filter changes (due to the tree being cleared)
            foreach (DockControl control in sandDockManager.GetDockControls())
            {
                control.Close();
            }

            // Grid has to be cleared first - otherwise the current settings will be saved in response to the filtertree clearing,
            // and notifying the grid that the selected filter changed.
            gridControl.Reset();
            filterTree.Clear();

            ribbon.Tabs.Clear();
            ribbon.ToolBar = null;

            // Hide all status bar items
            statusBar.MainStrip.Visible = false;

            panelDockingArea.Visible = false;
            DashboardManager.CloseDashboard();

            log.InfoFormat("UI hidden");
        }

        /// <summary>
        /// Show the normal UI
        /// </summary>
        private void ApplyCurrentUserLayout()
        {
            UserSettingsEntity settings = UserSession.User.Settings;

            // Show the tool bar
            ribbon.ToolBar = quickAccessToolBar;

            // Add back in the tabs
            foreach (RibbonTab tab in ribbonTabs)
            {
                ribbon.Tabs.Add(tab);

                // Preserve order
                tab.SendToBack();
            }

            // Show all status bar strips
            statusBar.MainStrip.Visible = true;

            // Load the user's saved state
            windowLayoutProvider.LoadLayout(settings.WindowLayout);

            // Load the user's saved menu settings
            gridMenuLayoutProvider.LoadLayout(settings.GridMenuLayout);

            // Initialize all the panels
            foreach (DockingPanelContentHolder holder in GetDockingPanelContentHolders())
            {
                holder.InitializeForCurrentUser();
            }

            // Show main UI areas
            panelDockingArea.Visible = true;

            // Make the filter tree
            filterTree.Focus();
        }

        /// <summary>
        /// Load all of the display settings for the current user
        /// </summary>
        private void LoadCurrentUserDisplaySettings()
        {
            UserEntity user = UserSession.User;

            // Update title
            ApplicationText = user.Username;

            // Load display from user settings
            ShipWorksDisplay.ColorScheme = (ColorScheme) user.Settings.DisplayColorScheme;
            ShipWorksDisplay.HideInSystemTray = user.Settings.DisplaySystemTray;

            ApplyDisplaySettings();
        }

        /// <summary>
        /// Save the layout for the current user
        /// </summary>
        private void SaveCurrentUserSettings()
        {
            if (!UserSession.IsLoggedOn)
            {
                throw new InvalidOperationException("Cannot save the current user layout when no user is logged on.");
            }

            Cursor.Current = Cursors.WaitCursor;

            UserSettingsEntity settings = UserSession.User.Settings;

            // Save the defaults for when no user is logged in
            ShipWorksDisplay.SaveDefault();

            // Save the panel settings
            foreach (DockingPanelContentHolder holder in this.GetDockingPanelContentHolders())
            {
                holder.SaveState();
            }

            // Save display settings
            settings.DisplayColorScheme = (int) ShipWorksDisplay.ColorScheme;
            settings.DisplaySystemTray = ShipWorksDisplay.HideInSystemTray;

            // Save the layout
            settings.WindowLayout = windowLayoutProvider.SerializeLayout();
            settings.GridMenuLayout = gridMenuLayoutProvider.SerializeLayout();

            // Save the filter expand collapse state
            settings.FilterExpandedFolders = filterTree.GetFolderState().GetState();

            // Save the last active filter
            if (gridControl.IsSearchActive)
            {
                settings.FilterLastActive = searchRestoreFilterNodeID;
            }
            else
            {
                settings.FilterLastActive = gridControl.ActiveFilterNode != null ? gridControl.ActiveFilterNode.FilterNodeID : 0;
            }

            // Save the grid column state
            gridControl.SaveGridColumnState();

            // Save the settings
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(settings);
            }
        }

        /// <summary>
        /// Apply the active display settings
        /// </summary>
        private void ApplyDisplaySettings()
        {
            // Apply the color scheme to ribbon
            ribbonManager.Renderer.Dispose();
            ribbonManager.Renderer = AppearanceHelper.CreateRibbonRenderer();

            // Apply color scheme to dock
            sandDockManager.Renderer.Dispose();
            TD.SandDock.Rendering.Office2007Renderer dockRenderer = AppearanceHelper.CreateDockRenderer();
            sandDockManager.Renderer = dockRenderer;

            // Apply color scheme to background
            panelDockingArea.BackColor = ribbonManager.Renderer.ColorTable.RibbonTabStripBackground;
            this.BackColor = panelDockingArea.BackColor;

            // Apply themne to grids
            gridControl.ApplyDisplaySettings();

            // Apply krypton scheme
            kryptonManager.GlobalPaletteMode = AppearanceHelper.GetKryptonPaletteMode();

            ApplySystemTrayProperties();
        }

        /// <summary>
        /// The user has double-clicked the tray icon, we need to show ourselves
        /// </summary>
        private void OnDoubleClickTrayIcon(object sender, EventArgs e)
        {
            RestoreFromSystemTray();
        }

        /// <summary>
        /// User has selected Open ShipWorks from the task tray menu
        /// </summary>
        private void OnTaskTrayMenuOpenShipWorks(object sender, EventArgs e)
        {
            RestoreFromSystemTray();
        }

        /// <summary>
        /// Restore shipworks from the tray
        /// </summary>
        private void RestoreFromSystemTray()
        {
            // This has to be done first, or floating docking panels don't restore
            ShowInTaskbar = true;
            notifyIcon.Visible = false;

            NativeMethods.ShowWindow(Handle, NativeMethods.SW_RESTORE);
        }

        /// <summary>
        /// The window is being resized
        /// </summary>
        private void OnResize(object sender, EventArgs e)
        {
            ApplySystemTrayProperties();
        }

        /// <summary>
        /// Apply the properites that need to be set for hiding in the system tray based on our current state
        /// </summary>
        private void ApplySystemTrayProperties()
        {
            bool isInTray = ShipWorksDisplay.HideInSystemTray && (WindowState == FormWindowState.Minimized);

            ShowInTaskbar = !isInTray;
            notifyIcon.Visible = isInTray;
        }

        /// <summary>
        /// Update the UI state that depends on the currently available stores and their properties
        /// </summary>
        private void UpdateStoreDependentUI()
        {
            // Security info is store-dependent
            if (UserSession.Security != null)
            {
                UserSession.Security.ClearPermissionCache();
            }

            // Update the download button
            UpdateDownloadButtonForStores();

            // Update the dashboard for new\removed trials
            DashboardManager.UpdateStoreDependentItems();

            // Update edition-based UI from stores
            EditionManager.UpdateRestrictions();
            
            // Update the state of the ui for the update online commands
            buttonUpdateOnline.Visible = OnlineUpdateCommandProvider.HasOnlineUpdateCommands();
            gridMenuLayoutProvider.UpdateStoreDependentUI();

            // The available columns depend on the store types that exist
            FilterNodeColumnManager.InitializeForCurrentSession();
            gridControl.ReloadGridColumns();

            // Update the panels based on the current store set
            foreach (DockingPanelContentHolder holder in GetDockingPanelContentHolders())
            {
                holder.UpdateStoreDependentUI();
            }

            // Update the availabilty of ribbon items based on security
            ribbonSecurityProvider.UpdateSecurityUI();
        }

        /// <summary>
        /// Apply the given set of MenuCommands to the given menuitem and ribbon poup
        /// </summary>
        private void ApplyMenuCommands(List<MenuCommand> commands, ToolStripMenuItem menuItem, Popup ribbonPopup, EventHandler actionHandler)
        {
            menuItem.DropDownItems.Clear();
            ribbonPopup.Items.Clear();
            
            // Update available local status options
            menuItem.DropDownItems.AddRange(MenuCommandConverter.ToToolStripItems(commands, actionHandler));
            ribbonPopup.Items.Add(MenuCommandConverter.ToRibbonMenu(commands, actionHandler));
        }

        /// <summary>
        /// Do the one-time initial prep-work for edition managed UI
        /// </summary>
        private void PrepareEditionManagedUI()
        {
            editionGuiHelper.RegisterElement(meuItemNewFilter, EditionFeature.FilterLimit, () => FilterLayoutContext.Current == null ? 0 : FilterLayoutContext.Current.GetTopLevelFilterCount());
            editionGuiHelper.RegisterElement(contextCustomerNewOrder, EditionFeature.AddOrderCustomer);
            editionGuiHelper.RegisterElement(buttonNewOrder, EditionFeature.AddOrderCustomer);
            editionGuiHelper.RegisterElement(buttonNewCustomer, EditionFeature.AddOrderCustomer);
            editionGuiHelper.RegisterElement(buttonEndiciaSCAN, EditionFeature.EndiciaScanForm);
        }

        /// <summary>
        /// Update all UI that depends on the current grid selection
        /// </summary>
        private void UpdateSelectionDependentUI()
        {
            // If the user is not logged in, forget it
            if (!UserSession.IsLoggedOn)
            {
                return;
            }

            UpdateStatusBar();
            UpdateComandState();
            UpdatePanelState();

            ribbonSecurityProvider.UpdateSecurityUI();
        }

        /// <summary>
        /// Update the contents of the status bar
        /// </summary>
        private void UpdateStatusBar()
        {
            labelStatusTotal.Text = string.Format("{0}: {1:#,##0}", EnumHelper.GetDescription(gridControl.ActiveFilterTarget), gridControl.TotalCount);
            labelStatusSelected.Text = string.Format("Selected: {0:#,##0}", gridControl.Selection.Count);
        }

        /// <summary>
        /// Update the state of the ribbon buttons based on the current selection
        /// </summary>
        private void UpdateComandState()
        {
            selectionDependentEnabler.UpdateCommandState(gridControl.Selection.Count, gridControl.ActiveFilterTarget);
        }

        /// <summary>
        /// Update the state of the panels based on the current selection
        /// </summary>
        private void UpdatePanelState()
        {
            foreach (DockControl dockControl in sandDockManager.GetDockControls().Where(d => d.Controls.Count == 1))
            {
                UpdatePanelState(dockControl);
            }
        }

        /// <summary>
        /// Update the state of the panel content for the given dock control, only if it contains a panel, and only if it's open.
        /// </summary>
        private void UpdatePanelState(DockControl dockControl)
        {
            // This function can get called as panels are activating.  Activation can be changing as we are closing them during logoff,
            // so we have to make sure we're logged on or updating would crash.
            if (!UserSession.IsLoggedOn)
            {
                return;
            }

            DockingPanelContentHolder holder = dockControl.Controls[0] as DockingPanelContentHolder;
            if (holder != null && dockControl.IsOpen)
            {
                // This happens to often to use GetOrderedSelectdKeys. If ordering becomes important, we'll need to improve
                // the performance of that somehow for massive selections where there is virtual selection.
                holder.UpdateContent(gridControl.ActiveFilterTarget, gridControl.Selection);
            }
        }

        /// <summary>
        /// The popup window for displaying panels is opening
        /// </summary>
        private void OnBeforePopupShowPanels(object sender, BeforePopupEventArgs e)
        {
            foreach (SandMenuItem menuItem in menuShowPanels.Items)
            {
                DockControl dockControl = (DockControl) menuItem.Tag;

                // Change this to IsOpen if the behavior requirement changes so that it would be 
                // checked only if its actually visible... So like if it was a non ative tab, or minimized,
                // it wouldnt get a check.  This way, it gets a check if its on the screen at all.
                menuItem.Checked = dockControl.DockSituation != DockSituation.None;
            }
        }

        /// <summary>
        /// Execute an invoked menu command
        /// </summary>
        void OnExecuteMenuCommand(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            MenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

            // Execute the command
            command.ExecuteAsync(this, gridControl.Selection.OrderedKeys, OnAsyncMenuCommandCompleted);
        }

        /// <summary>
        /// Execute an invoked update online command
        /// </summary>
        void OnExecuteUpdateOnlineCommand(object sender, EventArgs e)
        {
            MenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

            // Execute the command
            onlineUpdateCommandProvider.ExecuteCommandAsync(command, this, gridControl.Selection.OrderedKeys, OnAsyncMenuCommandCompleted);
        }

        /// <summary>
        /// Called when an async menu command operation has completed
        /// </summary>
        private void OnAsyncMenuCommandCompleted(object sender, MenuCommandCompleteEventArgs e)
        {
            ForceHeartbeat(HeartbeatOptions.ForceReload | HeartbeatOptions.ChangesExpected);

            e.ShowMessage(this);
        }

        /// <summary>
        /// Get the Popup that will be displayed for the given SandRibbon ComboBox
        /// </summary>
        private Popup GetSandRibbonComboPopup(SandComboBox comboBox)
        {
            SandLabel label = new SandLabel();
            comboBox.Items.Add(label);

            Popup popup = label.ParentItem.ParentItem as Popup;

            comboBox.Items.Remove(label);

            return popup;
        }

        /// <summary>
        /// The online update ribbon popup is opening
        /// </summary>
        private void OnUpdateOnlineRibbonOpening(object sender, BeforePopupEventArgs e)
        {
            UpdateOnlineUpdateCommands();
        }
        
        /// <summary>
        /// The online update context menu is opening
        /// </summary>
        private void OnUpdateOnlineMenuOpening(object sender, EventArgs e)
        {
            UpdateOnlineUpdateCommands();
        }

        /// <summary>
        /// Update the available set of online update commands
        /// </summary>
        private void UpdateOnlineUpdateCommands()
        {
            // Update the update online options
            List<MenuCommand> updateOnlineCommands = onlineUpdateCommandProvider.CreateOnlineUpdateCommands(gridControl.Selection.Keys);

            // Update the ui to display the commands
            ApplyMenuCommands(updateOnlineCommands, contextOrderOnlineUpdate, popupUpdateOnline, OnExecuteUpdateOnlineCommand);
        }

        /// <summary>
        /// The local status ribbon popup is opening
        /// </summary>
        private void OnLocalStatusRibbonOpening(object sender, BeforePopupEventArgs e)
        {
            UpdateLocalStatusCommands();
        }

        /// <summary>
        /// The local status context menu is opening
        /// </summary>
        private void OnLocalStatusMenuOpening(object sender, EventArgs e)
        {
            UpdateLocalStatusCommands();
        }

        /// <summary>
        /// Update the available local status commands
        /// </summary>
        private void UpdateLocalStatusCommands()
        {
            ApplyMenuCommands(
                StatusPresetCommands.CreateMenuCommands(StatusPresetTarget.Order, gridControl.SelectedStoreKeys, true),
                contextOrderLocalStatus,
                popupLocalStatus,
                OnExecuteMenuCommand);
        }

        /// <summary>
        /// Update the UI that's based on user initiated actions
        /// </summary>
        private void UpdateCustomButtonsActionsUI()
        {
            string ribbonChunkName = "Custom Actions";

            // Get the actions for all custom buttons that need to be added
            var enabledActions = ActionManager.Actions.Where(a => a.TriggerType == (int) ActionTriggerType.UserInitiated && a.Enabled).Select(a => new { Action = a, Trigger = (UserInitiatedTrigger) ActionManager.LoadTrigger(a) });

            // Get the actions for the ribbon
            var ribbonActions = enabledActions.Where(a => a.Trigger.ShowOnRibbon);

            // Get the ribbon chunk that holds our action buttons
            var actionChunk = ribbonTabHome.Chunks.Cast<RibbonChunk>().Where(c => c.Text == ribbonChunkName).SingleOrDefault();

            // Maybe we need to remove it
            if (actionChunk != null)
            {
                // Remove any buttons that are no longer around
                foreach (SandButton existingButton in actionChunk.Items.OfType<SandButton>().ToList())
                {
                    if (!ribbonActions.Any(a => a.Trigger.Guid == existingButton.Guid))
                    {
                        existingButton.Dispose();
                    }
                }

                // If there are no actions, kill the chunk
                if (!ribbonActions.Any())
                {
                    ribbonTabHome.Chunks.Remove(actionChunk);
                }
            }

            // See if there are any to show on the ribbon
            if (ribbonActions.Any())
            {
                // Maybe we need to create it
                if (actionChunk == null)
                {
                    actionChunk = new RibbonChunk() { Text = ribbonChunkName, FurtherOptions = false, ItemJustification = ItemJustification.Near };
                    ribbonTabHome.Chunks.Add(actionChunk);
                }

                // Add all the buttons
                foreach (var action in ribbonActions)
                {
                    // See if we can find the existing button
                    SandButton button = actionChunk.Items.OfType<SandButton>().FirstOrDefault(b => b.Guid == action.Trigger.Guid);

                    // If it doesn't exist, create it
                    if (button == null)
                    {
                        button = new SandButton();
                        button.Guid = action.Trigger.Guid;
                        button.Tag = action.Action.ActionID;
                        button.TextContentRelation = TextContentRelation.Underneath;
                        button.Activate += OnCustomActionButton;

                        actionChunk.Items.Add(button);
                    }

                    // Update the properties
                    button.Text = string.Join("\r\n", StringUtility.SplitLines(action.Action.Name, 20, 2));
                    button.Padding = button.Text.Contains("\r\n") ? new WidgetEdges(8, 2, 8, 2) : new WidgetEdges(3, 2, 4, 14);
                    button.Image = action.Trigger.LoadImage();

                    // Configure selection requirements
                    if (action.Trigger.SelectionRequirement != UserInitiatedSelectionRequirement.None)
                    {
                        selectionDependentEnabler.SetEnabledWhen(button, (action.Trigger.SelectionRequirement == UserInitiatedSelectionRequirement.Orders) ? SelectionDependentType.OneOrMoreOrders : SelectionDependentType.OneOrMoreCustomers);
                    }
                    else
                    {
                        selectionDependentEnabler.SetEnabledWhen(button, SelectionDependentType.Ignore);
                    }
                }

                UpdateSelectionDependentUI();
            }

            // Get the actions that should be displayed in the order menu
            var orderActions = enabledActions.Where(a => a.Trigger.ShowOnOrdersMenu);

            // Clear any that are in there now
            contextOrderCustomActions.DropDownItems.Clear();

            // Add in the new ones
            foreach (var action in orderActions)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(action.Action.Name, action.Trigger.LoadImage());
                menuItem.Tag = action.Action.ActionID;
                menuItem.Click += OnCustomActionMenu;

                contextOrderCustomActions.DropDownItems.Add(menuItem);
            }

            // Get the actions that should be displayed in the customer menu
            var customerActions = enabledActions.Where(a => a.Trigger.ShowOnCustomersMenu);

            // Clear any that are in there now
            contextCustomerCustomActions.DropDownItems.Clear();

            // Add in the new ones
            foreach (var action in customerActions)
            {
                ToolStripMenuItem menuItem = new ToolStripMenuItem(action.Action.Name, action.Trigger.LoadImage());
                menuItem.Tag = action.Action.ActionID;
                menuItem.Click += OnCustomActionMenu;

                contextCustomerCustomActions.DropDownItems.Add(menuItem);
            }

            // Update the menu visibility \ availability
            gridMenuLayoutProvider.UpdateUserInitiatedActionDependentUI();
        }

        /// <summary>
        /// User has clicked a custom action button
        /// </summary>
        void OnCustomActionButton(object sender, EventArgs e)
        {
            long actionID = (long) ((SandButton) sender).Tag;

            DispatchUserInitiatedAction(actionID);
        }

        /// <summary>
        /// User has clicked a custom action menu
        /// </summary>
        void OnCustomActionMenu(object sender, EventArgs e)
        {
            long actionID = (long) ((ToolStripMenuItem) sender).Tag;

            DispatchUserInitiatedAction(actionID);
        }

        /// <summary>
        /// Dispatch a user initiated action for the given ActionID
        /// </summary>
        private void DispatchUserInitiatedAction(long actionID)
        {
            Cursor.Current = Cursors.WaitCursor;

            ActionDispatcher.DispatchUserInitiated(actionID, gridControl.Selection.OrderedKeys);
        }

       #endregion

        #region App Menu

        /// <summary>
        /// The applicatino menu is about to be shown
        /// </summary>
        private void OnBeforePopupApplicationMenu(object sender, BeforePopupEventArgs e)
        {
            UpdateLoginLogoffMenu();

            // Only show backup \ restore if logged on
            if (!UserSession.IsLoggedOn)
            {
                mainMenuItemBackupDatabase.Visible = false;
            }
        }

        /// <summary>
        /// Update the display of the login\logoff menu
        /// </summary>
        private void UpdateLoginLogoffMenu()
        {
            // Cant login if the db is not configured
            mainMenuLogon.Visible = SqlSession.IsConfigured;

            if (SqlSession.IsConfigured)
            {
                if (UserSession.IsLoggedOn)
                {
                    mainMenuLogon.Text = "&Log Off";
                }
                else
                {
                    mainMenuLogon.Text = "&Log On";
                }
            }
        }

        /// <summary>
        /// User wants to logon \ logoff
        /// </summary>
        private void OnLogonLogoff(object sender, EventArgs e)
        {
            if (!UserSession.IsLoggedOn)
            {
                InitiateLogon();
            }
            else
            {
                InitiateLogoff(true);
            }
        }

        /// <summary>
        /// Open the database configuration window
        /// </summary>
        private void OnDatabaseConfiguration(object sender, EventArgs e)
        {
            // Indicates the user made it 100% succesfully through the database and setup wizards
            bool configurationComplete = false;

            // Indicates if the database changed in any way (which database, restored database, whatever)
            bool databaseChanged = false;

            using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("change database settings", this))
            {
                if (!scope.Acquired)
                {
                    return;
                } 
                
                // In case we end up changing databases, save before that happens
                if (UserSession.IsLoggedOn)
                {
                    SaveCurrentUserSettings();
                }

                // Open the database configuration window
                configurationComplete = OpenDatabaseConfiguration();

                // Now regardless of if that was succesful, see if it altered the database.  Some things can't be rolled back even if the user canceled.
                databaseChanged = scope.DatabaseChanged;

                // If the configuration is complete, or the database changed in any way...
                if (configurationComplete || databaseChanged)
                {
                    // If they were in the middle of upgrading MSDE to 08... and had to reboot, or whatever.  But then chose Setup Database and finished successfully
                    // then we need to forget about that MSDE upgrade that was in the middle of happening, the user has moved on.
                    SqlServerInstaller.CancelMsdeMigrationInProgress();

                    // This makes sure that when we exit the context scope, we don't still briefly look logged in to constantly 
                    // running background threads.  Being logged in asserts that the schema is correct - and at this point, we just connected to a random database, so we don't know that.
                    // We can't use LogOff here, because that tries to audit the logoff.  We are now connected to a different database, so it wouldn't make any sense to do that audit.
                    UserSession.Reset();
                }
            }

            // If the user completed the configuration, kick-off the Logon procedure to get the UI up-and-running for the new user and data
            if (configurationComplete)
            {
                // The Database setup can sometimes exit due to needing the machine to reboot before SW can continue.  
                // If this is the case configurationComplete would report true, but the SQL Session won't be setup yet, and we can't initiate logon.
                if (SqlSession.IsConfigured)
                {
                    InitiateLogon();
                }
            }
            // If the configuration did not complete, but the database changed in someway, that means the user got as far as selecting a new, or restoring a database - but then canceled
            // after the point that could have been rolled back.  So at this point, the user just clicked cancel, but is connected to a different database.  We don't want to initiate
            // logon, as that would prompt them to complete setup, which they just cancel.  But we can't do nothing, because the UI is showing them their old data.  So we have to just blank
            // out the UI, and they can try to logon to start over.
            else if (databaseChanged)
            {
                ShowBlankUI();
            }
        }

        /// <summary>
        /// Create a ShpWorks backup
        /// </summary>
        private void OnBackupShipWorks(object sender, EventArgs e)
        {
            if (!SqlSession.Current.IsLocalServer())
            {
                MessageHelper.ShowInformation(this, "A ShipWorks backup can only be made from the computer that is running SQL Server.");
                return;
            }

            using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("create a backup", this))
            {
                if (!scope.Acquired)
                {
                    return;
                }

                using (DatabaseBackupDlg dlg = new DatabaseBackupDlg(UserSession.User))
                {
                    dlg.ShowDialog(this);
                }
            }
        }

        /// <summary>
        /// Restore a database from backup
        /// </summary>
        private void OnRestoreBackup(object sender, EventArgs e)
        {
            if (!SqlSession.Current.IsLocalServer())
            {
                MessageHelper.ShowInformation(this, "A ShipWorks restore can only be done from the computer that is running SQL Server.");
                return;
            }

            bool needLogon = false;

            using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("restore a backup", this))
            {
                if (!scope.Acquired)
                {
                    return;
                }

                // In case we end up changing databases, save before that happens
                if (UserSession.IsLoggedOn)
                {
                    SaveCurrentUserSettings();
                }

                using (DatabaseRestoreDlg dlg = new DatabaseRestoreDlg(UserSession.User))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK || scope.DatabaseChanged)
                    {
                        needLogon = true;
                    }
                }
            }

            // This is down here so its outside of the scope
            if (needLogon)
            {
                InitiateLogon();
            }
        }

        /// <summary>
        /// Show the ShipWorks options window
        /// </summary>
        private void OnShowOptions(object sender, EventArgs e)
        {
            // Create the data structure to send to options
            ShipWorksOptionsData data = new ShipWorksOptionsData(ribbon.ToolBarPosition == QuickAccessPosition.Below, ribbon.Minimized);

            using (ShipWorksOptions dlg = new ShipWorksOptions(data))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    ApplyDisplaySettings();

                    // Apply ribbon settings
                    ribbon.ToolBarPosition = data.ShowQatBelowRibbon ? QuickAccessPosition.Below : QuickAccessPosition.Above;
                    ribbon.Minimized = data.MinimizeRibbon;
                }
            }
        }

        /// <summary>
        /// Open the window for connecting to interapptive remote assistance
        /// </summary>
        private void OnRemoteAssistance(object sender, EventArgs e)
        {
            using (RemoteAssistanceDlg dlg = new RemoteAssistanceDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Open the ShipWorks support forum
        /// </summary>
        private void OnSupportForum(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/support", this);
        }

        /// <summary>
        /// Help button clicked
        /// </summary>
        private void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            OnViewHelp(sender, null);
        }

        /// <summary>
        /// View the ShipWorks help "file"
        /// </summary>
        private void OnViewHelp(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.interapptive.com/shipworks/help", this);
        }

        /// <summary>
        /// View the ShipWorks about box
        /// </summary>
        private void OnAboutShipWorks(object sender, EventArgs e)
        {
            using (ShipWorksAboutDlg dlg = new ShipWorksAboutDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Submit a support case to interapptive
        /// </summary>
        private void OnRequestHelp(object sender, EventArgs e)
        {
            /*using (SubmitHelpRequestDlg dlg = new SubmitHelpRequestDlg())
            {
                dlg.ShowDialog(this);
            }*/

            WebHelper.OpenUrl("http://www.interapptive.com/company/contact.html", this);
        }

        /// <summary>
        /// Open the link to the ShipWorks supplies site
        /// </summary>
        private void OnBuySupplies(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://supplies.shipworks.com/", this);
        }

        #endregion

        #region Core

        /// <summary>
        /// Just send positive feedback if we are polled with this applications
        /// unique message id.  This means another instance is trying to startup.
        /// </summary>
        protected override void WndProc(ref Message msg)
        {
            if (SingleInstance.HandleMainWndProc(ref msg))
            {
                log.Info("Received SingleInstance query.");

                return;
            }

            if (msg.Msg == SingleInstance.SingleInstanceActivateMessageID)
            {
                log.Info("Other ShipWorks is telling me to activate");

                if (notifyIcon.Visible || this.WindowState == FormWindowState.Minimized)
                {
                    BeginInvoke(new MethodInvoker(RestoreFromSystemTray));
                }
            }

            base.WndProc(ref msg);
        }

        /// <summary>
        /// The form is being activated
        /// </summary>
        private void OnActivated(object sender, EventArgs e)
        {
            // Force a heartbeat, so that we refresh right away.  However, we use BeginInvoke, so that if we are being activated
            // due to one of our own modal dialog's returning, the heartbeat will not occur until after the method that called 
            // the modal window returns.
            BeginInvoke(new MethodInvoker(() =>
                {
                    ForceHeartbeat(HeartbeatOptions.None);
                }));
        }

        /// <summary>
        /// Called when items have been removed from the DataProvider.EntityCache b\c they are known to be dirty
        /// </summary>
        void OnEntityChangeDetected(object sender, EventArgs e)
        {
            ForceHeartbeat(HeartbeatOptions.ForceReload);
        }

        /// <summary>
        /// Force a heartbeat to occur before its next scheduled time.  If the paramter changesExpected is true, 
        /// this method will increase the heart rate until changes are found, or until the forced heart rate
        /// time period expires.  This is allowed to be called from any thread.
        /// </summary>
        private void ForceHeartbeat()
        {
            ForceHeartbeat(HeartbeatOptions.None);
        }

        /// <summary>
        /// Force a heartbeat to occur before its next scheduled time.  If the paramter changesExpected is true, 
        /// this method will increase the heart rate until changes are found, or until the forced heart rate
        /// time period expires.  This is allowed to be called from any thread.
        /// </summary>
        private void ForceHeartbeat(HeartbeatOptions options)
        {
            if (InvokeRequired)
            {
                // Put the heartbeat on the UI thread
                BeginInvoke((MethodInvoker) delegate { ForceHeartbeat(options);});
                return;
            }

            // Force it to go now, if it
            heartBeat.ForceHeartbeat(options);
        }

        /// <summary>
        /// Logic that happens when a modal window is opening
        /// </summary>
        void OnEnterThreadModal(object sender, EventArgs e)
        {
            if (IsDisposed || heartBeat.Pace == HeartbeatPace.Stopped)
            {
                return;
            }

            // Make sure we are not in a failure state
            if (ConnectionMonitor.Status != ConnectionMonitorStatus.Normal)
            {
                return;
            }

            if (UserSession.IsLoggedOn)
            {
                gridControl.SaveGridColumnState();
            }
        }

        /// <summary>
        /// Modal window is closing
        /// </summary>
        void OnLeaveThreadModal(object sender, EventArgs e)
        {
            if (IsDisposed || heartBeat.Pace == HeartbeatPace.Stopped)
            {
                return;
            }

            // Make sure we are not in a failure state
            if (ConnectionMonitor.Status != ConnectionMonitorStatus.Normal)
            {
                return;
            }

            if (UserSession.IsLoggedOn)
            {
                gridControl.ReloadGridColumns();
            }
        }

        /// <summary>
        /// A connection senstive scope is about to be acquired
        /// </summary>
        void OnAcquiringConnectionSensitiveScope(object sender, EventArgs e)
        {
            // Search has to be canceled before potentialy changing databases, otherwise it wouldnt have a chance to cleanup in the current database.
            gridControl.CancelSearch();
        }

        /// <summary>
        /// When an action is ran it could cause filtering side affects so we force the heartbeat right away
        /// </summary>
        void OnActionRan(object sender, EventArgs e)
        {
            ForceHeartbeat(HeartbeatOptions.ChangesExpected);
        }

        #endregion

        #region Filtering

        /// <summary>
        /// Called when the user selects a different filter.
        /// </summary>
        private void OnSelectedFilterNodeChanged(object sender, EventArgs e)
        {            
            // No longer need to restore an old node, if we had been searching
            searchRestoreFilterNodeID = 0;

            // Update the grid to show the new node
            gridControl.ActiveFilterNode = filterTree.SelectedFilterNode;

            // Could be changing to a Null node selection due to logging off.  If that's the case, we don't have to 
            // update UI, b\c its already blank.
            if (UserSession.IsLoggedOn)
            {
                // Update selection based ui
                UpdateSelectionDependentUI();

                // Update the detail view UI
                UpdateDetailViewSettingsUI();
            }
        }

        /// <summary>
        /// Called when the selection in the grid changes
        /// </summary>
        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            UpdateSelectionDependentUI();
        }

        /// <summary>
        /// Called when the sort in the grid changes
        /// </summary>
        private void OnGridSortChanged(object sender, EventArgs e)
        {
            // If the user is not logged in, forget it
            if (!UserSession.IsLoggedOn)
            {
                return;
            }

            UpdatePanelState();
        }

        /// <summary>
        /// The grid is going into or coming out of search mode
        /// </summary>
        private void OnGridSearchActiveChanged(object sender, EventArgs e)
        {
            if (gridControl.IsSearchActive)
            {
                searchRestoreFilterNodeID = filterTree.SelectedFilterNodeID;

                filterTree.SelectedFilterNodeChanged -= new EventHandler(OnSelectedFilterNodeChanged);

                filterTree.ActiveSearchNode = gridControl.ActiveFilterNode;
                filterTree.SelectedFilterNode = gridControl.ActiveFilterNode;

                filterTree.SelectedFilterNodeChanged += new EventHandler(OnSelectedFilterNodeChanged);
            }
            else
            {
                // Restore the previously selected node before search started
                if (searchRestoreFilterNodeID != 0)
                {
                    filterTree.SelectedFilterNodeID = searchRestoreFilterNodeID;
                }

                searchRestoreFilterNodeID = 0;

                filterTree.ActiveSearchNode = null;
            }

            UpdateSelectionDependentUI();
            UpdateDetailViewSettingsUI();
        }

        /// <summary>
        /// The active search query has changed
        /// </summary>
        private void OnGridSearchQueryChanged(object sender, EventArgs e)
        {
            // Make sure the counts in the tree and grid get updated
            ForceHeartbeat(HeartbeatOptions.ChangesExpected);
        }

        /// <summary>
        /// Select the initial filter based on the given user settings
        /// </summary>
        private void SelectInitialFilter(UserSettingsEntity settings)
        {
            long initialID;

            if (settings.FilterInitialUseLastActive)
            {
                initialID = settings.FilterLastActive;
            }
            else
            {
                initialID = settings.FilterInitialSpecified;
            }

            // Select it
            filterTree.SelectedFilterNodeID = initialID;

            // If there is nothing selected, that doesn't exist anymore
            if (filterTree.SelectedFilterNode == null)
            {
                filterTree.SelectedFilterNodeID = BuiltinFilter.GetTopLevelKey(FilterTarget.Orders);
            }
        }

        /// <summary>
        /// Open the filter organizer
        /// </summary>
        private void OnManageFilters(object sender, EventArgs e)
        {
            using (FilterOrganizerDlg dlg = new FilterOrganizerDlg(filterTree.SelectedFilterNode, filterTree.GetFolderState()))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Context menu for the filter tree is opening
        /// </summary>
        private void OnFilterTreeContextMenuOpening(object sender, CancelEventArgs e)
        {
            bool filtersPermission = UserSession.Security.HasPermission(PermissionType.ManageFilters);
            bool filterSelected = filterTree.SelectedFilterNode != null;
            bool myFilter = filterSelected ? FilterHelper.IsMyFilter(filterTree.SelectedFilterNode) : false;

            menuItemEditFilter.Enabled = filterSelected;
            menuItemEditFilter.Available = filtersPermission || myFilter;

            if (filterSelected && BuiltinFilter.IsSearchPlaceholderKey(filterTree.SelectedFilterNode.FilterID))
            {
                menuItemEditFilter.Available = false;
            }

            menuItemEditFilterSep.Available = menuItemEditFilter.Available;
        }

        /// <summary>
        /// Edit the selected filter or folder, if any
        /// </summary>
        private void OnEditFilter(object sender, EventArgs e)
        {
            FilterEditingResult result = FilterEditingService.EditFilter(filterTree.SelectedFilterNode, this);

            if (result == FilterEditingResult.OK)
            {
                filterTree.UpdateFilterName(filterTree.SelectedFilterNode.Filter);
            }
        }

        /// <summary>
        /// Create a new filter, based on the current selection
        /// </summary>
        private void OnNewFilter(object sender, EventArgs e)
        {
            CreateFilter(false, (filterTree.SelectedLocation != null) ? filterTree.SelectedLocation.ParentNode : null);
        }

        /// <summary>
        /// Create a new filter folder, based on the current selection
        /// </summary>
        private void OnNewFolder(object sender, EventArgs e)
        {
            CreateFilter(true, (filterTree.SelectedLocation != null) ? filterTree.SelectedLocation.ParentNode : null);
        }

        /// <summary>
        /// Initiate the creation of a new filter or folder
        /// </summary>
        private void CreateFilter(bool isFolder, FilterNodeEntity parent)
        {
            // Creating a filter can create more than one node (if the parent is linked), but 
            // this one will be the one that should be selected
            FilterNodeEntity primaryNode;

            FilterEditingResult result = FilterEditingService.NewFilter(
                isFolder,
                parent,
                filterTree.GetFolderState(),
                this,
                out primaryNode);

            if (result == FilterEditingResult.OK)
            {
                filterTree.ReloadLayouts();
                filterTree.SelectedFilterNode = primaryNode;

                // # of filters present can effect edition issues
                editionGuiHelper.UpdateUI();
            }
        }

        #endregion

        #region Downloading

        /// <summary>
        /// Update the download button to show a drop-down if there are multiple stores
        /// </summary>
        private void UpdateDownloadButtonForStores()
        {
            List<StoreEntity> stores = StoreManager.GetEnabledStores().Where(s => ComputerDownloadPolicy.Load(s).IsThisComputerAllowed).ToList();

            // Only enabled if more than one store
            buttonDownload.Enabled = stores.Count > 0;
            buttonDownload.ToolTip = stores.Count > 0 ? null : new SuperToolTip("Download", string.Format("There are no stores enabled for downloading on this computer."), null, false);
            
            if (stores.Count <= 1)
            {
                buttonDownload.PopupWidget = null;
            }
            else
            {
                Popup popup = new Popup();
                Divelements.SandRibbon.Menu menu = new Divelements.SandRibbon.Menu();

                // Add a menu item for each store
                foreach (StoreEntity store in stores)
                {
                    Divelements.SandRibbon.MenuItem menuItem = new Divelements.SandRibbon.MenuItem(store.StoreName);
                    menuItem.Tag = store;
                    menuItem.Image = EnumHelper.GetImage((StoreTypeCode) store.TypeCode);
                    menuItem.Activate += new EventHandler(OnDownloadOrdersSingleStore);
                    menu.Items.Add(menuItem);
                }

                popup.Items.Add(menu);

                buttonDownload.PopupWidget = popup;
            }
        }

        /// <summary>
        /// Initiate the downloading of orders
        /// </summary>
        private void OnDownloadOrders(object sender, EventArgs e)
        {
            // Get the list of stores that we will download for.
            ICollection<StoreEntity> stores = StoreManager.GetAllStores().Where(s => s.Enabled && ComputerDownloadPolicy.Load(s).IsThisComputerAllowed).ToList();

            if (stores.Count > 0)
            {
                // Start the download
                DownloadManager.StartDownload(stores, DownloadInitiatedBy.User);

                ShowDownloadProgress();
            }
            else
            {
                MessageHelper.ShowWarning(this, "There are no ShipWorks stores enabled for downloading.");
            }
        }

        /// <summary>
        /// Initiate the downloading of orders for a single store
        /// </summary>
        private void OnDownloadOrdersSingleStore(object sender, EventArgs e)
        {
            Divelements.SandRibbon.MenuItem menuItem = (Divelements.SandRibbon.MenuItem) sender;
            StoreEntity store = (StoreEntity) menuItem.Tag;

            // Start the download
            DownloadManager.StartDownload(new List<StoreEntity> { store }, DownloadInitiatedBy.User);

            ShowDownloadProgress();
        }

        /// <summary>
        /// A download is starting
        /// </summary>
        void OnDownloadStarting(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // Needs to be invoke, so the downloader doesn't move on until we have the UI updated.
                Invoke(new EventHandler(OnDownloadStarting), sender, e);
                return;
            }

            downloadingStatusLabel.PictureBox.Image = (ShipWorksDisplay.ColorScheme != ColorScheme.Black) ? Resources.arrows_greengray : Resources.refresh_greengray;
            downloadingStatusLabel.Text = "Downloading...";
            downloadingStatusLabel.Visible = true;
        }

        /// <summary>
        /// A download has ended
        /// </summary>
        void OnDownloadComplete(object sender, DownloadCompleteEventArgs e)
        {
            if (InvokeRequired)
            {
                // This does need to be Invoke and not BeginInvoke.  So that the downloader doesnt think we are done early, and we're messing
                // with the post-download UI, and another download starts.
                Invoke(new DownloadCompleteEventHandler(OnDownloadComplete), sender, e);
                return;
            }

            if (e.ShowDashboardError)
            {
                downloadingStatusLabel.Text = "Download Error";
                downloadingStatusLabel.PictureBox.Image = Resources.error16;

                // If the progress window isn't up, or if its not showing the current error, put up a local message
                if (!DownloadManager.IsProgressVisible || !e.IsProgressCurrent)
                {
                    DashboardManager.ShowLocalMessage("DownloadError",
                        DashboardMessageImageType.Error, 
                        "Download Error", 
                        "An error occurred while downloading.", 
                        new DashboardActionMethod("[link]View Details[/link]", ShowDownloadLog));
                }

                // We always check for new server messages after a download error, since if there was a download problem
                // it could be we put out a server message related to it.
                DashboardManager.DownloadLatestServerMessages();
            }
            else
            {
                downloadingStatusLabel.Visible = false;
            }

            ForceHeartbeat(HeartbeatOptions.ChangesExpected);
        }

        /// <summary>
        /// Clicking the download status label
        /// </summary>
        private void OnClickDownloadStatus(object sender, EventArgs e)
        {
            ShowDownloadProgress();
        }

        /// <summary>
        /// Show the download progress window
        /// </summary>
        private void ShowDownloadProgress()
        {
            DownloadManager.ShowProgressDlg(this);

            // If we are not downloading, and the window is closed, then clear out the status
            // label.  The label may still have been visible from an auto-download, or from
            // an error.
            if (!DownloadManager.IsDownloading())
            {
                downloadingStatusLabel.Visible = false;
            }
        }

        /// <summary>
        /// Open the download history
        /// </summary>
        private void ShowDownloadLog()
        {
            using (DownloadLogDlg dlg = new DownloadLogDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Open the download log
        /// </summary>
        private void OnDownloadHistory(object sender, EventArgs e)
        {
            ShowDownloadLog();
        }

        /// <summary>
        /// Open the audit window
        /// </summary>
        private void OnAudit(object sender, EventArgs e)
        {
            using (AuditDlg dlg = new AuditDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        #endregion

        #region Emailing

        /// <summary>
        /// Clicking the email status area to show the progress
        /// </summary>
        private void OnClickEmailStatus(object sender, EventArgs e)
        {
            ShowEmailProgress();
        }

        /// <summary>
        /// Emailing is beginning
        /// </summary>
        void OnEmailStarting(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler(OnEmailStarting), sender, e);
                return;
            }

            emailingStatusLabel.PictureBox.Image = (ShipWorksDisplay.ColorScheme != ColorScheme.Black) ? Resources.arrows_greengray : Resources.refresh_greengray;
            emailingStatusLabel.Text = "Sending email...";
            emailingStatusLabel.Visible = true;
        }

        /// <summary>
        /// Emailing has ended
        /// </summary>
        void OnEmailComplete(object sender, EmailCommunicationCompleteEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EmailCommunicationCompleteEventHandler(OnEmailComplete), sender, e);
                return;
            }

            if (e.HasErrors)
            {
                emailingStatusLabel.Text = "Email Error";
                emailingStatusLabel.PictureBox.Image = Resources.error16;
            }
            else
            {
                emailingStatusLabel.Visible = false;
            }

            ForceHeartbeat(HeartbeatOptions.ChangesExpected);
        }

        /// <summary>
        /// Show the email progress window
        /// </summary>
        private void ShowEmailProgress()
        {
            EmailCommunicator.ShowProgressDlg(this);

            // If we are not emailing, and the window is closed, then clear out the status
            // label.  The label may still have been visible from an auto-email, or from
            // an error.
            if (!EmailCommunicator.IsEmailing)
            {
                emailingStatusLabel.Visible = false;
            }
        }

        /// <summary>
        /// Open the email account window
        /// </summary>
        private void OnEmailAccounts(object sender, EventArgs e)
        {
            using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Open the email messages window
        /// </summary>
        private void OnEmailMessages(object sender, EventArgs e)
        {
            using (EmailOutlookDlg dlg = new EmailOutlookDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        #endregion

        #region Orders & Customers

        /// <summary>
        /// Lookup the customer for the selected order
        /// </summary>
        private void OnLookupCustomer(object sender, EventArgs e)
        {
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(gridControl.Selection.Keys.First());
            if (order == null)
            {
                MessageHelper.ShowMessage(this, "The order has been deleted.");
            }

            // Ensure the customer grid is active
            filterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Customers).FilterNode;
            gridControl.LoadSearchCriteria(QuickLookupCriteria.CreateCustomerLookupDefinition(order.CustomerID));
        }

        /// <summary>
        /// Lookup the orders for the selected customer.
        /// </summary>
        private void OnLookupOrders(object sender, EventArgs e)
        {
            CustomerEntity customer = (CustomerEntity) DataProvider.GetEntity(gridControl.Selection.Keys.First());
            if (customer == null)
            {
                MessageHelper.ShowMessage(this, "The customer has been deleted.");
            }

            // Ensure the customer grid is active
            filterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
            gridControl.LoadSearchCriteria(QuickLookupCriteria.CreateOrderLookupDefinition(customer));
        }

        /// <summary>
        /// A grid row is "activated"
        /// </summary>
        private void OnGridRowActivated(object sender, GridRowEventArgs e)
        {
            if (gridControl.Selection.Count != 1)
            {
                return;
            }

            long entityID = gridControl.Selection.Keys.First();

            if (gridControl.ActiveFilterTarget == FilterTarget.Orders)
            {
                EditOrder(entityID);
            }

            if (gridControl.ActiveFilterTarget == FilterTarget.Customers)
            {
                EditCustomer(entityID);
            }
        }

        /// <summary>
        /// Edit the selected order
        /// </summary>
        private void OnEditOrder(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            EditOrder(gridControl.Selection.Keys.First());
        }

        /// <summary>
        /// Initiate the editing of the given order
        /// </summary>
        private void EditOrder(long orderID)
        {
            OrderEditorDlg.Open(orderID, this);
        }

        /// <summary>
        /// Create a new order
        /// </summary>
        private void OnNewOrder(object sender, EventArgs e)
        {
            long? customerID = null;
            long? storeID = null;

            if (gridControl.ActiveFilterTarget == FilterTarget.Customers && gridControl.Selection.Count == 1)
            {
                customerID = gridControl.Selection.Keys.First();
                storeID = null;
            }

            if (gridControl.ActiveFilterTarget == FilterTarget.Orders && gridControl.Selection.Count == 1)
            {
                OrderEntity order = (OrderEntity) DataProvider.GetEntity(gridControl.Selection.Keys.First());
                if (order != null)
                {
                    customerID = order.CustomerID;
                    storeID = order.StoreID;
                }
            }

            using (AddOrderWizard dlg = new AddOrderWizard(customerID, storeID))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    LookupOrder(dlg.OrderID);
                }
            }
        }

        /// <summary>
        /// Lookup the specified order by selecting it via Quick Search
        /// </summary>
        private void LookupOrder(long orderID)
        {
            // Ensure the order grid is active
            filterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
            gridControl.LoadSearchCriteria(QuickLookupCriteria.CreateOrderLookupDefinition(orderID));
        }

        /// <summary>
        /// Create a new customer
        /// </summary>
        private void OnNewCustomer(object sender, EventArgs e)
        {
            using (AddCustomerDlg dlg = new AddCustomerDlg())
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Ensure the customer grid is active
                    filterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Customers).FilterNode;
                    gridControl.LoadSearchCriteria(QuickLookupCriteria.CreateCustomerLookupDefinition(dlg.CustomerID));
                }
            }
        }

        /// <summary>
        /// Edit the selected customer
        /// </summary>
        private void OnEditCustomer(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            EditCustomer(gridControl.Selection.Keys.First());
        }

        /// <summary>
        /// Initiate the editing of the given customer
        /// </summary>
        private void EditCustomer(long customerID)
        {
            CustomerEditorDlg.Open(customerID, this);
        }

        /// <summary>
        /// The delete key was pressed in the grid
        /// </summary>
        private void OnDelete(object sender, EventArgs e)
        {
            if (gridControl.Selection.Count == 0)
            {
                return;
            }

            // If the delete button isn't visible right now they got here through the keyboard... just pretend like 
            // there is no keyboard access either
            if (gridControl.ActiveFilterTarget == FilterTarget.Orders && !buttonDeleteOrders.Visible)
            {
                return;
            }

            // Same as above for customers
            if (gridControl.ActiveFilterTarget == FilterTarget.Customers && !buttonDeleteCustomer.Visible)
            {
                return;
            }

            string targetName = EnumHelper.GetDescription(gridControl.ActiveFilterTarget);

            DialogResult result = MessageHelper.ShowQuestion(this, 
                string.Format("Delete the selected {0}?", targetName.ToLowerInvariant()));

            if (result == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;

                PermissionAwareDeleter deleter = new PermissionAwareDeleter(this, PermissionType.OrdersModify);
                deleter.ExecuteCompleted += (s, a) =>
                    {
                        ForceHeartbeat(HeartbeatOptions.ChangesExpected);
                    };

                deleter.DeleteAsync(gridControl.Selection.OrderedKeys);
            }
        }

        /// <summary>
        /// An edit has occurred in one of hte panels
        /// </summary>
        private void OnPanelDataChanged(object sender, EventArgs e)
        {
            ForceHeartbeat(HeartbeatOptions.ChangesExpected);
        }

        #endregion

        #region View

        /// <summary>
        /// A panel should be shown
        /// </summary>
        private void OnShowPanel(object sender, EventArgs e)
        {
            SandMenuItem item = (SandMenuItem) sender;
            DockControl dockControl = (DockControl) item.Tag;

            dockControl.Open(WindowOpenMethod.OnScreenActivate);
        }

        // A dock control that didnt used to be open now is
        private void OnDockControlActivated(object sender, DockControlEventArgs e)
        {
            UpdatePanelState(e.DockControl);
        }

        /// <summary>
        /// Open the editor for the context menu of the active grid
        /// </summary>
        private void OnEditGridContextMenu(object sender, EventArgs e)
        {
            using (GridMenuEditor dlg = new GridMenuEditor(gridMenuLayoutProvider))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Do some necessary initialization of the Detail View controls
        /// </summary>
        private void InitializeDetailViewControls()
        {
            // The only way to get at the ribbon event for a combo is this
            GetSandRibbonComboPopup(detailViewDetailTemplate).BeforePopup += new BeforePopupEventHandler(OnDetailPreviewTemplatePopup);

            detailViewDetailHeight.Items.Add("Auto");

            // Height
            detailViewDetailHeight.ComboBox.MaxDropDownItems = DetailViewSettings.MaxDetailRows + 1;
            for (int i = 1; i <= DetailViewSettings.MaxDetailRows; i++)
            {
                detailViewDetailHeight.Items.Add(i.ToString());
            }

            // Template.  This fake item is what the ribbon uses to draw the text in the combo.
            detailViewDetailTemplate.Items.Add(new SandLabel());
            detailViewDetailTemplate.SelectedItem = detailViewDetailTemplate.Items[0];

            // More detail view stuff 
            buttonDetailViewNormal.Tag = DetailViewMode.Normal;
            buttonDetailViewNormalDetail.Tag = DetailViewMode.NormalWithDetail;
            buttonDetailViewDetail.Tag = DetailViewMode.DetailOnly;
        }

        /// <summary>
        /// The popup for choosing the data preview is opening
        /// </summary>
        private void OnDetailPreviewTemplatePopup(object sender, BeforePopupEventArgs e)
        {
            FolderExpansionState expansionState = null;

            TemplateEntity template = TemplateManager.Tree.GetTemplate(gridControl.ActiveDetailViewSettings.TemplateID);
            if (template != null)
            {
                expansionState = new FolderExpansionState();

                TemplateFolderEntity parentFolder = template.ParentFolder;
                while (parentFolder != null)
                {
                    expansionState.SetExpanded(parentFolder.TemplateFolderID, true);
                    parentFolder = parentFolder.ParentFolder;
                }
            }

            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnChangeDetailPreviewTemplate, expansionState);
        }

        /// <summary>
        /// The user has chosen a new template to use as the detail preview
        /// </summary>
        private void OnChangeDetailPreviewTemplate(object sender, TemplateNodeChangedEventArgs e)
        {
            DetailViewSettings settings = gridControl.ActiveDetailViewSettings;

            // Save the new setting
            settings.TemplateID = e.NewNode.ID;

            // Update the UI to display it
            UpdateDetailViewSettingsUI();
        }

        /// <summary>
        /// Increase the height of the details view
        /// </summary>
        private void OnDetailViewDetailHeightIncrease(object sender, EventArgs e)
        {
            detailViewDetailHeight.SelectedIndex = Math.Min(detailViewDetailHeight.SelectedIndex + 1, detailViewDetailHeight.Items.Count - 1);
        }

        /// <summary>
        /// Decrease the height of the details view
        /// </summary>
        private void OnDetailViewDetailHeightDecrease(object sender, EventArgs e)
        {
            detailViewDetailHeight.SelectedIndex = Math.Max(detailViewDetailHeight.SelectedIndex - 1, 0);
        }

        /// <summary>
        /// The height of the detail mode is changing
        /// </summary>
        private void OnChangeDetailViewDetailHeight(object sender, EventArgs e)
        {
            DetailViewSettings settings = gridControl.ActiveDetailViewSettings;

            // Save the new setting
            settings.DetailRows = detailViewDetailHeight.SelectedIndex;

            UpdateDetailViewSettingsUI();
        }

        /// <summary>
        /// The mode of the detail view is changing
        /// </summary>
        private void OnChangeDetailViewDetailMode(object sender, EventArgs e)
        {
            DetailViewSettings settings = gridControl.ActiveDetailViewSettings;

            // Save the new setting
            settings.DetailViewMode = (DetailViewMode) ((SandButton) sender).Tag;

            UpdateDetailViewSettingsUI();
        }

        /// <summary>
        /// Update the UI controls for the Detail View settings
        /// </summary>
        private void UpdateDetailViewSettingsUI()
        {
            DetailViewSettings settings = gridControl.ActiveDetailViewSettings;

            // If there is no grid info loaded, get out
            if (settings == null)
            {
                return;
            }

            List<SandButton> modeRadios = new List<SandButton> { buttonDetailViewDetail, buttonDetailViewNormalDetail, buttonDetailViewNormal };
            SandButton activeRadio = null;

            switch (settings.DetailViewMode)
            {
                case DetailViewMode.Normal: activeRadio = buttonDetailViewNormal; break;
                case DetailViewMode.DetailOnly: activeRadio = buttonDetailViewDetail; break;
                case DetailViewMode.NormalWithDetail: activeRadio = buttonDetailViewNormalDetail; break;
            }

            // Uncheck the old ones
            modeRadios.Remove(activeRadio);
            modeRadios.ForEach(b => b.Checked = false);

            // Check the right one
            activeRadio.Checked = true;

            // Select the height
            detailViewDetailHeight.SelectedIndex = settings.DetailRows;

            // Ensure we have the item the combo will use as the display text
            if (detailViewDetailTemplate.Items.Count == 0)
            {
                detailViewDetailTemplate.Items.Add(new SandLabel());
                detailViewDetailTemplate.SelectedItem = detailViewDetailTemplate.Items[0];
            }

            // Set the template
            TemplateEntity template = TemplateManager.Tree.GetTemplate(settings.TemplateID);
            string templateText = (template != null) ? template.FullName : "";
            detailViewDetailTemplate.Items[0].Text = PathUtility.CompactPath(templateText, detailViewDetailTemplate.DisplaySize.Width - 30);
            detailViewDetailTemplate.InvalidateDrawing();

            bool enableDetails = settings.DetailViewMode != DetailViewMode.Normal;

            // Details not enabled for normal mode
            labelDetailViewDetailView.Enabled = enableDetails;
            buttonDetailViewHeightIncrease.Enabled = enableDetails;
            buttonDetailViewHeightDecrease.Enabled = enableDetails;
            detailViewDetailHeight.Enabled = enableDetails;
            detailViewDetailHeight.ComboBox.Enabled = enableDetails;
            detailViewDetailTemplate.Enabled = enableDetails;

            if (enableDetails)
            {
                // Update increase\decrease availability
                buttonDetailViewHeightIncrease.Enabled = detailViewDetailHeight.SelectedIndex < detailViewDetailHeight.Items.Count - 1;
                buttonDetailViewHeightDecrease.Enabled = detailViewDetailHeight.SelectedIndex > 0;
            }
        }

        /// <summary>
        /// Edit the grid columns for the grid
        /// </summary>
        private void OnEditGridColumns(object sender, EventArgs e)
        {
            FilterNodeEntity filterNode;

            if (gridControl.IsSearchActive)
            {
                filterNode = gridControl.ActiveFilterNode;
            }
            else
            {
                filterNode = filterTree.SelectedFilterNode;
            }

            if (filterNode == null)
            {
                filterNode = FilterLayoutContext.Current.GetSharedLayout(gridControl.ActiveFilterTarget).FilterNode;
            }

            using (FilterNodeColumnDlg dlg = new FilterNodeColumnDlg(filterNode))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Save user selected parts of the environment
        /// </summary>
        private void OnSaveEnvironmentSettings(object sender, EventArgs e)
        {
            using (EnvironmentSaveDlg dlg = new EnvironmentSaveDlg(new EnvironmentController(windowLayoutProvider, gridMenuLayoutProvider)))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Load a previously saved environment settings file
        /// </summary>
        private void OnLoadEnvironmentSettings(object sender, EventArgs e)
        {
            using (EnvironmentLoadDlg dlg = new EnvironmentLoadDlg(new EnvironmentController(windowLayoutProvider, gridMenuLayoutProvider)))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Reset selected environment settings back to the defaults
        /// </summary>
        private void OnResetEnvironmentSettings(object sender, EventArgs e)
        {
            using (EnvironmentResetDlg dlg = new EnvironmentResetDlg(new EnvironmentController(windowLayoutProvider, gridMenuLayoutProvider)))
            {
                dlg.ShowDialog(this);
            }

            // If a new panel was shown, then we may need to update its display state
            UpdatePanelState();
        }

        /// <summary>
        /// Initialize the panels for the current user
        /// </summary>
        private void InitializePanels()
        {
            // First go through each panel and wrap it in a Panel control that will allow us to show messages to the user like "No orders are selected.";
            foreach (DockControl dockControl in sandDockManager.GetDockControls())
            {
                // See if this is one that needs wrapped
                if (dockControl.Controls.Count == 1 && dockControl.Controls[0] is IDockingPanelContent)
                {
                    DockingPanelContentHolder holder = new DockingPanelContentHolder();
                    holder.Initialize((IDockingPanelContent) dockControl.Controls[0]);

                    holder.Dock = DockStyle.Fill;
                    dockControl.Controls.Add(holder);
                }
            }

            // Initialize the note panel
            panelNotes.Initialize(new Guid("{FBE6CCDC-B822-45e2-813C-4AF64AE58657}"), GridColumnDefinitionSet.Notes, (GridColumnLayout noteLayout) =>
                {
                    noteLayout.DefaultSortColumnGuid = noteLayout.AllColumns[NoteFields.Edited].Definition.ColumnGuid;
                    noteLayout.DefaultSortOrder = ListSortDirection.Descending;

                    // Turn off all columns except the object and the note
                    foreach (GridColumnPosition position in noteLayout.AllColumns
                        .Where(p => p != noteLayout.AllColumns[NoteFields.ObjectID] && p != noteLayout.AllColumns[NoteFields.Text]))
                    {
                        position.Visible = false;
                    }
                });

            // Initialize the orders panel
            panelOrders.Initialize(new Guid("{06878FA9-7A6D-442c-B4F8-357C1B3F6A45}"), GridColumnDefinitionSet.OrderPanel, (GridColumnLayout layout) =>
            {
                layout.AllColumns[OrderFields.OrderDate].Visible = false;
                layout.AllColumns[OrderFields.RollupNoteCount].Visible = false;
                layout.AllColumns[OrderFields.ShipLastName].Visible = false;
                layout.AllColumns[OrderFields.ShipCountryCode].Visible = false;

                layout.AllColumns[OrderFields.OrderID].Width = 75;
                layout.AllColumns[OrderFields.LocalStatus].Width = 60;
                layout.AllColumns[OrderFields.OrderTotal].Width = 55;
                layout.AllColumns[OrderFields.OnlineStatus].Width = 60;

                // Hide all the store specific columns
                foreach (GridColumnPosition column in layout.AllColumns.Where(c => c.Definition.StoreTypeCode != null))
                {
                    column.Visible = false;
                }
            });

            // Initialize the items panel
            panelItems.Initialize(new Guid("{E102F97E-5C0F-4fa2-A0CF-47FC852C0B57}"), GridColumnDefinitionSet.OrderItems, (GridColumnLayout layout) =>
                {
                    // Adjust to fit for default panel size
                    layout.AllColumns[OrderItemFields.Name].Width = 120;
                    layout.AllColumns[OrderItemFields.Quantity].Width = 30;
                    layout.AllColumns[OrderItemFields.UnitPrice].Width = 50;
                    layout.AllColumns[OrderItemFields.LocalStatus].Width = 60;

                    // Hide all the store specific columns
                    foreach (GridColumnPosition column in layout.AllColumns.Where(c => c.Definition.StoreTypeCode != null))
                    {
                        column.Visible = false;
                    }
                });

            // Initialize the charges panel
            panelCharges.Initialize(new Guid("{E604A11A-2B98-4186-9814-644EB8F58204}"), GridColumnDefinitionSet.Charges, (GridColumnLayout layout) =>
                {
                    layout.AllColumns[OrderChargeFields.Type].Width = 70;
                });

            // Initialize the shipments panel
            panelShipments.Initialize(new Guid("{C5FFA0CC-3AD2-485a-BC26-1E6072636116}"), GridColumnDefinitionSet.ShipmentPanel, (GridColumnLayout layout) =>
                {
                    layout.AllColumns[new Guid("{98038AB5-AA95-4778-9801-574C2B723DD4}")].Visible = false;
                }); 

            // Initialize the email control
            panelEmail.Initialize(new Guid("{F0C792D7-90F5-4eab-9EF0-ADCEC49AC167}"), GridColumnDefinitionSet.EmailOutboundPanel, (GridColumnLayout layout) =>
                {
                    layout.AllColumns[EmailOutboundFields.AccountID].Visible = false;
                    layout.AllColumns[EmailOutboundFields.ContextID].Visible = false;
                    layout.AllColumns[EmailOutboundFields.Subject].Visible = false;
                });

            // Initialize the print control
            panelPrinted.Initialize(new Guid("{AEEDA4AE-5D84-447d-90BA-92AFCD0D4BD4}"), GridColumnDefinitionSet.PrintResult, (GridColumnLayout layout) =>
                {
                    layout.AllColumns[PrintResultFields.PrintDate].Visible = false;
                    layout.AllColumns[PrintResultFields.PaperSourceName].Visible = false;
                    layout.AllColumns[PrintResultFields.Copies].Visible = false;
                });

            // Initialize the payment details panel
            panelPaymentDetail.Initialize(new Guid("{1C5FD43A-E357-462b-A2BC-CF458E2EE45D}"), GridColumnDefinitionSet.PaymentDetails, null);
        }

        /// <summary>
        /// Get all of the DockingPanelContentHolder objects.
        /// </summary>
        private List<DockingPanelContentHolder> GetDockingPanelContentHolders()
        {
            List<DockingPanelContentHolder> holders = new List<DockingPanelContentHolder>();

            foreach (DockControl dockControl in sandDockManager.GetDockControls().Where(d => d.Controls.Count == 1))
            {
                DockingPanelContentHolder holder = dockControl.Controls[0] as DockingPanelContentHolder;
                if (holder != null)
                {
                    holders.Add(holder);
                }
            }

            return holders;
        }

        #endregion

        #region Administration

        /// <summary>
        /// Initiate the New User wizard
        /// </summary>
        private void OnNewUser(object sender, EventArgs e)
        {
            using (AddUserWizard dlg = new AddUserWizard())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Open user manager window
        /// </summary>
        private void OnManageUsers(object sender, EventArgs e)
        {
            using (UserManagerDlg dlg = new UserManagerDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Open the store manager
        /// </summary>
        private void OnManageStores(object sender, EventArgs e)
        {
            using (StoreManagerDlg dlg = new StoreManagerDlg())
            {
                dlg.ShowDialog(this);

                if (StoreManager.GetAllStores().Count == 0)
                {
                    InitiateLogon();
                }
                else
                {
                    UpdateStoreDependentUI();
                }
            }
        }

        /// <summary>
        /// Open the template manager
        /// </summary>
        private void OnManageTemplates(object sender, EventArgs e)
        {
            using (TemplateManagerDlg dlg = new TemplateManagerDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Open the action manager
        /// </summary>
        private void OnManageActions(object sender, EventArgs e)
        {
            using (ActionManagerDlg dlg = new ActionManagerDlg())
            {
                dlg.ShowDialog(this);
            }

            UpdateCustomButtonsActionsUI();
        }

        /// <summary>
        /// Open the shipping settings window
        /// </summary>
        private void OnShippingSettings(object sender, EventArgs e)
        {
            using (ShippingSettingsDlg dlg = new ShippingSettingsDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Change the active database connection
        /// </summary>
        private void OnChangeDatabaseLogon(object sender, EventArgs e)
        {
            bool needLogon = false;

            using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("change the SQL log on", this))
            {
                if (!scope.Acquired)
                {
                    return;
                }

                using (DatabaseLogonDlg dlg = new DatabaseLogonDlg())
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        needLogon = true;
                    }
                }
            }

            // This is down here so its outside of the scope
            if (needLogon)
            {
                InitiateLogon();
            }
        }

        /// <summary>
        /// Open the window for opening windows firewall
        /// </summary>
        private void OnWindowsFirewall(object sender, EventArgs e)
        {
            using (WindowsFirewallDlg dlg = new WindowsFirewallDlg())
            {
                dlg.ShowDialog(this);
            }
        }

        #endregion

        #region Shipping

        /// <summary>
        /// Ship the selected orders
        /// </summary>
        private void OnShipOrders(object sender, EventArgs e)
        {
            if (gridControl.Selection.Count > ShipmentsLoader.MaxAllowedOrders)
            {
                MessageHelper.ShowInformation(this, string.Format("You can only ship up to {0} orders at a time.", ShipmentsLoader.MaxAllowedOrders));
                return;
            }

            ShipmentsLoader loader = new ShipmentsLoader(this);

            // False = not tracking
            loader.Tag = false;

            loader.LoadCompleted += OnShipOrdersLoadShipmentsCompleted;
            loader.LoadAsync(gridControl.Selection.OrderedKeys);
        }

        /// <summary>
        /// Track shipments for the selected orders
        /// </summary>
        private void OnTrackShipments(object sender, EventArgs e)
        {
            if (gridControl.Selection.Count > ShipmentsLoader.MaxAllowedOrders)
            {
                MessageHelper.ShowInformation(this, string.Format("You can only track up to {0} orders at a time.", ShipmentsLoader.MaxAllowedOrders));
                return;
            }

            ShipmentsLoader loader = new ShipmentsLoader(this);

            // True = show tracking
            loader.Tag = true;

            loader.LoadCompleted += OnShipOrdersLoadShipmentsCompleted;
            loader.LoadAsync(gridControl.Selection.OrderedKeys);
        }

        /// <summary>
        /// The async loading of shipments for shipping has completed
        /// </summary>
        void OnShipOrdersLoadShipmentsCompleted(object sender, ShipmentsLoadedEventArgs e)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (e.Cancelled)
            {
                return;
            }

            // Show the shipping window.  The Tag property hold the value of whether we are shipping or tracking.
            using (ShippingDlg dlg = new ShippingDlg(e.Shipments, (bool) ((ShipmentsLoader) sender).Tag))
            {
                dlg.ShowDialog(this);

                // We always check for new server messages after shipping, since if there was a shipping problem
                // it could be we put out a server message related to it.
                DashboardManager.DownloadLatestServerMessages();
            }
        }

        /// <summary>
        /// The FedEx Close popup menu is opening, so we need to dynamically populate it
        /// </summary>
        private void OnFedExClosePopupOpening(object sender, BeforePopupEventArgs e)
        {
            FedExGroundClose.PopulatePrintReportsMenu(menuFedExPrintReports);

            menuFedExSmartPostClose.Visible = FedExUtility.GetSmartPostHubs().Count > 0;
        }

        /// <summary>
        /// Process FedEx end of day close
        /// </summary>
        private void OnFedExGroundClose(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                List<FedExEndOfDayCloseEntity> closings = FedExGroundClose.ProcessClose();

                if (closings != null && closings.Count > 0)
                {
                    DialogResult result = MessageHelper.ShowQuestion(this, "The close reports have been processed and are ready to be printed.\n\nPrint now?");

                    if (result == DialogResult.OK)
                    {
                        FedExGroundClose.PrintCloseReports(closings);
                    }
                }
                else
                {
                    MessageHelper.ShowInformation(this, "There were no shipments to be closed.");
                }
            }
            catch (FedExException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }


        /// <summary>
        /// User has initiated the FedEx SmartPost close
        /// </summary>
        private void OnFedExSmartPostClose(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                bool anyClosed = false;
                FedExShippingClerk shippingClerk = new FedExShippingClerk();

                // Process all accounts with configured hub ids
                foreach (FedExAccountEntity account in FedExAccountManager.Accounts.Where(a => XElement.Parse(a.SmartPostHubList).Descendants("HubID").Any()))
                {
                    if (shippingClerk.CloseSmartPost(account) != null)
                    {
                        // A non-null closed entity was returned, so there were shipments that were closed for this account.
                        anyClosed = true; 
                    }
                }

                if (anyClosed)
                {
                    MessageHelper.ShowInformation(this, "The close has been successfully processed.\n\nSmartPost Close does not generate any reports to be printed.  No further action is required.");
                }
                else
                {
                    MessageHelper.ShowInformation(this, "There were no shipments to be closed.");
                }
            }
            catch (FedExException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        /// <summary>
        /// The postal scan form popup is opening, we need to dynamically repopulate the print menu
        /// </summary>
        private void OnPostalScanFormOpening(object sender, BeforePopupEventArgs e)
        {
            List<IScanFormAccountRepository> repositories = new List<IScanFormAccountRepository>();
            repositories.Add(new EndiciaScanFormAccountRepository());
            repositories.Add(new Express1EndiciaScanFormAccountRepository());
            repositories.Add(new StampsScanFormAccountRepository());
            repositories.Add(new Express1StampsScanFormAccountRepository());

            ScanFormUtility.PopulateCreateScanFormMenu(menuCreateEndiciaScanForm, repositories);
            ScanFormUtility.PopulatePrintScanFormMenu(menuPrintEndiciaScanForm, repositories);
        }

        #endregion

        #region Create

        /// <summary>
        /// Quick Print using the template from the args
        /// </summary>
        private void OnQuickPrint(object sender, TemplateNodeChangedEventArgs e)
        {
            if (!TemplatePrinterSelectionDlg.EnsureConfigured(this, e.NewNode.Template))
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Create the print job using the default settings from the template
            PrintJob job = PrintJob.Create(e.NewNode.Template, gridControl.Selection.OrderedKeys);

            // Start the printing of the job
            StartPrintJob(job, PrintAction.Print);
        }

        /// <summary>
        /// Print using the template from the args
        /// </summary>
        private void OnPrint(object sender, TemplateNodeChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Create the print job using the default settings from the template
            PrintJob job = PrintJob.Create(e.NewNode.Template, gridControl.Selection.OrderedKeys);

            // Allow the user to change the defaults using the settings window
            using (PrintJobSettingsDlg dlg = new PrintJobSettingsDlg(job.Settings))
            {
                dlg.AcceptButtonText = "Print";

                // Cancel the print if not accepted
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }
            }

            // Start the printing of the job
            StartPrintJob(job, PrintAction.Print);
        }

        /// <summary>
        /// Print preview using the template from the args
        /// </summary>
        private void OnPrintPreview(object sender, TemplateNodeChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            TemplateEntity template = e.NewNode.Template;

            if (template.Type == (int) TemplateType.Thermal)
            {
                MessageHelper.ShowError(this,
                    "The selected template is for printing thermal labels.\n\n" +
                    "Thermal label data must be sent directly to a thermal printer, and cannot be previewed.");

                return;
            }

            // Create the print job using the default settings from the template
            PrintJob job = PrintJob.Create(e.NewNode.Template, gridControl.Selection.OrderedKeys);

            // Start the preview of the job
            StartPrintJob(job, PrintAction.Preview);
        }

        /// <summary>
        /// Start the printing or previewing of the given print job
        /// </summary>
        private void StartPrintJob(PrintJob job, PrintAction action)
        {
            // Show the progress window
            ProgressDlg progressDlg = new ProgressDlg(job.ProgressProvider);
            progressDlg.Title = (action == PrintAction.Preview) ? "Previewing" : "Printing";
            progressDlg.Description = string.Format("ShipWorks is {0}.", (action == PrintAction.Preview) ? "generating a preview" : "printing the selected items");

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

            if (action == PrintAction.Print)
            {
                job.PrintCompleted += new PrintActionCompletedEventHandler(OnPrintActionCompleted);
                job.PrintAsync(delayer);
            }
            else
            {
                job.PreviewShown += new PrintPreviewShownEventHandler(OnPrintPreviewShown);
                job.PreviewCompleted += new PrintActionCompletedEventHandler(OnPrintActionCompleted);
                job.PreviewAsync(this, delayer);
            }

            // Only show if time goes by
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));
        }

        /// <summary>
        /// The pring preview window is now visible
        /// </summary>
        private void OnPrintPreviewShown(object sender, PrintPreviewShownEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((PrintPreviewShownEventHandler) OnPrintPreviewShown, sender, e);
                return;
            }

            // Once we are on the UI thread, close the progres window
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) e.UserState;
            delayer.NotifyComplete();

            // The delayer will re-enable the main window.  We need to keep it disabled until the preview is done
            NativeMethods.EnableWindow(Handle, false);
        }

        /// <summary>
        /// An asynchronous print action has completed
        /// </summary>
        private void OnPrintActionCompleted(object sender, PrintActionCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((PrintActionCompletedEventHandler) OnPrintActionCompleted, sender, e);
                return;
            }

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) e.UserState;

            // Its possible its already in the completed state when its a preview and the PreviewShown even marks it as complete
            if (delayer.IsComplete)
            {
                // This should only happen for previews where the window is already shown.  And in that case, we manually disabled the window
                // and need to re-enable it now
                Debug.Assert(e.PrintAction == PrintAction.Preview);
                NativeMethods.EnableWindow(Handle, true);
            }
            else
            {
                delayer.NotifyComplete();
            }

            PrintJob job = (PrintJob) sender;
            job.PreviewShown -= this.OnPrintPreviewShown;
            job.PreviewCompleted -= this.OnPrintActionCompleted;
            job.PrintCompleted -= this.OnPrintActionCompleted;

            // Check for errors
            if (e.Error != null)
            {
                // See if its an error we know how to handle.
                PrintingException printingEx = e.Error as PrintingException;
                if (printingEx != null)
                {
                    PrintingExceptionDisplay.ShowError(this, printingEx);
                }

                // Rethrow the error as is
                else
                {
                    log.Error("Error while printing", e.Error);
                    throw new ApplicationException(e.Error.Message, e.Error);
                }

                ForceHeartbeat(HeartbeatOptions.ChangesExpected);
            }

            // If it was a preview, we may now need to move on to the printing
            else if (e.PrintAction == PrintAction.Preview && !e.Cancelled)
            {
                StartPrintJob(job, PrintAction.Print);
            }
        }

        /// <summary>
        /// Send email using the template from the args
        /// </summary>
        private void OnEmailSendNow(object sender, TemplateNodeChangedEventArgs e)
        {
            TemplateEntity template = e.NewNode.Template;

            if (template.Type == (int) TemplateType.Thermal)
            {
                MessageHelper.ShowError(this,
                    "The selected template is for printing thermal labels.\n\n" +
                    "Thermal label data must be sent directly to a thermal printer, and cannot be emailed.");

                return;
            }

            // First check that there are any email accounts to send with
            if (EmailAccountManager.EmailAccounts.Count == 0)
            {
                if (UserSession.Security.HasPermission(PermissionType.ManageEmailAccounts))
                {
                    if (MessageHelper.ShowQuestion(this,
                        "There are no email accounts configured for sending email.\n\n" +
                        "Setup an account now?") == DialogResult.OK)
                    {
                        using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
                        {
                            dlg.ShowDialog(this);
                        }
                    }
                }
                else
                {
                    MessageHelper.ShowError(this,
                        "There are no email accounts configured.  An administrator must log on to setup email accounts.");
                }

                // Get out either way - it seemed weird to have it continue sending after being
                // distracted by adding the account
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Create the email generator using the default settings from the template
            EmailGenerator emailGenerator = EmailGenerator.Create(template, gridControl.Selection.OrderedKeys);

            // Show the progress window
            ProgressDlg progressDlg = new ProgressDlg(emailGenerator.ProgressProvider);
            progressDlg.Title = "Emailing";
            progressDlg.Description = "ShipWorks is emailing the selected items";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

            emailGenerator.ResolveSettings += new EmailSettingsResolveEventHandler(OnEmailResolveSettings);
            emailGenerator.EmailGenerationCompleted += new EmailGenerationCompletedEventHandler(OnEmailGenerationCompleted);
            emailGenerator.GenerateAsync(delayer);

            // Only show if time goes by
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));
        }

        /// <summary>
        /// User input is required to help resolve what email settings should be used during an email generation operation
        /// </summary>
        void OnEmailResolveSettings(object sender, EmailSettingsResolveEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((EmailSettingsResolveEventHandler) OnEmailResolveSettings, sender, e);
                return;
            }

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) ((EmailGenerator) sender).UserState;

            // Ensures that the progress window has been shown before moving on
            delayer.EnsureShown();

            // Show the window to the user for resolving the settings
            using (EmailSettingsResolveDlg dlg = new EmailSettingsResolveDlg(e))
            {
                dlg.ShowDialog(delayer.ProgressDlg);
            }
        }

        /// <summary>
        /// An asynchronous email action has completed
        /// </summary>
        private void OnEmailGenerationCompleted(object sender, EmailGenerationCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((EmailGenerationCompletedEventHandler) OnEmailGenerationCompleted, sender, e);
                return;
            }

            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) e.UserState;
            delayer.NotifyComplete();

            EmailGenerator emailGenerator = (EmailGenerator) sender;
            emailGenerator.EmailGenerationCompleted -= new EmailGenerationCompletedEventHandler(OnEmailGenerationCompleted);
            emailGenerator.ResolveSettings -= new EmailSettingsResolveEventHandler(OnEmailResolveSettings);

            // Check for errors
            if (e.Error != null)
            {
                // See if its an error we know how to handle.
                EmailException emailEx = e.Error as EmailException;
                if (emailEx != null)
                {
                    MessageHelper.ShowError(this, emailEx.Message);
                }

                // Rethrow the error as is
                else
                {
                    log.Error("Error while emailing", e.Error);
                    throw new InvalidOperationException(e.Error.Message, e.Error);
                }
            }

            if (e.EmailsGenerated.Count > 0)
            {
                EmailCommunicator.StartEmailingMessages(e.EmailsGenerated);
            }
            else if (!e.Cancelled)
            {
                if (e.SecurityDenials == 0)
                {
                    MessageHelper.ShowWarning(this, TemplateHelper.NoResultsErrorMessage);
                }
            }

            if (e.SecurityDenials > 0)
            {
                MessageHelper.ShowInformation(this,
                    string.Format("{0} messages were not sent due to insufficient permissions to send email.", e.SecurityDenials));
            }
        }

        /// <summary>
        /// Compose email using the template from the args
        /// </summary>
        private void OnEmailCompose(object sender, TemplateNodeChangedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            using (EmailComposerDlg dlg = new EmailComposerDlg(e.NewNode.Template, gridControl.Selection.OrderedKeys))
            {
                dlg.ShowDialog(this);

                if (dlg.EmailsGenerated.Count > 0)
                {
                    EmailCommunicator.StartEmailingMessages(dlg.EmailsGenerated);
                }
            }
        }

        /// <summary>
        /// Save using the template from the args
        /// </summary>
        private void OnSave(object sender, TemplateNodeChangedEventArgs e)
        {
            StartSaveWriter(e.NewNode.Template, false);
        }

        /// <summary>
        /// Save and open using the template from the args
        /// </summary>
        private void OnSaveOpen(object sender, TemplateNodeChangedEventArgs e)
        {
            StartSaveWriter(e.NewNode.Template, true);
        }

        /// <summary>
        /// Initiates the save writer indiciating if the files should be opened after they are saved.
        /// </summary>
        private void StartSaveWriter(TemplateEntity template, bool openAfterSave)
        {
            if (template.Type == (int) TemplateType.Thermal)
            {
                MessageHelper.ShowError(this,
                    "The selected template is for printing thermal labels.\n\n" +
                    "Thermal label data must be sent directly to a thermal printer, and cannot be saved.");

                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Create the save job using the default settings from the template
            SaveWriter writer = SaveWriter.Create(template, gridControl.Selection.OrderedKeys);

            // Show the progress window
            ProgressDlg progressDlg = new ProgressDlg(writer.ProgressProvider);
            progressDlg.Title = "Saving";
            progressDlg.Description = "ShipWorks is saving the selected items";

            ProgressDisplayDelayer delayer = new ProgressDisplayDelayer(progressDlg);

            writer.SaveCompleted += new SaveCompletedEventHandler(OnSaveCompleted);
            writer.PromptForFile += new SavePromptForFileEventHandler(OnSavePromptForFile);
            writer.SaveAsync(new object[] { delayer, openAfterSave });

            // Only show if time goes by
            delayer.ShowAfter(this, TimeSpan.FromSeconds(.5));
        }

        /// <summary>
        /// It is necessary to prompt the user for a file or folder name during a save operation
        /// </summary>
        void OnSavePromptForFile(object sender, SavePromptForFileEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((SavePromptForFileEventHandler) OnSavePromptForFile, sender, e);
                return;
            }

            object[] asyncState = (object[]) e.UserState;
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) asyncState[0];

            // Ensures that the progress window has been shown before moving on
            delayer.EnsureShown();

            // Browse for folder
            if (e.PartRequested == SaveFileNamePart.Folder)
            {
                using (FolderBrowserDialog dlg = new FolderBrowserDialog())
                {
                    dlg.Description = "Choose the location to save the files.";
                    dlg.SelectedPath = e.InitialName;

                    if (dlg.ShowDialog(delayer.ProgressDlg) == DialogResult.OK)
                    {
                        e.ResultName = dlg.SelectedPath;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
            // Browse for file
            else
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    if (!string.IsNullOrEmpty(e.InitialName))
                    {
                        dlg.FileName = Path.GetFileName(e.InitialName);
                        dlg.InitialDirectory = Path.GetDirectoryName(e.InitialName);
                    }

                    dlg.Filter = e.FileFilter;
                    dlg.DefaultExt = Path.GetExtension(e.InitialName);

                    dlg.ValidateNames = true;
                    dlg.OverwritePrompt = true;

                    if (dlg.ShowDialog(delayer.ProgressDlg) == DialogResult.OK)
                    {
                        e.ResultName = dlg.FileName;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// An asynchronous save action has completed
        /// </summary>
        private void OnSaveCompleted(object sender, SaveCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((SaveCompletedEventHandler) OnSaveCompleted, sender, e);
                return;
            }

            object[] asyncState = (object[]) e.UserState;
            ProgressDisplayDelayer delayer = (ProgressDisplayDelayer) asyncState[0];
            bool openAfterSave = (bool) asyncState[1];

            delayer.NotifyComplete();

            SaveWriter writer = (SaveWriter) sender;
            writer.SaveCompleted -= this.OnSaveCompleted;
            writer.PromptForFile -= this.OnSavePromptForFile;

            // Check for errors
            if (e.Error != null)
            {
                // See if its an error we know how to handle.
                SaveException savingEx = e.Error as SaveException;
                if (savingEx != null)
                {
                    MessageHelper.ShowError(this, savingEx.Message);
                }

                // Rethrow the error as is
                else
                {
                    log.Error("Error while saving", e.Error);
                    throw new ApplicationException(e.Error.Message, e.Error);
                }
            }
            else if (!e.Cancelled && e.SavedFiles.Count == 0)
            {
                MessageHelper.ShowWarning(this, TemplateHelper.NoResultsErrorMessage);
            }
            else if (openAfterSave)
            {
                int maxFiles = 10;

                try
                {
                    foreach (string file in e.SavedFiles.Take(maxFiles))
                    {
                        Process.Start(file);

                        // This waiting is b\c with certain apps - noteably IE - if you open stuff to fast it misses it.
                        Thread.Sleep(500);
                    }

                    if (e.SavedFiles.Count > maxFiles)
                    {
                        MessageHelper.ShowInformation(this, "ShipWorks has opened the first " + maxFiles + " of " + e.SavedFiles.Count + " files.");
                    }
                }
                catch (Win32Exception ex)
                {
                    MessageHelper.ShowError(this, "There was an error opening a file.\n\n" + ex.Message);
                }
            }
        }

        #region Ribbon Popup Handlers

        /// <summary>
        /// The quick print popup is opening
        /// </summary>
        private void OnQuickPrintRibbonPopup(object sender, BeforePopupEventArgs e)
        {
            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnQuickPrint);
        }

        /// <summary>
        /// The print popup is opening
        /// </summary>
        private void OnPrintRibbonPopup(object sender, BeforePopupEventArgs e)
        {
            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnPrint);
        }

        /// <summary>
        /// The preview popup is opening
        /// </summary>
        private void OnPrintPreviewRibbonPopup(object sender, BeforePopupEventArgs e)
        {
            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnPrintPreview);
        }

        /// <summary>
        /// The send email popup is opening
        /// </summary>
        private void OnEmailSendNowRibbonPopup(object sender, BeforePopupEventArgs e)
        {
            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnEmailSendNow);
        }

        /// <summary>
        /// The compose popup is opening
        /// </summary>
        private void OnEmailComposeRibbonPopup(object sender, BeforePopupEventArgs e)
        {
            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnEmailCompose);
        }

        /// <summary>
        /// The save popup is opening
        /// </summary>
        private void OnSaveRibbonPopup(object sender, BeforePopupEventArgs e)
        {
            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnSave);
        }

        /// <summary>
        /// The save & open popup is opening
        /// </summary>
        private void OnSaveOpenRibbonPopup(object sender, BeforePopupEventArgs e)
        {
            TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnSaveOpen);
        }

        #endregion

        #region Context Menu Handlers

        /// <summary>
        /// Quck Print menu is being opened
        /// </summary>
        private void OnQuickPrintMenuOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(this, (ToolStripMenuItem) sender, OnQuickPrint);
        }

        /// <summary>
        /// The Print context menu is being opened
        /// </summary>
        private void OnPrintMenuOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(this, (ToolStripMenuItem) sender, OnPrint);
        }

        /// <summary>
        /// The Preview context menu is being opened
        /// </summary>
        private void OnPreviewMenuOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(this, (ToolStripMenuItem) sender, OnPrintPreview);
        }

        /// <summary>
        /// The Email Now context menu is being opened
        /// </summary>
        private void OnEmailNowMenuOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(this, (ToolStripMenuItem) sender, OnEmailSendNow);
        }

        /// <summary>
        /// The compose email context menu is being opened
        /// </summary>
        private void OnComposeEmailMenuOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(this, (ToolStripMenuItem) sender, OnEmailCompose);
        }

        /// <summary>
        /// The save context menu is being opened
        /// </summary>
        private void OnSaveMenuOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(this, (ToolStripMenuItem) sender, OnSave);
        }

        /// <summary>
        /// The save and open context menu is being opened
        /// </summary>
        private void OnSaveAndOpenMenuOpening(object sender, EventArgs e)
        {
            TemplateTreePopupController.ShowMenuDropDown(this, (ToolStripMenuItem) sender, OnSaveOpen);
        }

        #endregion

        #endregion

    }
}
