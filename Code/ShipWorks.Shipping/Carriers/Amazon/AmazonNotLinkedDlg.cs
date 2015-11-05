using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Dialog shown when AmazonNotLinked footer is clicked
    /// </summary>
    public partial class AmazonNotLinkedDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonNotLinkedDlg"/> class.
        /// </summary>
        /// <param name="accountToDisplay">The account to display.</param>
        public AmazonNotLinkedDlg(string accountToDisplay)
        {
            InitializeComponent();

            MessageLink.Text = string.Format(MessageLink.Text, accountToDisplay);

            SetLinkArea();
        }

        /// <summary>
        /// Sets the link area.
        /// </summary>
        private void SetLinkArea()
        {
            int startLink = MessageLink.Text.IndexOf("linked correctly", StringComparison.OrdinalIgnoreCase);
            int linkLength = MessageLink.Text.Length - startLink;
            MessageLink.LinkArea = new LinkArea(startLink, linkLength);
        }

        /// <summary>
        /// Called when [message link clicked].
        /// </summary>
        private void OnMessageLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            WebHelper.OpenUrl("http://support.shipworks.com/support/solutions/articles/4000066194", this);
        }

        /// <summary>
        /// Close Clicked
        /// </summary>
        private void OnClickOkButton(object sender, EventArgs e) => Close();
    }
}
