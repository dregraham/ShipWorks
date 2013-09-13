﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Validates that the credit card info on a Express1PaymentInfo is valid
    /// </summary>
    public class Express1CreditCardPaymentValidator : IPaymentValidator
    {
        private readonly Express1PaymentInfo express1PaymentInfo;
        private readonly List<Express1ValidationError> validationErrors;

        /// <summary>
        /// Consturctor that accepts an Express1PaymentInfo
        /// </summary>
        /// <param name="express1PaymentInfo">The Express1PaymentInfo to validate.</param>
        public Express1CreditCardPaymentValidator(Express1PaymentInfo express1PaymentInfo)
        {
            this.express1PaymentInfo = express1PaymentInfo;
            validationErrors = new List<Express1ValidationError>();
        }

        /// <summary>
        /// Validates payment information.
        /// </summary>
        /// <returns>A collection of Express1ValidationError objects. An empty collection indicates
        /// the payment information is valid; a non-empty collection indicates that the payment info
        /// did not pass validation.</returns>
        public IEnumerable<Express1ValidationError> ValidatePaymentInfo()
        {
            if (express1PaymentInfo.PaymentType == Express1PaymentType.Ach)
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidPaymentTypeAch));
                return validationErrors;
            }

            if (express1PaymentInfo.CardExpirationDate <= DateTime.Now)
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidCreditCardExpirationDate));
            }

            if (express1PaymentInfo.CardCvn <= 0)
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidCreditCardCvn));
            }

            if (string.IsNullOrWhiteSpace(express1PaymentInfo.CardAccountNumber))
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidCreditCardAccountNumber));
            }

            validationErrors.AddRange(ValidatePhysicalAddress(express1PaymentInfo.CardBillingAddress));

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
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.FirstName, Express1ValidationErrorMessages.InvalidCreditCardBillingAddressFirstName));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.LastName, Express1ValidationErrorMessages.InvalidCreditCardBillingAddressLastName));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.StreetAll, Express1ValidationErrorMessages.InvalidCreditCardBillingAddressStreet));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.City, Express1ValidationErrorMessages.InvalidCreditCardBillingAddressCity));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.StateProvCode, Express1ValidationErrorMessages.InvalidCreditCardBillingAddressStateProvince));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.PostalCode, Express1ValidationErrorMessages.InvalidCreditCardBillingAddressPostalCode));
                errors.AddRange(Express1Utilities.ValidateDataIsProvided(address.CountryCode, Express1ValidationErrorMessages.InvalidCreditCardBillingAddressCountryCode));
            }
            else
            {
                errors.Add(new Express1ValidationError(Express1ValidationErrorMessages.MissingCreditCardBillingAddress));
            }

            return errors;
        }
    }
}
