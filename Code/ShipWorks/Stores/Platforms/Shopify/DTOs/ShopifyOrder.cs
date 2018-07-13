﻿using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Shopify order
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class ShopifyOrder
    {
        /// <summary>
        /// Line items
        /// </summary>
        [JsonProperty("line_items", NullValueHandling = NullValueHandling.Ignore)]
        public ShopifyLineItem[] LineItems { get; set; } = new ShopifyLineItem[0];
    }
}
