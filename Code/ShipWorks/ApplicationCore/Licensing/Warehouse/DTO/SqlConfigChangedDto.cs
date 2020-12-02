using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    /// <summary>
    /// SqlConfigChanged DTO
    /// </summary>
    [Obfuscation(StripAfterObfuscation = false)]
    public class SqlConfigChangedDto
    {
        [JsonProperty("warehouseData")]
        public WarehouseData WarehouseData { get; set; }
    }

    /// <summary>
    /// Warehouse data
    /// </summary>
    [Obfuscation(StripAfterObfuscation = false)]
    public class WarehouseData
    {
        /// <summary>
        /// Warehouse ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The SQL Conf JSON string
        /// </summary>
        [JsonProperty("sqlConfig")]
        public string SqlConfig { get; set; }
    }
}
