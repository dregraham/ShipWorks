using ShipWorks.Stores.Platforms.ShopSite.Dto;

namespace ShipWorks.Stores.Platforms.ShopSite
{
    /// <summary>
    /// Interface to connecting to ShopSite using OAuth and getting an access token
    /// </summary>
    public interface IShopSiteOauthAccessTokenWebClient
    {
        /// <summary>
        /// Get an AccessResponse for use in actual web calls
        /// </summary>
        AccessResponse FetchAuthAccessResponse();
    }
}
