using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// The store-specific identification key of the request
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenShopKey
    {
        [JsonProperty("marketplaceIdentifier")]
        public string MarketplaceID { get; set; }

        [JsonProperty("shopUrl")]
        public string ShopURL { get; set; }

        /// <summary>
        /// Constructor for deserialization
        /// </summary>
        [JsonConstructor]
        public RakutenShopKey() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenShopKey(IRakutenStoreEntity store)
        {
            this.MarketplaceID = store.MarketplaceID;
            this.ShopURL = store.ShopURL;
        }
    }
}
