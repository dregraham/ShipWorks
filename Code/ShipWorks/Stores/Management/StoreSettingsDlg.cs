using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window for managing the settings of a single store
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class StoreSettingsDlg : Form
    {
        StoreEntity store;
        StoreType storeType;

        bool licenseTabInitialized = false;

        // License info
        ShipWorksLicense license;
        TrialDetail trialDetail;
        ILicenseAccountDetail accountDetail;

        // Download policy
        ComputerDownloadPolicy downloadPolicy;

        // The account settings control
        AccountSettingsControlBase accountControl;

        // The control for manual order settings
        ManualOrderSettingsControl manualOrderSettingsControl;

        // the control for store-specific settings
        StoreSettingsControlBase storeSettingsControl;
        private bool resetPendingValidations;

        public enum Section
        {
            OnlineAccount,
            StoreDetails,
            StatusPresets
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreSettingsDlg(StoreEntity store)
            : this(store, Section.StoreDetails)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreSettingsDlg(StoreEntity store, Section initialSection)
        {
            InitializeComponent();

            WindowStateSaver.Manage(this, WindowStateSaverOptions.Size);

            this.store = store;
            this.storeType = StoreTypeManager.GetType(store);

            switch (initialSection)
            {
                case Section.OnlineAccount: optionControl.SelectedPage = optionPageOnlineAccount; break;
                case Section.StoreDetails: optionControl.SelectedPage = optionPageStoreDetails; break;
                case Section.StatusPresets: optionControl.SelectedPage = optionPageStatusPreset; break;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageStores);

            // Download policy
            downloadPolicy = ComputerDownloadPolicy.Load(store);

            // Load the download policy choices and start listening for changes
            comboAllowDownload.LoadChoices(downloadPolicy.DefaultToYes);
            comboAllowDownload.SelectedIndex = -1;
            comboAllowDownload.SelectedIndexChanged += OnChangeAllowDownloading;

            // Load title
            titleStoreName.Text = store.StoreName;
            titleStoreType.Text = storeType.StoreTypeName;

            // Load general \ contact tab
            storeAddressControl.LoadStore(store);
            storeContactControl.LoadStore(store);

            // Store Settings
            LoadSettingsTab();

            // Load account tab
            accountControl = storeType.CreateAccountSettingsControl();
            accountControl.Dock = DockStyle.Fill;
            accountControl.LoadStore(store);
            optionPageOnlineAccount.Controls.Add(accountControl);

            // Load status preset tab
            orderStatusPresets.LoadPresets(store, StatusPresetTarget.Order);
            itemStatusPresets.LoadPresets(store, StatusPresetTarget.OrderItem);

            // If the store is not enabled, we disable\hide certain settings
            UpdateStoreEnabledDisplay();

            // Check whether or not to display license tab
            CheckLicense(store);
        }

        /// <summary>
        /// Checks the license to determine whether or not to display the license tab.
        /// </summary>
        /// <remarks>
        /// If store license (legacy customer), display it.
        /// If customer license (new customer), don't display it.
        /// </remarks>
        /// <param name="storeEntity">The store entity.</param>
        private void CheckLicense(StoreEntity storeEntity)
        {
            ILicenseService licenseService = IoC.UnsafeGlobalLifetimeScope.Resolve<ILicenseService>();
            ILicense license = licenseService.GetLicense(storeEntity);

            // If legacy customer or magic keys down, display license
            if (!license.IsLegacy && !InterapptiveOnly.MagicKeysDown)
            {
                optionControl.Controls.Remove(optionPageLicense);
            }
        }

        /// <summary>
        /// Update the enabled\disabled display of the store
        /// </summary>
        private void UpdateStoreEnabledDisplay()
        {
            if (!storeDisabled.Checked && !optionPageOnlineAccount.Enabled)
            {
                optionPageOnlineAccount.Enabled = true;

                licenseTabInitialized = false;
            }

            if (storeDisabled.Checked && optionPageOnlineAccount.Enabled)
            {
                optionPageOnlineAccount.Enabled = false;

                licenseTabInitialized = false;
            }
        }

        /// <summary>
        /// A page is being deselected
        /// </summary>
        private void OnPageDeselecting(object sender, OptionControlCancelEventArgs e)
        {
            if (e.OptionPage == optionPageOnlineAccount)
            {
                Cursor.Current = Cursors.WaitCursor;

                // The Store Settings tab may depend on what's currently in account settings, so we have to save it right away
                if (!accountControl.SaveToEntity(store))
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// The current page has changed
        /// </summary>
        private void OnPageSelected(object sender, OptionControlEventArgs e)
        {
            // Delay load license tab
            if (optionControl.SelectedPage == optionPageLicense && !licenseTabInitialized)
            {
                licenseTabInitialized = true;
                LoadLicenseTab();
            }
        }

        /// <summary>
        /// Change whether downloading is allowed from this computer
        /// </summary>
        private void OnChangeAllowDownloading(object sender, EventArgs e)
        {
            downloadPolicy.SetComputerAllowed(UserSession.Computer.ComputerID, (ComputerDownloadAllowed) comboAllowDownload.SelectedValue);

            automaticDownloadControl.Enabled = downloadPolicy.IsThisComputerAllowed;
        }

        /// <summary>
        /// Open the window to change the download policy for all computers
        /// </summary>
        private void OnConfigureDownloadPolicy(object sender, EventArgs e)
        {
            using (ComputerDownloadPolicyDlg dlg = new ComputerDownloadPolicyDlg(downloadPolicy, store.StoreName))
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    comboAllowDownload.LoadChoices(downloadPolicy.DefaultToYes);
                    comboAllowDownload.SelectedValue = downloadPolicy.GetComputerAllowed(UserSession.Computer.ComputerID);

                    // If the default changed, and the default is selected, the UI needs updated
                    OnChangeAllowDownloading(null, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Changing the enabled\disabled state of the store
        /// </summary>
        private void OnChangeEnabledState(object sender, EventArgs e)
        {
            UpdateStoreEnabledDisplay();
        }

        /// <summary>
        /// Load the content of the settings tab
        /// </summary>
        private void LoadSettingsTab()
        {
            EnumHelper.BindComboBox<AddressValidationStoreSettingType>(addressValidationSetting);

            manualOrderSettingsControl = storeType.CreateManualOrderSettingsControl();
            manualOrderSettingsControl.Location = new Point(23, sectionTitleManualOrders.Bottom + 8);
            manualOrderSettingsControl.Width = optionPageSettings.Width - manualOrderSettingsControl.Location.X - 10;
            manualOrderSettingsControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            optionPageSettings.Controls.Add(manualOrderSettingsControl);

            // store-specific settings control
            storeSettingsControl = storeType.CreateStoreSettingsControl();
            if (storeSettingsControl != null)
            {
                // Settings control gets location and width of the section title, so that each settings control can simply
                // set its titles to go all the way across with anchors.
                storeSettingsControl.Location = new Point(sectionTitleManualOrders.Left, manualOrderSettingsControl.Bottom + 8);
                storeSettingsControl.Width = sectionTitleManualOrders.Width; ;
                storeSettingsControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                optionPageSettings.Controls.Add(storeSettingsControl);

                UpdateControlPositionBelowControl(storeSettingsControl);

                storeSettingsControl.SizeChanged += (sender, args) => UpdateControlPositionBelowControl(storeSettingsControl);
            }
            else
            {
                UpdateControlPositionBelowControl(manualOrderSettingsControl);
            }

            panelAddressValidation.Top = panelStoreStatus.Bottom + 8;
            addressValidationSetting.SelectedValue = (AddressValidationStoreSettingType) store.AddressValidationSetting;

            panelDefaultFilters.Top = panelAddressValidation.Bottom + 8;

            // Download on\off
            comboAllowDownload.SelectedValue = downloadPolicy.GetComputerAllowed(UserSession.Computer.ComputerID);

            // Auto download
            automaticDownloadControl.LoadStore(store);

            // Manual orders
            manualOrderSettingsControl.LoadStore(store);

            // Store settings if present
            if (storeSettingsControl != null)
            {
                storeSettingsControl.LoadStore(store);
            }

            // Store status
            storeDisabled.Checked = !store.Enabled;
        }

        /// <summary>
        /// Update the location of controls below the specified control
        /// </summary>
        private void UpdateControlPositionBelowControl(Control control)
        {
            panelStoreStatus.Top = control.Bottom + 8;
            panelAddressValidation.Top = panelStoreStatus.Bottom + 8;
            panelDefaultFilters.Top = panelAddressValidation.Bottom + 8;
        }

        /// <summary>
        /// Save the values from the store settings tab to the entity
        /// </summary>
        private bool SaveSettingsTab()
        {
            store.ComputerDownloadPolicy = downloadPolicy.SerializeToXml();

            automaticDownloadControl.SaveToStoreEntity(store);
            manualOrderSettingsControl.SaveToEntity(store);

            bool result = true;

            if (storeSettingsControl != null)
            {
                result = storeSettingsControl.SaveToEntity(store);
            }

            // Check whether we should reset any pending address validations
            AddressValidationStoreSettingType currentSetting = (AddressValidationStoreSettingType) store.AddressValidationSetting;
            AddressValidationStoreSettingType newSetting = (AddressValidationStoreSettingType) addressValidationSetting.SelectedValue;

            resetPendingValidations = (currentSetting == AddressValidationStoreSettingType.ValidateAndApply ||
                                       currentSetting == AddressValidationStoreSettingType.ValidateAndNotify)
                                      && (newSetting == AddressValidationStoreSettingType.ManualValidationOnly ||
                                          newSetting == AddressValidationStoreSettingType.ValidationDisabled);

            store.AddressValidationSetting = (int) addressValidationSetting.SelectedValue;

            return result;
        }

        /// <summary>
        /// Load the data to display in the license tab
        /// </summary>
        private void LoadLicenseTab()
        {
            Refresh();
            Cursor.Current = Cursors.WaitCursor;

            // Create the license instance
            license = new ShipWorksLicense(store.License);
            trialDetail = null;
            accountDetail = null;

            try
            {
                // May be a trial
                if (license.IsTrial)
                {
                    licenseKey.Text = string.Format("Trial ({0})", store.License.Substring(0, store.License.Replace("-TRIAL", "").LastIndexOf('-')));

                    if (!storeDisabled.Checked)
                    {
                        trialDetail = TangoWebClient.GetTrial(store);
                        licenseStatus.Text = trialDetail.Description;

                        changeLicense.Text = "Enter License...";
                    }
                }
                else
                {
                    licenseKey.Text = license.Key;

                    if (!storeDisabled.Checked)
                    {
                        accountDetail = new TangoWebClientFactory().CreateWebClient().GetLicenseStatus(license.Key, store);
                        licenseStatus.Text = accountDetail.Description;

                        UpdateChangeLicenseText();
                    }
                }

                if (storeDisabled.Checked)
                {
                    changeLicense.Visible = false;
                    licenseStatus.Text = "The store has been disabled for downloading and shipping on the 'Store Settings' tab.";
                }
                else
                {
                    // Freemium special case
                    if (EditionSerializer.Restore(store) is FreemiumFreeEdition)
                    {
                        licenseStatus.Text = "Free for eBay";
                        changeLicense.Visible = false;
                    }
                    else
                    {
                        changeLicense.Visible = true;
                    }
                }
            }
            catch (TangoException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                licenseStatus.Text = "Error";
                changeLicense.Visible = false;
            }
        }

        /// <summary>
        /// Update the change license text
        /// </summary>
        private void UpdateChangeLicenseText()
        {
            if (accountDetail.ActivationState == LicenseActivationState.ActiveElsewhere)
            {
                changeLicense.Text = "Change Activation...";
            }
            else if (accountDetail.ActivationState == LicenseActivationState.ActiveNowhere)
            {
                changeLicense.Text = "Activate License...";
            }
            else
            {
                changeLicense.Text = "Change License...";
            }
        }

        /// <summary>
        /// Change \ enter \ activate a license
        /// </summary>
        private void OnChangeLicense(object sender, EventArgs e)
        {
            if (!license.IsTrial && accountDetail.ActivationState == LicenseActivationState.ActiveElsewhere)
            {
                using (ChangeActivationDlg dlg = new ChangeActivationDlg(store))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadLicenseTab();
                    }
                }
            }

            else if (!license.IsTrial && accountDetail.ActivationState == LicenseActivationState.ActiveNowhere)
            {
                if (LicenseActivationHelper.ActivateAndSetLicense(store, store.License, this) != LicenseActivationState.ActiveNowhere)
                {
                    LoadLicenseTab();
                }
            }

            else
            {
                using (ChangeLicenseDlg dlg = new ChangeLicenseDlg(store))
                {
                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadLicenseTab();
                    }
                }
            }
        }

        /// <summary>
        /// Save changes
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnOK(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            // Update enabled state.  Has to come now and not in the 'SaveSettings' section b\c some of the other save code looks at the value
            store.Enabled = !storeDisabled.Checked;

            storeContactControl.SaveToEntity(store);
            storeAddressControl.SaveToEntity(store);

            // Do an initial check of the store name
            if (!StoreManager.CheckStoreName(store.StoreName, store, this))
            {
                optionControl.SelectedPage = optionPageStoreDetails;
                return;
            }

            // Online Settings and Store Settings don't get applied for disabled stores
            if (store.Enabled)
            {
                if (!accountControl.SaveToEntity(store))
                {
                    optionControl.SelectedPage = optionPageOnlineAccount;
                    return;
                }

                // Save the settings tab to the store entity
                if (!SaveSettingsTab())
                {
                    optionControl.SelectedPage = optionPageSettings;
                    return;
                }
            }

            try
            {
                bool wasStoreDirty = store.IsDirty;

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    orderStatusPresets.Save(adapter);
                    itemStatusPresets.Save(adapter);

                    StoreManager.SaveStore(store, adapter);

                    adapter.Commit();
                }

                StoreManager.CheckForChanges();

                if (wasStoreDirty)
                {
                    // Let any subscribers know that the store has changed.
                    Messenger.Current.Send(new StoreChangedMessage(this, store));
                }

                // If the user has just disabled address validation, we should mark any pending orders
                // as not checked.  This is being done in its own transaction so that it doesn't affect
                // saving the changes to the store if the update fails.
                if (resetPendingValidations)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        OrderEntity orderUpdate = new OrderEntity
                        {
                            ShipAddressValidationStatus = (int) AddressValidationStatusType.NotChecked
                        };

                        var pendingOrderBucket = new RelationPredicateBucket();
                        pendingOrderBucket.PredicateExpression.Add(OrderFields.StoreID == store.StoreID);
                        pendingOrderBucket.PredicateExpression.AddWithAnd(OrderFields.ShipAddressValidationStatus == (int) AddressValidationStatusType.Pending);

                        adapter.UpdateEntitiesDirectly(orderUpdate, pendingOrderBucket);
                    }
                }

                // Have to make sure our in-memory list of presets stays current.  The in-memory Stores list does not get updated at this point -
                // that happens only in the MainForm
                StatusPresetManager.CheckForChanges();

                DialogResult = DialogResult.OK;
            }
            catch (DuplicateNameException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                optionControl.SelectedPage = optionPageStoreDetails;
                return;
            }
        }

        /// <summary>
        /// The window has closed
        /// </summary>
        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            // If the operation was canceled, rollback
            if (DialogResult != DialogResult.OK)
            {
                store.RollbackChanges();
            }
        }

        /// <summary>
        /// Called when the createFiltersButton is clicked.
        /// </summary>
        private void OnCreateFiltersClick(object sender, EventArgs e)
        {
            StoreManager.CreateStoreStatusFilters(this, store);
        }
    }
}