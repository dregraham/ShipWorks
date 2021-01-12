using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Filters.Content;

namespace ShipWorks.Warehouse.Configuration.Filters.DTO
{
    /// <summary>
    /// The response from a GetAllFilters call
    /// </summary>
    public class GetFiltersResponse
    {
        /// <summary>
        /// The ID for this filter in the Hub
        /// </summary>
        [JsonProperty("SK")]
        public string HubFilterID { get; set; }

        /// <summary>
        /// The name of the filter
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The filter's definition
        /// </summary>
        [JsonProperty("definition")]
        public FilterDefinition Definition { get; set; }

        /// <summary>
        /// This filter's children
        /// </summary>
        [JsonProperty("Children")]
        public List<GetFiltersResponse> Children { get; set; }
    }
}
