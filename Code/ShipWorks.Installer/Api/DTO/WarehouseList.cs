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
        public SQLConfig SQLConfig { get; set; }
    }

    /// <summary>
    /// A warehouse's SQL configuration
    /// </summary>
    public class SQLConfig
    {
        public SQLSession SQLSession { get; set; }
    }

    /// <summary>
    /// The SQL Session information for a warehouse
    /// </summary>
    public class SQLSession
    {
        public Server Server { get; set; }

        public Credentials Credentials { get; set; }
    }

    /// <summary>
    /// Server information for a SQL Session
    /// </summary>
    public class Server
    {
        /// <summary>
        /// The instance name
        /// </summary>
        public string Instance { get; set; }

        /// <summary>
        /// The database name
        /// </summary>
        public string Database { get; set; }
    }

    /// <summary>
    /// Credentials for a SQL Session
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Whether or not to use Windows Auth
        /// </summary>
        public bool WindowsAuth { get; set; }
    }
}
