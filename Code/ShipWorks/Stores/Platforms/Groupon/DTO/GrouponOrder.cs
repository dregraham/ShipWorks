﻿using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Groupon.DTO
{
    /// <summary>
    /// Order dto object that gets populated by the JsonConvert.DeserializeObject 
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class GrouponOrder
    {
    //    "data": [{
    //    "orderid": "25769-UE-896098",
    //    "customer": {
    //        "city": "CHICAGO",
    //        "state": "IL",
    //        "name": "Joshua Ulanski",
    //        "zip": "60655",
    //        "country": "USA",
    //        "address1": "10326 S SPAULDING AVE",
    //        "address2": "",
    //        "phone": ""
    //    },
    //    "date": "02/18/2015 09:36PM UTC",
    //    "amount": {
    //        "total": 24.99,
    //        "shipping": 0
    //    },
    //    "supplier": "AZ SHOPPING"
    //}

        [JsonProperty("orderid")]
        public string OrderId { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

    }
}
