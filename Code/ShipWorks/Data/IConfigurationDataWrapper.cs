using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    public interface IConfigurationDataWrapper
    {
        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        ConfigurationEntity GetConfiguration();

        /// <summary>
        /// Save the given entity as the current configuration
        /// </summary>
        void Save(ConfigurationEntity configuration);
    }
}