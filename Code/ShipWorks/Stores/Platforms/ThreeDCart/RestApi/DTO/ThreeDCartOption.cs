using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class ThreeDCartOption
    {
        [JsonProperty("OptionID")]
        public int OptionID { get; set; }

        [JsonProperty("OptionName")]
        public string OptionName { get; set; }

        [JsonProperty("OptionSelected")]
        public bool OptionSelected { get; set; }

        [JsonProperty("OptionHide")]
        public bool OptionHide { get; set; }

        [JsonProperty("OptionValue")]
        public decimal OptionValue { get; set; }

        [JsonProperty("OptionPartNumber")]
        public string OptionPartNumber { get; set; }

        [JsonProperty("OptionSorting")]
        public int OptionSorting { get; set; }

        [JsonProperty("OptionImagePath")]
        public string OptionImagePath { get; set; }

        [JsonProperty("OptionBundleCatalogId")]
        public int OptionBundleCatalogId { get; set; }

        [JsonProperty("OptionBundleQuantity")]
        public int OptionBundleQuantity { get; set; }
    }
}