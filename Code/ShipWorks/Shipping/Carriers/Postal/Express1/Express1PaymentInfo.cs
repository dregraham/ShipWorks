using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Data structure that holds the info on how the user is going to pay for an Express1 account
    /// </summary>
    public class Express1PaymentInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1PaymentInfo(Express1PaymentType express1PaymentType)
        {
            PaymentType = express1PaymentType;
        }

        /// <summary>
        /// Currently selected payment type (Credit card, ACH, etc...)
        /// </summary>
        public Express1PaymentType PaymentType
        {
            get; 
            set;
        }

        /// <summary>
        /// The billing address of the credit card
        /// </summary>
        public PersonAdapter CardBillingAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card type
        /// </summary>
        public Express1CreditCardType CardType
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card account number
        /// </summary>
        public string CardAccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card CVN
        /// </summary>
        public int CardCvn
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card expiration date
        /// </summary>
        public DateTime CardExpirationDate
        {
            get;
            set;
        }

        /// <summary>
        /// The ACH account type 
        /// </summary>
        public Express1AchType AchAccountType
        {
            get;
            set;
        }

        /// <summary>
        /// The bank name
        /// </summary>
        public string AchBankName
        {
            get;
            set;
        }

        /// <summary>
        /// The ACH account number
        /// </summary>
        public string AchAccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The routing id for the postage ACH account
        /// </summary>
        public string AchRoutingId
        {
            get;
            set;
        }

        /// <summary>
        /// The ACH account holder's name
        /// </summary>
        public string AchAccountHolderName
        {
            get;
            set;
        }
    }
}
