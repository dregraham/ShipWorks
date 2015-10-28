using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Shows a list of carriers that need terms and conditions to be accepted.
    /// </summary>
    public partial class AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl : RateFootnoteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl" /> class.
        /// </summary>
        public AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl(List<string> carrierNames)
        {
            InitializeComponent();

            // Property used for unit testing.
            CarrierNames = carrierNames;
        }

        /// <summary>
        /// List of carrier names the control received.
        /// Used for unit testing.
        /// </summary>
        public List<string> CarrierNames { private set; get; }

        private void openDialogLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AmazonCarrierTermsAndConditionsNotAcceptedDialog dialog = new AmazonCarrierTermsAndConditionsNotAcceptedDialog(CarrierNames);
            dialog.ShowDialog();
        }
    }
}
