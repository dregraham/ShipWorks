using System.Reflection;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Shopify store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ShopifyStore : ShipEngineStore
    {
        /// <summary>
        /// The date to start downloading orders from
        /// </summary>
        public ulong DownloadStartDate { get; set; }

        /// <summary>
        /// The auth token this store
        /// </summary>
        public string ShopifyToken { get; set; }

        /// <summary>
        /// The store name in the url
        /// </summary>
        public string ShopifyShopUrlName { get; set; }
    }
}
