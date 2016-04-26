using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Wizard;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Settings;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Stores;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Wizard Form for setting up shipping with endicia
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class EndiciaSetupWizard : ShipmentTypeSetupWizardForm
    {
        EndiciaAccountEntity account;
        EndiciaApiClient endiciaApiClient = new EndiciaApiClient();

        // track if the account is one being migrated from 2
        bool migratingDazzleAccount = false;

        // User has completed the signup process for the account
        bool signupCompleted = false;

        // Will be non-null if we are in freemium signup mode
        FreemiumFreeEdition freemiumEdition = null;
        private bool planTypeWasShown = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaSetupWizard() 
        {
            InitializeComponent();

            EnumHelper.BindComboBox<EndiciaCreditCardType>(cardType);
            cardExpireMonth.SelectedIndex = 0;
            cardExpireYear.SelectedIndex = 0;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Endicia);

            // For freemium we start fresh every time
            if (FreemiumFreeEdition.IsActive)
            {
                if (EndiciaAccountManager.GetAccounts(EndiciaReseller.None, true).Count > 0)
                {
                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.DeleteEntitiesDirectly(typeof(EndiciaAccountEntity), null);
                    }

                    EndiciaAccountManager.CheckForChangesNeeded();
                }
            }

            List<EndiciaAccountEntity> accounts = EndiciaAccountManager.GetAccounts(EndiciaReseller.None, true);

            // See if there's still an account waiting to be migrated from sw2.  That's our starting point if it exists
            account = accounts.FirstOrDefault(a => a.IsDazzleMigrationPending);
            migratingDazzleAccount = account != null;

            if (migratingDazzleAccount)
            {
                PersonAdapter person = new PersonAdapter(account, "");
                personControl.LoadEntity(person);
            }

            // If there is an account that they started setting up but didn't fully complete the process (and we're not just specifically updating some other account)
            if (accounts.Any(a => a.AccountNumber == null) && !migratingDazzleAccount)
            {
                account = accounts.FirstOrDefault(a => a.AccountNumber == null);

                Pages.Remove(wizardPageAccountType);
                Pages.Remove(wizardPageAddress);
                Pages.Remove(wizardPagePasswords);
                Pages.Remove(wizardPagePlan);
                Pages.Remove(wizardPagePayment);
            }

            // If its not already setup, load all the settings\configuration pages
            if (!ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Endicia))
            {
                optionsControl.LoadSettings(EndiciaReseller.None);

                Pages.Add(new ShippingWizardPageOrigin(shipmentType));
                Pages.Add(new ShippingWizardPageDefaults(shipmentType));
                Pages.Add(new ShippingWizardPagePrinting(shipmentType));
                Pages.Add(new ShippingWizardPageAutomation(shipmentType));

                // There are accounts with numbers (migrated v2 label server accounts)
                if (accounts.Count > 0 && accounts.All(a => a.AccountNumber != null) && !migratingDazzleAccount)
                {
                    account = accounts.FirstOrDefault(a => a.AccountNumber != null);

                    Pages.Remove(wizardPageAccountType);
                    Pages.Remove(wizardPageAddress);
                    Pages.Remove(wizardPageAccountNumber);
                    Pages.Remove(wizardPagePasswords);
                    Pages.Remove(wizardPagePlan);
                    Pages.Remove(wizardPagePayment);
                }
            }
            // Otherwise it will just be to setup the shipper
            else
            {
                Pages.Remove(wizardPageOptions);
            }

            // Ensure finish page is the last page
            Pages.Remove(wizardPageFinish);
            Pages.Add(wizardPageFinish);
            Pages[Pages.Count - 1].SteppingInto += new EventHandler<WizardSteppingIntoEventArgs>(OnSteppingIntoFinish);
        }

        /// <summary>
        /// Open the web page to show pricing information
        /// </summary>
        private void OnLinkPricing(object sender, EventArgs e)
        {
            // TODO: update this URL
            WebHelper.OpenUrl("http://www.interapptive.com", this);
        }

        /// <summary>
        /// Open the USPS privacy policy
        /// </summary>
        private void OnLinkUspsPrivacy(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.usps.com/privacyoffice/privacypolicy.htm#privacyact", this);
        }

        /// <summary>
        /// Open the postage privacy act window
        /// </summary>
        private void OnLinkPostagePrivacy(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.usps.com/cpim/ftp/hand/as353/as353apdx_051.htm", this);
        }

        /// <summary>
        /// Open the Endicia terms and conditions window
        /// </summary>
        private void OnLinkEndiciaTerms(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.endicia.com/SignUp/Notice/TermsAndConditions/", this);
        }

        /// <summary>
        /// Stepping into the account type page
        /// </summary>
        private void OnSteppingIntoAccountType(object sender, WizardSteppingIntoEventArgs e)
        {
            if (FreemiumFreeEdition.IsActive)
            {
                EbayStoreEntity store = (EbayStoreEntity) StoreManager.GetAllStores().Single();
                freemiumEdition = (FreemiumFreeEdition) EditionSerializer.Restore(store);

                if (freemiumEdition.AccountType != FreemiumAccountType.None)
                {
                    labelInfo1.Text = string.Format(
                        "You have already signed up for ShipWorks and Endicia using eBay ID '{0}'.\n\n" +
                        "To continue you will need provide some of the account information you used when you signed up.",
                        store.EBayUserID);

                    panelNewOrExisting.Visible = false;

                    radioNewAccount.Checked = false;
                    radioExistingAccount.Checked = true;
                }
            }
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        private void OnStepNextAccountType(object sender, WizardStepEventArgs e)
        {
            // Update some messaging for freemium
            if (freemiumEdition != null)
            {
                wizardPageAddress.Description = radioExistingAccount.Checked ?
                    "Enter the information for your ShipWorks account" :
                    "Enter the information for your ShipWorks and Endicia accounts.";

                wizardPagePayment.Description = radioExistingAccount.Checked ?
                    "Enter the credit card to use for your account." :
                    "Enter the credit card to use for your accounts.";

                labelEndiciaFee.Text = "Credit Card";

                labelCreditCardFees.Text = radioExistingAccount.Checked ?
                    "Your ShipWorks service fees are free, but you are required to have a credit card on file." :
                    "Your account fees are free, but you are required to pay for postage and insurance.";
            }
        }

        /// <summary>
        /// Stepping next from the address page
        /// </summary>
        [NDependIgnoreLongMethod]
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

            if (person.CountryCode != "US")
            {
                MessageHelper.ShowInformation(this, "USPS only supports US addresses.");
                e.NextPage = CurrentPage;
                return;
            }

            personCreditCard.LoadEntity(person);

            if (freemiumEdition != null)
            {
                // We already have a store record for them in Tango - they just need to enter the Endicia account info
                if (freemiumEdition.AccountType != FreemiumAccountType.None)
                {
                    e.NextPage = wizardPageExisting;
                }
                else
                {
                    // We don't have a store for them in tango - determine which information we need
                    if (radioExistingAccount.Checked)
                    {
                        e.NextPage = wizardPagePayment;
                    }
                    else
                    {
                        e.NextPage = wizardPagePasswords;
                    }
                }
            }
            else
            {
                if (radioExistingAccount.Checked)
                {
                    e.NextPage = wizardPageExisting;
                }
            }
        }

        /// <summary>
        /// Stepping into the plan page
        /// </summary>
        private void OnSteppingIntoPlan(object sender, WizardSteppingIntoEventArgs e)
        {
            // Can't choose plan if we're freemium - its a 'freemium' plan
            e.Skip = freemiumEdition != null;
        }

        /// <summary>
        /// The Plan Type page is been shown
        /// </summary>
        private void OnPlanTypeShown(object sender, EventArgs e)
        {
            // Due to the way control activation and RadioButton's work, the first one gets automatically checked 
            // when we want none to be checked when the page opens. Don't change the selection after the page is first
            // shown, however.
            if (!planTypeWasShown)
            {
                radioPlanStandard.Checked = false;
                planTypeWasShown = true;
            }
        }

        /// <summary>
        /// Stepping next from the plan type page
        /// </summary>
        private void OnStepNextPlanType(object sender, WizardStepEventArgs e)
        {
            // Have to have a selection
            if (!radioPlanPremium.Checked && !radioPlanStandard.Checked)
            {
                MessageHelper.ShowMessage(this, "Select a service level for your new Endicia account.");
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping next from the passwords page
        /// </summary>
        private void OnStepNextPasswords(object sender, WizardStepEventArgs e)
        {
            if (internetPassword.Text.Length < 5)
            {
                MessageHelper.ShowInformation(this, "Your internet password must be a least 5 characters.");
                e.NextPage = CurrentPage;
                return;
            }

            if (softwarePassword.Text.Length < 5)
            {
                MessageHelper.ShowInformation(this, "Your software password must be at least 5 characters.");
                e.NextPage = CurrentPage;
                return;
            }

            if (challengeQuestion.Text.Trim().Length < 5)
            {
                MessageHelper.ShowInformation(this, "Please enter a challenge question that is more than 5 characters.");
                e.NextPage = CurrentPage;
                return;
            }

            if (challengeAnswer.Text.Trim().Length < 5)
            {
                MessageHelper.ShowInformation(this, "Please enter a challenge answer that is more than 5 characters.");
                e.NextPage = CurrentPage;
                return;
            }

            if (!termsAndConditions.Checked)
            {
                MessageHelper.ShowInformation(this, "You must agree to the terms and conditions.");
                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping into the payments page
        /// </summary>
        private void OnSteppingIntoPayment(object sender, WizardSteppingIntoEventArgs e)
        {
            if (freemiumEdition != null)
            {
                panelFreemiumSwFees.Visible = true;

                if (radioExistingAccount.Checked)
                {
                    panelPostagePayments.Visible = false;
                    panelFreemiumSwFees.Top = panelPostagePayments.Top + 5;
                }
                else
                {
                    panelPostagePayments.Visible = true;
                    panelFreemiumSwFees.Top = panelPostagePayments.Bottom + 5;
                }
            }
        }

        /// <summary>
        /// Changing whether to pay for postage via credit card or check
        /// </summary>
        private void OnCheckChangedPostagePaymentType(object sender, EventArgs e)
        {
            labelCheckingAccount.Enabled = postagePaymentCheck.Checked;
            checkingAccount.Enabled = postagePaymentCheck.Checked;

            labelRouting.Enabled = postagePaymentCheck.Checked;
            checkingRouting.Enabled = postagePaymentCheck.Checked;
        }

        /// <summary>
        /// Stepping next from the payment page
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        private void OnStepNextPayment(object sender, WizardStepEventArgs e)
        {
            PersonAdapter creditPerson = new PersonAdapter();
            personCreditCard.SaveToEntity(creditPerson);

            if (freemiumEdition != null && !shipworksFeesAgree.Checked)
            {
                MessageHelper.ShowInformation(this, 
                    "You must agree that Interapptive can use your credit card to pay for insurance if you optionally choose to use ShipWorks Shipping Insurance.");

                e.NextPage = CurrentPage;
                return;
            }

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

            if (postagePaymentCheck.Checked)
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

            Cursor.Current = Cursors.WaitCursor;

            PersonAdapter accountAddress = new PersonAdapter();
            personControl.SaveToEntity(accountAddress);

            EndiciaPaymentInfo paymentInfo = GetPaymentInfo();

            // If we are signing up for ELS freemium, first validate the account information _would_ be ok for Tango - this doesnt actually create anything
            if (freemiumEdition != null && radioNewAccount.Checked)
            {
                try
                {
                    TangoWebClient.CreateFreemiumStore(freemiumEdition.Store, accountAddress, paymentInfo, "", true);
                }
                catch (Exception ex)
                {
                    if (ex is TangoException || ex is ShipWorksLicenseException)
                    {
                        MessageHelper.ShowError(this, "ShipWorks was unable to create your ShipWorks account:\n\n" + ex.Message);

                        e.NextPage = CurrentPage;
                        return;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If we aren't a freemium edition, we know we are creating a freemium account, b\c we wouldn't be here asking for credit card info in the case of DAZzle
            // For freemium edition, we DO ask for credit card either way, so in that case we only create the ELS account if "new account" is checked
            if (freemiumEdition == null || radioNewAccount.Checked)
            {
                try
                {

                    if (account == null)
                    {
                        account = new EndiciaAccountEntity();
                        account.EndiciaReseller = (int)EndiciaReseller.None;
                    }

                    EndiciaApiAccount.Signup(
                        account,
                        freemiumEdition != null ? EndiciaAccountType.Freemium : (radioPlanPremium.Checked ? EndiciaAccountType.Premium : EndiciaAccountType.Standard),
                        accountAddress,
                        internetPassword.Text,
                        softwarePassword.Text,
                        challengeQuestion.Text.Trim(),
                        challengeAnswer.Text.Trim(),
                        paymentInfo);

                    // For freemium, we are now OK to create the freemium store
                    if (freemiumEdition != null)
                    {
                        TangoWebClient.CreateFreemiumStore(freemiumEdition.Store, accountAddress, paymentInfo, "", false);
                    }
                }
                catch (EndiciaException ex)
                {
                    MessageHelper.ShowError(this, ex.Message);

                    e.NextPage = CurrentPage;
                    return;
                }
            }
            // For freemium\existing we don't create the freemium store here - we wait until we get their existing account#
            else
            {
                e.NextPage = wizardPageExisting;
            }
        }

        /// <summary>
        /// Get the payment as it is entered in the Credit Card UI
        /// </summary>
        private EndiciaPaymentInfo GetPaymentInfo()
        {
            PersonAdapter billingAddress = new PersonAdapter();
            personCreditCard.SaveToEntity(billingAddress);

            EndiciaPaymentInfo paymentInfo = new EndiciaPaymentInfo();
            paymentInfo.CardBillingAddress = billingAddress;
            paymentInfo.CardType = (EndiciaCreditCardType) cardType.SelectedValue;
            paymentInfo.CardNumber = cardNumber.Text.Trim();
            paymentInfo.CardExpirationMonth = cardExpireMonth.SelectedIndex + 1;
            paymentInfo.CardExpirationYear = cardExpireYear.SelectedIndex + 2009;
            paymentInfo.UseCheckingForPostage = postagePaymentCheck.Checked;
            paymentInfo.CheckingAccount = checkingAccount.Text.Trim();
            paymentInfo.CheckingRouting = checkingRouting.Text.Trim();

            return paymentInfo;
        }

        /// <summary>
        /// Stepping into the page to enter the newly signed up for account number
        /// </summary>
        private void OnSteppingIntoAccount(object sender, WizardSteppingIntoEventArgs e)
        {
            BackEnabled = false;

            accountNumber.Enabled = !signupCompleted;
        }

        /// <summary>
        /// Checks to see if the given account number is allowed based on the edition of ShipWorks
        /// </summary>
        private bool AccountAllowed(string endiciaAccountNumber)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return lifetimeScope.Resolve<ILicenseService>()
                        .HandleRestriction(EditionFeature.EndiciaAccountNumber, endiciaAccountNumber, this);
            }
        }

        /// <summary>
        /// Stepping next from the account page
        /// </summary>
        private void OnStepNextAccount(object sender, WizardStepEventArgs e)
        {
            if (signupCompleted)
            {
                return;
            }

            if (accountNumber.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter the account number you receive from Endicia.");
                e.NextPage = CurrentPage;
                return;
            }

            if (!AccountAllowed(accountNumber.Text.Trim()))
            {
                e.NextPage = CurrentPage;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                account.AccountNumber = accountNumber.Text.Trim();
                account.Description = EndiciaAccountManager.GetDefaultDescription(account);

                if (!account.TestAccount)
                {
                    // This is required to activate the account
                    account.ApiUserPassword = endiciaApiClient.ChangeApiPassphrase(
                        account.AccountNumber,
                        (EndiciaReseller) account.EndiciaReseller,
                        SecureText.Decrypt(account.ApiInitialPassword, "Endicia"),
                        SecureText.Decrypt(account.ApiUserPassword, "Endicia"));
                }

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.SaveAndRefetch(account);
                }

                // For freemium, we have to now permanently associate this account with the store
                if (freemiumEdition != null)
                {
                    try
                    {
                        TangoWebClient.SetFreemiumAccountNumber(freemiumEdition.Store, account.AccountNumber);
                    }
                    catch (Exception ex)
                    {
                        MessageHelper.ShowError(this, "ShipWorks was unable to update your Endicia account information with your ShipWorks account.\n\n" + ex.Message);

                        e.NextPage = CurrentPage;
                        return;
                    }
                }

                signupCompleted = true;
            }
            catch (EndiciaException ex)
            {
                account.AccountNumber = null;
                account.Description = "";

                MessageHelper.ShowError(this, "ShipWorks was not able to activate your account. Please check that the account number you entered is correct.\n\nError: " + ex.Message);

                e.NextPage = CurrentPage;
                return;
            }
        }

        /// <summary>
        /// Stepping into the page to create an existing account
        /// </summary>
        private void OnSteppingIntoExisting(object sender, WizardSteppingIntoEventArgs e)
        {
            if (!radioExistingAccount.Checked)
            {
                e.Skip = true;
            }

            if (signupCompleted)
            {
                BackEnabled = false;
                accountExisting.Enabled = false;
                passwordExisting.Enabled = false;
            }
            else
            {
                if (freemiumEdition != null && !string.IsNullOrWhiteSpace(freemiumEdition.AccountNumber))
                {
                    accountExisting.Text = freemiumEdition.AccountNumber;
                    accountExisting.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Stepping next from the existing account page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNextExisting(object sender, WizardStepEventArgs e)
        {
            if (signupCompleted)
            {
                return;
            }

            if (accountExisting.Text.Trim().Length == 0)
            {
                MessageHelper.ShowInformation(this, "Please enter your Endicia account number.");
                e.NextPage = CurrentPage;
                return;
            }
            
            // Edition check
            if (!AccountAllowed(accountExisting.Text.Trim()))
            {
                e.NextPage = CurrentPage;
                return;
            }
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                account = new EndiciaAccountEntity()
                {
                    AccountNumber = accountExisting.Text.Trim(),
                    ApiUserPassword = SecureText.Encrypt(passwordExisting.Text, "Endicia"),
                    CreatedByShipWorks = false,
                    EndiciaReseller = (int)EndiciaReseller.None,

                    // Stuff that only applies when signing up
                    SignupConfirmation = "",
                    WebPassword = "",
                    ApiInitialPassword = "",

                    // Account type stuff
                    AccountType = -1,
                    TestAccount = false,
                    ScanFormAddressSource = (int)EndiciaScanFormAddressSource.Provider
                };
                
                // Address
                personControl.SaveToEntity(new PersonAdapter(account, ""));
                account.MailingPostalCode = account.PostalCode;

                // Description
                account.Description = EndiciaAccountManager.GetDefaultDescription(account);

                // This is our first time knowing about the account - which means we still need to do the ChangePassword thing
                if (freemiumEdition != null && freemiumEdition.AccountType == FreemiumAccountType.LabelServer && string.IsNullOrWhiteSpace(freemiumEdition.AccountNumber) )
                {
                    account.TestAccount = EndiciaApiClient.UseTestServer;

                    // change the password if this isn't a Test account
                    if (!account.TestAccount)
                    {
                        // This is required to activate the account
                        string oldPassword = passwordExisting.Text + "_initial";
                        string newPassword = passwordExisting.Text;

                        account.ApiUserPassword = endiciaApiClient.ChangeApiPassphrase(account.AccountNumber, (EndiciaReseller) account.EndiciaReseller, oldPassword, newPassword);
                    }
                }

                // See if we can connect
                endiciaApiClient.GetAccountStatus(account);

                // Freemium flow...
                if (freemiumEdition != null)
                {
                    try
                    {
                        // If its LabelServer store in tango already exists - then if its not yet associated w/ the account# do it now
                        if (freemiumEdition.AccountType == FreemiumAccountType.LabelServer)
                        {
                            if (string.IsNullOrWhiteSpace(freemiumEdition.AccountNumber))
                            {
                                TangoWebClient.SetFreemiumAccountNumber(freemiumEdition.Store, account.AccountNumber);
                            }
                        }
                        // Otherwise then if there is NO store yet in tango, we know we are creating a DAZzle freemium account for the first time
                        else if (freemiumEdition.AccountType == FreemiumAccountType.None)
                        {
                            PersonAdapter accountAddress = new PersonAdapter();
                            personControl.SaveToEntity(accountAddress);

                            TangoWebClient.CreateFreemiumStore(freemiumEdition.Store, accountAddress, GetPaymentInfo(), account.AccountNumber, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is TangoException || ex is ShipWorksLicenseException)
                        {
                            string message = (freemiumEdition.AccountType != FreemiumAccountType.None) ?
                                "ShipWorks was unable to update your ShipWorks account with your Endicia account number." :
                                "ShipWorks was unable to create your ShipWorks account.";

                            MessageHelper.ShowError(this, message + "\n\n" + ex.Message);

                            // stay on this page
                            e.NextPage = CurrentPage;
                            return;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                // Save
                EndiciaAccountManager.SaveAccount(account);

                signupCompleted = true;
            }
            catch (EndiciaException ex)
            {
                MessageHelper.ShowError(this, "ShipWorks was unable to communicate with Endicia using this account information:\n\n" + ex.Message);
                e.NextPage = CurrentPage;

                account = null;
            }
        }

        /// <summary>
        /// Stepping next from the options page
        /// </summary>
        private void OnStepNextOptions(object sender, WizardStepEventArgs e)
        {
            var settings = ShippingSettings.Fetch();
            optionsControl.SaveSettings(settings);
            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Buy postage for the account
        /// </summary>
        private void OnBuyPostage(object sender, EventArgs e)
        {
            using (EndiciaBuyPostageDlg dlg = new EndiciaBuyPostageDlg(account))
            {
                dlg.ShowDialog(this);
            }
        }

        /// <summary>
        /// Wizard is closing
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            // Make sure any pending changes have been saved
            EndiciaAccountManager.SaveAccount(account);

            // Mark the new account as configured
            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.Endicia);
            
            // If this is the only account, update this shipment type profiles with this account
            List<EndiciaAccountEntity> accounts = EndiciaAccountManager.GetAccounts((EndiciaReseller)account.EndiciaReseller, false);
            if (accounts.Count == 1)
            {
                EndiciaAccountEntity accountEntity = accounts.First();

                // Update any profiles to use this account if this is the only account
                // in the system. This is to account for the situation where there a multiple
                // profiles that may be associated with a previous account that has since
                // been deleted. 
                foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType  == (int)ShipmentTypeCode.Endicia))
                {
                    if (shippingProfileEntity.Postal.Endicia.EndiciaAccountID.HasValue)
                    {
                        shippingProfileEntity.Postal.Endicia.EndiciaAccountID = accountEntity.EndiciaAccountID;
                        ShippingProfileManager.SaveProfile(shippingProfileEntity); 
                    }
                }
            }

            // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
            // that a new account has been added.
            RateCache.Instance.Clear();
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && account != null && !account.IsNew)
            {
                EndiciaAccountManager.DeleteAccount(account);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();
            }
        }
    }
}
