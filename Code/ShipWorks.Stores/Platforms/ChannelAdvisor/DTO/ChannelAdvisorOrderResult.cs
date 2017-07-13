using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Order Result entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorOrderResult
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("value")]
        public IList<ChannelAdvisorOrder> Orders { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }
    }
}