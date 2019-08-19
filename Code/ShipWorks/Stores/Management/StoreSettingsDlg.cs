using System;
using System.Data;
using System.Drawing;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Enums;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Messaging.Messages;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Window for managing the settings of a single store
    /// </summary>
    public partial class StoreSettingsDlg : Form
    {
        private StoreEntity store;
        private StoreType storeType;

        private bool licenseTabInitialized = false;

        // License info
        private ShipWorksLicense license;
        private TrialDetail trialDetail;
        private ILicenseAccountDetail accountDetail;

        // The account settings control
        private AccountSettingsControlBase accountControl;

        // The control for manual order settings
        private ManualOrderSettingsControl manualOrderSettingsControl;
        private IDownloadSettingsControl downloadSettingsControl;

        // the control for store-specific settings
        private StoreSettingsControlBase storeSettingsControl;
        private bool resetPendingValidations;
        private const int VerticalSpaceBetweenSections = 10;

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

            optionControl.DeselectingAsync = OnPageDeselectingAsync;
            WindowStateSaver.Manage(this, WindowStateSaverOptions.Size);

            this.store = store;
            this.storeType = StoreTypeManager.GetType(store);

            switch (initialSection)
            {
                case Section.OnlineAccount:
                    optionControl.SelectedPage = optionPageOnlineAccount;
                    break;
                case Section.StoreDetails:
                    optionControl.SelectedPage = optionPageStoreDetails;
                    break;
                case Section.StatusPresets:
                    optionControl.SelectedPage = optionPageStatusPreset;
                    break;
            }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            UserSession.Security.DemandPermission(PermissionType.ManageStores);

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
            ILicense storeLicense = licenseService.GetLicense(storeEntity);

            // If legacy customer or magic keys down, display license
            if (!storeLicense.IsLegacy && !InterapptiveOnly.MagicKeysDown)
            {
                optionControl.Controls.Remove(optionPageLicense);
            }
        }

        /// <summary>
        /// Checks the store to determine whether or not to display the store connection tab
        /// </summary>
        public void StoreHasConnectionString(StoreEntity storeEntity)
        {
            if (storeEntity.StoreTypeCode == StoreTypeCode.Manual)
            {
                // Initialize the download section but disable it
                // Manual Store does not allow downloading
                downloadSettingsControl = storeType.CreateDownloadSettingsControl();
                downloadSettingsControl.LoadStore(store);
                downloadSettingsControl.Location = new Point(17, sectionAutoDownloads.Bottom + VerticalSpaceBetweenSections);
                downloadSettingsControl.Width = optionPageSettings.Width - downloadSettingsControl.Location.X - 10;
                downloadSettingsControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                panelDownloading.Controls.Add(downloadSettingsControl as Control);
                panelDownloading.Enabled = false;

                // Initialize and display the manual orders section
                manualOrderSettingsControl = storeType.CreateManualOrderSettingsControl();
                manualOrderSettingsControl.Location = new Point(19, sectionTitleManualOrders.Bottom + VerticalSpaceBetweenSections);
                manualOrderSettingsControl.Width = optionPageSettings.Width - manualOrderSettingsControl.Location.X - 10;
                manualOrderSettingsControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                panelManualOrders.Controls.Add(manualOrderSettingsControl);
            }
            else
            {
                ConfigureDownloadSettingsControl();
                ConfigureManualOrderSettingsControl();
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
        private async Task<bool> OnPageDeselectingAsync(OptionPage optionPage, int index)
        {
            if (optionPage != optionPageOnlineAccount)
            {
                return false;
            }

            using (BlockUI())
            {
                // The Store Settings tab may depend on what's currently in account settings, so we have to save it right away
                return !await SaveAccountControl().ConfigureAwait(true);
            }
        }

        /// <summary>
        /// Block UI editing while an operation is taking place
        /// </summary>
        private IDisposable BlockUI()
        {
            ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope();
            IMessageHelper messageHelper = lifetimeScope.Resolve<IMessageHelper>();

            return new CompositeDisposable(
                messageHelper.SetCursor(Cursors.WaitCursor),
                DisableEditControls(),
                lifetimeScope);
        }

        /// <summary>
        /// Disable editing controls
        /// </summary>
        private IDisposable DisableEditControls()
        {
            cancel.Enabled = false;
            ok.Enabled = false;

            return Disposable.Create(() =>
            {
                cancel.Enabled = true;
                ok.Enabled = true;
            });
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

            // Load the download settings when the option page is selected, that way we show
            // the most up to date version based on the store
            if (optionControl.SelectedPage == optionPageSettings)
            {
                ConfigureDownloadSettingsControl();
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
        /// Configure the Download settings control
        /// </summary>
        private void ConfigureDownloadSettingsControl()
        {
            Control oldDownloadSettingsControl = downloadSettingsControl as Control;

            if (oldDownloadSettingsControl != null &&
                panelDownloading.Controls.Contains(oldDownloadSettingsControl))
            {
                panelDownloading.Controls.Remove(oldDownloadSettingsControl);
            }

            downloadSettingsControl = storeType.CreateDownloadSettingsControl();
            downloadSettingsControl.LoadStore(store);
            downloadSettingsControl.Location = new Point(17, sectionAutoDownloads.Bottom + VerticalSpaceBetweenSections);
            downloadSettingsControl.Width = optionPageSettings.Width - downloadSettingsControl.Location.X - 10;
            downloadSettingsControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            panelDownloading.Controls.Add(downloadSettingsControl as Control);
        }

        /// <summary>
        /// Configure the Manual Order Settings control
        /// </summary>
        private void ConfigureManualOrderSettingsControl()
        {
            manualOrderSettingsControl = storeType.CreateManualOrderSettingsControl();
            manualOrderSettingsControl.Location = new Point(19, sectionTitleManualOrders.Bottom + VerticalSpaceBetweenSections);
            manualOrderSettingsControl.Width = optionPageSettings.Width - manualOrderSettingsControl.Location.X - 10;
            manualOrderSettingsControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            panelManualOrders.Controls.Add(manualOrderSettingsControl);
        }

        /// <summary>
        /// Load the content of the settings tab
        /// </summary>
        private void LoadSettingsTab()
        {
            EnumHelper.BindComboBox<AddressValidationStoreSettingType>(domesticAddressValidationSetting);
            EnumHelper.BindComboBox<AddressValidationStoreSettingType>(internationalAddressValidationSetting);

            StoreHasConnectionString(store);

            // store-specific settings control
            storeSettingsControl = storeType.CreateStoreSettingsControl();

            if (storeSettingsControl != null)
            {
                storeSettingsControl.LoadStore(store);

                // Settings control gets location and width of the section title, so that each settings control can simply
                // set its titles to go all the way across with anchors.
                storeSettingsControl.Location = new Point(panelManualOrders.Left, panelManualOrders.Bottom + VerticalSpaceBetweenSections);
                storeSettingsControl.Width = panelManualOrders.Width;
                storeSettingsControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                optionPageSettings.Controls.Add(storeSettingsControl);

                UpdatePanelPositions();

                storeSettingsControl.SizeChanged += (sender, args) => UpdatePanelPositions();
            }
            else
            {
                UpdatePanelPositions();
            }

            panelAddressValidation.Top = panelStoreStatus.Bottom + VerticalSpaceBetweenSections;

            // Validation Settings
            domesticAddressValidationSetting.SelectedValue = store.DomesticAddressValidationSetting;
            internationalAddressValidationSetting.SelectedValue = store.InternationalAddressValidationSetting;

            panelDefaultFilters.Top = panelAddressValidation.Bottom + VerticalSpaceBetweenSections;

            // Manual orders
            manualOrderSettingsControl.LoadStore(store);

            // Store status
            storeDisabled.Checked = !store.Enabled;
        }

        /// <summary>
        /// Update the location of controls below the specified control
        /// </summary>
        private void UpdatePanelPositions()
        {
            panelManualOrders.Top = panelDownloading.Bottom + VerticalSpaceBetweenSections;

            if (storeSettingsControl == null || storeSettingsControl.Height == 0)
            {
                panelStoreStatus.Top = panelManualOrders.Bottom + VerticalSpaceBetweenSections;
                panelAddressValidation.Top = panelStoreStatus.Bottom + VerticalSpaceBetweenSections;
                panelDefaultFilters.Top = panelAddressValidation.Bottom + VerticalSpaceBetweenSections;
            }
            else
            {
                storeSettingsControl.Top = panelManualOrders.Bottom + VerticalSpaceBetweenSections;
                panelStoreStatus.Top = storeSettingsControl.Bottom + VerticalSpaceBetweenSections;
                panelAddressValidation.Top = panelStoreStatus.Bottom + VerticalSpaceBetweenSections;
                panelDefaultFilters.Top = panelAddressValidation.Bottom + VerticalSpaceBetweenSections;
            }
        }

        /// <summary>
        /// Save the values from the store settings tab to the entity
        /// </summary>
        private async Task<bool> SaveSettingsTab()
        {
            downloadSettingsControl.Save();
            manualOrderSettingsControl.SaveToEntity(store);

            bool result = true;

            if (storeSettingsControl != null)
            {
                result = storeSettingsControl.SaveToEntity(store);                
            }

            if (result && store?.WarehouseStoreID != null && store.IsDirty && storeType.ShouldUseHub(store))
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    Result warehouseResult;
                    using (scope.Resolve<IMessageHelper>().ShowProgressDialog("Updating store information...", "Updating store information..."))
                    {
                        warehouseResult = await scope.Resolve<IWarehouseStoreClient>().UpdateStoreCredentials(store).ConfigureAwait(true);
                    }

                    if (warehouseResult.Failure)
                    {
                        MessageHelper.ShowError(this, $"An error occurred saving the store to ShipWorks.{Environment.NewLine + Environment.NewLine + warehouseResult.Message}");
                        result = false;
                    }
                }
            }

            // Check whether we should reset any pending address validations
            AddressValidationStoreSettingType currentSetting = store.DomesticAddressValidationSetting;
            AddressValidationStoreSettingType newSetting = (AddressValidationStoreSettingType) domesticAddressValidationSetting.SelectedValue;

            resetPendingValidations = (currentSetting == AddressValidationStoreSettingType.ValidateAndApply ||
                                       currentSetting == AddressValidationStoreSettingType.ValidateAndNotify)
                                      && (newSetting == AddressValidationStoreSettingType.ManualValidationOnly ||
                                          newSetting == AddressValidationStoreSettingType.ValidationDisabled);

            store.DomesticAddressValidationSetting = (AddressValidationStoreSettingType) domesticAddressValidationSetting.SelectedValue;
            store.InternationalAddressValidationSetting = (AddressValidationStoreSettingType) internationalAddressValidationSetting.SelectedValue;

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
                        using (var lifetimeScope = IoC.BeginLifetimeScope())
                        {
                            var tangoWebClient = lifetimeScope.Resolve<ITangoWebClient>();

                            accountDetail = tangoWebClient.GetLicenseStatus(license.Key, store);
                            licenseStatus.Text = accountDetail.Description;
                        }
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
        private async void OnOK(object sender, EventArgs e)
        {
            using (BlockUI())
            {
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
                    if (!await SaveAccountControl().ConfigureAwait(true))
                    {
                        optionControl.SelectedPage = optionPageOnlineAccount;
                        return;
                    }

                    // Save the settings tab to the store entity
                    if (!await SaveSettingsTab().ConfigureAwait(true))
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
        }

        /// <summary>
        /// Save the account control
        /// </summary>
        private async Task<bool> SaveAccountControl()
        {
            if (accountControl.IsSaveAsync)
            {
                return await accountControl.SaveToEntityAsync(store).ConfigureAwait(false);
            }

            return accountControl.SaveToEntity(store);
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