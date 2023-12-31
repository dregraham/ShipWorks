using System.ComponentModel;
using System.Reflection;
using Interapptive.Shared.Utility;

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

        [Description("Brightpearl")]
        [StoreTypeIdentity("BRIGHTPEARL", "BTP")]
        [StoreTypeIcon("brightpearl")]
        [Deprecated]
        Brightpearl = 38,

        [Description("OrderDesk")]
        [StoreTypeIdentity("ORDERDESK", "ORD")]
        [StoreTypeIcon("orderdesk")]
        OrderDesk = 39,

        [Description("Cart66 Lite")]
        [StoreTypeIdentity("CART66LITE", "C6L")]
        [StoreTypeIcon("cart66")]
        Cart66Lite = 40,

        [Description("Cart66 Pro")]
        [StoreTypeIdentity("CART66PRO", "C6P")]
        [StoreTypeIcon("cart66")]
        Cart66Pro = 41,

        [Description("Shopp")]
        [StoreTypeIdentity("SHOPP", "SPP")]
        [StoreTypeIcon("shopp")]
        Shopp = 42,

        [Description("Shopperpress")]
        [StoreTypeIdentity("SHOPPERPRESS", "SHP")]
        [StoreTypeIcon("shopperpress")]
        Shopperpress = 43,

        [Description("WP eCommerce")]
        [StoreTypeIdentity("WPECOMMERCE", "WPE")]
        [StoreTypeIcon("wpecommerce")]
        WPeCommerce = 44,

        [Description("Jigoshop")]
        [StoreTypeIdentity("JIGOSHOP", "JIG")]
        [StoreTypeIcon("jigoshop")]
        Jigoshop = 45,

        [Description("WooCommerce")]
        [StoreTypeIdentity("WOOCOMMERCE", "WOO")]
        [StoreTypeIcon("woocommerce")]
        WooCommerce = 46,

        [Description("ChannelSale")]
        [StoreTypeIdentity("CHANNELSALE", "CHS")]
        [StoreTypeIcon("channelsale")]
        ChannelSale = 47,

        [Description("Fortune3")]
        [StoreTypeIdentity("FORTUNE3", "FT3")]
        [StoreTypeIcon("fortune3")]
        Fortune3 = 48,

        [Description("LiveSite")]
        [StoreTypeIdentity("LIVESITE", "LST")]
        [StoreTypeIcon("livesite")]
        LiveSite = 49,

        [Description("SureDone")]
        [StoreTypeIdentity("SUREDONE", "SDN")]
        [StoreTypeIcon("suredone")]
        SureDone = 50,

        [Description("Zenventory")]
        [StoreTypeIdentity("ZENVENTORY", "ZVN")]
        [StoreTypeIcon("zenventory")]
        Zenventory = 51,

        [Description("nopCommerce")]
        [StoreTypeIdentity("NOPCOMMERCE", "NOP")]
        [StoreTypeIcon("nopcommerce")]
        nopCommerce = 52,

        [Description("Lime Light CRM")]
        [StoreTypeIdentity("LIMELIGHTCRM", "LLC")]
        [StoreTypeIcon("limelightcrm")]
        LimeLightCRM = 53,

        [Description("OpenCart")]
        [StoreTypeIdentity("OPENCART", "OCT")]
        [StoreTypeIcon("opencart")]
        OpenCart = 54,

        [Description("SellerExpress")]
        [StoreTypeIdentity("SELLEREXPRESS", "SEE")]
        [StoreTypeIcon("sellerexpress")]
        SellerExpress = 55,

        [Description("Powersport Support")]
        [StoreTypeIdentity("POWERSPORTSSUPPORT", "PSS")]
        [StoreTypeIcon("powersportssupport")]
        PowersportsSupport = 56,

        [Description("Cloud Conversion")]
        [StoreTypeIdentity("CLOUDCONVERSION", "CLC")]
        [StoreTypeIcon("cloudconversion")]
        CloudConversion = 57,

        [Description("CS-Cart")]
        [StoreTypeIdentity("CSCART", "CSC")]
        [StoreTypeIcon("cscart")]
        CsCart = 58,

        [Description("PrestaShop")]
        [StoreTypeIdentity("PRESTA", "PRS")]
        [StoreTypeIcon("prestashop")]
        PrestaShop = 59,

        [Description("Loaded Commerce")]
        [StoreTypeIdentity("LOADEDCOMMERCE", "LDC")]
        [StoreTypeIcon("loadedcommerce")]
        LoadedCommerce = 60,

        [Description("Choxi")]
        [StoreTypeIdentity("NOMORERACK", "NMR")]
        [StoreTypeIcon("choxi")]
        Choxi = 61,

        [Description("Groupon")]
        [StoreTypeIdentity("GROUPON", "GON")]
        [StoreTypeIcon("groupon")]
        Groupon = 62,

        [Description("StageBloc")]
        [StoreTypeIdentity("STAGEBLOC", "STG")]
        [StoreTypeIcon("stagebloc")]
        StageBloc = 63,

        [Description("RevolutionParts")]
        [StoreTypeIdentity("REVOLUTIONPARTS", "REV")]
        [StoreTypeIcon("revolutionparts")]
        RevolutionParts = 64,

        [Description("InstanteStore")]
        [StoreTypeIdentity("INSTASTORE", "IST")]
        [StoreTypeIcon("instantestore")]
        InstaStore = 65,

        [Description("OrderBot")]
        [StoreTypeIdentity("ORDERBOT", "ODB")]
        [StoreTypeIcon("orderbot")]
        OrderBot = 66,

        [Description("OpenSky")]
        [StoreTypeIdentity("OPENSKY", "SKY")]
        [StoreTypeIcon("opensky")]
        OpenSky = 67,

        [Description("LemonStand")]
        [StoreTypeIdentity("LEMONSTAND", "LEM")]
        [StoreTypeIcon("lemonstand")]
        LemonStand = 68,

        [Description("SparkPay")]
        [StoreTypeIdentity("SPARKPAY", "SPK")]
        [StoreTypeIcon("sparkpay")]
        SparkPay = 69,

        [Description("ODBC")]
        [StoreTypeIdentity("ODBC", "DBC")]
        [StoreTypeIcon("odbc")]
        Odbc = 70,

        [Description("Amosoft")]
        [StoreTypeIdentity("AMOSOFT", "AMO")]
        [StoreTypeIcon("amosoft")]
        Amosoft = 71,

        [Description("SellerCloud")]
        [StoreTypeIdentity("SELLERCLOUD", "SCD")]
        [StoreTypeIcon("sellercloud")]
        SellerCloud = 73,

        [Description("InfiPlex")]
        [StoreTypeIdentity("INFIPLEX", "IPX")]
        [StoreTypeIcon("infiplex")]
        InfiPlex = 74,

        [Description("Walmart")]
        [StoreTypeIdentity("WALMART", "WAL")]
        [StoreTypeIcon("walmart")]
        Walmart = 75,

        [Description("SellerActive")]
        [StoreTypeIdentity("SELLERACTIVE", "SEA")]
        [StoreTypeIcon("selleractive")]
        SellerActive = 76,

        [Description("GeekSeller")]
        [StoreTypeIdentity("GEEKSELLER", "GEE")]
        [StoreTypeIcon("geekseller")]
        GeekSeller = 78,

        [Description("Jet")]
        [StoreTypeIdentity("JET", "JET")]
        [StoreTypeIcon("jet")]
        Jet = 79,

        [Description("Manual")]
        [StoreTypeIdentity("MANUAL", "MAN")]
        [StoreTypeIcon("shipworks")]
        Manual = 80,

        [Description("Overstock")]
        [StoreTypeIdentity("OVERSTOCK", "OVS")]
        [StoreTypeIcon("overstock")]
        Overstock = 82,

        [Description("CommerceV3")]
        [StoreTypeIdentity("CV3", "CV3")]
        [StoreTypeIcon("cv3")]
        CommerceV3 = 83,

        [Description("Zentail")]
        [StoreTypeIdentity("ZENTAIL", "ZTL")]
        [StoreTypeIcon("zentail")]
        Zentail = 84,

        [Description("Bonanza")]
        [StoreTypeIdentity("BONANZA", "BON")]
        [StoreTypeIcon("bonanza")]
        Bonanza = 85,

        [Description("Rakuten")]
        [StoreTypeIdentity("RAKUTEN", "RAK")]
        [StoreTypeIcon("rakuten")]
        Rakuten = 86,

        [Description("API")]
        [StoreTypeIdentity("API", "API")]
        [StoreTypeIcon("api")]
        Api = 87,
        
        [Description("Brightpearl")]
        [StoreTypeIdentity("BRIGHTPEARLHUB", "BPH")]
        [StoreTypeIcon("brightpearl")]
        BrightpearlHub = 88,

        [Description("Walmart")]
        [StoreTypeIdentity("WALMARTHUB", "WMH")]
        [StoreTypeIcon("walmart")]
        WalmartHub = 89,
        
        [Description("ChannelAdvisor")]
        [StoreTypeIdentity("CHANNELADVISORHUB", "CAH")]
        [StoreTypeIcon("channeladvisor")]
        ChannelAdvisorHub = 90,
        
        [Description("Volusion")]
        [StoreTypeIdentity("VOLUSIONHUB", "VLH")]
        [StoreTypeIcon("volusion")]
        VolusionHub = 91,
        
        [Description("Groupon")]
        [StoreTypeIdentity("GROUPONHUB", "GRH")]
        [StoreTypeIcon("groupon")]
        GrouponHub = 92,
    }
}