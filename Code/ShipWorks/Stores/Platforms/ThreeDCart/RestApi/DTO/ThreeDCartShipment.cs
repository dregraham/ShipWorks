﻿using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class ThreeDCartShipment
    {
        public long OrderID { get; set; }

        [JsonProperty("ShipmentID")]
        public long ShipmentID { get; set; }

        [JsonProperty("ShipmentLastUpdate")]
        public string ShipmentLastUpdate { get; set; }

        [JsonProperty("ShipmentOrderStatus")]
        public int ShipmentOrderStatus { get; set; }

        [JsonProperty("ShipmentAddress")]
        public string ShipmentAddress { get; set; }

        [JsonProperty("ShipmentAddress2")]
        public string ShipmentAddress2 { get; set; }

        [JsonProperty("ShipmentCity")]
        public string ShipmentCity { get; set; }

        [JsonProperty("ShipmentCompany")]
        public string ShipmentCompany { get; set; }

        [JsonProperty("ShipmentCost")]
        public decimal ShipmentCost { get;  set; }

        [JsonProperty("ShipmentCountry")]
        public string ShipmentCountry { get; set; }

        [JsonProperty("ShipmentEmail")]
        public string ShipmentEmail { get; set; }

        [JsonProperty("ShipmentFirstName")]
        public string ShipmentFirstName { get; set; }

        [JsonProperty("ShipmentLastName")]
        public string ShipmentLastName { get; set; }

        [JsonProperty("ShipmentMethodName")]
        public string ShipmentMethodName { get; set; }

        [JsonProperty("ShipmentShippedDate")]
        public string ShipmentShippedDate { get; set; }

        [JsonProperty("ShipmentPhone")]
        public string ShipmentPhone { get; set; }

        [JsonProperty("ShipmentState")]
        public string ShipmentState { get; set; }

        [JsonProperty("ShipmentZipCode")]
        public string ShipmentZipCode { get; set; }

        [JsonProperty("ShipmentWeight")]
        public double ShipmentWeight { get; set; }

        [JsonProperty("ShipmentTrackingCode")]
        public string ShipmentTrackingCode { get; set; }

        public bool ShouldSerializeShipmentCost()
        {
            return false;
        }

       public bool ShouldSerializeShipmentWeight()
       {
           return ShipmentWeight > 0;
       }
    }
}