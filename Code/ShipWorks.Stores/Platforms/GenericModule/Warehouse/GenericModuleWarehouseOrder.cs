using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.GenericModule.Warehouse
{
    /// <summary>
    /// Amazon warehouse order
    /// </summary>
    [Obfuscation]
    public class GenericModuleWarehouseOrder
    {
        /// <summary>
        /// The orders Amazon order ID
        /// </summary>
        [JsonProperty("amazonOrderId")]
        public string AmazonOrderID { get; set; }

        /// <summary>
        /// Is the order fulfilled by Amazon
        /// </summary>
        [JsonProperty("isFba")]
        public bool IsFBA { get; set; }

        /// <summary>
        /// The orders Amazon Prime status
        /// </summary>
        public int IsPrime { get; set; }

        /// <summary>
        /// Is the order same day
        /// </summary>
        public bool IsSameDay { get; set; }

        /// <summary>
        /// Order source
        /// </summary>
        public string OrderSource { get; set; }
    }
}
