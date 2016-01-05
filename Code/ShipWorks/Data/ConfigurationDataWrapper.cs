using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Wrapper for the ConfigurationData static class
    /// </summary>
    public class ConfigurationDataWrapper : IConfigurationDataWrapper
    {
        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        public ConfigurationEntity GetConfiguration()
        {
            // Fetch the current configuration entity
            ConfigurationEntity config = ConfigurationData.Fetch();

            // if its null because we are in the database setup wizard
            if (config == null)
            {
                ConfigurationData.InitializeForCurrentDatabase();
            }

            return ConfigurationData.Fetch();
        }

        /// <summary>
        /// Save the given entity as the current configuration
        /// </summary>
        public void Save(ConfigurationEntity configuration)
        {
            ConfigurationData.Save(configuration);
        }
    }
}