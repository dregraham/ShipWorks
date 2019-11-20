using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    /// <summary>
    /// Shipping entity returned by Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenShipment
    {
        [JsonProperty("orderPackageId")]
        public string OrderPackageID { get; set; }

        [JsonProperty("shippingMethod")]
        public string ShippingMethod { get; set; }

        [JsonProperty("shippingStatus")]
        public string ShippingStatus { get; set; }

        [JsonProperty("shippingFee")]
        public decimal ShippingFee { get; set; }

        [JsonProperty("deliveryAddress")]
        public RakutenAddress DeliveryAddress { get; set; }

        [JsonProperty("invoiceAddress")]
        public RakutenAddress InvoiceAddress { get; set; }
    }

    /// <summary>
    /// Address entity returned by Rakuten
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class RakutenAddress
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("stateCode")]
        public string StateCode { get; set; }

        [JsonProperty("cityName")]
        public string CityName { get; set; }

        [JsonProperty("address1")]
        public string Address1 { get; set; }

        [JsonProperty("address2")]
        public string Address2 { get; set; }

    }
}