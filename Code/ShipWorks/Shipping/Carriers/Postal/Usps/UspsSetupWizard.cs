using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Controls;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Setup wizard for processing shipments with USPS
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class UspsSetupWizard : ShipmentTypeSetupWizardForm
    {
        private readonly UspsRegistration uspsRegistration;
        private readonly ShipmentTypeCode shipmentTypeCode = ShipmentTypeCode.Usps;
        private readonly Dictionary<long, long> profileMap = new Dictionary<long, long>();

        bool registrationComplete;
        private readonly bool allowRegisteringExistingAccount;
        private readonly int initialPersonControlHeight;


        /// <summary>
        /// Initializes a new instance of the <see cref="UspsSetupWizard"/> class.
        /// </summary>
        public UspsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount):
            this(promotion, allowRegisteringExistingAccount, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsSetupWizard"/> class.
        /// </summary>
        public UspsSetupWizard(IRegistrationPromotion promotion, bool allowRegisteringExistingAccount, UspsAccountEntity uspsAccount)
        {
            UspsAccount = uspsAccount;

            InitializeComponent();

            initialPersonControlHeight = personControl.Height;

            UspsResellerType resellerType = UspsResellerType.None;

            // Load up a registration object using the USPS validator and the gateway to
            // the USPS API
            uspsRegistration = new UspsRegistration(new UspsRegistrationValidator(), new UspsRegistrationGateway(resellerType), promotion);
            this.allowRegisteringExistingAccount = allowRegisteringExistingAccount;

            if (promotion.IsMonthlyFeeWaived)
            {
                RemoveMonthlyFeeText();
            }

            // Set the shipment type is set correctly (could be USPS), so the
            // label type gets persisted to the correct profile
            optionsControl.ShipmentTypeCode = this.shipmentTypeCode;
        }

        /// <summary>
        /// Gets or sets the initial account address that to use when adding an account.
        /// </summary>
        public PersonAdapter InitialAccountAddress { get; set; }

        /// <summary>
        /// Gets the USPS account.
        /// </summary>
        public UspsAccountEntity UspsAccount { get; private set; }

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
            
            if (!allowRegisteringExistingAccount)
            {
                // Registering an existing account is not allowed, so choose new account (since the options have
                // been hidden from the user)
                radioNewAccount.Checked = true;
                radioExistingAccount.Checked = false;
            }
            else if (UspsAccount != null && UspsAccount.PendingInitialAccount)
            {
                radioNewAccount.Checked = false;
                radioExistingAccount.Checked = true;
            }

            Pages.Add(new ShippingWizardPageOrigin(shipmentType));
            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(new ShippingWizardPageFinish(shipmentType));

            if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Usps))
            {
                Pages.Remove(wizardPageOptions);
            }
            else
            {
                wizardPageOptions.StepNext += OnPageOptionsStepNext;
            }

            if (UspsAccount == null)
            {
                // Set default values on the USPS account and load the person control. Now the stampsAccount will
                // can be referred to throughout the wizard via the personControl
                UspsAccount = new UspsAccountEntity
                {
                    CountryCode = "US",
                    ContractType = (int) UspsAccountContractType.Unknown,
                    CreatedDate = DateTime.UtcNow
                };

                UspsAccount.InitializeNullsToDefault();

                personControl.LoadEntity(new PersonAdapter(UspsAccount, string.Empty));
            }

            // Hide the panel that lets the customer select to register a new account or use an existing account
            // until USPS has enabled ShipWorks to register new accounts
            accountTypePanel.Visible = allowRegisteringExistingAccount && !UspsAccount.PendingInitialAccount;

            uspsUsageType.Items.Add(new UspsAccountUsageDropdownItem(AccountType.Individual, "Individual"));
            uspsUsageType.Items.Add(new UspsAccountUsageDropdownItem(AccountType.HomeOffice, "Home Office"));
            uspsUsageType.Items.Add(new UspsAccountUsageDropdownItem(AccountType.HomeBasedBusiness, "Home-based Business"));
            uspsUsageType.Items.Add(new UspsAccountUsageDropdownItem(AccountType.OfficeBasedBusiness, "Office-based Business"));
            uspsUsageType.SelectedIndex = 0;

            CopyPostalRules();

            if (InitialAccountAddress != null)
            {
                // Pre-load the person control with our initial account address (in the event an account is being
                // created via the Activate Postage Discount dialog
                PersonControl.LoadEntity(InitialAccountAddress);
            }
        }

        /// <summary>
        /// A helper method to copy shipping rules from all other postal providers into the USPS shipment type.
        /// </summary>
        private void CopyPostalRules()
        {
            if (!ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Usps))
            {
                ClearExistingRulesAndProfiles();

                // Need to update any rules to swap out Endicia, Express1, and the original USPS
                // with USPS now that those types will no longer be active
                // once the account is added.
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Endicia);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.Express1Usps);
                UseUspsInDefaultShippingRulesFor(ShipmentTypeCode.PostalWebTools);
            }
        }

        /// <summary>
        /// User clicked the link to view the USPS terms and conditions.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnLinkUspsTermsConditions(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.stamps.com/conditions/terms.html", this);
        }

        /// <summary>
        /// Open the Stamps.com privacy policy
        /// </summary>
        private void OnLinkUspsPrivacyPolicy(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.stamps.com/privacy-policy/", this);
        }

        /// <summary>
        /// User clicked the link to view the USPS special offer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLinkUspsSpecialOffer(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.stamps.com/offerdetails/622/", this);
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
            PersonAdapter updatedUspsAccountAdapter = new PersonAdapter(UspsAccount, string.Empty);
            personControl.SaveToEntity(updatedUspsAccountAdapter);

            if (UspsAccount.CountryCode != "US")
            {
                MessageHelper.ShowInformation(this, "USPS only supports US addresses.");
                e.NextPage = CurrentPage;
                return;
            }

            if (HasAcceptedTermsConditions() && IsContactInfoComplete())
            {
                // We have the necessary information, so update our USPS registration
                uspsRegistration.UsageType = ((UspsAccountUsageDropdownItem)uspsUsageType.SelectedItem).AccountType;

                uspsRegistration.PhysicalAddress.FirstName = UspsAccount.FirstName;
                uspsRegistration.PhysicalAddress.LastName = UspsAccount.LastName;
                uspsRegistration.PhysicalAddress.Company = UspsAccount.Company;

                uspsRegistration.PhysicalAddress.PhoneNumber = UspsAccount.Phone;
                uspsRegistration.Email = UspsAccount.Email;

                uspsRegistration.PhysicalAddress.Address1 = UspsAccount.Street1;
                uspsRegistration.PhysicalAddress.Address2 = UspsAccount.Street2;
                uspsRegistration.PhysicalAddress.City = UspsAccount.City;
                uspsRegistration.PhysicalAddress.State = Geography.GetStateProvCode(UspsAccount.StateProvCode);
                uspsRegistration.PhysicalAddress.Country = Geography.GetCountryCode(UspsAccount.CountryCode);

                if (uspsRegistration.PhysicalAddress.AsAddressAdapter().IsDomesticCountry())
                {
                    // USPS inspects the ZIP code for US addresses
                    uspsRegistration.PhysicalAddress.ZIPCode = UspsAccount.PostalCode;
                }
                else
                {
                    // USPS inspects the postal code for international addresses
                    uspsRegistration.PhysicalAddress.PostalCode = UspsAccount.PostalCode;
                }

                // Determine which wizard page to go next
                if (radioExistingAccount.Checked)
                {
                    // The user has opted to use an existing USPS account, so skip to the
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
        /// A helper method to determine whether data for all the required contact info fields have been provided.
        /// </summary>
        /// <returns><c>true</c> if the [contact information is complete]; otherwise, <c>false</c>.</returns>
        private bool IsContactInfoComplete()
        {
            RequiredFieldChecker checker = new RequiredFieldChecker();

            checker.Check("Full Name", UspsAccount.FirstName);
            checker.Check("Street Address", UspsAccount.Street1);
            checker.Check("City", UspsAccount.City);
            checker.Check("State", UspsAccount.StateProvCode);
            checker.Check("Postal Code", UspsAccount.PostalCode);
            checker.Check("Phone", UspsAccount.Phone);
            checker.Check("Email", UspsAccount.Email);

            return checker.Validate(this);
        }

        /// <summary>
        /// Called when [step next registration credentials].
        /// </summary>
        private void OnStepNextRegistrationCredentials(object sender, WizardStepEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            IEnumerable<RegistrationValidationError> validationErrors = uspsRegistrationSecuritySettingsControl
                .ValidateRegistrationSettings()
                .ToList();

            if (validationErrors.Any())
            {
                string validationMessage = "ShipWorks cannot create a USPS account with the information provided. Stamps.com requires that the following field(s) be corrected:"
                    + System.Environment.NewLine + System.Environment.NewLine;

                validationErrors.ToList().ForEach(v => validationMessage += "\t" + v.Message + System.Environment.NewLine);

                MessageHelper.ShowInformation(this, validationMessage);
                e.NextPage = CurrentPage;
            }
            else
            {
                // The data passed validation, so we can update the USPS registration with the data provided
                // and move to the next step in teh wizard
                uspsRegistration.UserName = uspsRegistrationSecuritySettingsControl.Username;
                uspsRegistration.Password = uspsRegistrationSecuritySettingsControl.Password;

                uspsRegistration.FirstCodewordType = uspsRegistrationSecuritySettingsControl.FirstSecurityQuestionType;
                uspsRegistration.FirstCodewordValue = uspsRegistrationSecuritySettingsControl.FirstSecurityQuestionAnswer;

                uspsRegistration.SecondCodewordType = uspsRegistrationSecuritySettingsControl.SecondSecurityQuestionType;
                uspsRegistration.SecondCodewordValue = uspsRegistrationSecuritySettingsControl.SecondSecurityQuestionAnswer;
            }
        }

        /// <summary>
        /// Called when [step next new account payment].
        /// </summary>
        private void OnStepNextNewAccountPayment(object sender, WizardStepEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (uspsPaymentControl.ValidatePaymentData())
                {
                    // We now have all the user-supplied data needed to register the account with USPS
                    if (uspsPaymentControl.CreditCard != null)
                    {
                        uspsRegistration.CreditCard = uspsPaymentControl.CreditCard;

                        string cardholder = uspsRegistration.CreditCard.BillingAddress.FullName;
                        uspsRegistration.CreditCard.BillingAddress = uspsRegistration.PhysicalAddress;
                        uspsRegistration.CreditCard.BillingAddress.FirstName = "";
                        uspsRegistration.CreditCard.BillingAddress.LastName = "";
                        uspsRegistration.CreditCard.BillingAddress.FullName = cardholder;
                    }
                    else
                    {
                        uspsRegistration.AchAccount = uspsPaymentControl.BankAccount;
                    }

                    uspsRegistration.Submit();

                    // Save the USPS account now that it has been succesfully created
                    SaveUspsAccount(uspsRegistration.UserName, SecureText.Encrypt(uspsRegistration.Password, uspsRegistration.UserName));

                    registrationComplete = true;
                }
                else
                {
                    // The payment info provided could not be validated, so stay on the sasme page
                    e.NextPage = CurrentPage;
                }
            }
            catch (UspsRegistrationException ex)
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
        private void OnSteppingIntoExistingCredentials(object sender, WizardSteppingIntoEventArgs e)
        {
            // account registration is enabled (i.e. USPS has allowed ShipWorks to register accounts)
            if (radioNewAccount.Checked)
            {
                // The customer opted to create a new USPS account, so we'll skip the page
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

                new UspsWebClient(UspsResellerType.None).AuthenticateUser(userID, password.Text);

                if (UspsAccount == null)
                {
                    UspsAccount = new UspsAccountEntity
                    {
                        ContractType = (int)UspsAccountContractType.Unknown,
                        CreatedDate = DateTime.UtcNow
                    };
                }

                SaveUspsAccount(userID, SecureText.Encrypt(password.Text, userID));
            }
            catch (UspsException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Prepares the USPS account for save. This is just a hook to allow derived
        /// classes a chance to manipulate the account prior to it being persisted.
        /// </summary>
        protected virtual void PrepareUspsAccountForSave()
        {
            UspsAccount.UspsReseller = (int) UspsResellerType.None;
        }

        /// <summary>
        /// Saves the usps accountand initializes the stamps info control.
        /// </summary>
        /// <param name="accountUserName">The username.</param>
        /// <param name="encryptedPassword">The encrypted password.</param>
        protected virtual void SaveUspsAccount(string accountUserName, string encryptedPassword)
        {
            PrepareUspsAccountForSave();

            UspsAccount.Username = accountUserName;
            UspsAccount.Password = encryptedPassword;

            UspsAccount.Description = UspsAccountManager.GetDefaultDescription(UspsAccount);

            // Save the USPS account and use it to initialize the stamps info control
            UspsAccountManager.SaveAccount(UspsAccount);

            // Update the account contract type
            UspsShipmentType uspsShipmentType = PostalUtility.GetUspsShipmentTypeForUspsResellerType((UspsResellerType)UspsAccount.UspsReseller);
            uspsShipmentType.UpdateContractType(UspsAccount);
        }

        /// <summary>
        /// Stepping into the account info screen
        /// </summary>
        private void OnSteppingIntoAccountInfo(object sender, WizardSteppingIntoEventArgs e)
        {
            uspsAccountInfo.Initialize(UspsAccount);
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
            if (DialogResult != DialogResult.OK &&
                UspsAccount != null &&
                !UspsAccount.IsNew &&
                !UspsAccount.PendingInitialAccount)
            {
                UspsAccountManager.DeleteAccount(UspsAccount);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();

                UspsShipmentType shipmentType = (UspsShipmentType) ShipmentTypeManager.GetType(shipmentTypeCode);

                // If this is the only account, update this shipment type profiles with this account
                List<UspsAccountEntity> accounts = shipmentType.AccountRepository.Accounts.ToList();
                if (accounts.Count == 1)
                {
                    UspsAccountEntity accountEntity = accounts.First();

                    // Update any profiles to use this account if this is the only account
                    // in the system. This is to account for the situation where there a multiple
                    // profiles that may be associated with a previous account that has since
                    // been deleted.
                    IEnumerable<ShippingProfileEntity> shippingProfileEntities = ShippingProfileManager.Profiles
                        .Where(p => p.ShipmentType == (int) shipmentTypeCode)
                        .Where(shippingProfileEntity => shippingProfileEntity.Postal.Usps.UspsAccountID.HasValue);

                    foreach (ShippingProfileEntity shippingProfileEntity in shippingProfileEntities)
                    {
                        shippingProfileEntity.Postal.Usps.UspsAccountID = accountEntity.UspsAccountID;
                        ShippingProfileManager.SaveProfile(shippingProfileEntity);
                    }
                }

                ExcludeOtherPostalProviders();

                Messenger.Current.Send(new UspsAccountCreatedMessage(this, ShipmentTypeCode.Usps));
            }
        }

        /// <summary>
        /// Updates the shipping settings so the USPS shipment type is the only active postal provider.
        /// </summary>
        private void ExcludeOtherPostalProviders()
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            // We also need to exclude Endicia, and Express1 from the list
            // of active providers since the customer agreed to use USPS
            ExcludeShipmentType(settings, ShipmentTypeCode.Endicia);
            ExcludeShipmentType(settings, ShipmentTypeCode.Express1Endicia);
            ExcludeShipmentType(settings, ShipmentTypeCode.Express1Usps);
            ExcludeShipmentType(settings, ShipmentTypeCode.PostalWebTools);

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Removes the text about the $15.99 monthly fee for an account. This is intended for those users that
        /// may be signing up from that only have an Express1 account.
        /// </summary>
        private void RemoveMonthlyFeeText()
        {
            uspsPaymentControl.RemoveMonthlyFeeText();
        }

        /// <summary>
        /// Handle when the person control resizes
        /// </summary>
        private void OnPersonControlResize(object sender, EventArgs e)
        {
            panelTerms.Top = panelTerms.Top - (initialPersonControlHeight - personControl.Height);
        }

        /// <summary>
        /// Remove any existing rules and profiles
        /// </summary>
        private static void ClearExistingRulesAndProfiles()
        {
            // Make sure that if this is ever called after the Usps shipment type is configured that we don't
            // delete the users' data
            if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.Usps))
            {
                return;
            }

            // Performance is not a big deal here since this code only applies until Usps is configured
            // for the first time
            List<ShippingDefaultsRuleEntity> rules = ShippingDefaultsRuleManager.GetRules(ShipmentTypeCode.Usps);
            foreach (ShippingDefaultsRuleEntity rule in rules)
            {
                ShippingProfileEntity profile = ShippingProfileManager.GetProfile(rule.ShippingProfileID);
                if (profile != null)
                {
                    using (SqlAdapter sqlAdapter = new SqlAdapter())
                    {
                        sqlAdapter.DeleteEntity(profile);
                    }
                }

                ShippingDefaultsRuleManager.DeleteRule(rule);
            }
        }

        /// <summary>
        /// Excludes the given shipment type from the list of active shipping providers.
        /// </summary>
        /// <param name="settings">The settings being updated.</param>
        /// <param name="shipmentTypeToExclude">The shipment type code to be excluded.</param>
        private static void ExcludeShipmentType(ShippingSettingsEntity settings, ShipmentTypeCode shipmentTypeToExclude)
        {
            if (!settings.ExcludedTypes.Any(t => t == (int)shipmentTypeToExclude))
            {
                List<int> excludedTypes = settings.ExcludedTypes.ToList();
                excludedTypes.Add((int)shipmentTypeToExclude);

                settings.ExcludedTypes = excludedTypes.ToArray();
            }
        }

        /// <summary>
        /// Uses the USPS as the shipping provider for any rules using the given shipment type code.
        /// </summary>
        /// <param name="shipmentType">The shipment type code to be replaced with USPS.</param>
        private void UseUspsInDefaultShippingRulesFor(ShipmentTypeCode shipmentType)
        {
            List<ShippingDefaultsRuleEntity> rules = ShippingDefaultsRuleManager.GetRules(shipmentType);
            foreach (ShippingDefaultsRuleEntity rule in rules)
            {
                ShippingDefaultsRuleEntity clonedRule = CreateCopy(rule);

                clonedRule.ShipmentType = (int)ShipmentTypeCode.Usps;
                clonedRule.ShippingProfileID = GetRuleProfile(rule.ShippingProfileID);

                ShippingDefaultsRuleManager.SaveRule(clonedRule);
            }
        }

        /// <summary>
        /// Get a copied profile for the specified id
        /// </summary>
        private long GetRuleProfile(long profileId)
        {
            if (profileMap.ContainsKey(profileId))
            {
                return profileMap[profileId];
            }

            long newProfileId = CreateProfileClone(profileId);

            profileMap.Add(profileId, newProfileId);

            return newProfileId;
        }

        /// <summary>
        /// Create a copy of the profile with the given id
        /// </summary>
        private static long CreateProfileClone(long profileId)
        {
            ShippingProfileEntity profile = ShippingProfileManager.GetProfile(profileId);

            // If we can't find the requested profile (or it's not set in the original rule),
            // we'll just set the new rule to (none).
            if (profile == null)
            {
                return 0;
            }

            ShippingProfileEntity newProfile = CreateCopy(profile);

            newProfile.Name =
                $"{profile.Name} (from {EnumHelper.GetDescription((ShipmentTypeCode) profile.ShipmentType)})";
            newProfile.ShipmentType = (int)ShipmentTypeCode.Usps;
            newProfile.ShipmentTypePrimary = false;

            newProfile.Postal = CreateCopy(profile.Postal);
            newProfile.Postal.Usps = new UspsProfileEntity();

            EnsureUniqueName(newProfile, profile.Name);

            ShippingProfileManager.SaveProfile(newProfile);

            return newProfile.ShippingProfileID;
        }

        /// <summary>
        /// Attempt to ensure a unique name for the new profile
        /// </summary>
        private static void EnsureUniqueName(ShippingProfileEntity newProfile, string baseName)
        {
            int copyNumber = 1;
            while (ShippingProfileManager.DoesNameExist(newProfile))
            {
                newProfile.Name = string.Format("{0} (Copy {1})", baseName, copyNumber);
                copyNumber++;

                // If we can't find a unique name after 10 attempts, just give up...
                if (copyNumber == 10)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Create a copy of the specified object
        /// </summary>
        /// <returns>
        /// EntityUtility.CloneEntity would have worked, but this is specific to the needs of copying the profile. We don't want
        /// to copy any other postal profile data, Ups, FedEx, etc.
        /// </returns>
        private static T CreateCopy<T>(T copyFrom) where T : IEntity2, new()
        {
            T newObject = new T();

            foreach (IEntityField2 field in copyFrom.Fields.Cast<IEntityField2>().Where(field => !field.IsPrimaryKey && !field.IsReadOnly))
            {
                newObject.Fields[field.Name].CurrentValue = field.CurrentValue;
                newObject.Fields[field.Name].IsChanged = true;
            }

            newObject.Fields.IsDirty = true;

            return newObject;
        }

        /// <summary>
        /// Called when [step next welcome].
        /// </summary>
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            if (UspsAccount.PendingInitialAccount)
            {
                e.NextPage = wizardPageOptions;
            }
        }
    }
}
