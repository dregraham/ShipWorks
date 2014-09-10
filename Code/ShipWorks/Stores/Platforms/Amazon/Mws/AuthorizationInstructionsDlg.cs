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
        private readonly string amazonApiRegion;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="amazonApiRegion"></param>
        public AuthorizationInstructionsDlg(string amazonApiRegion)
        {
            InitializeComponent();

            this.amazonApiRegion = amazonApiRegion;
            mwsLink.Text = DeveloperUrl + ".";
            accountNumber.Text = AccountNumber;
        }

        /// <summary>
        /// Developer account number for access to Amazon API
        /// </summary>
        public string AccountNumber
        {
            get
            {
                switch (amazonApiRegion)
                {
                    case "US":
                        return "1025-5115-6476";
                    case "CA":
                        return "1025-5115-6476";
                    default:
                        return "2814-9468-9452";
                }
            }
        }

        /// <summary>
        /// Url for accessing the Amazon developer portal
        /// </summary>
        public string DeveloperUrl
        {
            get
            {
                switch (amazonApiRegion)
                {
                    case "US":
                        return "http://developer.amazonservices.com";
                    case "CA":
                        return "http://developer.amazonservices.ca";
                    default:
                        return "http://developer.amazonservices.co.uk";
                }
            }
        }

        /// <summary>
        /// Open the MWS registration page
        /// </summary>
        private void OnMWSLinkClick(object sender, EventArgs e)
        {
            WebHelper.OpenUrl(DeveloperUrl, this);
        }

        /// <summary>
        /// The entered and authenticated Merchant ID
        /// </summary>
        public string MerchantID
        {
            get { return merchantID.Text; }
        }

        /// <summary>
        /// The entered MWS Auth Token
        /// </summary>
        public string AuthToken
        {
            get { return authToken.Text; }
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

            if (string.IsNullOrWhiteSpace(authToken.Text))
            {
                MessageHelper.ShowMessage(this, "Please enter your MWS Auth Token.");
                return;
            }

            DialogResult = DialogResult.OK;
        }
    }
}
