using System;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.WizardPages;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Setup wizard for the "Other" service
    /// </summary>
    [KeyedComponent(typeof(ShipmentTypeSetupWizardForm), ShipmentTypeCode.Other)]
    public partial class OtherSetupWizard : ShipmentTypeSetupWizardForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OtherSetupWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(ShipmentTypeCode.Other);

            Pages.Add(new ShippingWizardPageOrigin(shipmentType));
            Pages.Add(new ShippingWizardPageDefaults(shipmentType));
            Pages.Add(new ShippingWizardPageAutomation(shipmentType));
            Pages.Add(new ShippingWizardPageFinish(shipmentType));
        }
    }
}
