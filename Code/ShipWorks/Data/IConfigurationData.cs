using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data
{
    /// <summary>
    /// Configuration data interface
    /// </summary>
    public interface IConfigurationData
    {
        /// <summary>
        /// Get the current configuration instance
        /// </summary>
        IConfigurationEntity FetchReadOnly();

        /// <summary>
        /// Load the configuration from the database
        /// </summary>
        void CheckForChangesNeeded();
    }
}