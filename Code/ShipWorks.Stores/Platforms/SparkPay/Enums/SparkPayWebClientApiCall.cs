using Interapptive.Shared.Utility;
using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.SparkPay.Enums
{

    /// <summary>
    /// SparkPay API calls we use
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum SparkPayWebClientApiCall
    {
        [Description("GetOrders")]
        [ApiValue("orders")]
        GetOrders = 0,

        [Description("GetStatuses")]
        [ApiValue("order_statuses")]
        GetStatuses = 1,

        [Description("GetAddress")]
        [ApiValue("addresses")]
        GetAddresses = 2,

        [Description("AddShipment")]
        [ApiValue("order_shipments")]
        AddShipment = 3,

        [Description("UpdateStatus")]
        [ApiValue("orders/{0}/status")]
        UpdateOrderStatus = 4,

        [Description("GetStores")]
        [ApiValue("stores")]
        GetStores = 5
    }
    
}
