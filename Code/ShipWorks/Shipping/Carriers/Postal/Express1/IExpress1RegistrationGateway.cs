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
        /// <param name="customerData">The registration data.</param>
        /// <returns>Returns the customer credentials from the registration call.</returns>
        CustomerCredentials Register(CustomerRegistrationData customerData);
    }
}
