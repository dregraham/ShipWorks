using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [SuppressMessage("SonarQube", "S125:Sections of code should not be \"commented out\"", 
        Justification = "Commented out code shows an example of the json that is returned from the api")]
    public class LemonStandBillingAddress
    {
//}
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