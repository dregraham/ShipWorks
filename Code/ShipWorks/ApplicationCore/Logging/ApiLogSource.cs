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
        AuctionSound,
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
        WebRequestTask,

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
        iParcel
    }
}
