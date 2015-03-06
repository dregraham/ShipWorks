namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    /// <summary>
    /// An interface for registering an account with USPS.
    /// </summary>
    public interface IUspsRegistrationGateway
    {
        /// <summary>
        /// Registers an account with USPS.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>A UspsRegistrationResult object.</returns>
        UspsRegistrationResult Register(UspsRegistration registration);
    }
}
