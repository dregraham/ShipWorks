using System.ComponentModel;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Shop details for a Shopify store
    /// </summary>
    [Obfuscation]
    public class ShopifyShop
    {
        /// <summary>
        /// The Store Name
        /// </summary>
        [DefaultValue("Shopify Store")]
        [JsonProperty("name", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string StoreName { get; set; }

        /// <summary>
        /// The Street 1
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("address1", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Street1 { get; set; }

        /// <summary>
        /// The City
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("city", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string City { get; set; }

        /// <summary>
        /// The State/Province Code
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("province", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string StateProvince { get; set; }

        /// <summary>
        /// The Postal Code
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("zip", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string PostalCode { get; set; }

        /// <summary>
        /// The Country Code
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("country", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Country { get; set; }

        /// <summary>
        /// The Email
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("email", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Email { get; set; }

        /// <summary>
        /// The Phone
        /// </summary>
        [DefaultValue("")]
        [JsonProperty("phone", DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Phone { get; set; }

        /// <summary>
        /// The Primary Location ID
        /// </summary>
        [JsonProperty("primary_location_id", DefaultValueHandling = DefaultValueHandling.Populate)]
        public long PrimaryLocationID { get; set; }
    }
}
