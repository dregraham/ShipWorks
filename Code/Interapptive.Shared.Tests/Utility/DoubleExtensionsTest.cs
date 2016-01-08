using Interapptive.Shared.Utility;
using Xunit;

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
    }
}