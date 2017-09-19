using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.PayPal
{
    public class PayPalStoreTypeTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly PayPalStoreType testObject;
        private readonly PayPalOrderEntity order;

        public PayPalStoreTypeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new PayPalOrderEntity { TransactionID = "ABC-123" };
            testObject = mock.Create<PayPalStoreType>(TypedParameter.From<StoreEntity>(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ReturnsCorrectType()
        {
            var identifier = testObject.CreateOrderIdentifier(order);
            Assert.IsType<PayPalOrderIdentifier>(identifier);
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsInvalidOperationException_WhenOrderIsNull()
        {
            Assert.Throws<InvalidOperationException>(() => testObject.CreateOrderIdentifier(null));
        }

        [Fact]
        public void CreateOrderIdentifier_ThrowsInvalidOperationException_WhenOrderIsNotPayPal()
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
