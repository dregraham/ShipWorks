using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Manager of all the StoreTypes available in ShipWorks
    /// </summary>
    public static class StoreTypeManager
    {
        /// <summary>
        /// Returns all store types in ShipWorks
        /// </summary>
        public static List<StoreType> StoreTypes
        {
            get
            {
                List<StoreType> storeTypes = new List<StoreType>();

                foreach (StoreTypeCode typeCode in Enum.GetValues(typeof(StoreTypeCode)))
                {
                    // Skip the invalid one
                    if (typeCode == StoreTypeCode.Invalid)
                    {
                        continue;
                    }

                    if (IsStoreTypeDisabled(typeCode))
                    {
                        // Temporary: don't show in ShipWorks for the new stores until marketing materials 
                        // and other ancillary materials are ready to go
                        continue;
                    }

                    StoreType storeType = GetType(typeCode);
                    storeTypes.Add(storeType);
                }

                // Sort based on the store name
                storeTypes.Sort(new Comparison<StoreType>( delegate (StoreType left, StoreType right)
                    {
                        return left.StoreTypeName.CompareTo(right.StoreTypeName);  
                    }));

                return storeTypes;
            }
        }

        /// <summary>
        /// Get the StoreType instance of the specified StoreEntity
        /// </summary>
        public static StoreType GetType(StoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            return GetType((StoreTypeCode) store.TypeCode, store);
        }

        /// <summary>
        /// The indexer of the class based on store type
        /// </summary>
        public static StoreType GetType(StoreTypeCode typeCode)
        {
            return GetType(typeCode, null);
        }

        /// <summary>
        /// Return the StoreType for the given store type.  If store is not null,
        /// then any "instance" methods of the StoreType will use it.
        /// </summary>
        private static StoreType GetType(StoreTypeCode typeCode, StoreEntity store)
        {
            switch (typeCode)
            {
                case StoreTypeCode.ChannelAdvisor: return new Platforms.ChannelAdvisor.ChannelAdvisorStoreType(store);
                case StoreTypeCode.CreLoaded: return new Platforms.CreLoaded.CreLoadedStoreType(store);
                case StoreTypeCode.GenericModule: return new Platforms.GenericModule.GenericModuleStoreType(store);
                case StoreTypeCode.GenericFile: return new Platforms.GenericFile.GenericFileStoreType(store);
                case StoreTypeCode.Magento: return new Platforms.Magento.MagentoStoreType(store);
                case StoreTypeCode.osCommerce: return new Platforms.osCommerce.oscStoreType(store);
                case StoreTypeCode.ShopSite: return new Platforms.ShopSite.ShopSiteStoreType(store);
                case StoreTypeCode.VirtueMart: return new Platforms.VirtueMart.VirtueMartStoreType(store);
                case StoreTypeCode.XCart: return new Platforms.XCart.XCartStoreType(store);
                case StoreTypeCode.ZenCart: return new Platforms.ZenCart.ZenCartStoreType(store);
                case StoreTypeCode.Infopia: return new Platforms.Infopia.InfopiaStoreType(store);
                case StoreTypeCode.PayPal: return new Platforms.PayPal.PayPalStoreType(store);
                case StoreTypeCode.Amazon: return new Platforms.Amazon.AmazonStoreType(store);
                case StoreTypeCode.Ebay: return new Platforms.Ebay.EbayStoreType(store);
                case StoreTypeCode.Miva: return new Platforms.Miva.MivaStoreType(store);
                case StoreTypeCode.MarketplaceAdvisor: return new Platforms.MarketplaceAdvisor.MarketplaceAdvisorStoreType(store);
                case StoreTypeCode.Yahoo: return new Platforms.Yahoo.YahooStoreType(store);
                case StoreTypeCode.SellerVantage: return new Platforms.SellerVantage.SellerVantageStoreType(store);
                case StoreTypeCode.OrderDynamics: return new Platforms.OrderDynamics.OrderDynamicsStoreType(store);
                case StoreTypeCode.WebShopManager: return new Platforms.WebShopManager.WebShopManagerStoreType(store);
                case StoreTypeCode.SearchFit: return new Platforms.SearchFit.SearchFitStoreType(store);
                case StoreTypeCode.ProStores: return new Platforms.ProStores.ProStoresStoreType(store);
                case StoreTypeCode.AmeriCommerce: return new Platforms.AmeriCommerce.AmeriCommerceStoreType(store);
                case StoreTypeCode.NetworkSolutions: return new Platforms.NetworkSolutions.NetworkSolutionsStoreType(store);
                case StoreTypeCode.Volusion: return new Platforms.Volusion.VolusionStoreType(store);
                case StoreTypeCode.OrderMotion: return new Platforms.OrderMotion.OrderMotionStoreType(store);
                case StoreTypeCode.ClickCartPro: return new Platforms.ClickCartPro.ClickCartProStoreType(store);
                case StoreTypeCode.CommerceInterface: return new Platforms.CommerceInterface.CommerceInterfaceStoreType(store);
                case StoreTypeCode.ThreeDCart: return new Platforms.ThreeDCart.ThreeDCartStoreType(store);
                case StoreTypeCode.BigCommerce: return new Platforms.BigCommerce.BigCommerceStoreType(store);
                case StoreTypeCode.Etsy: return new Platforms.Etsy.EtsyStoreType(store);
                case StoreTypeCode.Shopify: return new Platforms.Shopify.ShopifyStoreType(store);
                case StoreTypeCode.NeweggMarketplace: return new Platforms.Newegg.NeweggStoreType(store);             
                case StoreTypeCode.BuyDotCom: return new Platforms.BuyDotCom.BuyDotComStoreType(store);
                case StoreTypeCode.Sears: return new Platforms.Sears.SearsStoreType(store);
                case StoreTypeCode.SolidCommerce: return new Platforms.SolidCommerce.SolidCommerceStoreType(store);
                case StoreTypeCode.Brightpearl: return new Platforms.Brightpearl.BrightpearlStoreType(store);
                case StoreTypeCode.OrderDesk: return new Platforms.OrderDesk.OrderDeskStoreType(store);
                case StoreTypeCode.WooCommerce: return new Platforms.WooCommerce.WooCommerceStoreType(store);
                case StoreTypeCode.Cart66Lite: return new Platforms.Cart66.Cart66LiteStoreType(store);
                case StoreTypeCode.Cart66Pro: return new Platforms.Cart66.Cart66ProStoreType(store);
                case StoreTypeCode.Shopp: return new Platforms.Shopp.ShoppStoreType(store);
                case StoreTypeCode.Shopperpress: return new Platforms.Shopperpress.ShopperpressStoreType(store);
                case StoreTypeCode.WPeCommerce: return new Platforms.WPeCommerce.WPeCommerceStoreType(store);
                case StoreTypeCode.Jigoshop: return new Platforms.Jigoshop.JigoshopStoreType(store);
                case StoreTypeCode.ChannelSale: return new Platforms.ChannelSale.ChannelSaleStoreType(store);
                case StoreTypeCode.LiveSite: return new Platforms.LiveSite.LiveSiteStoreType(store);
				case StoreTypeCode.SureDone: return new Platforms.SureDone.SureDoneStoreType(store);
                case StoreTypeCode.Zenventory: return new Platforms.Zenventory.ZenventoryStoreType(store);
                case StoreTypeCode.Fortune3: return new Platforms.Fortune3.Fortune3StoreType(store);
                case StoreTypeCode.LimeLightCRM: return new Platforms.LimeLightCRM.LimeLightCRMStoreType(store);
				case StoreTypeCode.OpenCart: return new Platforms.OpenCart.OpenCartStoreType(store);
                case StoreTypeCode.nopCommerce: return new Platforms.nopCommerce.nopCommerceStoreType(store);
                case StoreTypeCode.SellerExpress: return new Platforms.SellerExpress.SellerExpressStoreType(store);
                case StoreTypeCode.PowersportsSupport: return new Platforms.PowersportsSupport.PowersportsSupportStoreType(store);
                case StoreTypeCode.CloudConversion: return new Platforms.CloudConversion.CloudConversionStoreType(store);
                case StoreTypeCode.CsCart: return new Platforms.CsCart.CsCartStoreType(store);
                case StoreTypeCode.PrestaShop: return new Platforms.PrestaShop.PrestaShopStoreType(store);
                case StoreTypeCode.LoadedCommerce: return new Platforms.LoadedCommerce.LoadedCommerceStoreType(store);
                case StoreTypeCode.NoMoreRack: return new Platforms.NoMoreRack.NoMoreRackStoreType(store);
                case StoreTypeCode.Groupon: return new Platforms.Groupon.GrouponStoreType(store);
            }

            throw new InvalidOperationException("Invalid store type.");
        }

        /// <summary>
        /// Determines whether the store type is disabled. This is only temporary, so we can continue
        /// to release ShipWorks until supporting materials for the new store types are ready.
        /// </summary>
        private static bool IsStoreTypeDisabled(StoreTypeCode typeCode)
        {
            // Don't show in ShipWorks until marketing materials and other ancillary 
            // materials are ready to go
            List<StoreTypeCode> disabledTypes = new List<StoreTypeCode>
            {

            };

            return disabledTypes.Contains(typeCode);
        }
    }
}
