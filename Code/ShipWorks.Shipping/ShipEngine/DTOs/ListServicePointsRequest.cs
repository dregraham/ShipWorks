using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    public class ListServicePointsRequest
    {
        [JsonProperty("radius")]
        public int Radius { get; set; }

        [JsonProperty("max_results")]
        public int MaxResults { get; set; }

        [JsonProperty("providers")]
        public IEnumerable<ServicePointProvider> Providers { get; set; }

        [JsonProperty("address")]
        public ServicePointSearchAddress SearchAddress { get; set; }
    }

    public class ServicePointProvider
    {
        [JsonProperty("carrier_id")]
        public string CarrierId { get; set; }
    }

    public class ServicePointSearchAddress
    {
        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("address_line3")]
        public string AddressLine3 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state_province")]
        public string StateProvince { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }
}
