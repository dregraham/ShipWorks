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
            { StoreTypeCode.Amosoft, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.BigCommerce, typeof(BigCommerceOrderIdentifier) },
            { StoreTypeCode.Brightpearl, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.BuyDotCom, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Cart66Lite, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Cart66Pro, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.ChannelSale, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Choxi, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.ClickCartPro, typeof(ClickCartProOrderIdentifier) },
            { StoreTypeCode.CloudConversion, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.CommerceInterface, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.CreLoaded, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.CsCart, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Ebay, typeof(EbayOrderIdentifier) },
            { StoreTypeCode.Fortune3, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.GeekSeller, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.GenericFile, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.GenericModule, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Groupon, typeof(GrouponOrderIdentifier) },
            { StoreTypeCode.InfiPlex, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.InstaStore, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Jet, typeof(JetOrderIdentifier) },
            { StoreTypeCode.Jigoshop, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.LemonStand, typeof(LemonStandOrderIdentifier) },
            { StoreTypeCode.LimeLightCRM, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.LiveSite, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.LoadedCommerce, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Magento, typeof(MagentoOrderIdentifier) },
            { StoreTypeCode.MarketplaceAdvisor, typeof(MarketplaceAdvisorOrderNumberIdentifier) },
            { StoreTypeCode.Miva, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.NetworkSolutions, typeof(NetworkSolutionsOrderIdentifier) },
            { StoreTypeCode.nopCommerce, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Odbc, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.OpenCart, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.OpenSky, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.OrderBot, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.OrderDesk, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.OrderDynamics, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.OrderMotion, typeof(OrderMotionOrderIdentifier) },
            { StoreTypeCode.osCommerce, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.PayPal, typeof(PayPalOrderIdentifier) },
            { StoreTypeCode.PowersportsSupport, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.PrestaShop, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.RevolutionParts, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.SearchFit, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Sears, typeof(SearsOrderIdentifier) },
            { StoreTypeCode.SellerActive, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.SellerCloud, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.SellerExpress, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.SellerVantage, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Shopify, typeof(ShopifyOrderIdentifier) },
            { StoreTypeCode.Shopp, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Shopperpress, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.SolidCommerce, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.StageBloc, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.SureDone, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.ThreeDCart, typeof(ThreeDCartOrderIdentifier) },
            { StoreTypeCode.VirtueMart, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Walmart, typeof(WalmartOrderIdentifier) },
            { StoreTypeCode.WebShopManager, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.WooCommerce, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.WPeCommerce, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.XCart, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Yahoo, typeof(YahooOrderIdentifier) },
            { StoreTypeCode.ZenCart, typeof(AlphaNumericOrderIdentifier) },
            { StoreTypeCode.Zenventory, typeof(AlphaNumericOrderIdentifier) }
        };

        public void Dispose() => context.Dispose();
    }
}