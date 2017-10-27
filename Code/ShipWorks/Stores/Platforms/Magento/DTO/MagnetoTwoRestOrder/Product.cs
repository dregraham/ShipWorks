﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ProductsResponse
    {
        [JsonProperty("items")]
        public IEnumerable<Product> Products { get; set; }
    }

    public class Product
    {
        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("options")]
        public IEnumerable<ProductOptionDetail> Options { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }
    }
}