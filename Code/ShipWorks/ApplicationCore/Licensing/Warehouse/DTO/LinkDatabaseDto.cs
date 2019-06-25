using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    /// <summary>
    /// DTO for linking a database to a warehouse
    /// </summary>
    [Obfuscation]
    public class LinkDatabaseDto
    {
        [JsonProperty("databaseId")]
        public string DatabaseId { get; set; }
    }
}
