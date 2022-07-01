using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    /// <summary>
    /// DTO for migrating an MWS store to an SP store
    /// </summary>
    [Obfuscation]
    public class MigrateMwsToSpRequest
    {
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("sellingPartnerId")]
        public string SellingPartnerId { get; set; }

        [JsonProperty("mwsAuthToken")]
        public string MwsAuthToken { get; set; }

        [JsonProperty("lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [JsonProperty("includeFba")]
        public bool IncludeFba { get; set; }
    }
}
