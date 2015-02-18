using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// A dialog for activating the USPS shipment type and creating 
    /// a new account or converting an existing account.
    /// </summary>
    public partial class UspsActivateDiscountDlg : Form, IDiscountedAccountDlg
    {
        private IRegistrationPromotion promotion;
        private ShipmentEntity shipment;

        private readonly string singleAccountMarketingText = 
            "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks and " +
            "IntuiShip, all through one single USPS account. {0}"
            + Environment.NewLine + Environment.NewLine + "To get these discounts, you " +
            "just need to open a free USPS account which will enable you to easily print both USPS Priority Mail " +
            "and Priority Mail Express labels and First Class shipping labels, all within one account.";

        private readonly string normalDescription =
            "You can now save up to 46% on USPS Priority Mail and Priority Mail Express Shipments with ShipWorks " +
            "and IntuiShip, all through one single Stamps.com account." + Environment.NewLine + Environment.NewLine +
            "To get these discounts, you just need to open a Stamps.com account which will enable you to easily " +
            "print both USPS Priority Mail and Priority Mail Express labels and First Class shipping labels.";

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsActivateDiscountDlg"/> class.
        /// </summary>
        public UspsActivateDiscountDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the the form based on the given shipment.
        /// </summary>
        /// <param name="shipmentEntity">The shipment.</param>
        /// <param name="showSingleAccountMarketing"></param>
        public virtual void Initialize(ShipmentEntity shipmentEntity, bool showSingleAccountMarketing)
        {
            shipment = shipmentEntity;
            promotion = new RegistrationPromotionFactory().CreateRegistrationPromotion();

            labelDiscountInfo.Text = showSingleAccountMarketing ? 
                BuildSingleAccountMarketingMessage() : 
                normalDescription;
        }

        /// <summary>
        /// Initiate the signup for a new Express1 account
        /// </summary>
        private void OnSignup(object sender, EventArgs e)
        {
            ShippingSettings.CheckForChangesNeeded();

            using (UspsSetupWizard setupWizard = new UspsSetupWizard(promotion, false))
            {
                // Pre-load the account address details
                setupWizard.InitialAccountAddress = GetDefaultAccountPerson();

                bool accountWasCreated = CreateAccount(setupWizard);

                if (accountWasCreated)
                {
                    ConvertShipmentToUsps();
                    Close();
                }
            }
        }

        /// <summary>
        /// Prompt the user to create the Usps account
        /// </summary>
        private bool CreateAccount(UspsSetupWizard setupWizard)
        {
            UspsShipmentType shipmentType = new UspsShipmentType();

            return ShippingManager.IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode) ? 
                setupWizard.ShowDialog(this) == DialogResult.OK : 
                ShipmentTypeSetupControl.SetupShipmentType(this, shipmentType.ShipmentTypeCode, setupWizard);
        }

        /// <summary>
        /// Update the shipment to use Usps
        /// </summary>
        private void ConvertShipmentToUsps()
        {
            ShippingManager.RefreshShipment(shipment);

            // Only way we should require a signup is not already using a USPS account for 
            // this shipment, so we need to change the shipment type to USPS
            // in order to take advantage of the new rates (since USPS API doesn't match 
            // with Endicia API and shipment configurations differ).
            shipment.ShipmentType = (int) ShipmentTypeCode.Usps;
            ShippingManager.SaveShipment(shipment);

            // Now that the shipment has been updated, we need to broadcast that the shipping 
            // settings have been changed, so any listeners have a chance to react
            ShippingSettingsEventDispatcher.DispatchUspsAutomaticExpeditedChanged(this, new ShippingSettingsEventArgs((ShipmentTypeCode) shipment.ShipmentType));

            RateCache.Instance.Clear();
        }

        /// <summary>
        /// Learn more about using Expedited with USPS provider
        /// </summary>
        private void OnExpedited1LearnMore(object sender, EventArgs e)
        {
            MessageHelper.ShowInformation(this,
                                          "With IntuiShip you get some of the best postal rates available, saving you significant money on each of your " +
                                          "domestic and international Priority and Express shipments." + Environment.NewLine + Environment.NewLine +
                                          "Simply create a USPS account and ShipWorks will automatically utilize it for discounted rates from " +
                                          "IntuiShip when creating postage labels." + Environment.NewLine + Environment.NewLine + "For more information, " +
                                          "please contact us at www.interapptive.com/company/contact.html.");
        }

        /// <summary>
        /// Gets a person to use as the default for new USPS accounts
        /// </summary>
        private static PersonAdapter GetDefaultAccountPerson()
        {
            List<UspsAccountEntity> accounts = UspsAccountManager.GetAccounts(UspsResellerType.None);
            return accounts.Count == 1 ? new PersonAdapter(accounts.Single(), "") : null;
        }

        /// <summary>
        /// Build the message that will be used for single account marketing
        /// </summary>
        private string BuildSingleAccountMarketingMessage()
        {
            string express1TargetedText = string.Empty;

            if (EndiciaAccountManager.Express1Accounts.Any() || UspsAccountManager.Express1Accounts.Any())
            {
                express1TargetedText = "No more switching between accounts to get the lowest rates!";
            }

            return string.Format(singleAccountMarketingText, express1TargetedText);
        }
    }
}
