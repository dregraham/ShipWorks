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
    /// Last page in shipping setup wizard
    /// </summary>
    public partial class ShippingWizardPageFinish : WizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingWizardPageFinish(ShipmentType shipmentType)
        {
            InitializeComponent();

            labelSuccess.Text = string.Format(labelSuccess.Text, shipmentType.ShipmentTypeName);
            Description = string.Format(Description, shipmentType.ShipmentTypeName);
        }
    }
}
