using System;
using System.Reflection.Emit;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Net;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Settings;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Profiles;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// The setup wizard used for adding a new i-parcel account to ShipWorks.
    /// </summary>
    public partial class iParcelSetupWizard : ShipmentTypeSetupWizardForm
    {
        private readonly IParcelAccountEntity iParcelAccount;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="iParcelSetupWizard" /> class.
        /// </summary>
        public iParcelSetupWizard()
        {
            InitializeComponent();

            iParcelAccount = new IParcelAccountEntity();
            iParcelAccount.CountryCode = "US";
        }

        /// <summary>
        /// Called when the form is loaded to add additional shipping wizard pages and load up the 
        /// settings into the options control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.iParcel);

            iParcelOptionsControl.LoadSettings();

            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(new ShippingWizardPageFinish(shipmentType));

            if (ShippingManager.IsShipmentTypeConfigured(ShipmentTypeCode.iParcel))
            {
                Pages.Remove(wizardPageOptions);
            }

            // Wire up the stepping into event of the finish page to the event handler
            Pages[Pages.Count - 1].SteppingInto += new EventHandler<WizardSteppingIntoEventArgs>(OnSteppingIntoFinish);
        }

        /// <summary>
        /// Called when the registration link is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void OnClickRegistrationLink(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("https://www.i-parcel.com/en/contact-us", this);
        }

        /// <summary>
        /// Called when stepping next from the credentials page
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardStepEventArgs" /> instance containing the event data.</param>
        private void OnStepNextCredentials(object sender, WizardStepEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                iParcelCredentials credentials = new iParcelCredentials(username.Text, password.Text, false, new iParcelServiceGateway());
                credentials.Validate();

                // We have a valid username/password, so save it to the account entity
                credentials.SaveToEntity(iParcelAccount);
            }
            catch (iParcelException exception)
            {
                MessageHelper.ShowError(this, exception.Message);
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Called when stepping next from the contact info page.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="WizardStepEventArgs" /> instance containing the event data.</param>
        private void OnStepNextContactInfo(object sender, WizardStepEventArgs e)
        {
            if (contactInformation.ValidateRequiredFields())
            {
                PersonAdapter personAdapter = new PersonAdapter(iParcelAccount, string.Empty);
                contactInformation.SaveToEntity(personAdapter);

                iParcelAccount.Description = iParcelAccountManager.GetDefaultDescription(iParcelAccount);
                iParcelAccountManager.SaveAccount(iParcelAccount);
            }
            else
            {
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Wizard is finishing
        /// </summary>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            iParcelOptionsControl.SaveSettings();
            ShippingSettings.Save(settings);

            iParcelAccountManager.SaveAccount(iParcelAccount);
        }

        /// <summary>
        /// The window is closing
        /// </summary>
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK && iParcelAccount != null && !iParcelAccount.IsNew)
            {
                iParcelAccountManager.DeleteAccount(iParcelAccount);
            }
            else if (DialogResult == DialogResult.OK)
            {
                // We need to clear out the rate cache since rates (especially best rate) are no longer valid now
                // that a new account has been added.
                RateCache.Instance.Clear();

                if (iParcelAccountManager.Accounts.Count == 1)
                {
                    // Update any profiles to use this iparcel account if this is the only account
                    // in the system. This is to account for the situation where there a multiple
                    // profiles that may be associated with a previous iparcel account that has since
                    // been deleted. 
                    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)ShipmentTypeCode.iParcel))
                    {
                        if (shippingProfileEntity.IParcel.IParcelAccountID.HasValue)
                        {
                            shippingProfileEntity.IParcel.IParcelAccountID = iParcelAccount.IParcelAccountID;
                            ShippingProfileManager.SaveProfile(shippingProfileEntity);
                        }
                    }

                    // Make sure the shipment is marked as configured and activated
                    ShippingSettings.MarkAsActivated(ShipmentTypeCode.iParcel);
                    ShippingSettings.MarkAsConfigured(ShipmentTypeCode.iParcel);
                }

            }
        }
    }
}
