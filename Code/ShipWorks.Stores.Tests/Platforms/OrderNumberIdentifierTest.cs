using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms
{
    public class OrderNumberIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly OrderEntity order;

        public OrderNumberIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new OrderEntity();
        }

        [Fact]
        public void ToString_ReturnsOrderID()
        {
            var testObject = new OrderNumberIdentifier(123);
            Assert.Equal("OrderNumber:123", testObject.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
