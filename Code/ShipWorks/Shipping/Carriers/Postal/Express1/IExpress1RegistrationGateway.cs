using ShipWorks.Shipping.Carriers.Postal.Express1.WebServices.CustomerService;

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
        /// <param name="registration">The registration data.</param>
        /// <returns>Returns the result of the registration call to Express1 containing the 
        /// customer credentials that were just created.</returns>
        Express1RegistrationResult Register(Express1Registration registration);
    }
}
