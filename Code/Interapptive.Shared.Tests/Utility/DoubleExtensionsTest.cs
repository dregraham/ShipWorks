using System;
using Interapptive.Shared.Utility;
using Xunit;
using Xunit.Sdk;

namespace Interapptive.Shared.Tests.Utility
{
    public class DoubleExtensionTest
    {
        [Theory]
        [InlineData(1, 1.1)]
        [InlineData(1, 1.01)]
        [InlineData(-1, -1.01)]
        public void IsEquivalentTo_ReturnsFalse_WhenValuesVarianceIsGreaterThanOneTousandths(double number, double comparer) =>
            Assert.False(number.IsEquivalentTo(comparer));

        [Theory]
        [InlineData(1, 1.001)]
        [InlineData(1, 1.0001)]
        [InlineData(-1, -1.0001)]
        public void IsEquivalentTo_ReturnsTrue_WhenValuesVarianceIsLessThanOneTousandths(double number, double comparer) =>
            Assert.True(number.IsEquivalentTo(comparer));

        [Theory]
        [InlineData(-1D, 0D)]
        [InlineData(0D, 0D)]
        [InlineData(1D, 1D)]
        [InlineData(9D, 9D)]
        [InlineData(10D, 10D)]
        [InlineData(11D, 10D)]
        public void Clamp_ReturnsExpectedValue(double value, double expected)
        {
            double result = value.Clamp(0, 10);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(5D, true)]
        [InlineData(0D, true)]
        [InlineData(.1D, false)]
        [InlineData(42.00001D, false)]
        [InlineData(33.999999D, false)]
        [InlineData(double.Epsilon * 100 - .1E-320, false)]
        [InlineData(double.Epsilon * 100 - .1E-321, true)]
        public void IsInt(double value, bool expected)
        {
            bool result = value.IsInt();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Clamp_ThrowsArgumentOutOfRangeException_WhenMinIsGreaterThanMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 0D.Clamp(10, 9));
        }
    }
}