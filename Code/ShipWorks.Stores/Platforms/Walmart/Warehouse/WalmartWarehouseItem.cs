using System.Reflection;

namespace ShipWorks.Stores.Platforms.Walmart.Warehouse
{
    /// <summary>
    /// Walmart warehouse order item
    /// </summary>
    [Obfuscation]
    public class WalmartWarehouseItem
    {
        public string LineNumber { get; set; }
        public string OnlineStatus { get; set; }
    }
}
