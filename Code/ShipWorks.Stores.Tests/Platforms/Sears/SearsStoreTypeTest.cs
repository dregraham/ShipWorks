using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Sears
{
    public class SearsStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SearsStoreType testObject;
        private readonly SearsOrderEntity order;

        public SearsStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new SearsOrderEntity { PoNumber = "ABC-123" };
            testObject = mock.Create<SearsStoreType>(TypedParameter.From<StoreEntity>(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsCorrectType()
        {
            var identifier = testObject.CreateOrderIdentifier(order);
            Assert.IsType<SearsOrderIdentifier>(identifier);
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsInvalidOperationException_WhenOrderIsNull()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CreateOrderIdentifier(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsInvalidOperationException_WhenOrderIsNotSears()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CreateOrderIdentifier(new OrderEntity()));
        }

        [Fact]
        public void GetAuditDescription_ReturnsValueWhenOrderIsCorrectType()
        {
            var identifier = testObject.GetAuditDescription(order);
            Assert.Equal("ABC-123", identifier);
        }

        [Fact]
        public void GetAuditDescription_ReturnsEmptyWhenOrderIsNull()
        {
            var identifier = testObject.GetAuditDescription(null);
            Assert.Empty(identifier);
        }

        [Fact]
        public void GetAuditDescription_ReturnsEmptyWhenOrderIsNotCorrectType()
        {
            var identifier = testObject.GetAuditDescription(new OrderEntity());
            Assert.Empty(identifier);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
