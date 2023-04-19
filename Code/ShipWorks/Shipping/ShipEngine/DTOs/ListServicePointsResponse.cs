using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    public class ListServicePointsResponse : BaseShipEngineResponse
    {
        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("long")]
        public float Longitude { get; set; }

        [JsonProperty("service_points")]
        public ServicePoint[] ServicePoints { get; set; }
    }

    public class ServicePoint
    {
        [JsonProperty("carrier_code")]
        public string CarrierCode { get; set; }

        [JsonProperty("service_codes")]
        public string[] ServiceCodes { get; set; }

        [JsonProperty("service_point_id")]
        public string ServicePointId { get; set; }

        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        [JsonProperty("address_line1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("address_line2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("address_line2")]
        public string AddressLine3 { get; set; }

        [JsonProperty("city_locality")]
        public string City { get; set; }

        [JsonProperty("state_province")]
        public string StateProvince { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("lat")]
        public float Latitude { get; set; }

        [JsonProperty("long")]
        public float Longitude { get; set; }

        [JsonProperty("distance_in_meters")]
        public float DistanceInMeters { get; set; }
    }
}
