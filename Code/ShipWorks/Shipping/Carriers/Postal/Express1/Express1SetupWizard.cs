using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Settings.WizardPages;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Settings;

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
        private PostalOptionsControlBase optionsControl;
        private PostalAccountManagerControlBase accountControl;

        private Express1Registration registration;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1SetupWizard(PostalAccountManagerControlBase accountControl, PostalOptionsControlBase optionsControl, Express1Registration registration) : 
            this(accountControl, optionsControl, registration, false)
        {
        }

        public Express1SetupWizard(PostalAccountManagerControlBase accountControl, PostalOptionsControlBase optionsControl, Express1Registration registration, bool forceAccountOnly)
        {
            if (accountControl == null)
            {
                throw new ArgumentNullException("accountControl");
            }

            if (optionsControl == null)
            {
                throw new ArgumentNullException("optionsControl");
            }

            InitializeComponent();

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
                        registration.UserName, registration.Password);
                }
            }
        }

        /// <summary>
        /// Stepping next from the address page
        /// </summary>
        private void OnStepNextAddress(object sender, WizardStepEventArgs e)
        {
            if (!personControl.ValidateRequiredFields())
            {
                e.NextPage = CurrentPage;
                return;
            }

            registration.MailingAddress = new PersonAdapter();
            personControl.SaveToEntity(registration.MailingAddress);

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

                if (!registration.CreateNewAccount())
                {
                    e.NextPage = CurrentPage;
                    Cursor.Current = Cursors.Arrow;
                }
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
                registration.UserName = accountExisting.Text.Trim();
                registration.Password = passwordExisting.Text;

                if (!registration.AddExistingAccount())
                {
                    e.NextPage = CurrentPage;
                    Cursor.Current = Cursors.Arrow;
                }
            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks was unable to communicate with Express1 using this account information:\n\n" + ex.Message);
                e.NextPage = CurrentPage;
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
