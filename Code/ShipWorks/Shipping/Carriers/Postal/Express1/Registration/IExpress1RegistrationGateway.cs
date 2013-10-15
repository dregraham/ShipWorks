using System.Collections.Generic;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    /// <summary>
    /// An interface for registering an account with Express1.
    /// </summary>
    public interface IExpress1RegistrationGateway
    {
        /// <summary>
        /// Registers an account with Express1.
        /// </summary>
        /// <param name="registration">The registration data.</param>
        /// <returns>Returns the result of the registration call to Express1 containing the 
        /// customer credentials that were just created.</returns>
        Express1RegistrationResult Register(Express1Registration registration);

        /// <summary>
        /// Verifies that the specified username and password map to a valid account
        /// </summary>
        /// <param name="registration">Registration that defines the account to test</param>
        void VerifyAccount(Express1Registration registration);
    }
}
