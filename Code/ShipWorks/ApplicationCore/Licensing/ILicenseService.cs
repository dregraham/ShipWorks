using System.Collections.Generic;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for license service
    /// </summary>
    public interface ILicenseService
    {
        /// <summary>
        /// Gets all licenses for ShipWorks
        /// </summary>
        /// <returns></returns>
        IEnumerable<ILicense> GetLicenses();

        /// <summary>
        /// Can the customer Logon?
        /// </summary>
        EnumResult<LogOnRestrictionLevel> AllowsLogOn();
    }
}