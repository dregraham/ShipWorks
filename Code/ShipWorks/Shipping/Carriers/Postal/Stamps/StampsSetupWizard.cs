using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Setup wizard for processing shipments with Stamps.com
    /// </summary>
    public partial class StampsSetupWizard : ShipmentTypeSetupWizardForm
    {
        private StampsAccountEntity stampsAccount;
        private readonly StampsRegistration stampsRegistration;
        private readonly IEnumerable<PostalAccountRegistrationType> availableRegistrationTypes;

        bool registrationComplete = false;
        private readonly bool allowRegisteringExistingAccount;


        /// <summary>
        /// Initializes a new instance of the <see cref="StampsSetupWizard"/> class.
        /// </summary>
        /// <param name="promotion">The promotion.</param>
        /// <param name="allowRegisteringExistingAccount">if set to <c>true</c> [allow registering existing account].</param>
        public StampsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount)
        {
            InitializeComponent();

            // Load up a registration object using the stamps validator and the gateway to 
            // the stamps.com API
            stampsRegistration = new StampsRegistration(new StampsRegistrationValidator(), new StampsRegistrationGateway(), promotion);
            this.allowRegisteringExistingAccount = allowRegisteringExistingAccount;
            this.availableRegistrationTypes = promotion.AvailableAccountTypes;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Stamps);

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

            Pages[Pages.Count - 1].SteppingInto += new EventHandler<WizardSteppingIntoEventArgs>(OnSteppingIntoFinish);

            // Set default values on the stamps account and load the person control. Now the stampsAccount will
            // can be referred to throughout the wizard via the personControl
            stampsAccount = new StampsAccountEntity { CountryCode = "US" };
            stampsAccount.InitializeNullsToDefault();

            personControl.LoadEntity(new PersonAdapter(stampsAccount, string.Empty));

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
            WebHelper.OpenUrl("http://www.stamps.com/img/offer/Pro1799_5pa_1030wc_1060wc_1090wc_10120wc_scfree_wk35/webreg2-learn-more.html", this);
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
            // Save the data entered in the person control back to our stampsAccount
            personControl.SaveToEntity();

            RequiredFieldChecker checker = new RequiredFieldChecker();
            checker.Check("Full Name", stampsAccount.FirstName);
            checker.Check("Street Address", stampsAccount.Street1);
            checker.Check("City", stampsAccount.City);
            checker.Check("State", stampsAccount.StateProvCode);
            checker.Check("Postal Code", stampsAccount.PostalCode);
            checker.Check("Phone", stampsAccount.Phone);
            checker.Check("Email", stampsAccount.Email);

            if (HasAcceptedTermsConditions() && checker.Validate(this))
            {
                // We have the necessary information, so update our stamps.com registration
                stampsRegistration.UsageType = ((StampsAccountUsageDropdownItem)stampsUsageType.SelectedItem).AccountType;
                stampsRegistration.RegistrationType = (PostalAccountRegistrationType) stampsAccountRegistrationType.SelectedItem;

                stampsRegistration.PhysicalAddress.FirstName = stampsAccount.FirstName;
                stampsRegistration.PhysicalAddress.LastName = stampsAccount.LastName;
                stampsRegistration.PhysicalAddress.Company = stampsAccount.Company;

                stampsRegistration.PhysicalAddress.PhoneNumber = stampsAccount.Phone;
                stampsRegistration.Email = stampsAccount.Email;

                stampsRegistration.PhysicalAddress.Address1 = stampsAccount.Street1;
                stampsRegistration.PhysicalAddress.Address2 = stampsAccount.Street2;
                stampsRegistration.PhysicalAddress.City = stampsAccount.City;
                stampsRegistration.PhysicalAddress.State = Geography.GetStateProvCode(stampsAccount.StateProvCode);
                stampsRegistration.PhysicalAddress.Country = Geography.GetCountryCode(stampsAccount.CountryCode);

                if (PostalUtility.IsDomesticCountry(stampsRegistration.PhysicalAddress.Country))
                {
                    // stamps.com inspects the ZIP code for US addresses
                    stampsRegistration.PhysicalAddress.ZIPCode = stampsAccount.PostalCode;
                }
                else
                {
                    // stamps.com inspects the postal code for international addresses
                    stampsRegistration.PhysicalAddress.PostalCode = stampsAccount.PostalCode;
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

                new StampsApiSession().AuthenticateUser(userID, password.Text, false);

                if (stampsAccount == null)
                {
                    stampsAccount = new StampsAccountEntity();
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
        /// Saves the stamps accountand initializes the stamps info control.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="encryptedPassword">The encrypted password.</param>
        private void SaveStampsAccount(string username, string encryptedPassword)
        {
            stampsAccount.Username = username;
            stampsAccount.Password = encryptedPassword;

            // Save the stamps account and use it to initialize the stamps info control
            StampsAccountManager.SaveAccount(stampsAccount);
        }

        /// <summary>
        /// Stepping into the account info screen
        /// </summary>
        private void OnSteppingIntoAccountInfo(object sender, WizardSteppingIntoEventArgs e)
        {
            stampsAccountInfo.Initialize(stampsAccount);
        }

        /// <summary>
        /// Wizard is finishing
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            if (Pages.Contains(wizardPageOptions))
            {
                var settings = ShippingSettings.Fetch();

                optionsControl.SaveSettings(settings);
                ShippingSettings.Save(settings);
            }
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && stampsAccount != null && !stampsAccount.IsNew)
            {
                StampsAccountManager.DeleteAccount(stampsAccount);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();

                // If this is the only account, update this shipment type profiles with this account
                List<StampsAccountEntity> accounts = StampsAccountManager.GetAccounts(false);
                if (accounts.Count == 1)
                {
                    StampsAccountEntity accountEntity = accounts.First();

                    // Update any profiles to use this account if this is the only account
                    // in the system. This is to account for the situation where there a multiple
                    // profiles that may be associated with a previous account that has since
                    // been deleted. 
                    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)ShipmentTypeCode.Stamps))
                    {
                        if (shippingProfileEntity.Postal.Stamps.StampsAccountID.HasValue)
                        {
                            shippingProfileEntity.Postal.Stamps.StampsAccountID = accountEntity.StampsAccountID;
                            ShippingProfileManager.SaveProfile(shippingProfileEntity);
                        }
                    }
                }
            }
        }

    }
}
