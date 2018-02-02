using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Extensions
{
    public class DecimalExtensionsTest
    {
        [Theory]
        [InlineData(2, 6, 2, 2)]
        [InlineData(2, 6, 1, 2)]
        [InlineData(2, 6, 3, 3)]
        [InlineData(2, 6, 6, 6)]
        [InlineData(2, 6, 7, 6)]
        [InlineData(2, 6, 4, 4)]
        [InlineData(6, 2, 2, 2)]
        [InlineData(6, 2, 1, 2)]
        [InlineData(6, 2, 3, 3)]
        [InlineData(6, 2, 6, 6)]
        [InlineData(6, 2, 7, 6)]
        [InlineData(6, 2, 4, 4)]
        public void Clamp_ClampsValue_ToRange(decimal min, decimal max, decimal value, decimal expected) =>
            Assert.Equal(expected, value.Clamp(min, max));
    }
}
