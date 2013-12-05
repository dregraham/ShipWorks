using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Properties;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Setup wizard for initial shipping setup
    /// </summary>
    public partial class ShippingSetupWizard : WizardForm
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingSetupWizard()
        {
            InitializeComponent();
        }

        #region Packing Slips

        /// <summary>
        /// Stepping into the packing slips page
        /// </summary>
        private void OnSteppingIntoPackingSlips(object sender, WizardSteppingIntoEventArgs e)
        {
            OnChangeIncludePackingSlip(null, EventArgs.Empty);
        }

        /// <summary>
        /// Changing whether to include packing slips with each shipping label
        /// </summary>
        private void OnChangeIncludePackingSlip(object sender, EventArgs e)
        {
            // Packing slips included
            if (includePackingSlip.Checked)
            {
                // picturePackingSlip.Image = (labelPrinterType.Technology == PrinterTechnology.Thermal) ? Resources.thermal_roll_w_packing_slip : Resources.document_plain_packing_slip;
            }
            // No packing slip
            else
            {
                // picturePackingSlip.Image = (labelPrinterType.Technology == PrinterTechnology.Thermal) ? Resources.thermal_roll_w_shipping_label : Resources.document_plain_shipping_labels;
            }
        }

        #endregion

    }
}
