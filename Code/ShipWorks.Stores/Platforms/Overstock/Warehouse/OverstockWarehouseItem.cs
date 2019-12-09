using System.Reflection;

namespace ShipWorks.Stores.Platforms.Overstock.Warehouse
{
    /// <summary>
    /// Overstock specific details
    /// </summary>
    [Obfuscation]
    public class OverstockWarehouseItem
    {
        /// <summary>
        /// Sales channel line number from Overstock
        /// </summary>
        public long SalesChannelLineNumber { get; set; }
    }
}
