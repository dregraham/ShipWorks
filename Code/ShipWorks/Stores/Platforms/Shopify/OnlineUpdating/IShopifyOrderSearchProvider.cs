using ShipWorks.Stores.Orders.Combine;

namespace ShipWorks.Stores.Platforms.Shopify.OnlineUpdating
{
    /// <summary>
    /// Order search provider for Shopify
    /// </summary>
    public interface IShopifyOrderSearchProvider : ICombineOrderSearchProvider<long>
    {
    }
}
