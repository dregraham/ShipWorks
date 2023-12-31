﻿using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Registration
{
    /// <summary>
    /// An IStampsRegistrationGateway implementation that calls into the Stamps API to register
    /// a new account. 
    /// </summary>
    public class StampsRegistrationGateway : IUspsRegistrationGateway
    {
        private readonly UspsResellerType resellerType;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsRegistrationGateway"/> class.
        /// </summary>
        public StampsRegistrationGateway(UspsResellerType resellerType)
        {
            this.resellerType = resellerType;
        }

        /// <summary>
        /// Registers an account with Stamps.com.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A StampsRegistrationResult object.</returns>
        public UspsRegistrationResult Register(StampsRegistration registration)
        {
            UspsRegistrationResult result = new UspsWebClient(resellerType).RegisterAccount(registration);

            if (!result.IsSuccessful)
            {
                string message = "An error occurred registering your account with Stamps.com. Please try again later or register on the Stamps.com website.";

                if (!string.IsNullOrEmpty(result.SuggestedUsername))
                {
                    // Stamps.com will suggest a username if the reason for failure is because
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
