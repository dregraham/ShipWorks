using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class OrderItemAttributeEntityTest
    {
        [Fact]
        public void Constructor_OrderIsSet()
        {
            var orderItem = new OrderItemEntity();
            var testObject = new OrderItemAttributeEntity(orderItem);

            Assert.Equal(orderItem, testObject.OrderItem);
        }

        [Fact]
        public void Constructor_StringFieldIsSetToBlank()
        {
            var orderItem = new OrderItemEntity();
            var testObject = new OrderItemAttributeEntity(orderItem);

            Assert.Equal(string.Empty, testObject.Description);
        }

        [Fact]
        public void Constructor_IsManualIsFalse()
        {
            var orderItem = new OrderItemEntity();
            var testObject = new OrderItemAttributeEntity(orderItem);

            Assert.False(testObject.IsManual);
        }
    }
}
