namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for reading the license information
    /// </summary>
    public interface ICustomerLicenseReader
    {
        /// <summary>
        /// Reads this License
        /// </summary>
        string Read();
    }
}
