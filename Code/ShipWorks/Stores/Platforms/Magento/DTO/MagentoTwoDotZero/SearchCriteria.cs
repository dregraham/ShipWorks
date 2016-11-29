using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class SearchCriteria
    {
        [JsonProperty("filterGroups")]
        public IList<FilterGroup> FilterGroups { get; set; }

        [JsonProperty("sortOrders")]
        public IList<SortOrder> SortOrders { get; set; }

        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
    }
}