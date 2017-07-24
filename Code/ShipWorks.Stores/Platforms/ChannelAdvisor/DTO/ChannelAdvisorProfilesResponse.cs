using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Channel Advisor profiles
    /// </summary>
    public class ChannelAdvisorProfilesResponse
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public IList<ChannelAdvisorProfile> Profiles { get; set; }
    }

    /// <summary>
    /// ChannelAdvisor profile info
    /// </summary>
    public class ChannelAdvisorProfile
    {
        [JsonProperty("ID")]
        public int ProfileId { get; set; }

        [JsonProperty("AccountName")]
        public string AccountName { get; set; }

        [JsonProperty("CompanyName")]
        public string CompanyName { get; set; }

        [JsonProperty("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("TimeZoneRegion")]
        public string TimeZoneRegion { get; set; }

        [JsonProperty("TimeZoneDescription")]
        public string TimeZoneDescription { get; set; }

        [JsonProperty("DefaultDistributionCenterID")]
        public string DefaultDistributionCenterId { get; set; }
    }
}