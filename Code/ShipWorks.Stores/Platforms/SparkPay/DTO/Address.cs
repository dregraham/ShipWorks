using Newtonsoft.Json;
using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public class Address
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("customer_id")]
        public int? CustomerId { get; set; }

        [JsonProperty("address_line_1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("address_line_2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("is_default_shipping_address")]
        public bool IsDefaultShippingAddress { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("alternate_phone")]
        public string AlternatePhone { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("is_default_billing_address")]
        public bool IsDefaultBillingAddress { get; set; }

        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }
    }
}
