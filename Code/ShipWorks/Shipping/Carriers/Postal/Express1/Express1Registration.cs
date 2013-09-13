using System;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Express1Registration"/> class.
    /// </summary>
    public abstract class Express1Registration
    {
        //private IStampsRegistrationValidator validator;
        protected readonly IExpress1RegistrationGateway registrationGateway;

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1Registration"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="gateway">The gateway.</param>
        public Express1Registration(IExpress1RegistrationGateway gateway)
        {
            //this.validator = validator;
            registrationGateway = gateway;
            
            UserName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;

            AccountType = new AccountType();
            
            FirstCodewordType = new CodewordType();
            FirstCodewordValue = string.Empty;

            SecondCodewordType = new CodewordType();
            SecondCodewordValue = string.Empty;

            PhysicalAddress = new Address();
            MailingAddress = new Address();

            // Grab the version 4 IP address of the client machine, so the consumer doesn't 
            // have to explicitly set the IP address if it doesn't have reason to
            MachineInfo = new MachineInfo();

            try
            {
                NetworkUtility networkUtility = new NetworkUtility();
                MachineInfo.IPAddress = networkUtility.GetIPAddress();
            }
            catch (NetworkException ex)
            {
                throw new StampsRegistrationException("Stamps.com requires an IP address for registration, but ShipWorks could not obtain the IP address of this machine.", ex);
            }

            PromoCode = string.Empty;

            CreditCard = new CreditCard();
            AchAccount = new AchAccount();
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <value>The type of the account.</value>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Gets or sets the first type of the codeword.
        /// </summary>
        /// <value>The first type of the codeword.</value>
        public CodewordType FirstCodewordType { get; set; }

        /// <summary>
        /// Gets or sets the first code word value.
        /// </summary>
        /// <value>The first code word value.</value>
        public string FirstCodewordValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the second codeword.
        /// </summary>
        /// <value>The type of the second codeword.</value>
        public CodewordType SecondCodewordType { get; set; }

        /// <summary>
        /// Gets or sets the second codeword value.
        /// </summary>
        /// <value>The second codeword value.</value>
        public string SecondCodewordValue { get; set; }

        /// <summary>
        /// Gets or sets the physical address.
        /// </summary>
        /// <value>The physical address.</value>
        public Address PhysicalAddress { get; set; }

        /// <summary>
        /// Gets or sets the mailing address.
        /// </summary>
        /// <value>The mailing address.</value>
        public Address MailingAddress { get; set; }

        /// <summary>
        /// Gets or sets the machine info.
        /// </summary>
        /// <value>The machine info.</value>
        public MachineInfo MachineInfo { get; set; }

        /// <summary>
        /// Gets or sets the promo code.
        /// </summary>
        /// <value>The promo code.</value>
        public string PromoCode { get; set; }

        /// <summary>
        /// Gets or sets the credit card.
        /// </summary>
        /// <value>The credit card.</value>
        public CreditCard CreditCard { get; set; }

        /// <summary>
        /// Gets or sets the ach account.
        /// </summary>
        /// <value>The ach account.</value>
        public AchAccount AchAccount { get; set; }

        /// <summary>
        /// Gets and sets the Express1
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets the type of shipment associated with this registration
        /// </summary>
        public abstract ShipmentTypeCode ShipmentTypeCode { get; }

        ///// <summary>
        ///// Submits the registration to Stamps.com.
        ///// </summary>
        ///// <returns>Any registration validation errors that may have occurred.</returns>
        //public Express1RegistrationResult Submit(CustomerRegistrationData customData)
        //{
        //    //// Validate the registration info
        //    //IEnumerable<RegistrationValidationError> errors = validator.Validate(this);

        //    //if (errors.Count() > 0)
        //    //{
        //    //    StampsRegistrationException registrationException = CreateRegistrationException(errors);
        //    //    throw registrationException;
        //    //}

        //    // Send the registration data to the stamps.com web service API
        //    return registrationGateway.Register(customData);
        //}

        ///// <summary>
        ///// Creates a registration exception.
        ///// </summary>
        ///// <param name="validationErrors">The validation errors.</param>
        ///// <returns>A StampsRegistrationException object.</returns>
        //private StampsRegistrationException CreateRegistrationException(IEnumerable<RegistrationValidationError> validationErrors)
        //{
        //    StringBuilder message = new StringBuilder("There were problems registering your account with Stamps.com. Please correct the following field(s) in your stamps.com registration:" + Environment.NewLine);

        //    foreach (RegistrationValidationError error in validationErrors)
        //    {
        //        message.AppendLine(error.Message);
        //    }

        //    return new StampsRegistrationException(message.ToString());
        //}

        /// <summary>
        /// Populates an AddressInfo with data from an Endicia Account
        /// </summary>
        protected static AddressInfo CreateAddressInfo(PersonAdapter fromAddress)
        {
            return new AddressInfo
                {
                    Address1 = fromAddress.Street1,
                    Address2 = fromAddress.Street2,
                    City = fromAddress.City,
                    PostalCode = fromAddress.PostalCode,
                    State = fromAddress.StateProvCode
                };
        }

        public abstract void SaveAccount();

        public abstract void Signup(PersonAdapter accountAddress, Express1PaymentInfo paymentInfo);
    }
}