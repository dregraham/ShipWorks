using System;
using System.Collections.Generic;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment
{
    /// <summary>
    /// Validates that the credit card info on a Express1PaymentInfo is valid
    /// </summary>
    [Component]
    public class Express1CreditCardPaymentValidator : IExpress1PaymentValidator
    {
        public const string InvalidPaymentTypeAch = "ShipWorks is unable to validate credit card payment info because the payment type is currently ACH.";
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

        /// <summary>
        /// Validates payment information.
        /// </summary>
        /// <returns>A collection of Express1ValidationError objects. An empty collection indicates
        /// the payment information is valid; a non-empty collection indicates that the payment info
        /// did not pass validation.</returns>
        public IEnumerable<Express1ValidationError> ValidatePaymentInfo(Express1PaymentInfo paymentInfo)
        {

            List<Express1ValidationError> validationErrors = new List<Express1ValidationError>();

            if (paymentInfo.PaymentType == Express1PaymentType.Ach)
            {
                validationErrors.Add(new Express1ValidationError(InvalidPaymentTypeAch));
                return validationErrors;
            }

            // Allow a credit card that expires at the end of the current month by comparing the expiration date
            // with the first day of the current month (i.e. a credit card that expires this month should be
            // considered valid)
            DateTime minimumAllowedExpirationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (paymentInfo.CreditCardExpirationDate < minimumAllowedExpirationDate)
            {
                validationErrors.Add(new Express1ValidationError(InvalidCreditCardExpirationDate));
            }

            if (string.IsNullOrWhiteSpace(paymentInfo.CreditCardAccountNumber))
            {
                validationErrors.Add(new Express1ValidationError(InvalidCreditCardAccountNumber));
            }

            if (string.IsNullOrWhiteSpace(paymentInfo.CreditCardNameOnCard))
            {
                validationErrors.Add(new Express1ValidationError(InvalidCreditCardNameOnCard));
            }

            validationErrors.AddRange(ValidatePhysicalAddress(paymentInfo.CreditCardBillingAddress));

            return validationErrors;
        }

        /// <summary>
        /// Validates the address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>An IEnumerable of RegistrationValidationError objects.</returns>
        private static IEnumerable<Express1ValidationError> ValidatePhysicalAddress(PersonAdapter address)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            if (address != null)
            {
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.FirstName, InvalidCreditCardBillingAddressFirstName));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.LastName, InvalidCreditCardBillingAddressLastName));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.StreetAll, InvalidCreditCardBillingAddressStreet));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.City, InvalidCreditCardBillingAddressCity));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.StateProvCode, InvalidCreditCardBillingAddressStateProvince));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.PostalCode, InvalidCreditCardBillingAddressPostalCode));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.CountryCode, InvalidCreditCardBillingAddressCountryCode));
            }
            else
            {
                errors.Add(new Express1ValidationError(MissingCreditCardBillingAddress));
            }

            return errors;
        }
    }
}
