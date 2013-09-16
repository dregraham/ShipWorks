using System.Collections.Generic;
using System.Text.RegularExpressions;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Validates Registration information prior to sending registration request to Express1.
    /// </summary>
    public class Express1RegistrationValidator : IExpress1RegistrationValidator
    {
        // Password must contain at least one letter, one number, and between 6 and 20 characters long
        private const string PasswordRegexPattern = "^(?=.*\\d)(?=.*[a-zA-Z]).{6,20}$";

        /// <summary>
        /// Validates the specified registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of Express1ValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        public List<Express1ValidationError> Validate(Express1Registration registration)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            // required fields contain data
            errors.AddRange(ValidateRequiredFields(registration));

            // Only perform the following if we have all of the required fields populated
            if (errors.Count == 0)
            {
                if (registration.UserName.Length > 40)
                {
                    // Username too long
                    errors.Add(new Express1ValidationError("The username provided is too long. Usernames must be 40 characters or less."));
                }

                // Password checks - length (too short/long), strength requirements
                if (!Regex.IsMatch(registration.Password, PasswordRegexPattern))
                {
                    errors.Add(new Express1ValidationError("Passwords must be between 6 and 20 characters long and contain at least one letter and one number."));
                }

                if (registration.Email.Length > 41)
                {
                    // Email address too long
                    errors.Add(new Express1ValidationError("Email address is too long. Stamps.com only allows email addresses that are less than 41 characters long."));
                }

            }

            return errors;
        }

        /// <summary>
        /// A helper method that makes sure all the required fields have data provided.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        private List<Express1ValidationError> ValidateRequiredFields(Express1Registration registration)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();
            errors.AddRange(ValidateDataIsProvided(registration.UserName, "Username is required."));
            errors.AddRange(ValidateDataIsProvided(registration.Password, "Password is required."));
            errors.AddRange(ValidateDataIsProvided(registration.Email, "Email address is required."));

            return errors;
        }

        /// <summary>
        /// Simple validation that just checks that the string is not empty.
        /// </summary>
        /// <param name="registrationField">The registration field.</param>
        /// <param name="validationMessage">The validation message.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        private List<Express1ValidationError> ValidateDataIsProvided(string registrationField, string validationMessage)
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