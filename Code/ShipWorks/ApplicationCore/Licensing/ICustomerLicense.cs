using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for customer license
    /// </summary>
    public interface ICustomerLicense : ILicense
    {
        /// <summary>
        /// Saves the license
        /// </summary>
        void Save();

        /// <summary>
        /// Activate a customer license
        /// </summary>
        void Activate(string email, string password);

        /// <summary>
        /// IEnumerable of ActiveStores for the license
        /// </summary>
        IEnumerable<ActiveStore> GetActiveStores();

        /// <summary>
        /// The license capabilities
        /// </summary>
        ILicenseCapabilities LicenseCapabilities { get; set; }
    }
}