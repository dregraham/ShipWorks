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
        GrouponOrder = 32
    }
}
