using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;

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
        /// <param name="shipmentTypeCode"></param>
        public AmazonNotLinkedDlg(ShipmentTypeCode shipmentTypeCode)
        {
            Debug.Assert(shipmentTypeCode == ShipmentTypeCode.Usps || shipmentTypeCode == ShipmentTypeCode.UpsOnLineTools);

            InitializeComponent();

            SetLinkArea(shipmentTypeCode);
        }

        /// <summary>
        /// Sets the link area.
        /// </summary>
        /// <param name="shipmentTypeCode"></param>
        private void SetLinkArea([Obfuscation(Exclude = true)] ShipmentTypeCode shipmentTypeCode)
        {
            string providerType;
            string extraMessaging;

            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Usps:
                    providerType = "Stamps.com";
                    extraMessaging = string.Empty;
                    break;
                case ShipmentTypeCode.UpsOnLineTools:
                    providerType = "UPS";
                    extraMessaging = " and has been added to ShipWorks";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shipmentTypeCode), shipmentTypeCode, "Only supports Stamps.com and UPS");
            }

            const string linkText = "Amazon Seller Central";

            MessageLink.Text = $"Shipworks could not retrieve {EnumHelper.GetDescription(shipmentTypeCode)} rates. Please confirm your {providerType} account is linked correctly in {linkText}{extraMessaging}. ";

            MessageLink.LinkArea = new LinkArea(
                MessageLink.Text.IndexOf(linkText, StringComparison.OrdinalIgnoreCase),
                linkText.Length);
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