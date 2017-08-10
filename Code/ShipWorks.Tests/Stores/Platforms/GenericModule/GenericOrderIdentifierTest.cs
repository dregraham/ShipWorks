using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.GenericModule
{
    public class GenericOrderIdentifierTest
    {
        private readonly OrderEntity order;

        public GenericOrderIdentifierTest()
        {
            order = new OrderEntity();
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorLongStringString()
        {
            var testObject = new GenericOrderIdentifier(1, "a", "b");
            testObject.ApplyTo(order);
            Assert.Equal(1, order.OrderNumber);
            Assert.Equal("a1b", order.OrderNumberComplete);
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorStringStringString()
        {
            var testObject = new GenericOrderIdentifier("1", "a", "b");
            testObject.ApplyTo(order);
            Assert.Equal(1, order.OrderNumber);
            Assert.Equal("a1b", order.OrderNumberComplete);
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorString()
        {
            var testObject = new GenericOrderIdentifier("a1b");
            testObject.ApplyTo(order);
            Assert.Equal(long.MinValue, order.OrderNumber);
            Assert.Equal("a1b", order.OrderNumberComplete);
        }
    }
}