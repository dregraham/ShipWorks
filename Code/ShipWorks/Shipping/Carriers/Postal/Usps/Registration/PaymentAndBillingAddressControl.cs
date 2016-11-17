using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// Address control for the payment and billing screen
    /// </summary>
    public partial class PaymentAndBillingAddressControl : UserControl
    {
        private readonly Dictionary<CreditCardTypeInternal, CreditCardType> creditCardTranslation =
            new Dictionary<CreditCardTypeInternal, CreditCardType>
            {
                { CreditCardTypeInternal.AmericanExpress, CreditCardType.AmericanExpress },
                { CreditCardTypeInternal.Discover, CreditCardType.Discover },
                { CreditCardTypeInternal.MasterCard, CreditCardType.MasterCard },
                { CreditCardTypeInternal.Visa, CreditCardType.Visa },
            };

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
        public int CreditCardExpirationYear
        {
            get
            {
                return string.IsNullOrWhiteSpace(creditCardExpirationYear.Text) ?
                    0 :
                    Convert.ToInt32(creditCardExpirationYear.Value);
            }
        }

        /// <summary>
        /// Gets the credit card expiration month.
        /// </summary>
        public int CreditCardExpirationMonth
        {
            get
            {
                return string.IsNullOrWhiteSpace(creditCardExpirationMonth.Text) ?
                    0 :
                    Convert.ToInt32(creditCardExpirationMonth.Value);
            }
        }

        /// <summary>
        /// Gets the type of the card.
        /// </summary>
        public CreditCardType CardType => creditCardTranslation[(CreditCardTypeInternal) cardType.SelectedValue];

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
