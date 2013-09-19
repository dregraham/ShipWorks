using System;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment
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
        public PersonAdapter CreditCardBillingAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card type
        /// </summary>
        public Express1CreditCardType CreditCardType
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card account number
        /// </summary>
        public string CreditCardAccountNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card CVN
        /// </summary>
        public int CreditCardVerificationNumber
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card expiration date
        /// </summary>
        public DateTime CreditCardExpirationDate
        {
            get;
            set;
        }

        /// <summary>
        /// Credit card account number
        /// </summary>
        public string CreditCardNameOnCard
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
        /// The ACH account name
        /// </summary>
        public string AchAccountName
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

        /// <summary>
        /// Gets the Express1 API Card type from the Express1 Card Type
        /// </summary>
        public CreditCardTypeEnum ApiCardType
        {
            get
            {
                switch (CreditCardType)
                {
                    case Express1CreditCardType.AmericanExpress:
                        return CreditCardTypeEnum.AmericanExpress;
                    case Express1CreditCardType.Discover:
                        return CreditCardTypeEnum.Discover;
                    case Express1CreditCardType.MasterCard:
                        return CreditCardTypeEnum.MasterCard;
                    case Express1CreditCardType.Visa:
                        return CreditCardTypeEnum.Visa;

                    default:
                        throw new InvalidOperationException("Unsupported Credit Card type.");
                }
            }
        }
    }
}
