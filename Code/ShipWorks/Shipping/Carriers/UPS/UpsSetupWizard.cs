using System;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Controls;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Wizard;
using System.Xml;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using System.Reflection;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Common.IO.Hardware.Printers;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Wizard for setting up UPS OLT for the first time
    /// </summary>
    [NDependIgnoreLongTypes]
    public partial class UpsSetupWizard : ShipmentTypeSetupWizardForm
    {
        ShipmentType shipmentType;
        bool forceAccountOnly;
        DateTime? notifyTime;

        string upsLicense;

        // The ups shipper we are creating
        UpsAccountEntity upsAccount = new UpsAccountEntity();

        private OpenAccountRequest openAccountRequest;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsSetupWizard(ShipmentTypeCode shipmentTypeCode)
            : this(shipmentTypeCode, false)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsSetupWizard(ShipmentTypeCode shipmentTypeCode, bool forceAccountOnly)
        {
            InitializeComponent();

            if (shipmentTypeCode != ShipmentTypeCode.UpsWorldShip && shipmentTypeCode != ShipmentTypeCode.UpsOnLineTools)
            {
                throw new InvalidOperationException("ShipmentTypeCode must be UPS");
            }

            shipmentType = ShipmentTypeManager.GetType(shipmentTypeCode);
            this.forceAccountOnly = forceAccountOnly;

            upsBusinessInfoControl.IndustryChanged = IndustryChanged;
        }

        /// <summary>
        /// Hide/show pharmacutical control based on industry selected.
        /// </summary>
        private void IndustryChanged(UpsBusinessIndustry upsBusinessIndustry)
        {
            switch (upsBusinessIndustry)
            {
                case UpsBusinessIndustry.Automotive:
                case UpsBusinessIndustry.HighTech:
                case UpsBusinessIndustry.IndustrialManufacturingAndDistribution:
                case UpsBusinessIndustry.Government:                    
                    upsPharmaceuticalControl.Visible = false;
                    break;
                case UpsBusinessIndustry.RetailAndConsumerGoods:
                case UpsBusinessIndustry.ProfessionalServices:
                case UpsBusinessIndustry.ConsumerServices:
                case UpsBusinessIndustry.Healthcare:
                case UpsBusinessIndustry.Other:
                    upsPharmaceuticalControl.Visible = true;
                    break;
                default:
                    upsPharmaceuticalControl.Visible = true;
                    break;
            }
        }

        /// <summary>
        /// Now that we have two separate selections, new or existing account, we have to reset to the original list of pages.
        /// Otherwise, if the user clicks next, pages for the currently selection option would be set, but if they click back
        /// and choose the other option, it's pages wouldn't be in the collection.
        /// </summary>
        [NDependIgnoreLongMethod]
        private void ResetWizardPagesCollection()
        {
            Pages.Clear();

            Pages.AddRange(new[] {
                wizardPageWelcomeOlt,
                wizardPageWelcomeWorldShip,
                wizardPageAccountList,
                wizardPageLicense,
                wizardPageOpenAccountCharacteristics,
                wizardPageAccount,
                wizardPageOpenAccountPickupSchedule,
                wizardPageOpenAccountPageBillingContactInfo,
                wizardPageOpenAccountPickupLocation,
                wizardPageRates,
                wizardPageOptionsOlt,
                wizardPageOptionsWorldShip,
                wizardPageFinishOlt,
                wizardPageFinishAddAccount});

            bool addAccountOnly = ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode) || forceAccountOnly;

            // Prepare the correct Welcome page
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                Pages.Remove(wizardPageWelcomeWorldShip);
            }
            else
            {
                Pages.Remove(wizardPageWelcomeOlt);
                Pages.Remove(wizardPageOpenAccountCharacteristics);
                Pages.Remove(wizardPageOpenAccountPickupSchedule);
                Pages.Remove(wizardPageOpenAccountPageBillingContactInfo);
                Pages.Remove(wizardPageOpenAccountPickupLocation);
            }

            upsAccount.CountryCode = "US";
            upsAccount.InvoiceAuth = false;
            upsAccount.RateType = (int)UpsRateType.DailyPickup;
            upsAccount.InitializeNullsToDefault();

            personControl.LoadEntity(new PersonAdapter(upsAccount, ""));

            // it will just be to setup to add a new account
            if (addAccountOnly)
            {
                // In this case the user is explicity wanting to add a new account, so drop the list and just do the add-account flow
                Pages.Remove(wizardPageAccountList);

                // And remove the settings pages
                Pages.Remove(wizardPageOptionsOlt);
                Pages.Remove(wizardPageOptionsWorldShip);
            }
            // Otherwise setup to configure settings completely
            else
            {
                // If there are no shippers yet (like from the other UPS shipment type), then remove the account page
                if (UpsAccountManager.Accounts.Count == 0)
                {
                    Pages.Remove(wizardPageAccountList);
                }
                // If there are other shippers (added from the other UPS shipment type), then just show the list
                else
                {
                    Pages.Remove(wizardPageLicense);
                    Pages.Remove(wizardPageAccount);
                    Pages.Remove(wizardPageRates);
                }

                // Add in the correct options page
                if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
                {
                    optionsControlOlt.LoadSettings();
                    Pages.Remove(wizardPageOptionsWorldShip);
                }
                else
                {
                    optionsControlWorldShip.LoadSettings();
                    Pages.Remove(wizardPageOptionsOlt);
                }

                Pages.Add(new ShippingWizardPageOrigin(shipmentType));
                Pages.Add(new ShippingWizardPageDefaults(shipmentType));

                ShippingWizardPageAutomation automationPage = new ShippingWizardPageAutomation(shipmentType);

                // WorldShip doesn't need printing
                if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
                {
                    Pages.Add(new ShippingWizardPagePrinting(shipmentType));
                }

                Pages.Add(automationPage);
            }

            Pages.Remove(wizardPageFinishAddAccount);

            // Insure finish is last
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
            {
                Pages.Remove(wizardPageFinishOlt);
                Pages.Add(wizardPageFinishOlt);
            }
            else
            {
                Pages.Remove(wizardPageFinishOlt);

                if (addAccountOnly)
                {
                    Pages.Add(wizardPageFinishAddAccount);
                }
                else
                {
                    Pages.Add(new ShippingWizardPageFinish(shipmentType));
                }
            }

            if (Pages.Contains(wizardPageAccountList))
            {
                accountControl.Initialize(shipmentType.ShipmentTypeCode);
            }

            // Listen for finish
            Pages[Pages.Count - 1].SteppingInto += new EventHandler<WizardSteppingIntoEventArgs>(OnSteppingIntoFinish);

            // Add in the first page
            SetCurrent(0);
        }


        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ResetWizardPagesCollection();
            ShowAccountNumberPanel();
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        [NDependIgnoreLongMethod]
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            string accountNumber = EnteredAccountNumber();

            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                if (!worldShipAgree1.Checked || !worldShipAgree2.Checked)
                {
                    MessageHelper.ShowInformation(this, "You must read and agree to the WorldShip information statements.");

                    e.NextPage = CurrentPage;
                    return;
                }
            }
            else
            {
                // Make sure one of the check boxes is checked.
                if (!newAccount.Checked && !existingAccount.Checked)
                {
                    MessageHelper.ShowMessage(this, "Please select an account option, New or Existing.");
                    e.NextPage = CurrentPage;
                }
            }

            // Validate the entered account number
            if (string.IsNullOrWhiteSpace(accountNumber) && existingAccount.Checked)
            {
                // Note: this will need to be refactored when we unhide the ability to create 
                // a new UPS account from ShipWorks
                MessageHelper.ShowMessage(this, "Please enter your account number.");
                e.NextPage = CurrentPage;
            }

            // Start with a fresh page collection
            ResetWizardPagesCollection();

            // If this is for an existing account, remove the open account wizards.
            if (existingAccount.Checked)
            {
                Pages.Remove(wizardPageOpenAccountCharacteristics);
                Pages.Remove(wizardPageOpenAccountPageBillingContactInfo);
                Pages.Remove(wizardPageOpenAccountPickupLocation);
                Pages.Remove(wizardPageOpenAccountPickupSchedule);
            }
            else
            {
                // Create a new OpenAccountRequest
                openAccountRequest = new OpenAccountRequest();

                if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsOnLineTools)
                {
                    // We are creating a new account, so remove the existing account entry wizard page
                    Pages.Remove(wizardPageAccount);
                }

                Pages.Remove(wizardPageRates);
            }

            // If the account list page is present, that means we arent creating accounts from this wizard flow directly
            if (Pages.Contains(wizardPageAccountList))
            {
                return;
            }

            // We already got the license, don't need to do it again
            if (upsLicense != null)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.LicenseAgreement, null);

            // <AccessLicenseProfile> block (performed a diff, and license agreement is same for both US and CA)
            xmlWriter.WriteStartElement("AccessLicenseProfile");
            xmlWriter.WriteElementString("CountryCode", "US");
            xmlWriter.WriteElementString("LanguageCode", "EN");
            xmlWriter.WriteEndElement();

            UpsWebClient.AppendToolList(xmlWriter);

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Process the XML request
                XmlDocument upsResponse = UpsWebClient.ProcessRequest(xmlWriter);

                // Extract the license text and return it to be passed on to the next stage of the wizard
                upsLicense = (string) upsResponse.CreateNavigator().Evaluate("string(//AccessLicenseText)");
            }
            catch (UpsException ex)
            {
                MessageBox.Show(this,
                    "An error occurred: " + ex.Message,
                    "ShipWorks",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping into the agreement page
        /// </summary>
        private void OnSteppingIntoAgreement(object sender, WizardSteppingIntoEventArgs e)
        {
            if (e.StepReason != WizardStepReason.StepBack)
            {
                licenseAgreement.Text = upsLicense;

                radioDeclineAgreement.Checked = true;
                NextEnabled = false;
            }
        }

        /// <summary>
        /// Stepping next from the license agreement page
        /// </summary>
        private void OnSteppingNextAgreement(object sender, WizardStepEventArgs e)
        {
            // Code below was for certification purposes

            //
            // This is a UPS requirement - but it causes annoyance and problems for our customers. They probably will never notice.
            //
            //NativeMethods.POINT point = new NativeMethods.POINT();
            //NativeMethods.SendMessage(licenseAgreement.Handle, NativeMethods.EM_GETSCROLLPOS, IntPtr.Zero, ref point);

            //if (point.y <= 7550)
            //{
            //    MessageHelper.ShowInformation(this, "UPS requires that we make you scroll to the bottom of the agreement before moving on.");
            //    e.NextPage = CurrentPage;
            //}
        }

        /// <summary>
        /// Changing if they accept the license agreement
        /// </summary>
        private void OnChangeAcceptAgreement(object sender, EventArgs e)
        {
            NextEnabled = radioAcceptAgreement.Checked;
        }

        /// <summary>
        /// Begin the printing process
        /// </summary>
        private void OnPrintAgreement(object sender, EventArgs e)
        {
            PrintUtility.PrintText(this, "ShipWorks - UPS License Agreement", upsLicense, true);
        }

        /// <summary>
        /// Open the link to open a UPS account
        /// </summary>
        private void OnLinkOpenAccount(object sender, EventArgs e)
        {
            string url = "https://www.ups.com/myups/info/openacct";

            PersonAdapter personAdapter = new PersonAdapter();
            personControl.SaveToEntity(personAdapter);

            if (personAdapter.CountryCode == "CA")
            {
                url = "https://www.ups.com/one-to-one/login?loc=en_CA";
            }
            else if (personAdapter.CountryCode == "PR")
            {
                url = "https://www.ups.com/one-to-one/login?loc=en_PR";
            }
            
            WebHelper.OpenUrl(url, this);
        }

        /// <summary>
        /// Gets the account number the user entered
        /// </summary>
        /// <returns></returns>
        private string EnteredAccountNumber()
        {
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                return wsUpsAccountNumber.Text.Trim();
            }
            return account.Text.Trim();
        }

        /// <summary>
        /// Checks to see if the given account number is allowed based on the edition of ShipWorks
        /// </summary>
        private bool AccountAllowed(string upsAccountNumber)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return lifetimeScope.Resolve<ILicenseService>()
                        .HandleRestriction(EditionFeature.UpsAccountNumbers, upsAccountNumber, this);
            }
        }

        /// <summary>
        /// Stepping next from the account page
        /// </summary>
        private void OnStepNextAccount(object sender, WizardStepEventArgs e)
        {
            personControl.SaveToEntity(new PersonAdapter(upsAccount, string.Empty));
            upsAccount.AccountNumber = EnteredAccountNumber();

            // Edition check
            if (!AccountAllowed(upsAccount.AccountNumber))
            {
                e.NextPage = CurrentPage;
                return;
            }
            
            RequiredFieldChecker checker = new RequiredFieldChecker();
            checker.Check("UPS Account", upsAccount.AccountNumber);
            checker.Check("Full Name", upsAccount.FirstName);
            checker.Check("Company", upsAccount.Company);
            checker.Check("Street Address", upsAccount.Street1);
            checker.Check("City", upsAccount.City);
            checker.Check("State", upsAccount.StateProvCode);
            checker.Check("Postal Code", upsAccount.PostalCode);
            checker.Check("Phone", upsAccount.Phone);
            checker.Check("Website", upsAccount.Website);
            checker.Check("Email", upsAccount.Email);

            if (!checker.Validate(this))
            {
                e.NextPage = CurrentPage;
                return;
            }

            if (ShippingSettings.Fetch().UpsAccessKey == null)
            {
                if (!GetUpsAccessKey())
                {
                    e.NextPage = CurrentPage;
                    return;
                }
            }

            if (!ProcessRegistration(3, true))
            {
                e.NextPage = CurrentPage;
                return;
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                UpsAccountManager.SaveAccount(upsAccount);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Get the global instanced UPS access key
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool GetUpsAccessKey()
        {
            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.AccessKey, null);

            xmlWriter.WriteStartElement("AccessLicenseProfile");
            xmlWriter.WriteElementString("CountryCode", upsAccount.CountryCode);
            xmlWriter.WriteElementString("LanguageCode", "EN");
            xmlWriter.WriteElementString("AccessLicenseText", upsLicense);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteElementString("CompanyName", upsAccount.Company);
            xmlWriter.WriteElementString("CompanyURL", upsAccount.Website);
            xmlWriter.WriteElementString("ShipperNumber", upsAccount.AccountNumber);

            xmlWriter.WriteStartElement("Address");
            xmlWriter.WriteElementString("AddressLine1", upsAccount.Street1);
            xmlWriter.WriteElementString("AddressLine2", upsAccount.Street2);
            xmlWriter.WriteElementString("AddressLine3", upsAccount.Street3);
            xmlWriter.WriteElementString("City", upsAccount.City);
            xmlWriter.WriteElementString("StateProvinceCode", upsAccount.StateProvCode);
            xmlWriter.WriteElementString("PostalCode", upsAccount.PostalCode);
            xmlWriter.WriteElementString("CountryCode", upsAccount.CountryCode);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("PrimaryContact");
            xmlWriter.WriteElementString("Name", new PersonName(new PersonAdapter(upsAccount, "")).FullName);
            xmlWriter.WriteElementString("Title", "N\\A");
            xmlWriter.WriteElementString("EMailAddress", upsAccount.Email);
            xmlWriter.WriteElementString("PhoneNumber", new PersonAdapter(upsAccount, "").Phone10Digits);
            xmlWriter.WriteEndElement();

            UpsWebClient.AppendToolList(xmlWriter);

            xmlWriter.WriteStartElement("ClientSoftwareProfile");
            xmlWriter.WriteElementString("SoftwareInstaller", "User");
            xmlWriter.WriteElementString("SoftwareProductName", "ShipWorks");
            xmlWriter.WriteElementString("SoftwareProvider", "Interapptive, Inc.");
            xmlWriter.WriteElementString("SoftwareVersionNumber", Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
            xmlWriter.WriteEndElement();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // Process the XML request
                XmlDocument upsResponse = UpsWebClient.ProcessRequest(xmlWriter);

                // Now we can get the Access License number
                string accessKey = (string) upsResponse.CreateNavigator().Evaluate("string(//AccessLicenseNumber)");

                ShippingSettingsEntity settings = ShippingSettings.Fetch();
                settings.UpsAccessKey = SecureText.Encrypt(accessKey, "UPS");

                ShippingSettings.Save(settings);

                return true;
            }
            catch (UpsException ex)
            {
                MessageBox.Show(this,
                    "An error occurred: " + ex.Message,
                    "ShipWorks",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        /// <summary>
        /// Get a UserId and Password
        /// </summary>
        [NDependIgnoreLongMethod]
        private bool ProcessRegistration(int tries, bool showErrorMessage)
        {
            // Create the client for connecting to the UPS server
            using (XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.Register, null))
            {
                string userId = Guid.NewGuid().ToString("N").Substring(0, 16);
                string password = Guid.NewGuid().ToString("N").Substring(0, 8);

                xmlWriter.WriteElementString("UserId", userId);
                xmlWriter.WriteElementString("Password", password);

                xmlWriter.WriteStartElement("RegistrationInformation");
                xmlWriter.WriteElementString("UserName", new PersonName(new PersonAdapter(upsAccount, "")).FullName);
                xmlWriter.WriteElementString("CompanyName", upsAccount.Company);
                xmlWriter.WriteElementString("Title", "N\\A");

                xmlWriter.WriteStartElement("Address");
                xmlWriter.WriteElementString("AddressLine1", upsAccount.Street1);
                xmlWriter.WriteElementString("AddressLine2", upsAccount.Street2);
                xmlWriter.WriteElementString("AddressLine3", upsAccount.Street3);
                xmlWriter.WriteElementString("City", upsAccount.City);
                xmlWriter.WriteElementString("StateProvinceCode", upsAccount.StateProvCode);
                xmlWriter.WriteElementString("PostalCode", upsAccount.PostalCode);
                xmlWriter.WriteElementString("CountryCode", upsAccount.CountryCode);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteElementString("PhoneNumber", new PersonAdapter(upsAccount, "").Phone10Digits);
                xmlWriter.WriteElementString("EMailAddress", upsAccount.Email);

                xmlWriter.WriteStartElement("ShipperAccount");
                xmlWriter.WriteElementString("AccountName", "Interapptive");
                xmlWriter.WriteElementString("ShipperNumber", upsAccount.AccountNumber);
                xmlWriter.WriteElementString("PickupPostalCode", upsAccount.PostalCode);
                xmlWriter.WriteElementString("PickupCountryCode", upsAccount.CountryCode);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();

                try
                {
                    Cursor.Current = Cursors.WaitCursor;

                    UpsWebClient.ProcessRequest(xmlWriter);

                    upsAccount.UserID = userId;
                    upsAccount.Password = password;

                    return true;
                }
                catch (UpsException ex)
                {
                    // If UserId is already taken, try again
                    if (ex.ErrorCode == "160500")
                    {
                        // If we still want to try some more
                        if (tries > 0)
                        {
                            return ProcessRegistration(tries - 1, showErrorMessage);
                        }

                        const string message = "A unique UserID could not be generated.  Please try again.";
                        ShowOrThrowErrorMessage(showErrorMessage, message, ex);
                        return false;
                    }
                    ShowOrThrowErrorMessage(showErrorMessage, ex.Message, ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Shows the error message or throws an error with the message.
        /// </summary>
        /// <exception cref="UpsException"></exception>
        private void ShowOrThrowErrorMessage(bool showErrorMessage, string message, UpsException ex)
        {
            if (showErrorMessage)
            {
                // Give up for now
                MessageHelper.ShowError(this, message);
            }
            else
            {
                throw new UpsException(message, ex);
            }
        }

        /// <summary>
        /// Stepping next from the rates page
        /// </summary>
        private void OnStepNextRates(object sender, WizardStepEventArgs e)
        {
            try
            {
                upsRateTypeControl.RegisterAndSaveToEntity();
            }
            catch (CarrierException ex)
            {
                MessageHelper.ShowMessage(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Stepping next from the options page
        /// </summary>
        private void OnStepNextOptionsOlt(object sender, WizardStepEventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            optionsControlOlt.SaveSettings();

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Stepping next from the WorldShip options page
        /// </summary>
        private void OnStepNextOptionsWorldShip(object sender, WizardStepEventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            optionsControlWorldShip.SaveSettings(settings);

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Stepping into the finishing page
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                WorldShipImportMonitor.Start();
            }

            // If the account list page is present that means we didn't create the account from this wizard
            if (!Pages.Contains(wizardPageAccountList))
            {
                upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                UpsAccountManager.SaveAccount(upsAccount);

                // Mark the new account as configured
                ShippingSettings.MarkAsConfigured(shipmentType.ShipmentTypeCode);

                // If this is the only account, update this UPS shipment type profiles with this account
                if (UpsAccountManager.Accounts.Count == 1)
                {
                    UpsAccountEntity upsAccountEntity = UpsAccountManager.Accounts.First();

                    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)shipmentType.ShipmentTypeCode))
                    {
                        shippingProfileEntity.Ups.UpsAccountID = upsAccountEntity.UpsAccountID;
                        ShippingProfileManager.SaveProfile(shippingProfileEntity);
                    }
                }

                // We created a new UPS account
                if (newAccount.Checked)
                {
                    labelSetupComplete1.Text = "Congratulations, you successfully created a UPS account within ShipWorks!";
                    labelSetupComplete2.Text = $"Your new UPS account number: {upsAccount.AccountNumber}";
                    labelSetupComplete3.Text = "Please watch your email for a confirmation from UPS and more information on how to use your account.";
                    if (notifyTime.HasValue)
                    {
                        labelSetupCompleteNotifyTime.Text = $"UPS Smart Pickup Notify Time: {notifyTime.Value.ToString("t")}";
                    }
                }
            }
        }

        /// <summary>
        /// Window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            // If we are canceling - but had created an account - we need to undo that
            if (DialogResult == DialogResult.Cancel && upsAccount != null && !upsAccount.IsNew)
            {
                UpsAccountManager.DeleteAccount(upsAccount);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();
            }
        }

        /// <summary>
        /// Stepping next from the Open Account Billing Info page
        /// </summary>
        private void OnStepNextOpenAccountBillingInfo(object sender, WizardStepEventArgs e)
        {
            try
            {
                upsBillingContactInfoControl.SaveToAccountAndRequest(openAccountRequest, upsAccount);
                if (upsBillingContactInfoControl.SameAsPickup)
                {
                    CreateAccount();
                    e.NextPage = Pages[Pages.Count - 1]; // Go to last page.
                }
            }
            catch (UpsOpenAccountException ex)
            {
                HandleOpenAccountException(e, ex);
            }
        }

        /// <summary>
        /// Stepping next from the Open Account Billing Info page
        /// </summary>
        private void OnStepNextOpenAccountPickupLocationInfo(object sender, WizardStepEventArgs e)
        {
            try
            {
                upsPickupLocationControl.SavePickupInfoToAccountAndRequest(openAccountRequest, upsAccount);
                CreateAccount();
            }
            catch (UpsOpenAccountException ex)
            {
                HandleOpenAccountException(e, ex);
            }
        }

        /// <summary>
        /// Handles the open account exception.
        /// </summary>
        private void HandleOpenAccountException(WizardStepEventArgs wizardStepEventArgs, UpsOpenAccountException openAccountException)
        {
            switch (openAccountException.ErrorCode)
            {
                case UpsOpenAccountErrorCode.MissingRequiredFields:
                    // If MissingRequiredFields, The person control already showed a message, cancel and return.
                    wizardStepEventArgs.NextPage = CurrentPage;
                    break;

                case UpsOpenAccountErrorCode.NotRegistered:
                    Pages.Remove(wizardPageFinishOlt);
                    Pages.Add(wizardPageFinishCreateAccountRegistrationFailed);
                    wizardStepEventArgs.NextPage = wizardPageFinishCreateAccountRegistrationFailed;
                    FinishCancels = true;

                    labelCreateAccountRegistrationFailed2.Text = string.Format("The new UPS account is currently not registered within the ShipWorks software.  To add this account later, select “Use an existing UPS account” and enter {0} as your UPS account number.", upsAccount.AccountNumber);
                    labelCreateAccountRegistrationFailed3.Text = string.Format("Your new UPS account number:  {0}", upsAccount.AccountNumber);

                    break;

                default:
                    MessageHelper.ShowMessage(this, openAccountException.Message);
                    wizardStepEventArgs.NextPage = CurrentPage;
                    break;
            }
        }

        /// <summary>
        /// Called when [step next wizard page open account characteristics].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardStepEventArgs" /> instance containing the event data.</param>
        private void OnStepNextWizardPageOpenAccountCharacteristics(object sender, WizardStepEventArgs e)
        {
            try
            {
                shipmentCharacteristics.SaveToRequest(openAccountRequest);
                accountCharacteristics.SaveToRequest(openAccountRequest);
                upsBusinessInfoControl.SaveToRequest(openAccountRequest);
                upsPharmaceuticalControl.SaveToRequest(openAccountRequest);
            }
            catch (UpsOpenAccountException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error");
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Called when [step next wizard page open account pickup schedule].
        /// </summary>
        private void OnStepNextWizardPageOpenAccountPickupSchedule(object sender, WizardStepEventArgs e)
        {
            try
            {
                pickupSchedule.SaveToRequest(openAccountRequest);
            }
            catch (UpsOpenAccountException ex)
            {
                MessageBox.Show(ex.Message, "Validation Error");
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Called when [stepping into rates].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardSteppingIntoEventArgs"/> instance containing the event data.</param>
        private void OnSteppingIntoRates(object sender, WizardSteppingIntoEventArgs e)
        {
            upsRateTypeControl.Initialize(upsAccount, newAccount.Checked);
        }

        /// <summary>
        /// Called when [account option check changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAccountOptionCheckChanged(object sender, EventArgs e)
        {
            ShowAccountNumberPanel();
        }

        /// <summary>
        /// Shows the account number panel.
        /// </summary>
        private void ShowAccountNumberPanel()
        {
            accountNumberPanel.Visible = existingAccount.Checked;
        }

        /// <summary>
        /// Show Open Account help when help clicked.
        /// </summary>
        private void OnHelpClick(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/solution/articles/4000035267-installing-ups-using-the-ups-setup-wizard", this);
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        private void CreateAccount()
        {
            UpsOpenAccountResponseDTO upsOpenAccountResponse = OpenUpsAccount(new UpsClerk(upsAccount));

            try
            {
                RegisterNewAccount(upsOpenAccountResponse.AccountNumber);
                notifyTime = upsOpenAccountResponse.NotifyTime;
            }
            catch (UpsException ex)
            {
                throw new UpsOpenAccountException(ex.Message, UpsOpenAccountErrorCode.MissingRequiredFields);
            }
        }

        /// <summary>
        /// Registers the account.
        /// </summary>
        private void RegisterNewAccount(string newAccountNumber)
        {
            upsAccount.AccountNumber = newAccountNumber;
            try
            {
                ProcessRegistration(3, false);

                NextEnabled = true;

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                    UpsAccountManager.SaveAccount(upsAccount);

                    adapter.Commit();
                }
            }
            catch (UpsException ex)
            {
                throw new UpsOpenAccountException(ex.Message, UpsOpenAccountErrorCode.NotRegistered, ex);
            }
        }

        /// <summary>
        /// Creates the ups account. Note the recursive call to correct the address.
        /// </summary>
        /// <param name="clerk">The clerk.</param>
        /// <returns></returns>
        private UpsOpenAccountResponseDTO OpenUpsAccount(IUpsClerk clerk)
        {
            return OpenUpsAccount(clerk, false);
        }

        /// <summary>
        /// Creates the ups account. Note the recursive call to correct the address.
        /// </summary>
        /// <param name="clerk">The clerk.</param>
        /// <param name="retrySmartPost">if set to <c>true</c> [retry smart post].</param>
        /// <returns></returns>
        /// <exception cref="UpsOpenAccountException">Ups didn't return a new account number.</exception>
        private UpsOpenAccountResponseDTO OpenUpsAccount(IUpsClerk clerk, bool retrySmartPost)
        {
            UpsOpenAccountResponseDTO upsOpenAccountResponse = null;

            try
            {
                OpenAccountResponse response = clerk.OpenAccount(openAccountRequest);


                upsOpenAccountResponse = new UpsOpenAccountResponseDTO(response.ShipperNumber, response.NotifyTime);

            }
            catch (UpsOpenAccountPickupAddressException ex)
            {
                if (CorrectPickupAddress(ex.SuggestedAddress, openAccountRequest.PickupAddress))
                {
                    upsOpenAccountResponse = OpenUpsAccount(clerk);
                }
            }
            catch (UpsOpenAccountBusinessAddressException ex)
            {
                if (CorrectBillingAddress(ex.SuggestedAddress, openAccountRequest.BillingAddress))
                {
                    upsOpenAccountResponse = OpenUpsAccount(clerk);
                }
            }
            catch (UpsOpenAccountSoapException ex)
            {
                throw new UpsOpenAccountException(string.Format("Ups returned the following error: {0}", ex.Message), ex);
            }
            catch (UpsOpenAccountException ex)
            {
                if (ex.ErrorCode == UpsOpenAccountErrorCode.SmartPickupError && !retrySmartPost)
                {
                    string correctedAddress = UpsUtility.CorrectSmartPickupError(openAccountRequest.PickupAddress.City);

                    if (!string.IsNullOrEmpty(correctedAddress))
                    {
                        openAccountRequest.PickupAddress.City = correctedAddress;

                        upsOpenAccountResponse = OpenUpsAccount(clerk, true);
                    }
                    else
                    {
                        throw new UpsOpenAccountException("UPS couldn't resolve the pickup address. If there are alternate spellings, try again using one of those.", ex);
                    }
                }
                else
                {
                    throw;
                }
            }

            return upsOpenAccountResponse;
        }

        /// <summary>
        /// Validates the pickup address.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="pickupAddressType">Type of the pickup address.</param>
        /// <returns></returns>
        /// <exception cref="ShipWorks.Shipping.Carriers.UPS.OpenAccount.UpsOpenAccountInvalidAddressException"></exception>
        private static bool CorrectPickupAddress(AddressKeyCandidateType addressCandidate, PickupAddressType pickupAddressType)
        {
            bool isAddressCorrected = false;

            using (UpsOpenAccountInvalidAddressDlg invalidAddressDlg = new UpsOpenAccountInvalidAddressDlg())
            {
                invalidAddressDlg.SetAddress(addressCandidate, "Pickup");
                DialogResult result = invalidAddressDlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    pickupAddressType.StreetAddress = addressCandidate.StreetAddress;
                    pickupAddressType.City = addressCandidate.City;
                    pickupAddressType.StateProvinceCode = addressCandidate.State;
                    pickupAddressType.PostalCode = addressCandidate.PostalCode;
                    pickupAddressType.CountryCode = addressCandidate.CountryCode;

                    isAddressCorrected = true;
                }
            }

            return isAddressCorrected;
        }

        /// <summary>
        /// Validates the address.
        /// </summary>
        /// <param name="addressCandidate">The address candidate.</param>
        /// <param name="billingAddressType">Type of the billing address.</param>
        /// <returns></returns>
        /// <exception cref="ShipWorks.Shipping.Carriers.UPS.OpenAccount.UpsOpenAccountInvalidAddressException">If address suggested and user cancels, throw</exception>
        private static bool CorrectBillingAddress(AddressKeyCandidateType addressCandidate, BillingAddressType billingAddressType)
        {
            bool isAddressCorrected = false;

            using (UpsOpenAccountInvalidAddressDlg invalidAddressDlg = new UpsOpenAccountInvalidAddressDlg())
            {
                invalidAddressDlg.SetAddress(addressCandidate, "Billing");
                DialogResult result = invalidAddressDlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    billingAddressType.StreetAddress = addressCandidate.StreetAddress;
                    billingAddressType.City = addressCandidate.City;
                    billingAddressType.StateProvinceCode = addressCandidate.State;
                    billingAddressType.PostalCode = addressCandidate.PostalCode;
                    billingAddressType.CountryCode = addressCandidate.CountryCode;

                    isAddressCorrected = true;
                }
            }

            return isAddressCorrected;
        }
    }
}
