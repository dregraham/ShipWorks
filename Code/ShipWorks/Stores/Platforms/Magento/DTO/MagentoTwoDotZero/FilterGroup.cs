using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class FilterGroup
    {
        [JsonProperty("filters")]
        public IList<Filter> Filters { get; set; }
    }
}