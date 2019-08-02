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
        /// The OrderAddressId of the BigCommerceOrderItem
        /// </summary>
        public long OrderAddressId { get; set; }

        /// <summary>
        /// The OrderProductId of the BigCommerceOrderItem
        /// </summary>
        public long OrderProductId { get; set; }

        /// <summary>
        /// The ParentOrderProductId of the BigCommerceOrderItem
        /// </summary>
        public long ParentOrderProductId { get; set; }

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
