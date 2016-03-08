using System;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    public partial class SimpleAddressControl : UserControl
    {
        public SimpleAddressControl()
        {
            InitializeComponent();
        }

        public StringBuilder GetValidationErrors()
        {
            StringBuilder validationMessages = new StringBuilder();

            if (string.IsNullOrEmpty(street.Text))
            {
                validationMessages.AppendLine("Street");
            }

            if (string.IsNullOrEmpty(city.Text))
            {
                validationMessages.AppendLine("City");
            }

            if (string.IsNullOrEmpty(state.Text))
            {
                validationMessages.AppendLine("State");
            }

            if (string.IsNullOrEmpty(postalCode.Text))
            {
                validationMessages.AppendLine("Postal code");
            }

            if (validationMessages.Length > 0)
            {
                string message = $"Please correct the following field(s):{Environment.NewLine}{validationMessages}";

                MessageHelper.ShowInformation(this, message);
            }

            return validationMessages;
        }
    }
}
