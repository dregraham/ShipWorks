namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    ///
    /// </summary>
    public interface ICustomerLicenseActivationService
    {
        /// <summary>
        /// Activates a CustomerLicense
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        ICustomerLicense Activate(string username, string password);
    }
}