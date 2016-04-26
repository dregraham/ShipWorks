using System;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// Dialog allowing customer to choose hold location close to the distination address.
    /// </summary>
    public partial class UpsOpenAccountInvalidAddressDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsOpenAccountInvalidAddressDlg" /> class.
        /// </summary>
        public UpsOpenAccountInvalidAddressDlg()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Formats the address result for display.
        /// </summary>
        public void SetAddress(AddressKeyCandidateType suggestedAddress, string addressName)
        {
            addressLine1.Text = suggestedAddress.StreetAddress;
            addressLine2.Text = string.Format("{0}, {1} {2}",
                suggestedAddress.City,
                suggestedAddress.State,
                suggestedAddress.PostalCode);
            country.Text = suggestedAddress.CountryCode;

            labelInstructions.Text = string.Format("UPS does not consider the {0} address as valid. UPS suggests the following address: ", addressName);
        }

        /// <summary>
        /// Clicks the ok button.
        /// </summary>
        private void ClickOkButton(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Clicks the cancel button.
        /// </summary>
        private void ClickCancelButton(object sender, EventArgs e)
        {
            Close();
        }
    }
}