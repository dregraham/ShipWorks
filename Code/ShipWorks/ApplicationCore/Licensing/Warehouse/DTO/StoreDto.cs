using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class StoreDto
    {
        /// <summary>
        /// The type of store this is
        /// </summary>
        [JsonProperty("storeType")]
        public string StoreType { get; set; }
        
        /// <summary>
        /// Store data needed to download orders, such as credentials
        /// </summary>
        [JsonProperty("storeData")]
        public object StoreData { get; set; }
    }
}