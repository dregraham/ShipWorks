using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Warehouse.Orders.DTO
{
    /// <summary>
    /// Charge for a warehouse order
    /// </summary>
    [Obfuscation]
    public class WarehouseOrderCharge
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Type of the order charge
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description of the order charge
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Amount of the order charge
        /// </summary>
        public decimal Amount { get; set; }
    }
}
