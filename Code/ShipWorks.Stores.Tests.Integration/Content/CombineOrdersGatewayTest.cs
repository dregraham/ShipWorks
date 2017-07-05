using System.ComponentModel;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
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

        [Fact]
        public async Task CanCombine_ReturnTrue_WhenNotPrime()
        {
            var testObject = context.Mock.Create<CombineOrdersGateway>();

            //AmazonStoreEntity store = new AmazonStoreEntity();
            //store.StoreTypeCode = StoreTypeCode.Amazon;

            Create.Order<AmazonOrderEntity>(context.Store, context.Customer)
                .Set(x => x.IsPrime = 2)
                .Save();

            Assert.True(await testObject.CanCombine(context.Store, new []{ context.Order.OrderID }));
        }

        //[Fact]
        //public async Task CanCombine_ReturnFalse_WhenPrime()
        //{
        //    var testObject = context.Mock.Create<CombineOrdersGateway>();

        //    var isPrime = Create.Order<AmazonOrderEntity>(context.Store, context.Customer)
        //        .Set(x => x.IsPrime = 2)
        //        .Save();

        //    Assert.True(await testObject.CanCombine(context.Store, new[] { context.Order.OrderID }));
        //}
    }
}
