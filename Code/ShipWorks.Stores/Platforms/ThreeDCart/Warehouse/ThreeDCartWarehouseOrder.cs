using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Warehouse
{
    /// <summary>
    /// 3D Cart specific details
    /// </summary>
    [Obfuscation]
    public class ThreeDCartWarehouseOrder
    {
        /// <summary>
        /// Order ID from 3D Cart
        /// </summary>
        public long ThreeDCartOrderID { get; set; }
    }
}
