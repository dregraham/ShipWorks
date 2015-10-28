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
        public AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl(List<string> carrierNames)
        {
            InitializeComponent();

            // Property used for unit testing.
            CarrierNames = carrierNames;

            carrierNamesMessageLabel.Text = $"Terms and conditions have not been accepted for {string.Join(", ", CarrierNames)}.";
        }

        /// <summary>
        /// List of carrier names the control received.
        /// Used for unit testing.
        /// </summary>
        public List<string> CarrierNames { private set; get; }
    }
}
