using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Shopify.OnlineUpdating
{
    /// <summary>
    /// Order search provider for Shopify
    /// </summary>
    public interface IShopifyOrderSearchProvider : ICombineOrderSearchProvider<ShopifyOrderSearchEntity>
    {
    }
}
