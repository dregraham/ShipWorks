using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class AmazonStoreData
    {
        /// <summary>
        /// The date to start downloading orders from
        /// </summary>
        [JsonProperty("DownloadStartDate")]
        public DateTime? DownloadStartDate { get; set; }
        
        /// <summary>
        /// The merchant ID for this store
        /// </summary>
        [JsonProperty("MerchantID")]
        public string MerchantID { get; set; }
        
        /// <summary>
        /// The marketplace ID for this store
        /// </summary>
        [JsonProperty("MarketplaceID")]
        public string MarketplaceID { get; set; }
        
        /// <summary>
        /// The auth token for this store
        /// </summary>
        [JsonProperty("AuthToken")]
        public string AuthToken { get; set; }
        
        /// <summary>
        /// Whether or not this store should exclude FBA orders
        /// </summary>
        [JsonProperty("ExcludeFBA")]
        public bool ExcludeFBA { get; set; }
        
        /// <summary>
        /// The region code for this store
        /// </summary>
        [JsonProperty("Region")]
        public string Region { get; set; }
        
        /// <summary>
        /// Whether or not Amazon VATS applies to this store
        /// </summary>
        [JsonProperty("AmazonVATS")]
        public bool AmazonVATS { get; set; }
    }
}