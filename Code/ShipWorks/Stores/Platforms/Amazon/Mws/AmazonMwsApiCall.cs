using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// MWS API calls we use
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonMwsApiCall
    {
        // Orders API
        ListOrders = 0,
        ListOrderItems = 1,
        GetServiceStatus = 2,
        ListOrdersByNextToken = 3,
        ListOrderItemsByNextToken = 4,
        SubmitFeed = 5,
        ListMarketplaceParticipations = 6,

        // Product API
        GetMatchingProductForId = 7,

        GetAuthToken = 8,

        // Fulfilment Api
        GetEligibleShippingServices = 9,
    }
}
