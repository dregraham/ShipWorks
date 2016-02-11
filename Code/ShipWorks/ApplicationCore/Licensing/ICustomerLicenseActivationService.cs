namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// A service interface for activating customer licenses in ShipWorks.
    /// </summary>
    public interface ICustomerLicenseActivationService
    {
        /// <summary>
        /// Attempts to use the email address and password provided to activate a customer license with Tango.
        /// </summary>
        ICustomerLicense Activate(string email, string password);
    }
}