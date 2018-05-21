using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Editions.Freemium;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Account;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Stores;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Wizard Form for setting up shipping with endicia
    /// </summary>
    [KeyedComponent(typeof(IShipmentTypeSetupWizard), ShipmentTypeCode.Endicia)]
    public partial class EndiciaSetupWizard : WizardForm, IShipmentTypeSetupWizard
    {
        private EndiciaAccountEntity account;

        private readonly IEndiciaApiClient endiciaApiClient = new EndiciaApiClient();
        // User has completed the signup process for the account
        private bool signupCompleted = false;

        // Will be non-null if we are in freemium signup mode
        private FreemiumFreeEdition freemiumEdition = null;
        private bool planTypeWasShown = false;

        private readonly int currentYear;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaSetupWizard()
        {
            InitializeComponent();

            currentYear = DateTime.Now.Year;
            cardExpireYear.DataSource = Enumerable.Range(currentYear, 10).ToList();

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

            // If there is an account that they started setting up but didn't fully complete the process (and we're not just specifically updating some other account)
            if (accounts.Any(a => a.AccountNumber == null))
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
                if (accounts.Count > 0 && accounts.All(a => a.AccountNumber != null))
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
        /// Open the USPS privacy policy
        /// </summary>
        private void OnLinkUspsPrivacy(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://about.usps.com/who-we-are/privacy-policy/welcome.htm", this);
        }

        /// <summary>
        /// Open the postage privacy act window
        /// </summary>
        private void OnLinkPostagePrivacy(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://about.usps.com/handbooks/as353/as353apdx_054.htm", this);
        }

        /// <summary>
        /// Open the Endicia terms and conditions window
        /// </summary>
        private void OnLinkEndiciaTerms(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://www.endicia.com/policy/terms-and-conditions", this);
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
        private void OnStepNextAddress(object sender, WizardStepEventArgs e)
        {
            PersonAdapter person = new PersonAdapter();
            personControl.SaveToEntity(person);

            RequiredFieldChecker fieldChecker = new RequiredFieldChecker();
            fieldChecker.Check("Contact name", person.FirstName.Trim());
            fieldChecker.Check("Email address", person.Email.Trim());
            fieldChecker.Check("Phone number", person.Phone.Trim());
            fieldChecker.Check("Street address", person.Street1.Trim());
            fieldChecker.Check("City", person.City.Trim());
            fieldChecker.Check("Postal code", person.PostalCode.Trim());

            if (!fieldChecker.Validate(this))
            {
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
                    e.NextPage = radioExistingAccount.Checked ? wizardPagePayment : wizardPagePasswords;
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
        private void OnStepNextPayment(object sender, WizardStepEventArgs e)
        {
            if (!ValidatePaymentInfo(e))
            {                    
                e.NextPage = CurrentPage;
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            PersonAdapter accountAddress = new PersonAdapter();
            personControl.SaveToEntity(accountAddress);

            EndiciaPaymentInfo paymentInfo = GetPaymentInfo();

            // If we are signing up for ELS freemium, first validate the account information _would_ be ok for Tango - this doesn't actually create anything
            if (freemiumEdition != null && radioNewAccount.Checked)
            {
                try
                {
                    TangoWebClient.CreateFreemiumStore(freemiumEdition.Store, accountAddress, paymentInfo, "", true);
                }
                catch (Exception ex) when (ex is TangoException || ex is ShipWorksLicenseException)
                {
                    MessageHelper.ShowError( this, "ShipWorks was unable to create your ShipWorks account:\n\n" + ex.Message);

                    e.NextPage = CurrentPage;
                    return;
                }
            }

            // If we aren't a freemium edition, we know we are creating a freemium account, b\c we wouldn't be here asking for credit card info in the case of DAZzle
            // For freemium edition, we DO ask for credit card either way, so in that case we only create the ELS account if "new account" is checked
            if (freemiumEdition == null || radioNewAccount.Checked)
            {
                try
                {
                    CreateEndiciaAccount(accountAddress, paymentInfo);

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
                }
            }
            // For freemium\existing we don't create the freemium store here - we wait until we get their existing account#
            else
            {
                e.NextPage = wizardPageExisting;
            }
        }

        /// <summary>
        /// Creates an Endicia account using the payment information provided
        /// </summary>
        private void CreateEndiciaAccount(PersonAdapter accountAddress, EndiciaPaymentInfo paymentInfo)
        {
            if (account == null)
            {
                account = new EndiciaAccountEntity();
                account.EndiciaReseller = (int) EndiciaReseller.None;
                account.AcceptedFCMILetterWarning = false;
            }

            EndiciaNewAccountCredentials credentials = new EndiciaNewAccountCredentials(
                internetPassword.Text, softwarePassword.Text, challengeQuestion.Text.Trim(),
                challengeAnswer.Text.Trim());

            EndiciaAccountType accountType = freemiumEdition != null ? 
                EndiciaAccountType.Freemium : 
                (radioPlanPremium.Checked ? EndiciaAccountType.Premium : EndiciaAccountType.Standard);

            endiciaApiClient.Signup(account, accountType, accountAddress, credentials, paymentInfo);
        }

        /// <summary>
        /// Ensure all required payment fields are entered
        /// </summary> 
        /// <returns>True if all required payment fields have values, otherwise False</returns>
        private bool ValidatePaymentInfo(WizardStepEventArgs e)
        {
            PersonAdapter creditPerson = new PersonAdapter();
            personCreditCard.SaveToEntity(creditPerson);

            if (freemiumEdition != null && !shipworksFeesAgree.Checked)
            {
                MessageHelper.ShowInformation(this,
                    "You must agree that Interapptive can use your credit card to pay for insurance if you optionally choose to use ShipWorks Shipping Insurance.");

                e.NextPage = CurrentPage;
                // return here instead of setting isValid, because we want this message to be displayed seperately
                return false;
            }
            
            RequiredFieldChecker fieldChecker = new RequiredFieldChecker();
            fieldChecker.Check("Street address", creditPerson.Street1.Trim());
            fieldChecker.Check("City", creditPerson.City.Trim());
            fieldChecker.Check("Postal code", creditPerson.PostalCode.Trim());
            fieldChecker.Check("Credit card Number", cardNumber.Text.Trim());
            fieldChecker.Check("CVV", cvv.Text.Trim());

            if (postagePaymentCheck.Checked)
            {
                fieldChecker.Check("Checking account number", checkingAccount.Text.Trim());
                fieldChecker.Check("Checking routing number", checkingRouting.Text.Trim());
            }

            return fieldChecker.Validate(this);
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
            paymentInfo.CardExpirationYear = cardExpireYear.SelectedIndex + currentYear;
            paymentInfo.CVV = cvv.Text.Trim();
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

                // This is required to activate the account
                account.ApiUserPassword = endiciaApiClient.ChangeApiPassphrase(
                    account.AccountNumber,
                    (EndiciaReseller) account.EndiciaReseller,
                    SecureText.Decrypt(account.ApiInitialPassword, "Endicia"),
                    SecureText.Decrypt(account.ApiUserPassword, "Endicia"));

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
                    EndiciaReseller = (int) EndiciaReseller.None,

                    // Stuff that only applies when signing up
                    SignupConfirmation = "",
                    WebPassword = "",
                    ApiInitialPassword = "",

                    // Account type stuff
                    AccountType = -1,
                    TestAccount = false,
                    ScanFormAddressSource = (int) EndiciaScanFormAddressSource.Provider,
                    AcceptedFCMILetterWarning = false
                };

                // Address
                personControl.SaveToEntity(new PersonAdapter(account, ""));
                account.MailingPostalCode = account.PostalCode;

                // Description
                account.Description = EndiciaAccountManager.GetDefaultDescription(account);

                // This is our first time knowing about the account - which means we still need to do the ChangePassword thing
                if (freemiumEdition != null && freemiumEdition.AccountType == FreemiumAccountType.LabelServer && string.IsNullOrWhiteSpace(freemiumEdition.AccountNumber))
                {
                    account.TestAccount = EndiciaApiClient.UseTestServer;

                    // This is required to activate the account
                    string oldPassword = passwordExisting.Text + "_initial";
                    string newPassword = passwordExisting.Text;
                    account.ApiUserPassword = endiciaApiClient.ChangeApiPassphrase(account.AccountNumber, (EndiciaReseller) account.EndiciaReseller, oldPassword, newPassword);
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
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                EndiciaBuyPostageDlg dlg = lifetimeScope.Resolve<EndiciaBuyPostageDlg>();
                dlg.ShowDialog(this, account);
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
            List<EndiciaAccountEntity> accounts = EndiciaAccountManager.GetAccounts((EndiciaReseller) account.EndiciaReseller, false);
            if (accounts.Count == 1)
            {
                EndiciaAccountEntity accountEntity = accounts.First();

                // Update any profiles to use this account if this is the only account
                // in the system. This is to account for the situation where there a multiple
                // profiles that may be associated with a previous account that has since
                // been deleted.
                foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == ShipmentTypeCode.Endicia))
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

        /// <summary>
        /// Gets the wizard without any wrapping wizards
        /// </summary>
        public IShipmentTypeSetupWizard GetUnwrappedWizard() => this;
    }
}
