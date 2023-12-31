﻿using System.Reflection;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    [SuppressMessage("SonarQube", "S125:Sections of code should not be \"commented out\"",
        Justification = "Commented out code shows an example of the json that is returned from the api")]
    public class LemonStandShipment
    {
        //    "data": {
        //    "id": 1,
        //    "shop_shipment_id": null,
        //    "shop_shipment_status_id": 1,
        //    "shop_shipping_method_id": 2,
        //    "number": 1,
        //    "shipping_service": "First Class",
        //    "shipping_quote": 2.04,
        //    "discount": null,
        //    "notes": null,
        //    "created_by": null,
        //    "updated_by": null,
        //    "created_at": "2015-09-02T14:43:34-0700",
        //    "updated_at": "2015-09-02T14:43:36-0700",
        //    "shipping_address": {
        //        "data": {
        //            "id": 4,
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
        //            "is_default": 0,
        //            "is_billing": 0,
        //            "is_business": 0,
        //            "created_at": "2015-09-02T14:43:34-0700",
        //            "updated_at": "2015-09-02T14:43:34-0700"
        //        }
        //    }
        //}

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("shipping_service")]
        public string ShippingService { get; set; }
    }
}