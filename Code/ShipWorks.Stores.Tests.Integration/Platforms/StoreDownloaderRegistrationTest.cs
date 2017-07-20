using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.AmeriCommerce;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BuyDotCom;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using ShipWorks.Stores.Platforms.ClickCartPro;
using ShipWorks.Stores.Platforms.CommerceInterface;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.Etsy;
using ShipWorks.Stores.Platforms.GenericFile;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Groupon;
using ShipWorks.Stores.Platforms.Infopia;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor;
using ShipWorks.Stores.Platforms.Miva;
using ShipWorks.Stores.Platforms.NetworkSolutions;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.OrderMotion;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.ProStores;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Stores.Platforms.ShopSite;
using ShipWorks.Stores.Platforms.SparkPay;
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Stores.Platforms.Volusion;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Yahoo;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Trait("Category", "ContinuousIntegration")]
    public class StoreDownloaderRegistrationTest : IDisposable
    {
        readonly IContainer container;

        public StoreDownloaderRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
            IoC.Initialize(container);
        }

        /// <summary>
        /// Test that all the store types that are not called out at the bottom of this test resolve to the GenericModuleDownloader
        /// </summary>
        /// <remarks>
        /// This test does not use InlineData because it's MUCH faster to do it this way.
        /// </remarks>
        [Fact]
        public void EnsureNonSpecificStoreTypesHaveGenericModuleDownloaderRegistered()
        {
            var storesToSkip = specificDownloaders.Select(x => x.Key).Concat(new[] { StoreTypeCode.Invalid });
            var storesToTest = Enum.GetValues(typeof(StoreTypeCode)).OfType<StoreTypeCode>().Except(storesToSkip);

            foreach (var storeTypeCode in storesToTest)
            {
                StoreType storeType = container.Resolve<IStoreTypeManager>().GetType(storeTypeCode);
                StoreEntity store = storeType.CreateStoreInstance();
                store.StoreTypeCode = storeTypeCode;
                IStoreDownloader service = container.ResolveKeyed<IStoreDownloader>(storeTypeCode, TypedParameter.From(store));
                Assert.IsAssignableFrom<GenericModuleDownloader>(service);
            }
        }

        /// <summary>
        /// Test that all the store types called out at the bottom of this test resolve their respective downloaders
        /// </summary>
        /// <remarks>
        /// This test does not use InlineData because it's MUCH faster to do it this way.
        /// </remarks>
        [Fact]
        public void EnsureSpecificStoreDownloadersAreRegisteredCorrectly()
        {
            foreach (var entry in specificDownloaders)
            {
                var storeTypeCode = entry.Key;
                var expectedDownloaderType = entry.Value;

                StoreType storeType = container.Resolve<IStoreTypeManager>().GetType(storeTypeCode);
                StoreEntity store = storeType.CreateStoreInstance();
                IStoreDownloader retriever = container.ResolveKeyed<IStoreDownloader>(storeTypeCode, TypedParameter.From(store));
                Assert.Equal(expectedDownloaderType, retriever.GetType());
            }
        }

        public void Dispose() => container?.Dispose();

        private readonly Dictionary<StoreTypeCode, Type> specificDownloaders = new Dictionary<StoreTypeCode, Type>
        {
            { StoreTypeCode.Amazon, typeof(AmazonDownloaderFactory) },
            { StoreTypeCode.AmeriCommerce, typeof(AmeriCommerceDownloader) },
            { StoreTypeCode.BigCommerce, typeof(BigCommerceDownloader) },
            { StoreTypeCode.BuyDotCom, typeof(BuyDotComDownloader) },
            { StoreTypeCode.ChannelAdvisor, typeof(ChannelAdvisorDownloader) },
            { StoreTypeCode.ClickCartPro, typeof(ClickCartProDownloader) },
            { StoreTypeCode.CommerceInterface, typeof(CommerceInterfaceDownloader) },
            { StoreTypeCode.Ebay, typeof(EbayDownloader) },
            { StoreTypeCode.Etsy, typeof(EtsyDownloader) },
            { StoreTypeCode.GenericFile, typeof(GenericFileDownloaderFactory) },
            { StoreTypeCode.GenericModule, typeof(GenericModuleDownloader) },
            { StoreTypeCode.Groupon, typeof(GrouponDownloader) },
            { StoreTypeCode.Infopia, typeof(InfopiaDownloader) },
            { StoreTypeCode.LemonStand, typeof(LemonStandDownloader) },
            { StoreTypeCode.Magento, typeof(MagentoDownloaderFactory) },
            { StoreTypeCode.MarketplaceAdvisor, typeof(MarketplaceAdvisorDownloaderFactory) },
            { StoreTypeCode.Miva, typeof(MivaDownloader) },
            { StoreTypeCode.NetworkSolutions, typeof(NetworkSolutionsDownloader) },
            { StoreTypeCode.NeweggMarketplace, typeof(NeweggDownloader) },
            { StoreTypeCode.Odbc, typeof(OdbcStoreDownloader) },
            { StoreTypeCode.OrderMotion, typeof(OrderMotionDownloader) },
            { StoreTypeCode.PayPal, typeof(PayPalDownloader) },
            { StoreTypeCode.ProStores, typeof(ProStoresDownloader) },
            { StoreTypeCode.Sears, typeof(SearsDownloader) },
            { StoreTypeCode.Shopify, typeof(ShopifyDownloader) },
            { StoreTypeCode.ShopSite, typeof(ShopSiteDownloader) },
            { StoreTypeCode.SparkPay, typeof(SparkPayDownloader) },
            { StoreTypeCode.ThreeDCart, typeof(ThreeDCartDownloaderFactory) },
            { StoreTypeCode.Volusion, typeof(VolusionDownloader) },
            { StoreTypeCode.Walmart, typeof(WalmartDownloader) },
            { StoreTypeCode.Yahoo, typeof(YahooDownloaderFactory) }
        };
    }
}
