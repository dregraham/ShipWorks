using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Account
{
    /// <summary>
    /// Data structure that holds the info on how the user is going to pay for an endicia account
    /// </summary>
    public class EndiciaPaymentInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EndiciaPaymentInfo()
        {
            UseCheckingForPostage = false;
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
        public EndiciaCreditCardType CardType
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card number
        /// </summary>
        public string CardNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card expiration month
        /// </summary>
        public int CardExpirationMonth
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card expiration year
        /// </summary>
        public int CardExpirationYear
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card CVV
        /// </summary>
        public string CVV { get; set; }

        /// <summary>
        /// Indicates if postage should come out of a checking account
        /// </summary>
        public bool UseCheckingForPostage
        {
            get;
            set;
        }

        /// <summary>
        /// The checking account that postage should come out of
        /// </summary>
        public string CheckingAccount
        {
            get;
            set;
        }

        /// <summary>
        /// The routing number for the postage checking account
        /// </summary>
        public string CheckingRouting
        {
            get;
            set;
        }
    }
}
