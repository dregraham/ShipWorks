﻿using System;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    /// <summary>
    /// Setup wizard for Amazon shipment type
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressSetupWizard : WizardForm, IShipmentTypeSetupWizard
    {
        private readonly DhlExpressShipmentType shipmentType;
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IMessageHelper messageHelper;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private ShippingWizardPageFinish shippingWizardPageFinish;
        private readonly DhlExpressAccountEntity account;
        private const string DhlExpressAccountUrl = "http://www.dhl-usa.com/en/express/shipping/open_account.html";

        /// <summary>
        /// Constructor to be used by Visual Studio designer
        /// </summary>
        protected DhlExpressSetupWizard()
        {
            InitializeComponent();

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressSetupWizard(DhlExpressShipmentType shipmentType, IDhlExpressAccountRepository accountRepository, IShipEngineWebClient shipEngineWebClient, IShippingSettings shippingSettings,
            IMessageHelper messageHelper) : this()
        {
            this.shipmentType = shipmentType;
            this.accountRepository = accountRepository;
            this.shippingSettings = shippingSettings;
            this.messageHelper = messageHelper;
            this.shipEngineWebClient = shipEngineWebClient;

            account = new DhlExpressAccountEntity();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            wizardPageWelcome.StepNextAsync = OnStepNextWelcome;

            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(CreateFinishPage());
        }

        /// <summary>
        /// Create the finish wizard page
        /// </summary>
        private WizardPage CreateFinishPage()
        {
            shippingWizardPageFinish = new ShippingWizardPageFinish(shipmentType);
            shippingWizardPageFinish.SteppingInto += OnSteppingIntoFinish;
            return shippingWizardPageFinish;
        }

        /// <summary>
        /// Called when [step next welcome].
        /// </summary>
        private async Task OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            long dhlAccountNumber;

            if (string.IsNullOrWhiteSpace(accountNumber.Text))
            {
                ShowWizardError("Please enter your DHL Express account number.", e);
            }

            if (long.TryParse(accountNumber.Text, out dhlAccountNumber))
            {
                GenericResult<string> connectAccountResult = await shipEngineWebClient.ConnectDhlAccount(dhlAccountNumber.ToString());
                if (connectAccountResult.Success)
                {
                    account.AccountNumber = dhlAccountNumber;
                    account.ShipEngineCarrierId = connectAccountResult.Value;
                }
                else
                {
                    ShowWizardError(connectAccountResult.Message, e);
                }
            }
            else
            {
                ShowWizardError("DHL Express account number must contain only numbers.", e);
            }
        }

        /// <summary>
        /// Next pressed on contact screen
        /// </summary>
        private void OnStepNextContactInfo(object sender, WizardStepEventArgs e)
        {
            if (contactInformation.ValidateRequiredFields())
            {
                // Create a person adapter from the new ShippingOriginEntity
                PersonAdapter person = new PersonAdapter();
                contactInformation.SaveToEntity(person);

                account.FirstName = person.FirstName;
                account.MiddleName = person.MiddleName;
                account.LastName = person.LastName;
                account.Company = person.Company;
                account.Street1 = person.Street1;
                account.City = person.City;
                account.StateProvCode = Geography.GetStateProvCode(person.StateProvCode);
                account.PostalCode = person.PostalCode;
                account.CountryCode = Geography.GetCountryCode(person.CountryCode);
                account.Email = person.Email;
                account.Phone = person.Phone;
            }
            else
            {
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Saves the account.
        /// </summary>
        private void SaveAccount()
        {
            account.Description = GetDefaultDescription(account);

            accountRepository.Save(account);
            shippingSettings.MarkAsConfigured(ShipmentTypeCode.DhlExpress);
        }

        /// <summary>
        /// Finish the wizard
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            SaveAccount();

            // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
            // that a new account has been added.
            RateCache.Instance.Clear();
        }

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;

        /// <summary>
        /// Called when [open account link clicked].
        /// </summary>
        private void OnOpenAccountLinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl(DhlExpressAccountUrl, this);
        }

        /// <summary>
        /// Shows the error message and prevents the user from proceeding through the wizard
        /// </summary>
        private void ShowWizardError(string errorMessage, WizardStepEventArgs e)
        {
            messageHelper.ShowError(errorMessage);
            e.NextPage = CurrentPage;
        }

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(DhlExpressAccountEntity account)
        {
            StringBuilder description = new StringBuilder(account.AccountNumber.ToString());

            if (account.Street1.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(account.Street1);
            }

            if (account.PostalCode.Length > 0)
            {
                if (description.Length > 0)
                {
                    description.Append(", ");
                }

                description.Append(account.PostalCode);
            }

            return description.ToString();
        }
    }
}
