using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides access to the global configuration object
    /// </summary>
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
        public ConfigurationEntity Fetch() =>
            ConfigurationData.Fetch();

        /// <summary>
        /// Load the configuration from the database
        /// </summary>
        public void CheckForChangesNeeded() =>
            ConfigurationData.CheckForChangesNeeded();
    }
}
