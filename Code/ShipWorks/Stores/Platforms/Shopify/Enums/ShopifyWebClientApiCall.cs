using System.ComponentModel;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Shopify.Enums
{
    /// <summary>
    /// Shopify API calls we use
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum ShopifyWebClientApiCall
    {
        [Description("GetOrderCount")]
        GetOrderCount = 0,
        [Description("GetOrders")]
        GetOrders = 1,
        [Description("GetOrder")]
        GetOrder = 2,
        [Description("GetProduct")]
        GetProduct = 3,
        [Description("IsRealShopifyShopUrlName")]
        IsRealShopifyShopUrlName = 4,
        [Description("GetAccessToken")]
        GetAccessToken = 5,
        [Description("GetShop")]
        GetShop = 6,
        [Description("GetServerCurrentDateTime")]
        GetServerCurrentDateTime = 7,
        [Description("AddFulfillment")]
        AddFulfillment = 8,
        [Description("GetFraud")]
        GetFraud = 9
    }
}
