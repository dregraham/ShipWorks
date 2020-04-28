namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Interface for an ApiSettingsRepository
    /// </summary>
    public interface IApiSettingsRepository
    {
        /// <summary>
        /// Save settings
        /// </summary>
        void Save(ApiSettings settings);

        /// <summary>
        /// Load Settings
        /// </summary>
        ApiSettings Load();
    }
}
