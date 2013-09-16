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

                if (registration.FirstCodewordType == registration.SecondCodewordType)
                {
                    // Code word types cannot be the same
                    errors.Add(new Express1ValidationError("You must select two different code word questions."));
                }

                // Check the code word values
                errors.AddRange(ValidateCodewordValue(registration.FirstCodewordValue, 1));
                errors.AddRange(ValidateCodewordValue(registration.SecondCodewordValue, 2));

                if (registration.Email.Length > 41)
                {
                    // Email address too long
                    errors.Add(new Express1ValidationError("Email address is too long. Stamps.com only allows email addresses that are less than 41 characters long."));
                }

                if (registration.PromoCode.Length > 50)
                {
                    // Promo code too long
                    errors.Add(new Express1ValidationError("An invalid promo code was provided. Stamps.com only recognizes promo codes that are 50 characters or less."));
                }

                // At least one payment method must be present
                if (registration.CreditCard == null && registration.AchAccount == null)
                {
                    errors.Add(new Express1ValidationError("Stamps.com requires that either credit card or account be provided in the registration process."));
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
            errors.AddRange(ValidateDataIsProvided(registration.FirstCodewordValue, "First code word is required."));
            errors.AddRange(ValidateDataIsProvided(registration.SecondCodewordValue, "Second code word is required."));
            errors.AddRange(ValidateDataIsProvided(registration.Email, "Email address is required."));

            // Address (physical)
            errors.AddRange(ValidatePhysicalAddress(registration.PhysicalAddress));

            // Machine Info
            errors.AddRange(ValidateMachineInfo(registration.MachineInfo));

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

        /// <summary>
        /// Validates the codeword value.
        /// </summary>
        /// <param name="codewordValue">The codeword value.</param>
        /// <param name="codewordNumber">The codeword number.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        private List<Express1ValidationError> ValidateCodewordValue(string codewordValue, int codewordNumber)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            // Code words too short/long
            if (codewordValue.Length < 2 || codewordValue.Length > 33)
            {
                string codewordAdjective = codewordNumber == 1 ? "first" : "second";
                string validationMessage = string.Format("The {0} code word must be between 2 and 33 characters long.", codewordAdjective);

                errors.Add(new Express1ValidationError(validationMessage));
            }

            return errors;
        }

        /// <summary>
        /// Validates the machine info.
        /// </summary>
        /// <param name="machineInfo">The machine info.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        private List<Express1ValidationError> ValidateMachineInfo(MachineInfo machineInfo)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            if (machineInfo == null || string.IsNullOrEmpty(machineInfo.IPAddress))
            {
                errors.Add(new Express1ValidationError("Machine info is required."));
            }

            return errors;
        }

        /// <summary>
        /// Validates the physical address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>A List of Express1ValidationError objects.</returns>
        private List<Express1ValidationError> ValidatePhysicalAddress(Address address)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            if (address != null)
            {
                errors.AddRange(ValidateDataIsProvided(address.Address1, "A street address is required for the physical address."));
                errors.AddRange(ValidateDataIsProvided(address.City, "A city is required for the physical address."));

                if (!string.IsNullOrEmpty(address.Country) && PostalUtility.IsDomesticCountry(address.Country))
                {
                    errors.AddRange(ValidateDataIsProvided(address.State, "A state is required for the physical address."));
                    errors.AddRange(ValidateDataIsProvided(address.ZIPCode, "A postal code is required for the physical address."));
                }
                else
                {
                    errors.AddRange(ValidateDataIsProvided(address.Province, "A province is required for the physical address."));
                    errors.AddRange(ValidateDataIsProvided(address.PostalCode, "A postal code is required for the physical address."));
                }
            }
            else
            {
                errors.Add(new Express1ValidationError("A physical address must be provided."));
            }

            return errors;
        }
    }
}