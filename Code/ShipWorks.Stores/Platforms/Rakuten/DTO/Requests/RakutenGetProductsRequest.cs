using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO.Requests
{
    /// <summary>
    /// The request to get products from Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenGetProductsRequest
    {
        /// <summary>
        /// The base SKU to get information about
        /// </summary>
        [JsonProperty("baseSku")]
        public string BaseSKU { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RakutenGetProductsRequest(string baseSKU)
        {
            this.BaseSKU = baseSKU;
        }
    }
}
