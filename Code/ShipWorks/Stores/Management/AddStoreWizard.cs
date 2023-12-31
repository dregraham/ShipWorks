using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using Quartz.Util;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Setup;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Filters;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;
using ShipWorks.Users.Logon;
using ShipWorks.Users.Security;
using ShipWorks.Warehouse.Configuration.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;
using Control = System.Windows.Controls.Control;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Wizard for adding a new store to ShipWorks
    /// </summary>
    public partial class AddStoreWizard : WizardForm, IStoreWizard
    {
        private readonly ILifetimeScope scope;

        // State container for use by wizard pages
        private Dictionary<string, object> stateBag = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        // The store that is being created
        private StoreEntity store;
        private StoreType storeOverride;

        // All of the store specific wizard pages currently added.
        private List<WizardPage> storePages = new List<WizardPage>();

        // So we know when to update the config of the pages
        private long initialDownloadConfiguredStoreID = 0;
        private long onlineUpdateConfiguredStoreID = 0;

        // Indicates if we are in a freemium setup flow
        private bool isFreemiumMode = false;

        /// <summary>
        /// Indicates if we show the activation page.
        /// </summary>
        private bool showActivationError = false;
        private bool wasStoreSelectionSkipped;
        private readonly ILicenseService licenseService;
        private readonly Func<IChannelLimitFactory> channelLimitFactory;
        private readonly IDefaultWarehouseCreator defaultWarehouseCreator;

        private ActionConfiguration actionConfiguration;

        /// <summary>
        /// Constructor
        /// </summary>
        public AddStoreWizard(ILifetimeScope scope,
            ILicenseService licenseService,
            Func<IChannelLimitFactory> channelLimitFactory,
            IDefaultWarehouseCreator defaultWarehouseCreator)
        {
            this.scope = scope;
            this.licenseService = licenseService;
            this.channelLimitFactory = channelLimitFactory;
            this.defaultWarehouseCreator = defaultWarehouseCreator;
            InitializeComponent();

            // wire up custom event handlers here because the designer keeps deleting them
            wizardPageFinished.SteppingIntoAsync += OnSteppingIntoComplete;
            wizardPageAddress.SteppingIntoAsync += OnSteppingIntoAddress;
            wizardPageAddress.StepNextAsync += OnStepNextAddress;

            labelDefaultWarehosue.Visible = false;
        }

        #region Wizard Creation

        /// <summary>
        /// Run the setup wizard.  Will return false if the user doesn't have permissions, the user canceled, or if the Wizard was not able to run because
        /// it was already running on another computer.
        /// </summary>
        public static bool RunWizard(IWin32Window owner, OpenedFromSource openedFrom)
        {
            // See if we have permissions
            if (!(UserSession.Security.HasPermission(PermissionType.ManageStores)))
            {
                MessageHelper.ShowInformation(owner, "An administrator must log on to setup ShipWorks.");
                return false;
            }

            try
            {
                if (!IsLicenseCompliant(owner))
                {
                    return false;
                }

                using (new ShipWorksSetupLock())
                using (ILifetimeScope scope = IoC.BeginLifetimeScope(ConfigureAddStoreWizardDependencies))
                using (AddStoreWizard wizard = scope.Resolve<AddStoreWizard>())
                using (IStoreSettingsTrackedDurationEvent storeSettingsEvent =
                    scope.Resolve<IStoreSettingsTrackedDurationEvent>(
                        new TypedParameter(typeof(string), "Store.{0}.Setup")))
                {
                    wizard.OpenedFrom = openedFrom;

                    // Show the wizard and collect report the store configuration/settings
                    // for telemetry purposes
                    DialogResult dialogResult = wizard.ShowDialog(owner);

                    if (dialogResult == DialogResult.OK)
                    {
                        CollectTelemetry(wizard.Store, storeSettingsEvent, dialogResult, openedFrom);
                        // The store was added, so make sure our local list of stores is refreshed
                        StoreManager.CheckForChanges();
                        return true;
                    }

                    // If canceling out of the add store wizard, collect telemetry from the AbandonedStore property
                    CollectTelemetry(wizard.AbandonedStore, storeSettingsEvent, dialogResult, openedFrom);

                    return false;
                }
            }
            catch (SqlAppResourceLockException)
            {
                MessageHelper.ShowInformation(owner, "Another user is already setting up ShipWorks. This can only be done on one computer at a time.");
                return false;
            }
        }

        /// <summary>
        /// Collects the telemetry.
        /// </summary>
        private static void CollectTelemetry(StoreEntity store, IStoreSettingsTrackedDurationEvent storeSettingsEvent,
            DialogResult dialogResult, OpenedFromSource openedFrom)
        {
            if (store != null)
            {
                storeSettingsEvent.RecordStoreConfiguration(store);
            }

            storeSettingsEvent.AddProperty("Abandoned", dialogResult == DialogResult.OK ? "No" : "Yes");
            storeSettingsEvent.AddProperty("OpenedFrom", openedFrom.ToString());
        }

        /// <summary>
        /// Configures the add store wizard dependencies.
        /// </summary>
        private static void ConfigureAddStoreWizardDependencies(ContainerBuilder builder)
        {
            builder.RegisterType<AddStoreWizard>()
                .AsSelf()
                .As<IWin32Window>()
                .SingleInstance();
        }

        /// <summary>
        /// Returns true if license is in compliance.
        /// </summary>
        /// <remarks>
        /// Prompts user to get in compliance if needed.
        /// Then it checks to see if they are now in compliance and returns the result.
        /// </remarks>
        private static bool IsLicenseCompliant(IWin32Window owner)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = scope.Resolve<ILicenseService>();
                ILicense license = licenseService.GetLicenses().FirstOrDefault();

                if (license == null)
                {
                    return true; // must be legacy and no store set up...
                }

                // We need to force the refresh at this point to make sure we have the most
                // recent store/channel information prior to enforcing capabilities
                license.ForceRefresh();
                license.EnforceCapabilities(EnforcementContext.OnAddingStore, owner);

                try
                {
                    license.EnforceCapabilities(EnforcementContext.OnAddingStore);
                    return true;
                }
                catch (ShipWorksLicenseException ex)
                {
                    MessageHelper.ShowError(owner, ex.Message);
                    return false;
                }
            }
        }

        /// <summary>
        /// Designed to be called from the last step of another wizard where a brand new database and user account was just created, this makes it look to the user
        /// like the ShipWorks Setup wizard is a seamless continuation of the previous wizard.  The DialogResult of the ShipWorks Setup is used as the DialogResult
        /// that closes the original wizard.
        /// </summary>
        public static void ContinueAfterCreateDatabase(WizardForm originalWizard, string username, string password)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Initialize the session
            UserSession.InitializeForCurrentDatabase();

            bool complete = false;

            EnumResult<UserServiceLogonResultType> logonResult;

            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IUserService userService = scope.Resolve<IUserService>();
                logonResult = userService.Logon(new LogonCredentials(username, password, true));
            }

            if (logonResult.Value == UserServiceLogonResultType.Success)
            {
                // Initialize the session
                UserManager.InitializeForCurrentUser();
                UserSession.InitializeForCurrentSession(Program.ExecutionMode);

                originalWizard.BeginInvoke(new MethodInvoker(originalWizard.Hide));

                // Run the setup wizard
                complete = RunWizard(originalWizard, OpenedFromSource.InitialSetup);

                // If the wizard didn't complete, then the we can't exit this with the user still looking like they were logged in
                if (!complete)
                {
                    UserSession.Logoff(false);
                }
            }
            else
            {
                MessageHelper.ShowWarning(originalWizard.Owner, logonResult.Message);
            }

            // Counts as a cancel on the original wizard if they didn't complete the setup.
            originalWizard.DialogResult = complete ? DialogResult.OK : DialogResult.Cancel;
        }

        #endregion

        /// <summary>
        /// From where was this dialog opened
        /// </summary>
        public OpenedFromSource OpenedFrom { get; set; } = OpenedFromSource.NotSpecified;

        /// <summary>
        /// The store currently being configured by the wizard
        /// </summary>
        public StoreEntity Store => store;

        /// <summary>
        /// The store that was being configured when the wizard is abandoned
        /// </summary>
        public StoreEntity AbandonedStore { get; set; }

        /// <summary>
        /// Can the setup be skipped
        /// </summary>
        private bool CanSkipStoreSelection =>
            !licenseService.IsLegacy &&
            (OpenedFrom == OpenedFromSource.InitialSetup || OpenedFrom == OpenedFromSource.NoStores);

        /// <summary>
        /// Wizard is loading
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageStores);

            skipPanel.Visible = CanSkipStoreSelection;

            LoadStoreTypes();

            isFreemiumMode = FreemiumFreeEdition.IsActive;

            // Customize wizard experience based on edition
            if (isFreemiumMode)
            {
                StoreType storeType = StoreTypeManager.GetType(StoreTypeCode.Ebay);

                // Force this StoreType and it's wizard
                comboStoreType.Items.Clear();
                comboStoreType.Items.Add(new ImageComboBoxItem(storeType.StoreTypeName, storeType, EnumHelper.GetImage(storeType.TypeCode)));
                comboStoreType.SelectedIndex = 0;

                // Setup for the configured single store type
                SetupForStoreType(storeType);

                // Remove the initial page, since they can't choose now
                Pages.Remove(wizardPageStoreType);
            }
        }

        #region State Container

        /// <summary>
        /// Returns a named value from the state bag
        /// </summary>
        public object GetStateValue(string name)
        {
            if (stateBag.ContainsKey(name))
            {
                return stateBag[name];
            }

            return null;
        }

        /// <summary>
        /// Assigns a named value in the state bag
        /// </summary>
        public void SetStateValue(string name, object value)
        {
            stateBag[name] = value;
        }

        #endregion

        #region StoreType Page

        /// <summary>
        /// Load the list of store types to choose from
        /// </summary>
        private void LoadStoreTypes()
        {
            comboStoreType.Items.Add("Choose...");

            // Add each store type as a radio
            foreach (StoreType storeType in StoreTypeManager.StoreTypes.Where(s=>s.CanAddStoreType))
            {
                comboStoreType.Items.Add(new ImageComboBoxItem(storeType.StoreTypeName, storeType, EnumHelper.GetImage(storeType.TypeCode)));
            }

            comboStoreType.SelectedIndex = 0;
        }

        /// <summary>
        /// Entering the store type wizard page.
        /// </summary>
        private void OnSteppingIntoStoreType(object sender, WizardSteppingIntoEventArgs e)
        {
            wasStoreSelectionSkipped = false;
            storeOverride = null;

            UpdateStoreConnectionUI();
        }

        /// <summary>
        /// Changing the option to use a real store or sample orders
        /// </summary>
        private void OnChangeStoreConnection(object sender, EventArgs e)
        {
            UpdateStoreConnectionUI();
        }

        /// <summary>
        /// User has chosen a specific store type to try.
        /// </summary>
        private void OnChooseStoreType(object sender, EventArgs e)
        {
            UpdateStoreConnectionUI();
        }

        /// <summary>
        /// Update the UI for store connection
        /// </summary>
        private void UpdateStoreConnectionUI()
        {
            NextEnabled = SelectedStoreType != null;
        }

        /// <summary>
        /// Returns the StoreTypeBase that corresponds with the selected radio, if any.
        /// </summary>
        private StoreType SelectedStoreType
        {
            get
            {
                if (storeOverride != null)
                {
                    return storeOverride;
                }

                if (comboStoreType.SelectedIndex > 0)
                {
                    ImageComboBoxItem item = (ImageComboBoxItem) comboStoreType.SelectedItem;

                    return (StoreType) item.Value;
                }

                return null;
            }
        }

        /// <summary>
        /// Stepping next from the initial page.
        /// </summary>
        private void OnStepNextStoreType(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            StoreType storeType = SelectedStoreType;

            if (storeType == null)
            {
                MessageHelper.ShowInformation(this, "Please select the online platform that you use.");

                e.NextPage = CurrentPage;
                return;
            }

            // Setup for the selected\licensed storetype
            SetupForStoreType(storeType);
            store.License = "";

            // Move to the now newly inserted next page
            e.NextPage = Pages[Pages.IndexOf(CurrentPage) + 1];
        }

        /// <summary>
        /// Setup the wizard UI for the given store type
        /// </summary>
        private void SetupForStoreType(StoreType storeType)
        {
            // If we are changing store types, clear the existing pages
            if (store == null || store.TypeCode != (int) storeType.TypeCode)
            {
                // If there was an old store we need to clean it up
                if (store != null)
                {
                    DeletionService.DeleteStore(store, UserSession.Security);
                    store = null;
                }

                // Remove all pages we added.
                foreach (WizardPage page in storePages)
                {
                    this.Pages.Remove(page);
                }

                storePages.Clear();

                // Create the store instance of this type
                store = storeType.CreateStoreInstance();

                // Fill in non-null values for anything the store is not yet configured for
                store.InitializeNullsToDefault();
                store.StartSetup();

                // Save it right away It is marked as SetupComplete = false so it doesn't show up
                // to the rest of ShipWorks.  We save it right away so we can get a PK for wizard steps that create FK tables.
                StoreManager.SaveStore(store);

                // Load the store specific pages
                storePages = storeType.CreateAddStoreWizardPages(scope);

                // Add all the pages
                for (int i = storePages.Count - 1; i >= 0; i--)
                {
                    storePages[i].Dock = DockStyle.Fill;
                    Pages.Insert(Pages.IndexOf(CurrentPage) + 1, storePages[i]);
                }
            }
        }

        #endregion

        #region Address

        /// <summary>
        /// Stepping into the address page
        /// </summary>
        private async Task OnSteppingIntoAddress(object sender, WizardSteppingIntoEventArgs e)
        {
            if ((await defaultWarehouseCreator.NeedsDefaultWarehouse().ConfigureAwait(true)).Value)
            {
                labelDefaultWarehosue.Visible = true;
            }

            storeAddressControl.LoadStore(store);
        }

        /// <summary>
        /// Stepping next from the address page
        /// </summary>
        private async Task OnStepNextAddress(object sender, WizardStepEventArgs e)
        {
            storeAddressControl.SaveToEntity(store);

            // Do an initial check on the name
            if (!StoreManager.CheckStoreName(store.StoreName, this))
            {
                e.NextPage = CurrentPage;
                return;
            }

            var shouldCreateDefaultWarehouse = (await defaultWarehouseCreator.NeedsDefaultWarehouse().ConfigureAwait(true)).Value;

            if (shouldCreateDefaultWarehouse && !ValidateWarehouseAddress(store))
            {
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Validate that the required warehouse filds are filled out
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        private bool ValidateWarehouseAddress(StoreEntity store)
        {
            PersonAdapter storeAddress = store.Address;

            var results = new List<Result>()
            {
                storeAddress.StreetAll.ValidateLength(30, 1, "The street address must not be empty."),
                storeAddress.City.ValidateLength(30, 1, "The city must not be empty."),
            };

            if (!Regex.IsMatch(storeAddress.PostalCode, "^([0-9]{5})(-?([0-9]{4}))?$"))
            {
                results.Add(Result.FromError("The postal code you entered is not valid."));
            }

            if (results.Any(r => r.Failure))
            {
                MessageHelper.ShowError(this, string.Join(Environment.NewLine, results.Where(r => r.Failure).Select(r => r.Message)));
                return false;
            }

            return true;
        }

        #endregion

        #region ContactInfo Page

        /// <summary>
        /// Stepping into the page to edit contact info
        /// </summary>
        private void OnSteppingIntoContactInfo(object sender, WizardSteppingIntoEventArgs e)
        {
            storeContactControl.LoadStore(store);
        }

        /// <summary>
        /// Stepping out of the contact info page
        /// </summary>
        private void OnStepNextContactInfo(object sender, WizardStepEventArgs e)
        {
            // Save the contact info
            storeContactControl.SaveToEntity(store);
        }

        #endregion

        #region Settings Page

        /// <summary>
        /// Stepping into the settings page
        /// </summary>
        private void OnSteppingIntoSettings(object sender, WizardSteppingIntoEventArgs e)
        {
            StoreType storeType = StoreTypeManager.GetType(Store);
            if (!storeType.ShowTaskWizardPage())
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;
            }
            else
            {
                // We can't skip this screen anymore since all stores will have the option of auto validating addresses
                e.Skip = false;
                e.RaiseStepEventWhenSkipping = false;
                PrepareSettingsInitialDownloadUI(e);
                PrepareSettingsActionUI(e);
            }
        }

        /// <summary>
        /// Prepare the UI for the initial download settings
        /// </summary>
        private bool PrepareSettingsInitialDownloadUI(WizardSteppingIntoEventArgs e)
        {
            StoreType storeType = StoreTypeManager.GetType(Store);
            InitialDownloadPolicy policy = storeType.InitialDownloadPolicy;

            // Only reset the UI if the store has changed.  The only potential issue here is with generic where you could actually
            // be pointing to a totally different capabilities type without changing store types, but thats unlikely, and not catastrophic.
            // We also want to reset ui for ODBC, since it is possible for them to go back and change the download strategy. We don't want
            // to show the "Download all of my orders" option for ODBC download by last modified.
            if (initialDownloadConfiguredStoreID != store.StoreID || e.FirstTime || Store.TypeCode == (int) StoreTypeCode.Odbc)
            {
                store.InitialDownloadDays = null;
                store.InitialDownloadOrder = null;

                // Don't show initial download policy
                if (policy.RestrictionType == InitialDownloadRestrictionType.None)
                {
                    // Hide the download range settings completely
                    panelDownloadSettings.Visible = false;

                    return false;
                }
                else
                {
                    initialDownloadConfiguredStoreID = store.StoreID;

                    // Show the download range settings
                    panelDownloadSettings.Visible = true;

                    // Initially, the simple view will be shown and the editor wont.
                    panelViewDownloadRange.Visible = true;
                    panelEditDownloadRange.Visible = false;
                    panelDownloadSettings.Height = panelViewDownloadRange.Bottom + 5;

                    // Restrict by order number
                    if (policy.RestrictionType == InitialDownloadRestrictionType.OrderNumber)
                    {
                        downloadRange.Text = (policy.DefaultStartingOrderNumber == 0) ? "Your first order" : string.Format("Order {0}", policy.DefaultStartingOrderNumber);

                        panelDateRange.Visible = false;

                        panelFirstOrder.Visible = true;
                        panelFirstOrder.Top = panelDateRange.Top;

                        initialDownloadFirstOrder.Text = policy.DefaultStartingOrderNumber.ToString();
                        radioStartNumberLimit.Checked = true;
                    }
                    // Restrict by date range
                    else
                    {
                        downloadRange.Text = string.Format("{0} days ago", policy.DefaultDaysBack);

                        panelDateRange.Visible = true;
                        panelFirstOrder.Visible = false;

                        dateRangeDays.Text = policy.DefaultDaysBack.ToString();
                        radioDateRangeDays.Checked = true;

                        dateRangeRadioHider.Visible = policy.MaxDaysBack != null;
                        radioDateRangeAll.Visible = policy.MaxDaysBack == null;
                    }

                    // update link position for edit mode
                    linkEditDownloadRange.Left = downloadRange.Right + 3;
                }
            }

            return true;
        }

        /// <summary>
        /// Prepare the UI for the actions stuff
        /// </summary>
        private bool PrepareSettingsActionUI(WizardSteppingIntoEventArgs e)
        {
            // Ensure no actions are left over from this store, b\c we'll be recreating them when we move next
            ActionManager.DeleteStoreActions(store.StoreID);

            if (e.StepReason == WizardStepReason.StepBack)
            {
                return true;
            }

            StoreType storeType = StoreTypeManager.GetType(store);

            OnlineUpdateActionControlBase onlineUpdateControl = storeType.CreateAddStoreWizardOnlineUpdateActionControl();
            if (onlineUpdateControl == null)
            {
                // Clear out any previous control they may have had
                panelOnlineUpdatePlaceholder.Controls.Clear();
                onlineUpdateConfiguredStoreID = 0;
                panelUploadSettings.Visible = false;

                return false;
            }

            panelUploadSettings.Visible = true;

            // See if the existing one is already for this store
            if (store.StoreID == onlineUpdateConfiguredStoreID)
            {
                onlineUpdateControl = (OnlineUpdateActionControlBase) panelOnlineUpdatePlaceholder.Controls[0];
            }
            else
            {
                // Clear out any previous control they may have had and add the new one
                panelOnlineUpdatePlaceholder.Controls.Clear();
                panelOnlineUpdatePlaceholder.Controls.Add(onlineUpdateControl);

                // Update what store we are configured for
                onlineUpdateConfiguredStoreID = store.StoreID;
            }

            // Let the control initialize itself
            onlineUpdateControl.UpdateForStore(store);

            panelOnlineUpdatePlaceholder.Width = onlineUpdateControl.Width;
            panelOnlineUpdatePlaceholder.Height = onlineUpdateControl.Height;
            panelUploadSettings.Height = panelOnlineUpdatePlaceholder.Bottom;

            return true;
        }

        /// <summary>
        /// User wants to edit the initial download range
        /// </summary>
        private void OnEditInitialDownloadRange(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelViewDownloadRange.Visible = false;

            panelEditDownloadRange.Top = panelViewDownloadRange.Top;
            panelEditDownloadRange.Visible = true;

            panelEditDownloadRange.Height = panelDateRange.Bottom;
            panelDownloadSettings.Height = panelEditDownloadRange.Bottom;

        }

        /// <summary>
        /// Changing the selected radio option
        /// </summary>
        private void OnChangeInitialDownloadOption(object sender, EventArgs e)
        {
            dateRangeDays.Enabled = radioDateRangeDays.Checked;
            initialDownloadFirstOrder.Enabled = radioStartNumberLimit.Checked;
        }

        /// <summary>
        /// Stepping next from the settings page
        /// </summary>
        private void OnStepNextSettings(object sender, WizardStepEventArgs e)
        {
            if (!SaveSettingsInitialDownload())
            {
                e.NextPage = CurrentPage;
                return;
            }

            if (!SaveSettingsActions())
            {
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Save the settings from the initial download control
        /// </summary>
        private bool SaveSettingsInitialDownload()
        {
            StoreType storeType = StoreTypeManager.GetType(Store);
            InitialDownloadPolicy policy = storeType.InitialDownloadPolicy;

            // Start over, and we'll fill it back in..
            store.InitialDownloadDays = null;
            store.InitialDownloadOrder = null;

            // Restrict by order number
            if (policy.RestrictionType == InitialDownloadRestrictionType.OrderNumber)
            {
                if (radioStartNumberLimit.Checked)
                {
                    // Make sure the entered something
                    if (initialDownloadFirstOrder.Text.Trim().Length == 0)
                    {
                        MessageHelper.ShowInformation(this, "Enter an order number that ShipWorks should start downloading from.");
                        return false;
                    }

                    long firstOrder;
                    if (!long.TryParse(initialDownloadFirstOrder.Text.Trim(), out firstOrder))
                    {
                        MessageHelper.ShowInformation(this, "Enter a valid order number that ShipWorks should start downloading from.");
                        return false;
                    }

                    store.InitialDownloadOrder = firstOrder;
                }
            }

            // Restrict by date range
            if (policy.RestrictionType == InitialDownloadRestrictionType.DaysBack)
            {
                if (radioDateRangeDays.Checked)
                {
                    // Make sure the entered something
                    if (dateRangeDays.Text.Trim().Length == 0)
                    {
                        MessageHelper.ShowInformation(this, "Enter the number of days ShipWorks should go back.");
                        return false;
                    }

                    int daysBack;
                    if (!int.TryParse(dateRangeDays.Text.Trim(), out daysBack))
                    {
                        MessageHelper.ShowInformation(this, "Enter a valid number of days.");
                        return false;
                    }

                    if (daysBack <= 0 || (policy.MaxDaysBack != null && daysBack > policy.MaxDaysBack))
                    {
                        if (policy.MaxDaysBack != null)
                        {
                            MessageHelper.ShowInformation(this, string.Format("The number of days back must be between 1 and {0}.", policy.MaxDaysBack));
                        }
                        else
                        {
                            MessageHelper.ShowInformation(this, "The number of days back must be greater than zero.");
                        }

                        return false;
                    }

                    store.InitialDownloadDays = daysBack;
                }
            }

            return true;
        }

        /// <summary>
        /// Save the settings from the actions ui
        /// </summary>
        private bool SaveSettingsActions()
        {
            // If this store does not support uploads, the online update placeholder will have no controls so just return.
            if (panelOnlineUpdatePlaceholder.Controls.Count == 0)
            {
                return true;
            }

            OnlineUpdateActionControlBase control = (OnlineUpdateActionControlBase) panelOnlineUpdatePlaceholder.Controls[0];

            List<ActionTask> tasks;

            try
            {
                // See what tasks are configured to be created
                tasks = control.CreateActionTasks(scope, store);
            }
            catch (OnlineUpdateActionCreateException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);
                return false;
            }

            // If there are any, we need to create the action for it
            if (tasks != null && tasks.Count > 0)
            {
                ActionEntity action = null;

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Setup the basic action
                    action = new ActionEntity();
                    action.Name = "Store Update";
                    action.Enabled = true;

                    action.ComputerLimitedType = (int) ComputerLimitedType.TriggeringComputer;
                    action.InternalComputerLimitedList = string.Empty;

                    action.StoreLimited = true;
                    action.StoreLimitedList = new long[] { store.StoreID };

                    // Setup the trigger.  We know ShipmentProcessedTrigger doesn't save extra state to the DB, so we don't need to call that function.
                    ShipmentProcessedTrigger trigger = new ShipmentProcessedTrigger();
                    action.TriggerType = (int) trigger.TriggerType;
                    action.TriggerSettings = trigger.GetXml();

                    // Set the summary
                    action.TaskSummary = ActionManager.GetTaskSummary(tasks);

                    // Save the action
                    ActionManager.SaveAction(action, adapter);

                    // Save all the tasks
                    foreach (ActionTask task in tasks)
                    {
                        // Make sure the StepIndex is accurate
                        task.Entity.StepIndex = tasks.IndexOf(task);

                        task.Save(action, adapter);
                    }

                    adapter.Commit();
                }

                actionConfiguration = new ActionConfiguration
                {
                    Action = action,
                    Tasks = tasks.Select(t => t.Entity)
                };
            }

            return true;
        }

        #endregion

        #region Complete

        /// <summary>
        /// Stepping into the complete page
        /// </summary>
        private async Task OnSteppingIntoComplete(object sender, WizardSteppingIntoEventArgs e)
        {
            NextEnabled = false;
            Cursor = Cursors.WaitCursor;

            wizardPageFinished.LoadDownloadControl();

            try
            {
                // Make sure we have a fresh up-to-date layout context in case we need to create store-specific filters
                FilterLayoutContext.PushScope();

                if (!ValidateLicense(e))
                {
                    return;
                }

                await CreateDefaultWarehouse(store).ConfigureAwait(true);

                if (store.WarehouseStoreID == null &&
                    SelectedStoreType.ShouldUseHub(store) &&
                    (await UploadStoreToWarehouse(e).ConfigureAwait(true)).Failure)
                {
                    return;
                }

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    SaveStore(adapter);
                    SaveUIMode(adapter);

                    adapter.Commit();
                }

                // Sync the store after it's been successfully saved
                var syncResult = await scope.Resolve<IHubStoreSynchronizer>().SynchronizeStore(store, actionConfiguration).ConfigureAwait(true);

                if (syncResult.Failure)
                {
                    var logger = scope.Resolve<Func<Type, ILog>>();
                    logger(typeof(StoreSettingsDlg)).Error("Failed to Sync store to Hub.", syncResult.Exception);
                }

                // Refresh the nudges, just in case there were any that shouldn't be displayed now due to the addition of this store.
                // Ask the store manager to check for changes so that it returns the store we just added.  The heart beat may
                // not have run to force the check yet.
                StoreManager.CheckForChanges();
                NudgeManager.Refresh(StoreManager.GetAllStores());

                // Ensure the license has the latest capabilities now that a new store
                // has been added.
                licenseService.GetLicense(store)?.ForceRefresh();

                SelectedStoreType.RaiseStoreAdded(store, scope);
            }
            catch (DuplicateNameException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);

                e.Skip = true;
                e.SkipToPage = wizardPageContactInfo;
            }
            finally
            {
                FilterLayoutContext.PopScope();
                Cursor = DefaultCursor;
                NextEnabled = true;
            }
        }

        /// <summary>
        /// Create a default warehouse if needed
        /// </summary>
        private async Task CreateDefaultWarehouse(StoreEntity store)
        {
            if ((await defaultWarehouseCreator.NeedsDefaultWarehouse().ConfigureAwait(true)).Value)
            {
                var createResult = await defaultWarehouseCreator.Create(store).ConfigureAwait(true);

                if (createResult.Failure)
                {
                    MessageHelper.ShowError(this, $"An error occurred creating your default warehouse.{Environment.NewLine}{createResult.Message}");
                }
            }
        }

        /// <summary>
        /// Upload the store to the ShipWorks Warehouse app
        /// </summary>
        private async Task<Result> UploadStoreToWarehouse(WizardSteppingIntoEventArgs e)
        {
            using (var innerScope = scope.BeginLifetimeScope())
            {
                Result result = await innerScope.Resolve<IWarehouseStoreClient>().UploadStoreToWarehouse(store, true)
                   .ConfigureAwait(true);

                if (result.Failure)
                {
                    innerScope.Resolve<Func<Type, ILog>>()(typeof(AddStoreWizard)).Error(result.Message);

                    MessageHelper.ShowError(
                        this,
                        $"An error occurred saving the store to ShipWorks.");
                    e.Skip = true;
                    e.SkipToPage = wizardPageContactInfo;
                    BackEnabled = true;
                }

                return result;
            }
        }

        /// <summary>
        /// Saves the selected UIMode
        /// </summary>
        private void SaveUIMode(SqlAdapter adapter)
        {
            UserSettingsEntity settings = UserSession.User.Settings;

            // The only time the user will be presented with the uiModeSelectionControl is
            // when the user is first created signified by UIMode being set to Pending.
            if (settings.UIMode == UIMode.Pending)
            {
                uiModeSelectionControl.SaveTo(settings);
                adapter.SaveAndRefetch(settings);
            }
        }

        /// <summary>
        /// Save the store using the given adapter
        /// </summary>
        private void SaveStore(SqlAdapter adapter)
        {
            // Create the default presets
            CreateDefaultStatusPreset(store, StatusPresetTarget.Order, adapter);
            CreateDefaultStatusPreset(store, StatusPresetTarget.OrderItem, adapter);

            StoreFilterRepository storeFilterRepository = new StoreFilterRepository(store);
            storeFilterRepository.Save(true);

            AdjustShipmentType();

            // Mark that this store is now ready
            store.CompleteSetup();
            StoreManager.SaveStore(store, adapter);

            if (wasStoreSelectionSkipped && OpenedFrom == OpenedFromSource.InitialSetup)
            {
                var origin = new ShippingOriginEntity();
                origin.InitializeNullsToDefault();
                origin.Description = store.StoreName;

                new PersonAdapter(store, string.Empty).CopyTo(origin, string.Empty);

                var value = store.StoreName;

                PersonName name = PersonName.Parse(value);

                int maxFirst = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonFirst);
                if (name.First.Length > maxFirst)
                {
                    name.Middle = name.First.Substring(maxFirst) + name.Middle;
                    name.First = name.First.Substring(0, maxFirst);
                }

                int maxMiddle = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonMiddle);
                if (name.Middle.Length > maxMiddle)
                {
                    name.Last = name.Middle.Substring(maxMiddle) + name.Last;
                    name.Middle = name.Middle.Substring(0, maxMiddle);
                }

                int maxLast = EntityFieldLengthProvider.GetMaxLength(EntityFieldLengthSource.PersonLast);
                if (name.Last.Length > maxLast)
                {
                    name.Last = name.Last.Substring(0, maxLast);
                }

                origin.FirstName = name.First;
                origin.MiddleName = name.Middle;
                origin.LastName = name.Last;

                adapter.SaveEntity(origin);
            }
        }

        /// <summary>
        /// Adjust the default shipment type based on edition
        /// </summary>
        private void AdjustShipmentType()
        {
            if (StoreManager.GetAllStores().Count == 0)
            {
                ShipmentTypeCode? defaultType = EditionSerializer.Restore(store).DefaultShipmentType;

                if (defaultType != null)
                {
                    ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
                    shippingSettings.DefaultType = (int) defaultType.Value;

                    ShippingSettings.Save(shippingSettings);
                }
            }
        }

        /// <summary>
        /// Validates the License - sets up skip properties when appropriate
        /// </summary>
        private bool ValidateLicense(WizardSteppingIntoEventArgs e)
        {
            BackEnabled = false;

            if (licenseService.IsLegacy)
            {
                var customerLicenseKey = licenseService.GetCustomerLicenseKey(CustomerLicenseKeyType.Legacy);
                var response = TangoWebClient.AddStore(customerLicenseKey, store);
                store.License = response.Key;
            }
            else
            {
                ILicense license = licenseService.GetLicense(store);

                EnumResult<LicenseActivationState> activateResult;
                try
                {
                    activateResult = license.Activate(store);
                }
                catch (TangoException)
                {
                    activateResult = null;
                }

                if (activateResult?.Value != LicenseActivationState.Active)
                {
                    e.Skip = true;

                    if (activateResult?.Value == LicenseActivationState.MaxChannelsExceeded)
                    {
                        IChannelLimitFactory factory = channelLimitFactory();
                        Control channelLimitControl =
                            (Control) factory.CreateControl((ICustomerLicense) license, (StoreTypeCode) Store.TypeCode,
                                EditionFeature.ChannelCount, this);

                        wizardPageActivationError.SetElementHost(channelLimitControl);
                    }

                    showActivationError = true;
                    wizardPageActivationError.ErrorMessage = activateResult?.Message ?? "Error activating license.";
                    e.SkipToPage = wizardPageActivationError;
                }
            }

            return !e.Skip;
        }

        /// <summary>
        /// Create a default status preset for the store
        /// </summary>
        private void CreateDefaultStatusPreset(StoreEntity store, StatusPresetTarget presetTarget, SqlAdapter adapter)
        {
            StatusPresetEntity preset = new StatusPresetEntity();
            preset.StoreID = store.StoreID;
            preset.StatusTarget = (int) presetTarget;
            preset.StatusText = "";
            preset.IsDefault = true;

            adapter.SaveEntity(preset);
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            // If there was a store we need to clean it up
            if (DialogResult != DialogResult.OK)
            {
                if (store != null)
                {
                    AbandonedStore = store.DeepClone();
                    DeletionService.DeleteStore(store, UserSession.Security);
                    store = null;
                }
            }
        }

        #endregion

        /// <summary>
        /// Skip this page if no error has occurred.
        /// </summary>
        private void OnSteppingIntoWizardPageActivationError(object sender, WizardSteppingIntoEventArgs e)
        {
            BackEnabled = false;
            e.Skip = !showActivationError;
            showActivationError = false;
        }

        /// <summary>
        /// Skip the store setup
        /// </summary>
        private void OnSkipButtonClick(object sender, EventArgs e)
        {
            var manualStoreType = comboStoreType.Items
                .OfType<ImageComboBoxItem>()
                .Select(x => x.Value)
                .OfType<StoreType>()
                .FirstOrDefault(x => x.TypeCode == StoreTypeCode.Manual);

            if (manualStoreType != null)
            {
                storeOverride = manualStoreType;
                MoveNext();
                wasStoreSelectionSkipped = true;
            }
        }
        /// <summary>
        /// Help link for Manual Store
        /// </summary>
        private void ManualStoreHelpLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://shipworks.zendesk.com/hc/en-us/articles/360022464792", this);
        }

        /// <summary>
        /// The only time the user will be presented with the uiModeSelectionControl is
        /// when the user is first created signified by UIMode being set to Pending.
        /// </summary>
        private void OnSteppingIntoWizardPageUIMode(object sender, WizardSteppingIntoEventArgs e)
        {
            if (UserSession.User.Settings.UIMode != UIMode.Pending)
            {
                e.Skip = true;
            }
        }
    }
}
