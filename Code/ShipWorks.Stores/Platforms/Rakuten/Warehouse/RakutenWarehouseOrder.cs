using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Rakuten.DTO;

namespace ShipWorks.Stores.Platforms.Rakuten.Warehouse
{
    /// <summary>
    /// Rakuten warehouse order
    /// </summary>
    [Obfuscation]
    public class RakutenWarehouseOrder
    {
        /// <summary>
        /// Order number postfix
        /// </summary>
        /// <value></value>
        [JsonProperty("packageId")]
        public string PackageID { get; set; }
    }
}
