namespace ShipWorks.ApplicationCore.Settings.Api
{
    /// <summary>
    /// Represents the API page in Settings
    /// </summary>
    public interface IApiSettingsPage : ISettingsPage
    {
        /// <summary>
        /// Whether or not the api settings page is currently in the process of saving
        /// </summary>
        bool IsSaving { get; }
    }
}
