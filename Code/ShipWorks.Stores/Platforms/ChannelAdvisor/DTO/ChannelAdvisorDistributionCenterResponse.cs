using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    public class ChannelAdvisorDistributionCenterResponse
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public IEnumerable<ChannelAdvisorDistributionCenter> DistributionCenters { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }
    }
}