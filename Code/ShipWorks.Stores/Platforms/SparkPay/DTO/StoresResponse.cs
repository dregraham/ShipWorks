using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public class Store
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("domain_name")]
        public string domain_name { get; set; }
        [JsonProperty("email")]
        public string email { get; set; }
        [JsonProperty("keywords")]
        public string keywords { get; set; }
        [JsonProperty("description")]
        public string description { get; set; }
        [JsonProperty("address_line_1")]
        public string address_line_1 { get; set; }
        [JsonProperty("address_line_2")]
        public string address_line_2 { get; set; }
        [JsonProperty("city")]
        public string city { get; set; }
        [JsonProperty("state")]
        public string state { get; set; }
        [JsonProperty("country")]
        public string country { get; set; }
        [JsonProperty("postal_code")]
        public string postal_code { get; set; }
        [JsonProperty("phone")]
        public string phone { get; set; }
        [JsonProperty("fax")]
        public string fax { get; set; }
        [JsonProperty("is_micro_store")]
        public bool is_micro_store { get; set; }
        [JsonProperty("parent_store_id")]
        public int parent_store_id { get; set; }
        [JsonProperty("company_name")]
        public string company_name { get; set; }
        [JsonProperty("billing_first_name")]
        public string billing_first_name { get; set; }
        [JsonProperty("billing_last_name")]
        public string billing_last_name { get; set; }
        [JsonProperty("tech_first_name")]
        public string tech_first_name { get; set; }
        [JsonProperty("tech_last_name")]
        public string tech_last_name { get; set; }
        [JsonProperty("tech_email")]
        public string tech_email { get; set; }
        [JsonProperty("tech_same_as_billing")]
        public bool tech_same_as_billing { get; set; }
        [JsonProperty("profile_id")]
        public int profile_id { get; set; }
    }

    public class StoresResponse
    {
        [JsonProperty("total_count")]
        public int total_count { get; set; }
        [JsonProperty("stores")]
        public List<Store> stores { get; set; }
    }
}
