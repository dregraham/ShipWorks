namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for reading the license information
    /// </summary>
    public interface ICustomerLicenseReader
    {
        /// <summary>
        /// Reads the customer license key from the database
        /// </summary>
        string Read();
    }
}
