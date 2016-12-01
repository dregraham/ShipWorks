using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class ExtensionAttributes : IExtensionAttributes
    {
        [JsonProperty("shipping_assignments")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IShippingAssignment>, List<ShippingAssignment>>))]
        public IEnumerable<IShippingAssignment> ShippingAssignments { get; set; }

        [JsonProperty("custom_options")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<ICustomOption>, List<CustomOption>>))]
        public IEnumerable<ICustomOption> CustomOptions { get; set; }

        [JsonProperty("bundle_options")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IBundleOption>, List<BundleOption>>))]
        public IEnumerable<IBundleOption> BundleOptions { get; set; }

        [JsonProperty("downloadable_option")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IDownloadableOption, DownloadableOption>))]
        public IDownloadableOption DownloadableOption { get; set; }

        [JsonProperty("configurable_item_options")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IConfigurableItemOption>, List<ConfigurableItemOption>>))]
        public IEnumerable<IConfigurableItemOption> ConfigurableItemOptions { get; set; }
    }
}