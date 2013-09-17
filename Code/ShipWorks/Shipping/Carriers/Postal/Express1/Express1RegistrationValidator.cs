using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Validates registration information prior to sending registration request to Express1.
    /// </summary>
    public class Express1RegistrationValidator : IExpress1RegistrationValidator
    {
        /// <summary>
        /// Validates Registration information prior to sending registration request to Express1.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of Express1ValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        public List<Express1ValidationError> Validate(Express1Registration registration)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            // Required fields contain data
            errors.AddRange(ValidateDataIsProvided(registration.Name, "Name is required"));
            errors.AddRange(ValidateDataIsProvided(registration.Phone10Digits, "Phone number is required"));
            errors.AddRange(ValidateDataIsProvided(registration.Email, "An email address is required"));
            
            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.Street1, "A street address is required"));
            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.City, "City is required"));
            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.StateProvCode, "A state/province is required"));
            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.PostalCode, "A postal code is required"));

            return errors;
        }
        
        /// <summary>
        /// Simple validation that just checks that the string is not empty.
        /// </summary>
        /// <param name="registrationField">The registration field.</param>
        /// <param name="validationMessage">The validation message.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        private IEnumerable<Express1ValidationError> ValidateDataIsProvided(string registrationField, string validationMessage)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            if (string.IsNullOrEmpty(registrationField))
            {
                errors.Add(new Express1ValidationError(validationMessage));
            }

            return errors;
        }
    }
}