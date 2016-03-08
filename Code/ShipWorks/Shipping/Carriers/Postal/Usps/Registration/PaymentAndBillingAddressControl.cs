using System;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    public partial class PaymentAndBillingAddressControl : UserControl
    {
        private const string CreditCardPaymentMethod = "Credit Card";

        public PaymentAndBillingAddressControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoad(object sender, EventArgs e)
        {
            // Load the card types
            cardType.Items.Add(new CreditCardTypeDropDownItem(CreditCardType.Visa, "Visa"));
            cardType.Items.Add(new CreditCardTypeDropDownItem(CreditCardType.MasterCard, "MasterCard"));
            cardType.Items.Add(new CreditCardTypeDropDownItem(CreditCardType.AmericanExpress, "American Express"));
            cardType.Items.Add(new CreditCardTypeDropDownItem(CreditCardType.Discover, "Discover"));
            cardType.SelectedIndex = 0;

            // Set the minimum/maximum value of the credit card expiration month and year to adjust
            // for the current date
            creditCardExpirationMonth.Value = DateTime.Now.Month;
            creditCardExpirationYear.Minimum = DateTime.Now.Year;
            creditCardExpirationYear.Maximum = creditCardExpirationYear.Minimum + 15;
        }

        /// <summary>
        /// Gets the credit card expiration year.
        /// </summary>
        private int CreditCardExpirationYear
        {
            get { return Convert.ToInt32(creditCardExpirationYear.Value); }
        }

        /// <summary>
        /// Gets the credit card expiration month.
        /// </summary>
        private int CreditCardExpirationMonth
        {
            get { return Convert.ToInt32(creditCardExpirationMonth.Value); }
        }

        /// <summary>
        /// Gets the type of the card.
        /// </summary>
        private CreditCardType CardType
        {
            get { return ((CreditCardTypeDropDownItem)cardType.SelectedItem).CardType; }
        }

        /// <summary>
        /// Gets the credit card and information provided if the selected payment is credit card. A null
        /// value is returned if the selected payment method is not credit card.
        /// </summary>
        public CreditCard CreditCard
        {
            get
            {
                return new CreditCard()
                {
                    AccountNumber = creditCardNumber.Text,
                    ExpirationDate = new DateTime(CreditCardExpirationYear, CreditCardExpirationMonth, 1),
                    CreditCardType = CardType,
                    BillingAddress = new Address()
                    {
                        FullName = cardholderName.Text,
                    }
                };
            }
        }

        /// <summary>
        /// Validates the payment data.
        /// </summary>
        /// <returns><code>true</code>if the validation was successful; otherwise <code>false</code>.</returns>
        public bool ValidateData()
        {
            bool isValid = true;
            StringBuilder validationMessages = addressControl.GetValidationErrors();

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
