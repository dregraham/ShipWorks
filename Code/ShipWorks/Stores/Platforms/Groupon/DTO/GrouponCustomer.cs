using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Groupon.DTO
{
    /// <summary>
    /// Order dto object that gets populated by the JsonConvert.DeserializeObject 
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class GrouponCustomer
    {
  
    //    "customer": {
    //        "city": "CHICAGO",
    //        "state": "IL",
    //        "name": "Joshua Ulanski",
    //        "zip": "60655",
    //        "country": "USA",
    //        "address1": "10326 S SPAULDING AVE",
    //        "address2": "",
    //        "phone": ""
    //    }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
