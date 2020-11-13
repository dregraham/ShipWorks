using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.AutoInstall.DTO
{
    /// <summary>
    /// Auto install shipworks config DTO
    /// </summary>
    [Obfuscation(StripAfterObfuscation = false)]
    public class AutoInstallShipWorksDto
    {
        /// <summary>
        /// The installation path
        /// </summary>
        public string InstallPath { get; set; }

        /// <summary>
        /// The db connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The tango email address to use
        /// </summary>
        public string TangoEmail { get; set; }

        /// <summary>
        /// The warehouse that we should pull config data from
        /// </summary>
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// If an installation error occurred, this is the message
        /// </summary>
        public string AutoInstallErrorMessage { get; set; }
    }

    /// <summary>
    /// A warehouse downloaded from the Hub
    /// </summary>
    [Obfuscation(StripAfterObfuscation = false)]
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
    [Obfuscation(StripAfterObfuscation = false)]
    public class Details
    {
        /// <summary>
        /// The zip
        /// </summary>
        [JsonProperty("zip")]
        public string Zip { get; set; }

        /// <summary>
        /// The city
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// The street
        /// </summary>
        [JsonProperty("street")]
        public string Street { get; set; }

        /// <summary>
        /// The customer tango id
        /// </summary>
        [JsonProperty("customerID")]
        public int CustomerId { get; set; }

        /// <summary>
        /// The name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The warehouse ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// The state
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// The database ID this warehouse is linked to
        /// </summary>
        [JsonProperty("shipWorksDatabaseId")]
        public string ShipWorksDatabaseId { get; set; }
    }
}
