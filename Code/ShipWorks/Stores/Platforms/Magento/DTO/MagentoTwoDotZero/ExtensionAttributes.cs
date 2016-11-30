using System.Collections.Generic;
using Interapptive.Shared.Utility.Json;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ExtensionAttributes : IExtensionAttributes
    {
        [JsonProperty("shippingAssignments")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IShippingAssignment>, List<ShippingAssignment>>))]
        public IEnumerable<IShippingAssignment> ShippingAssignments { get; set; }

        [JsonProperty("customOptions")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<ICustomOption>, List<CustomOption>>))]
        public IEnumerable<ICustomOption> CustomOptions { get; set; }

        [JsonProperty("bundleOptions")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IBundleOption>, List<BundleOption>>))]
        public IEnumerable<IBundleOption> BundleOptions { get; set; }

        [JsonProperty("downloadableOption")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IDownloadableOption, DownloadableOption>))]
        public IDownloadableOption DownloadableOption { get; set; }

        [JsonProperty("configurableItemOptions")]
        [JsonConverter(typeof(InterfaceToClassJsonConverter<IEnumerable<IConfigurableItemOption>, List<ConfigurableItemOption>>))]
        public IEnumerable<IConfigurableItemOption> ConfigurableItemOptions { get; set; }

        [JsonProperty("fileInfo")]
        public FileInfo FileInfo { get; set; }
    }
}