namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface to writing the license information
    /// </summary>
    public interface ICustomerLicenseWriter
    {
        /// <summary>
        /// Writes the license
        /// </summary>
        void Write(ICustomerLicense customerLicense);
    }
}