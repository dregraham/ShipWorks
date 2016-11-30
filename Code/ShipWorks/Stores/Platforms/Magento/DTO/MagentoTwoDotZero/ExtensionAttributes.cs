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
        public IList<CustomOption> CustomOptions { get; set; }

        [JsonProperty("bundleOptions")]
        public IList<BundleOption> BundleOptions { get; set; }

        [JsonProperty("downloadableOption")]
        public DownloadableOption DownloadableOption { get; set; }

        [JsonProperty("configurableItemOptions")]
        public IList<ConfigurableItemOption> ConfigurableItemOptions { get; set; }

        [JsonProperty("fileInfo")]
        public FileInfo FileInfo { get; set; }
    }
}