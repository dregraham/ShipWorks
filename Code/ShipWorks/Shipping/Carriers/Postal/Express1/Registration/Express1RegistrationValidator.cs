using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore.ComponentRegistration;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// Validates registration information prior to sending registration request to Express1.
    /// </summary>
    [Component(RegistrationType.Self)]
    public class Express1RegistrationValidator : IExpress1RegistrationValidator
    {
        private readonly IExpress1PaymentValidator paymentValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1RegistrationValidator"/> class.
        /// </summary>
        public Express1RegistrationValidator(IExpress1PaymentValidator paymentValidator)
        {
            this.paymentValidator = paymentValidator;
        }

        /// <summary>
        /// Validates Registration information prior to sending registration request to Express1.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of Express1ValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        public List<Express1ValidationError> Validate(Express1Registration registration)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            errors.AddRange(ValidatePersonalInfo(registration));
            errors.AddRange(ValidatePaymentInfo(registration));

            return errors;
        }

        /// <summary>
        /// Validates the personal information (name, address, contact info, etc.) of an Express1 registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of Express1ValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        public List<Express1ValidationError> ValidatePersonalInfo(Express1Registration registration)
        {
            List<Express1ValidationError> errors = new List<Express1ValidationError>();

            // Required fields contain data
            errors.AddRange(ValidateDataIsProvided(registration.Name, "Name is required"));
            errors.AddRange(ValidateDataIsProvided(registration.Company, "Company is required"));
            errors.AddRange(ValidateDataIsProvided(registration.Phone10Digits, "Phone number is required"));
            errors.AddRange(ValidateDataIsProvided(registration.Email, "An email address is required"));

            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.Street1, "A street address is required"));
            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.City, "City is required"));
            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.StateProvCode, "A state/province is required"));
            errors.AddRange(ValidateDataIsProvided(registration.MailingAddress.PostalCode, "A postal code is required"));

            return errors;
        }

        /// <summary>
        /// Validates the payment information of an Express1 registration.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A List of Express1ValidationError objects containing all of the items that
        /// failed to pass validation. An empty list indicates the registration passed validation.</returns>
        public List<Express1ValidationError> ValidatePaymentInfo(Express1Registration registration)
        {
            return paymentValidator.ValidatePaymentInfo(registration.Payment).ToList();
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