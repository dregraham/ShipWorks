using System.Collections.Generic;
using System.Text.RegularExpressions;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// Validates the fields within a stamps.com registration according to the Stamps.com documentation.
    /// </summary>
    public class UspsRegistrationValidator : IUspsRegistrationValidator
    {
        // Password must contain at least one letter, one number, and between 6 and 20 characters long
        private const string PasswordRegexPattern = "^(?=.*\\d)(?=.*[a-zA-Z]).{6,20}$";

        /// <summary>
        /// Validates the specified registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of RegistrationValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        public IEnumerable<RegistrationValidationError> Validate(StampsRegistration registration)
        {
            List<RegistrationValidationError> errors = new List<RegistrationValidationError>();
            
            // required fields contain data
            errors.AddRange(ValidateRequiredFields(registration));

            // Only perform the following if we have all of the required fields populated
            if (errors.Count == 0)
            {
                if (registration.UserName.Length > 14)
                {
                    // Username too long
                    errors.Add(new RegistrationValidationError("The username provided is too long. Usernames must be 14 characters or less."));
                }

                // Password checks - length (too short/long), strength requirements
                if (!Regex.IsMatch(registration.Password, PasswordRegexPattern))
                {
                    errors.Add(new RegistrationValidationError("Passwords must be between 6 and 20 characters long and contain at least one letter and one number."));
                }

                if (registration.FirstCodewordType == registration.SecondCodewordType)
                {
                    // Code word types cannot be the same
                    errors.Add(new RegistrationValidationError("You must select two different code word questions."));
                }
                
                // Check the code word values
                errors.AddRange(ValidateCodewordValue(registration.FirstCodewordValue, 1));
                errors.AddRange(ValidateCodewordValue(registration.SecondCodewordValue, 2));

                if (registration.Email.Length > 41)
                {
                    // Email address too long
                    errors.Add(new RegistrationValidationError("Email address is too long. Stamps.com only allows email addresses that are less than 41 characters long."));
                }

                if (registration.PromoCode.Length > 50)
                {
                    // Promo code too long
                    errors.Add(new RegistrationValidationError("An invalid promo code was provided. Stamps.com only recognizes promo codes that are 50 characters or less."));
                }

                // At least one payment method must be present
                if (registration.CreditCard == null && registration.AchAccount == null)
                {
                    errors.Add(new RegistrationValidationError("Stamps.com requires that either credit card or account be provided in the registration process."));
                }

                if (registration.PhysicalAddress.PhoneNumber.Length > 10)
                {
                    // Stamps.com API has max length of 10 for phone numbers, so attempt to 
                    // clean up the phone number that will get sent to Stamps.com. If the phone number
                    // is still too long, let the request go through and allow Stamps to handle the error
                    string cleansedPhoneNumber = registration.PhysicalAddress.PhoneNumber.Replace("-", string.Empty);
                    if (cleansedPhoneNumber.Length > 10)
                    {
                        // Phone number is still too long, so allow the user to correct it
                        errors.Add(new RegistrationValidationError("Stamps.com requires that the phone number cannot exceed 10 characters."));
                    }
                    else
                    {
                        // The cleansed phone number is a valid length, so we'll just use that to pass on to Stamps.com
                        registration.PhysicalAddress.PhoneNumber = cleansedPhoneNumber;
                    }
                }
            }

            return errors;
        }
                
        /// <summary>
        /// A helper method that makes sure all the required fields have data provided.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of RegistrationValidationError objects.</returns>
        private List<RegistrationValidationError> ValidateRequiredFields(StampsRegistration registration)
        {
            List<RegistrationValidationError> errors = new List<RegistrationValidationError>();
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
        /// <returns>A List of RegistrationValidationError objects.</returns>
        private List<RegistrationValidationError> ValidateDataIsProvided(string registrationField, string validationMessage)
        {
            List<RegistrationValidationError> errors = new List<RegistrationValidationError>();

            if (string.IsNullOrEmpty(registrationField))
            {
                errors.Add(new RegistrationValidationError(validationMessage));
            }

            return errors;
        }

        /// <summary>
        /// Validates the codeword value.
        /// </summary>
        /// <param name="codewordValue">The codeword value.</param>
        /// <param name="codewordNumber">The codeword number.</param>
        /// <returns>A List of RegistrationValidationError objects.</returns>
        private List<RegistrationValidationError> ValidateCodewordValue(string codewordValue, int codewordNumber)
        {
            List<RegistrationValidationError> errors = new List<RegistrationValidationError>();

            // Code words too short/long
            if (codewordValue.Length < 2 || codewordValue.Length > 33)
            {
                string codewordAdjective = codewordNumber == 1 ? "first" : "second";
                string validationMessage = string.Format("The {0} code word must be between 2 and 33 characters long.", codewordAdjective);

                errors.Add(new RegistrationValidationError(validationMessage));
            }

            return errors;
        }

        /// <summary>
        /// Validates the machine info.
        /// </summary>
        /// <param name="machineInfo">The machine info.</param>
        /// <returns>A List of RegistrationValidationError objects.</returns>
        private List<RegistrationValidationError> ValidateMachineInfo(MachineInfo machineInfo)
        {
            List<RegistrationValidationError> errors = new List<RegistrationValidationError>();

            if (machineInfo == null || string.IsNullOrEmpty(machineInfo.IPAddress))
            {
                errors.Add(new RegistrationValidationError("Machine info is required."));
            }

            return errors;
        }

        /// <summary>
        /// Validates the physical address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>A List of RegistrationValidationError objects.</returns>
        private List<RegistrationValidationError> ValidatePhysicalAddress(Address address)
        {
            List<RegistrationValidationError> errors = new List<RegistrationValidationError>();

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
                errors.Add(new RegistrationValidationError("A physical address must be provided."));
            }

            return errors;
        }
    }
}
