using System;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// Control for collecting Stamp.com payment information.
    /// </summary>
    public partial class UspsPaymentControl : UserControl
    {
        private const string CreditCardPaymentMethod = "Credit Card";
        private const string AchAccountPaymentMethod = "ACH Account";

        private const string CheckingAccountTypeName = "Checking";
        private const string SavingsAccountTypeName = "Savings";

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsPaymentControl"/> class.
        /// </summary>
        public UspsPaymentControl()
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
            // Move the ACH panel where it needs to be
            panelAchAccount.Top = panelCreditCardInfo.Top;

            // Load the available payment types
            paymentMethod.Items.Add(CreditCardPaymentMethod);
            paymentMethod.Items.Add(AchAccountPaymentMethod);
            paymentMethod.SelectedIndex = 0;

            // Load the account types
            achAccountType.Items.Add(AchAccountType.Checking);
            achAccountType.Items.Add(AchAccountType.Savings);
            achAccountType.SelectedIndex = 0;

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
                CreditCard creditCard = null;

                if (paymentMethod.SelectedItem.ToString() == CreditCardPaymentMethod)
                {
                    // Credit card has been selected for the payment method, so create
                    // the credit card object
                    creditCard = new CreditCard()
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

                return creditCard;
            }
        }

        /// <summary>
        /// Gets the bank account if the selected payment is ACH account. A null
        /// value is returned if the selected payment method is not ACH Account.
        /// </summary>
        public AchAccount BankAccount
        {
            get
            {
                AchAccount account = null;

                if (paymentMethod.SelectedItem.ToString() == AchAccountPaymentMethod)
                {
                    account = new AchAccount()
                    {
                        AchAccountType = (AchAccountType)achAccountType.SelectedItem,
                        AccountHolderName = accountHolderName.Text,
                        BankName = bankName.Text,
                        AccountNumber = accountNumber.Text,
                        RouteID = routingNumber.Text
                    };
                }

                return account;
            }
        }

        /// <summary>
        /// Removes the text about the $15.99 monthly fee for an account.
        /// </summary>
        public void RemoveMonthlyFeeText()
        {
            labelPaymentInfo.Text = labelPaymentInfo.Text.Replace(" and for the $15.99 Stamps.com monthly service fee", string.Empty);
        }

        /// <summary>
        /// Validates the payment data.
        /// </summary>
        /// <returns><code>true</code>if the validation was successful; otherwise <code>false</code>.</returns>
        public bool ValidatePaymentData()
        {
            bool isValid = true;
            StringBuilder validationMessages = new StringBuilder();

            if (paymentMethod.SelectedItem.ToString() == CreditCardPaymentMethod)
            {
                if (string.IsNullOrEmpty(cardholderName.Text))
                {
                    validationMessages.AppendLine("Cardholder name");
                }

                // We're only concerned with validating the credit card info
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
            }
            else
            {
                // Just need to validate the bank account info
                if (string.IsNullOrEmpty(bankName.Text))
                {
                    validationMessages.AppendLine("Bank name");
                }

                if (string.IsNullOrEmpty(accountNumber.Text))
                {
                    validationMessages.AppendLine("Account number");
                }

                if (string.IsNullOrEmpty(accountHolderName.Text))
                {
                    validationMessages.AppendLine("Account holder name");
                }

                if (string.IsNullOrEmpty(routingNumber.Text))
                {
                    validationMessages.AppendLine("Routing number");
                }
            }

            if (validationMessages.Length > 0)
            {
                string message = string.Format("Please correct the following field(s):{0}{1}", Environment.NewLine, validationMessages.ToString());
                MessageHelper.ShowInformation(this, message);
                
                isValid = false;
            }

            return isValid;
        }
        
        /// <summary>
        /// Called when [payment method changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnPaymentMethodChanged(object sender, EventArgs e)
        {
            if (paymentMethod.SelectedItem.ToString() == CreditCardPaymentMethod)
            {
                // Show the panel to collect credit card info
                panelAchAccount.Visible = false;
                panelCreditCardInfo.Visible = true;                
            }
            else
            {
                // Show the panel to collect bank account info
                panelCreditCardInfo.Visible = false;
                panelAchAccount.Visible = true;
            }

            panelAchAccount.Invalidate(true);
            panelCreditCardInfo.Invalidate(true);
            this.Invalidate(true);
        }

        
    }
}
