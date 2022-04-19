using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Amazon store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class AmazonStore : Store
    {
        /// <summary>
        /// The date to start downloading orders from
        /// </summary>
        public ulong DownloadStartDate { get; set; }

        /// <summary>
        /// The merchant ID for this store
        /// </summary>
        [JsonProperty("merchantId")]
        public string MerchantID { get; set; }

        /// <summary>
        /// The marketplace ID for this store
        /// </summary>
        [JsonProperty("marketplaceId")]
        public string MarketplaceID { get; set; }

        /// <summary>
        /// The auth token for this store
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Whether or not this store should exclude FBA orders
        /// </summary>
        [JsonProperty("excludeFba")]
        public bool ExcludeFBA { get; set; }

        /// <summary>
        /// The region code for this store
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Whether or not Amazon VATS applies to this store
        /// </summary>
        [JsonProperty("amazonVats")]
        public bool AmazonVATS { get; set; }

        /// <summary>
        /// The platform order source id 
        /// </summary>
        [JsonProperty("orderSourceId")]
        public string OrderSourceID { get; set; }

        /// <summary>
        /// The platform account id (Account Service AccountId)
        /// </summary>
        public string PlatformAccountId { get; set; }
    }
}