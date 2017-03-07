using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides access to the global configuration object
    /// </summary>
    [Component]
    public class ConfigurationDataWrapper : IInitializeForCurrentDatabase, IConfigurationData
    {
        /// <summary>
        /// Completely reload the count cache
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            ConfigurationData.InitializeForCurrentDatabase();

        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public IConfigurationEntity FetchReadOnly() =>
            ConfigurationData.FetchReadOnly();

        /// <summary>
        /// Load the configuration from the database
        /// </summary>
        public void CheckForChangesNeeded() =>
            ConfigurationData.CheckForChangesNeeded();
    }
}
