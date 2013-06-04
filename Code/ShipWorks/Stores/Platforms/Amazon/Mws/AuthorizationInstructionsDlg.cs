using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    public partial class AuthorizationInstructionsDlg : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AuthorizationInstructionsDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Open the MWS registration page
        /// </summary>
        private void OnMWSLinkClick(object sender, EventArgs e)
        {
            WebHelper.OpenUrl("http://developer.amazonservices.com", this);
        }

        /// <summary>
        /// The entered and authenticated Merchant ID
        /// </summary>
        public string MerchantID
        {
            get { return merchantID.Text; }
        }

        /// <summary>
        /// Closing the window
        /// </summary>
        private void OnOK(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(merchantID.Text))
            {
                MessageHelper.ShowMessage(this, "Please enter your Amazon Merchant ID.");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
