using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class LemonStandBillingAddress
    {

    //    "data": {
    //    "id": 1,
    //    "first_name": "Chris",
    //    "last_name": "Hicks",
    //    "email": "c.hicks@shipworks.com",
    //    "notes": null,
    //    "is_guest": 1,
    //    "created_by": null,
    //    "updated_by": null,
    //    "created_at": "2015-09-02T14:43:34-0700",
    //    "updated_at": "2015-09-02T14:43:34-0700",
    //    "billing_addresses": {
    //        "data": [{
    //            "id": 1,
    //            "first_name": "Chris",
    //            "last_name": "Hicks",
    //            "phone": "(618)319-3658",
    //            "company": null,
    //            "street_address": "1648 N Main St",
    //            "city": "Dupo",
    //            "postal_code": "62239",
    //            "country": "United States",
    //            "country_code": "US",
    //            "state": "Illinois",
    //            "state_code": "IL",
    //            "is_default": 1,
    //            "is_billing": 1,
    //            "is_business": 0,
    //            "created_at": "2015-09-02T14:43:34-0700",
    //            "updated_at": "2015-09-02T14:43:34-0700"
    //        }]
    //    }
    //}
//}
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("street_address")]
        public string StreetAddress { get; set; }

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
