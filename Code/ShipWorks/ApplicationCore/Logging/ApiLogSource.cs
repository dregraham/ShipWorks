using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Sources of API logging
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralFields", Exclude = false, StripAfterObfuscation = false)]
    public enum ApiLogSource
    {
        ChannelAdvisor,
        CreLoaded,
        eBay,
        Infopia,
        MarketplaceAdvisor,
        Miva,
        OSCommerce,
        ProStores,
        ShopSite,
        Yahoo,
        XCart,
        GenericModuleStore,
        ZenCart,
        VirtueMart,
        Magento,
        Amazon,
        SellerVantage,
        OrderDynamics,
        WebShopManager,
        SearchFit,
        CartKeeper,
        AmeriCommerce,
        NetworkSolutions,
        Volusion,
        OrderMotion,
        ClickCartPro,
        CommerceInterface,
        ThreeDCart,
        BigCommerce,
        Shopify,
        Etsy,
        Newegg,
        BuyDotCom,
        Sears,
        SolidCommerce,
        WebRequestTask,
        Brightpearl,
        OrderDeskCart,
        LiveSite,
        Zenventory,
		Fortune3,
        OpenCart,
        CloudConversion,
		
        [ApiPrivateLogSource]
        UspsNoPostage,

        [ApiPrivateLogSource]
        UspsStamps,

        [ApiPrivateLogSource]
        UspsEndicia,

        [ApiPrivateLogSource]
        UspsExpress1Endicia,

        [ApiPrivateLogSource]
        UspsExpress1Stamps,

        [ApiPrivateLogSource]
        FedEx,

        [ApiPrivateLogSource]
        UPS,

        [ApiPrivateLogSource]
        PayPal,

        [ApiPrivateLogSource]
        ShipWorks,

        [ApiPrivateLogSource]
        OnTrac,

        [ApiPrivateLogSource]
        iParcel,

        WooCommerce,
        Cart66Lite,
        Cart66Pro,
        Shopp,
        Shopperpress,
        WPeCommerce,
        Jigoshop,
        ChannelSale,
        SureDone,
        nopCommerce,
		LimeLightCRM,
		
		[ApiPrivateLogSource]
        InsureShip,
		
        SellerExpress,
        PowersportsSupport,
        CsCart,
        PrestaShop,
		LoadedCommerce,
        NoMoreRack,
		
    }
}
