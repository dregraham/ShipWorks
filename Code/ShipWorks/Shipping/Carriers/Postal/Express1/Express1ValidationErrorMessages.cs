using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Error messages for payment validation.
    /// </summary>
    public static class Express1ValidationErrorMessages
    {
        public const string InvalidPaymentTypeCreditCard = "ShipWorks is unable to validate ACH payment info because the payment type is currently Credit Card.";
        public const string InvalidPaymentTypeAch = "ShipWorks is unable to validate ACH payment info because the payment type is currently ACH.";

        public const string InvalidAchAccountNumber = "The ACH account number is required.";
        public const string InvalidAchAccountHolderName = "The ACH account holder name is required.";
        public const string InvalidAchBankName = "The ACH bank name is required.";
        public const string InvalidAchRoutingId = "The ACH routing number is required.";
        public const string InvalidAchAccountName = "The ACH account name is required.";

        public const string InvalidCreditCardExpirationDate = "The credit card expiration date must be in the future.";
        public const string InvalidCreditCardCvn = "A valid credit card CVN is required.";
        public const string InvalidCreditCardAccountNumber = "The credit card number is required.";
        public const string InvalidCreditCardNameOnCard = "The name on credit card is required.";

        public const string InvalidCreditCardBillingAddressFirstName = "A person's first name is required for the address.";
        public const string InvalidCreditCardBillingAddressLastName = "A person's last name is required for the address.";
        public const string InvalidCreditCardBillingAddressStreet = "A street address is required for the address.";
        public const string InvalidCreditCardBillingAddressCity = "A city is required for the address.";
        public const string InvalidCreditCardBillingAddressStateProvince = "A state is required for the address.";
        public const string InvalidCreditCardBillingAddressPostalCode = "A postal code is required for the address.";
        public const string InvalidCreditCardBillingAddressCountryCode = "A country is required for the address.";
        public const string MissingCreditCardBillingAddress = "An address must be provided.";
    }
}
