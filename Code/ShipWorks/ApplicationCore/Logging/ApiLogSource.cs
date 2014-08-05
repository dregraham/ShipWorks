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
        EquaShip,

        [ApiPrivateLogSource]
        OnTrac,

        [ApiPrivateLogSource]
        iParcel,

        [ApiPrivateLogSource]
        WooCommerce,

        [ApiPrivateLogSource]
        Cart66Lite,

        [ApiPrivateLogSource]
        Cart66Pro,

        [ApiPrivateLogSource]
        Shopp,

        [ApiPrivateLogSource]
        Shopperpress,

        [ApiPrivateLogSource]
        WPeCommerce,

        [ApiPrivateLogSource]
        Jigoshop,

        [ApiPrivateLogSource]
        ChannelSale,

        [ApiPrivateLogSource]
        SureDone,
		[ApiPrivateLogSource]
        nopCommerce,

        [ApiPrivateLogSource]
		LimeLightCRM,
		
		[ApiPrivateLogSource]
        InsureShip
    }
}
