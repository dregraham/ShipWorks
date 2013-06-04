using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Settings.WizardPages
{
    /// <summary>
    /// Wizard page for setting up automated processing actions
    /// </summary>
    public partial class ShippingWizardPageAutomation : WizardPage
    {
        ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingWizardPageAutomation(ShipmentType shipmentType)
        {
            InitializeComponent();

            this.shipmentType = shipmentType;
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            automationControl.EnsureInitialized(shipmentType.ShipmentTypeCode);
        }

        /// <summary>
        /// Stepping next from the page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            automationControl.SaveSettings();
        }
    }
}
