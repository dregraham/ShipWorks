using System;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Authentication;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Controls;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Setup wizard for processing shipments with Stamps.com
    /// </summary>
    public partial class OnTracSetupWizard : WizardForm
    {
        OnTracAccountEntity account;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.OnTrac);

            optionsControl.LoadSettings();

            var shippingWizardPageFinish = new ShippingWizardPageFinish(shipmentType);

            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(shippingWizardPageFinish);

            shippingWizardPageFinish.SteppingInto += OnSteppingIntoFinish;
        }

        /// <summary>
        /// User clicked the link to open the OnTrac website
        /// </summary>
        private void OnLinkOnTracWebsite(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://www.ontrac.com", this);
        }

        /// <summary>
        /// Stepping next from the credentials page
        /// </summary>
        private void OnNextStepCredentials(object sender, WizardStepEventArgs e)
        {
            int accountNumberValue;

            if (!int.TryParse(accountNumber.Text, out accountNumberValue))
            {
                // since this is a numeric textbox control, if the parse fails and we are here,
                // the textbox is blank.
                MessageHelper.ShowMessage(this, "Please enter your OnTrac account number.");
                e.NextPage = CurrentPage;

                return;
            }

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var authenticationRequest = new OnTracAuthentication(accountNumberValue, password.Text.Trim());

                authenticationRequest.IsValidUser();

                if (account == null)
                {
                    account = new OnTracAccountEntity();
                    account.CountryCode = "US";
                }

                account.AccountNumber = accountNumberValue;
                account.Password = SecureText.Encrypt(password.Text, accountNumberValue.ToString());
            }
            catch (OnTracException ex)
            {
                MessageHelper.ShowError(this, ex.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Next pressed on contact screen
        /// </summary>
        private void OnStepNextContactInfo(object sender, WizardStepEventArgs e)
        {
            if (!contactInformation.ValidateRequiredFields())
            {
                e.NextPage = CurrentPage;
                return;
            }

            PersonAdapter personAdapter = new PersonAdapter(account, "");
            contactInformation.SaveToEntity(personAdapter);

            account.Description = OnTracAccountManager.GetDefaultDescription(account);

            OnTracAccountManager.SaveAccount(account);
        }

        /// <summary>
        /// Wizard is finishing
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();
            optionsControl.SaveSettings(settings);
            ShippingSettings.Save(settings);

            OnTracAccountManager.SaveAccount(account);
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && account != null)
            {
                OnTracAccountManager.DeleteAccount(account);
            }
        }
    }
}