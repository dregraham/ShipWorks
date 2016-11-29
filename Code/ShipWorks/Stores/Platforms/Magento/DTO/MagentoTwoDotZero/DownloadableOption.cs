using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class DownloadableOption
    {
        [JsonProperty("downloadableLinks")]
        public IList<int> DownloadableLinks { get; set; }
    }
}