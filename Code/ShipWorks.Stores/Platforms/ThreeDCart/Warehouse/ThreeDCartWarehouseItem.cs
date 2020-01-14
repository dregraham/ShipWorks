using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Warehouse
{
    /// <summary>
    /// 3D Cart specific details
    /// </summary>
    [Obfuscation]
    public class ThreeDCartWarehouseItem
    {
        /// <summary>
        /// Shipment ID from 3D Cart
        /// </summary>
        public long ThreeDCartShipmentId { get; set; }
    }
}
