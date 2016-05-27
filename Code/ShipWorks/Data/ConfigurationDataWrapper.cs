using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides access to the global configuration object
    /// </summary>
    public class ConfigurationDataWrapper : IInitializeForCurrentDatabase
    {
        /// <summary>
        /// Completely reload the count cache
        /// </summary>
        public void InitializeForCurrentDatabase(ExecutionMode executionMode) =>
            ConfigurationData.InitializeForCurrentDatabase();
    }
}
