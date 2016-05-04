namespace ShipWorks.ApplicationCore.Licensing.Activation
{
    /// <summary>
    /// An interface intended for carrying out the work for activating a customer license.
    /// </summary>
    public interface ICustomerLicenseActivationActivity
    {
        /// <summary>
        /// Uses the credentials provided to activate a customer license with the Tango web 
        /// client. Once verified with Tango, the license key is saved.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>An instance of an activated ICustomerLicense.</returns>
        ICustomerLicense Execute(string email, string password);
    }
}