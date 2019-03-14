﻿using System;
using System.Threading.Tasks;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI.Wizard;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Amazon.SWA;

namespace ShipWorks.Shipping.UI.Carriers.Amazon.SWA
{
    /// <summary>
    /// Setup wizard for AmazonSWA shipment type
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.AmazonSWA)]
    public partial class AmazonSWASetupWizard : WizardForm, IShipmentTypeSetupWizard
    {
        private readonly AmazonSWAShipmentType shipmentType;
        private readonly IAmazonSWAAccountRepository accountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IMessageHelper messageHelper;
        private readonly IShipEngineWebClient shipEngineClient;
        private ShippingWizardPageFinish shippingWizardPageFinish;
        private readonly AmazonSWAAccountEntity account;
        private const string AmazonSWAAccountUrl = "http://www.AmazonSWA.com/contact";

        /// <summary>
        /// Constructor to be used by Visual Studio designer
        /// </summary>
        protected AmazonSWASetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSWASetupWizard(
            AmazonSWAShipmentType shipmentType,
            IAmazonSWAAccountRepository accountRepository,
            IShipEngineWebClient shipEngineClient,
            IShippingSettings shippingSettings,
            IMessageHelper messageHelper) : this()
        {
            this.shipmentType = shipmentType;
            this.accountRepository = accountRepository;
            this.shippingSettings = shippingSettings;
            this.messageHelper = messageHelper;
            this.shipEngineClient = shipEngineClient;

            account = new AmazonSWAAccountEntity();
        }

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(AmazonSWAAccountEntity account)
        {
            return new AmazonSWAAccountDescription().GetDefaultAccountDescription(account);
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
            throw new NotImplementedException();
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
            shippingSettings.MarkAsConfigured(ShipmentTypeCode.AmazonSWA);
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
        /// Shows the error message and prevents the user from proceeding through the wizard
        /// </summary>
        private void ShowWizardError(string errorMessage, WizardStepEventArgs e)
        {
            messageHelper.ShowError(errorMessage);
            e.NextPage = CurrentPage;
        }
    }
}
