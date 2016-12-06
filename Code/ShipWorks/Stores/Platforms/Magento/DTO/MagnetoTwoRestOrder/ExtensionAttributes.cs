using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class ExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        public IEnumerable<ShippingAssignment> ShippingAssignments { get; set; }

        [JsonProperty("custom_options")]
        public IEnumerable<CustomOption> CustomOptions { get; set; }

        [JsonProperty("bundle_options")]
        public IEnumerable<BundleOption> BundleOptions { get; set; }

        [JsonProperty("downloadable_option")]
        public DownloadableOption DownloadableOption { get; set; }

        [JsonProperty("configurable_item_options")]
        public IEnumerable<ConfigurableItemOption> ConfigurableItemOptions { get; set; }
    }
}