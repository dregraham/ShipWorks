using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.UI;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// Wizard for setting up shipping with Express1
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class Express1SetupWizard : ShipmentTypeSetupWizardForm
    {
        private bool hideDetailedConfiguration;
        private PersonAdapter initialAccountAddress;

        private readonly PostalOptionsControlBase optionsControl;
        private readonly PostalAccountManagerControlBase accountControl;
        private readonly IExpress1PurchasePostageDlg postageDialog;

        private readonly Express1Registration registration;
        private readonly IEnumerable<IEntity2> existingExpress1Accounts;
        private int initialPersonCreditCardHeight;

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1SetupWizard"/> class. Since this wizard is intended to be
        /// used regardless of the carrier that Express1 partners with (e.g. Endicia or Usps), the caller needs to provide
        /// the appropriate controls and postage dialog that are specific to the Express1 partner.
        /// </summary>
        [NDependIgnoreTooManyParams]
        public Express1SetupWizard(IExpress1PurchasePostageDlg postageDialog, PostalAccountManagerControlBase accountControl, PostalOptionsControlBase optionsControl, Express1Registration registration, IEnumerable<IEntity2> existingExpress1Accounts)
        {
            if (postageDialog == null)
            {
                throw new ArgumentNullException("postageDialog");
            }

            ForceAccountOnly = false;
            if (accountControl == null)
            {
                throw new ArgumentNullException("accountControl");
            }

            if (optionsControl == null)
            {
                throw new ArgumentNullException("optionsControl");
            }

            InitializeComponent();

            initialPersonCreditCardHeight = personCreditCard.Height;

            this.postageDialog = postageDialog;

            this.accountControl = accountControl;
            accountControlPanel.Controls.Add(accountControl);
            accountControl.Dock = DockStyle.Fill;

            this.optionsControl = optionsControl;
            optionsControlPanel.Controls.Add(optionsControl);
            optionsControl.Dock = DockStyle.Fill;

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

            this.ForceAccountOnly = false;
            this.existingExpress1Accounts = existingExpress1Accounts ?? new List<IEntity2>();
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
        /// Gets and sets whether the wizard should configure the shipping type as well as creating the account
        /// </summary>
        public bool ForceAccountOnly { get; set; }

        /// <summary>
        /// Initialization
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(registration.ShipmentTypeCode);
            bool addAccountOnly = ShippingManager.IsShipmentTypeConfigured(registration.ShipmentTypeCode) || ForceAccountOnly;

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
                if (!existingExpress1Accounts.Any())
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

                // In the case of adding Express1 accounts to use for Usps, we really want to super-simplify the setup.  Thats how this would get set.
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
                    optionsControl.LoadSettings();

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
                accountControl.Initialize();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
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
                        registration.UserName, registration.PlainTextPassword);
                }
            }
        }

        /// <summary>
        /// Stepping next from the address page
        /// </summary>
        private void OnStepNextAddress(object sender, WizardStepEventArgs e)
        {
            registration.MailingAddress = new PersonAdapter();
            personControl.SaveToEntity(registration.MailingAddress);

            if (registration.MailingAddress.CountryCode != "US")
            {
                MessageHelper.ShowInformation(this, "USPS only supports US addresses.");
                e.NextPage = CurrentPage;
                return;
            }

            if (!personControl.ValidateRequiredFields())
            {
                e.NextPage = CurrentPage;
                return;
            }

            // pre-load these details into the CC controls
            personCreditCard.LoadEntity(registration.MailingAddress);

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
            // prepare payment information
            registration.Payment = GetPaymentInfo();

            var errors = registration.ValidatePaymentInfo();
            if (errors.Any())
            {
                // This is the same error formatting as the person control uses
                MessageHelper.ShowInformation(this, "The following issues were found:" +
                    Environment.NewLine +
                    errors.Select(x => "    " + x.Message).Combine(Environment.NewLine));
                e.NextPage = CurrentPage;
                return;
            }

            try
            {
                // wait
                Cursor.Current = Cursors.WaitCursor;

                registration.CreateNewAccount();

                Cursor.Current = Cursors.Arrow;
            }
            catch (Express1RegistrationException ex)
            {
                MessageHelper.ShowError(this, ex.Message);

                e.NextPage = CurrentPage;
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
                    CreditCardNameOnCard = registration.Name,
                    CreditCardBillingAddress = billingAddress,
                    CreditCardType = (Express1CreditCardType)cardType.SelectedValue,
                    CreditCardAccountNumber = cardNumber.Text.Trim(),
                    CreditCardExpirationDate = new DateTime(cardExpireYear.SelectedIndex + 2009, cardExpireMonth.SelectedIndex + 1, 1),
                    AchAccountNumber = string.Empty,
                    AchRoutingId = string.Empty
                };

            return paymentInfo;
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

            registration.UserName = accountExisting.Text.Trim();
            registration.PlainTextPassword = passwordExisting.Text;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                registration.AddExistingAccount();
            }
            catch (Express1RegistrationException ex)
            {
                string message = string.Format("ShipWorks encountered an error while trying to add the Express1 account.{0}{0}{1}",
                    Environment.NewLine, ex.Message);

                MessageHelper.ShowError(this, message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Buy postage for the account
        /// </summary>
        private void OnClickBuyPostage(object sender, EventArgs e)
        {
            try
            {
                if (registration.AccountId.HasValue)
                {
                    // Show the Postage Purchasing dialog
                    postageDialog.ShowDialog(this, registration.AccountId.Value);
                }
                else
                {
                    MessageHelper.ShowError(this, "An account needs to be created before trying to buy postage.");
                }
            }
            catch (UspsException uspsException)
            {
                // This could be refactored to catch a more general type of "purchase postage" exception
                // when Endicia is incorporated into this setup wizard
                MessageHelper.ShowError(this, uspsException.Message);
            }
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

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                // The user closed/canceled before completing the wizard, so we need to clean up the
                // account that may have been created since the account is saved in ShipWorks as
                // soon as possible
                registration.DeleteAccount();
            }
            else
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();
            }
        }

        /// <summary>
        /// Stepping next from the settings page
        /// </summary>
        private void OnStepNextSettings(object sender, WizardStepEventArgs e)
        {
            if (Pages.Contains(wizardPageOptions))
            {
                var settings = ShippingSettings.Fetch();

                optionsControl.SaveSettings(settings);
                ShippingSettings.Save(settings);
            }
        }

        /// <summary>
        /// Adjust the location of the credit card details if the credit card person control changes
        /// </summary>
        private void OnPersonCreditCardResize(object sender, EventArgs e)
        {
            creditCardDetailsPanel.Top = creditCardDetailsPanel.Top - (initialPersonCreditCardHeight - personCreditCard.Height);
        }
    }
}
