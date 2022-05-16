using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Carriers.DhlEcommerce.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    /// <summary>
    /// Setup wizard for processing shipments with Dhl eCommerce
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.DhlEcommerce)]
    public partial class DhlEcommerceSetupWizard : WizardForm, IShipmentTypeSetupWizard
    {
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;
        private readonly IShipEngineWebClient shipEngineClient;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IDhlEcommerceAccountRepository accountRepo;
        private readonly IShippingProfileManager shippingProfileManager;
        private DhlEcommerceAccountEntity account;

        /// <summary>
        /// Constructor to be used by Visual Studio Designer
        /// </summary>
        public DhlEcommerceSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceSetupWizard(IShipmentTypeManager shipmentTypeManager,
            IShippingManager shippingManager,
            IMessageHelper messageHelper,
            IShipEngineWebClient shipEngineClient,
            IEncryptionProviderFactory encryptionProviderFactory,
            IDhlEcommerceAccountRepository accountRepo,
            IShippingProfileManager shippingProfileManager) : this()
        {
            this.shipmentTypeManager = shipmentTypeManager;
            this.shippingManager = shippingManager;
            this.messageHelper = messageHelper;
            this.shipEngineClient = shipEngineClient;
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.accountRepo = accountRepo;
            this.shippingProfileManager = shippingProfileManager;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            wizardPageCredentials.StepNextAsync = OnStepNextCredentials;

            var shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.DhlEcommerce);

            optionsControl.LoadSettings();

            var shippingWizardPageFinish = new ShippingWizardPageFinish(shipmentType);

            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(shippingWizardPageFinish);

            if (shippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.DhlEcommerce))
            {
                Pages.Remove(wizardPageOptions);
            }
            else
            {
                wizardPageOptions.StepNext += (o, args) => optionsControl.SaveSettings();
            }

            shippingWizardPageFinish.SteppingInto += OnSteppingIntoFinish;

            LoadComboBox<DhlEcommerceDistributionCenters>(distributionCenters);
        }

        /// <summary>
        /// Load the given combobox from the enum T
        /// </summary>
        private void LoadComboBox<T>(ComboBox comboBox) where T : struct
        {
            var values = new Dictionary<string, string>();

            Enum.GetValues(typeof(T))
                .Cast<T>()
                .ForEach(x => values.Add(EnumHelper.GetApiValue(x as Enum), EnumHelper.GetDescription(x as Enum)));

            comboBox.DataSource = new BindingSource(values, null);
            comboBox.ValueMember = "Key";
            comboBox.DisplayMember = "Value";
        }

        /// <summary>
        /// User clicked the link to open the support article for configuring a DHL eCommerce account
        /// </summary>
        private void OnLinkDhlECommerceConfigArticle(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://support.shipworks.com/hc/en-us/articles/5895141486363", this);
        }

        /// <summary>
        /// User clicked the link to open the DHL website
        /// </summary>
        private void OnLinkDhlWebsite(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.dhl.com", this);
        }

        /// <summary>
        /// Stepping next from the credentials page
        /// </summary>
        private async Task OnStepNextCredentials(object sender, WizardStepEventArgs e)
        {
            var fieldChecker = new RequiredFieldChecker();
            fieldChecker.Check("Client ID", pickupNumber.Text);
            fieldChecker.Check("API Secret", apiSecret.Text);
            fieldChecker.Check("Pickup Account Number", clientId.Text);
            fieldChecker.Check("Description", accountDescription.Text);

            var validationResult = fieldChecker.Validate();

            if (validationResult.Failure)
            {
                ShowWizardError(validationResult.Message, e);
                return;
            }

            try
            {
                Application.UseWaitCursor = true;
                Cursor.Current = Cursors.WaitCursor;
                NextEnabled = false;
                BackEnabled = false;
                CanCancel = false;

                var result = await shipEngineClient.ConnectDhlEcommerceAccount(new DhlEcommerceRegistrationRequest
                {
                    ClientId = clientId.Text,
                    ApiSecret = apiSecret.Text,
                    PickupNumber = pickupNumber.Text,
                    DistributionCenter = distributionCenters.SelectedValue?.ToString() ?? distributionCenters.Text,
                    SoldTo = soldTo.Text,
                    Nickname = accountDescription.Text,
                });

                if (result.Failure)
                {
                    ShowWizardError(result.Message, e);
                    return;
                }

                if (account == null)
                {
                    account = new DhlEcommerceAccountEntity();
                    account.CreatedDate = DateTime.Now;
                }

                var secureText = encryptionProviderFactory.CreateSecureTextEncryptionProvider(clientId.Text);

                account.ShipEngineCarrierId = result.Value;
                account.ClientId = clientId.Text;
                account.ApiSecret = secureText.Encrypt(apiSecret.Text);
                account.PickupNumber = pickupNumber.Text;
                account.DistributionCenter = distributionCenters.SelectedValue?.ToString() ?? distributionCenters.Text;
                account.SoldTo = soldTo.Text;
                account.Description = accountDescription.Text;

                var shipmentType = shipmentTypeManager.Get(ShipmentTypeCode.DhlEcommerce);
                var defaultProfile = shippingProfileManager.GetOrCreatePrimaryProfile(shipmentType);

                shippingProfileManager.SaveProfile(defaultProfile);
            }
            catch (Exception ex)
            {
                ShowWizardError($"An error occurred adding the DHL eCommerce account:\n\n{ex.Message}", e);
            }
            finally
            {
                NextEnabled = true;
                BackEnabled = true;
                CanCancel = true;
                Application.UseWaitCursor = false;
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// Next pressed on contact screen
        /// </summary>
        private void OnStepNextContactInfo(object sender, WizardStepEventArgs e)
        {
            if (!contactInformation.ValidateRequiredFields())
            {
                e.NextPage = CurrentPage;
                return;
            }

            PersonAdapter personAdapter = new PersonAdapter(account, "");
            contactInformation.SaveToEntity(personAdapter);

            accountRepo.Save(account);
        }

        /// <summary>
        /// Wizard is finishing
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            accountRepo.Save(account);

            // Mark the new account as configured
            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.DhlEcommerce);

            // If this is the only account, update this shipment type profiles with this account
            var accounts = accountRepo.Accounts;
            if (accounts.Count() == 1)
            {
                var accountEntity = accounts.First();

                // Update any profiles to use this account if this is the only account
                // in the system. This is to account for the situation where there a multiple
                // profiles that may be associated with a previous account that has since
                // been deleted.
                foreach (var shippingProfileEntity in shippingProfileManager.Profiles.Where(p => p.ShipmentType == ShipmentTypeCode.DhlEcommerce))
                {
                    if (shippingProfileEntity.DhlEcommerce.DhlEcommerceAccountID.HasValue)
                    {
                        shippingProfileEntity.DhlEcommerce.DhlEcommerceAccountID = accountEntity.DhlEcommerceAccountID;
                        shippingProfileManager.SaveProfile(shippingProfileEntity);
                    }
                }
            }
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && account != null)
            {
                accountRepo.DeleteAccount(account);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();
            }
        }

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;

        /// <summary>
        /// Shows the error message and prevents the user from proceeding through the wizard
        /// </summary>
        private void ShowWizardError(string errorMessage, WizardStepEventArgs e)
        {
            if (errorMessage.IsNullOrWhiteSpace())
            {
                errorMessage = "An unknown error occurred";
            }

            messageHelper.ShowError(errorMessage);
            e.NextPage = CurrentPage;
        }
    }
}