using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.BigCommerce.Warehouse
{
    /// <summary>
    /// BigCommerce warehouse item
    /// </summary>
    [Obfuscation]
    public class BigCommerceWarehouseItem
    {
        /// <summary>
        /// The OrderAddressID of the BigCommerceOrderItem
        /// </summary>
        public long OrderAddressID { get; set; }

        /// <summary>
        /// The OrderProductID of the BigCommerceOrderItem
        /// </summary>
        public long OrderProductID { get; set; }

        /// <summary>
        /// The ParentOrderProductID of the BigCommerceOrderItem
        /// </summary>
        public long ParentOrderProductID { get; set; }

        /// <summary>
        /// The IsDigitalItem of the BigCommerceOrderItem
        /// </summary>
        public bool IsDigitalItem { get; set; }

        /// <summary>
        /// The EventDate of the BigCommerceOrderItem
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// The EventName of the BigCommerceOrderItem
        /// </summary>
        public string EventName { get; set; }
    }
}
