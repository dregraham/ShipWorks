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

        /// <summary>
        /// The site accounts
        /// </summary>
        [JsonProperty("SiteAccounts")]
        public IList<ChannelAdvisorSiteAccount> SiteAccounts { get; set; }
    }

    /// <summary>
    /// A ChannelAdisor Site Account
    /// </summary>
    public class ChannelAdvisorSiteAccount
    {
        /// <summary>
        /// The Site Account ID
        /// </summary>
        [JsonProperty("ID")]
        public int ID { get; set; }

        /// <summary>
        /// The name of the Site Account
        /// </summary>
        [JsonProperty("SiteAccountName")]
        public string SiteAccountName { get; set; }

        /// <summary>
        /// The Site ID
        /// </summary>
        [JsonProperty("SiteID")]
        public int SiteID { get; set; }

        /// <summary>
        /// The name of the Site
        /// </summary>
        [JsonProperty("SiteName")]
        public string SiteName { get; set; }

        /// <summary>
        /// Whether or not this Site Account is enabled
        /// </summary>
        [JsonProperty("Enabled")]
        public bool Enabled { get; set; }
    }
}