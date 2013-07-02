using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
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

        // The permissions existing users will get for this store
        PermissionSet permissions;
        long permissionsPlaceholderStoreID = -5;

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

        /// <summary>
        /// Run the wizard.  We do it this way so that we can control access to when the wizard dialog is created.  It's possible
        /// it may not be able to be created due to the wizard open on another computer.  Only one wizard per-computer can be open
        /// so that we can be sure that the store that is marked "IsSetupComplete" does not get wiped out from under a wizard.
        /// </summary>
        public static StoreEntity RunWizard(IWin32Window owner)
        {
            try
            {
                using (AddStoreWizardLock wizardLock = new AddStoreWizardLock())
                {
                    using (AddStoreWizard wizard = new AddStoreWizard())
                    {
                        if (wizard.ShowDialog(owner) == DialogResult.OK)
                        {
                            return wizard.Store;
                        }
                    }
                }
            }
            catch (SqlAppResourceLockException)
            {
                MessageHelper.ShowInformation(owner, "The Add Store Wizard is open on another computer running ShipWorks.  Only one Add Store Wizard can be open at a time.");
                return null;
            }

            return null;
        }

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
                comboStoreType.Items.Add(storeType);
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
        /// Indicates if the Wizard is in trial mode vs. entered an actual license.
        /// </summary>
        public bool TrialMode
        {
            get
            {
                return tryRadio.Checked;
            }
            set
            {
                tryRadio.Checked = value;
                licenseRadio.Checked = !value;
            }
        }

        /// <summary>
        /// Load the list of store types to choose from
        /// </summary>
        private void LoadStoreTypes()
        {
            comboStoreType.Items.Add("Choose...");

            // Add each store type as a radio
            foreach (StoreType storeType in StoreTypeManager.StoreTypes)
            {
                comboStoreType.Items.Add(storeType);
            }

            comboStoreType.SelectedIndex = 0;
        }

        /// <summary>
        /// User is choosing to do the trial
        /// </summary>
        private void OnChooseTryShipWorks(object sender, EventArgs e)
        {
            if (tryRadio.Checked)
            {
                NextEnabled = false;
                comboStoreType.Enabled = true;

                licenseKey.Enabled = false;
            }
        }

        /// <summary>
        /// User selected the radio to enter a license key.
        /// </summary>
        private void OnChooseEnterLicense(object sender, EventArgs e)
        {
            if (licenseRadio.Checked)
            {
                comboStoreType.SelectedIndex = 0;
                comboStoreType.Enabled = false;

                NextEnabled = true;

                licenseKey.Enabled = true;
            }
        }

        /// <summary>
        /// User has chosen a specific store type to try.
        /// </summary>
        private void OnChooseStoreType(object sender, EventArgs e)
        {
            NextEnabled = (SelectedStoreType != null);
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
                    return (StoreType) comboStoreType.SelectedItem;
                }

                return null;
            }
        }

        /// <summary>
        /// Entering the store type wizard page.
        /// </summary>
        private void OnSteppingIntoStoreType(object sender, WizardSteppingIntoEventArgs e)
        {
            NextEnabled = licenseRadio.Checked || SelectedStoreType != null;
        }

        /// <summary>
        /// Stepping next from the initial page.
        /// </summary>
        private void OnStepNextStoreType(object sender, WizardStepEventArgs e)
        {
            StoreType storeType = null;

            // Validate the license if they are entering a license
            if (licenseRadio.Checked)
            {
                ShipWorksLicense license = new ShipWorksLicense(licenseKey.Text.Trim());

                // Only go the the next page if the status is now Active
                // for this license.
                if (!license.IsValid)
                {
                    MessageHelper.ShowInformation(this, "The license entered is not a valid ShipWorks license.");

                    e.NextPage = CurrentPage;
                    return;
                }

                storeType = StoreTypeManager.GetType(license.StoreTypeCode);
            }
            else
            {
                storeType = SelectedStoreType;

                if (storeType == null)
                {
                    MessageHelper.ShowInformation(this, "Please select the online platform you use, or enter a license key.");

                    e.NextPage = CurrentPage;
                    return;
                }
            }

            // Setup for the selected\licensed storetype
            SetupForStoreType(storeType);

            // Apply the license
            if (licenseRadio.Checked)
            {
                store.License = licenseKey.Text.Trim();
            }
            else
            {
                store.License = "";
            }

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

            // Do an initial check on the name
            if (!StoreManager.CheckStoreName(store.StoreName, this))
            {
                e.NextPage = CurrentPage;
                return;
            }
        }

        #endregion

        #region Initial Download Page

        /// <summary>
        /// Stepping into the InitialDownload page
        /// </summary>
        private void OnSteppingIntoInitialDownloadPage(object sender, WizardSteppingIntoEventArgs e)
        {
            StoreType storeType = StoreTypeManager.GetType(Store);
            InitialDownloadPolicy policy = storeType.InitialDownloadPolicy;

            // Don't show initial download policy
            if (policy.RestrictionType == InitialDownloadRestrictionType.None)
            {
                e.Skip = true;
                e.RaiseStepEventWhenSkipping = true;

                return;
            }

            // Only reset the UI if the store has changed.  The only potential issue here is with generic where you could actually
            // be pointing to a totally different capabilities type without changing store types, but thats unlikely, and not catastrophic.
            if (initialDownloadConfiguredStoreID != store.StoreID)
            {
                initialDownloadConfiguredStoreID = store.StoreID;

                // Restrict by order number
                if (policy.RestrictionType == InitialDownloadRestrictionType.OrderNumber)
                {
                    panelDateRange.Visible = false;

                    panelFirstOrder.Visible = true;
                    panelFirstOrder.Top = panelDateRange.Top;

                    initialDownloadFirstOrder.Text = policy.DefaultStartingOrderNumber.ToString();
                    radioStartNumberLimit.Checked = true;
                }
                // Restrict by date range
                else
                {
                    panelDateRange.Visible = true;
                    panelFirstOrder.Visible = false;

                    dateRangeDays.Text = policy.DefaultDaysBack.ToString();
                    radioDateRangeDays.Checked = true;

                    dateRangeRadioHider.Visible = policy.MaxDaysBack != null;
                    radioDateRangeAll.Visible = policy.MaxDaysBack == null;
                }
            }
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
        /// Stepping next from the eBay initial download page
        /// </summary>
        private void OnStepNextInitialDownload(object sender, WizardStepEventArgs e)
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
                        e.NextPage = CurrentPage;
                        return;
                    }

                    long firstOrder;
                    if (!long.TryParse(initialDownloadFirstOrder.Text.Trim(), out firstOrder))
                    {
                        MessageHelper.ShowInformation(this, "Enter a valid order number that ShipWorks should start downloading from.");
                        e.NextPage = CurrentPage;
                        return;
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
                        e.NextPage = CurrentPage;
                        return;
                    }

                    int daysBack;
                    if (!int.TryParse(dateRangeDays.Text.Trim(), out daysBack))
                    {
                        MessageHelper.ShowInformation(this, "Enter a valid number of days.");
                        e.NextPage = CurrentPage;
                        return;
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

                        e.NextPage = CurrentPage;
                        return;
                    }

                    store.InitialDownloadDays = daysBack;
                }
            }
        }

        #endregion

        #region Online Update Page

        /// <summary>
        /// Stepping into the online update page
        /// </summary>
        private void OnSteppingIntoOnlineUpdate(object sender, WizardSteppingIntoEventArgs e)
        {
            // Ensure no actions are left over from this store, b\c we'll be recreating them when we move next
            ActionManager.DeleteStoreActions(store.StoreID);

            if (e.StepReason == WizardStepReason.StepBack)
            {
                return;
            }

            StoreType storeType = StoreTypeManager.GetType(store);

            OnlineUpdateActionControlBase onlineUpdateControl = storeType.CreateAddStoreWizardOnlineUpdateActionControl();
            if (onlineUpdateControl == null)
            {
                // Clear out any previous control they may have had
                panelOnlineUpdatePlaceholder.Controls.Clear();
                onlineUpdateConfiguredStoreID = 0;

                e.Skip = true;
                return;
            }

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
        }

        /// <summary>
        /// Stepping next from the online update page
        /// </summary>
        private void OnStepNextOnlineUpdate(object sender, WizardStepEventArgs e)
        {
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
                    action.ComputerLimitedType = (int) ComputerLimitationType.TriggeringComputer;
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

        #region Settings Page

        /// <summary>
        /// Stepping into the settings page
        /// </summary>
        private void OnSteppingIntoSettings(object sender, WizardSteppingIntoEventArgs e)
        {
            // Load the email settings
            EmailUtility.LoadEmailAccounts(emailAccountDefault);

            // Load the download settings
            automaticDownloadControl.LoadStore(store);

            // Don't need to mess with user-rights at all if there are no standard users
            panelUserRights.Visible = UserManager.GetUsers(false).Any(u => !u.IsAdmin);

            if (permissions == null)
            {
                // Use a placeholder StoreID since we don't have a real one yet
                permissions = UserUtility.CreateDefaultStorePermissionSet(permissionsPlaceholderStoreID);
            }
        }

        /// <summary>
        /// Stepping next out of the settings page
        /// </summary>
        private void OnStepNextSettings(object sender, WizardStepEventArgs e)
        {
            automaticDownloadControl.SaveToStoreEntity(store);

            store.DefaultEmailAccountID = (emailAccountDefault.Items.Count > 0) ?
                (long) emailAccountDefault.SelectedValue :
                -1;

            ActivateAndFinish(e);
        }

        /// <summary>
        /// Open the window for managing email accounts for the store
        /// </summary>
        private void OnEmailAccounts(object sender, EventArgs e)
        {
            using (EmailAccountManagerDlg dlg = new EmailAccountManagerDlg())
            {
                dlg.ShowDialog(this);

                EmailUtility.LoadEmailAccounts(emailAccountDefault);
            }
        }

        /// <summary>
        /// Open the window for editing the default user rights
        /// </summary>
        private void OnEditUserRights(object sender, EventArgs e)
        {
            // Make a copy of the permissions to give to the dialog
            PermissionSet copy = new PermissionSet(permissions);

            using (AddStorePermissionsDlg dlg = new AddStorePermissionsDlg(copy, permissionsPlaceholderStoreID))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    permissions = copy;
                }
            }
        }

        /// <summary>
        /// Activate and step into the appropriate account page
        /// </summary>
        private void ActivateAndFinish(WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Default is to move to the finished page
            e.NextPage = wizardPageFinished;

            // If the selected to enter a key, use that
            if (licenseRadio.Checked)
            {
                LicenseActivationState licenseState = LicenseActivationHelper.ActivateAndSetLicense(store, store.License, this);

                if (licenseState != LicenseActivationState.Active)
                {
                    e.NextPage = CurrentPage;
                    return;
                }
                else
                {
                    if (EditionSerializer.Restore(store) is FreemiumFreeEdition && !isFreemiumMode)
                    {
                        MessageHelper.ShowError(this, "The license you entered can only be used with the Endicia Free for eBay ShipWorks edition.");

                        e.NextPage = CurrentPage;
                        return;
                    }
                }
            }
            else
            {
                try
                {
                    // Create a license for the given store
                    trialDetail = TangoWebClient.GetTrial(store);

                    // Save the license
                    store.License = trialDetail.License.Key;
                    store.Edition = EditionSerializer.Serialize(trialDetail.Edition);

                    // Already an active license available
                    if (trialDetail.IsConverted)
                    {
                        e.NextPage = wizardPageTrialConverted;
                    }

                    // If its expired
                    else if (trialDetail.IsExpired)
                    {
                        e.NextPage = wizardPageTrialExpired;
                    }

                    // Can't attach to a freemium trial if not freemium mode
                    else if (trialDetail.Edition is FreemiumFreeEdition && !isFreemiumMode)
                    {
                        MessageHelper.ShowError(this, string.Format("You are already using ShipWorks with eBay ID '{0}' with the Endicia Free for eBay version.", StoreTypeManager.GetType(store).LicenseIdentifier));

                        e.NextPage = CurrentPage;
                    }

                    // If it wasnt started today, we consider it in progress. Also special case for freemium - which creates a trial initially, but the user shouldn't know what.
                    else if (trialDetail.Started != trialDetail.ServerDate && !isFreemiumMode)
                    {
                        e.NextPage = wizardPageTrialInProgress;
                    }
                }
                catch (Exception ex)
                {
                    if (ex is ShipWorksLicenseException || ex is TangoException)
                    {
                        MessageHelper.ShowError(this, ex.Message);

                        e.NextPage = CurrentPage;
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        #endregion

        #region Trial In Progress

        /// <summary>
        /// Stepping into the trial in progress page
        /// </summary>
        private void OnSteppingIntoTrialInProgress(object sender, WizardSteppingIntoEventArgs e)
        {
            string display = "The trial period for your store has already begun.  There are now {0} days left in your trial.";
            labelTrialStarted.Text = string.Format(display, trialDetail.DaysRemaining);
        }

        /// <summary>
        /// Stepping next out of the trial in progress page
        /// </summary>
        private void OnStepNextTrialInProgress(object sender, WizardStepEventArgs e)
        {
            e.NextPage = wizardPageFinished;
        }

        #endregion

        #region Trial Expired

        /// <summary>
        /// The trial expired page is being displayed.
        /// </summary>
        private void OnSteppingIntoTrialExpired(object sender, WizardSteppingIntoEventArgs e)
        {
            panelCanExtend.Visible = trialDetail.CanExtend;
            panelNoExtend.Visible = !trialDetail.CanExtend;
        }

        /// <summary>
        /// Stepping next from the "Expired" screen.
        /// </summary>
        private void OnStepNextTrialExpired(object sender, WizardStepEventArgs e)
        {
            e.NextPage = wizardPageFinished;
        }

        /// <summary>
        /// Open the link to signup for trial services.
        /// </summary>
        private void OnTrialSignup(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://www.interapptive.com/store/?store=" + StoreTypeManager.GetType(store).TangoCode, this);
        }

        /// <summary>
        /// Open the link to signup
        /// </summary>
        private void OnTrialExpiredSignup(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("https://www.interapptive.com/store/?store=" + StoreTypeManager.GetType(store).TangoCode, this);
        }

        /// <summary>
        /// User wants to extend their trial.
        /// </summary>
        private void OnTrialExtend(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                TangoWebClient.ExtendTrial(store);

                MessageHelper.ShowMessage(this, "Your trial has been extended.");

                // Simulate next click
                MoveNext();
            }
            catch (ShipWorksLicenseException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
            catch (TangoException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
            }
        }

        #endregion

        #region Trial Converted

        /// <summary>
        /// Stepping next from the trial convertd page
        /// </summary>
        private void OnStepNextTrialConverted(object sender, WizardStepEventArgs e)
        {
            LicenseActivationState licenseState = LicenseActivationHelper.ActivateAndSetLicense(store, trialLicense.Text.Trim(), this);

            if (licenseState != LicenseActivationState.Active)
            {
                e.NextPage = CurrentPage;
                return;
            }
            else
            {
                if (EditionSerializer.Restore(store) is FreemiumFreeEdition && !isFreemiumMode)
                {
                    MessageHelper.ShowError(this, "The license you entered can only be used with the Endicia Free for eBay ShipWorks edition.");

                    e.NextPage = CurrentPage;
                    return;
                }
            }

            // Done
            e.NextPage = wizardPageFinished;
        }

        #endregion

        #region Complete

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

                    // Now that we have a StoreID, we have to update our permission set to use
                    // it instead of the fake zero one
                    foreach (PermissionEntity permission in permissions)
                    {
                        permission.ObjectID = store.StoreID;
                    }

                    // Apply permissions to existing users
                    foreach (UserEntity user in UserManager.GetUsers(false))
                    {
                        permissions.CopyTo(user.UserID, false, adapter);
                    }

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
