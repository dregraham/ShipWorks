using ShipWorks.Actions;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.UI;

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

        /// <summary>
        /// Gets the ActionQueueType for this process based on if it's background or not
        /// </summary>
        public ActionQueueType ExecutionModeActionQueueType
        {
            get
            {
                ActionQueueType actionQueueType = ActionQueueType.Scheduled;

                if (Program.ExecutionMode.IsUIDisplayed)
                {
                    actionQueueType = ActionQueueType.UserInterface;
                }

                return actionQueueType;
            }
        }

        /// <summary>
        /// Should UI actions be included.  If the UI isn't running somehwere, 
        /// and we are the background process, go ahead and do UI actions too since it's not open
        /// </summary>
        public bool IncludeUserInterfaceActions => !Program.ExecutionMode.IsUISupported &&
            !(Program.ExecutionMode is UserInterfaceExecutionMode || SingleInstance.IsAlreadyRunning);
    }
}
