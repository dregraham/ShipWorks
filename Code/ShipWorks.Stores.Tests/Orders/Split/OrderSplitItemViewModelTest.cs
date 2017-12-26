using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderSplitItemViewModelTest
    {
        private readonly AutoMock mock;

        public OrderSplitItemViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenOrderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderSplitItemViewModel(null));
        }

        [Fact]
        public void Constructor_PopulatesProperties()
        {
            var order = new OrderItemEntity { OrderItemID = 1050, Name = "Foo", Quantity = 3 };
            order.OrderItemAttributes.Add(new OrderItemAttributeEntity { Name = "Bar", Description = "This is bar" });
            order.OrderItemAttributes.Add(new OrderItemAttributeEntity { Name = "Baz", Description = "This is baz" });

            var testObject = new OrderSplitItemViewModel(order);

            Assert.Equal(1050, testObject.OrderItemID);
            Assert.Equal("Foo", testObject.Name);
            Assert.Equal(3, testObject.OriginalQuantity);
            Assert.Equal(0, testObject.SplitQuantity);
            Assert.Contains("Bar", testObject.Attributes);
            Assert.Contains("Baz", testObject.Attributes);
        }

        [Theory]
        [InlineData(10, 4, 6)]
        [InlineData(10, 10, 0)]
        [InlineData(10, 0, 10)]
        [InlineData(10, 3.3, 6.7)]
        public void SplitQuantity_UpdatesOriginalQuantity_WhenChanged(double total, double split, double expected)
        {
            var order = new OrderItemEntity { Quantity = total };
            var testObject = new OrderSplitItemViewModel(order);

            testObject.SplitQuantity = split;

            Assert.Equal(expected, testObject.OriginalQuantity);
        }

        [Theory]
        [InlineData(10, 11, 10)]
        [InlineData(10, 10.01, 10)]
        [InlineData(10, -1, 0)]
        [InlineData(10, -0.01, 0)]
        public void SplitQuantity_ClampsSplitQuantity_WhenEnteredValueIsNotValid(double total, double split, double expected)
        {
            var order = new OrderItemEntity { Quantity = total };
            var testObject = new OrderSplitItemViewModel(order);

            testObject.SplitQuantity = split;

            Assert.Equal(expected, testObject.SplitQuantity);
        }
    }
}
