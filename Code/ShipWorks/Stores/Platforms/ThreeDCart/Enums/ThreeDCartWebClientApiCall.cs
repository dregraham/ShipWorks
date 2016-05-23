using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.ThreeDCart.Enums
{
    /// <summary>
    /// 3dcart API calls we use
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ThreeDCartWebClientApiCall
    {
        // Get API calls
        [Description("CreateFulfillment")]
        CreateFulfillment = 0,

        [Description("UpdateOrderStatus")]
        UpdateOrderStatus = 1,

        [Description("GetOrder")]
        GetOrder = 2,

        [Description("GetOrders")]
        GetOrders = 3,

        [Description("GetOrderCount")]
        GetOrderCount = 4,

        [Description("GetOrderStatuses")]
        GetOrderStatuses = 5,

        [Description("GetProducts")]
        GetProducts = 6,

        [Description("GetProduct")]
        GetProduct = 7,
        
        [Description("DetermineDbVersionSqlServer")]
        DetermineDbVersionSqlServer = 8,

        [Description("DetermineDbVersionMsAccess")]
        DetermineDbVersionMsAccess = 9,

        [Description("TestConnection")]
        TestConnection = 10
    }
}
