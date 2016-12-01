using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ShippingAddress : IShippingAddress
    {
        [JsonProperty("addressType")]
        public string AddressType { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("countryId")]
        public string CountryId { get; set; }

        [JsonProperty("customerAddressId")]
        public int CustomerAddressId { get; set; }

        [JsonProperty("customerId")]
        public int CustomerId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("entityId")]
        public int EntityId { get; set; }

        [JsonProperty("fax")]
        public string Fax { get; set; }

        [JsonProperty("firstname")]
        public string Firstname { get; set; }

        [JsonProperty("lastname")]
        public string Lastname { get; set; }

        [JsonProperty("middlename")]
        public string Middlename { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("postcode")]
        public string Postcode { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("regionCode")]
        public string RegionCode { get; set; }

        [JsonProperty("regionId")]
        public int RegionId { get; set; }

        [JsonProperty("street")]
        public IList<string> Street { get; set; }

        [JsonProperty("suffix")]
        public string Suffix { get; set; }

        [JsonProperty("telephone")]
        public string Telephone { get; set; }

        [JsonProperty("vatId")]
        public string VatId { get; set; }

        [JsonProperty("vatIsValid")]
        public int VatIsValid { get; set; }

        [JsonProperty("vatRequestDate")]
        public string VatRequestDate { get; set; }

        [JsonProperty("vatRequestId")]
        public string VatRequestId { get; set; }

        [JsonProperty("vatRequestSuccess")]
        public int VatRequestSuccess { get; set; }

        [JsonProperty("extensionAttributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}