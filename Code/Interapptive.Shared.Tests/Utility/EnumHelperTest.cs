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
            Assert.Equal(string.Empty, EnumHelper.GetDetails(TestEnum.NoDetails));
        }

        [Fact]
        public void Description_ReturnsDescription_WhenDescriptionIsAvailable()
        {
            var details = EnumHelper.GetDescription(TestEnum.HasDetails);
            Assert.Equal("some desc", details);
        }

        [Fact]
        public void Description_ThrowsException_WhenDescriptionIsNotSet()
        {
            Assert.Throws<NullReferenceException>(() => { EnumHelper.GetDescription(TestEnum.NoDetails); });
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
