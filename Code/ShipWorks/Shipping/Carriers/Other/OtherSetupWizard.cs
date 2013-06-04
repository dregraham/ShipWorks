using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Data.Connection;
using ShipWorks.Actions;
using ShipWorks.Shipping.Settings;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Settings.WizardPages;

namespace ShipWorks.Shipping.Carriers.Other
{
    /// <summary>
    /// Setup wizard for the "Other" service
    /// </summary>
    public partial class OtherSetupWizard : WizardForm
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
