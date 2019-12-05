using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Overstock.Warehouse
{
    /// <summary>
    /// Overstock specific details
    /// </summary>
    [Obfuscation]
    public class OverstockWarehouseOrder
    {
        /// <summary>
        /// Name of the sales channel
        /// </summary>
        public string SalesChannelName { get; set; }

        /// <summary>
        /// Warehouse code
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// SOFS Created Date
        /// </summary>
        public DateTime SofsCreatedDate { get; set; }
    }
}
