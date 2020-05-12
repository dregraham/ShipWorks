namespace ShipWorks.Escalator
{
    /// <summary>
    /// Interface for AutoUpdateSettingsWrapper
    /// </summary>
    public interface IAutoUpdateSettings
    {
        /// <summary>
        /// Whether Auto Update is enabled (this inverts the IsAutoUpdateDisabled for clarity)
        /// </summary>
        bool IsAutoUpdateEnabled { get; }

        /// <summary>
        /// Whether the last auto update succeeded
        /// </summary>
        bool LastAutoUpdateSucceeded { get; }
    }
}
