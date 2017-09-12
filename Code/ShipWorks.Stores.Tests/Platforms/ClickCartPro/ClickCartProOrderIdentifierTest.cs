using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ClickCartPro;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ClickCartPro
{
    public class ClickCartProOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ClickCartProOrderEntity order;

        public ClickCartProOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new ClickCartProOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsClickCartProOrderID()
        {
            var testObject = new ClickCartProOrderIdentifier("ABC-123");
            Assert.Equal("ClickCartProOrderID:ABC-123", testObject.ToString());
        }

        [Fact]
        public void AuditValue_ReturnsClickCartProOrderID()
        {
            var testObject = new ClickCartProOrderIdentifier("ABC-123");
            Assert.Equal("ABC-123", testObject.AuditValue);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
