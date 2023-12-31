﻿using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Order Item entity returned by Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenOrderItem
    {
        [JsonProperty("baseSku")]
        public string BaseSKU { get; set; }

        [JsonProperty("orderItemId")]
        public string OrderItemID { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("discount")]
        public decimal Discount { get; set; }

        [JsonProperty("itemTotal")]
        public decimal ItemTotal { get; set; }

        [JsonProperty("sku")]
        public string SKU { get; set; }

        [JsonProperty("name")]
        public Dictionary<string, string> Name { get; set; }
    }
}