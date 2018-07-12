using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Net;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.Shopify.DTOs
{
    /// <summary>
    /// Shop details for a Shopify store
    /// </summary>
    public class ShopifyShop
    {
        private readonly JToken shopJson;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyShop(JToken shopJson)
        {
            this.shopJson = shopJson;
        }

        /// <summary>
        /// The Store Name
        /// </summary>
        public string StoreName => shopJson.GetValue("name", "Shopify Store");

        /// <summary>
        /// The Street 1
        /// </summary>
        public string Street1 => shopJson.GetValue("address1", string.Empty);

        /// <summary>
        /// The City
        /// </summary>
        public string City => shopJson.GetValue("city", string.Empty);

        /// <summary>
        /// The State/Province Code
        /// </summary>
        public string StateProvCode => Geography.GetStateProvCode(shopJson.GetValue("province", string.Empty));

        /// <summary>
        /// The Postal Code
        /// </summary>
        public string PostalCode => shopJson.GetValue("zip", string.Empty);

        /// <summary>
        /// The Country Code
        /// </summary>
        public string CountryCode => Geography.GetCountryCode(shopJson.GetValue("country", string.Empty));

        /// <summary>
        /// The Email
        /// </summary>
        public string Email => shopJson.GetValue("email", string.Empty);

        /// <summary>
        /// The Phone
        /// </summary>
        public string Phone => shopJson.GetValue("phone", string.Empty);

        /// <summary>
        /// The Primary Location ID
        /// </summary>
        public long? PrimaryLocationID => shopJson.GetValue<long?>("primary_location_id");
    }
}
