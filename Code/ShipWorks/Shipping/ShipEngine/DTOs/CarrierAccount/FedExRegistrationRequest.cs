using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs.CarrierAccount
{
    /// <summary>
    /// Request DTO for registering new FedEx account
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class FedExRegistrationRequest
    {
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }
        
        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("phone")]
        public string Phone { get; set; }
        
        [JsonProperty("address1")]
        public string Address1 { get; set; }
        
        [JsonProperty("address2")]
        public string Address2 { get; set; }
        
        [JsonProperty("city")]
        public string City { get; set; }
        
        [JsonProperty("state")]
        public string State { get; set; }
        
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
        
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
        
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("agree_to_eula")]
        public string AgreeToEula { get; set; }
    }
}
