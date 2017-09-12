using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Amazon
{
    public class AmazonStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly AmazonStoreType testObject;
        private readonly AmazonOrderEntity order;

        public AmazonStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new AmazonOrderEntity { AmazonOrderID = "ABC-123" };
            testObject = mock.Create<AmazonStoreType>(TypedParameter.From<StoreEntity>(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsCorrectType()
        {
            var identifier = testObject.CreateOrderIdentifier(order);
            Assert.IsType<AmazonOrderIdentifier>(identifier);
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsNullReferenceException_WhenOrderIsNull()
        {
            Assert.Throws<NullReferenceException>(() => testObject.CreateOrderIdentifier(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsInvalidCastException_WhenOrderIsNotAmazon()
        {
            Assert.Throws<InvalidCastException>(() => testObject.CreateOrderIdentifier(new OrderEntity()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
