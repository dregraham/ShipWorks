using System.Collections.Generic;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Shows a message indicating that additional rates are available, and a link to open a dialog box that shows a the list of carriers
    /// for which the terms and conditions have not been accepted
    /// </summary>
    public partial class AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl : RateFootnoteControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl" /> class.
        /// </summary>
        public AmazonCarrierTermsAndConditionsNotAcceptedFootnoteControl(IEnumerable<string> carrierNames)
        {
            InitializeComponent();

            // Property used for unit testing.
            CarrierNames = carrierNames;
        }

        /// <summary>
        /// List of carrier names the control received.
        /// Used for unit testing.
        /// </summary>
        public IEnumerable<string> CarrierNames { private set; get; }

        /// <summary>
        /// Creates and opens an <see cref="AmazonCarrierTermsAndConditionsNotAcceptedDialog"/>
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void openDialogLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AmazonCarrierTermsAndConditionsNotAcceptedDialog dialog = new AmazonCarrierTermsAndConditionsNotAcceptedDialog(CarrierNames);
            dialog.ShowDialog();
        }
    }
}
