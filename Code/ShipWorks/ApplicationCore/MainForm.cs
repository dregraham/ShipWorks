using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Autofac;
using Divelements.SandGrid;
using Divelements.SandRibbon;
using ICSharpCode.SharpZipLib.Zip;
using Interapptive.Shared;
using Interapptive.Shared.AutoUpdate;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Data;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.IO.Zip;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Interapptive.Shared.Win32;
using log4net;
using ShipWorks.Actions;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Appearance;
using ShipWorks.ApplicationCore.Crashes;
using ShipWorks.ApplicationCore.Dashboard;
using ShipWorks.ApplicationCore.Dashboard.Content;
using ShipWorks.ApplicationCore.Enums;
using ShipWorks.ApplicationCore.Help;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Licensing.Warehouse.Messages;
using ShipWorks.ApplicationCore.MessageBoxes;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Carriers.Services;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Common.Threading;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Administration.SqlServerSetup;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Email.Outlook;
using ShipWorks.Filters;
using ShipWorks.Filters.Controls;
using ShipWorks.Filters.Grid;
using ShipWorks.Filters.Management;
using ShipWorks.Filters.Search;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Orders;
using ShipWorks.Messaging.Messages.Panels;
using ShipWorks.OrderLookup;
using ShipWorks.Products;
using ShipWorks.Properties;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Asendia;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.UPS.OneBalance;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.ShipEngine.Manifest;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Stores;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Orders.Archive;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Stores.Services;
using ShipWorks.Templates;
using ShipWorks.Templates.Controls;
using ShipWorks.Templates.Controls.DefaultPickListTemplate;
using ShipWorks.Templates.Emailing;
using ShipWorks.Templates.Management;
using ShipWorks.Templates.Printing;
using ShipWorks.Templates.Saving;
using ShipWorks.UI;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Logon;
using ShipWorks.Users.Security;
using ShipWorks.Warehouse.Configuration;
using ShipWorks.Warehouse.Configuration.Stores;
using TD.SandDock;
using static Interapptive.Shared.Utility.Functional;
using Application = System.Windows.Forms.Application;
using SandButton = Divelements.SandRibbon.Button;
using SandComboBox = Divelements.SandRibbon.ComboBox;
using SandLabel = Divelements.SandRibbon.Label;
using SandMenuItem = Divelements.SandRibbon.MenuItem;

namespace ShipWorks
{
	/// <summary>
	/// Main window of the application.
	/// </summary>
	[NDependIgnoreLongTypes]
	public partial class MainForm : RibbonForm, IMainForm
	{
		// Logger
		private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

		// Indicates if the background async login was a success.  This is to make sure we don't keep going on the UI thread
		// if it failed.
		private bool logonAsyncLoadSuccess = false;
		private readonly ConcurrentQueue<Action> logonActions = new ConcurrentQueue<Action>();

		// We have to remember these so that we can restore them after blanking the UI
		private readonly List<RibbonTab> ribbonTabs = new List<RibbonTab>();

		// Used to manage the UI state of the online update commands
		private readonly OnlineUpdateCommandProvider onlineUpdateCommandProvider = new OnlineUpdateCommandProvider();

		// Used to keep ShipWorks "pumping" looking for data changes
		private Heartbeat heartBeat;

		// The FilterNode to restore if search is canceled
		private long searchRestoreFilterNodeID = 0;
		private readonly Lazy<DockControl> shipmentDock;
		private ILifetimeScope menuCommandLifetimeScope;
		private IArchiveNotificationManager archiveNotificationManager;
		private readonly ICurrentUserSettings currentUserSettings;
		private ILifetimeScope orderLookupLifetimeScope;
		private ILifetimeScope productsLifetimeScope;
		private IOrderLookup orderLookupControl;
		private string orderLookupOrderShortcut = string.Empty;
		private IShipmentHistory shipmentHistory;
		private IUpdateService updateService;
		private IProductsMode productsMode;
		private readonly string unicodeCheckmark = $"    {'\u2714'.ToString()}";
		DeviceIdentificationControl deviceIdentificationControl;

		// Is ShipWorks currently changing modes
		private bool isChangingMode = false;

		/// <summary>
		/// Constructor
		/// </summary>
		[SuppressMessage("Recommendations", "RECS0026: Possible unassigned object created by 'new' expression",
			Justification = "The WindowStateSaver's constructor does the work, so we don't need to store the variable.")]
		public MainForm()
		{
			currentUserSettings = IoC.UnsafeGlobalLifetimeScope.Resolve<ICurrentUserSettings>();

			InitializeComponent();

			panelDockingArea.Visible = false;

			foreach (IMainFormElementRegistration registration in IoC.UnsafeGlobalLifetimeScope.Resolve<IEnumerable<IMainFormElementRegistration>>())
			{
				registration.Register(sandDockManager, ribbon);
			}

			// Create the heartbeat
			heartBeat = new BatchModeUIHeartbeat(this);

			// Persist size\position of the window
			WindowStateSaver.Manage(this, WindowStateSaverOptions.FullState | WindowStateSaverOptions.InitialMaximize, "MainForm");
			shipmentDock = new Lazy<DockControl>(GetShipmentDockControl);

			InitializeCustomEnablerComponents();

			SetShipmentButtonEnabledState();

			SetButtonTelemetry();
		}

		/// <summary>
		/// Wire up updating the CreateLabel and ApplyProfile enabled state
		/// </summary>
		private void SetShipmentButtonEnabledState()
		{
			// Listen for message to enable Create Label button
			Messenger.Current
				.Subscribe(x =>
			{

				// The reason we are checking UIMode after the message is that when we add it to a Where above the subscribe,
				// UIMode is null when we get our first message.

				if (x is ShipmentSelectionChangedMessage && currentUserSettings.GetUIMode() == UIMode.OrderLookup)
				{
					var canProcess = orderLookupControl?.CreateLabelAllowed() == true;
					buttonOrderLookupViewCreateLabel.Enabled = canProcess;
					buttonOrderLookupViewApplyProfile.Enabled = canProcess;

					buttonOrderLookupViewNewShipment.Enabled = orderLookupControl?.CreateNewShipmentAllowed() == true;
					buttonOrderLookupViewShipShipAgain.Enabled = orderLookupControl?.ShipAgainAllowed() == true;
                    buttonOrderLookupViewReprint2.Enabled = orderLookupControl?.ReprintAllowed() == true;

                }

				if ((x is ShipmentSelectionChangedMessage || x is OrderLookupUnverifyMessage || x is OrderVerifiedMessage) &&
				currentUserSettings.GetUIMode() == UIMode.OrderLookup)
				{
					buttonOrderLookupViewUnverify.Enabled = orderLookupControl?.UnverifyOrderAllowed() == true;
                    buttonOrderLookupViewReprint2.Enabled = orderLookupControl?.ReprintAllowed() == true;
                }

				if (x is OrderLookupClearOrderMessage && currentUserSettings.GetUIMode() == UIMode.OrderLookup)
				{
					buttonOrderLookupViewCreateLabel.Enabled = false;
					buttonOrderLookupViewApplyProfile.Enabled = false;
					buttonOrderLookupViewNewShipment.Enabled = false;
					buttonOrderLookupViewShipShipAgain.Enabled = false;
                    buttonOrderLookupViewReprint2.Enabled = false;
                }
			});
		}

		/// <summary>
		/// Setup the SetEnabledWhen for components that need actions to determine enabled state
		/// </summary>
		private void InitializeCustomEnablerComponents()
		{
			selectionDependentEnabler.SetEnabledWhen(buttonCombine,
				SelectionDependentType.AppliesFunction,
				ShouldCombineOrderBeEnabled);

			selectionDependentEnabler.SetEnabledWhen(buttonSplit,
				SelectionDependentType.AppliesFunction,
				ShouldSplitOrderBeEnabled);

			selectionDependentEnabler.SetEnabledWhen(contextOrderCombineOrder,
				SelectionDependentType.AppliesFunction,
				ShouldCombineOrderBeEnabled);

			selectionDependentEnabler.SetEnabledWhen(contextOrderSplitOrder,
				SelectionDependentType.AppliesFunction,
				ShouldSplitOrderBeEnabled);
		}

		/// <summary>
		/// Disable the SetEnabledWhen for components that need actions to determine enabled state
		/// </summary>
		private IDisposable DisableCustomEnablerComponents()
		{
			selectionDependentEnabler.SetEnabledWhen(buttonCombine, SelectionDependentType.Ignore);
			selectionDependentEnabler.SetEnabledWhen(buttonSplit, SelectionDependentType.Ignore);
			selectionDependentEnabler.SetEnabledWhen(contextOrderCombineOrder, SelectionDependentType.Ignore);
			selectionDependentEnabler.SetEnabledWhen(contextOrderSplitOrder, SelectionDependentType.Ignore);

			return Disposable.Create(InitializeCustomEnablerComponents);
		}

		/// <summary>
		/// Collection of panels on the main form
		/// </summary>
		public IEnumerable<DockControl> Panels => sandDockManager.GetDockControls();

		/// <summary>
		/// Get the current UIMode
		/// </summary>
		public UIMode UIMode { get; private set; }

		/// <summary>
		/// User clicks the Create Label button in Order Lookup Mode
		/// </summary>
		private void OnButtonOrderLookupViewCreateLabel(object sender, EventArgs e)
		{
			orderLookupControl.CreateLabel().Forget();
		}

		/// <summary>
		/// User clicks the New Shipment button in Scan to Ship (formerly Order Lookup) mode
		/// </summary>
		private void OnButtonOrderLookupViewNewShipment(object sender, EventArgs e)
		{
			orderLookupControl.CreateNewShipment();
		}

		/// <summary>
		/// User clicks the Ship Again button in Order Lookup Mode
		/// </summary>
		private void OnButtonOrderLookupViewShipAgain(object sender, EventArgs e)
		{
			orderLookupControl.ShipAgain();
		}

		/// <summary>
		/// User clicks the OrderLookupViewUnverify button
		/// </summary>
		private void OnButtonOrderLookupViewUnverify(object sender, EventArgs e)
		{
			orderLookupControl.Unverify();
		}

        /// <summary>
        /// User clicks the OrderLookupViewUnverify button
        /// </summary>
        private void OnButtonOrderLookupViewReprint(object sender, EventArgs e)
        {
            orderLookupControl.Reprint();
        }

        #region Initialization \ Shutdown

        /// <summary>
        /// Attempt to auto update
        /// </summary>
        /// <returns>true if we kicked off the auto update process</returns>
        private bool AutoUpdate()
		{
			if (SqlSession.IsConfigured)
			{
				updateService = IoC.UnsafeGlobalLifetimeScope.Resolve<IUpdateService>();

				updateService.ListenForAutoUpdateStart(this);

				Result result = updateService.TryUpdate();

				return result.Success;
			}

			return false;
		}

		/// <summary>
		/// Form is loading, this is before its visible
		/// </summary>
		[NDependIgnoreLongMethod]
		private void OnLoad(object sender, EventArgs e)
		{
			new AutoUpdateStatusProvider().CloseSplashScreen();

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
			foreach (DockControl dockControl in Panels.OrderBy(d => d.Text))
			{
				SandMenuItem menuItem = new SandMenuItem(dockControl.Text);
				menuItem.Image = dockControl.TabImage;
				menuItem.Tag = dockControl;
				menuItem.Activate += new EventHandler(OnShowPanel);
				menuShowPanels.Items.Add(menuItem);
			}

			// Prepare app level initialization
			DashboardManager.InitializeForApplication(dashboardArea);
			archiveNotificationManager = IoC.UnsafeGlobalLifetimeScope.Resolve<IArchiveNotificationManager>(TypedParameter.From(panelArchiveNotification));
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
			ribbonSecurityProvider.AddAdditionalCondition(buttonOrderLookupViewFedExClose, () => FedExAccountManager.Accounts.Count > 0);
			ribbonSecurityProvider.AddAdditionalCondition(buttonAsendiaClose, AreThereAnyAsendiaAccounts);
			ribbonSecurityProvider.AddAdditionalCondition(buttonOrderLookupViewAsendiaClose, AreThereAnyAsendiaAccounts);
			ribbonSecurityProvider.AddAdditionalCondition(buttonDhlEcommerceManifest, AreThereAnyDhlEcommerceAccounts);
			ribbonSecurityProvider.AddAdditionalCondition(buttonOrderLookupViewDhlEcommerceManifest, AreThereAnyDhlEcommerceAccounts);
			ribbonSecurityProvider.AddAdditionalCondition(buttonEndiciaSCAN, AreThereAnyPostalAccounts);
			ribbonSecurityProvider.AddAdditionalCondition(buttonOrderLookupViewSCANForm, AreThereAnyPostalAccounts);
			ribbonSecurityProvider.AddAdditionalCondition(buttonFirewall, () => SqlSession.IsConfigured && !SqlSession.Current.Configuration.IsLocalDb());
			ribbonSecurityProvider.AddAdditionalCondition(buttonChangeConnection, () => SqlSession.IsConfigured && !SqlSession.Current.Configuration.IsLocalDb());

			// Prepare stuff that needs prepare for dealing with UI changes for Editions
			PrepareEditionManagedUI();

			// Start off with a blank looking UI
			ShowBlankUI();

			// Load default display settings
			ShipWorksDisplay.LoadDefault();

			ApplyDisplaySettings();

			ApplyEditingContext();

			SqlSession.Initialize();

			AutoUpdateTelemetryCollector.CollectTelemetry(ShipWorksSession.InstanceID);

			deviceIdentificationControl = new DeviceIdentificationControl() { Size = new Size(0, 0) };
			Controls.Add(deviceIdentificationControl);

			if (AutoUpdate())
			{
				Close();
			}
		}

		/// <summary>
		/// The device Identify needed to register a UPS account. I don't like this being here either...
		/// </summary>
		public string DeviceIdentity => deviceIdentificationControl.Identity;

