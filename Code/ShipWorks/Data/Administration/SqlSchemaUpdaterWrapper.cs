using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Wrapper for the static SqlSchemaUpdater
    /// </summary>
    [Component]
    public class SqlSchemaUpdaterWrapper : ISqlSchemaUpdater
    {
        /// <summary>
        /// Get the schema version of the ShipWorks database
        /// </summary>
        public Version GetInstalledSchemaVersion() =>
            SqlSchemaUpdater.GetInstalledSchemaVersion();

        /// <summary>
        /// Get the current build version
        /// </summary>
        /// <returns></returns>
        public Version GetBuildVersion() =>
            SqlSchemaUpdater.GetBuildVersion();
    }
}
