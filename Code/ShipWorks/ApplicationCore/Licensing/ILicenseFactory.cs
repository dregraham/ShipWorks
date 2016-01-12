using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Factory to create a License
    /// </summary>
    public interface ILicenseFactory
    {
        /// <summary>
        /// Returns the correct ILicense for the store
        /// </summary>
        ILicense GetLicense(StoreEntity store);

        /// <summary>
        /// Gets all Licenses.
        /// </summary>
        IEnumerable<ILicense> GetLicenses();
    }
}