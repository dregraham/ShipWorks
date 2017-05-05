using ShipWorks.Actions;
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

        /// <summary>
        /// Gets the ActionQueueType for this process based on if it's background or not
        /// </summary>
        ActionQueueType ExecutionModeActionQueueType { get; }

        /// <summary>
        /// Should UI actions be included.  If the UI isn't running somehwere, 
        /// and we are the background process, go ahead and do UI actions too since it's not open
        /// </summary>
        bool IncludeUserInterfaceActions { get; }
    }
}