		/// <summary>
		/// Checks whether we have an Asendia account
		/// </summary>
		private static bool AreThereAnyAsendiaAccounts() =>
			Using(IoC.BeginLifetimeScope(),
				scope => scope.Resolve<IAsendiaAccountRepository>().AccountsReadOnly.Any());

		/// <summary>
		/// Checks whether we have a DHL eCommerce account
		/// </summary>
		private static bool AreThereAnyDhlEcommerceAccounts() =>
			Using(IoC.BeginLifetimeScope(),
				scope => scope.Resolve<IDhlEcommerceAccountRepository>().AccountsReadOnly.Any());

		/// <summary>
		/// Checks whether we have any postal accounts
		/// </summary>
		private static bool AreThereAnyPostalAccounts() =>
			Using(IoC.BeginLifetimeScope(),
				scope => scope.Resolve<IEnumerable<IScanFormAccountRepository>>().Any(x => x.HasAccounts));

		/// <summary>
		/// Main form has been made visible
		/// </summary>
		private void OnShown(object sender, EventArgs e)
		{
			// Its visible, but possibly not completely drawn
			Refresh();

			// If the action is to open the DB setup, we can do that now - no need to logon first.
			if (StartupController.StartupAction == StartupAction.OpenDatabaseSetup)
			{
				using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
				{
					using (DetailedDatabaseSetupWizard dlg = new DetailedDatabaseSetupWizard(lifetimeScope))
					{
						dlg.ShowDialog(this);
					}
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

			// If its still not setup, don't go on
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
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				// If we aren't configured at all
				if (!SqlSession.IsConfigured)
				{
					SqlServerInstaller sqlServerInstaller = lifetimeScope.Resolve<SqlServerInstaller>();

					// If we aren't configured and 2012 is supported, open the fast track setup wizard
					if (sqlServerInstaller.IsSqlServer2017Supported || sqlServerInstaller.IsSqlServer2014Supported)
					{
						using (SimpleDatabaseSetupWizard wizard = new SimpleDatabaseSetupWizard(lifetimeScope))
						{
							return wizard.ShowDialog(this) == DialogResult.OK;
						}
					}
					else
					{
						using (DetailedDatabaseSetupWizard wizard = new DetailedDatabaseSetupWizard(lifetimeScope))
						{
							return wizard.ShowDialog(this) == DialogResult.OK;
						}
					}
				}
				// Otherwise, we use our normal database setup wizard
				else
				{
					using (DatabaseDetailsDlg dlg = new DatabaseDetailsDlg(lifetimeScope))
					{
						dlg.ShowDialog(this);

						return dlg.DatabaseConfigurationChanged;
					}
				}
			}
		}

		/// <summary>
		/// The window is about to close
		/// </summary>
		/// <remarks>This is an override instead of an event handler to ensure that any other event handlers run before
		/// this code.  That's because this clears the user session which is needed by other components on shutdown.</remarks>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

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

			if (CrashDialog.IsApplicationCrashed)
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
		public void InitiateLogon() => InitiateLogon(null);

		/// <summary>
		/// Initiate the process of logging on to the system
		/// </summary>
		public void InitiateLogon(UserEntity user)
		{
			if (!SqlSession.IsConfigured)
			{
				throw new InvalidOperationException("Cannot initiate log on without a valid SqlSession.");
			}

			ShowBlankUI();

			if (LogonToSqlServer())
			{
				// Make sure that change tracking is enabled for the database and all applicable tables.
				SqlChangeTracking sqlChangeTracking = new SqlChangeTracking();
				sqlChangeTracking.Enable();

				LogonToShipWorks(user);

				if (UserSession.IsLoggedOn)
				{
					DashboardManager.ShowOneBalancePromo();

					InsuranceUtility.GetInsuranceRates();

					// If we successfully logged on, the last update must have succeeded.
					AutoUpdateSettings.LastAutoUpdateSucceeded = true;

					ShipSenseLoader.LoadDataAsync();
				}
			}
			else
			{
				UserSession.Reset();
			}
		}

		/// <summary>
		/// Initiate the process of logging off the system.
		/// </summary>
		public bool InitiateLogoff(bool clearRememberMe)
		{
			// This causes the shipping panel to lose focus, which causes it to save. If we don't do this, it will try
			// to save later, after the user has logged out. This caused an exception because we couldn't audit the save.
			// This was moved to its current location because if we're crashing, calling Focus can cause a cross-thread
			// exception, preventing ShipWorks from shutting down. If we're crashing, we can't be sure a save is valid anyway.
			Focus();
			UnloadOrderLookupMode();

			// Don't need a scope if we're already in one
			using (ConnectionSensitiveScope scope = (ConnectionSensitiveScope.IsActive ? null : new ConnectionSensitiveScope("log off", this)))
			{
				if (scope != null && !scope.Acquired)
				{
					return false;
				}

				SaveCurrentUserSettings();

				shipmentHistory?.Dispose();
				shipmentHistory = null;

				UserSession.Logoff(clearRememberMe);
			}

			// Can't do anything when logged off
			ShowBlankUI();

			return true;
		}

		/// <summary>
		/// Open the window for logging on to SQL Server
		/// </summary>
		[NDependIgnoreLongMethod]
		private bool LogonToSqlServer()
		{
			bool canConnect = SqlSession.Current.CanConnect();

			// If we couldn't connect, see if it's b\c another ShipWorks is upgrading or restoring
			if (!canConnect)
			{
				try
				{
					SqlSession master = new SqlSession(SqlSession.Current);
					master.Configuration.DatabaseName = "master";

					SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(master.Configuration.GetConnectionString());
					csb.Pooling = false;
					var connectionString = csb.ToString();

					using (DbConnection testConnection = new SqlConnection(connectionString))
					{
						testConnection.Open();

						if (SqlUtility.RenameArchvingDbIfNeeded(testConnection, SqlSession.Current.Configuration.DatabaseName))
						{
							canConnect = SqlSession.Current.CanConnect();
						}

						if (!canConnect && SqlUtility.IsSingleUser(connectionString, SqlSession.Current.Configuration.DatabaseName))
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
			using (DbConnection con = SqlSession.Current.OpenConnection())
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
		[NDependIgnoreLongMethod]
		private void LogonToShipWorks(UserEntity logonAsUser)
		{
			UserManager.InitializeForCurrentUser();

			// May already be logged on
			if (!UserSession.IsLoggedOn && !AttemptLogin(logonAsUser))
			{
				return;
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

			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				if (lifetimeScope.Resolve<IQuickStart>().ShouldShow)
				{
					QueueLogonAction(ShowQuickStart);
				}
			}

			// If there are no stores, we need to make sure one is added before continuing
			if (StoreManager.GetDatabaseStoreCount() == 0)
			{
				if (!AddStoreWizard.RunWizard(this, OpenedFromSource.NoStores))
				{
					UserSession.Logoff(false);
					UserSession.Reset();

					ShowBlankUI();

					return;
				}
			}
			else
			{
				using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
				{
					ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
					try
					{
						licenseService.GetLicenses().FirstOrDefault()?.EnforceCapabilities(EnforcementContext.Login, this);
					}
					catch (ShipWorksLicenseException ex)
					{
						// The enforcer threw a ShipWorksLicenseException
						MessageHelper.ShowError(this, ex.Message);

						// Log off
						UserSession.Logoff(false);
						UserSession.Reset();
						ShowBlankUI();
						return;
					}
				}
			}

			ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(LogonToShipWorksAsyncGetLicenseStatus, "checking license status"));

			UserEntity user = UserSession.User;

			CheckForMessages();

			MigrateStoresToHub();

			SynchronizeConfig();

            MigrateFedExAccountsToShipEngine();

            MigrateSqlConfigToHub();

			ConvertLegacyTrialStores();

			ConvertAmazonStoresToSP();

			FetchAmazonOrderSourceIDs();

			// Update the custom actions UI.  Has to come before applying the layout, so the QAT can pickup the buttons
			UpdateCustomButtonsActionsUI();

			// We can now show the normal UI
			ApplyCurrentUserLayout();

			currentUserSettings.SetUIMode(UIMode.Batch);

			//The Grid can be already initialized at this point. Make sure its clean.
			gridControl.Reset();

			// Display the appropriate UI mode for this user. Don't start heartbeat here, need other code to run first
			UpdateUIMode(user, false, ModeChangeBehavior.ChangeTabAlways);

			log.InfoFormat("UI shown");

			// Start the dashboard.  Has to be before updating store depending UI - as that affects dashboard display.
			DashboardManager.OpenDashboard();

			archiveNotificationManager.ShowIfNecessary();

			// Update the Detail View UI
			UpdateDetailViewSettingsUI();

			// Check the disk usage of SQL server to warn the user if they are about out
			CheckDatabaseDiskUsage();

			// Start the heartbeat
			heartBeat.Start();

			ExecuteLogonActions();

			UsingAsync(
					IoC.BeginLifetimeScope(),
					lifetimeScope => lifetimeScope.Resolve<IAmazonTermsOrchestrator>().Handle()
				)
				.Do(x => { }, ex => Console.WriteLine(ex.Message))
				.Forget();

			// Update the nudges from Tango and show any upgrade related nudges
			NudgeManager.Initialize(StoreManager.GetAllStores());
			NudgeManager.ShowNudge(this, NudgeManager.GetFirstNudgeOfType(NudgeType.ShipWorksUpgrade));

			// Start auto downloading immediately
			DownloadManager.StartAutoDownloadIfNeeded(true);
			ribbon.Minimized = UserSession.User.Settings.MinimizeRibbon;
			ribbon.ToolBarPosition = UserSession.User.Settings.ShowQAToolbarBelowRibbon ?
																QuickAccessPosition.Below :
																QuickAccessPosition.Above;

			// Then, if we are downloading any stores for the very first time, auto-show the progress
			if (StoreManager.GetLastDownloadTimes().Any(pair => pair.Value == null && DownloadManager.IsDownloading(pair.Key)))
			{
				ShowDownloadProgress();
			}

			SendPanelStateMessages();

			UsingAsync(
				IoC.BeginLifetimeScope(),
				lifetimeScope => lifetimeScope.Resolve<IUpgradeResultsChecker>().ShowUpgradeNotificationIfNecessary(this, user))
				.Do(x => { }, ex => Console.WriteLine(ex.Message))
				.Forget();
		}

		/// <summary>
		/// Attempt to logon
		/// </summary>
		private bool AttemptLogin(UserEntity logonAsUser)
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				IUserService userService = lifetimeScope.Resolve<IUserService>();
				EnumResult<UserServiceLogonResultType> logonResult;

				try
				{
					logonResult = userService.Logon();
				}
				catch (EncryptionException ex)
				{
					log.Error("Error logging in", ex);

					IDialog customerLicenseActivation = lifetimeScope.ResolveNamed<IDialog>("CustomerLicenseActivationDlg");
					customerLicenseActivation.LoadOwner(this);
					customerLicenseActivation.DataContext =
						lifetimeScope.Resolve<ICustomerLicenseActivartionDlgViewModel>();

					if (customerLicenseActivation.ShowDialog() ?? false)
					{
						logonResult = userService.Logon();
					}
					else
					{
						return false;
					}
				}

				if (logonResult.Value == UserServiceLogonResultType.TangoAccountDisabled)
				{
					MessageHelper.ShowError(this, logonResult.Message);
					return false;
				}

				if (logonResult.Value == UserServiceLogonResultType.InvalidCredentials && logonAsUser != null)
				{
					logonResult = userService.Logon(logonAsUser, true);
				}

				if (logonResult.Value == UserServiceLogonResultType.InvalidCredentials)
				{
					using (LogonDlg dlg = new LogonDlg())
					{
						if (dlg.ShowDialog(this) != DialogResult.OK)
						{
							return false;
						}
					}
				}
			}

			if (!CheckDatabaseVersion())
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Show the batch view
		/// </summary>
		private void OnShowBatchView(object sender, EventArgs e) => ChangeUIMode(UIMode.Batch);

		/// <summary>
		/// Show the order lookup view
		/// </summary>
		private void OnShowOrderLookupView(object sender, EventArgs e) => ChangeUIMode(UIMode.OrderLookup);

		/// <summary>
		/// Show the products view
		/// </summary>
		private void OnShowProductsView(object sender, EventArgs e) => ChangeUIMode(UIMode.Products);


		/// <summary>
		/// Change to the given UI mode
		/// </summary>
		private void ChangeUIMode(UIMode uiMode) => ChangeUIMode(uiMode, ModeChangeBehavior.ChangeTabAlways);

		/// <summary>
		/// Change to the given UI mode
		/// </summary>
		private void ChangeUIMode(UIMode uiMode, ModeChangeBehavior modeChangeBehavior)
		{
			if (isChangingMode)
			{
				return;
			}

			using (StartChangingModes())
			{
				if (UIMode == uiMode)
				{
					return;
				}

				SaveCurrentUserSettings();

				currentUserSettings.SetUIMode(uiMode);

				OrderEntity selectedOrder = null;
				// If there is a single order selected, save it so we can pull it up in the new mode.
				if (gridControl.Selection.Keys.IsCountEqualTo(1))
				{
					selectedOrder = OrderUtility.FetchOrder(gridControl.Selection.Keys.First());
				}

				UpdateUIMode(UserSession.User, true, modeChangeBehavior);

				// If we went from batch mode to scan to ship mode and we had an order selected, load it in scan to ship.
				if (UIMode == UIMode.OrderLookup && selectedOrder != null)
				{
					Messenger.Current.Send(new OrderLookupLoadOrderMessage(this, selectedOrder));
				}
			}
		}

		/// <summary>
		/// Start changing a mode
		/// </summary>
		private IDisposable StartChangingModes()
		{
			isChangingMode = true;

			return Disposable.Create(() => isChangingMode = false);
		}

		/// <summary>
		/// Update the UI
		/// </summary>
		private void UpdateUIMode(IUserEntity user, bool startHeartbeat, ModeChangeBehavior modeChangeBehavior)
		{
			bool hasProductsPermissions = UserSession.Security.HasPermission(PermissionType.ManageProducts);
			mainMenuItemProducts.Visible = hasProductsPermissions;

			heartBeat?.Stop();

			UnloadOrderLookupMode();
			UnloadProductsMode();

			UIMode currentMode = currentUserSettings.GetUIMode();
			if (currentMode == UIMode.Products && !hasProductsPermissions)
			{
				currentMode = UIMode.Batch;
			}

			ToggleUiModeCheckbox(currentMode);

			EnableUiMode(currentMode, user);

			UIMode = currentMode;

			EnableRibbonTabs();

			UpdateStatusBar();

			// Notify telemetry of the UI Mode change
			Telemetry.SetUserInterfaceView(EnumHelper.GetDescription(UIMode));

			if (startHeartbeat)
			{
				heartBeat.Start();
			}

			panelDockingArea.Visible = true;

			SelectModeAppropriateTab(modeChangeBehavior);
		}

		/// <summary>
		/// Ensure we are currently in batch mode
		/// </summary>
		private void EnsureBatchMode()
		{
			ChangeUIMode(UIMode.Batch, ModeChangeBehavior.ChangeTabIfNotAppropriate);
		}

		/// <summary>
		/// Select a default tab for the given mode if one is not already selected
		/// </summary>
		private void SelectModeAppropriateTab(ModeChangeBehavior modeChangeBehavior)
		{
			if (modeChangeBehavior == ModeChangeBehavior.ChangeTabIfNotAppropriate && IsModeAgnosticTab(ribbon.SelectedTab))
			{
				return;
			}

			if (UIMode == UIMode.Batch && !IsBatchSpecificTab(ribbon.SelectedTab))
			{
				ribbon.SelectedTab = ribbonTabGridViewHome;
			}
			else if (UIMode == UIMode.OrderLookup && !IsOrderLookupSpecificTab(ribbon.SelectedTab))
			{
				ribbon.SelectedTab = ribbonTabOrderLookupViewShipping;
			}
			else if (UIMode == UIMode.Products && !IsProductSpecificTab(ribbon.SelectedTab))
			{
				ribbon.SelectedTab = ribbonTabProducts;
			}
		}

		/// <summary>
		/// Is the tab mode agnostic
		/// </summary>
		private bool IsModeAgnosticTab(RibbonTab selectedTab) =>
			selectedTab == ribbonTabGridOutput ||
			selectedTab == ribbonTabAdmin ||
			selectedTab == ribbonTabView ||
			selectedTab == ribbonTabHelp;

		/// <summary>
		/// Enables the given UI mode
		/// </summary>
		private void EnableUiMode(UIMode mode, IUserEntity user)
		{
			if (mode == UIMode.OrderLookup)
			{
				EnableOrderLookupMode();
			}
			else if (mode == UIMode.Products)
			{
				EnableProductsMode(user);
			}
			else
			{
				EnableBatchMode(user);
			}
		}

		/// <summary>
		/// Update the UI mode check boxes
		/// </summary>
		private void ToggleUiModeCheckbox(UIMode currentMode)
		{
			mainMenuItemBatchGrid.Text = GetTextForModeMenuItem("Classic", currentMode == UIMode.Batch);
			mainMenuItemOrderLookup.Text = GetTextForModeMenuItem("Scan-to-Ship", currentMode == UIMode.OrderLookup);
			mainMenuItemProducts.Text = GetTextForModeMenuItem("Products", currentMode == UIMode.Products);
		}

		/// <summary>
		/// Get the text for a mode menu item
		/// </summary>
		private string GetTextForModeMenuItem(string text, bool isSelected) =>
			isSelected ? text + unicodeCheckmark : text;

		/// <summary>
		/// Switch from order lookup to batch mode
		/// </summary>
		private void EnableBatchMode(IUserEntity user)
		{
			heartBeat = new BatchModeUIHeartbeat(this);

			try
			{
				windowLayoutProvider.LoadLayout(user.Settings.WindowLayout);
			}
			catch (AppearanceException)
			{
				windowLayoutProvider.LoadDefault();
				MessageHelper.ShowMessage(this,
				"Your appearance settings file has been corrupted. Appearance settings have been reset to the defaults.");

				//Ensure that the defaults are saved.
				SaveCurrentUserSettings();
			}

			gridMenuLayoutProvider.LoadLayout(user.Settings.GridMenuLayout);

			foreach (DockingPanelContentHolder holder in GetDockingPanelContentHolders())
			{
				holder.InitializeForCurrentUser();
			}

			// Initialize the grid
			gridControl.InitializeForTarget(FilterTarget.Orders, contextMenuOrderGrid, contextOrderCopy);
			gridControl.InitializeForTarget(FilterTarget.Customers, contextMenuCustomerGrid, contextCustomerCopy);

			// Initialize any filter trees
			InitializeFilterTrees(user);

			// Update all UI items that are related to the current stores
			UpdateStoreDependentUI();

			gridControl.Show();
			orderFilterTree.Show();
			customerFilterTree.Show();

			// Select the active filter
			SelectInitialFilter(user.Settings);

			SendPanelStateMessages();

			if (!string.IsNullOrWhiteSpace(orderLookupOrderShortcut))
			{
				gridControl.PerformBarcodeSearch(orderLookupOrderShortcut);
			}
		}

		/// <summary>
		/// Switch from order lookup to batch mode
		/// </summary>
		private void EnableProductsMode(IUserEntity user)
		{
			heartBeat = new BasicUIHeartbeat(this);

			DisableBatchModeControls();

			productsLifetimeScope = IoC.BeginLifetimeScope();

			productsMode = productsLifetimeScope.Resolve<IProductsMode>();

			productsMode.Initialize(panelDockingArea.Controls.Add, panelDockingArea.Controls.Remove);

			AttachProductEventHandlers();
		}

		/// <summary>
		/// Attach event handlers to product mode buttons
		/// </summary>
		private void AttachProductEventHandlers()
		{
			addProductMenuItemNewProduct.Activate += OnAddNewProduct;
			addProductMenuItemFromFile.Activate += OnAddProductFromFile;
			addProductMenuItemVariant.Activate += OnAddProductVariant;

			buttonProductCatalogEditProduct.Activate += OnEditProduct;
			buttonProductCatalogExportProduct.Activate += OnExportProduct;

			if (productsMode != null)
			{
				productsMode.ProductSelectionChanged += OnProductSelectionChanged;
			}
		}

		/// <summary>
		/// Add New Product event handler
		/// </summary>
		private void OnAddNewProduct(object sender, EventArgs e)
		{
			productsMode?.AddProduct.Execute(null);
		}

		/// <summary>
		/// Add Product From File event handler
		/// </summary>
		private void OnAddProductFromFile(object sender, EventArgs e)
		{
			productsMode?.ImportProducts.Execute(null);
		}

		/// <summary>
		/// Add Product Variant event handler
		/// </summary>
		private void OnAddProductVariant(object sender, EventArgs e)
		{
			productsMode?.CopyAsVariant.Execute(null);
		}

		/// <summary>
		/// Edit Product event handler
		/// </summary>
		private void OnEditProduct(object sender, EventArgs e)
		{
			productsMode?.EditProductVariantButton.Execute(null);
		}

		/// <summary>
		/// Export product event handler
		/// </summary>
		private void OnExportProduct(object sender, EventArgs e)
		{
			productsMode?.ExportProducts.Execute(null);
		}

		/// <summary>
		/// Product selection event handler
		/// </summary>
		private void OnProductSelectionChanged(object sender, ProductSelectionChangedEventArgs e)
		{
			if (e.SingleSelection)
			{
				addProductMenuItemVariant.Enabled = true;
				buttonProductCatalogEditProduct.Enabled = true;
			}
			else
			{
				addProductMenuItemVariant.Enabled = false;
				buttonProductCatalogEditProduct.Enabled = false;
			}
		}

		/// <summary>
		/// Switch from batch to order lookup mode
		/// </summary>
		private void EnableOrderLookupMode()
		{
			// clear out any selected orders from the batch view
			Messenger.Current.Send(new OrderSelectionChangingMessage(this, new long[0]));

			heartBeat = new BasicUIHeartbeat(this);

			DisableBatchModeControls();

			orderLookupLifetimeScope = IoC.BeginLifetimeScopeWithOverrides<IOrderLookupRegistrationOverride>();

			foreach (IScanToShipPipeline service in orderLookupLifetimeScope.Resolve<IEnumerable<IScanToShipPipeline>>())
			{
				service.InitializeForCurrentScope();
			}

			orderLookupControl = orderLookupLifetimeScope.Resolve<IOrderLookup>();

			var profilePopupService = orderLookupLifetimeScope.Resolve<IProfilePopupService>();
			orderLookupControl.RegisterProfileHandler(
				(getShipment, onApply) => profilePopupService.BuildMenu(
					buttonOrderLookupViewApplyProfile,
					new Guid("98602808-B402-48F9-A5D1-EE10FCE16565"),
					getShipment,
					onApply));

			// Since no order will be selected when they change modes, set these to false.
			buttonOrderLookupViewCreateLabel.Enabled = false;
			buttonOrderLookupViewApplyProfile.Enabled = false;
			buttonOrderLookupViewNewShipment.Enabled = false;
			buttonOrderLookupViewShipShipAgain.Enabled = false;
			buttonOrderLookupViewUnverify.Enabled = false;
            buttonOrderLookupViewReprint2.Enabled = false;
        }

		/// <summary>
		/// Unload the Order Lookup Mode
		/// </summary>
		private void UnloadOrderLookupMode()
		{
			if (orderLookupLifetimeScope != null)
			{
				IOrderEntity order = orderLookupControl?.Order;
				orderLookupOrderShortcut = order != null ? orderLookupLifetimeScope.Resolve<ISingleScanOrderShortcut>().GetShortcutText(order) : string.Empty;

				panelDockingArea.Controls.Remove(orderLookupControl?.Control);
				panelDockingArea.Controls.Remove(shipmentHistory?.Control);
				orderLookupControl.Unload();
				orderLookupLifetimeScope.Dispose();
				orderLookupLifetimeScope = null;
			}
		}

		/// <summary>
		/// Unload the Products Mode
		/// </summary>
		private void UnloadProductsMode()
		{
			DetachProductEventHandlers();
			addProductMenuItemVariant.Enabled = false;
			buttonProductCatalogEditProduct.Enabled = false;
			productsLifetimeScope?.Dispose();
			productsLifetimeScope = null;
			productsMode = null;
		}

		/// <summary>
		/// Detach event handlers from product mode buttons
		/// </summary>
		private void DetachProductEventHandlers()
		{
			addProductMenuItemNewProduct.Activate -= OnAddNewProduct;
			addProductMenuItemFromFile.Activate -= OnAddProductFromFile;
			addProductMenuItemVariant.Activate -= OnAddProductVariant;

			buttonProductCatalogEditProduct.Activate -= OnEditProduct;
			buttonProductCatalogExportProduct.Activate -= OnExportProduct;
		}
		/// <summary>
		/// Disable the main batch mode controls
		/// </summary>
		private void DisableBatchModeControls()
		{
			panelDockingArea.Visible = false;
			// Hide all dock windows.  Hide them first so they don't attempt to save when the filter changes (due to the tree being cleared)
			foreach (DockControl control in Panels)
			{
				control.Close();
			}

			// Grid has to be cleared first - otherwise the current settings will be saved in response to the filtertree clearing,
			// and notifying the grid that the selected filter changed.
			gridControl.Reset();
			gridControl.Visible = false;

			// Clear Filter Trees
			ClearFilterTrees();
			panelDockingArea.Visible = true;
		}

		/// <summary>
		/// Is the specified tab a Batch mode specific tab
		/// </summary>
		private bool IsBatchSpecificTab(RibbonTab tab) => tab == ribbonTabGridViewHome;

		/// <summary>
		/// Is the specified tab an order lookup mode specific tab
		/// </summary>
		private bool IsOrderLookupSpecificTab(RibbonTab tab) =>
			tab == ribbonTabOrderLookupViewShipmentHistory ||
			tab == ribbonTabOrderLookupViewShipping;

		/// <summary>
		/// Is the specified tab a products mode specific tab
		/// </summary>
		private bool IsProductSpecificTab(RibbonTab tab) => tab == ribbonTabProducts;

		/// <summary>
		/// Handle when the currently selected ribbon tab has changed
		/// </summary>
		private void OnRibbonSelectedTabChanged(object sender, System.EventArgs e)
		{
			// The selected tab changes when showing a blank ui on logoff, so without this, we'd crash
			if (!UserSession.IsLoggedOn)
			{
				return;
			}

			// Ensure we're in the right mode
			if (IsBatchSpecificTab(ribbon.SelectedTab))
			{
				ChangeUIMode(UIMode.Batch);
			}
			else if (IsOrderLookupSpecificTab(ribbon.SelectedTab))
			{
				ChangeUIMode(UIMode.OrderLookup);

				if (ribbon.SelectedTab == ribbonTabOrderLookupViewShipping)
				{
					ToggleVisiblePanel(orderLookupControl?.Control);
					shipmentHistory?.Deactivate();
				}
				else if (ribbon.SelectedTab == ribbonTabOrderLookupViewShipmentHistory)
				{
					// Save the order in case changes were made before switching to this tab
					orderLookupControl?.Save();
					ToggleVisiblePanel(shipmentHistory?.Control);
					shipmentHistory.Activate(buttonOrderLookupViewVoid, buttonOrderLookupViewReprint, buttonOrderLookupViewShipAgain);
				}
			}
			else if (IsProductSpecificTab(ribbon.SelectedTab))
			{
				ChangeUIMode(UIMode.Products);
			}

			UpdateStatusBar();
		}

		/// <summary>
		/// True if shipping history control is active
		/// </summary>
		public bool IsShipmentHistoryActive()
		{
			return shipmentHistory != null && panelDockingArea.Controls.Contains(shipmentHistory.Control);
		}

		/// <summary>
		/// Toggle which control is visible in the panel docking area
		/// </summary>
		private void ToggleVisiblePanel(Control toAdd)
		{
			// Remove the existing control before we add the new one.
			if (toAdd != shipmentHistory?.Control)
			{
				panelDockingArea.Controls.Remove(shipmentHistory?.Control);
			}

			if (toAdd != orderLookupControl?.Control)
			{
				panelDockingArea.Controls.Remove(orderLookupControl?.Control);
			}

			if (!panelDockingArea.Controls.Contains(toAdd) && toAdd != null)
			{
				panelDockingArea.Controls.Add(toAdd);
				toAdd.BringToFront();
			}
		}

		/// <summary>
		/// Open the shipping settings window
		/// </summary>
		private void OnManageOrderLookupFields(object sender, EventArgs e) =>
			Using(
				IoC.BeginLifetimeScope(),
				scope => scope.Resolve<IOrderLookupFieldManager>().ShowManager());

		/// <summary>
		/// Execute any logon actions that have been queued
		/// </summary>
		private void ExecuteLogonActions()
		{
			while (logonActions.TryDequeue(out Action action))
			{
				action();
			}
		}

		/// <summary>
		/// Queue an action to run at the end of the logon process
		/// </summary>
		public void QueueLogonAction(Action action) => logonActions.Enqueue(action);

		/// <summary>
		/// Show the Quick Start dialog
		/// </summary>
		public void ShowQuickStart()
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				IQuickStart quickStart = lifetimeScope.Resolve<IQuickStart>();
				quickStart.ShowDialog();
			}
		}

		/// <summary>
		/// Send the state of each panel as a message
		/// </summary>
		private void SendPanelStateMessages()
		{
			foreach (DockControl panel in Panels)
			{
				IShipWorksMessage message = panel.IsOpen ?
					 new PanelShownMessage(this, panel) :
					(IShipWorksMessage) new PanelHiddenMessage(this, panel);

				Messenger.Current.Send(message);
			}
		}

		/// <summary>
		/// Initialize the filter trees for display
		/// </summary>
		private void InitializeFilterTrees(IUserEntity user)
		{
			orderFilterTree.LoadLayouts(FilterTarget.Orders);
			orderFilterTree.ApplyFolderState(new FolderExpansionState(user.Settings.OrderFilterExpandedFolders));

			customerFilterTree.LoadLayouts(FilterTarget.Customers);
			customerFilterTree.ApplyFolderState(new FolderExpansionState(user.Settings.CustomerFilterExpandedFolders));
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
					string.Format("You are using {0} of your {1} GB database size limit.", StringUtility.FormatByteCount(SqlDiskUsage.GetDatabaseSpaceUsed()), gbLimit),
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
		/// Load initial data when logging in a background thread.  Its not perfect, but it does keep it a bit more responsive
		/// </summary>
		private void LogonToShipWorksAsyncLoad(object state)
		{
			ManualResetEvent loadedEvent = (ManualResetEvent) state;
			logonAsyncLoadSuccess = false;

			try
			{
				UserSession.InitializeForCurrentSession(Program.ExecutionMode);

				using (var lifetimeScope = IoC.BeginLifetimeScope())
				{
					lifetimeScope.Resolve<IOrderArchiveFilterRegenerator>().Regenerate();
				}

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
			// Update our edition for each store.  Eventually this will also be where we log with tango the ShipWorks version being used and maybe other things
			ILicenseService licenseService = IoC.UnsafeGlobalLifetimeScope.Resolve<ILicenseService>();
			List<ILicense> licenses = licenseService.GetLicenses().ToList();

			// refresh the license if it is older than 10 mins
			licenses.ForEach(license => license.Refresh());

			Telemetry.TrackStartShipworks(GetStartupTelemetryData());

			// now that we updated license info we can refresh the UI to match
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(editionGuiHelper.UpdateUI));
			}
			else
			{
				editionGuiHelper.UpdateUI();
			}

			ForceHeartbeat();
		}

		/// <summary>
		/// Get telemetry data for ShipWorks
		/// </summary>
		/// <returns></returns>
		private static IDictionary<string, string> GetStartupTelemetryData()
		{
			Dictionary<string, string> values = new Dictionary<string, string>();

			try
			{
				return values.Union(SqlServerInfo.Fetch())
					.Union(ShippingSettings.GetTelemetryData())
					.Union(ShippingProfileManager.GetTelemetryData())
					.Union(ConfigurationData.GetTelemetryData())
					.Union(FilterTelemetryService.GetTelemetryData())
					.Union(ProductCatalogTelemetryService.GetTelemetryData())
					.ToDictionary(k => k.Key, v => v.Value);
			}
			catch (Exception ex)
			{
				log.Error("Error collecting ShipWorks telemetry data.", ex);
				return values;
			}
		}

		/// <summary>
		/// Check that the version of the connected database is what we need.
		/// </summary>
		private bool CheckDatabaseVersion()
		{
			Version installedVersion;

			try
			{
				installedVersion = SqlSchemaUpdater.GetInstalledSchemaVersion();
			}
			catch (InvalidShipWorksDatabaseException ex)
			{
				log.Error("CheckDatabaseVersion failed on GetInstalledDbVersion.", ex);

				MessageHelper.ShowError(this, "The database is not a valid ShipWorks database.");
				return false;
			}

			log.InfoFormat("CheckDatabaseVersion: Installed: {0}, Required {1}", installedVersion, SqlSchemaUpdater.GetRequiredSchemaVersion());

			// See if it needs upgraded
			if (SqlSchemaUpdater.IsUpgradeRequired() || !SqlSession.Current.IsSqlServer2008OrLater())
			{
				using (ConnectionSensitiveScope scope = new ConnectionSensitiveScope("update the database", this))
				{
					if (!scope.Acquired)
					{
						return false;
					}

					if (DatabaseUpdateWizard.Run(this))
					{
						// If the upgrade went OK, we still need to check that the current user has adequate permissions to work in the restored database
						if (!SqlSession.Current.CheckPermissions(SqlSessionPermissionSet.Standard, this))
						{
							return false;
						}

						// Kick off the loader for loading data into ShipSense as needed
						ShipSenseLoader.LoadDataAsync();
					}
					else
					{
						return false;
					}
				}
			}

			// See if its too new
			if (!SqlSchemaUpdater.IsCorrectSchemaVersion())
			{
				UserSession.Reset();
				using (NeedUpgradeShipWorks dlg = new NeedUpgradeShipWorks())
				{
					DialogResult result = dlg.ShowDialog(this);
					if (result == DialogResult.OK)
					{
						Close();
					}
				}

				return false;
			}

			// update the build  number stored in the database, this has nothing to do with the database's schema
			UpdateDatabaseBuildNumber();

			return true;
		}

		/// <summary>
		/// Update the database build number if its out of date
		/// </summary>
		private void UpdateDatabaseBuildNumber()
		{
			Version localBuildVersion = typeof(MainForm).Assembly.GetName().Version;
			Version databaseBuildVersion = SqlSchemaUpdater.GetBuildVersion();

			if (localBuildVersion > databaseBuildVersion)
			{
				using (DbConnection con = SqlSession.Current.OpenConnection())
				{
					using (DbCommand command = con.CreateCommand())
					{
						SqlSchemaUpdater.UpdateBuildVersionProcedure(command);
					}
				}
			}
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

			archiveNotificationManager.Reset();

			// Hide all dock windows.  Hide them first so they don't attempt to save when the filter changes (due to the tree being cleared)
			foreach (DockControl control in Panels)
			{
				control.Close();
			}

			// Grid has to be cleared first - otherwise the current settings will be saved in response to the filtertree clearing,
			// and notifying the grid that the selected filter changed.
			gridControl.Reset();

			// Clear Filter Trees
			ClearFilterTrees();

			ribbon.Tabs.Clear();
			ribbon.ToolBar = null;

			// Hide all status bar items
			statusBar.MainStrip.Visible = false;

			panelDockingArea.Visible = false;
			DashboardManager.CloseDashboard();

			// Take focus away from other controls after we've logged out. The grid control was setting focus on its search box when
			// it reset, which was causing a crash when any key was pressed after a user logged out.
			Focus();

			log.InfoFormat("UI hidden");
		}

		/// <summary>
		/// Clear each of the filter trees
		/// </summary>
		private void ClearFilterTrees()
		{
			orderFilterTree.Clear();
			customerFilterTree.Clear();
		}

		/// <summary>
		/// Show the normal UI
		/// </summary>
		private void ApplyCurrentUserLayout()
		{
			UserSettingsEntity settings = UserSession.User.Settings;
			bool hasProductsPermissions = UserSession.Security.HasPermission(PermissionType.ManageProducts);

			// Show the tool bar
			ribbon.ToolBar = quickAccessToolBar;

			// Add back in the tabs
			foreach (RibbonTab tab in ribbonTabs)
			{
				if (!hasProductsPermissions && IsProductSpecificTab(tab))
				{
					continue;
				}

				ribbon.Tabs.Add(tab);

				// Preserve order
				tab.SendToBack();
			}

			EnableRibbonTabs();

			shipmentHistory = IoC.UnsafeGlobalLifetimeScope.Resolve<IShipmentHistory>();

			// Show all status bar strips
			statusBar.MainStrip.Visible = true;

			// Load the user's saved state
			try
			{
				windowLayoutProvider.LoadLayout(settings.WindowLayout);
			}
			catch (AppearanceException)
			{
				windowLayoutProvider.LoadDefault();
				MessageHelper.ShowMessage(this,
				"Your appearance settings file has been corrupted. Appearance settings have been reset to the defaults.");

				//Ensure that the defaults are saved.
				SaveCurrentUserSettings();
			}

			// Make sure any users upgrading from a previous version will always see (and
			// be made aware of) the rate panel; they can still choose to remove it later
			OpenNewPanelsOnUpgrade(settings);

			// Load the user's saved menu settings
			gridMenuLayoutProvider.LoadLayout(settings.GridMenuLayout);

			// Initialize all the panels
			foreach (DockingPanelContentHolder holder in GetDockingPanelContentHolders())
			{
				holder.InitializeForCurrentUser();
			}

			// Allows for the Shipment panel visibility to be initialized when starting
			// ShipWorks in Batch Mode.
			if (UIMode == UIMode.Batch)
			{
				panelDockingArea.Visible = true;
			}
		}

		/// <summary>
		/// Enable the ribbon tabs
		/// </summary>
		private void EnableRibbonTabs()
		{
			if (UIMode == UIMode.OrderLookup)
			{
				using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
				{
					ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();
					EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);
				}
			}
		}

