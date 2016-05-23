using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandBillingAddress
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("street_address")]
        public string StreetAddress { get; set; }

        [JsonProperty("street_address_line2")]
        public string StreetAddress2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}