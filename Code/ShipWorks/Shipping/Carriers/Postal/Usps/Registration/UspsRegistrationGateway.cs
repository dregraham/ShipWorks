using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// An IUspsRegistrationGateway implementation that calls into the USPS API to register
    /// a new account. 
    /// </summary>
    public class UspsRegistrationGateway : IUspsRegistrationGateway
    {
        private readonly UspsResellerType resellerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRegistrationGateway"/> class.
        /// </summary>
        public UspsRegistrationGateway(UspsResellerType resellerType)
        {
            this.resellerType = resellerType;
        }

        /// <summary>
        /// Registers an account with USPS .
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A UspsRegistrationResult object.</returns>
        public UspsRegistrationResult Register(UspsRegistration registration)
        {
            UspsRegistrationResult result = new UspsWebClient(resellerType).RegisterAccount(registration);

            if (!result.IsSuccessful)
            {
                string message = "An error occurred registering your account with Stamps.com. Please try again later or register on the Stamps.com website.";

                if (!string.IsNullOrEmpty(result.SuggestedUsername))
                {
                    // USPS will suggest a username if the reason for failure is because
                    // the username that was provided already being used.
                    message = string.Format("The username {0} is unavailable within Stamps.com, but {1} is available. Please enter a different username and try again.", 
                        registration.UserName, result.SuggestedUsername);
                }

                throw new UspsRegistrationException(message);
            }

            return result;
        }
    }
}
