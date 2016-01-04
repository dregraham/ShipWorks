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
        public ConfigurationEntity Fetch()
        {
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