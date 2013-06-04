using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace ShipWorks.Stores
{
    /// <summary>
    /// All possible store types in ShipWorks
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum StoreTypeCode
    {
        [Description("Invalid")]
        Invalid = -1,

        [Description("Miva Merchant")]
        [StoreTypeIdentity("MIVA", "MQA")]
        Miva = 0,

        [Description("eBay")]
        [StoreTypeIdentity("EBAY", "EQA")]
        Ebay = 1,

        [Description("Yahoo!")]
        [StoreTypeIdentity("YAHOO", "YQA")]
        Yahoo = 2,

        [Description("ShopSite")]
        [StoreTypeIdentity("SHOPSITE", "SQA")]
        ShopSite = 3,

        [Description("MarketplaceAdvisor")]
        [StoreTypeIdentity("MARKETWORKS", "MWK")]
        MarketplaceAdvisor = 4,

        [Description("osCommerce")]
        [StoreTypeIdentity("OSCOMMERCE", "OSC")]
        osCommerce = 5,

        [Description("ProStores")]
        [StoreTypeIdentity("PROSTORES", "PRO")]
        ProStores = 6,

        [Description("ChannelAdvisor")]
        [StoreTypeIdentity("CHANNELADVISOR", "CHA")]
        ChannelAdvisor = 7,

        [Description("Infopia")]
        [StoreTypeIdentity("INFOPIA", "INF")]
        Infopia = 8,

        [Description("CRE Loaded")]
        [StoreTypeIdentity("CRELOADED", "CRE")]
        CreLoaded = 9,

        [Description("Amazon")]
        [StoreTypeIdentity("AMAZON", "AMA")]
        Amazon = 10,

        [Description("X-Cart")]
        [StoreTypeIdentity("XCART", "XCT")]
        XCart = 11,

        [Description("OrderMotion")]
        [StoreTypeIdentity("ORDERMOTION", "OMN")]
        OrderMotion = 12,

        [Description("Zen Cart")]
        [StoreTypeIdentity("ZENCART", "ZEN")]
        ZenCart = 13,

        // No longer supported in 3x
        // CartKeeper = 14,

        // No longer supported in 3x
        // EtailComplete = 15,

        [Description("VirtueMart")]
        [StoreTypeIdentity("VIRTUEMART", "VMT")]
        VirtueMart = 16,

        [Description("ClickCartPro")]
        [StoreTypeIdentity("CLICKCARTPRO", "CCP")]
        ClickCartPro = 17,

        [Description("PayPal")]
        [StoreTypeIdentity("PAYPAL", "PAY")]
        PayPal = 18,

        [Description("Volusion")]
        [StoreTypeIdentity("VOLUSION", "VLN")]
        Volusion = 19,

        [Description("Network Solutions")]
        [StoreTypeIdentity("NETWORKSOLUTIONS", "NWS")]
        NetworkSolutions = 20,

        [Description("Magento")]
        [StoreTypeIdentity("MAGENTO", "MAG")]
        Magento = 21,

        [Description("OrderDynamics")]
        [StoreTypeIdentity("ORDERDYNAMICS", "ODM")]
        OrderDynamics = 22,

        [Description("SellerVantage")]
        [StoreTypeIdentity("AUCTIONSOUND", "ASN")]
        AuctionSound = 23,

        [Description("Web Shop Manager")]
        [StoreTypeIdentity("WDSOLUTIONS", "WDS")]
        WebShopManager = 24,

        [Description("AmeriCommerce")]
        [StoreTypeIdentity("AMERICOMMERCE", "AMC")]
        AmeriCommerce = 25,

        [Description("CommerceInterface")]
        [StoreTypeIdentity("COMMERCEINTERFACE", "CMI")]
        CommerceInterface = 26,

        [Description("SearchFit")]
        [StoreTypeIdentity("SEARCHFIT", "SFT")]
        SearchFit = 27,

        [Description("Generic - Module")]
        [StoreTypeIdentity("GENERIC", "GEN")]
        GenericModule = 28,

        [Description("3DCart")]
        [StoreTypeIdentity("3DCART", "3DC")]
        ThreeDCart = 29,

        [Description("BigCommerce")]
        [StoreTypeIdentity("BIGCOMMERCE", "BIG")]
        BigCommerce = 30,

        [Description("Generic - File")]
        [StoreTypeIdentity("GENERICFILE", "GNF")]
        GenericFile = 31,

        [Description("Shopify")]
        [StoreTypeIdentity("SHOPIFY", "SPY")]
        Shopify = 32,

        [Description("Etsy")]
        [StoreTypeIdentity("ETSY", "ETY")]
        Etsy = 33,

        [Description("Newegg")]
        [StoreTypeIdentity("NEWEGG", "EGG")]
        NeweggMarketplace = 34,

        [Description("Buy.com")]
        [StoreTypeIdentity("BUYDOTCOM", "BUY")]
        BuyDotCom = 35,

        [Description("Sears")]
        [StoreTypeIdentity("SEARS", "SRS")]
        Sears = 36
    }
}
