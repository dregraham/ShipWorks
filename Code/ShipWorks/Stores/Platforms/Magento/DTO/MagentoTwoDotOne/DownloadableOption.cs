using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class DownloadableOption : IDownloadableOption
    {
        [JsonProperty("downloadable_links")]
        public IEnumerable<int> DownloadableLinks { get; set; }
    }
}