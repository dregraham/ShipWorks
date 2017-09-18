using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.OnlineUpdating;
using ShipWorks.Stores.Platforms.AmeriCommerce.OnlineUpdating;
using ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating;
using ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating;
using ShipWorks.Stores.Platforms.ChannelAdvisor.OnlineUpdating;
using ShipWorks.Stores.Platforms.CommerceInterface.OnlineUpdating;
using ShipWorks.Stores.Platforms.Ebay.OnlineUpdating;
using ShipWorks.Stores.Platforms.Etsy.OnlineUpdating;
using ShipWorks.Stores.Platforms.GenericModule.OnlineUpdating;
using ShipWorks.Stores.Platforms.Groupon.OnlineUpdating;
using ShipWorks.Stores.Platforms.Infopia.OnlineUpdating;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.LemonStand.OnlineUpdating;
using ShipWorks.Stores.Platforms.Magento.OnlineUpdating;
using ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating;
using ShipWorks.Stores.Platforms.ProStores.OnlineUpdating;
using ShipWorks.Stores.Platforms.Sears.OnlineUpdating;
using ShipWorks.Stores.Platforms.Shopify.OnlineUpdating;
using ShipWorks.Stores.Platforms.SparkPay.Factories;
using ShipWorks.Stores.Platforms.ThreeDCart.OnlineUpdating;
using ShipWorks.Stores.Platforms.Volusion.OnlineUpdating;
using ShipWorks.Stores.Platforms.Walmart.OnlineUpdating;
using ShipWorks.Stores.Platforms.Yahoo.OnlineUpdating;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "ContinuousIntegration")]
    public class OnlineCommandCreatorRegistrationTest : IDisposable
    {
        readonly IContainer container;

        public OnlineCommandCreatorRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
            IoC.Initialize(container);
        }

        /// <summary>
        /// Test that all the store types that are not called out at the bottom of this test are not registered
        /// </summary>
        /// <remarks>
        /// This test does not use InlineData because it's MUCH faster to do it this way.
        /// </remarks>
        [Fact]
        public void EnsureNonSpecificStoreTypesAreNotRegistered()
        {
            var storesToSkip = specificUploaders.Select(x => x.Key).Concat(new[] { StoreTypeCode.Invalid });
            var storesToTest = Enum.GetValues(typeof(StoreTypeCode)).OfType<StoreTypeCode>().Except(storesToSkip);

            foreach (var storeTypeCode in storesToTest)
            {
                StoreType storeType = container.Resolve<IStoreTypeManager>().GetType(storeTypeCode);
                StoreEntity store = storeType.CreateStoreInstance();
                store.StoreTypeCode = storeTypeCode;

                Assert.False(container.IsRegisteredWithKey<IOnlineUpdateCommandCreator>(storeTypeCode));
            }
        }

        /// <summary>
        /// Test that all the store types called out at the bottom of this test resolve their respective online update creators
        /// </summary>
        /// <remarks>
        /// This test does not use InlineData because it's MUCH faster to do it this way.
        /// </remarks>
        [Fact]
        public void EnsureSpecificOnlineCommandCreatorsAreRegisteredCorrectly()
        {
            foreach (var entry in specificUploaders)
            {
                var storeTypeCode = entry.Key;
                var expectedDownloaderType = entry.Value;

                StoreType storeType = container.Resolve<IStoreTypeManager>().GetType(storeTypeCode);
                StoreEntity store = storeType.CreateStoreInstance();
                IOnlineUpdateCommandCreator retriever = container.ResolveKeyed<IOnlineUpdateCommandCreator>(storeTypeCode, TypedParameter.From(store));
                Assert.Equal(expectedDownloaderType, retriever.GetType());
            }
        }

        public void Dispose() => container?.Dispose();

        private readonly Dictionary<StoreTypeCode, Type> specificUploaders = new Dictionary<StoreTypeCode, Type>
        {
            { StoreTypeCode.Amazon, typeof(AmazonOnlineUpdateCommandCreator) },
            { StoreTypeCode.AmeriCommerce, typeof(AmeriCommerceOnlineUpdateCommandCreator) },
            { StoreTypeCode.Amosoft, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.BigCommerce, typeof(BigCommerceCommandCreator) },
            { StoreTypeCode.Brightpearl, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.BuyDotCom, typeof(BuyDotComOnlineUpdateCommandCreator) },
            { StoreTypeCode.Cart66Lite, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Cart66Pro, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.ChannelAdvisor, typeof(ChannelAdvisorOnlineUpdateCommandCreator) },
            { StoreTypeCode.ChannelSale, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Choxi, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.ClickCartPro, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.CloudConversion, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.CommerceInterface, typeof(CommerceInterfaceCommandCreator) },
            { StoreTypeCode.CreLoaded, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.CsCart, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Ebay, typeof(EbayOnlineUpdateCommandCreator) },
            { StoreTypeCode.Etsy, typeof(EtsyOnlineUpdateCommandCreator) },
            { StoreTypeCode.Fortune3, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.GeekSeller, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.GenericModule, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Groupon, typeof(GrouponOnlineUpdateCommandCreator) },
            { StoreTypeCode.InfiPlex, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Infopia, typeof(InfopiaOnlineUpdateCommandCreator) },
            { StoreTypeCode.InstaStore, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Jet, typeof(JetUpdateOnlineCommandCreator) },
            { StoreTypeCode.Jigoshop, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.LemonStand, typeof(LemonStandCommandCreator) },
            { StoreTypeCode.LimeLightCRM, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.LiveSite, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.LoadedCommerce, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Magento, typeof(MagentoOnlineUpdateCommandCreator) },
            { StoreTypeCode.Miva, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.NetworkSolutions, typeof(NetworkSolutionsOnlineUpdateCommandCreator) },
            { StoreTypeCode.NeweggMarketplace, typeof(NeweggOnlineUpdateCommandCreator) },
            { StoreTypeCode.nopCommerce, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Odbc, typeof(OdbcOnlineUpdateCommandCreator) },
            { StoreTypeCode.OpenCart, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.OpenSky, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.OrderBot, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.OrderDesk, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.OrderDynamics, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.OrderMotion, typeof(OrderMotionOnlineUpdateCommandCreator) },
            { StoreTypeCode.osCommerce, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.PowersportsSupport, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.PrestaShop, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.ProStores, typeof(ProStoresOnlineUpdateCommandCreator) },
            { StoreTypeCode.RevolutionParts, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.SearchFit, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Sears, typeof(SearsOnlineUpdateCommandCreator) },
            { StoreTypeCode.SellerActive, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.SellerCloud, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.SellerExpress, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.SellerVantage, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Shopify, typeof(ShopifyOnlineUpdateCommandCreator) },
            { StoreTypeCode.Shopp, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Shopperpress, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.SolidCommerce, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.SparkPay, typeof(SparkPayOnlineUpdateCommandCreator) },
            { StoreTypeCode.StageBloc, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.SureDone, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.ThreeDCart, typeof(ThreeDCartOnlineUpdateCommandCreator) },
            { StoreTypeCode.VirtueMart, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Volusion, typeof(VolusionOnlineUpdateCommandCreator) },
            { StoreTypeCode.Walmart, typeof(WalmartOnlineUpdateInstanceCommands) },
            { StoreTypeCode.WebShopManager, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.WooCommerce, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.WPeCommerce, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.XCart, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Yahoo, typeof(YahooOnlineUpdateCommandCreator) },
            { StoreTypeCode.ZenCart, typeof(GenericModuleOnlineUpdateCommandCreator) },
            { StoreTypeCode.Zenventory, typeof(GenericModuleOnlineUpdateCommandCreator) }
        };
    }
}
