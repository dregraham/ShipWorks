using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class ThreeDCartOrderItem
    {
        [JsonProperty("CatalogID")]
        public int CatalogID { get; set; }

        [JsonProperty("ItemIndexID")]
        public int ItemIndexID { get; set; }

        [JsonProperty("ItemID")]
        public string ItemID { get; set; }

        [JsonProperty("ItemShipmentID")]
        public int ItemShipmentID { get; set; }

        [JsonProperty("ItemQuantity")]
        public double ItemQuantity { get; set; }

        [JsonProperty("ItemWarehouseID")]
        public int ItemWarehouseID { get; set; }

        [JsonProperty("ItemDescription")]
        public string ItemDescription { get; set; }

        [JsonProperty("ItemUnitPrice")]
        public decimal ItemUnitPrice { get; set; }

        [JsonProperty("ItemWeight")]
        public double ItemWeight { get; set; }

        [JsonProperty("ItemOptionPrice")]
        public decimal ItemOptionPrice { get; set; }

        [JsonProperty("ItemAdditionalField1")]
        public string ItemAdditionalField1 { get; set; }

        [JsonProperty("ItemAdditionalField2")]
        public string ItemAdditionalField2 { get; set; }

        [JsonProperty("ItemAdditionalField3")]
        public string ItemAdditionalField3 { get; set; }

        [JsonProperty("ItemPageAdded")]
        public string ItemPageAdded { get; set; }

        [JsonProperty("ItemDateAdded")]
        public DateTime ItemDateAdded { get; set; }

        [JsonProperty("ItemUnitCost")]
        public decimal ItemUnitCost { get; set; }

        [JsonProperty("ItemUnitStock")]
        public int ItemUnitStock { get; set; }

        [JsonProperty("ItemOptions")]
        public string ItemOptions { get; set; }

        [JsonProperty("ItemCatalogIDOptions")]
        public string ItemCatalogIDOptions { get; set; }
    }
}