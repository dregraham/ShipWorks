using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    /// <summary>
    /// Order Item Result entity returned by ChannelAdvisor
    /// </summary>
    public class ChannelAdvisorOrderItemsResult
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }
        
        [JsonProperty("value")]
        public IList<ChannelAdvisorOrderItem> OrderItems { get; set; }
    }
}