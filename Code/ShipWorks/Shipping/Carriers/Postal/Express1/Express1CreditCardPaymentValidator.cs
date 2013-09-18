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
    public class Express1CreditCardPaymentValidator : IExpress1PaymentValidator
    {
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
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidPaymentTypeAch));
                return validationErrors;
            }

            // Allow a credit card that expires at the end of the current month by comparing the expiration date 
            // with the first day of the current month (i.e. a credit card that expires this month should be
            // considered valid)
            DateTime minimumAllowedExpirationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (paymentInfo.CreditCardExpirationDate < minimumAllowedExpirationDate)
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidCreditCardExpirationDate));
            }

            if (string.IsNullOrWhiteSpace(paymentInfo.CreditCardAccountNumber))
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidCreditCardAccountNumber));
            }

            if (string.IsNullOrWhiteSpace(paymentInfo.CreditCardNameOnCard))
            {
                validationErrors.Add(new Express1ValidationError(Express1ValidationErrorMessages.InvalidCreditCardNameOnCard));
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
