using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Sears;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Sears
{
    public class SearsOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly SearsOrderEntity order;

        public SearsOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new SearsOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsSearsOrderID()
        {
            var testObject = new SearsOrderIdentifier(456, "ABC-123");
            Assert.Equal("ConfirmationNumber:456;PoNumber:ABC-123", testObject.ToString());
        }

        [Fact]
        public void AuditValue_ReturnsSearsOrderID()
        {
            var testObject = new SearsOrderIdentifier(456, "ABC-123");
            Assert.Equal("456;ABC-123", testObject.AuditValue);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
