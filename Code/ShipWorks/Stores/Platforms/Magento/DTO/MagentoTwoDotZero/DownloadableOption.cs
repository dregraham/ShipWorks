using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class DownloadableOption : IDownloadableOption
    {
        [JsonProperty("downloadableLinks")]
        public IEnumerable<int> DownloadableLinks { get; set; }
    }
}