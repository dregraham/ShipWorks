using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Setup;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Filters;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Wizard;
using ShipWorks.Users;
using ShipWorks.Users.Logon;
using ShipWorks.Users.Security;
using Control = System.Windows.Controls.Control;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Wizard for adding a new store to ShipWorks
    /// </summary>
    [NDependIgnoreLongTypes]
    partial class AddStoreWizard : WizardForm
    {
        // State container for use by wizard pages
        Dictionary<string, object> stateBag = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        // The store that is being created
        StoreEntity store;

        // All of the store specific wizard pages currently added.
        List<WizardPage> storePages = new List<WizardPage>();

        // If doing a trial, this is the current trial data
        TrialDetail trialDetail;

        // So we know when to update the config of the pages
        long initialDownloadConfiguredStoreID = 0;
        long onlineUpdateConfiguredStoreID = 0;

        // Indicates if we are in a freemium setup flow
        bool isFreemiumMode = false;

        /// <summary>
        /// Indicates if we show the activation page.
        /// </summary>
        private bool showActivationError = false;

        private readonly ILicenseService licenseService = IoC.UnsafeGlobalLifetimeScope.Resolve<ILicenseService>();

        /// <summary>
        /// Constructor
        /// </summary>
        private AddStoreWizard()
        {
            InitializeComponent();
        }

        #region Wizard Creation

        /// <summary>
        /// Run the setup wizard.  Will return false if the user doesn't have permissions, the user canceled, or if the Wizard was not able to run because
        /// it was already running on another computer.
        /// </summary>
        public static bool RunWizard(IWin32Window owner)
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

                using (ShipWorksSetupLock wizardLock = new ShipWorksSetupLock())
                {
                    using (AddStoreWizard wizard = new AddStoreWizard())
                    {
                        // If it was successful, make sure our local list of stores is refreshed
                        if (wizard.ShowDialog(owner) == DialogResult.OK)
                        {
                            StoreManager.CheckForChanges();

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (SqlAppResourceLockException)
            {
                MessageHelper.ShowInformation(owner, "Another user is already setting up ShipWorks. This can only be done on one computer at a time.");
                return false;
            }
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

            IUserService userService = IoC.UnsafeGlobalLifetimeScope.Resolve<IUserService>();

            EnumResult<UserServiceLogonResultType> logonResult = userService.Logon(new LogonCredentials(username, password, true));

            if (logonResult.Value == UserServiceLogonResultType.Success)
            {
                // Initialize the session
                UserManager.InitializeForCurrentUser();
                UserSession.InitializeForCurrentSession(Program.ExecutionMode);

                originalWizard.BeginInvoke(new MethodInvoker(originalWizard.Hide));

                // Run the setup wizard
                complete = RunWizard(originalWizard);

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
        /// The store currently being configured by the wizard
        /// </summary>
        public StoreEntity Store
        {
            get { return store; }
        }

        /// <summary>
        /// Wizard is loading
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageStores);

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
            foreach (StoreType storeType in StoreTypeManager.StoreTypes)
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
            NextEnabled = SelectedStoreType != null || radioStoreSamples.Checked;

            comboStoreType.Enabled = radioStoreConnect.Checked;

            if (radioStoreSamples.Checked)
            {
                // Index zero is the choose option
                comboStoreType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Returns the StoreTypeBase that corresponds with the selected radio, if any.
        /// </summary>
        private StoreType SelectedStoreType
        {
            get
            {
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
            if (radioStoreSamples.Checked)
            {
                MessageHelper.ShowError(this, "Not done.");
                e.NextPage = CurrentPage;
            }
            else
            {
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

                // Save it right away It is marked as SetupComplete = false so it doesn't show up
                // to the rest of ShipWorks.  We save it right away so we can get a PK for wizard steps that create FK tables.
                StoreManager.SaveStore(store);

                // Load the store specific pages
                storePages = storeType.CreateAddStoreWizardPages();

                // Add all the pages
                for (int i = storePages.Count - 1; i >= 0; i--)
                {
                    Pages.Insert(Pages.IndexOf(CurrentPage) + 1, storePages[i]);
                }
            }
        }

        #endregion

        #region Already Active

        /// <summary>
        /// Stepping into the page that determines if the store is already active
        /// </summary>
        private void OnSteppingIntoAlreadyActive(object sender, WizardSteppingIntoEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ILicense license = licenseService.GetLicense(store);

                // If we are legacy or are in freemium mode, and we already have a valid freemium edition store, then don't try to get a trial - it would fail anyway saying they aren't eligible
                if (!license.IsLegacy || (isFreemiumMode && (EditionSerializer.Restore(store) is FreemiumFreeEdition) && new ShipWorksLicense(store.License).IsValid))
                {
                    // We already have their license
                    e.Skip = true;
                    e.RaiseStepEventWhenSkipping = false;
                }
                else
                {
                    // Create a license for the given store
                    trialDetail = TangoWebClient.GetTrial(store);

                    // Save the license
                    store.License = trialDetail.License.Key;
                    store.Edition = EditionSerializer.Serialize(trialDetail.Edition);

                    // If it's not converted, then we can skip this page
                    if (!trialDetail.IsConverted)
                    {
                        // Can't attach to a freemium trial if not freemium mode
                        if (trialDetail.Edition is FreemiumFreeEdition && StoreManager.GetDatabaseStoreCount() > 0)
                        {
                            MessageHelper.ShowError(this, string.Format("You are already using ShipWorks with eBay ID '{0}' with the Endicia Free for eBay edition. That edition only supports a single store, and you already have some stores in ShipWorks.", StoreTypeManager.GetType(store).LicenseIdentifier));

                            // Go back to the previous page
                            e.Skip = true;
                            e.SkipToPage = Pages[CurrentIndex - 1];
                        }

                        // No license, just skip
                        e.Skip = true;
                        e.RaiseStepEventWhenSkipping = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is ShipWorksLicenseException || ex is TangoException)
                {
                    MessageHelper.ShowError(this, ex.Message);

                    // Go back to the previous page
                    e.Skip = true;
                    e.SkipToPage = Pages[CurrentIndex - 1];
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Stepping next from the already active license page
        /// </summary>
        private void OnStepNextAlreadyActive(object sender, WizardStepEventArgs e)
        {
            if (EditionSerializer.Restore(store) is FreemiumFreeEdition && StoreManager.GetDatabaseStoreCount() > 0)
            {
                MessageHelper.ShowError(this, "The license you entered is for the Endicia Free for eBay ShipWorks edition.  That edition only supports a single store, and you already have some stores in ShipWorks.");

                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Open the link for a user to get their license key
        /// </summary>
        private void OnHelpFindLicenseKey(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://www.interapptive.com/account", this);
        }

        #endregion

        #region Address

        /// <summary>
        /// Stepping into the address page
        /// </summary>
        private void OnSteppingIntoAddress(object sender, WizardSteppingIntoEventArgs e)
        {
            storeAddressControl.LoadStore(store);
        }

        /// <summary>
        /// Stepping next from the address page
        /// </summary>
        private void OnStepNextAddress(object sender, WizardStepEventArgs e)
        {
            storeAddressControl.SaveToEntity(store);

            // Do an initial check on the name
            if (!StoreManager.CheckStoreName(store.StoreName, this))
            {
                e.NextPage = CurrentPage;
                return;
            }
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
            // The generic file store type has no controls to put in the settings page
            // which results in a blank wizard page, so skip it.
            if (store.TypeCode == (int) StoreTypeCode.GenericFile)
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
            if (initialDownloadConfiguredStoreID != store.StoreID || e.FirstTime)
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
                tasks = control.CreateActionTasks(store);
            }
            catch (OnlineUpdateActionCreateException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);
                return false;
            }

            // If there are any, we need to create the action for it
            if (tasks != null && tasks.Count > 0)
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Setup the basic action
                    ActionEntity action = new ActionEntity();
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
            }

            return true;
        }

        #endregion

        #region Complete

        /// <summary>
        /// User clicked the link to open the getting started guide
        /// </summary>
        private void OnLinkGettingStartedGuide(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.shipworks.com/shipworks/help/ShipWorks-GettingStarted.pdf", this);
        }

        /// <summary>
        /// Stepping into the complete page
        /// </summary>
        private void OnSteppingIntoComplete(object sender, WizardSteppingIntoEventArgs e)
        {
            try
            {
                // Make sure we have a fresh up-to-date layout context in case we need to create store-specific filters
                FilterLayoutContext.PushScope();

                if (!string.IsNullOrEmpty(licenseKey.Text))
                {
                    store.License = licenseKey.Text;
                }

                if (!ValidateLicense(e))
                {
                    return;
                }

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Create the default presets
                    CreateDefaultStatusPreset(store, StatusPresetTarget.Order, adapter);
                    CreateDefaultStatusPreset(store, StatusPresetTarget.OrderItem, adapter);

                    StoreFilterRepository storeFilterRepository = new StoreFilterRepository(store);
                    storeFilterRepository.Save(true);

                    AdjustShipmentType();

                    // By default we auto-download every 15 minutes
                    store.AutoDownload = true;
                    store.AutoDownloadMinutes = 15;
                    store.AutoDownloadOnlyAway = false;

                    // Mark that this store is now ready
                    store.SetupComplete = true;
                    StoreManager.SaveStore(store, adapter);

                    adapter.Commit();
                }

                // Refresh the nudges, just in case there were any that shouldn't be displayed now due to the addition of this store.
                // Ask the store manager to check for changes so that it returns the store we just added.  The heart beat may
                // not have run to force the check yet.
                StoreManager.CheckForChanges();
                NudgeManager.Refresh(StoreManager.GetAllStores());

                // Ensure the license has the latest capabilities now that a new store
                // has been added.
                licenseService.GetLicense(store)?.ForceRefresh();
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

            ILicense license = licenseService.GetLicense(store);
            EnumResult<LicenseActivationState> activateResult = license.Activate(store);

            if (activateResult.Value != LicenseActivationState.Active)
            {
                e.Skip = true;

                if (license.IsLegacy)
                {
                    MessageHelper.ShowError(this, activateResult.Message);
                    e.SkipToPage = wizardPageAlreadyActive;
                }
                else
                {
                    if (activateResult.Value == LicenseActivationState.MaxChannelsExceeded)
                    {
                        IChannelLimitFactory factory = IoC.UnsafeGlobalLifetimeScope.Resolve<IChannelLimitFactory>();
                        Control channelLimitControl =
                            (Control) factory.CreateControl((ICustomerLicense) license, (StoreTypeCode) Store.TypeCode, EditionFeature.ChannelCount, this);

                        wizardPageActivationError.SetElementHost(channelLimitControl);
                    }

                    showActivationError = true;
                    wizardPageActivationError.ErrorMessage = activateResult.Message;
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
    }
}
