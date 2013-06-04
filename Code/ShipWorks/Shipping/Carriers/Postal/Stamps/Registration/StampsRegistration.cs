using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.Postal.Stamps.WebServices;
using System.Net;
using log4net;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    public class StampsRegistration
    {
        private IStampsRegistrationValidator validator;
        private IStampsRegistrationGateway registrationGateway;
        private ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsRegistration"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="gateway">The gateway.</param>
        public StampsRegistration(IStampsRegistrationValidator validator, IStampsRegistrationGateway gateway)
            : this(validator, gateway, LogManager.GetLogger(typeof(StampsRegistration)))
        { }

        /// <summary>
        /// Constructor for testing purposes. Initializes a new instance of the <see cref="StampsRegistration"/> class.
        /// </summary>
        public StampsRegistration(IStampsRegistrationValidator validator, IStampsRegistrationGateway gateway, ILog log)
        {
            this.validator = validator;
            registrationGateway = gateway;
            this.log = log;

            
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
            MachineInfo.IPAddress = GetIPAddress();

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
        /// Submits the registration to Stamps.com.
        /// </summary>
        /// <returns>Any registration validation errors that may have occurred.</returns>
        public StampsRegistrationResult Submit()
        {
            // Validate the registration info
            IEnumerable<RegistrationValidationError> errors = validator.Validate(this);

            if (errors.Count() > 0)
            {
                StampsRegistrationException registrationException = CreateRegistrationException(errors);
                throw registrationException;
            }

            // Send the registration data to the stamps.com web service API
            return registrationGateway.Register(this);
        }

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <returns>A version 4 IP address.</returns>
        private string GetIPAddress()
        {
            string ipAddress = string.Empty;

            // Grab the version 4 IP address of the client machine before submitting the registration to Stamps.com
            string hostName = Dns.GetHostName();
            if (!string.IsNullOrEmpty(hostName))
            {
                ipAddress = Dns.GetHostAddresses(hostName).Where(ip => !IPAddress.IsLoopback(ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).First().ToString();
                log.InfoFormat("IP address submitted for Stamps.com registration was {0}", ipAddress);
            }
            else
            {
                throw new StampsRegistrationException("Stamps.com requires an IP address for registration, but ShipWorks could not obtain the IP address of this machine.");
            }

            return ipAddress;

        }

        /// <summary>
        /// Creates a registration exception.
        /// </summary>
        /// <param name="validationErrors">The validation errors.</param>
        /// <returns>A StampsRegistrationException object.</returns>
        private StampsRegistrationException CreateRegistrationException(IEnumerable<RegistrationValidationError> validationErrors)
        {
            StringBuilder message = new StringBuilder("There were problems registering your account with Stamps.com. Please correct the following field(s) in your stamps.com registration:" + Environment.NewLine);

            foreach (RegistrationValidationError error in validationErrors)
            {
                message.AppendLine(error.Message);
            }

            return new StampsRegistrationException(message.ToString());
        }
    }
}
