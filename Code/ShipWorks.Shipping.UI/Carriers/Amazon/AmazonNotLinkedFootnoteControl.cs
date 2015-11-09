using System;
using System.Diagnostics;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Control to show an account not linked dialog
    /// </summary>
    public partial class AmazonNotLinkedFootnoteControl : RateFootnoteControl
    {
        private readonly ShipmentTypeCode shipmentTypeCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonNotLinkedFootnoteControl"/> class.
        /// </summary>
        public AmazonNotLinkedFootnoteControl(ShipmentTypeCode shipmentTypeCode)
        {
            this.shipmentTypeCode = shipmentTypeCode;
            Debug.Assert(shipmentTypeCode == ShipmentTypeCode.Usps || shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools);

            InitializeComponent();

            SetLinkText();
        }

        /// <summary>
        /// Sets the link area.
        /// </summary>
        private void SetLinkText()
        {
            infoLink.Text = string.Format(infoLink.Text, EnumHelper.GetDescription(shipmentTypeCode));
            int startLink = infoLink.Text.IndexOf("here", StringComparison.OrdinalIgnoreCase);
            int linkLength = infoLink.Text.Length - startLink - 1;
            infoLink.LinkArea = new LinkArea(startLink, linkLength);
        }

        /// <summary>
        /// Called when [click information link] - Shows the dialog
        /// </summary>
        private void OnClickInfoLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (AmazonNotLinkedDlg dlg = new AmazonNotLinkedDlg(shipmentTypeCode))
            {
                dlg.ShowDialog();
            }
        }
    }
}
