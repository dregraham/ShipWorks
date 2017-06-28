using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.SqlServer.Filters.DirtyCounts
{
    /// <summary>
    /// Represents a table that can be filtered on and its column tracked for changes
    /// </summary>
    public enum FilterNodeColumnMaskTable
    {
        Customer = 0,
        Order = 1,
        OrderItem = 2,
        OrderCharge = 3,
        Note = 4,
        Shipment = 5,
        PrintResult = 6,
        EmailOutbound = 7,
        EmailOutboundRelation = 8,
        OrderPaymentDetail = 9,
        DownloadDetail = 10,
        AmazonOrder = 11,
        ChannelAdvisorOrder = 12,
        ChannelAdvisorOrderItem = 13,
        EbayOrder = 14,
        EbayOrderItem = 15,
        MarketplaceAdvisorOrder = 16,
        OrderMotionOrder = 17,
        PayPalOrder = 18,
        ProStoresOrder = 19,
        PostalShipment = 20,
        UpsShipment = 21,
        FedExShipment = 22,
        CommerceInterfaceOrder = 23,
        ShopifyOrder = 24,
        EtsyOrder = 25,
        YahooOrder = 26,
        NeweggOrder = 27,
        BuyDotComOrderItem = 28,
        SearsOrder = 29,
        BigCommerceOrderItem = 30,
        InsurancePolicy = 31,
        GrouponOrder = 32,
        LemonStandOrder = 33,
        WalmartOrder = 34,
        WalmartOrderItem = 35,
        AmazonOrderSearch = 36,
        ChannelAdvisorOrderSearch = 37,
        ClickCartProOrderSearch = 38,
        CommerceInterfaceOrderSearch = 39,
        EbayOrderSearch = 40,
        EtsyOrderSearch = 41,
        GrouponOrderSearch = 42,
        LemonStandOrderSearch = 43,
        MagentoOrderSearch = 44,
        MarketplaceAdvisorOrderSearch = 45,
        NetworkSolutionsOrderSearch = 46,
        NeweggOrderSearch = 47,
        OrderMotionOrderSearch = 48,
        PayPalOrderSearch = 49,
        ProStoresOrderSearch = 50,
        SearsOrderSearch = 51,
        ShopifyOrderSearch = 52,
        ThreeDCartOrderSearch = 53,
        WalmartOrderSearch = 54,
        YahooOrderSearch = 55,
        OrderSearch = 56,
    }
}
