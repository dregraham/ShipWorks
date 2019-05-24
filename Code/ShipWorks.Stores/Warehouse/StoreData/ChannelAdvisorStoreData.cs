using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;

namespace ShipWorks.Stores.Warehouse.StoreData
{
    /// <summary>
    /// Channel Advisor store credentials needed for downloading
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ChannelAdvisorStore : Store
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