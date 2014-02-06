using System;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Setup wizard for USPS w/o Postage shipping
    /// </summary>
    public partial class PostalWebSetupWizard : ShipmentTypeSetupWizardForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PostalWebSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.PostalWebTools);

            Pages.Add(new ShippingWizardPageOrigin(shipmentType));
            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPagePrinting(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(new ShippingWizardPageFinish(shipmentType));
        }
    }
}
