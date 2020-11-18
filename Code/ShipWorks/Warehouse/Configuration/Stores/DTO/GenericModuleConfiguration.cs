using Newtonsoft.Json;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    /// <summary>
    /// Configuration for a Generic Module store
    /// </summary>
    public class GenericModuleConfiguration
    {
        public string Username { get; set; }

        public string Password { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        public string OnlineStoreCode { get; set; }

        public long? ImportStartDetails { get; set; }
    }
}
