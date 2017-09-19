using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Ebay;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Ebay
{
    public class EbayStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly EbayStoreType testObject;
        private readonly EbayOrderEntity order;

        public EbayStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new EbayOrderEntity { EbayOrderID = 456, SellingManagerRecord = 123 };
            testObject = mock.Create<EbayStoreType>(TypedParameter.From<StoreEntity>(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsCorrectType()
        {
            var identifier = testObject.CreateOrderIdentifier(order);
            Assert.IsType<EbayOrderIdentifier>(identifier);
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsInvalidOperationException_WhenOrderIsNull()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CreateOrderIdentifier(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsInvalidOperationException_WhenOrderIsNotEbay()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CreateOrderIdentifier(new OrderEntity()));
        }

        [Fact]
        public void GetAuditDescription_ReturnsValueWhenOrderIsCorrectType()
        {
            var identifier = testObject.GetAuditDescription(order);
            Assert.Equal("123", identifier);
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
