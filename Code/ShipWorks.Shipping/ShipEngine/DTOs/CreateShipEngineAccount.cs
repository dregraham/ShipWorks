using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    /// <summary>
    /// Create ShipEngine account DTO
    /// </summary>
    [Obfuscation()]
    public class CreateShipEngineAccount
    {
        /// <summary>
        /// First Name
        /// </summary>
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Company Name
        /// </summary>
        [JsonProperty("company_name")]
        public string CompanyName { get; set; }

        /// <summary>
        /// External Account ID
        /// </summary>
        [JsonProperty("external_account_id")]
        public string ExternalAccountID { get; set; }

        /// <summary>
        /// Origin Country Code
        /// </summary>
        [JsonProperty("origin_country_code")]
        public string OriginCountryCode { get; set; }
    }
}
