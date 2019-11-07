using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Response returned by Rakuten when searching for orders
    /// </summary>
    public class RakutenOrdersResponse
    {
        [JsonProperty("orders")]
        IList<RakutenOrder> Orders { get; set; }

        [JsonProperty("errors")]
        RakutenErrors Errors { get; set; }

        [JsonProperty("totalCount")]
        int TotalCount { get; set; }
    }
}
