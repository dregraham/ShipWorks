using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    /// <summary>
    /// Setup wizard for Dhl Express shipment type
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.DhlExpress)]
    [KeyedComponent(typeof(IOneBalanceSetupWizard), ShipmentTypeCode.DhlExpress)]
    public partial class DhlExpressSetupWizard : WizardForm, IShipmentTypeSetupWizard, IOneBalanceSetupWizard
    {
        private readonly DhlExpressShipmentType shipmentType;
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly IShippingSettings shippingSettings;
        private readonly IMessageHelper messageHelper;
        private readonly IShipEngineWebClient shipEngineClient;
        private ShippingWizardPageFinish shippingWizardPageFinish;
        private readonly DhlExpressAccountEntity account;
        private bool skipAccountSetup = false;

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
        public DhlExpressSetupWizard(DhlExpressShipmentType shipmentType, IDhlExpressAccountRepository accountRepository, IShipEngineWebClient shipEngineClient, IShippingSettings shippingSettings,
            IMessageHelper messageHelper) : this()
        {
            this.shipmentType = shipmentType;
            this.accountRepository = accountRepository;
            this.shippingSettings = shippingSettings;
            this.messageHelper = messageHelper;
            this.shipEngineClient = shipEngineClient;

            account = new DhlExpressAccountEntity();
        }

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;

        /// <summary>
        /// Get the default description to use for the given account
        /// </summary>
        public static string GetDefaultDescription(DhlExpressAccountEntity account)
        {
            return new DhlExpressAccountDescription().GetDefaultAccountDescription(account);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            wizardPageWelcome.StepNextAsync = OnStepNextWelcome;
            var defaultPage = new ShippingWizardPageDefaults(shipmentType);
            if (skipAccountSetup)
            {
                Pages.Remove(wizardPageWelcome);
                Pages.Remove(wizardPageContactInfo);
                defaultPage.LoadSettings();
            }

            Pages.Add(defaultPage);
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
                this.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                GenericResult<string> connectAccountResult = await shipEngineClient.ConnectDhlAccount(dhlAccountNumber.ToString());
                if (connectAccountResult.Success)
                {
                    account.AccountNumber = dhlAccountNumber;
                    account.ShipEngineCarrierId = connectAccountResult.Value;
                }
                else
                {
                    ShowWizardError(connectAccountResult.Message, e);
                }

                this.Enabled = true;
                this.Cursor = Cursors.Default;
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
        /// Shows the error message and prevents the user from proceeding through the wizard
        /// </summary>
        private void ShowWizardError(string errorMessage, WizardStepEventArgs e)
        {
            messageHelper.ShowError(errorMessage);
            e.NextPage = CurrentPage;
        }

        /// <summary>
        /// Setup DHL Express One Balance account. 
        /// </summary>
        public DialogResult SetupOneBalanceAccount(IWin32Window owner)
        {
            var uspsAccounts = UspsAccountManager.UspsAccountsReadOnly;
            IUspsAccountEntity uspsAccount = null;

            if (uspsAccounts.IsCountEqualTo(1))
            {
                uspsAccount = uspsAccounts.First();
            }
            else
            {
                uspsAccount = uspsAccounts.FirstOrDefault(x => x.ShipEngineCarrierId != null);
            }

            // Only skip the account screen if they already have a One Balance USPS account.
            skipAccountSetup = uspsAccount != null;

            if (skipAccountSetup)
            {
                CreateAccountFromUsps(uspsAccount);
            }
            return ShowDialog(owner);
        }

        ///<summary>
        /// Add an existing DHL Express account
        /// </summary>
        public DialogResult SetupExistingAccount(IWin32Window owner)
        {
            return ShowDialog(owner);
        }

        /// <summary>
        /// Copies the account info from a usps account
        /// </summary>
        private void CreateAccountFromUsps(IUspsAccountEntity uspsAccount)
        {
            account.AccountNumber = uspsAccount.UspsAccountID;
            account.FirstName = uspsAccount.FirstName;
            account.MiddleName = uspsAccount.MiddleName;
            account.LastName = uspsAccount.LastName;
            account.Company = uspsAccount.Company;
            account.Street1 = uspsAccount.Street1;
            account.City = uspsAccount.City;
            account.StateProvCode = Geography.GetStateProvCode(uspsAccount.StateProvCode);
            account.PostalCode = uspsAccount.PostalCode;
            account.CountryCode = Geography.GetCountryCode(uspsAccount.CountryCode);
            account.Email = uspsAccount.Email;
            account.Phone = uspsAccount.Phone;
            account.UspsAccountId = uspsAccount.UspsAccountID;
        }
    }
}
