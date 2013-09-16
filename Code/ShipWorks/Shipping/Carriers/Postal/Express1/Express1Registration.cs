using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Express1Registration"/> class.
    /// </summary>
    public class Express1Registration
    {
        //private IStampsRegistrationValidator validator;
        private readonly IExpress1RegistrationGateway registrationGateway;
        private readonly IExpress1RegistrationRepository registrationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1Registration"/> class.
        /// </summary>
        /// <param name="shipmentType">Type of shipment this account will be used for.</param>
        /// <param name="gateway">The gateway.</param>
        public Express1Registration(ShipmentTypeCode shipmentType, IExpress1RegistrationGateway gateway, IExpress1RegistrationRepository repository)
        {
            ShipmentTypeCode = shipmentType;

            //this.validator = validator;
            registrationGateway = gateway;
            registrationRepository = repository;
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
        /// Gets and sets the Express1
        /// </summary>
        public string AccountNumber { get; set; }

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
        /// Add an existing Express1 account to ShipWorks
        /// </summary>
        public void AddExistingAccount()
        {
            registrationRepository.Save(this);
        }

        /// <summary>
        /// Create a new Express1 account
        /// </summary>
        public void CreateNewAccount()
        {
            registrationGateway.Register(this);
            registrationRepository.Save(this);
        }

        /// <summary>
        /// Save changes to an account.
        /// </summary>
        public void SaveAccount()
        {
            registrationRepository.Save(this);
        }
    }
}