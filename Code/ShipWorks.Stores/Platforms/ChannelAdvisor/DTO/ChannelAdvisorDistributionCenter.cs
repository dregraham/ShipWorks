using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ChannelAdvisor.DTO
{
    public class ChannelAdvisorDistributionCenter
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("ContactName")]
        public string ContactName { get; set; }

        [JsonProperty("ContactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty("ContactPhone")]
        public string ContactPhone { get; set; }

        [JsonProperty("Address1")]
        public string Address1 { get; set; }

        [JsonProperty("Address2")]
        public string Address2 { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("StateOrProvince")]
        public string StateOrProvince { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("PickupLocation")]
        public bool PickupLocation { get; set; }

        [JsonProperty("ShipLocation")]
        public bool ShipLocation { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("IsExternallyManaged")]
        public bool IsExternallyManaged { get; set; }

        [JsonProperty("IsDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("DeletedDateUtc")]
        public object DeletedDateUtc { get; set; }
    }
}