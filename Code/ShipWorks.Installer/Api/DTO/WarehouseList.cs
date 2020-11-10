using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Installer.Api.DTO
{
    /// <summary>
    /// List of Warehouses downloaded from the hub
    /// </summary>
    public class WarehouseList
    {
        /// <summary>
        /// The list of warehouses
        /// </summary>
        [JsonProperty("warehouses")]
        public List<Warehouse> warehouses { get; set; } = new List<Warehouse>();
    }

    /// <summary>
    /// A warehouse downloaded from the Hub
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// The warehouse ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Details about the warehouse
        /// </summary>
        [JsonProperty("details")]
        public Details Details { get; set; }
    }

    /// <summary>
    /// Details about a warehouse
    /// </summary>
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
        public string ID { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// The database ID this warehouse is linked to
        /// </summary>
        [JsonProperty("shipWorksDatabaseId")]
        public string ShipWorksDatabaseId { get; set; }

        /// <summary>
        /// The SQL configuration for this warehouse
        /// </summary>
        [JsonProperty("sqlConfig")]
        public string SQLConfig { get; set; }
    }
}
