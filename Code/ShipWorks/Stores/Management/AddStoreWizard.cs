using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.AddressValidation;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.Licensing;
using Interapptive.Shared;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.UI;
using Interapptive.Shared.Net;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Platforms;
using ShipWorks.Templates.Emailing;
using ShipWorks.Email;
using ShipWorks.Email.Accounts;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using ShipWorks.Common.Threading;
using System.Threading;
using Interapptive.Shared.Utility;
using Interapptive.Shared.UI;
using ShipWorks.Data.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.Filters;
using ShipWorks.Filters.Content;
using ShipWorks.Filters.Content.Conditions.Orders;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping;
using ShipWorks.Editions.Brown;
using ShipWorks.ApplicationCore.Setup;
using ShipWorks.UI.Controls;
using ShipWorks.Stores.Communication;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Wizard for adding a new store to ShipWorks
    /// </summary>
    partial class AddStoreWizard : WizardForm
    {
        // State container for use by wizard pages
        Dictionary<string, object> stateBag = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

        // The store that is being creatd
        StoreEntity store;

        // All of the store specifc wizard pages currently added.
        List<WizardPage> storePages = new List<WizardPage>();

        // If doing a trial, this is the current trial data
        TrialDetail trialDetail;

        // So we know when to update the config of the pages
        long initialDownloadConfiguredStoreID = 0;
        long onlineUpdateConfiguredStoreID = 0;

        // Indicates if we are in a freemium setup flow
        bool isFreemiumMode = false;

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
                MessageHelper.ShowInformation(owner, "An administrator must log on to setup to ShipWorks.");
                return false;
            }

            try
            {
                using (ShipWorksSetupLock wizardLock = new ShipWorksSetupLock())
                {
                    using (AddStoreWizard wizard = new AddStoreWizard())
                    {
                        // If it was succesful, make sure our local list of stores is refreshed
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
                MessageHelper.ShowInformation(owner, "Another user is already setting up ShipWorks.  This can only be done on one computer at a time.");
                return false;
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

            // Logon the user
            UserSession.Logon(username, password, true);

            // Initialize the session
            UserManager.InitializeForCurrentUser();
            UserSession.InitializeForCurrentSession();

            originalWizard.BeginInvoke(new MethodInvoker(originalWizard.Hide));

            // Run the setup wizard
            bool complete = RunWizard(originalWizard);

            // If the wizard didn't complete, then the we can't exit this with the user still looking like they were logged in
            if (!complete)
            {
                UserSession.Logoff(false);
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
            
            EnumHelper.BindComboBox<AddressValidationStoreSettingType>(addressValidationSetting);

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
                    DeletionService.DeleteStore(store);
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
                // If w are in freemium mode, and we already have a valid freemium edition store, then don't try to get a trial - it would fail anyway saying they aren't eligible
                if (isFreemiumMode && (EditionSerializer.Restore(store) is FreemiumFreeEdition) && new ShipWorksLicense(store.License).IsValid)
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
            LicenseActivationState licenseState = LicenseActivationHelper.ActivateAndSetLicense(store, licenseKey.Text.Trim(), this);

            if (licenseState != LicenseActivationState.Active)
            {
                e.NextPage = CurrentPage;
                return;
            }
            else
            {
                if (EditionSerializer.Restore(store) is FreemiumFreeEdition && StoreManager.GetDatabaseStoreCount() > 0)
                {
                    MessageHelper.ShowError(this, "The license you entered is for the Endicia Free for eBay ShipWorks edition.  That edition only supports a single store, and you already have some stores in ShipWorks.");

                    e.NextPage = CurrentPage;
                    return;
                }
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
            bool downloadSettings = PrepareSettingsInitialDownloadUI(e);
            bool uploadSettings = PrepareSettingsActionUI(e);

            addressValidationSetting.SelectedValue = (AddressValidationStoreSettingType)store.AddressValidationSetting;

            // Update the location of the autoAddressValidation control based on which upload and download controls are shown
            if (!uploadSettings && !downloadSettings)
            {
                addressValidationSetting.Top = panelDownloadSettings.Top + 12;
                labelAddressValidationSetting.Top = panelDownloadSettings.Top + 15;
            }
            else if (!uploadSettings)
            {
                addressValidationSetting.Top = panelDownloadSettings.Bottom + 12;
                labelAddressValidationSetting.Top = panelDownloadSettings.Bottom + 15;
            }
            else
            {
                addressValidationSetting.Top = panelUploadSettings.Bottom + 12;
                labelAddressValidationSetting.Top = panelUploadSettings.Bottom + 15;
            }

            // We can't skip this screen anymore since all stores will have the option of auto validating addresses
            e.Skip = false;
            e.RaiseStepEventWhenSkipping = false;
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

            addressValidationSetting.Top += panelEditDownloadRange.Bottom - panelViewDownloadRange.Bottom;
            labelAddressValidationSetting.Top += panelEditDownloadRange.Bottom - panelViewDownloadRange.Bottom;
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

            SaveSettingsActions();

            store.AddressValidationSetting = (int) addressValidationSetting.SelectedValue;
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
        private void SaveSettingsActions()
        {
            // If this store does not support uploads, the online update placeholder will have no controls so just return.
            if (panelOnlineUpdatePlaceholder.Controls.Count == 0)
            {
                return;
            }

            OnlineUpdateActionControlBase control = (OnlineUpdateActionControlBase) panelOnlineUpdatePlaceholder.Controls[0];

            // See what tasks are configured to be created
            List<ActionTask> tasks = control.CreateActionTasks(store);

            // If there are any, we need to create the action for ti
            if (tasks != null && tasks.Count > 0)
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Setup the basic action
                    ActionEntity action = new ActionEntity();
                    action.Name = "Online Update";
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
            int storeCount = StoreManager.GetAllStores().Count;

            try
            {
                // Make sure we have a fresh up-to-date layout context in case we need to create store-specific filters
                FilterLayoutContext.PushScope();

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Create the default presets
                    CreateDefaultStatusPreset(store, StatusPresetTarget.Order, adapter);
                    CreateDefaultStatusPreset(store, StatusPresetTarget.OrderItem, adapter);

                    // See if the store has needs to create any store-specific filters
                    List<FilterEntity> storeFilters = StoreTypeManager.GetType(store).CreateInitialFilters();
                    if (storeFilters != null && storeFilters.Count > 0)
                    {
                        // Find the root orders node
                        FilterNodeEntity ordersNode = FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));

                        // Create the child stores node
                        FilterNodeEntity storeNode = FilterLayoutContext.Current.AddFilter(CreateStoreFilterFolder(), ordersNode, ordersNode.ChildNodes.Count)[0];

                        // Always create an "All Orders" filter so that the folder count show's the full store orders's count.  Otherwise our users would
                        // likely be confused.
                        FilterLayoutContext.Current.AddFilter(CreateStoreFilterAllOrders(), storeNode, 0);

                        // Create each filter under the store node
                        for (int i = 0; i < storeFilters.Count; i++)
                        {
                            FilterLayoutContext.Current.AddFilter(storeFilters[i], storeNode, i + 1);
                        }
                    }

                    // Adjust the default shipment type based on edition
                    if (storeCount == 0)
                    {
                        ShipmentTypeCode? defaultType = EditionSerializer.Restore(store).DefaultShipmentType;

                        if (defaultType != null)
                        {
                            ShippingSettingsEntity shippingSettings = ShippingSettings.Fetch();
                            shippingSettings.DefaultType = (int) defaultType.Value;

                            ShippingSettings.Save(shippingSettings);
                        }
                    }

                    // By default we auto-download every 15 minutes
                    store.AutoDownload = true;
                    store.AutoDownloadMinutes = 15;
                    store.AutoDownloadOnlyAway = false;

                    // Mark that this store is now ready
                    store.SetupComplete = true;
                    StoreManager.SaveStore(store, adapter);

                    adapter.Commit();
                }
            }
            catch (DuplicateNameException ex)
            {
                MessageHelper.ShowInformation(this, ex.Message);

                e.Skip = true;
                e.SkipToPage = wizardPageContactInfo;

                return;
            }
            finally
            {
                FilterLayoutContext.PopScope();
            }
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
        /// Create a filter folder entity that has a definition that filters for orders only in the current store
        /// </summary>
        private FilterEntity CreateStoreFilterFolder()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.Conditions.Add(new StoreCondition { Operator = EqualityOperator.Equals, Value = store.StoreID });

            FilterEntity folder = new FilterEntity();
            folder.Name = string.Format("{0} ({1})", StoreTypeManager.GetType(store).StoreTypeName, store.StoreName);
            folder.FilterTarget = (int) FilterTarget.Orders;
            folder.IsFolder = true;
            folder.Definition = definition.GetXml();

            return folder;
        }

        /// <summary>
        /// Create a filter that filter's for all orders of this store
        /// </summary>
        private FilterEntity CreateStoreFilterAllOrders()
        {
            FilterDefinition definition = new FilterDefinition(FilterTarget.Orders);
            definition.RootContainer.FirstGroup.Conditions.Add(new StoreCondition { Operator = EqualityOperator.Equals, Value = store.StoreID });

            FilterEntity filter = new FilterEntity();
            filter.Name = "All Orders";
            filter.FilterTarget = (int) FilterTarget.Orders;
            filter.IsFolder = false;
            filter.Definition = definition.GetXml();

            return filter;
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
                    DeletionService.DeleteStore(store);
                    store = null;
                }
            }
        }

        #endregion

    }
}
