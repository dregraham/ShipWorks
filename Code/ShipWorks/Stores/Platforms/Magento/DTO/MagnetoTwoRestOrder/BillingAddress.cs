using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class BillingAddress
    {
        [JsonProperty("address_type")]
        public string AddressType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country_id")]
        public string CountryId { get; set; }

        [JsonProperty("customer_address_id")]
        public int CustomerAddressId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("middlename")]
        public string Middlename { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("region_code")]
        public string RegionCode { get; set; }

        [JsonProperty("region_id")]
        public int RegionId { get; set; }

        [JsonProperty("street")]
        public IList<string> Street { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }
    }
}