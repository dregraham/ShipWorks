using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Utility
{
    public class EnumHelperTest
    {
        [Fact]
        public void Details_ReturnsDetails_WhenDetailsAreAvailable()
        {
            var details = EnumHelper.GetDetails(TestEnum.HasDetails);
            Assert.Equal("Blah", details);
        }

        [Fact]
        public void Details_ReturnsEmptyString_WhenDetailsAreNotSet()
        {
            var details = EnumHelper.GetDetails(TestEnum.NoDetails);
            Assert.Equal(string.Empty, details);
        }
    }

    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum TestEnum
    {
        NoDetails = 0,

        [Description("some desc")]
        [Details("Blah")]
        HasDetails = 1,
    }
}
