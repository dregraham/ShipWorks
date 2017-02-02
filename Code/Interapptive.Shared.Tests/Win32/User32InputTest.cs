using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Win32;
using Interapptive.Shared.Win32.Native;
using Xunit;
using Xunit.Sdk;

namespace Interapptive.Shared.Tests.Win32
{
    public class User32InputTest
    {
        [Theory]
        [InlineData(VirtualKeys.B, false, false, false, "b")]
        [InlineData(VirtualKeys.B, false, false, true, "b")]
        [InlineData(VirtualKeys.B, false, true, false, "\u0002")]
        [InlineData(VirtualKeys.B, false, true, true, "\u0002")]
        [InlineData(VirtualKeys.B, true, false, false, "B")]
        [InlineData(VirtualKeys.B, true, false, true, "B")]
        [InlineData(VirtualKeys.B, true, true, false, "\u0002")]
        [InlineData(VirtualKeys.B, true, true, true, "\u0002")]
        public void GetCharactersFromKeys_ReturnsCorrectValue(VirtualKeys key, bool shift, bool control, bool altGr, string expectedResult)
        {
            User32Input user32Input = new User32Input();

            string result = user32Input.GetCharactersFromKeys(VirtualKeys.B, shift, control, altGr);

            Assert.Equal(expectedResult, result);
        }
    }
}
