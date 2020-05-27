using System.Reflection;

namespace ShipWorks.Products.Warehouse.DTO
{
    /// <summary>
    /// Response data from changing a product on the hub
    /// </summary>
    [Obfuscation]
    public class ChangeProductResponseData
    {
        /// <summary>
        /// Version of the product on the Hub
        /// </summary>
        /// <remarks>
        /// This is used for optimistic concurrency
        /// </remarks>
        public int Version { get; set; }
    }
}
