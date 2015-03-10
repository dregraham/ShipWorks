using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Express1Registration"/> class.
    /// </summary>
    public class Express1Registration
    {
        private readonly IExpress1RegistrationValidator registrationValidator;
        private readonly IExpress1RegistrationGateway registrationGateway;
        private readonly IExpress1RegistrationRepository registrationRepository;
        private readonly IExpress1PasswordEncryptionStrategy passwordEncryptionStrategy;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1Registration" /> class.
        /// </summary>
        /// <param name="shipmentType">Type of shipment this account will be used for.</param>
        /// <param name="gateway">The gateway used to interface with the Express1 API</param>
        /// <param name="repository">Repository used to interface with the underlying account entities</param>
        /// <param name="encryptionStrategy">An abstraction for the strategy used for encrypting passwords since 
        /// this can vary depending on the underlying Express1 carrier.</param>
        /// <param name="validator">Validator that will be used to ensure data is correct</param>
        public Express1Registration(ShipmentTypeCode shipmentType, IExpress1RegistrationGateway gateway, IExpress1RegistrationRepository repository, IExpress1PasswordEncryptionStrategy encryptionStrategy, IExpress1RegistrationValidator validator)
        {
            ShipmentTypeCode = shipmentType;

            registrationValidator = validator;
            registrationGateway = gateway;
            registrationRepository = repository;
            passwordEncryptionStrategy = encryptionStrategy;

            UserName = string.Empty;
            PlainTextPassword = string.Empty;
        }

        /// <summary>
        /// Gets and sets the id of the underlying account
        /// </summary>
        public long? AccountId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password. 
        /// </summary>
        /// <value>The password.</value>
        public string PlainTextPassword { get; set; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get
            {
                return (MailingAddress == null) ? string.Empty : MailingAddress.Email;
            }
        }

        /// <summary>
        /// Gets the name of the person registering
        /// </summary>
        public string Name
        {
            get
            {
                return (MailingAddress == null) ?
                    string.Empty :
                    new PersonName(MailingAddress.FirstName, MailingAddress.MiddleName, MailingAddress.LastName).FullName;
            }
        }

        /// <summary>
        /// Gets or sets the mailing address.
        /// </summary>
        /// <value>The mailing address.</value>
        public PersonAdapter MailingAddress { get; set; }

        /// <summary>
        /// Gets the type of shipment associated with this registration
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Gets the phone number for the registration
        /// </summary>
        public string Phone10Digits
        {
            get
            {
                return (MailingAddress == null) ? string.Empty : MailingAddress.Phone;
            }
        }

        /// <summary>
        /// Gets the company for the registration
        /// </summary>
        public string Company
        {
            get
            {
                return (MailingAddress == null) ? string.Empty : MailingAddress.Company;
            }
        }

        /// <summary>
        /// Gets and sets the payment info for the registration
        /// </summary>
        public Express1PaymentInfo Payment { get; set; }

        /// <summary>
        /// Gets a list of validation errors generated while saving or adding accounts
        /// </summary>
        public IList<Express1ValidationError> ValidationErrors { get; private set; }

        /// <summary>
        /// Gets the encrypted password.
        /// </summary>
        public string EncryptedPassword
        {
            get
            {
                // Defer to the password encryption strategy since this can vary by
                // the underlying Express1 carrier (Endicia or Usps)
                return passwordEncryptionStrategy.EncryptPassword(this);
            }
        }

        /// <summary>
        /// Add an existing Express1 account to ShipWorks
        /// </summary>
        public void AddExistingAccount()
        {
            // Check for any validation errors and ensure that the credentials we 
            // have are for a valid Express1/Usps account
            ValidationErrors = registrationValidator.ValidatePersonalInfo(this);

            if (ValidationErrors.Any())
            {
                // Either the personal info was incomplete or the credentials could not be confirmed 
                string message = "The following issues were found: " +
                                 Environment.NewLine + ValidationErrors.Select(m => "    " + m.Message).Combine(Environment.NewLine);

                throw new Express1RegistrationException(message);
            }

            registrationGateway.VerifyAccount(this);
            SaveAccount();
        }

        /// <summary>
        /// Create a new Express1 account
        /// </summary>
        /// <exception cref="Express1RegistrationException"></exception>
        public void CreateNewAccount()
        {
            ValidationErrors = registrationValidator.Validate(this);
            
            if (ValidationErrors.Any())
            {
                string message = "The following issues were found: " +
                                 Environment.NewLine + ValidationErrors.Select(m => "    " + m.Message).Combine(Environment.NewLine);

                throw new Express1RegistrationException(message);
            }

            try
            {
                // Use the gateway to make the API call to Express1
                Express1RegistrationResult registrationResult = registrationGateway.Register(this);

                // Note the account number and password from the registration result
                UserName = registrationResult.AccountNumber;
                PlainTextPassword = registrationResult.Password;

                // Use the the repository to save the registration in ShipWorks
                SaveAccount();
            }
            catch (Exception ex)
            {
                string message = string.Format("ShipWorks encountered an error while trying to create a new Express1 account. {0}", ex.Message);
                throw new Express1RegistrationException(message, ex);
            }
        }

        /// <summary>
        /// Save changes to an account.
        /// </summary>
        public void SaveAccount()
        {
            AccountId = registrationRepository.Save(this);
        }

        /// <summary>
        /// Deletes the account entity from ShipWorks that may be associated with this registration.
        /// </summary>
        public void DeleteAccount()
        {
            if (AccountId.HasValue)
            {
                // We have a value for the account ID, meaning the account has been created at some point.
                registrationRepository.Delete(this);
            }
        }

        /// <summary>
        /// Validates the personal information about the registration
        /// </summary>
        /// <returns>A list of validation errors</returns>
        public IEnumerable<Express1ValidationError> ValidatePersonalInfo()
        {
            return registrationValidator.ValidatePersonalInfo(this);
        }

        /// <summary>
        /// Validates the payment information about the registration
        /// </summary>
        /// <returns>A list of validation errors</returns>
        public IEnumerable<Express1ValidationError> ValidatePaymentInfo()
        {
            return registrationValidator.ValidatePaymentInfo(this);
        }
    }
}