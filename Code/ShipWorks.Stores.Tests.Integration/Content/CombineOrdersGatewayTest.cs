using System.ComponentModel;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Content
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class CombineOrdersGatewayTest
    {
        private readonly DataContext context;

        public CombineOrdersGatewayTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public async Task CanCombine_ReturnsFalse_WhenProcessed()
        {
            Modify.Order(context.Order)
                .WithShipment(x => x.Set(y => y.Processed = true))
                .Save();

            var testObject = context.Mock.Create<CombineOrdersGateway>();

            Assert.False(await testObject.CanCombine(context.Store, new []{ context.Order.OrderID }));
        }

        [Fact]
        public async Task CanCombine_ReturnsTrue_WhenNoShipments()
        {
            var testObject = context.Mock.Create<CombineOrdersGateway>();

            Assert.True(await testObject.CanCombine(context.Store, new[] { context.Order.OrderID }));
        }

        [Fact]
        public async Task CanCombine_ReturnsTrue_WhenNotProcessed()
        {
            Modify.Order(context.Order)
                .WithShipment(x => x.Set(y => y.Processed = false))
                .Save();

            var testObject = context.Mock.Create<CombineOrdersGateway>();

            Assert.True(await testObject.CanCombine(context.Store, new[] { context.Order.OrderID, 2 }));
        }

        [Fact]
        public async Task CanCombine_ReturnsFalse_StoreDoNotMatch()
        {
            var testObject = context.Mock.Create<CombineOrdersGateway>();

            StoreEntity store = new StoreEntity(17);
            store.StoreTypeCode = StoreTypeCode.GenericModule;

            Assert.False(await testObject.CanCombine(store, new[] { context.Order.OrderID }));
        }

        [Theory]
        [InlineData(AmazonMwsIsPrime.Yes, false)]
        [InlineData(AmazonMwsIsPrime.No, true)]
        [InlineData(AmazonMwsIsPrime.Unknown, false)]
        public async Task CanCombine_QueriesPrime(AmazonMwsIsPrime isPrime, bool expected)
        {
            var testObject = context.Mock.Create<CombineOrdersGateway>();

            var store = Create.Store<AmazonStoreEntity>(StoreTypeCode.Amazon).Save();

            var order = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.IsPrime = (int) isPrime)
                .Set(x => x.FulfillmentChannel = (int) AmazonMwsFulfillmentChannel.MFN)
                .Save();

            var result = await testObject.CanCombine(store, new[] {order.OrderID});

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(AmazonMwsFulfillmentChannel.AFN, false)]
        [InlineData(AmazonMwsFulfillmentChannel.MFN, true)]
        [InlineData(AmazonMwsFulfillmentChannel.Unknown, false)]
        public async Task CanCombine_QueriesAmazonFulfillment(AmazonMwsFulfillmentChannel isFulfillment, bool expected)
        {
            var testObject = context.Mock.Create<CombineOrdersGateway>();

            var store = Create.Store<AmazonStoreEntity>(StoreTypeCode.Amazon).Save();

            var order = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.FulfillmentChannel = (int) isFulfillment)
                .Set(x => x.IsPrime = (int) AmazonMwsIsPrime.No)
                .Save();

            var result = await testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }
    }
}
