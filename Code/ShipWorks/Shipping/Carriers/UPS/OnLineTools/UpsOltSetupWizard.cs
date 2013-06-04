using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using System.Xml;
using System.IO;
using System.Drawing.Printing;
using ShipWorks.UI;
using ShipWorks.Common.Web;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data;
using System.Reflection;
using ShipWorks.Common.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Wizard for setting up UPS OLT for the first time
    /// </summary>
    public partial class UpsOltSetupWizard : WizardForm
    {
        string upsLicense = null;

        // The ups shipper we are creating
        UpsAccountEntity upsAccount = new UpsAccountEntity();

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.UpsOnLineTools);

            upsAccount.CountryCode = "US";
            upsAccount.RateType = (int) UpsRateType.DailyPickup;
            upsAccount.InitializeNullsToDefault();

            personControl.LoadEntity(new PersonAdapter(upsAccount, ""));

            // If its not already setup, load all the the settings\configuration pages
            if (!ShippingManager.IsShipmentTypeSetup(ShipmentTypeCode.UpsOnLineTools))
            {
                optionsControl.LoadSettings();

                Pages.Add(new ShippingWizardPageOrigin(shipmentType));
                Pages.Add(new ShippingWizardPageDefaults(shipmentType));
                Pages.Add(new ShippingWizardPagePrinting(shipmentType));
                Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            }
            // Otherwise it will just be to setup the shipper
            else
            {
                Pages.Remove(wizardPageOptions);
            }

            // Insure finish is last
            Pages.Remove(wizardPageFinish);
            Pages.Add(wizardPageFinish);
        }

        /// <summary>
        /// Stepping next from the welcome page
        /// </summary>
        private void OnStepNextWelcome(object sender, WizardStepEventArgs e)
        {
            // We already got the license, don't need to do it again
            if (ShippingSettings.Fetch().UpsAccessKey != null || upsLicense != null)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.LicenseAgreement, null);

            // <AccessLicenseProfile> block
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
            if (ShippingSettings.Fetch().UpsAccessKey != null)
            {
                e.Skip = true;
            }
            else if (e.StepReason != WizardStepReason.StepBack)
            {
                licenseAgreement.Text = upsLicense;

                radioDeclineAgreement.Checked = true;
                NextEnabled = false;
            }
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
            // Show the print dialog first
            if (printDialog.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                using (StringReader streamToPrint = new StringReader(upsLicense))
                {
                    printDialog.Tag = streamToPrint;
                    printDocument.Print();
                }
            }
            catch (InvalidPrinterException ex)
            {
                MessageHelper.ShowError(this, "There was a problem printing the document: " + ex.Message);
            }
        }

        /// <summary>
        /// Print a page of the license agreement
        /// </summary>
        private void OnPrintAgreementPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float leftMargin = e.MarginBounds.Left;
            float topMargin = e.MarginBounds.Top;

            // Calculate the number of lines per page.
            float linesPerPage = e.MarginBounds.Height / Font.GetHeight(e.Graphics);

            string line = null;
            int count = 0;

            StringReader streamToPrint = (StringReader) printDialog.Tag;

            // Print each line of the file.
            while (count < linesPerPage &&
                ((line = streamToPrint.ReadLine()) != null))
            {
                float yPos = topMargin + (count * Font.GetHeight(e.Graphics));

                e.Graphics.DrawString(
                    line,
                    Font,
                    Brushes.Black,
                    leftMargin,
                    yPos,
                    new StringFormat());

                count++;
            }

            // If more lines exist, print another page.
            e.HasMorePages = (line != null);
        }

        /// <summary>
        /// Open the link to open a UPS account
        /// </summary>
        private void OnLinkOpenAccount(object sender, EventArgs e)
        {
            WebUtility.OpenUrl("https://www.ups.com/myups/info/openacct", this);
        }

        /// <summary>
        /// Stepping next from the account page
        /// </summary>
        private void OnStepNextAccount(object sender, WizardStepEventArgs e)
        {
            personControl.SaveToEntity();
            upsAccount.AccountNumber = account.Text.Trim();

            RequireFieldChecker checker = new RequireFieldChecker();
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

            upsRateTypeControl.Initialize(upsAccount);
        }

        /// <summary>
        /// Get the global instanced UPS access key
        /// </summary>
        private bool GetUpsAccessKey()
        {
            // Create the client for connecting to the UPS server
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.AccessKey, null);

            xmlWriter.WriteStartElement("AccessLicenseProfile");
            xmlWriter.WriteElementString("CountryCode", "US");
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
                if (ex.ErrorCode == 160500)
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
            upsRateTypeControl.SaveToEntity();
        }

        /// <summary>
        /// Stepping next from the options page
        /// </summary>
        private void OnStepNextOptions(object sender, WizardStepEventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            optionsControl.SaveSettings(settings);

            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Link to the UPS homepage
        /// </summary>
        private void OnLinkUpsHome(object sender, EventArgs e)
        {
            WebUtility.OpenUrl("http://www.ups.com", this);
        }

        /// <summary>
        /// Link to the UPS OnLine Tools page
        /// </summary>
        private void OnLinkUpsOnlineTools(object sender, EventArgs e)
        {
            WebUtility.OpenUrl("http://www.ups.com/content/us/en/shipping/index.html", this);
        }

        /// <summary>
        /// Stepping into the finishing page
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                upsAccount.Description = UpsAccountManager.GetDefaultDescription(upsAccount);
                UpsAccountManager.SaveAccount(upsAccount);

                adapter.Commit();
            }
        }
    }
}
