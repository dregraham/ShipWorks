using System;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.Responses
{
    public class ThreeDCartShipment
    {
        [JsonProperty("ShipmentID")]
        public int ShipmentID { get; set; }

        [JsonProperty("ShipmentLastUpdate")]
        public DateTime ShipmentLastUpdate { get; set; }

        [JsonProperty("ShipmentBoxes")]
        public int ShipmentBoxes { get; set; }

        [JsonProperty("ShipmentInternalComment")]
        public string ShipmentInternalComment { get; set; }

        [JsonProperty("ShipmentOrderStatus")]
        public int ShipmentOrderStatus { get; set; }

        [JsonProperty("ShipmentAddress")]
        public string ShipmentAddress { get; set; }

        [JsonProperty("ShipmentAddress2")]
        public string ShipmentAddress2 { get; set; }

        [JsonProperty("ShipmentAlias")]
        public string ShipmentAlias { get; set; }

        [JsonProperty("ShipmentCity")]
        public string ShipmentCity { get; set; }

        [JsonProperty("ShipmentCompany")]
        public string ShipmentCompany { get; set; }

        [JsonProperty("ShipmentCost")]
        public double ShipmentCost { get; set; }

        [JsonProperty("ShipmentCountry")]
        public string ShipmentCountry { get; set; }

        [JsonProperty("ShipmentEmail")]
        public string ShipmentEmail { get; set; }

        [JsonProperty("ShipmentFirstName")]
        public string ShipmentFirstName { get; set; }

        [JsonProperty("ShipmentLastName")]
        public string ShipmentLastName { get; set; }

        [JsonProperty("ShipmentMethodID")]
        public int ShipmentMethodID { get; set; }

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

        [JsonProperty("ShipmentTax")]
        public double ShipmentTax { get; set; }

        [JsonProperty("ShipmentWeight")]
        public double ShipmentWeight { get; set; }

        [JsonProperty("ShipmentTrackingCode")]
        public string ShipmentTrackingCode { get; set; }

        [JsonProperty("ShipmentUserID")]
        public string ShipmentUserID { get; set; }

        [JsonProperty("ShipmentNumber")]
        public int ShipmentNumber { get; set; }

        [JsonProperty("ShipmentAddressTypeID")]
        public int ShipmentAddressTypeID { get; set; }
    }
}