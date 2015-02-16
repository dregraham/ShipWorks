using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    public class UspsRegistration
    {
        private readonly IUspsRegistrationValidator validator;
        private readonly IUspsRegistrationGateway registrationGateway;
        private readonly IRegistrationPromotion promotion;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistration"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="gateway">The gateway.</param>
        public UspsRegistration(IUspsRegistrationValidator validator, IUspsRegistrationGateway gateway)
            : this(validator, gateway, new RegistrationPromotionFactory().CreateRegistrationPromotion())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistration" /> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="gateway">The gateway.</param>
        /// <param name="promotion">The promotion.</param>
        /// <exception cref="UspsRegistrationException">Stamps.com requires an IP address for registration, but ShipWorks could not obtain the IP address of this machine.</exception>
        public UspsRegistration(IUspsRegistrationValidator validator, IUspsRegistrationGateway gateway, IRegistrationPromotion promotion)
        {
            this.validator = validator;
            registrationGateway = gateway;
            this.promotion = promotion;

            UserName = string.Empty;
            Password = string.Empty;
            Email = string.Empty;

            UsageType = new AccountType();
            
            FirstCodewordType = new CodewordType2();
            FirstCodewordValue = string.Empty;

            SecondCodewordType = new CodewordType2();
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
                throw new UspsRegistrationException("Stamps.com requires an IP address for registration, but ShipWorks could not obtain the IP address of this machine.", ex);
            }
            
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
        /// Gets or sets the intended usage of the the account. 
        /// </summary>
        /// <value>The intended usage of the account.</value>
        public AccountType UsageType { get; set; }

        /// <summary>
        /// Gets or sets the first type of the codeword.
        /// </summary>
        /// <value>The first type of the codeword.</value>
        public CodewordType2 FirstCodewordType { get; set; }

        /// <summary>
        /// Gets or sets the first code word value.
        /// </summary>
        /// <value>The first code word value.</value>
        public string FirstCodewordValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the second codeword.
        /// </summary>
        /// <value>The type of the second codeword.</value>
        public CodewordType2 SecondCodewordType { get; set; }

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
        /// Gets or sets the type of the registration.
        /// </summary>
        /// <value>The type of the registration.</value>
        public PostalAccountRegistrationType RegistrationType { get; set; }

        /// <summary>
        /// Gets or sets the promo code based on the valud of the RegistrationType property.
        /// </summary>
        /// <value>The promo code.</value>
        public string PromoCode
        {
            get { return promotion.GetPromoCode(); }
        }

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
        /// Submits the registration to Stamps.com.
        /// </summary>
        /// <returns>Any registration validation errors that may have occurred.</returns>
        public UspsRegistrationResult Submit()
        {
            // Validate the registration info
            IEnumerable<RegistrationValidationError> errors = validator.Validate(this);

            if (errors.Count() > 0)
            {
                UspsRegistrationException registrationException = CreateRegistrationException(errors);
                throw registrationException;
            }

            // Send the registration data to the stamps.com web service API
            return registrationGateway.Register(this);
        }

        /// <summary>
        /// Creates a registration exception.
        /// </summary>
        /// <param name="validationErrors">The validation errors.</param>
        /// <returns>A StampsRegistrationException object.</returns>
        private UspsRegistrationException CreateRegistrationException(IEnumerable<RegistrationValidationError> validationErrors)
        {
            StringBuilder message = new StringBuilder("There were problems registering your account with Stamps.com. Please correct the following field(s) in your stamps.com registration:" + Environment.NewLine);

            foreach (RegistrationValidationError error in validationErrors)
            {
                message.AppendLine(error.Message);
            }

            return new UspsRegistrationException(message.ToString());
        }
    }
}
