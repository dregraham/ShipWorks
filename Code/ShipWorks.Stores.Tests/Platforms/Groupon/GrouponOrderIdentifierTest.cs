using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Groupon;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Groupon
{
    public class GrouponOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly GrouponOrderEntity order;

        public GrouponOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new GrouponOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsGrouponOrderID()
        {
            var testObject = new GrouponOrderIdentifier("ABC-123");
            Assert.Equal("GrouponStoreOrderID:ABC-123", testObject.ToString());
        }

        [Fact]
        public void AuditValue_ReturnsGrouponOrderID()
        {
            var testObject = new GrouponOrderIdentifier("ABC-123");
            Assert.Equal("ABC-123", testObject.AuditValue);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
