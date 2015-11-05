using System;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;
using ShipWorks.UI.Wizard;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Setup wizard for Amazon shipment type
    /// </summary>
    public partial class AmazonShipmentSetupWizard : ShipmentTypeSetupWizardForm
    {
        private readonly AmazonShipmentType shipmentType;
        private readonly IShippingSettings shippingSettings;
        private ShippingWizardPageFinish shippingWizardPageFinish;

        /// <summary>
        /// Constructor to be used by Visual Studio designer
        /// </summary>
        protected AmazonShipmentSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shipmentType"></param>
        /// <param name="credentialViewModel"></param>
        /// <param name="accountManager"></param>
        /// <param name="shippingSettings"></param>
        public AmazonShipmentSetupWizard(AmazonShipmentType shipmentType, IShippingSettings shippingSettings) : this()
        {
            this.shipmentType = shipmentType;
            this.shippingSettings = shippingSettings;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            HelpLink.LinkClicked += (x, y) => WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/4000066194", this);

            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(CreateFinishPage());
        }

        /// <summary>
        /// Create the finish wizard page
        /// </summary>
        private WizardPage CreateFinishPage()
        {
            shippingWizardPageFinish = new ShippingWizardPageFinish(shipmentType);
            shippingWizardPageFinish.SteppingInto += OnSteppingIntoFinish;
            return shippingWizardPageFinish;
        }

        /// <summary>
        /// Stepping next from the credentials page
        /// </summary>
        private void OnNextStepCredentials(object sender, WizardStepEventArgs e)
        {

        }

        /// <summary>
        /// Next pressed on contact screen
        /// </summary>
        private void OnStepNextContactInfo(object sender, WizardStepEventArgs e)
        {
            if (!contactInformation.ValidateRequiredFields())
            {
                e.NextPage = CurrentPage;
            }
        }

        /// <summary>
        /// Finish the wizard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSteppingIntoFinish(object sender, WizardSteppingIntoEventArgs e)
        {
            shippingSettings.MarkAsConfigured(ShipmentTypeCode.Amazon);

            //TODO: Enable this logic when we implement profiles for Amazon
            //// If this is the only account, update this shipment type profiles with this account
            //List<AmazonAccountEntity> accounts = accountManager.Accounts.ToList();

            //if (accounts.Count == 1)
            //{
            //    AmazonAccountEntity accountEntity = accounts.First();

            //    // Update any profiles to use this account if this is the only account
            //    // in the system. This is to account for the situation where there a multiple
            //    // profiles that may be associated with a previous account that has since
            //    // been deleted. 
            //    foreach (ShippingProfileEntity shippingProfileEntity in ShippingProfileManager.Profiles.Where(p => p.ShipmentType == (int)ShipmentTypeCode.OnTrac))
            //    {
            //        if (shippingProfileEntity.OnTrac.OnTracAccountID.HasValue)
            //        {
            //            shippingProfileEntity.OnTrac.OnTracAccountID = accountEntity.OnTracAccountID;
            //            ShippingProfileManager.SaveProfile(shippingProfileEntity);
            //        }
            //    }
            //}
        }
    }
}
