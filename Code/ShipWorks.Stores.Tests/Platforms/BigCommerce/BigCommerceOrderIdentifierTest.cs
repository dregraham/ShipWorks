using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using Xunit;

namespace ShipWorks.Tests.Stores.Content
{
    public class BigCommerceOrderIdentifierTest
    {
        private readonly OrderEntity order;

        public BigCommerceOrderIdentifierTest()
        {
            order = new OrderEntity();
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorLongString()
        {
            var testObject = new BigCommerceOrderIdentifier(1, "b");
            testObject.ApplyTo(order);
            Assert.Equal(1, order.OrderNumber);
            Assert.Equal("1b", order.OrderNumberComplete);
        }
    }
}