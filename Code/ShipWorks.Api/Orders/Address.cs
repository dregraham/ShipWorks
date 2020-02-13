using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// Address object
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class Address
    {
        /// <summary>
        /// The name of the recipient
        /// </summary>
        [JsonProperty("recipientName")]
        public string RecipientName { get; set; }

        /// <summary>
        /// Line 1 of the street address
        /// </summary>
        [JsonProperty("street1")]
        public string Street1 { get; set; }

        /// <summary>
        /// Line 2 of the street address
        /// </summary>
        [JsonProperty("street2")]
        public string Street2 { get; set; }

        /// <summary>
        /// Line 3 of the street address
        /// </summary>
        [JsonProperty("street3")]
        public string Street3 { get; set; }

        /// <summary>
        /// The city
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The state or province
        /// </summary>
        [JsonProperty("stateProvince")]
        public string StateProvince { get; set; }

        /// <summary>
        /// The 2 digit ISO country code
        /// </summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// The postal code
        /// </summary>
        [JsonProperty("postalCode")]
        public int PostalCode { get; set; }
    }
}
