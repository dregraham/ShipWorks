using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class DownloadableOption
    {
        [JsonProperty("downloadable_links")]
        public IEnumerable<int> DownloadableLinks { get; set; }
    }
}