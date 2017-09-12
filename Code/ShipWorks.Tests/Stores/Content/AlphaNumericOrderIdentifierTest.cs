using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.GenericModule;
using Xunit;

namespace ShipWorks.Tests.Stores.Content
{
    public class AlphaNumericOrderIdentifierTest
    {
        private readonly OrderEntity order;

        public AlphaNumericOrderIdentifierTest()
        {
            order = new OrderEntity();
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorLongStringString()
        {
            var testObject = new AlphaNumericOrderIdentifier(1, "a", "b");
            testObject.ApplyTo(order);
            Assert.Equal(1, order.OrderNumber);
            Assert.Equal("a1b", order.OrderNumberComplete);
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorStringStringString()
        {
            var testObject = new AlphaNumericOrderIdentifier("1", "a", "b");
            testObject.ApplyTo(order);
            Assert.Equal(1, order.OrderNumber);
            Assert.Equal("a1b", order.OrderNumberComplete);
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorString()
        {
            var testObject = new AlphaNumericOrderIdentifier("a1b");
            testObject.ApplyTo(order);
            Assert.Equal(long.MinValue, order.OrderNumber);
            Assert.Equal("a1b", order.OrderNumberComplete);
        }
    }
}