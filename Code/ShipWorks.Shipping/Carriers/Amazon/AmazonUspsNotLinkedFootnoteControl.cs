using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public partial class AmazonUspsNotLinkedFootnoteControl : RateFootnoteControl
    {
        private readonly string accountTypeToDisplay;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonUspsNotLinkedFootnoteControl"/> class.
        /// </summary>
        public AmazonUspsNotLinkedFootnoteControl(string accountTypeToDisplay)
        {
            InitializeComponent();

            this.accountTypeToDisplay = accountTypeToDisplay;
            infoLink.Text = string.Format(infoLink.Text, accountTypeToDisplay);

            SetLinkArea();
        }

        /// <summary>
        /// Sets the link area.
        /// </summary>
        private void SetLinkArea()
        {
            int startLink = infoLink.Text.IndexOf("here", StringComparison.OrdinalIgnoreCase);
            int linkLength = infoLink.Text.Length - startLink - 1;
            infoLink.LinkArea = new LinkArea(startLink, linkLength);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonUspsNotLinkedFootnoteControl"/> class.
        /// </summary>
        public AmazonUspsNotLinkedFootnoteControl()
        {
            InitializeComponent();
        }

        private void OnClickInfoLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AmazonNotLinkedDlg dlg = new AmazonNotLinkedDlg(accountTypeToDisplay);
            dlg.ShowDialog();
        }
    }
}
