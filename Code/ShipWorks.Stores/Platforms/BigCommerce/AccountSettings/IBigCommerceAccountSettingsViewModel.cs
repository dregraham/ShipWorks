namespace ShipWorks.Stores.Platforms.BigCommerce.AccountSettings
{
    /// <summary>
    /// Basic interface for the BigCommerce account settings view model
    /// </summary>
    public interface IBigCommerceAccountSettingsViewModel
    {
        /// <summary>
        /// User name for legacy API access
        /// </summary>
        string BasicUsername { get; set; }

        /// <summary>
        /// Token for legacy API access
        /// </summary>
        string BasicToken { get; set; }

        /// <summary>
        /// Client ID for OAuth access
        /// </summary>
        string OauthClientID { get; set; }

        /// <summary>
        /// Access Token for OAuth access
        /// </summary>
        string OauthToken { get; set; }
    }
}