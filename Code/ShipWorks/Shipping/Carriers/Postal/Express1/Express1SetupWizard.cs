using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Express1;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Settings.WizardPages;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Wizard for setting up shipping with Express1
    /// </summary>
    public partial class Express1SetupWizard : WizardForm
    {
        //StampsAccountEntity account = null;
        bool forceAccountOnly = false;
        bool hideDetailedConfiguration;
        PersonAdapter initialAccountAddress;

        private Express1Registration registration;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1SetupWizard(Express1Registration registration) : 
            this(registration, false)
        {
        }

        public Express1SetupWizard(Express1Registration registration, bool forceAccountOnly)
        {
            InitializeComponent();

            this.registration = registration;

            List<KeyValuePair<Express1CreditCardType, string>> cardTypes = new List<KeyValuePair<Express1CreditCardType, string>>
                {
                    new KeyValuePair<Express1CreditCardType, string>(Express1CreditCardType.AmericanExpress, "American Express"),
                    new KeyValuePair<Express1CreditCardType, string>(Express1CreditCardType.Discover, "Discover"),
                    new KeyValuePair<Express1CreditCardType, string>(Express1CreditCardType.MasterCard, "MasterCard"),
                    new KeyValuePair<Express1CreditCardType, string>(Express1CreditCardType.Visa, "Visa")
                };

            cardType.ValueMember = "Key";
            cardType.DisplayMember = "Value";
            cardType.DataSource = cardTypes;

            cardExpireMonth.SelectedIndex = 0;
            cardExpireYear.SelectedIndex = 0;

            // Credit Card
            paymentMethod.SelectedIndex = 1;

            this.forceAccountOnly = forceAccountOnly;
        }

        /// <summary>
        /// Indicates if the detailed configuration pages are hidden to simplify setup
        /// </summary>
        public bool HideDetailedConfiguration
        {
            get { return hideDetailedConfiguration; }
            set { hideDetailedConfiguration = value; }
        }

        /// <summary>
        /// Get the initial address to populate for the account window
        /// </summary>
        public PersonAdapter InitialAccountAddress
        {
            get { return initialAccountAddress; }
            set { initialAccountAddress = value; }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(registration.ShipmentTypeCode);
            bool addAccountOnly = ShippingManager.IsShipmentTypeConfigured(registration.ShipmentTypeCode) || forceAccountOnly;

            // Initialize country to US
            if (initialAccountAddress != null)
            {
                personControl.LoadEntity(initialAccountAddress);
            }

            // it will just be to add a new account
            if (addAccountOnly)
            {
                Pages.Remove(wizardPageAccountList);

                // remove the settings pages
                Pages.Remove(wizardPageOptions);
            }
            else
            {
                // if there are no shippers yet, then remove the account page
                if (!StampsAccountManager.Express1Accounts.Any())
                {
                    Pages.Remove(wizardPageAccountList);
                }
                // there are other shippers, just show the list
                else
                {
                    Pages.Remove(wizardPageAccountType);
                    Pages.Remove(wizardPageAddress);
                    Pages.Remove(wizardPageAgreement);
                    Pages.Remove(wizardPagePayment);
                    Pages.Remove(wizardPageExisting);
                    Pages.Remove(wizardPageOptions);
                }

                // In the case of adding Express1 accounts to use for Stamps, we really want to super-simplify the setup.  Thats how this would get set.
                if (hideDetailedConfiguration)
                {
                    Pages.Remove(wizardPageOptions);

                    // We still add the automation page, but our special event handlers keep it from showing, and setup printing at the same time
                    var automationPage = new ShippingWizardPageAutomation(shipmentType);
                    automationPage.SteppingInto += (unused, args) =>
                    {
                        Cursor.Current = Cursors.Default;
                        args.Skip = true;
                        ShipmentPrintHelper.InstallDefaultRules(shipmentType.ShipmentTypeCode, true, this);
                    };

                    Pages.Add(automationPage);
                }
                else
                {
                    //TODO: implement the options control in a generic way
                    //optionsControl.LoadSettings(true);

                    Pages.Add(new ShippingWizardPageOrigin(shipmentType));
                    Pages.Add(new ShippingWizardPageDefaults(shipmentType));
                    Pages.Add(new ShippingWizardPagePrinting(shipmentType));
                    Pages.Add(new ShippingWizardPageAutomation(shipmentType));
                }
            }

            // Ensure the Finish page is the last one
            Pages.Remove(wizardPageFinish);
            Pages.Add(wizardPageFinish);
            Pages[Pages.Count - 1].SteppingInto += OnSteppingIntoFinish;

            if (Pages.Contains(wizardPageAccountList))
            {
                //TODO: implement the account control in a generic way
                //accountControl.Initialize(true);
            }
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            if (Pages.Contains(wizardPageOptions))
            {
                var settings = ShippingSettings.Fetch();

                optionsControl.SaveSettings(settings);
                ShippingSettings.Save(settings);
            }

            // Make sure any pending changes have been saved
            if (Pages.Contains(wizardPageAccountList))
            {
                panelAccountNumber.Visible = false;
                panelAccountCredentials.Visible = false;

                labelFinish.Text = "ShipWorks is now configured for Express1.";
            }
            else
            {
                registration.SaveAccount();

                // if it's a new account, display the account number and password for the user to copy since Express1 selects both for the user
                panelAccountCredentials.Visible = radioNewAccount.Checked;
                if (radioNewAccount.Checked)
                {
                    accountDetailsTextBox.Text = String.Format("Express1 Account Number: {0}\r\nPassword: {1}",
                        registration.AccountNumber, SecureText.Decrypt(registration.Password, "Stamps"));
                }
            }
        }

        /// <summary>
        /// Stepping next from the address page
        /// </summary>
        private void OnStepNextAddress(object sender, WizardStepEventArgs e)
        {
            PersonAdapter person = new PersonAdapter();
            personControl.SaveToEntity(person);

            if (person.FirstName.Trim().Length == 0 || person.LastName.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your contact name.");
                e.NextPage = CurrentPage;
                return;
            }

            if (person.Email.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your email address.");
                e.NextPage = CurrentPage;
                return;
            }

            if (person.Phone.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your phone number.");
                e.NextPage = CurrentPage;
                return;
            }

            if (person.Street1.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your street address.");
                e.NextPage = CurrentPage;
                return;
            }

            if (person.City.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your city.");
                e.NextPage = CurrentPage;
                return;
            }

            if (person.PostalCode.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your postal code.");
                e.NextPage = CurrentPage;
                return;
            }

            // pre-load these details into the CC controls
            personCreditCard.LoadEntity(person);

            if (radioExistingAccount.Checked)
            {
                e.NextPage = wizardPageExisting;
            }
        }

        /// <summary>
        /// Stepping next from the agreement page.
        /// </summary>
        private void OnStepNextAgreement(object sender, WizardStepEventArgs e)
        {
            if (!termsCheckBox.Checked)
            {
                MessageHelper.ShowInformation(this, "Please agree to the Express1 terms and conditions before creating your account.");
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping Next from the payment page
        /// </summary>
        private void OnStepNextPayment(object sender, WizardStepEventArgs e)
        {
            // Checking Account
            if (paymentMethod.SelectedIndex == 0)
            {
                if (checkingAccount.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Please enter your checking account number.");
                    e.NextPage = CurrentPage;
                    return;
                }

                if (checkingRouting.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Please enter your checking routing number.");
                    e.NextPage = CurrentPage;
                    return;
                }
            }
            // Credit Card
            else
            {
                PersonAdapter creditPerson = new PersonAdapter();
                personCreditCard.SaveToEntity(creditPerson);

                if (creditPerson.Street1.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Please enter your street address.");
                    e.NextPage = CurrentPage;
                    return;
                }

                if (creditPerson.City.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Please enter your city.");
                    e.NextPage = CurrentPage;
                    return;
                }

                if (creditPerson.PostalCode.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Please enter your postal code.");
                    e.NextPage = CurrentPage;
                    return;
                }

                if (cardNumber.Text.Trim().Length == 0)
                {
                    MessageHelper.ShowInformation(this, "Please enter your credit card number.");
                    e.NextPage = CurrentPage;
                    return;
                }
            }

            // get the address information entered on the demographics page
            PersonAdapter accountAddress = new PersonAdapter();
            personControl.SaveToEntity(accountAddress);

            // prepare payment information
            Express1PaymentInfo paymentInfo = GetPaymentInfo();
            try
            {
                // wait
                Cursor.Current = Cursors.WaitCursor;

                registration.Signup(accountAddress, paymentInfo);
            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Get the payment as it is entered in the Credit Card UI
        /// </summary>
        private Express1PaymentInfo GetPaymentInfo()
        {
            PersonAdapter billingAddress = new PersonAdapter();
            personCreditCard.SaveToEntity(billingAddress);

            Express1PaymentInfo paymentInfo = new Express1PaymentInfo(Express1PaymentType.CreditCard)
                {
                    CreditCardBillingAddress = billingAddress,
                    CreditCardType = (Express1CreditCardType)cardType.SelectedValue,
                    CreditCardAccountNumber = cardNumber.Text.Trim(),
                    CreditCardExpirationDate = new DateTime(cardExpireYear.SelectedIndex + 2009, cardExpireMonth.SelectedIndex + 1, 1),
                    AchAccountNumber = checkingAccount.Text.Trim(),
                    AchRoutingId = checkingRouting.Text.Trim()
                };

            return paymentInfo;
        }

        /// <summary>
        /// Payment Method was changed
        /// </summary>
        private void OnChangedPaymentMethod(object sender, EventArgs e)
        {
            // Checking Account
            if (paymentMethod.SelectedIndex == 0)
            {
                creditCardPanel.Visible = false;
                checkingPanel.Visible = true;

                // move checking details into place
                checkingPanel.Top = creditCardPanel.Top + 30;
            }
            // Credit Card
            else
            {
                creditCardPanel.Visible = true;
                checkingPanel.Visible = false;
            }
        }

        /// <summary>
        /// Stepping into the Existing Account page
        /// </summary>
        private void OnSteppingIntoExisting(object sender, WizardSteppingIntoEventArgs e)
        {
            if (!radioExistingAccount.Checked)
            {
                e.Skip = true;
            }
        }

        /// <summary>
        /// Stepping next from the Existing Account page.
        /// </summary>
        private void OnStepExistingNext(object sender, WizardStepEventArgs e)
        {
            if (accountExisting.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your Express1 account number.");
                e.NextPage = CurrentPage;
                return;
            }

            if (passwordExisting.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your Express1 account password.");
                e.NextPage = CurrentPage;
                return;
            }

            try
            {
                //// credentials
                //registration.AccountNumber = accountExisting.Text.Trim();
                //registration.Password = SecureText.Encrypt(passwordExisting.Text, "Stamps");

                //// other  data
                //account.CreatedByShipWorks = false;
                //account.AccountType = (int)StampsAccountType.Standard;
                //account.ApiInitialPassword = "";
                //account.SignupConfirmation = "";
                //account.WebPassword = "";
                //account.TestAccount = Express1StampsUtility.UseTestServer;
                //account.ScanFormAddressSource = (int)StampsScanFormAddressSource.Provider;

                //// address
                //personControl.SaveToEntity(new PersonAdapter(account, ""));
                //account.MailingPostalCode = account.PostalCode;

                //// description
                //account.Description = StampsAccountManager.GetDefaultDescription(account);

                //// see if we can connect
                //StampsApiClient.GetAccountStatus(account);

                //// save
                //StampsAccountManager.SaveAccount(account);

                //// signup complete...

            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks was unable to communicate with Stamps using this account information:\n\n" + ex.Message);
                e.NextPage = CurrentPage;

                //account = null;
            }
        }

        /// <summary>
        /// Buy postage for the account
        /// </summary>
        private void OnClickBuyPostage(object sender, EventArgs e)
        {
            //// Show the Postage Purchasing dialog
            //using (StampsBuyPostageDlg dlg = new StampsBuyPostageDlg(account))
            //{
            //    dlg.ShowDialog(this);
            //}
        }

        /// <summary>
        /// Stepping into the Options page
        /// </summary>
        private void OnSteppingIntoOptions(object sender, WizardSteppingIntoEventArgs e)
        {
            // if we're creating a new account
            if (radioNewAccount.Checked)
            {
                BackEnabled = false;
            }
        }
    }
}
