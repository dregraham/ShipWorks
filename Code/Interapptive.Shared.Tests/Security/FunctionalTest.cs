using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Security
{
    public class FunctionalTest
    {
        [Theory]
        [InlineData("false", false)]
        [InlineData("FALSE", false)]
        [InlineData("False", false)]
        [InlineData("true", true)]
        [InlineData("TRUE", true)]
        [InlineData("True", true)]
        public void ParseBool_ReturnsValue_WhenStringIsBoolean(string input, bool expected)
        {
            var result = Functional.ParseBool(input).Match(x => (bool?) x, _ => null);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("foo")]
        public void ParseBool_ReturnsFailure_WhenValueIsNotBooleanString(string input)
        {
            var result = Functional.ParseBool(input).Match(x => false, _ => true);
            Assert.True(result);
        }
    }
}
