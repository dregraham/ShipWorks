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
    /// Wizard page for setting up the defaults for a shipment type
    /// </summary>
    public partial class ShippingWizardPageDefaults : WizardPage
    {
        ShipmentType shipmentType;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingWizardPageDefaults(ShipmentType shipmentType)
        {
            InitializeComponent();

            this.shipmentType = shipmentType;
        }

        /// <summary>
        /// Stepping into the wizard page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            LoadSettings();
        }

        /// <summary>
        /// Step next to the next page 
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            defaultsControl.SaveSettings();
        }

        /// <summary>
        /// Loads the settings for the defaults control
        /// </summary>
        public void LoadSettings()
        {
            if (shipmentType != null)
            {
                defaultsControl.LoadSettings(shipmentType);
                shipmentType = null;
            }
        }
    }
}
