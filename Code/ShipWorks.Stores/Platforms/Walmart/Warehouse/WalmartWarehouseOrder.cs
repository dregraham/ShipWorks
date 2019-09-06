using System;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Walmart.Warehouse
{
    /// <summary>
    /// Walmart warehouse order
    /// </summary>
    [Obfuscation]
    public class WalmartWarehouseOrder
    {
        public string PurchaseOrderId { get; set; }
        public string CustomerOrderId { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        public DateTime EstimatedShipDate { get; set; }
        public string RequestedShippingMethodCode { get; set; }
    }
}
