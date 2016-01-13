using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for license factory
    /// </summary>
    public interface ILicenseFactory
    {
        /// <summary>
        /// Gets all licenses for shipworks
        /// </summary>
        /// <returns></returns>
        IEnumerable<ILicense> GetLicenses();
    }
}