using System;
using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Extensions
{
    public class DateTimeExtensionsTest
    {
        [Theory]
        [InlineData(2, 2, 2)]
        [InlineData(2, 1, 2)]
        [InlineData(2, 3, 3)]
        public void AtLeast_ClampsValue_ToMinimum(int minDay, int valueDate, int expectedDay)
        {
            var min = new DateTime(2000, 1, minDay);
            var value = new DateTime(2000, 1, valueDate);
            var expected = new DateTime(2000, 1, expectedDay);

            Assert.Equal(expected, value.AtLeast(min));
        }

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
        public void Clamp_ClampsValue_ToRange(int minDay, int maxText, int valueDate, int expectedDay)
        {
            var min = new DateTime(2000, 1, minDay);
            var max = new DateTime(2000, 1, maxText);
            var value = new DateTime(2000, 1, valueDate);
            var expected = new DateTime(2000, 1, expectedDay);

            Assert.Equal(expected, value.Clamp(min, max));
        }
    }
}
