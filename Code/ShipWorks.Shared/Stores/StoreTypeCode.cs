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
        [StoreTypeIcon("miva")]
        Miva = 0,

        [Description("eBay")]
        [StoreTypeIdentity("EBAY", "EQA")]
        [StoreTypeIcon("ebay")]
        Ebay = 1,

        [Description("Yahoo!")]
        [StoreTypeIdentity("YAHOO", "YQA")]
        [StoreTypeIcon("yahoo")]
        Yahoo = 2,

        [Description("ShopSite")]
        [StoreTypeIdentity("SHOPSITE", "SQA")]
        [StoreTypeIcon("shopsite")]
        ShopSite = 3,

        [Description("MarketplaceAdvisor")]
        [StoreTypeIdentity("MARKETWORKS", "MWK")]
        [StoreTypeIcon("channeladvisor")]
        MarketplaceAdvisor = 4,

        [Description("osCommerce")]
        [StoreTypeIdentity("OSCOMMERCE", "OSC")]
        [StoreTypeIcon("oscommerce")]
        osCommerce = 5,

        [Description("ProStores")]
        [StoreTypeIdentity("PROSTORES", "PRO")]
        [StoreTypeIcon("prostores")]
        ProStores = 6,

        [Description("ChannelAdvisor")]
        [StoreTypeIdentity("CHANNELADVISOR", "CHA")]
        [StoreTypeIcon("channeladvisor")]
        ChannelAdvisor = 7,

        [Description("Infopia")]
        [StoreTypeIdentity("INFOPIA", "INF")]
        [StoreTypeIcon("infopia")]
        Infopia = 8,

        [Description("CRE Loaded")]
        [StoreTypeIdentity("CRELOADED", "CRE")]
        [StoreTypeIcon("creloaded")]
        CreLoaded = 9,

        [Description("Amazon")]
        [StoreTypeIdentity("AMAZON", "AMA")]
        [StoreTypeIcon("amazon")]
        Amazon = 10,

        [Description("X-Cart")]
        [StoreTypeIdentity("XCART", "XCT")]
        [StoreTypeIcon("xcart")]
        XCart = 11,

        [Description("OrderMotion")]
        [StoreTypeIdentity("ORDERMOTION", "OMN")]
        [StoreTypeIcon("ordermotion")]
        OrderMotion = 12,

        [Description("Zen Cart")]
        [StoreTypeIdentity("ZENCART", "ZEN")]
        [StoreTypeIcon("zencart")]
        ZenCart = 13,

        // No longer supported in 3x
        // CartKeeper = 14,

        // No longer supported in 3x
        // EtailComplete = 15,

        [Description("VirtueMart")]
        [StoreTypeIdentity("VIRTUEMART", "VMT")]
        [StoreTypeIcon("virtuemart")]
        VirtueMart = 16,

        [Description("ClickCartPro")]
        [StoreTypeIdentity("CLICKCARTPRO", "CCP")]
        [StoreTypeIcon("clickcartpro")]
        ClickCartPro = 17,

        [Description("PayPal")]
        [StoreTypeIdentity("PAYPAL", "PAY")]
        [StoreTypeIcon("paypal")]
        PayPal = 18,

        [Description("Volusion")]
        [StoreTypeIdentity("VOLUSION", "VLN")]
        [StoreTypeIcon("volusion")]
        Volusion = 19,

        [Description("Network Solutions")]
        [StoreTypeIdentity("NETWORKSOLUTIONS", "NWS")]
        [StoreTypeIcon("networksolutions")]
        NetworkSolutions = 20,

        [Description("Magento")]
        [StoreTypeIdentity("MAGENTO", "MAG")]
        [StoreTypeIcon("magento")]
        Magento = 21,

        [Description("OrderDynamics")]
        [StoreTypeIdentity("ORDERDYNAMICS", "ODM")]
        [StoreTypeIcon("orderdynamics")]
        OrderDynamics = 22,

        [Description("SellerVantage")]
        [StoreTypeIdentity("AUCTIONSOUND", "ASN")]
        [StoreTypeIcon("sellervantage")]
        SellerVantage = 23,

        [Description("Web Shop Manager")]
        [StoreTypeIdentity("WDSOLUTIONS", "WDS")]
        [StoreTypeIcon("webshopmanager")]
        WebShopManager = 24,

        [Description("AmeriCommerce")]
        [StoreTypeIdentity("AMERICOMMERCE", "AMC")]
        [StoreTypeIcon("americommerce")]
        AmeriCommerce = 25,

        [Description("CommerceInterface")]
        [StoreTypeIdentity("COMMERCEINTERFACE", "CMI")]
        [StoreTypeIcon("commerceinterface")]
        CommerceInterface = 26,

        [Description("SearchFit")]
        [StoreTypeIdentity("SEARCHFIT", "SFT")]
        [StoreTypeIcon("searchfit")]
        SearchFit = 27,

        [Description("Generic - Module")]
        [StoreTypeIdentity("GENERIC", "GEN")]
        [StoreTypeIcon("genericmodule")]
        GenericModule = 28,

        [Description("3DCart")]
        [StoreTypeIdentity("3DCART", "3DC")]
        [StoreTypeIcon("three3dcart")]
        ThreeDCart = 29,

        [Description("BigCommerce")]
        [StoreTypeIdentity("BIGCOMMERCE", "BIG")]
        [StoreTypeIcon("bigcommerce")]
        BigCommerce = 30,

        [Description("Generic - File")]
        [StoreTypeIdentity("GENERICFILE", "GNF")]
        [StoreTypeIcon("genericfile")]
        GenericFile = 31,

        [Description("Shopify")]
        [StoreTypeIdentity("SHOPIFY", "SPY")]
        [StoreTypeIcon("shopify")]
        Shopify = 32,

        [Description("Etsy")]
        [StoreTypeIdentity("ETSY", "ETY")]
        [StoreTypeIcon("etsy")]
        Etsy = 33,

        [Description("Newegg")]
        [StoreTypeIdentity("NEWEGG", "EGG")]
        [StoreTypeIcon("newegg")]
        NeweggMarketplace = 34,

        [Description("Buy.com")]
        [StoreTypeIdentity("BUYDOTCOM", "BUY")]
        [StoreTypeIcon("rakuten")]
        BuyDotCom = 35,

        [Description("Sears")]
        [StoreTypeIdentity("SEARS", "SRS")]
        [StoreTypeIcon("sears")]
        Sears = 36,

        [Description("SolidCommerce")]
        [StoreTypeIdentity("SOLIDCOMMERCE", "SDC")]
        [StoreTypeIcon("solidcommerce")]
        SolidCommerce = 37,

        [Description("BrightPearl")]
        [StoreTypeIdentity("BRIGHTPEARL", "BTP")]
        [StoreTypeIcon("brightpearl")]
        BrightPearl = 38
    }
}
