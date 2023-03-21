using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Shopify
{
    public interface IShopifyDaysBackViewModel
    {
        /// <summary>
        /// Validation error message
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Number of days back to start downloading
        /// </summary>
        int InitialDownloadDays { get; set; }

        /// <summary>
        /// Save to the order source
        /// </summary>
        bool Save(ShopifyStoreEntity store);
    }
}