		/// <summary>
		/// Inspects the WindowLayout of the user settings to see if the user has upgraded from a
		/// version of ShipWorks that does not have the panels we want shown to all users on upgrade
		/// so they are aware of it and don't have to manually enabled it.
		/// </summary>
		/// <param name="settings">The user settings being inspected.</param>
		/// <exception cref="AppearanceException">The file is not a valid ShipWorks layout.</exception>
		private void OpenNewPanelsOnUpgrade(UserSettingsEntity settings)
		{
			// Write out the user's current window layout to disk
			string tempFile = Path.Combine(DataPath.ShipWorksTemp, Guid.NewGuid().ToString("N") + ".swl");
			File.WriteAllBytes(tempFile, settings.WindowLayout);

			// The path that items from the .swl file will be extracted to
			string tempPath = DataPath.CreateUniqueTempPath();

			try
			{
				// Write all the contents out to a temporary folder
				using (ZipReader reader = new ZipReader(tempFile))
				{
					foreach (ZipReaderItem item in reader.ReadItems())
					{
						item.Extract(Path.Combine(tempPath, item.Name));
					}
				}
			}
			catch (ZipException ex)
			{
				throw new AppearanceException("The file is not a valid ShipWorks layout.", ex);
			}

			// Read the panels.xml file that was extracted
			string panelXml = File.ReadAllText(Path.Combine(tempPath, "panels.xml"), Encoding.Unicode);
			XmlDocument panelDoc = new XmlDocument();
			panelDoc.LoadXml(panelXml);

			// Check to see if the Window GUID for the rates panel is present. The GUID value is set at
			// design-time by the designer sandDockManager, so we can look for it
			const string RatePanelID = "61946061-0df9-4143-92ed-0e71826d7d5f";

			XmlNode ratePanelNode = panelDoc.SelectSingleNode(string.Format("/Layout/Window[@Guid='{0}']", RatePanelID));

			if (ratePanelNode == null)
			{
				// There wasn't an item in the user settings for the rate panel, meaning the user just
				// upgraded from a previous version without the rate panel
				DockControl dockControl = Panels.FirstOrDefault(c => c.Guid == Guid.Parse(RatePanelID));
				if (dockControl != null)
				{
					// We want to display the rate panel for everyone after an upgrade by default
					dockControl.Open(WindowOpenMethod.OnScreen);
				}
			}

			// Attach the customers filter panel to the same container layout as the order filter panel.
			// The GUID value is set at design-time by the designer sandDockManager, so we can look for it in the panel XML. If the
			// node is not found in the panelDoc XML, ShipWorks was just upgraded from a previous version. The section below
			// will add the customers filter panel alongside the orders filter panel if it is being shown/used in a dock container
			const string dockableCustomersFilterID = "5f3097be-c6e4-4f85-b9ff-24844749ae44";
			XmlNode customerFiltersPanelNode = panelDoc.SelectSingleNode(string.Format("/Layout/Window[@Guid='{0}']", dockableCustomersFilterID));

			if (customerFiltersPanelNode == null)
			{
				// The customer filter panel doesn't exists in the panel settings, so we need to attach it to the order filters
				// if the order filters are being used in a container
				DockContainer orderFiltersContainer = sandDockManager.GetDockContainers().FirstOrDefault(c => c.Controls.Contains(dockableWindowOrderFilters));
				if (orderFiltersContainer != null)
				{
					// We've found the container the order filters below to, so we need to add the customer
					// filters panel to the same layout of the order filter panel. This creates the appearance
					// of the panels appearing as "tabs" within the container's layout
					dockableWindowOrderFilters.LayoutSystem.Controls.Add(dockableWindowCustomerFilters);
				}
			}
		}

