using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.OrderMotion;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.OrderMotion
{
    public class OrderMotionOrderIdentifierTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly OrderMotionOrderEntity order;

        public OrderMotionOrderIdentifierTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            order = new OrderMotionOrderEntity();
        }

        [Fact]
        public void ToString_ReturnsOrderMotionOrderID()
        {
            var testObject = new OrderMotionOrderIdentifier(123, 4);
            Assert.Equal("OrderMotion:123 - 4", testObject.ToString());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
