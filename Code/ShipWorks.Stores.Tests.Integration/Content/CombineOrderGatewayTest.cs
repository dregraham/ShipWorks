using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.GeekSeller;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.OpenSky;
using ShipWorks.Stores.Platforms.ZenCart;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Content
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class CombineOrderGatewayTest
    {
        private readonly DataContext context;

        public CombineOrderGatewayTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void CanCombine_ReturnsFalse_WhenProcessed()
        {
            Modify.Order(context.Order)
                .WithShipment(x => x.Set(y => y.Processed = true))
                .Save();

            var testObject = context.Mock.Create<CombineOrderGateway>();

            Assert.False(testObject.CanCombine(context.Store, new[] { context.Order.OrderID }));
        }

        [Fact]
        public void CanCombine_ReturnsTrue_WhenNoShipments()
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            Assert.True(testObject.CanCombine(context.Store, new[] { context.Order.OrderID }));
        }

        [Fact]
        public void CanCombine_ReturnsTrue_WhenNotProcessed()
        {
            Modify.Order(context.Order)
                .WithShipment(x => x.Set(y => y.Processed = false))
                .Save();

            var testObject = context.Mock.Create<CombineOrderGateway>();

            Assert.True(testObject.CanCombine(context.Store, new[] { context.Order.OrderID, 2 }));
        }

        [Fact]
        public void CanCombine_ReturnsFalse_StoreDoNotMatch()
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            StoreEntity store = new StoreEntity(17);
            store.StoreTypeCode = StoreTypeCode.GenericModule;

            Assert.False(testObject.CanCombine(store, new[] { context.Order.OrderID }));
        }
        
        [Theory]
        [InlineData(AmazonIsPrime.Yes, false, StoreTypeCode.GenericModule)]
        [InlineData(AmazonIsPrime.Yes, false, StoreTypeCode.GeekSeller)]
        [InlineData(AmazonIsPrime.No, true, StoreTypeCode.GenericModule)]
        [InlineData(AmazonIsPrime.No, true, StoreTypeCode.ZenCart)]
        [InlineData(AmazonIsPrime.Unknown, false, StoreTypeCode.GenericModule)]
        [InlineData(AmazonIsPrime.Unknown, false, StoreTypeCode.OpenSky)]
        public void CanCombine_QueriesPrime_FromGenericModuleBasedStore(AmazonIsPrime isPrime, bool expected, StoreTypeCode storeTypeCode)
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<StoreEntity>(storeTypeCode).Save();

            var order = Create.Order<GenericModuleOrderEntity>(store, context.Customer)
                .Set(x => x.IsPrime = isPrime)
                .Set(x => x.IsFBA = false)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData(false, true, StoreTypeCode.GenericModule)]
        [InlineData(false, true, StoreTypeCode.GeekSeller)]
        [InlineData(true, false, StoreTypeCode.GenericModule)]
        [InlineData(true, false, StoreTypeCode.ZenCart)]
        [InlineData(false, true, StoreTypeCode.OpenSky)]
        public void CanCombine_QueriesIsFBA_FromGenericModuleBasedStore(bool isFBA, bool expected, StoreTypeCode storeTypeCode)
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<StoreEntity>(storeTypeCode).Save();

            var order = Create.Order<GenericModuleOrderEntity>(store, context.Customer)
                .Set(x => x.IsFBA = isFBA)
                .Set(x => x.IsPrime = AmazonIsPrime.No)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(AmazonIsPrime.Yes, false)]
        [InlineData(AmazonIsPrime.No, true)]
        [InlineData(AmazonIsPrime.Unknown, false)]
        public void CanCombine_QueriesPrime_FromAmazon(AmazonIsPrime isPrime, bool expected)
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<AmazonStoreEntity>(StoreTypeCode.Amazon).Save();

            var order = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.IsPrime = (int) isPrime)
                .Set(x => x.FulfillmentChannel = (int) AmazonMwsFulfillmentChannel.MFN)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(AmazonMwsFulfillmentChannel.AFN, false)]
        [InlineData(AmazonMwsFulfillmentChannel.MFN, true)]
        [InlineData(AmazonMwsFulfillmentChannel.Unknown, false)]
        public void CanCombine_QueriesAmazonFulfillment_FromAmazon(AmazonMwsFulfillmentChannel isFulfillment, bool expected)
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<AmazonStoreEntity>(StoreTypeCode.Amazon).Save();

            var order = Create.Order<AmazonOrderEntity>(store, context.Customer)
                .Set(x => x.FulfillmentChannel = (int) isFulfillment)
                .Set(x => x.IsPrime = (int) AmazonIsPrime.No)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void CanCombine_QueriesEbay(bool isGsp, bool expected)
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<EbayStoreEntity>(StoreTypeCode.Ebay).Save();

            var order = Create.Order<EbayOrderEntity>(store, context.Customer)
                .Set(x => x.GspEligible = isGsp)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CanCombine_ReturnsFalse_WhenEbayOrdersHaveDifferentPaymentMethods()
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<EbayStoreEntity>(StoreTypeCode.Ebay).Save();

            var order = Create.Order<EbayOrderEntity>(store, context.Customer)
                .Set(x => x.RollupEffectiveCheckoutStatus, 1)
                .Save();

            var order2 = Create.Order<EbayOrderEntity>(store, context.Customer)
                .Set(x => x.RollupEffectiveCheckoutStatus, 2)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID, order2.OrderID });

            Assert.False(result);
        }

        [Theory]
        [InlineData(AmazonIsPrime.Yes, false)]
        [InlineData(AmazonIsPrime.No, true)]
        [InlineData(AmazonIsPrime.Unknown, false)]
        public void CanCombine_QueriesPrime_FromChannelAdvisor(AmazonIsPrime isPrime, bool expected)
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<ChannelAdvisorStoreEntity>(StoreTypeCode.ChannelAdvisor).Save();

            var order = Create.Order<ChannelAdvisorOrderEntity>(store, context.Customer)
                .Set(x => x.IsPrime = (int) isPrime)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(false, false, true)]
        [InlineData(true, false, false)]
        [InlineData(true, true, false)]
        public void CanCombine_QueriesAmazonFulfillment_FromChannelAdvisor(bool itemIsFba1, bool itemIsFba2, bool expected)
        {
            var testObject = context.Mock.Create<CombineOrderGateway>();

            var store = Create.Store<ChannelAdvisorStoreEntity>(StoreTypeCode.ChannelAdvisor).Save();

            var order = Create.Order<ChannelAdvisorOrderEntity>(store, context.Customer)
                .WithItem<ChannelAdvisorOrderItemEntity>(i => i.Set(x => x.IsFBA = itemIsFba1))
                .WithItem<ChannelAdvisorOrderItemEntity>(i => i.Set(x => x.IsFBA = itemIsFba2))
                .Set(x => x.IsPrime = (int) AmazonIsPrime.No)
                .Save();

            var result = testObject.CanCombine(store, new[] { order.OrderID });

            Assert.Equal(expected, result);
        }
    }
}