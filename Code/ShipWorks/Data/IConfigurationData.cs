using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    public interface IConfigurationData
    {
        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        ConfigurationEntity Fetch();

        /// <summary>
        /// Load the configuration from the database
        /// </summary>
        void CheckForChangesNeeded();
    }
}