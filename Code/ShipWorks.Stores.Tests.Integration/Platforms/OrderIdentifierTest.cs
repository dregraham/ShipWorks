using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.ClickCartPro;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Groupon;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor;
using ShipWorks.Stores.Platforms.NetworkSolutions;
using ShipWorks.Stores.Platforms.OrderMotion;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Stores.Platforms.Walmart;
using ShipWorks.Stores.Platforms.Yahoo;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using Xunit.Abstractions;

namespace ShipWorks.Stores.Tests.Integration.Platforms
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class OrderIdentifierTest : IDisposable
    {
        private readonly DataContext context;
        private readonly ITestOutputHelper testOutputHelper;

        public OrderIdentifierTest(DatabaseFixture db, ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsCorrectType()
        {
            var identifiers = Enum.GetValues(typeof(StoreTypeCode))
                .OfType<StoreTypeCode>()
                .Except(new[] { StoreTypeCode.Invalid })
                .Select(x => context.Mock.Container.ResolveKeyed<StoreType>(x, TypedParameter.From<StoreEntity>(null)))
                .Select(x => new
                {
                    StoreType = x.TypeCode,
                    Actual = CreateIdentifier(x).GetType(),
                    Expected = GetExpectedType(x.TypeCode)
                })
                .Where(x => x.Actual != x.Expected)
                .ToList();

            foreach (var result in identifiers)
            {
                testOutputHelper.WriteLine($"{result.StoreType} failed: expected {result.Expected.Name} but got {result.Actual.Name}");
            }

            Assert.Empty(identifiers);
        }

        [Fact]
        public void CreateCombinedSearchQuery_JoinsOrderSearch()
        {
            QueryFactory factory = new QueryFactory();

            using (ISqlAdapter sqlAdapter = context.Mock.Container.Resolve<ISqlAdapterFactory>().Create())
            {
                var storeTypes = Enum.GetValues(typeof(StoreTypeCode))
                    .OfType<StoreTypeCode>()
                    .Except(new[] { StoreTypeCode.Invalid })
                    .Select(x => context.Mock.Container.ResolveKeyed<StoreType>(x, TypedParameter.From<StoreEntity>(null)))
                    .Select(x => TestCreateCombinedSearchQuery(sqlAdapter, factory, x))
                    .Where(x => x.Failure)
                    .ToList();

                foreach (var result in storeTypes)
                {
                    testOutputHelper.WriteLine($"{result.Value} failed: {result.Message}");
                }

                Assert.Empty(storeTypes);
            }
        }

        private GenericResult<StoreTypeCode> TestCreateCombinedSearchQuery(ISqlAdapter sqlAdapter, QueryFactory factory, StoreType storeType)
        {
            try
            {
                OrderIdentifier identifier = CreateIdentifier(storeType);

                QuerySpec combinedSearchQuery = identifier.CreateCombinedSearchQuery(factory);
                combinedSearchQuery.AndWhere(OrderSearchFields.StoreID == 1005);
                DynamicQuery query = factory.Create().Select(combinedSearchQuery.Any());

                sqlAdapter.FetchScalar<bool?>(query);

                return GenericResult.FromSuccess(storeType.TypeCode);
            }
            catch (Exception ex)
            {
                return GenericResult.FromError(ex, storeType.TypeCode);
            }
        }

        private OrderIdentifier CreateIdentifier(StoreType storeType)
        {
            var order = storeType.CreateOrder();

            if (orderConfig.ContainsKey(storeType.TypeCode))
            {
                orderConfig[storeType.TypeCode](order);
            }
            else
            {
                order.ChangeOrderNumber("12345");
            }

            var identifier = storeType.CreateOrderIdentifier(order);
            return identifier;
        }

        private Type GetExpectedType(StoreTypeCode typeCode) =>
            expectedIdentifierTypes.ContainsKey(typeCode) ? expectedIdentifierTypes[typeCode] : typeof(OrderNumberIdentifier);

        private readonly IDictionary<StoreTypeCode, Action<OrderEntity>> orderConfig = new Dictionary<StoreTypeCode, Action<OrderEntity>>
        {
            { StoreTypeCode.MarketplaceAdvisor, x => x.ChangeOrderNumber("foo-2") }
        };

        private readonly IDictionary<StoreTypeCode, Type> expectedIdentifierTypes = new Dictionary<StoreTypeCode, Type>
        {
            { StoreTypeCode.Amazon, typeof(AmazonOrderIdentifier) },
            { StoreTypeCode.Amosoft, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.BigCommerce, typeof(BigCommerceOrderIdentifier) },
            { StoreTypeCode.Brightpearl, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Cart66Lite, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Cart66Pro, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.ChannelSale, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Choxi, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.ClickCartPro, typeof(ClickCartProOrderIdentifier) },
            { StoreTypeCode.CloudConversion, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.CommerceInterface, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.CreLoaded, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.CsCart, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Ebay, typeof(EbayOrderIdentifier) },
            { StoreTypeCode.Fortune3, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.GeekSeller, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.GenericFile, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.GenericModule, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Groupon, typeof(GrouponOrderIdentifier) },
            { StoreTypeCode.InfiPlex, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.InstaStore, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Jet, typeof(JetOrderIdentifier) },
            { StoreTypeCode.Jigoshop, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.LemonStand, typeof(LemonStandOrderIdentifier) },
            { StoreTypeCode.LimeLightCRM, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.LiveSite, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.LoadedCommerce, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Magento, typeof(MagentoOrderIdentifier) },
            { StoreTypeCode.MarketplaceAdvisor, typeof(MarketplaceAdvisorOrderNumberIdentifier) },
            { StoreTypeCode.Miva, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.NetworkSolutions, typeof(NetworkSolutionsOrderIdentifier) },
            { StoreTypeCode.nopCommerce, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Odbc, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.OpenCart, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.OpenSky, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.OrderBot, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.OrderDesk, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.OrderDynamics, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.OrderMotion, typeof(OrderMotionOrderIdentifier) },
            { StoreTypeCode.osCommerce, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.PayPal, typeof(PayPalOrderIdentifier) },
            { StoreTypeCode.PowersportsSupport, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.PrestaShop, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.RevolutionParts, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.SearchFit, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Sears, typeof(SearsOrderIdentifier) },
            { StoreTypeCode.SellerActive, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.SellerCloud, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.SellerExpress, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.SellerVantage, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Shopify, typeof(ShopifyOrderIdentifier) },
            { StoreTypeCode.Shopp, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Shopperpress, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.SolidCommerce, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.StageBloc, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.SureDone, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.ThreeDCart, typeof(ThreeDCartOrderIdentifier) },
            { StoreTypeCode.VirtueMart, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Walmart, typeof(WalmartOrderIdentifier) },
            { StoreTypeCode.WebShopManager, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.WooCommerce, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.WPeCommerce, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.XCart, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Yahoo, typeof(YahooOrderIdentifier) },
            { StoreTypeCode.ZenCart, typeof(GenericOrderIdentifier) },
            { StoreTypeCode.Zenventory, typeof(GenericOrderIdentifier) }
        };

        public void Dispose() => context.Dispose();
    }
}