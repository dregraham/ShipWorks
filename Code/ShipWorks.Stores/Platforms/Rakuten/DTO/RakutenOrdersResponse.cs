using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
	/// <summary>
	/// Response returned by Rakuten when searching for orders
	/// </summary>
	[Obfuscation(Exclude = true)]
	public class RakutenOrdersResponse : RakutenBaseResponse
    {
        /// <summary>
        /// The list of Rakuten orders
        /// </summary>
        [JsonProperty("orders")]
        public IList<RakutenOrder> Orders { get; set; }

        /// <summary>
        /// The total number of orders in all pages of the response
        /// </summary>
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}
