using System;

namespace ShipWorks.Data.Administration
{
    public interface ISqlSchemaVersion
    {
        /// <summary>
        /// Get the schema version of the ShipWorks database using the current connection
        /// </summary>
        Version GetInstalledSchemaVersion();

        /// <summary>
        /// Determines if on a version where customer license is supported
        /// </summary>
        /// <returns></returns>
        bool IsCustomerLicenseSupported();
    }
}