using System;
using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;
using ShipWorks.UI.Wizard;
using System.Xml;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using System.Reflection;
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
using ShipWorks.Editions;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Wizard for setting up UPS OLT for the first time
    /// </summary>
    public partial class UpsSetupWizard : ShipmentTypeSetupWizardForm
    {
        ShipmentType shipmentType;
        bool forceAccountOnly;

        string upsLicense = null;

        // The ups shipper we are creating
        UpsAccountEntity upsAccount = new UpsAccountEntity();

        private OpenAccountRequest openAccountRequest;

        private bool isAccountCreated = false;

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

            this.shipmentType = ShipmentTypeManager.GetType(shipmentTypeCode);
            this.forceAccountOnly = forceAccountOnly;
        }

        /// <summary>
        /// Now that we have two separate selections, new or existing account, we have to reset to the original list of pages.
        /// Otherwise, if the user clicks next, pages for the currently selection option would be set, but if they click back
        /// and choose the other option, it's pages wouldn't be in the collection.
        /// </summary>
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
                wizardPageOpenAccountShipmentCharacteristics,
                wizardPageOpenAccountBusinessInfo,
                wizardPageOpenAccountPickupSchedule,
                wizardPageOpenAccountPageBillingContactInfo,
                wizardPageOpenAccountPickupLocation,
                wizardPageOpenAccountCreateAccount,
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
                Pages.Remove(wizardPageOpenAccountShipmentCharacteristics);
                Pages.Remove(wizardPageOpenAccountBusinessInfo);
                Pages.Remove(wizardPageOpenAccountPickupSchedule);
                Pages.Remove(wizardPageOpenAccountPageBillingContactInfo);
                Pages.Remove(wizardPageOpenAccountPickupLocation);
                Pages.Remove(wizardPageOpenAccountCreateAccount);
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
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            if (shipmentType.ShipmentTypeCode == ShipmentTypeCode.UpsWorldShip)
            {
                if (!worldShipAgree1.Checked || !worldShipAgree2.Checked)
                {
                    MessageHelper.ShowInformation(this,
                                                  "You must read and agree to the WorldShip information statements.");

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

            // Start with a fresh page collection
            ResetWizardPagesCollection();

            // If this is for an existing account, remove the open account wizards.
            if (existingAccount.Checked)
            {
                Pages.Remove(wizardPageOpenAccountBusinessInfo);
                Pages.Remove(wizardPageOpenAccountCharacteristics);
                Pages.Remove(wizardPageOpenAccountCreateAccount);
                Pages.Remove(wizardPageOpenAccountPageBillingContactInfo);
                Pages.Remove(wizardPageOpenAccountPickupLocation);
                Pages.Remove(wizardPageOpenAccountPickupSchedule);
                Pages.Remove(wizardPageOpenAccountShipmentCharacteristics);
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

                // TODO: May need to remove the rates page too
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
        private void OnPrintAgreement(object sender, System.EventArgs e)
        {
            PrintUtility.PrintText(this, "ShipWorks - UPS License Agreement", upsLicense, true);
        }

        /// <summary>
        /// Open the link to open a UPS account
        /// </summary>
        private void OnLinkOpenAccount(object sender, EventArgs e)
        {
            string url = "https://www.ups.com/myups/info/openacct";
         
            if (personControl.CountryCode == "CA")
            {
                url = "https://www.ups.com/one-to-one/login?loc=en_CA";
            }
            else if (personControl.CountryCode == "PR")
            {
                url = "https://www.ups.com/one-to-one/login?loc=en_PR";
            }
            
            WebHelper.OpenUrl(url, this);
        }

        /// <summary>
        /// Stepping next from the account page
        /// </summary>
        private void OnStepNextAccount(object sender, WizardStepEventArgs e)
        {
            personControl.SaveToEntity();
            upsAccount.AccountNumber = account.Text.Trim();

            // Edition check
            if (!EditionManager.HandleRestrictionIssue(this, EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.UpsAccountNumbers, upsAccount.AccountNumber)))
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

            if (!ProcessRegistration(3))
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
        private bool ProcessRegistration(int tries)
        {
            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.Register, null);

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
                        return ProcessRegistration(tries - 1);
                    }

                    else
                    {
                        // Give up for now
                        MessageHelper.ShowError(this, "A unique UserID could not be generated.  Please try again.");
                        return false;
                    }
                }

                else
                {
                    MessageHelper.ShowError(this, ex.Message);
                    return false;
                }
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

            optionsControlOlt.SaveSettings(settings);

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
        /// Link to the UPS homepage
        /// </summary>
        private void OnLinkUpsHome(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.ups.com", this);
        }

        /// <summary>
        /// Link to the UPS OnLine Tools page
        /// </summary>
        private void OnLinkUpsOnlineTools(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.ups.com/content/us/en/shipping/index.html", this);
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
        }

        /// <summary>
        /// Stepping next from the WorldShip options page
        /// </summary>
        private void OnStepNextBusinessInfo(object sender, WizardStepEventArgs e)
        {
            try
            {
                upsBusinessInfoControl.SaveToRequest(openAccountRequest);
            }
            catch (UpsOpenAccountException ex)
            {
                if (ex.ErrorCode == UpsOpenAccountErrorCode.MissingRequiredFields)
                {
                    MessageHelper.ShowMessage(this, ex.Message);
                    e.NextPage = CurrentPage;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary> 
        /// Called when [stepping into open account business info]. 
        /// </summary> 
        /// <param name="sender">The sender.</param> 
        /// <param name="e">The <see cref="WizardSteppingIntoEventArgs" /> instance containing the event data.</param> 
        private void OnSteppingIntoBusinessInfo(object sender, WizardSteppingIntoEventArgs e)
        {
            if (openAccountRequest.AccountCharacteristics.CustomerClassification.Code != EnumHelper.GetApiValue(UpsCustomerClassificationCode.Business))
            {
                e.Skip = true;
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
            }
            catch (UpsOpenAccountException ex)
            {
                if (ex.ErrorCode == UpsOpenAccountErrorCode.MissingRequiredFields)
                {
                    // The person control already showed a message, cancel and return.
                    e.NextPage = CurrentPage;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Stepping next from the Open Account Billing Info page
        /// </summary>
        private void OnStepNextOpenAccountPickupLocationInfo(object sender, WizardStepEventArgs e)
        {
            try
            {
                upsPickupLocationControl.SaveToRequest(openAccountRequest);
            }
            catch (UpsOpenAccountException ex)
            {
                if (ex.ErrorCode == UpsOpenAccountErrorCode.MissingRequiredFields)
                {
                    // The person control already showed a message, cancel and return.
                    e.NextPage = CurrentPage;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Called when [step next wizard page open account characteristics].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardStepEventArgs" /> instance containing the event data.</param>
        private void OnStepNextWizardPageOpenAccountCharacteristics(object sender, WizardStepEventArgs e)
        {
            accountCharacteristics.SaveToRequest(openAccountRequest); 
        }

        /// <summary>
        /// Called when [step next wizard page open account shipment characteristics].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardStepEventArgs" /> instance containing the event data.</param>
        private void OnStepNextWizardPageOpenAccountShipmentCharacteristics(object sender, WizardStepEventArgs e)
        {
            try
            {
                shipmentCharacteristics.SaveToRequest(openAccountRequest);
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
        /// Steppings the into wizard page open account create account.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardSteppingIntoEventArgs"/> instance containing the event data.</param>
        private void SteppingIntoWizardPageOpenAccountCreateAccount(object sender, WizardSteppingIntoEventArgs e)
        {
            createAccount.OpenAccountRequest = openAccountRequest;

            NextEnabled = isAccountCreated;

            createAccount.AccountCreated = accountNumber =>
            {
                upsAccount.AccountNumber = accountNumber;
                bool processRegistration = ProcessRegistration(3);
                if (processRegistration)
                {
                    NextEnabled = true;
                    isAccountCreated = true;

                    MessageHelper.ShowInformation(this, "Account created. Click next to continue.");                    
                }
                else
                {
                    MessageHelper.ShowInformation(this, string.Format("Ups Account created, but not registered in ShipWorks."));
                }
            };

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
    }
}
