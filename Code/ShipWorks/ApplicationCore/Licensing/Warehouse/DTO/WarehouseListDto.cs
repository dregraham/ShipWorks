using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class WarehouseListDto
    {
        [JsonProperty("warehouses")]
        public List<Warehouse> warehouses { get; set; } = new List<Warehouse>();

        [JsonProperty("count")]
        public int count { get; set; }

        [JsonProperty("scannedCount")]
        public int scannedCount { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Warehouse
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("details")]
        public Details details { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Details
    {
        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("customerID")]
        public int CustomerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("shipWorksDatabaseId")]
        public string ShipWorksDatabaseId { get; set; }
    }
}
