using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.BigCommerce.Enums
{
    /// <summary>
    /// BigCommerce API calls we use
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum BigCommerceWebClientApiCall
    {
        // Get API calls
        [Description("GetOrderCount")]
        GetOrderCount = 0,
        [Description("GetOrders")]
        GetOrders = 1,
        [Description("GetOrder")]
        GetOrder = 2,
        [Description("GetOrderStatuses")]
        GetOrderStatuses = 3,
        [Description("GetProducts")]
        GetProducts = 4,
        [Description("GetProduct")]
        GetProduct = 5,
        [Description("GetShipments")]
        GetShipments = 6,
        [Description("GetShippingAddress")]
        GetShippingAddress = 7,
        [Description("GetCoupons")]
        GetCoupons = 8,
        [Description("CreateShipment")]
        CreateShipment = 9,
        [Description("UpdateOrderStatus")]
        UpdateOrderStatus = 10,
        [Description("GetOrderProducts")]
        GetOrderProducts = 11
    }
}
