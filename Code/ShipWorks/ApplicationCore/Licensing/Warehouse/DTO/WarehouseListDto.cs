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
        public string zip { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("city")]
        public string city { get; set; }

        [JsonProperty("street")]
        public string street { get; set; }

        [JsonProperty("customerID")]
        public int customerID { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("state")]
        public string state { get; set; }

        [JsonProperty("shipWorksLink")]
        public string shipWorksLink { get; set; }
    }
}
