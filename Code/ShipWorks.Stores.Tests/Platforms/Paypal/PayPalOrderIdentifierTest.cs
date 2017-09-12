using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.PayPal;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.PayPal
{
    public class PayPalOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly PayPalOrderEntity order;

        public PayPalOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new PayPalOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsPayPalOrderID()
        {
            var testObject = new PayPalOrderIdentifier("ABC-123");
            Assert.Equal("PayPalTransactionID:ABC-123", testObject.ToString());
        }

        [Fact]
        public void AuditValue_ReturnsPayPalOrderID()
        {
            var testObject = new PayPalOrderIdentifier("ABC-123");
            Assert.Equal("ABC-123", testObject.AuditValue);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
