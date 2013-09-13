namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// An interface for registering an account with Express1.
    /// </summary>
    public interface IExpress1RegistrationGateway
    {
        /// <summary>
        /// Registers an account with Express1.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>An Express1RegistrationResult object.</returns>
        Express1RegistrationResult Register(Express1Registration registration);
    }
}
