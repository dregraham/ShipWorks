using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ChannelAdvisorStoreData
    {
        /// <summary>
        /// ChannelAdvisor OAuth Refresh token
        /// </summary>
        [JsonProperty("RefreshToken")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// The date to start downloading orders from
        /// </summary>
        [JsonProperty("DownloadStartDate")]
        public DateTime? DownloadStartDate { get; set; }

        /// <summary>
        /// A list of item attribute names to be imported
        /// </summary>
        [JsonProperty("ItemAttributesToImport")]
        public IEnumerable<string> ItemAttributesToImport { get; set; }

        /// <summary>
        /// Country code for this store
        /// </summary>
        [JsonProperty("CountryCode")]
        public string CountryCode { get; set; }
    }
}