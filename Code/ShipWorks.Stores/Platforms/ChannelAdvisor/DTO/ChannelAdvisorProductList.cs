using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// List of products returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorProductList
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public IList<ChannelAdvisorProduct> Products { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }

    }
}