using System;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    public partial class PaymentAndBillingAddressControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentAndBillingAddressControl"/> class.
        /// </summary>
        public PaymentAndBillingAddressControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            EnumHelper.BindComboBox<CreditCardType>(cardType);

            // Set the minimum/maximum value of the credit card expiration month and year to adjust
            // for the current date
            creditCardExpirationMonth.Value = DateTime.Now.Month;
            creditCardExpirationYear.Minimum = DateTime.Now.Year;
            creditCardExpirationYear.Maximum = creditCardExpirationYear.Minimum + 15;
        }

        /// <summary>
        /// Gets the credit card expiration year.
        /// </summary>
        public int CreditCardExpirationYear
        {
            get { return Convert.ToInt32(creditCardExpirationYear.Value); }
        }

        /// <summary>
        /// Gets the credit card expiration month.
        /// </summary>
        public int CreditCardExpirationMonth
        {
            get { return Convert.ToInt32(creditCardExpirationMonth.Value); }
        }

        /// <summary>
        /// Gets the type of the card.
        /// </summary>
        public CreditCardType CardType => (CreditCardType) cardType.SelectedValue;

        /// <summary>
        /// Gets the name of the card holder.
        /// </summary>
        public string CardHolderName => cardholderName.Text;

        /// <summary>
        /// Gets the card number.
        /// </summary>
        public string CardNumber => creditCardNumber.Text;

        /// <summary>
        /// Gets the billing address.
        /// </summary>
        public PersonAdapter BillingAddress
        {
            get
            {
                PersonAdapter billingAddressAdapter = new PersonAdapter();
                billingAddress.SaveToEntity(billingAddressAdapter);

                return billingAddressAdapter;
            }
        }

        /// <summary>
        /// Validates the payment data.
        /// </summary>
        /// <returns><code>true</code>if the validation was successful; otherwise <code>false</code>.</returns>
        public bool ValidateData()
        {
            bool isValid = true;
            if (!billingAddress.ValidateRequiredFields())
            {
                return false;
            }

            StringBuilder validationMessages = new StringBuilder();

            if (string.IsNullOrEmpty(cardholderName.Text))
            {
                validationMessages.AppendLine("Cardholder name");
            }

            if (CreditCardExpirationMonth < 1 || CreditCardExpirationMonth > 12)
            {
                validationMessages.AppendLine("Credit card expiration month");
            }

            if (CreditCardExpirationYear < DateTime.Now.Year)
            {
                validationMessages.AppendLine("Credit card expiration year");
            }

            if (string.IsNullOrEmpty(creditCardNumber.Text))
            {
                validationMessages.AppendLine("Credit card number");
            }

            if (validationMessages.Length > 0)
            {
                string message = $"Please correct the following field(s):{Environment.NewLine}{validationMessages}";

                MessageHelper.ShowInformation(this, message);

                isValid = false;
            }

            return isValid;
        }
    }
}
