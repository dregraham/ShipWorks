using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
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
        /// Initializes a new instance of the <see cref="AmazonNotLinkedFootnoteControl"/> class.
        /// </summary>
        public AmazonNotLinkedFootnoteControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [click information link] - Shows the dialog
        /// </summary>
        private void OnClickInfoLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AmazonNotLinkedDlg dlg = new AmazonNotLinkedDlg(shipmentTypeCode);
            dlg.ShowDialog();
        }
    }
}
