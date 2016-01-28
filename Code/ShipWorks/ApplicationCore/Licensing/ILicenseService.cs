using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

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

        /// <summary>
        /// Returns the correct ILicense for the store
        /// </summary>
        ILicense GetLicense(StoreEntity store);
        
        /// <summary>
        /// If License is over the channel limit prompt user to delete channels
        /// </summary>
        void EnforceChannelLimit();
    }
}