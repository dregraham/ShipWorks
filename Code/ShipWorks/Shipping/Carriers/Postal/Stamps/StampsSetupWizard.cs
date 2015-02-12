using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Settings.WizardPages;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using Interapptive.Shared.Business;
using ShipWorks.Data.Controls;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Setup wizard for processing shipments with Stamps.com
    /// </summary>
    public partial class StampsSetupWizard : ShipmentTypeSetupWizardForm
    {
        private readonly StampsRegistration stampsRegistration;
        private readonly IEnumerable<PostalAccountRegistrationType> availableRegistrationTypes;
        private readonly ShipmentTypeCode shipmentTypeCode;

        bool registrationComplete = false;
        private readonly bool allowRegisteringExistingAccount;
        private int initialPersonControlHeight;


        /// <summary>
        /// Initializes a new instance of the <see cref="StampsSetupWizard"/> class.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        /// <param name="allowRegisteringExistingAccount">if set to <c>true</c> [allow registering existing account].</param>
        public StampsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount)
            : this (promotion, allowRegisteringExistingAccount, ShipmentTypeCode.Stamps)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsSetupWizard"/> class.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        /// <param name="allowRegisteringExistingAccount">if set to <c>true</c> [allow registering an existing account].</param>
        /// <param name="shipmentTypeCode">The shipment type code.</param>
        /// <exception cref="StampsRegistrationException">There weren't any registration types provided to the Stamps.com setup wizard.</exception>
        protected StampsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount, ShipmentTypeCode shipmentTypeCode)
        {
            InitializeComponent();

            initialPersonControlHeight = personControl.Height;

            this.shipmentTypeCode = shipmentTypeCode;
            StampsResellerType resellerType = shipmentTypeCode == ShipmentTypeCode.Stamps ? StampsResellerType.None : StampsResellerType.StampsExpedited;

            // Load up a registration object using the stamps validator and the gateway to 
            // the stamps.com API
            stampsRegistration = new StampsRegistration(new StampsRegistrationValidator(), new StampsRegistrationGateway(resellerType), promotion);
            this.allowRegisteringExistingAccount = allowRegisteringExistingAccount;
            availableRegistrationTypes = promotion.AvailableRegistrationTypes;

            if (!availableRegistrationTypes.Any())
            {
                throw new StampsRegistrationException("There weren't any registration types provided to the Stamps.com setup wizard.");
            }

            if (promotion.IsMonthlyFeeWaived)
            {
                RemoveMonthlyFeeText();
            }

            // Set the shipment type is set correctly (could be USPS or Stamps.com), so the 
            // label type gets persisted to the correct profile
            optionsControl.ShipmentTypeCode = this.shipmentTypeCode;
        }

        /// <summary>
        /// Gets the stamps account.
        /// </summary>
        protected UspsAccountEntity UspsAccount { get; private set; }

        /// <summary>
        /// Gets the person control associated with the Stamps.com account.
        /// </summary>
        protected AutofillPersonControl PersonControl
        {
            get { return personControl; }
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected virtual void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipmentTypeCode);

            optionsControl.LoadSettings();

            // Hide the panel that lets the customer select to register a new account or use an existing account
            // until Stamps.com has enabled ShipWorks to register new accounts
            accountTypePanel.Visible = allowRegisteringExistingAccount;
            
            if (!allowRegisteringExistingAccount)
            {
                // Registering an existing account is not allowed, so choose new account (since the options have
                // been hidden from the user)
                radioNewAccount.Checked = true;
                radioExistingAccount.Checked = false;
            }

            Pages.Add(new ShippingWizardPageOrigin(shipmentType));
            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(new ShippingWizardPageFinish(shipmentType));

            if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Stamps))
            {
                Pages.Remove(wizardPageOptions);
            }
            else
            {
                wizardPageOptions.StepNext += OnPageOptionsStepNext;
            }

            // Set default values on the stamps account and load the person control. Now the uspsAccount will
            // can be referred to throughout the wizard via the personControl
            UspsAccount = new UspsAccountEntity { CountryCode = "US", ContractType = (int)StampsAccountContractType.Unknown };
            UspsAccount.InitializeNullsToDefault();

            personControl.LoadEntity(new PersonAdapter(UspsAccount, string.Empty));

            stampsUsageType.Items.Add(new StampsAccountUsageDropdownItem(AccountType.Individual, "Individual"));
            stampsUsageType.Items.Add(new StampsAccountUsageDropdownItem(AccountType.HomeOffice, "Home Office"));
            stampsUsageType.Items.Add(new StampsAccountUsageDropdownItem(AccountType.HomeBasedBusiness, "Home-based Business"));
            stampsUsageType.Items.Add(new StampsAccountUsageDropdownItem(AccountType.OfficeBasedBusiness, "Office-based Business"));
            stampsUsageType.SelectedIndex = 0;

            LoadAccountRegistrationTypes();
        }

        private void LoadAccountRegistrationTypes()
        {
            stampsAccountRegistrationType.Items.Clear();
            foreach (PostalAccountRegistrationType type in availableRegistrationTypes)
            {
                stampsAccountRegistrationType.Items.Add(new StampsRegistrationTypeDropdownItem(type, EnumHelper.GetDescription(type)));
            }

            if (stampsAccountRegistrationType.Items.Count == 1)
            {
                int adjustedHeight = stampsUsageType.Top - stampsAccountRegistrationType.Top;

                // Don't show the registration drop down if there's only one item available
                // and move up the other controls accordingly
                stampsAccountRegistrationType.Visible = false;
                stampsUsageType.Top = stampsAccountRegistrationType.Top;
                
                labelUsageType.Top = labelAccountType.Top;
                labelAccountType.Visible = false;

                panelAccountType.Height -= adjustedHeight;
                personControl.Top -= adjustedHeight;
                panelTerms.Top -= adjustedHeight;
            }

            // Default the selection to the first item in the list (in case it's hidden)
            stampsAccountRegistrationType.SelectedIndex = 0;
        }
        

        /// <summary>
        /// User clicked the link to open the stamps.com website
        /// </summary>
        private void OnLinkStampsWebsite(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.stamps.com", this);
        }

        /// <summary>
        /// User clicked the link to view the Stamps.com terms and conditions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnLinkStampsTermsConditions(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.stamps.com/conditions/terms.html", this);
        }
        
        /// <summary>
        /// Open the Stamps.com privacy policy
        /// </summary>
        private void OnLinkStampsPrivacyPolicy(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.stamps.com/privacy-policy/", this);
        }

        /// <summary>
        /// User clicked the link to view the Stamps.com special offer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLinkStampsSpecialOffer(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.stamps.com/img/offer/Pro1599_GoldShipper_50offscale_50offprinter_0pwk_1/webreg2-learn-more.html", this);
        }

        /// <summary>
        /// Stepping into the account address page
        /// </summary>
        private void OnSteppingIntoAccountAddress(object sender, WizardSteppingIntoEventArgs e)
        {
            panelAccountType.Visible = radioNewAccount.Checked;
            personControl.Top = radioNewAccount.Checked ? panelAccountType.Bottom : panelAccountType.Top;

            panelTerms.Visible = radioNewAccount.Checked;
            panelTerms.Top = personControl.Bottom + 4;
        }

        /// <summary>
        /// Called when [step next account address].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ShipWorks.UI.Wizard.WizardStepEventArgs"/> instance containing the event data.</param>
        private void OnStepNextAccountAddress(object sender, WizardStepEventArgs e)
        {
            // Save the data entered in the person control back to our uspsAccount
            PersonAdapter updatedStampsAccountAdapter = new PersonAdapter(UspsAccount, string.Empty);
            personControl.SaveToEntity(updatedStampsAccountAdapter);

            if (UspsAccount.CountryCode != "US")
            {
                MessageHelper.ShowInformation(this, "USPS only supports US addresses.");
                e.NextPage = CurrentPage;
                return;
            }

            RequiredFieldChecker checker = new RequiredFieldChecker();
            checker.Check("Full Name", UspsAccount.FirstName);
            checker.Check("Street Address", UspsAccount.Street1);
            checker.Check("City", UspsAccount.City);
            checker.Check("State", UspsAccount.StateProvCode);
            checker.Check("Postal Code", UspsAccount.PostalCode);
            checker.Check("Phone", UspsAccount.Phone);
            checker.Check("Email", UspsAccount.Email);

            if (HasAcceptedTermsConditions() && checker.Validate(this))
            {
                // We have the necessary information, so update our stamps.com registration
                stampsRegistration.UsageType = ((StampsAccountUsageDropdownItem)stampsUsageType.SelectedItem).AccountType;

                StampsRegistrationTypeDropdownItem selectedStampsRegistrationTypeDropdownItem = (StampsRegistrationTypeDropdownItem) stampsAccountRegistrationType.SelectedItem;
                stampsRegistration.RegistrationType = selectedStampsRegistrationTypeDropdownItem.RegistrationType;

                stampsRegistration.PhysicalAddress.FirstName = UspsAccount.FirstName;
                stampsRegistration.PhysicalAddress.LastName = UspsAccount.LastName;
                stampsRegistration.PhysicalAddress.Company = UspsAccount.Company;

                stampsRegistration.PhysicalAddress.PhoneNumber = UspsAccount.Phone;
                stampsRegistration.Email = UspsAccount.Email;

                stampsRegistration.PhysicalAddress.Address1 = UspsAccount.Street1;
                stampsRegistration.PhysicalAddress.Address2 = UspsAccount.Street2;
                stampsRegistration.PhysicalAddress.City = UspsAccount.City;
                stampsRegistration.PhysicalAddress.State = Geography.GetStateProvCode(UspsAccount.StateProvCode);
                stampsRegistration.PhysicalAddress.Country = Geography.GetCountryCode(UspsAccount.CountryCode);

                if (PostalUtility.IsDomesticCountry(stampsRegistration.PhysicalAddress.Country))
                {
                    // stamps.com inspects the ZIP code for US addresses
                    stampsRegistration.PhysicalAddress.ZIPCode = UspsAccount.PostalCode;
                }
                else
                {
                    // stamps.com inspects the postal code for international addresses
                    stampsRegistration.PhysicalAddress.PostalCode = UspsAccount.PostalCode;
                }

                // Determine which wizard page to go next
                if (radioExistingAccount.Checked)
                {
                     // The user has opted to use an existing stamps account, so skip to the
                     // existing credentials page for entering a username/password
                    e.NextPage = wizardPageExistingAccountCredentials;
                }
            }
            else
            {
                // Validation failed
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Called when [step next registration credentials].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ShipWorks.UI.Wizard.WizardStepEventArgs"/> instance containing the event data.</param>
        private void OnStepNextRegistrationCredentials(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            IEnumerable<RegistrationValidationError> validationErrors = stampsRegistrationSecuritySettingsControl.ValidateRegistrationSettings();
            
            if (validationErrors.Count() > 0)
            {
                string validationMessage = "ShipWorks cannot create a Stamps.com account with the information provided. Stamps.com requires that the following field(s) be corrected:"
                    + System.Environment.NewLine + System.Environment.NewLine;

                validationErrors.ToList().ForEach(v => validationMessage += "\t" + v.Message + System.Environment.NewLine);

                MessageHelper.ShowInformation(this, validationMessage);
                e.NextPage = CurrentPage;
            }
            else
            {
                // The data passed validation, so we can update the stamps registration with the data provided
                // and move to the next step in teh wizard
                stampsRegistration.UserName = stampsRegistrationSecuritySettingsControl.Username;
                stampsRegistration.Password = stampsRegistrationSecuritySettingsControl.Password;

                stampsRegistration.FirstCodewordType = stampsRegistrationSecuritySettingsControl.FirstSecurityQuestionType;
                stampsRegistration.FirstCodewordValue = stampsRegistrationSecuritySettingsControl.FirstSecurityQuestionAnswer;

                stampsRegistration.SecondCodewordType = stampsRegistrationSecuritySettingsControl.SecondSecurityQuestionType;
                stampsRegistration.SecondCodewordValue = stampsRegistrationSecuritySettingsControl.SecondSecurityQuestionAnswer;
            }
        }

        /// <summary>
        /// Called when [step next new account payment].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ShipWorks.UI.Wizard.WizardStepEventArgs"/> instance containing the event data.</param>
        private void OnStepNextNewAccountPayment(object sender, WizardStepEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (stampsPaymentControl.ValidatePaymentData())
                {
                    // We now have all the user-supplied data needed to register the account with Stamps.com
                    if (stampsPaymentControl.CreditCard != null)
                    {
                        stampsRegistration.CreditCard = stampsPaymentControl.CreditCard;

                        string cardholder = stampsRegistration.CreditCard.BillingAddress.FullName;
                        stampsRegistration.CreditCard.BillingAddress = stampsRegistration.PhysicalAddress;
                        stampsRegistration.CreditCard.BillingAddress.FirstName = "";
                        stampsRegistration.CreditCard.BillingAddress.LastName = "";
                        stampsRegistration.CreditCard.BillingAddress.FullName = cardholder;
                    }
                    else
                    {
                        stampsRegistration.AchAccount = stampsPaymentControl.BankAccount;
                    }
                                        
                    stampsRegistration.Submit();

                    // Save the stamps account now that it has been succesfully created
                    SaveStampsAccount(stampsRegistration.UserName, SecureText.Encrypt(stampsRegistration.Password, stampsRegistration.UserName));

                    registrationComplete = true;
                }
                else
                {
                    // The payment info provided could not be validated, so stay on the sasme page
                    e.NextPage = CurrentPage;
                }
            }
            catch (StampsRegistrationException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Determines whether [has accepted terms conditions].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [has accepted terms conditions]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasAcceptedTermsConditions()
        {
            if (radioNewAccount.Checked && !termsCheckBox.Checked)
            {
                MessageHelper.ShowInformation(this, "You must accept the terms and conditions.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Stepping into the options page
        /// </summary>
        private void OnSteppingIntoOptions(object sender, WizardSteppingIntoEventArgs e)
        {
            if (registrationComplete)
            {
                BackEnabled = false;
            }
        }

        /// <summary>
        /// Called when [stepping into credentials].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs"/> instance containing the event data.</param>
        private void OnSteppingIntoExistingCredentials(object sender, WizardSteppingIntoEventArgs e)
        {
            // account registration is enabled (i.e. stamps has allowed ShipWorks to register accounts)
            if (radioNewAccount.Checked)
            {
                // The customer opted to create a new stamps.com account, so we'll skip the page 
                // asking for existing credentials
                e.Skip = true;
                BackEnabled = false;
            }
        }


        /// <summary>
        /// Stepping next from the credentials page
        /// </summary>
        private void OnStepNextExistingCredentials(object sender, WizardStepEventArgs e)
        {
            string userID = username.Text.Trim();
            if (userID.Length == 0)
            {
                MessageHelper.ShowMessage(this, "Enter your Stamps.com username.");
                e.NextPage = CurrentPage;

                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                new StampsWebClient(StampsResellerType.None).AuthenticateUser(userID, password.Text);

                if (UspsAccount == null)
                {
                    UspsAccount = new UspsAccountEntity { ContractType = (int)StampsAccountContractType.Unknown };
                }

                SaveStampsAccount(userID, SecureText.Encrypt(password.Text, userID));
            }
            catch (StampsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Prepares the stamps account for save. This is just a hook to allow derived
        /// classes a chance to manipulate the account prior to it being persisted.
        /// </summary>
        protected virtual void PrepareStampsAccountForSave()
        {
            // Do nothing - just a hook for derived classes
        }

        /// <summary>
        /// Saves the stamps accountand initializes the stamps info control.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="encryptedPassword">The encrypted password.</param>
        protected virtual void SaveStampsAccount(string username, string encryptedPassword)
        {
            PrepareStampsAccountForSave();

            UspsAccount.Username = username;
            UspsAccount.Password = encryptedPassword;

            // Save the stamps account and use it to initialize the stamps info control
            StampsAccountManager.SaveAccount(UspsAccount);

            // Update the account contract type
            StampsShipmentType stampsShipmentType = PostalUtility.GetStampsShipmentTypeForStampsResellerType((StampsResellerType)UspsAccount.UspsReseller);
            stampsShipmentType.UpdateContractType(UspsAccount);
        }

        /// <summary>
        /// Stepping into the account info screen
        /// </summary>
        private void OnSteppingIntoAccountInfo(object sender, WizardSteppingIntoEventArgs e)
        {
            stampsAccountInfo.Initialize(UspsAccount);
        }

        /// <summary>
        /// Wizard has just stepped out of the options page
        /// </summary>
        private void OnPageOptionsStepNext(object sender, WizardStepEventArgs e)
        {
            var settings = ShippingSettings.Fetch();
            optionsControl.SaveSettings(settings);
            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        protected virtual void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && UspsAccount != null && !UspsAccount.IsNew)
            {
                StampsAccountManager.DeleteAccount(UspsAccount);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();

                StampsShipmentType shipmentType = (StampsShipmentType)ShipmentTypeManager.GetType(shipmentTypeCode);

                // If this is the only account, update this shipment type profiles with this account
                List<UspsAccountEntity> accounts = shipmentType.AccountRepository.Accounts.ToList();
                if (accounts.Count == 1)
                {
                    UspsAccountEntity accountEntity = accounts.First();

                    // Update any profiles to use this account if this is the only account
                    // in the system. This is to account for the situation where there a multiple
                    // profiles that may be associated with a previous account that has since
                    // been deleted. 
                    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)shipmentTypeCode))
                    {
                        if (shippingProfileEntity.Postal.Usps.UspsAccountID.HasValue)
                        {
                            shippingProfileEntity.Postal.Usps.UspsAccountID = accountEntity.UspsAccountID;
                            ShippingProfileManager.SaveProfile(shippingProfileEntity);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes the text about the $15.99 monthly fee for an account. This is intended for those users that
        /// may be signing up from that only have an Express1 account.
        /// </summary>
        protected void RemoveMonthlyFeeText()
        {
            stampsPaymentControl.RemoveMonthlyFeeText();
        }

        /// <summary>
        /// Handle when the person control resizes
        /// </summary>
        private void OnPersonControlResize(object sender, EventArgs e)
        {
            panelTerms.Top = panelTerms.Top - (initialPersonControlHeight - personControl.Height);
        }
    }
}
