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
            Assert.Equal(0, testObject.SplitAmount);
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

            testObject.SplitAmount = split;

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

            testObject.SplitAmount = split;

            Assert.Equal(expected, testObject.SplitAmount);
        }
    }
}
