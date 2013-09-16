namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// A data transfer object that encapsulates the registration status and values of the output parameters
    /// from the Express1 API registration request.
    /// </summary>
    public class Express1RegistrationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Express1RegistrationResult"/> class.
        /// </summary>
        public Express1RegistrationResult()
        {
            AccountNumber = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}