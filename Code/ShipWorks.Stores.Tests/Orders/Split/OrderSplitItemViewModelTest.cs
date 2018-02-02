using System;
using System.Collections.Generic;
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
            Assert.Equal(0, testObject.SplitQuantityValue);
            Assert.Contains(new KeyValuePair<string, string>("Bar", "This is bar"), testObject.Attributes);
            Assert.Contains(new KeyValuePair<string, string>("Baz", "This is baz"), testObject.Attributes);
        }

        [Theory]
        [InlineData(10, 0.01, "0.01")]
        [InlineData(10, 5, "5")]
        [InlineData(10, 5.009, "5.01")]
        [InlineData(-10, -3, "-3")]
        [InlineData(-10, -3.05, "-3.05")]
        public void SplitQuantity_GetsFormattedValue(decimal total, decimal input, string expected)
        {
            var order = new OrderItemEntity { Quantity = (double) total };
            var testObject = new OrderSplitItemViewModel(order) { SplitQuantityValue = input };
            Assert.Equal(expected, testObject.SplitQuantity);
        }

        [Theory]
        [InlineData(10, 4, 6)]
        [InlineData(10, 10, 0)]
        [InlineData(10, 0, 10)]
        [InlineData(10, 3.3, 6.7)]
        public void SplitQuantity_UpdatesOriginalQuantity_WhenChanged(decimal total, decimal split, decimal expected)
        {
            var order = new OrderItemEntity { Quantity = (double) total };
            var testObject = new OrderSplitItemViewModel(order);

            testObject.SplitQuantity = split.ToString();

            Assert.Equal(expected, testObject.OriginalQuantity);
        }

        [Theory]
        [InlineData(10, 11, 10)]
        [InlineData(10, 10.01, 10)]
        [InlineData(10, -1, 0)]
        [InlineData(10, -0.01, 0)]
        public void SplitQuantity_ClampsSplitQuantity_WhenEnteredValueIsNotValid(decimal total, decimal split, decimal expected)
        {
            var order = new OrderItemEntity { Quantity = (double) total };
            var testObject = new OrderSplitItemViewModel(order);

            testObject.SplitQuantity = split.ToString();

            Assert.Equal(expected, testObject.SplitQuantityValue);
        }

        [Fact]
        public void SplitQuantity_ResetsValue_WhenInputIsNotValidDecimal()
        {
            var order = new OrderItemEntity { Quantity = 10 };
            var testObject = new OrderSplitItemViewModel(order) { SplitQuantity = "6" };
            testObject.SplitQuantity = "Foo";
            Assert.Equal(6, testObject.SplitQuantityValue);
        }

        [Theory]
        [InlineData(10, 4, 5, 5)]
        [InlineData(10, 10, 0, 10)]
        [InlineData(10, 0, 9, 1)]
        [InlineData(10, 3.3, 5.7, 4.3)]
        public void Increment_SetsValuesCorrectly(decimal total, decimal split, decimal expectedOriginal, decimal expectedSplit)
        {
            var order = new OrderItemEntity { Quantity = (double) total };
            var testObject = new OrderSplitItemViewModel(order) { SplitQuantity = split.ToString() };
            testObject.Increment.Execute(null);
            Assert.Equal(expectedOriginal, testObject.OriginalQuantity);
            Assert.Equal(expectedSplit, testObject.SplitQuantityValue);
        }

        [Theory]
        [InlineData(10, 10, false)]
        [InlineData(10, 9, true)]
        [InlineData(10, 9.99, true)]
        [InlineData(-10, -6, true)]
        [InlineData(-10, -0.99, true)]
        [InlineData(-10, 0, false)]
        public void Increment_CanExecute_ReturnsCorrectValue(decimal total, decimal split, bool expected)
        {
            var order = new OrderItemEntity { Quantity = (double) total };
            var testObject = new OrderSplitItemViewModel(order) { SplitQuantityValue = split };
            var result = testObject.Increment.CanExecute(null);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(6, 4, 7, 3)]
        [InlineData(10, 0, 10, 0)]
        [InlineData(0, 10, 1, 9)]
        [InlineData(6.7, 3.3, 7.7, 2.3)]
        public void Decrement_SetsValuesCorrectly(decimal original, decimal split, decimal expectedOriginal, decimal expectedSplit)
        {
            var order = new OrderItemEntity { Quantity = (double) (original + split) };
            var testObject = new OrderSplitItemViewModel(order) { SplitQuantity = split.ToString() };
            testObject.Decrement.Execute(null);
            Assert.Equal(expectedOriginal, testObject.OriginalQuantity);
            Assert.Equal(expectedSplit, testObject.SplitQuantityValue);
        }

        [Theory]
        [InlineData(10, 0, false)]
        [InlineData(10, 1, true)]
        [InlineData(10, 0.01, true)]
        [InlineData(-10, -6, true)]
        [InlineData(-10, -9.99, true)]
        [InlineData(-10, -10, false)]
        public void Decrement_CanExecute_ReturnsCorrectValue(decimal total, decimal split, bool expected)
        {
            var order = new OrderItemEntity { Quantity = (double) total };
            var testObject = new OrderSplitItemViewModel(order) { SplitQuantityValue = split };
            var result = testObject.Decrement.CanExecute(null);
            Assert.Equal(expected, result);
        }
    }
}
