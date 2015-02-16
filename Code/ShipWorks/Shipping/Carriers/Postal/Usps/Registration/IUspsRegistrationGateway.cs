using ShipWorks.Shipping.Carriers.Postal.Stamps.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// An interface for registering an account with Stamps.com.
    /// </summary>
    public interface IUspsRegistrationGateway
    {
        /// <summary>
        /// Registers an account with Stamps.com.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A StampsRegistrationResult object.</returns>
        UspsRegistrationResult Register(UspsRegistration registration);
    }
}
