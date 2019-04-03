using System;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Represents the SqlSchemaUpdater
    /// </summary>
    public interface ISqlSchemaUpdater
    {
        /// <summary>
        /// Get the schema version of the ShipWorks database
        /// </summary>
        Version GetInstalledSchemaVersion();

        /// <summary>
        /// Get the current build version
        /// </summary>
        /// <returns></returns>
        Version GetBuildVersion();
    }
}