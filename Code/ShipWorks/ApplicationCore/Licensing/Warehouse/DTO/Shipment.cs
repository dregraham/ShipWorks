using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class RecipientAddress
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("street1")]
        public string Street1 { get; set; }

        [JsonProperty("street2")]
        public string Street2 { get; set; }

        [JsonProperty("street3")]
        public string Street3 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("stateCode")]
        public string StateCode { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Package
    {
        [JsonProperty("weightInPounds")]
        public double WeightInPounds { get; set; }

        [JsonProperty("lengthInInches")]
        public double LengthInInches { get; set; }

        [JsonProperty("widthInInches")]
        public double WidthInInches { get; set; }

        [JsonProperty("heightInInches")]
        public double HeightInInches { get; set; }

        [JsonProperty("packagingType")]
        public string PackagingType { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Shipment
    {
        [JsonProperty("tangoShipmentId")]
        public long TangoShipmentId { get; set; }

        [JsonProperty("shipworksShipmentId")]
        public long ShipworksShipmentId { get; set; }

        [JsonProperty("carrier")]
        public string Carrier { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("trackingNumber")]
        public string TrackingNumber { get; set; }

        [JsonProperty("shipDate")]
        public DateTime ShipDate { get; set; }

        [JsonProperty("carrierCost")]
        public decimal CarrierCost { get; set; }

        [JsonProperty("shipworksInsured")]
        public int ShipworksInsured { get; set; }

        [JsonProperty("carrierInsured")]
        public int CarrierInsured { get; set; }

        [JsonProperty("isReturn")]
        public int IsReturn { get; set; }

        [JsonProperty("recipientAddress")]
        public RecipientAddress RecipientAddress { get; set; }

        [JsonProperty("originPostalCode")]
        public string OriginPostalCode { get; set; }

        [JsonProperty("originCountryCode")]
        public string OriginCountryCode { get; set; }

        [JsonProperty("shippingAccount")]
        public string ShippingAccount { get; set; }

        [JsonProperty("labelFormat")]
        public string LabelFormat { get; set; }

        [JsonProperty("estimatedDeliveryDate")]
        public DateTime EstimatedDeliveryDate { get; set; }

        [JsonProperty("packages")]
        public IEnumerable<Package> Packages { get; set; }
    }
}
