using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Warehouse.Configuration.TaxIdentifiers.DTO
{
    [Obfuscation]
    public class TaxIdentifierConfiguration
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("authority")]
        public string Authority { get; set; }
    }
}
