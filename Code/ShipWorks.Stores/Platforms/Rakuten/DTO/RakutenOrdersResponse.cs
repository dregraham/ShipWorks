using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Response returned by Rakuten when searching for orders
    /// </summary>
    public class RakutenOrdersResponse
    {
        /// <summary>
        /// The list of Rakuten orders
        /// </summary>
        [JsonProperty("orders")]
        public IList<RakutenOrder> Orders { get; set; }

        /// <summary>
        /// The list of errors encountered, if any
        /// </summary>
        [JsonProperty("errors")]
        public RakutenErrors Errors { get; set; }

        /// <summary>
        /// The total number of orders in all pages of the response
        /// </summary>
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
