using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Groupon.DTO
{
    /// <summary>
    /// Order dto object that gets populated by the JsonConvert.DeserializeObject 
    /// </summary>
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


        public string city { get; set; }
        public string state { get; set; }
        public string name { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string phone { get; set; }


    }
}
