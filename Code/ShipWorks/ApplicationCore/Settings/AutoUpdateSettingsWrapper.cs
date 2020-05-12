using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Escalator;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Wrapper for AutoUpdateSettings
    /// </summary>
    [Component]
    public class AutoUpdateSettingsWrapper : IAutoUpdateSettings
    {
        /// <summary>
        /// Whether Auto Update is enabled (this inverts the IsAutoUpdateDisabled for clarity)
        /// </summary>
        public bool IsAutoUpdateEnabled => !AutoUpdateSettings.IsAutoUpdateDisabled;

        /// <summary>
        /// Whether the last auto update succeeded
        /// </summary>
        public bool LastAutoUpdateSucceeded => AutoUpdateSettings.LastAutoUpdateSucceeded;
    }
}
