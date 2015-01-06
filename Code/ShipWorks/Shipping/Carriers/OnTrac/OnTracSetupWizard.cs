using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Net.Authentication;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Setup wizard for processing shipments with OnTrac
    /// </summary>
    public partial class OnTracSetupWizard : ShipmentTypeSetupWizardForm
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

            if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.OnTrac))
            {
                Pages.Remove(wizardPageOptions);
            }
            else
            {
                wizardPageOptions.StepNext += (o, args) => optionsControl.SaveSettings();
            }

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

            if (personAdapter.CountryCode != "US")
            {
                MessageHelper.ShowInformation(this, "OnTrac only supports US addresses.");
                e.NextPage = CurrentPage;
                return;
            }

            account.Description = OnTracAccountManager.GetDefaultDescription(account);

            OnTracAccountManager.SaveAccount(account);
        }

        /// <summary>
        /// Wizard is finishing
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            OnTracAccountManager.SaveAccount(account);

            // Mark the new account as configured
            ShippingSettings.MarkAsConfigured(ShipmentTypeCode.OnTrac);

            // If this is the only account, update this shipment type profiles with this account
            List<OnTracAccountEntity> accounts = OnTracAccountManager.Accounts;
            if (accounts.Count == 1)
            {
                OnTracAccountEntity accountEntity = accounts.First();

                // Update any profiles to use this account if this is the only account
                // in the system. This is to account for the situation where there a multiple
                // profiles that may be associated with a previous account that has since
                // been deleted. 
                foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)ShipmentTypeCode.OnTrac))
                {
                    if (shippingProfileEntity.OnTrac.OnTracAccountID.HasValue)
                    {
                        shippingProfileEntity.OnTrac.OnTracAccountID = accountEntity.OnTracAccountID;
                        ShippingProfileManager.SaveProfile(shippingProfileEntity);
                    }
                }
            }
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
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();
            }
        }
    }
}