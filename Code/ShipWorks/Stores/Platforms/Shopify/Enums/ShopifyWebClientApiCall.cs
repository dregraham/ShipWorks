using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

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
        AddFulfillment = 8
    }
}
