using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using Xunit;

namespace Interapptive.Shared.Tests.Win32
{
    public class RegistryHelperTest
    {
        [Theory]
        [InlineData(null, 10, 10)]
        [InlineData("10", 0, 10)]

        public void GetValue_T_ReturnsCorrectIntValue(object value, int defaultValue, int expectedResult)
        {
            var result = RegistryHelper.GetValue(value, defaultValue);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, 10.1, 10.1)]
        [InlineData("10.1", 0.1, 10.1)]

        public void GetValue_T_ReturnsCorrectDoubleValue(object value, double defaultValue, double expectedResult)
        {
            var result = RegistryHelper.GetValue(value, defaultValue);

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "10.1", "10.1")]
        [InlineData("10.1", "0.1", "10.1")]

        public void GetValue_T_ReturnsCorrectStringValue(object value, string defaultValue, string expectedResult)
        {
            var result = RegistryHelper.GetValue(value, defaultValue);

            Assert.Equal(expectedResult, result);
        }
    }
}
