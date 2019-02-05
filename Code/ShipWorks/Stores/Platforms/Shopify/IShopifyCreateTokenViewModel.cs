using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// View model to create a Shopify token
    /// </summary>
    public interface IShopifyCreateTokenViewModel
    {
        /// <summary>
        /// Create a Shopify token
        /// </summary>
        GenericResult<(string name, string token)> CreateToken();

        /// <summary>
        /// Regenerate a Shopify token
        /// </summary>
        GenericResult<string> RefreshToken(string name);
    }
}