		/// <summary>
		/// Load all of the display settings for the current user
		/// </summary>
		private void LoadCurrentUserDisplaySettings()
		{
			UserEntity user = UserSession.User;

			// Update title
			using (var lifetimeScope = IoC.BeginLifetimeScope())
			{
				var configurationData = lifetimeScope.Resolve<IConfigurationData>();
				ApplicationText = user.Username + (configurationData.IsArchive() ? " [Archive]" : string.Empty);
			}

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

			SaveCurrentUserModeSpecificSettings(settings);

			//Save the Ribbon minimized state
			settings.MinimizeRibbon = ribbon.Minimized;

			// Save the settings
			using (SqlAdapter adapter = new SqlAdapter())
			{
				adapter.SaveAndRefetch(settings);
			}
		}

		/// <summary>
		/// Save the current user mode specific settings.
		/// </summary>
		private void SaveCurrentUserModeSpecificSettings(UserSettingsEntity settings)
		{
			// Save the layout
			if (UIMode == UIMode.Batch)
			{
				settings.WindowLayout = windowLayoutProvider.SerializeLayout();
				settings.GridMenuLayout = gridMenuLayoutProvider.SerializeLayout();

				// Save the filter expand collapse state
				settings.OrderFilterExpandedFolders = orderFilterTree.GetFolderState().GetState();
				settings.CustomerFilterExpandedFolders = customerFilterTree.GetFolderState().GetState();

				// Save the last active filter
				if (gridControl.IsSearchActive)
				{
					if (gridControl.ActiveFilterTarget == FilterTarget.Customers)
					{
						settings.CustomerFilterLastActive = searchRestoreFilterNodeID;
						settings.OrderFilterLastActive = 0;
					}
					else
					{
						settings.OrderFilterLastActive = searchRestoreFilterNodeID;
						settings.CustomerFilterLastActive = 0;
					}
				}
				else
				{
					settings.CustomerFilterLastActive = customerFilterTree.SelectedFilterNodeID;
					settings.OrderFilterLastActive = orderFilterTree.SelectedFilterNodeID;
				}

				// Save the grid column state
				gridControl.SaveGridColumnState();
			}

			shipmentHistory?.SaveGridColumnState();
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

			// Apply theme to grids
			gridControl.ApplyDisplaySettings();

			// Apply krypton scheme
			kryptonManager.GlobalPaletteMode = AppearanceHelper.GetKryptonPaletteMode();

			ApplySystemTrayProperties();

			archiveNotificationManager.RefreshUI();
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
		/// Apply the properties that need to be set for hiding in the system tray based on our current state
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

			// Update the dashboard for new/removed trials
			DashboardManager.UpdateTrialItems();

			// Update store type specific dashboard items
			DashboardManager.UpdateStoreTypeDependentItems();

			// Update edition-based UI from stores
			EditionManager.UpdateRestrictions();

			// Update the state of the ui for the update online commands
			buttonUpdateOnline.Visible = OnlineUpdateCommandProvider.HasOnlineUpdateCommands();

			if (UIMode != UIMode.OrderLookup)
			{
				gridMenuLayoutProvider.UpdateStoreDependentUI();

				// The available columns depend on the store types that exist
				FilterNodeColumnManager.InitializeForCurrentSession();
				gridControl.ReloadGridColumns();

				// Update the panels based on the current store set
				foreach (DockingPanelContentHolder holder in GetDockingPanelContentHolders())
				{
					holder.UpdateStoreDependentUI();
				}
			}

			// Update the availability of ribbon items based on security
			ribbonSecurityProvider.UpdateSecurityUI();
		}

