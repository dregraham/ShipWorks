using System.Reflection;

namespace ShipWorks.Stores.Platforms.Amazon.DTO
{
    /// <summary>
    /// DTO for migrating an MWS store to an SP store
    /// </summary>
    [Obfuscation]
    public class MigrateMwsToSpResponse
    {
        public string OrderSourceId { get; set; }
    }
}
