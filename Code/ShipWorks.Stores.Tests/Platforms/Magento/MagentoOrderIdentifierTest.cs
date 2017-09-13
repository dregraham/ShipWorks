using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento;
using Xunit;

namespace ShipWorks.Tests.Stores.Content
{
    public class MagentoOrderIdentifierTest
    {
        private readonly OrderEntity order;

        public MagentoOrderIdentifierTest()
        {
            order = new OrderEntity();
        }

        [Fact]
        public void ApplyTo_SetsOrderCorrectly_WhenUsingCtorLongStringString()
        {
            var testObject = new MagentoOrderIdentifier(1, "a", "b");
            testObject.ApplyTo(order);
            Assert.Equal(1, order.OrderNumber);
            Assert.Equal("a1b", order.OrderNumberComplete);
        }

        [Fact]
        public void AuditValue_ReturnsCorrectValue_WhenUsingCtorLongStringString()
        {
            var testObject = new MagentoOrderIdentifier(1, "a", "b");
            Assert.Equal("a1b", testObject.AuditValue);
        }
    }
}