		/// <summary>
		/// Apply the given set of MenuCommands to the given menuitem and ribbon popup
		/// </summary>
		private void ApplyMenuCommands(IEnumerable<IMenuCommand> commands, ToolStripMenuItem menuItem,
			Popup ribbonPopup, EventHandler actionHandler, ILifetimeScope lifetimeScope)
		{
			menuItem.DropDownItems.Clear();
			ribbonPopup.Items.Clear();

			menuCommandLifetimeScope?.Dispose();
			menuCommandLifetimeScope = lifetimeScope;

			// Update available local status options
			menuItem.DropDownItems.AddRange(MenuCommandConverter.ToToolStripItems(commands, actionHandler));
			ribbonPopup.Items.Add(MenuCommandConverter.ToRibbonMenu(commands, actionHandler));
		}

		/// <summary>
		/// Do the one-time initial prep-work for edition managed UI
		/// </summary>
		private void PrepareEditionManagedUI()
		{
			editionGuiHelper.RegisterElement(contextCustomerNewOrder, EditionFeature.AddOrderCustomer);
			editionGuiHelper.RegisterElement(buttonNewOrder, EditionFeature.AddOrderCustomer);
			editionGuiHelper.RegisterElement(buttonNewCustomer, EditionFeature.AddOrderCustomer);
			editionGuiHelper.RegisterElement(buttonEndiciaSCAN, EditionFeature.EndiciaScanForm);
			editionGuiHelper.RegisterElement(buttonOrderLookupViewSCANForm, EditionFeature.EndiciaScanForm);
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
            UpdateCommandState();
            UpdatePanelsState();

			ribbonSecurityProvider.UpdateSecurityUI();
		}

		/// <summary>
		/// Update the contents of the status bar
		/// </summary>
		public void UpdateStatusBar()
		{
			if (UIMode == UIMode.Batch)
			{
				labelStatusTotal.Visible = true;
				labelStatusTotal.Text = string.Format("{0}: {1:#,##0}", EnumHelper.GetDescription(gridControl.ActiveFilterTarget), gridControl.TotalCount);

				labelStatusSelected.Visible = true;
				labelStatusSelected.Text = string.Format("Selected: {0:#,##0}", gridControl.Selection.Count);
			}
			else if (IsShipmentHistoryActive())
			{
				labelStatusTotal.Visible = true;
				labelStatusTotal.Text = string.Format("Shipments: {0:#,##0}", shipmentHistory.RowCount);

				labelStatusSelected.Visible = false;
			}
			else
			{
				labelStatusTotal.Visible = false;
				labelStatusSelected.Visible = false;
			}
		}

		/// <summary>
		/// Select the order lookup tab
		/// </summary>
		public void SelectScanToShipTab()
		{
			if (ribbon.InvokeRequired)
			{
				ribbon.Invoke((MethodInvoker) SelectScanToShipTab);
			}
			else
			{
				ribbon.SelectedTab = ribbonTabOrderLookupViewShipping;
			}
		}

		/// <summary>
		/// Get the shipment dock control
		/// </summary>
		private DockControl GetShipmentDockControl()
		{
			return Panels.FirstOrDefault(d => d.Name == "dockableWindowShipment");
		}

		/// <summary>
		/// Allow the combine button to be enabled
		/// </summary>
		private bool ShouldCombineOrderBeEnabled(IEnumerable<long> keys)
		{
			using (ILifetimeScope scope = IoC.BeginLifetimeScope())
			{
				IOrderCombineValidator orderCombineValidator = scope.Resolve<IOrderCombineValidator>();
				return orderCombineValidator.Validate(keys).Success;
			}
		}

		/// <summary>
		/// Allow the combine button to be enabled
		/// </summary>
		private bool ShouldSplitOrderBeEnabled(IEnumerable<long> keys)
		{
			if (!keys.IsCountEqualTo(1))
			{
				return false;
			}

			using (ILifetimeScope scope = IoC.BeginLifetimeScope())
			{
				ISplitOrderValidator orderSplitValidator = scope.Resolve<ISplitOrderValidator>();
				return orderSplitValidator.Validate(keys).Success;
			}
		}

		/// <summary>
		/// Update the state of the ribbon buttons based on the current selection
		/// </summary>
		private void UpdateCommandState()
		{
			int selectionCount = gridControl.Selection.Count;
			selectionDependentEnabler.UpdateCommandState(gridControl.Selection, gridControl.ActiveFilterTarget);

			if (selectionCount == 0 || gridControl.ActiveFilterTarget != FilterTarget.Orders)
			{
				ribbon.SetEditingContext(null);
				return;
			}

			// Don't show the shipping context menu if the shipping panel doesn't exist or isn't open
			if (!IsShippingPanelOpen())
			{
				ribbon.SetEditingContext(null);
				return;
			}

			ribbon.SetEditingContext("SHIPPINGMENU");
		}

		/// <summary>
		/// True if shippingPanel is Open
		/// </summary>
		public bool IsShippingPanelOpen()
		{
			return shipmentDock.Value?.IsOpen == true;
		}

		/// <summary>
		/// Adds Editing Contexts to the ribbon
		/// </summary>
		private void ApplyEditingContext()
		{
			ribbon.EditingContexts.Add(new EditingContext("Shipping Tools", "SHIPPINGMENU", System.Drawing.Color.LightBlue));
		}

        /// <summary>
        /// Update the state of the panels based on the current selection
        /// </summary>
        private void UpdatePanelsState()
        {
            var controls = Panels
                .Where(dockControl => dockControl.IsOpen && dockControl.Controls.Count == 1)
                .Select(d => d.Controls[0] as DockingPanelContentHolder)
                .Where(h => h != null).ToList();

            foreach (var dockingPanelContentHolder in controls)
            {
                // This function can get called as panels are activating.  Activation can be changing as we are closing them during logoff,
                // so we have to make sure we're logged on or updating would crash.
                if (!UserSession.IsLoggedOn)
                {
                    return;
                }
                // This happens to often to use GetOrderedSelectdKeys. If ordering becomes important, we'll need to improve
                // the performance of that somehow for massive selections where there is virtual selection.
                _ = dockingPanelContentHolder.UpdateContent(gridControl.ActiveFilterTarget, gridControl.Selection);
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
				// checked only if its actually visible... So like if it was a non active tab, or minimized,
				// it wouldn't get a check.  This way, it gets a check if its on the screen at all.
				menuItem.Checked = dockControl.DockSituation != DockSituation.None;
			}
		}

		/// <summary>
		/// Execute an invoked menu command
		/// </summary>
		private async void OnExecuteMenuCommand(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			IMenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

			// Execute the command
			await command.ExecuteAsync(this, gridControl.Selection.OrderedKeys, OnAsyncMenuCommandCompleted).ConfigureAwait(false);
		}

		/// <summary>
		/// Execute an invoked update online command
		/// </summary>
		private async void OnExecuteUpdateOnlineCommand(object sender, EventArgs e)
		{
			IMenuCommand command = MenuCommandConverter.ExtractMenuCommand(sender);

			// Execute the command
			await onlineUpdateCommandProvider.ExecuteCommandAsync(command, this, gridControl.Selection.OrderedKeys, OnAsyncMenuCommandCompleted)
				.ConfigureAwait(false);
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
			var lifetimeScope = IoC.BeginLifetimeScope();

			// Update the update online options
			IEnumerable<IMenuCommand> updateOnlineCommands = onlineUpdateCommandProvider.CreateOnlineUpdateCommands(gridControl.Selection.Keys, lifetimeScope);

			// Update the ui to display the commands
			ApplyMenuCommands(updateOnlineCommands, contextOrderOnlineUpdate, popupUpdateOnline, OnExecuteUpdateOnlineCommand, lifetimeScope);
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
				OnExecuteMenuCommand,
				null);
		}

