using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderSplitChargeViewModelTest
    {
        private readonly AutoMock mock;

        public OrderSplitChargeViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenOrderIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OrderSplitChargeViewModel(null));
        }

        [Fact]
        public void Constructor_PopulatesProperties()
        {
            var order = new OrderChargeEntity { OrderChargeID = 1050, Type = "Foo", Amount = 3 };

            var testObject = new OrderSplitChargeViewModel(order);

            Assert.Equal(1050, testObject.OrderChargeID);
            Assert.Equal("Foo", testObject.Type);
            Assert.Equal(3, testObject.OriginalAmount);
            Assert.Equal(0, testObject.SplitAmountValue);
        }

        [Theory]
        [InlineData(10, 0.01, "$0.01")]
        [InlineData(10, 5, "$5.00")]
        [InlineData(10, 5.009, "$5.01")]
        [InlineData(-10, -3, "($3.00)")]
        public void SplitAmount_GetsFormattedValue(decimal total, decimal input, string expected)
        {
            var order = new OrderChargeEntity { Amount = total };
            var testObject = new OrderSplitChargeViewModel(order) { SplitAmountValue = input };

            Assert.Equal(expected, testObject.SplitAmount);
        }

        [Theory]
        [InlineData(10, 4, 6)]
        [InlineData(10, 10, 0)]
        [InlineData(10, 0, 10)]
        [InlineData(10, 3.3, 6.7)]
        public void SplitAmount_UpdatesOriginalAmount_WhenChanged(decimal total, decimal split, decimal expected)
        {
            var order = new OrderChargeEntity { Amount = total };
            var testObject = new OrderSplitChargeViewModel(order);

            testObject.SplitAmount = split.ToString();

            Assert.Equal(expected, testObject.OriginalAmount);
        }

        [Theory]
        [InlineData(10, 11, 10)]
        [InlineData(10, 10.01, 10)]
        [InlineData(10, -1, 0)]
        [InlineData(10, -0.01, 0)]
        public void SplitAmount_ClampsSplitAmount_WhenEnteredAmountIsNotValid(decimal total, decimal split, decimal expected)
        {
            var order = new OrderChargeEntity { Amount = total };
            var testObject = new OrderSplitChargeViewModel(order);

            testObject.SplitAmount = split.ToString();

            Assert.Equal(expected, testObject.SplitAmountValue);
        }

        [Fact]
        public void SplitAmount_ResetsValue_WhenInputIsNotValidDecimal()
        {
            var order = new OrderChargeEntity { Amount = 10 };
            var testObject = new OrderSplitChargeViewModel(order) { SplitAmount = "6" };

            testObject.SplitAmount = "Foo";

            Assert.Equal(6, testObject.SplitAmountValue);
        }

        [Theory]
        [InlineData(10, 4, 5, 5)]
        [InlineData(10, 10, 0, 10)]
        [InlineData(10, 0, 9, 1)]
        [InlineData(10, 3.3, 5.7, 4.3)]
        public void Increment_SetsValuesCorrectly(decimal total, decimal split, decimal expectedOriginal, decimal expectedSplit)
        {
            var order = new OrderChargeEntity { Amount = total };
            var testObject = new OrderSplitChargeViewModel(order) { SplitAmount = split.ToString() };

            testObject.Increment.Execute(null);

            Assert.Equal(expectedOriginal, testObject.OriginalAmount);
            Assert.Equal(expectedSplit, testObject.SplitAmountValue);
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
            var order = new OrderChargeEntity { Amount = total };
            var testObject = new OrderSplitChargeViewModel(order) { SplitAmountValue = split };

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
            var order = new OrderChargeEntity { Amount = original + split };
            var testObject = new OrderSplitChargeViewModel(order) { SplitAmount = split.ToString() };

            testObject.Decrement.Execute(null);

            Assert.Equal(expectedOriginal, testObject.OriginalAmount);
            Assert.Equal(expectedSplit, testObject.SplitAmountValue);
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
            var order = new OrderChargeEntity { Amount = total };
            var testObject = new OrderSplitChargeViewModel(order) { SplitAmountValue = split };

            var result = testObject.Decrement.CanExecute(null);

            Assert.Equal(expected, result);
        }
    }
}
