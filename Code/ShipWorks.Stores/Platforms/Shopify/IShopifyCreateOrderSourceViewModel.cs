using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Viewmodel to set up an Shopify Order Source
    /// </summary>
    public interface IShopifyCreateOrderSourceViewModel
    {
        /// <summary>
        /// Load the store
        /// </summary>
        void Load(ShopifyStoreEntity store);
        
        /// <summary>
        /// Save the store
        /// </summary>
        bool Save(ShopifyStoreEntity store);

        string EncodedOrderSource { get; set; }

        string ShopifyShopUrlName { get; set; }
    }
}