		/// <summary>
		/// Update the UI that's based on user initiated actions
		/// </summary>
		[NDependIgnoreLongMethod]
		[NDependIgnoreComplexMethod]
		private void UpdateCustomButtonsActionsUI()
		{
			string ribbonChunkName = "Custom Actions";

			// Get the actions for all custom buttons that need to be added
			var enabledActions = ActionManager.Actions.Where(a => a.TriggerType == (int) ActionTriggerType.UserInitiated && a.Enabled).Select(a => new { Action = a, Trigger = (UserInitiatedTrigger) ActionManager.LoadTrigger(a) });

			// Get the actions for the ribbon
			var ribbonActions = enabledActions.Where(a => a.Trigger.ShowOnRibbon);

			// Get the ribbon chunk that holds our action buttons
			var actionChunk = ribbonTabGridViewHome.Chunks.Cast<RibbonChunk>().Where(c => c.Text == ribbonChunkName).SingleOrDefault();

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
					ribbonTabGridViewHome.Chunks.Remove(actionChunk);
				}
			}

			// See if there are any to show on the ribbon
			if (ribbonActions.Any())
			{
				// Maybe we need to create it
				if (actionChunk == null)
				{
					actionChunk = new RibbonChunk() { Text = ribbonChunkName, FurtherOptions = false, ItemJustification = ItemJustification.Near };
					ribbonTabGridViewHome.Chunks.Add(actionChunk);
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
		private void OnCustomActionButton(object sender, EventArgs e)
		{
			long actionID = (long) ((SandButton) sender).Tag;

			DispatchUserInitiatedAction(actionID);
		}

		/// <summary>
		/// User has clicked a custom action menu
		/// </summary>
		private void OnCustomActionMenu(object sender, EventArgs e)
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
		/// The application menu is about to be shown
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
			// Indicates the user made it 100% successfully through the database and setup wizards
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

				// Now regardless of if that was successful, see if it altered the database.  Some things can't be rolled back even if the user canceled.
				databaseChanged = scope.DatabaseChanged;

				// If the configuration is complete, or the database changed in any way...
				if (configurationComplete || databaseChanged)
				{
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

				using (DatabaseBackupDlg dlg = new DatabaseBackupDlg(UserSession.User, new TelemetricResult<Unit>("Database.Backup")))
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

			bool needReLogon = false;

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
						needReLogon = true;
					}
				}
			}

			// This is down here so its outside of the scope
			if (needReLogon)
			{
				//Make sure the user is logged off so that we dont crash when updating
				UserSession.Logoff(false);
				InitiateLogon();
			}
		}

		/// <summary>
		/// Archive a set of orders
		/// </summary>
		private async void OnArchive(object sender, EventArgs e)
		{
			using (ILifetimeScope scope = IoC.BeginLifetimeScope())
			{
				await scope.Resolve<IArchiveManagerDialogViewModel>().ShowManager().ConfigureAwait(false);
			}
		}

		/// <summary>
		/// Show the ShipWorks settings window
		/// </summary>
		private void OnShowSettings(object sender, EventArgs e)
		{
			if (UserSession.IsLoggedOn)
			{
				UserSession.User.Settings.MinimizeRibbon = ribbon.Minimized;
				UserSession.User.Settings.ShowQAToolbarBelowRibbon = ribbon.ToolBarPosition == QuickAccessPosition.Below;
			}
			using (ILifetimeScope scope = IoC.BeginLifetimeScope())
			{
				using (ShipWorksSettings dlg = new ShipWorksSettings(scope))
				{
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						ApplyDisplaySettings();

						// Apply ribbon settings
						if (UserSession.IsLoggedOn)
						{
							ribbon.ToolBarPosition = UserSession.User.Settings.ShowQAToolbarBelowRibbon ?
							QuickAccessPosition.Below :
							QuickAccessPosition.Above;

							ribbon.Minimized = UserSession.User.Settings.MinimizeRibbon;
						}
					}
				}
			}
		}

		/// <summary>
		/// Open the window for connecting to interapptive remote assistance
		/// </summary>
		private void OnRemoteAssistance(object sender, EventArgs e)
		{
			new RemoteAssistance().InitiateRemoteAssistance(this);
		}

		/// <summary>
		/// Open the ShipWorks support forum
		/// </summary>
		private void OnSupportForum(object sender, EventArgs e)
		{
			WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us/community/topics", this);
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
			WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us", this);
		}

		/// <summary>
		/// View the ShipWorks about box
		/// </summary>
		private void OnAboutShipWorks(object sender, EventArgs e)
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				IAboutShipWorksDialog aboutDlg = lifetimeScope.Resolve<IAboutShipWorksDialog>();
				aboutDlg.ShowDialog();
			}
		}

		/// <summary>
		/// Submit a support case to ShipWorks
		/// </summary>
		private void OnRequestHelp(object sender, EventArgs e)
		{
			WebHelper.OpenUrl(ShipWorksBusinessConstants.ContactUrl, this);
		}

		/// <summary>
		/// Open the link to the ShipWorks supplies site
		/// </summary>
		private void OnBuySupplies(object sender, EventArgs e)
		{
			WebHelper.OpenUrl("http://supplies.shipworks.com/", this);
		}

		/// <summary>
		/// Open the landing page for our uShip partnership
		/// </summary>
		private void OnUShip(object sender, EventArgs e)
		{
			WebHelper.OpenUrl("http://www.uship.com/shipworks/", this);
		}

		#endregion

		#region Core

		/// <summary>
		/// Just send positive feedback if we are polled with this applications
		/// unique message id.  This means another instance is trying to startup.
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			if (SingleInstance.HandleMainWndProc(ref m))
			{
				log.Info("Received SingleInstance query.");

				return;
			}

			if (m.Msg == SingleInstance.SingleInstanceActivateMessageID)
			{
				log.Info("Other ShipWorks is telling me to activate");

				if (notifyIcon.Visible || this.WindowState == FormWindowState.Minimized)
				{
					BeginInvoke(new MethodInvoker(RestoreFromSystemTray));
				}
			}

			// Because of a bug in the SandRibbon controls, we need to make sure that LParam is
			// an Int32. It calls ToInt32 on the LParam, which will throw if the value is 64-bits,
			// even if the value could fit in 32-bits without any data loss because it's using the checked keyword.
			if (m.Msg == NativeMethods.WM_NCHITTEST)
			{
				m.LParam = new IntPtr((int) m.LParam.ToInt64());
			}

			// A similar bug as above requires that we ensure the WParam value is an Int32 for these messages
			if (m.Msg == NativeMethods.WM_NCACTIVATE || m.Msg == NativeMethods.WM_NCRBUTTONUP)
			{
				m.WParam = new IntPtr((int) m.WParam.ToInt64());
			}

			base.WndProc(ref m);
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
		private void OnEntityChangeDetected(object sender, EventArgs e)
		{
			ForceHeartbeat(HeartbeatOptions.ForceReload);
		}

		/// <summary>
		/// Force a heartbeat to occur before its next scheduled time.  If the parameter changesExpected is true,
		/// this method will increase the heart rate until changes are found, or until the forced heart rate
		/// time period expires.  This is allowed to be called from any thread.
		/// </summary>
		internal void ForceHeartbeat()
		{
			ForceHeartbeat(HeartbeatOptions.None);
		}

		/// <summary>
		/// Force a heartbeat to occur before its next scheduled time.  If the parameter changesExpected is true,
		/// this method will increase the heart rate until changes are found, or until the forced heart rate
		/// time period expires.  This is allowed to be called from any thread.
		/// </summary>
		public void ForceHeartbeat(HeartbeatOptions options)
		{
			if (InvokeRequired)
			{
				// Put the heartbeat on the UI thread
				BeginInvoke((MethodInvoker) delegate
				{ ForceHeartbeat(options); });
				return;
			}

			// Force it to go now, if it
			heartBeat.ForceHeartbeat(options);
		}

		/// <summary>
		/// Logic that happens when a modal window is opening
		/// </summary>
		private void OnEnterThreadModal(object sender, EventArgs e)
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

			if (UserSession.IsLoggedOn && UIMode == UIMode.Batch)
			{
				gridControl.SaveGridColumnState();
			}
		}

		/// <summary>
		/// Modal window is closing
		/// </summary>
		private void OnLeaveThreadModal(object sender, EventArgs e)
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

			if (UserSession.IsLoggedOn && UIMode == UIMode.Batch)
			{
				gridControl.ReloadGridColumns();
			}
		}

		/// <summary>
		/// A connection sensitive scope is about to be acquired
		/// </summary>
		private void OnAcquiringConnectionSensitiveScope(object sender, EventArgs e)
		{
			// Search has to be canceled before potential changing databases, otherwise it wouldn't have a chance to cleanup in the current database.
			gridControl.CancelSearch();
		}

		/// <summary>
		/// When an action is ran it could cause filtering side affects so we force the heartbeat right away
		/// </summary>
		private void OnActionRan(object sender, EventArgs e)
		{
			ForceHeartbeat(HeartbeatOptions.ChangesExpected);
		}

		#endregion

		#region Filtering

		/// <summary>
		///  Update filter tree filter counts
		/// </summary>
		public void UpdateFilterCounts()
		{
			orderFilterTree.UpdateFilterCounts();
			customerFilterTree.UpdateFilterCounts();
		}

		/// <summary>
		///  Reload filter tree layouts
		/// </summary>
		public void ReloadFilterLayouts()
		{
			orderFilterTree.SelectedFilterNodeChanged -= new EventHandler(OnSelectedFilterNodeChanged);
			customerFilterTree.SelectedFilterNodeChanged -= new EventHandler(OnSelectedFilterNodeChanged);

			orderFilterTree.ReloadLayouts();
			customerFilterTree.ReloadLayouts();

			customerFilterTree.SelectedFilterNodeChanged += new EventHandler(OnSelectedFilterNodeChanged);
			orderFilterTree.SelectedFilterNodeChanged += new EventHandler(OnSelectedFilterNodeChanged);
		}

		/// <summary>
		///  Return the currently selected filter tree node of the visible window.
		/// </summary>
		public FilterNodeEntity SelectedFilterNode()
		{
			return ActiveFilterTree().SelectedFilterNode;
		}

		/// <summary>
		/// Update the filter in the appropriate filter tree.
		/// </summary>
		public void UpdateFilter(FilterEntity filter)
		{
			if (filter.FilterTarget == (int) FilterTarget.Orders)
			{
				orderFilterTree.UpdateFilter(filter);
			}
			else
			{
				customerFilterTree.UpdateFilter(filter);
			}
		}

		/// <summary>
		/// Returns true if any filter tree has calculating nodes
		/// </summary>
		/// <returns></returns>
		public bool FiltersHaveCalculatingNodes()
		{
			return orderFilterTree.HasCalculatingNodes() || customerFilterTree.HasCalculatingNodes();
		}

		/// <summary>
		/// Called when one of the filter windows gets displayed.  It then brings focus to the correct filter.
		/// </summary>
		private void OnFilterDockableWindowVisibleChanged(object sender, System.EventArgs e)
		{
			// Get the dockable window so that we can get the filter tree
			DockControl dockableWindowFilters = (DockableWindow) sender;
			if (dockableWindowFilters == null)
			{
				throw new InvalidOperationException("OnFilterDockableWindowVisibleChanged called by an object that is not a DockableWindow.");
			}

			FilterTree currentFilterTree = dockableWindowFilters.Controls.OfType<FilterTree>().FirstOrDefault();
			if (currentFilterTree == null)
			{
				throw new InvalidOperationException("MainForm has a dockable window that is missing a filter tree.");
			}

			// If we are switching from order to/from customer and there was a custom search going,
			// we need to cancel out of it so we don't crash.
			if (gridControl.IsSearchActive)
			{
				gridControl.CancelSearch();
			}

			// We only want to refresh if this window is visible, no search, and not exiting the app (user IS logged in)
			if (dockableWindowFilters.Visible && dockableWindowFilters.IsOpen &&
				currentFilterTree.ActiveSearchNode == null && UserSession.IsLoggedOn)
			{
				// If no filter node is selected, select the first one.
				if (currentFilterTree.SelectedFilterNodeID == 0)
				{
					currentFilterTree.SelectFirstNode();
				}
				else
				{
					// There is a selected node, force a refresh of the UI
					OnSelectedFilterNodeChanged(currentFilterTree, null);
				}
			}
		}

		/// <summary>
		/// Handle when the filter tree wants to load a filter as an advanced search
		/// </summary>
		private void OnFilterTreeLoadAsAdvancedSearch(object sender, FilterNodeEntity e)
		{
			gridControl.LoadFilterInAdvancedSearch(e);
		}

		/// <summary>
		/// Called when the user selects a different filter.
		/// </summary>
		private void OnSelectedFilterNodeChanged(object sender, EventArgs e)
		{
			// No longer need to restore an old node, if we had been searching
			searchRestoreFilterNodeID = 0;

			// Update the grid to show the new node
			gridControl.ActiveFilterNode = ((FilterTree) sender).SelectedFilterNode;

			DeselectFilterNodeFromOtherContextFilterTree(sender);

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
		/// Deselect the filter node from the context that is not active
		/// </summary>
		private void DeselectFilterNodeFromOtherContextFilterTree(object sender)
		{
			// When a search is ending, we get two selection modifications and we don't
			// want to deselect anything for the search related one
			if (gridControl.IsSearchEnding)
			{
				return;
			}

			FilterTree otherTree = sender == orderFilterTree ? customerFilterTree : orderFilterTree;

			if (otherTree.SelectedFilterNode != null)
			{
				otherTree.SelectedFilterNodeChanged -= OnSelectedFilterNodeChanged;
				otherTree.SelectedFilterNode = null;
				otherTree.SelectedFilterNodeChanged += OnSelectedFilterNodeChanged;
			}
		}

		/// <summary>
		/// Called when the selection in the grid changes
		/// </summary>
		private void OnGridSelectionChanged(object sender, EventArgs e)
		{
			if (gridControl.ActiveFilterTarget == FilterTarget.Orders)
			{
				Messenger.Current.Send(new OrderSelectionChangingMessage(this, gridControl.Selection.Keys));
			}

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

            UpdatePanelsState();
        }

		/// <summary>
		/// The grid is going into or coming out of search mode
		/// </summary>
		private void OnGridSearchActiveChanged(object sender, EventArgs e)
		{
			FilterTree currentFilterTree = ActiveFilterTree();
			FilterTree inactiveFilterTree = InActiveFilterTree();

			if (gridControl.IsSearchActive)
			{
				searchRestoreFilterNodeID = currentFilterTree.SetSearch(gridControl.ActiveFilterNode, OnSelectedFilterNodeChanged);
			}
			else
			{
				// Restore the previously selected node before search started
				if (searchRestoreFilterNodeID != 0)
				{
					currentFilterTree.SelectedFilterNodeID = searchRestoreFilterNodeID;
				}

				searchRestoreFilterNodeID = 0;

				currentFilterTree.ActiveSearchNode = null;
			}

			// Now that the user can be switching back and forth between order and customer trees, we have to
			// manually kill the opposite filter tree's search node otherwise we get object null exceptions.
			inactiveFilterTree.ActiveSearchNode = null;

			UpdateSelectionDependentUI();
			UpdateDetailViewSettingsUI();
		}

		/// <summary>
		/// Handle when a filter is saved from the advanced search
		/// </summary>
		private void OnGridControlFilterSaved(object sender, FilterNodeEntity nodeEntity)
		{
			var activeFilterTree = ActiveFilterTree();

			if (activeFilterTree != null)
			{
				activeFilterTree.ReloadLayouts();
				activeFilterTree.SelectedFilterNode = nodeEntity;
			}
		}

		/// <summary>
		/// Determine and return the INactive filter tree based on the grid
		/// </summary>
		private FilterTree InActiveFilterTree()
		{
			switch (gridControl.ActiveFilterTarget)
			{
				case FilterTarget.Orders:
					return customerFilterTree;
				case FilterTarget.Customers:
					return orderFilterTree;
				case FilterTarget.Shipments:
				case FilterTarget.Items:
				default:
					return null;
			}
		}

		/// <summary>
		/// Determine and return the active filter tree based on the grid
		/// </summary>
		private FilterTree ActiveFilterTree()
		{
			switch (gridControl.ActiveFilterTarget)
			{
				case FilterTarget.Orders:
					return orderFilterTree;
				case FilterTarget.Customers:
					return customerFilterTree;
				case FilterTarget.Shipments:
				case FilterTarget.Items:
				default:
					return null;
			}
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
		private void SelectInitialFilter(IUserSettingsEntity settings)
		{
			long initialID = settings.FilterInitialUseLastActive ?
				settings.CustomerFilterLastActive : settings.FilterInitialSpecified;
			FilterNodeEntity filterNode = FilterLayoutContext.Current.FindNode(initialID);

			FilterTarget target = filterNode?.Filter.FilterTarget != null ?
				(FilterTarget) filterNode.Filter.FilterTarget :
				FilterTarget.Orders;
			FilterTree filterTree = target == FilterTarget.Orders ?
				orderFilterTree : customerFilterTree;

			filterTree.Focus();
			filterTree.SelectInitialFilter(settings, target);
		}

		/// <summary>
		/// Open the filter organizer
		/// </summary>
		private void OnManageFilters(object sender, EventArgs e)
		{
			using (FilterOrganizerDlg dlg = new FilterOrganizerDlg(orderFilterTree.SelectedFilterNode, orderFilterTree.GetFolderState()))
			{
				dlg.ShowDialog(this);
			}
		}

		#endregion

		#region Downloading

		/// <summary>
		/// Update the download button to show a drop-down if there are multiple stores
		/// </summary>
		private void UpdateDownloadButtonForStores()
		{
			List<StoreEntity> stores = GetDownloadEnabledStores().ToList();

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
			List<StoreEntity> stores = GetDownloadEnabledStores().ToList();

			if (stores.Count > 0)
			{
				using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
				{
					ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();

					licenseService.GetLicenses().FirstOrDefault()?.EnforceCapabilities(EnforcementContext.Download, this);
				}

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
		/// Get the list of stores that have downloading enabled
		/// </summary>
		private IEnumerable<StoreEntity> GetDownloadEnabledStores()
		{
			// Get the list of stores that we will download for.
			return StoreManager.GetEnabledStores()
				.Where(s => ComputerDownloadPolicy.Load(s).IsThisComputerAllowed)
				.Where(x => x.StoreTypeCode != StoreTypeCode.Manual);
		}

		/// <summary>
		/// Initiate the downloading of orders for a single store
		/// </summary>
		private void OnDownloadOrdersSingleStore(object sender, EventArgs e)
		{
			Divelements.SandRibbon.MenuItem menuItem = (Divelements.SandRibbon.MenuItem) sender;
			StoreEntity store = (StoreEntity) menuItem.Tag;

			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				ILicenseService licenseService = lifetimeScope.Resolve<ILicenseService>();

				licenseService.GetLicenses().FirstOrDefault()?.EnforceCapabilities(EnforcementContext.Download, this);
			}
			// Start the download
			DownloadManager.StartDownload(new List<StoreEntity> { store }, DownloadInitiatedBy.User);

			ShowDownloadProgress();
		}

		/// <summary>
		/// A download is starting
		/// </summary>
		private void OnDownloadStarting(object sender, EventArgs e)
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
		private void OnDownloadComplete(object sender, DownloadCompleteEventArgs e)
		{
			if (InvokeRequired)
			{
				// This does need to be Invoke and not BeginInvoke.  So that the downloader doesn't think we are done early, and we're messing
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
		private void OnEmailStarting(object sender, EventArgs e)
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
		private void OnEmailComplete(object sender, EmailCommunicationCompleteEventArgs e)
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
			customerFilterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Customers).FilterNode;
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

			// Activate the orders grid
			dockableWindowOrderFilters.Open();
			orderFilterTree.Focus();

			// Ensure the order grid is active
			orderFilterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
			gridControl.ActiveFilterNode = orderFilterTree.SelectedFilterNode;
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
		/// Create a new combined order
		/// </summary>
		private async void OnCombineOrders(object sender, EventArgs e)
		{
			if (!ShouldCombineOrderBeEnabled(gridControl.Selection.OrderedKeys))
			{
				UpdateCommandState();
				return;
			}

			GenericResult<long> result = await CombineOrders().ConfigureAwait(true);

			if (result.Success)
			{
				LookupOrder(result.Value);
			}
			else
			{
				UpdateCommandState();
			}
		}

		/// <summary>
		/// Combine the currently selected orders
		/// </summary>
		private async Task<GenericResult<long>> CombineOrders()
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				using (DisableCustomEnablerComponents())
				{
					ICombineOrderOrchestrator orchestrator = lifetimeScope.Resolve<ICombineOrderOrchestrator>();
					return await orchestrator.Combine(gridControl.Selection.OrderedKeys).ConfigureAwait(false);
				}
			}
		}

		/// <summary>
		/// Split an order
		/// </summary>
		private async void OnSplitOrder(object sender, EventArgs e)
		{
			if (!ShouldSplitOrderBeEnabled(gridControl.Selection.OrderedKeys))
			{
				UpdateCommandState();
				return;
			}

			await SplitOrder()
				.Do(LookupOrders, _ => UpdateCommandState(), ContinueOn.CurrentThread)
				.Recover(ex => Enumerable.Empty<long>())
				.ConfigureAwait(false);
		}

		/// <summary>
		/// Combine the currently selected orders
		/// </summary>
		private async Task<IEnumerable<long>> SplitOrder()
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				using (DisableCustomEnablerComponents())
				{
					IOrderSplitOrchestrator orchestrator = lifetimeScope.Resolve<IOrderSplitOrchestrator>();
					return await orchestrator.Split(gridControl.Selection.OrderedKeys.FirstOrDefault()).ConfigureAwait(false);
				}
			}
		}

		/// <summary>
		/// Lookup the specified order by selecting it via Quick Search
		/// </summary>
		private void LookupOrder(long orderID)
		{
			dockableWindowOrderFilters.Open(WindowOpenMethod.OnScreenSelect);

			// Ensure the order grid is active
			orderFilterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
			gridControl.ActiveFilterNode = orderFilterTree.SelectedFilterNode;
			gridControl.LoadSearchCriteria(QuickLookupCriteria.CreateOrderLookupDefinition(orderID));
		}

		/// <summary>
		/// Lookup the specified order by selecting it via Quick Search
		/// </summary>
		private void LookupOrders(IEnumerable<long> orderIDs)
		{
			dockableWindowOrderFilters.Open(WindowOpenMethod.OnScreenSelect);

			// Ensure the order grid is active
			orderFilterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Orders).FilterNode;
			gridControl.ActiveFilterNode = orderFilterTree.SelectedFilterNode;
			gridControl.LoadSearchCriteria(QuickLookupCriteria.CreateOrderLookupDefinition(orderIDs));
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
					dockableWindowCustomerFilters.Open(WindowOpenMethod.OnScreenSelect);

					// Ensure the customer grid is active
					customerFilterTree.SelectedFilterNode = FilterLayoutContext.Current.GetSharedLayout(FilterTarget.Customers).FilterNode;
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
		/// Edit the customer associated with the selected order
		/// </summary>
		private void OnContextOrderEditCustomer(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			OrderEntity order = (OrderEntity) DataProvider.GetEntity(gridControl.Selection.Keys.First());
			EditCustomer(order.CustomerID);
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
			// Cache the list of items to delete because it was possible to delete an item then hit the delete key before
			// the item completely deleted. This would cause the initial check of selected items to pass, but the deletion
			// process would throw because by that point, the selection was cleared and the list was empty.
			List<long> keysToDelete = gridControl.Selection.OrderedKeys.ToList();

			if (keysToDelete.Count == 0)
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

				deleter.DeleteAsync(keysToDelete);
			}
		}

		/// <summary>
		/// An edit has occurred in one of the panels
		/// </summary>
		private void OnPanelDataChanged(object sender, EventArgs e)
		{
			ForceHeartbeat(HeartbeatOptions.ChangesExpected);
		}

		#endregion

		#region View

		/// <summary>
		/// Show the specified panel
		/// </summary>
		public void ShowPanel(DockControl dockControl)
		{
			dockControl.Open(WindowOpenMethod.OnScreenActivate);

			Messenger.Current.Send(new PanelShownMessage(this, dockControl));
		}

		/// <summary>
		/// A panel should be shown
		/// </summary>
		private void OnShowPanel(object sender, EventArgs e)
		{
			EnsureBatchMode();

			SandMenuItem item = (SandMenuItem) sender;

			ShowPanel((DockControl) item.Tag);
		}

		/// <summary>
		/// Called when a dock control that didn't used to be open now is
		/// </summary>
		private void OnDockControlActivated(object sender, DockControlEventArgs e)
		{
			UpdateSelectionDependentUI();
		}

		/// <summary>
		/// When closing a order or customer filter, switch to the other one.
		/// </summary>
		private void OnDockControlClosing(object sender, DockControlClosingEventArgs e)
		{
			if (UserSession.IsLoggedOn)
			{
				if (e.DockControl.Guid == dockableWindowOrderFilters.Guid && dockableWindowCustomerFilters.DockSituation != DockSituation.None)
				{
					customerFilterTree.SelectInitialFilter(UserSession.User.Settings, FilterTarget.Customers);
					gridControl.ActiveFilterNode = customerFilterTree.SelectedFilterNode;
				}

				if (e.DockControl.Guid == dockableWindowCustomerFilters.Guid && dockableWindowOrderFilters.DockSituation != DockSituation.None)
				{
					orderFilterTree.SelectInitialFilter(UserSession.User.Settings, FilterTarget.Orders);
					gridControl.ActiveFilterNode = orderFilterTree.SelectedFilterNode;
				}
			}

			Messenger.Current.Send(new PanelHiddenMessage(this, e.DockControl));
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
			EnsureBatchMode();

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
			var newHeightIndex = detailViewDetailHeight.SelectedIndex;

			EnsureBatchMode();

			DetailViewSettings settings = gridControl.ActiveDetailViewSettings;

			// Save the new setting
			settings.DetailRows = newHeightIndex;

			UpdateDetailViewSettingsUI();
		}

		/// <summary>
		/// The mode of the detail view is changing
		/// </summary>
		private void OnChangeDetailViewDetailMode(object sender, EventArgs e)
		{
			EnsureBatchMode();

			DetailViewSettings settings = gridControl.ActiveDetailViewSettings;

			// Save the new setting
			settings.DetailViewMode = (DetailViewMode) ((SandButton) sender).Tag;

			UpdateDetailViewSettingsUI();
		}

		/// <summary>
		/// Update the UI controls for the Detail View settings
		/// </summary>
		[NDependIgnoreLongMethod]
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
				case DetailViewMode.Normal:
					activeRadio = buttonDetailViewNormal;
					break;
				case DetailViewMode.DetailOnly:
					activeRadio = buttonDetailViewDetail;
					break;
				case DetailViewMode.NormalWithDetail:
					activeRadio = buttonDetailViewNormalDetail;
					break;
			}

			// Uncheck the old ones
			modeRadios.Remove(activeRadio);
			modeRadios.ForEach(b => b.Checked = false);

			// Check the right one
			if (activeRadio != null)
			{
				activeRadio.Checked = true;
			}

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
				filterNode = ActiveFilterTree().SelectedFilterNode;
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
			EnsureBatchMode();

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
			EnsureBatchMode();

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

			EnsureBatchMode();

            // If a new panel was shown, then we may need to update its display state
            UpdatePanelsState();
        }

        /// <summary>
        /// Initialize the panels for the current user
        /// </summary>
        [NDependIgnoreLongMethod]
        private void InitializePanels()
        {
            // First go through each panel and wrap it in a Panel control that will allow us to show messages to the user like "No orders are selected.";
            foreach (DockControl dockControl in Panels)
            {
                // See if this is one that needs wrapped
                if (dockControl.Controls.Count == 1 && dockControl.Controls[0] is IDockingPanelContent content)
                {
                    DockingPanelContentHolder holder = new DockingPanelContentHolder();
                    holder.Initialize(content);

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
					.Where(p => p != noteLayout.AllColumns[NoteFields.EntityID] && p != noteLayout.AllColumns[NoteFields.Text]))
				{
					position.Visible = false;
				}
			});

			// Initialize the orders panel
			panelOrders.Initialize(new Guid("{06878FA9-7A6D-442c-B4F8-357C1B3F6A45}"), GridColumnDefinitionSet.OrderPanel, layout =>
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

			foreach (DockControl dockControl in Panels.Where(d => d.Controls.Count == 1))
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

				if (StoreManager.GetAllStoresReadOnly().None())
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
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				using (ActionManagerDlg dlg = new ActionManagerDlg(lifetimeScope))
				{
					dlg.ShowDialog(this);
				}
			}

			UpdateCustomButtonsActionsUI();
		}

		/// <summary>
		/// Open the shipping settings window
		/// </summary>
		private void OnManageShippingSettings(object sender, EventArgs e)
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				using (ShippingSettingsDlg dlg = new ShippingSettingsDlg(lifetimeScope))
				{
					dlg.ShowDialog(this);
					ribbonSecurityProvider.UpdateSecurityUI();
				}
			}
		}

		/// <summary>
		/// Open the shipping settings window
		/// </summary>
		private void OnManageShippingProfiles(object sender, EventArgs e)
		{
			Messenger.Current.Send(new OpenProfileManagerDialogMessage(this));
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
			Messenger.Current.Send(new OpenShippingDialogWithOrdersMessage(this, gridControl.Selection.OrderedKeys, InitialShippingTabDisplay.Shipping));
		}

		/// <summary>
		/// Track shipments for the selected orders
		/// </summary>
		private void OnTrackShipments(object sender, EventArgs e)
		{
			Messenger.Current.Send(new OpenShippingDialogWithOrdersMessage(this, gridControl.Selection.OrderedKeys, InitialShippingTabDisplay.Tracking));
		}

		/// <summary>
		/// Submit an insurance claim for the selected orders
		/// </summary>
		private void OnSubmitClaim(object sender, EventArgs e)
		{
			Messenger.Current.Send(new OpenShippingDialogWithOrdersMessage(this, gridControl.Selection.OrderedKeys, InitialShippingTabDisplay.Insurance));
			// Show the shipping window.
		}

		/// <summary>
		/// The FedEx Close popup menu is opening, so we need to dynamically populate it
		/// </summary>
		private void OnFedExClosePopupOpening(object sender, BeforePopupEventArgs e) =>
			PopulateFedExCloseMenu(menuFedExPrintReports, menuFedExSmartPostClose);

		/// <summary>
		/// The FedEx Close popup menu is opening, so we need to dynamically populate it
		/// </summary>
		private void OnOrderLookupViewFedExClosePopupOpening(object sender, BeforePopupEventArgs e) =>
			PopulateFedExCloseMenu(menuOrderLookupViewFedExPrintReports, menuOrderLookupViewFedExSmartPostClose);

        /// <summary>
        /// Populate the FedEx close menu
        /// </summary>
        /// <param name="printMenu"></param>
        /// <param name="closeMenu"></param>
        private void PopulateFedExCloseMenu(Divelements.SandRibbon.Menu printMenu, SandMenuItem closeMenu)
        {
            using (var scope = IoC.BeginLifetimeScope())
            {
                var accountRetriever = scope.ResolveKeyed<ICarrierAccountRetriever>(ShipmentTypeCode.FedEx);

                PopulateShipEngineManifestMenu(closeMenu, printMenu, accountRetriever, scope);
            }

            closeMenu.Visible = FedExAccountManager.Accounts.Any(a => a.SmartPostHub != (int)FedExSmartPostHub.None);
        }

		/// <summary>
		/// Process FedEx end of day close
		/// </summary>
		private void OnFedExGroundClose(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;

                List<long> closings = FedExGroundClose.ProcessClose();

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
				using (var lifetimeScope = IoC.BeginLifetimeScope())
				{
					var clerk = lifetimeScope.Resolve<IFedExShippingClerkFactory>().Create(null);
					Cursor.Current = Cursors.WaitCursor;

					bool anyClosed = false;

					// Process all accounts with configured hub ids
					foreach (FedExAccountEntity account in FedExAccountManager.Accounts.Where(a => XElement.Parse(a.SmartPostHubList).Descendants("HubID").Any()))
					{
						if (clerk.CloseSmartPost(account) != null)
						{
							// A non-null closed entity was returned, so there were shipments that were closed for this account.
							anyClosed = true;
						}
					}

                    if (anyClosed)
                    {
                        MessageHelper.ShowInformation(this, "The close has been successfully processed.\n\nFedEx Ground® Economy Close does not generate any reports to be printed.  No further action is required.");
                    }
                    else
                    {
                        MessageHelper.ShowInformation(this, "There were no shipments to be closed.");
                    }
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
		private void OnPostalScanFormOpening(object sender, BeforePopupEventArgs e) =>
			PopulatePostalSCANFormMenu(menuCreateEndiciaScanForm, menuPrintEndiciaScanForm);

		/// <summary>
		/// The postal scan form popup is opening, we need to dynamically repopulate the print menu
		/// </summary>
		private void OnOrderLookupViewPostalScanFormOpening(object sender, BeforePopupEventArgs e) =>
			PopulatePostalSCANFormMenu(menuOrderLookupViewCreateEndiciaScanForm, menuOrderLookupViewPrintEndiciaScanForm);

		/// <summary>
		/// Populate the contents of the postal scan form menu
		/// </summary>
		private void PopulatePostalSCANFormMenu(SandMenuItem createMenu, Divelements.SandRibbon.Menu printMenu)
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				var repositories = lifetimeScope.Resolve<IEnumerable<IScanFormAccountRepository>>();

				ScanFormUtility.PopulateCreateScanFormMenu(createMenu, repositories);
				ScanFormUtility.PopulatePrintScanFormMenu(printMenu, repositories);
			}
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
		public void StartPrintJob(IPrintJob job, PrintAction action)
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
		/// The print preview window is now visible
		/// </summary>
		private void OnPrintPreviewShown(object sender, PrintPreviewShownEventArgs e)
		{
			if (InvokeRequired)
			{
				BeginInvoke((PrintPreviewShownEventHandler) OnPrintPreviewShown, sender, e);
				return;
			}

			// Once we are on the UI thread, close the progress window
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
		private void OnEmailResolveSettings(object sender, EmailSettingsResolveEventArgs e)
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
				using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
				{
					string message = lifetimeScope.Resolve<IConfigurationData>().IsArchive() ?
						ArchiveConstants.InvalidActionInArchiveMessage :
						string.Format("{0} messages were not sent due to insufficient permissions to send email.", e.SecurityDenials);

					lifetimeScope.Resolve<IMessageHelper>().ShowInformation(this, message);
				}
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
		/// Initiates the save writer indicating if the files should be opened after they are saved.
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
		private void OnSavePromptForFile(object sender, SavePromptForFileEventArgs e)
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

						// This waiting is b\c with certain apps - notably IE - if you open stuff too fast it misses it.
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

		/// <summary>
		/// Action when the user clicks the print pick list button
		/// </summary>
		private void OnPrintPickList(object sender, EventArgs e)
		{
			IEnumerable<long> selectedOrderIDs = gridControl.Selection.OrderedKeys;

			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				IPickListPrintingService pickListPrintingService =
					lifetimeScope.Resolve<IPickListPrintingService>();
				pickListPrintingService.PrintPickList(selectedOrderIDs);
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
			Telemetry.TrackButtonClick("QuickAccess.Print.Preview", string.Empty);
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
		/// The save and open popup is opening
		/// </summary>
		private void OnSaveOpenRibbonPopup(object sender, BeforePopupEventArgs e)
		{
			TemplateTreePopupController.ShowRibbonPopup((Popup) sender, e, OnSaveOpen);
		}

		#endregion

		#region Context Menu Handlers

		/// <summary>
		/// Quick Print menu is being opened
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

		#region Utility

		/// <summary>
		/// Returns true if any forms, other than the main UI form or floating panels, are open.  False otherwise.
		/// </summary>
		public bool AdditionalFormsOpen()
		{
			if (InvokeRequired)
			{
				return (bool) Invoke(new Func<bool>(AdditionalFormsOpen));
			}
			return Visible && !CanFocus;
		}

		#endregion

		/// <summary>
		/// The Create Asendia Manifest button is pushed
		/// </summary>
		private async void OnAsendiaManifest(object sender, EventArgs e)
		{
			using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
			{
				var result = await lifetimeScope.Resolve<IAsendiaManifestCreator>().CreateManifest().ConfigureAwait(true);

				if (result.Success)
				{
					MessageHelper.ShowMessage(this, "Asendia manifest created.");
					return;
				}
				MessageHelper.ShowError(this, "An error occurred creating the Asendia manifest");
				log.Error(result.Exception.Message);
			}
		}

		/// <summary>
		/// The DHL eCommerce manifest popup is opening, we need to dynamically repopulate the print menu
		/// </summary>
		private void OnDhlEcommerceManifestOpening(object sender, BeforePopupEventArgs e)
		{
			using (var scope = IoC.BeginLifetimeScope())
			{
				var accountRetriever = scope.ResolveKeyed<ICarrierAccountRetriever>(ShipmentTypeCode.DhlEcommerce);

				PopulateShipEngineManifestMenu(menuItemCreateDhlEcommerceManifest, menuPrintDhlEcommerceManifest, accountRetriever, scope);
			}
		}

		/// <summary>
		/// The DHL eCommerce manifest order lookup view popup is opening, we need to dynamically repopulate the print menu
		/// </summary>
		private void OnOrderLookupViewDhlEcommerceManifestOpening(object sender, BeforePopupEventArgs e)
		{
			using (var scope = IoC.BeginLifetimeScope())
			{
				var accountRetriever = scope.ResolveKeyed<ICarrierAccountRetriever>(ShipmentTypeCode.DhlEcommerce);

				PopulateShipEngineManifestMenu(menuItemOrderLookupViewCreateDhlEcommerceManifest, menuOrderLookupViewPrintDhlEcommerceManifest, accountRetriever, scope);
			}
		}

		/// <summary>
		/// Populate a ShipEngine manifest menu
		/// </summary>
		private void PopulateShipEngineManifestMenu(SandMenuItem createMenu,
			Divelements.SandRibbon.Menu printMenu,
			ICarrierAccountRetriever accountRetriever,
			ILifetimeScope scope)
		{
			var manifestUtility = scope.Resolve<IShipEngineManifestUtility>();

			manifestUtility.PopulateCreateManifestMenu(createMenu, accountRetriever);
			manifestUtility.PopulatePrintManifestMenu(printMenu, accountRetriever);
		}

		/// <summary>
		/// Check for any messages
		/// This is async void so we will fire-and-forget, and
		/// just display the messages as soon as we fetch them
		/// </summary>
		[SuppressMessage("SonarQube", "S3168:Async methods should not return void",
			Justification = "We want this to fire and forget")]
		private async void CheckForMessages()
		{
			try
			{
				log.Info("Checking for messages");

				using (var lifetimeScope = IoC.BeginLifetimeScope())
				{
					await lifetimeScope.Resolve<HubMessageRetriever>().GetMessages(this);
				}
			}
			catch (Exception ex)
			{
				log.Error("Failed to fetch messages", ex);
			}
		}

		/// <summary>
		/// Kick off the HubMigrator MigrateStoresToHub
		/// </summary>
		private void MigrateStoresToHub()
		{
			try
			{
				using (var lifetimeScope = IoC.BeginLifetimeScope())
				{
					if (lifetimeScope.Resolve<ILicenseService>().IsHub)
					{
						lifetimeScope.Resolve<HubMigrator>().MigrateStores(this);
					}
				}
			}
			catch (Exception ex)
			{
				log.Error("Failed to migrate stores to Hub", ex);
			}
		}

		/// <summary>
		/// Kick off the HubMigrator MigrateSqlConfigToHub
		/// </summary>
		private void MigrateSqlConfigToHub()
		{
			try
			{
				using (var lifetimeScope = IoC.BeginLifetimeScope())
				{
					if (lifetimeScope.Resolve<ILicenseService>().IsHub)
					{
						var migrator = lifetimeScope.Resolve<HubMigrator>();

						Task.Run(async () => await migrator.MigrateSqlConfigToHub());
					}
				}
			}
			catch (Exception ex)
			{
				log.Error("Failed to migrate SQL Config to Hub", ex);
			}
		}

		/// <summary>
		/// Synchronize our database with the config from Hub
		/// </summary>
		private void SynchronizeConfig()
		{
			try
			{
				using (var lifetimeScope = IoC.BeginLifetimeScope())
				{
					var configuration = lifetimeScope.Resolve<IConfigurationData>().FetchReadOnly();

					if (!string.IsNullOrEmpty(configuration.WarehouseID))
					{
						var hubConfig = lifetimeScope.Resolve<HubStoreImporter>().ImportStores(this, configuration.WarehouseID);
						lifetimeScope.Resolve<HubConfigurationSynchronizer>().Synchronize(hubConfig);
					}
				}
			}
			catch (Exception ex)
			{
				// Log a generic message here - more detailed error logging is done by the importer/synchronizer
				log.Error("Failed to synchronize with Hub configuration", ex);
			}
		}

		/// <summary>
		/// Convert legacy trial stores to non-trial stores. Trial status will be associated with the license moving forward
		/// </summary>
		private void ConvertLegacyTrialStores()
		{
			using (var lifetimeScope = IoC.BeginLifetimeScope())
			{
				var legacyTrialStoreConverter = lifetimeScope.Resolve<ILegacyStoreTrialKeyConverter>();

				legacyTrialStoreConverter.ConvertTrials();
			}
		}

		/// <summary>
		/// Convert MWS Amazon stores to SP
		/// </summary>
		private void ConvertAmazonStoresToSP()
		{
			using (var lifetimeScope = IoC.BeginLifetimeScope())
			{
				var amazonConverter = lifetimeScope.Resolve<IAmazonMwsToSpConverter>();

				amazonConverter.ConvertStores(this);
			}
		}

		/// <summary>
		/// Fetch missing Amazon OrderSourceIDs
		/// </summary>
		private void FetchAmazonOrderSourceIDs()
		{
			using (var lifetimeScope = IoC.BeginLifetimeScope())
			{
				var fetcher = lifetimeScope.Resolve<IAmazonOrderSourceIdFetcher>();

				fetcher.FetchOrderSourceIds(this);
			}
		}

        /// <summary>
        /// Migrate FedEx Accounts to ShipEngine
        /// </summary>
        private void MigrateFedExAccountsToShipEngine()
        {
            using (var lifetimeScope = IoC.BeginLifetimeScope())
            {
                var migrator = lifetimeScope.Resolve<IFedExShipEngineMigrator>();

                migrator.Migrate(this);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

			// Send telemetry data to Azure, giving it 2 seconds to complete.
			Telemetry.Flush();
			Thread.Sleep(2000);

			base.Dispose(disposing);
		}
	}
}
