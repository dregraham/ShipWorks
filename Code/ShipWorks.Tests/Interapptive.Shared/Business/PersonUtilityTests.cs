using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Business
{
    public class PersonUtilityTests
    {
        [Fact]
        public void GetPhoneDigits10_RemovesLeading1_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("13145551212");
            Assert.Equal("3145551212", result);
            Assert.Equal(10, result.Length);
        }

        [Fact]
        public void GetPhoneDigits10_LeavesLeading0_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("03145551212");
            Assert.Equal("0314555121", result);
            Assert.Equal(10, result.Length);
        }

        [Fact]
        public void GetPhoneDigits10_ReturnsFullValue_WhenLengthLessThan10_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("1551212");
            Assert.Equal("1551212", result);
            Assert.Equal(7, result.Length);
        }

        [Fact]
        public void GetPhoneDigits10_ReturnsOnlyDigits_WhenNonAlphasInTest_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("!1~5#5$1%2^&1*2())");
            Assert.Equal("1551212", result);
            Assert.Equal(7, result.Length);
        }

        [Fact]
        public void GetPhoneDigits10_ConvertsKeypadAlphasToDigits_Test()
        {
            string result = PersonUtility.GetPhoneDigits10("1jk1a1a");
            Assert.Equal("1551212", result);
            Assert.Equal(7, result.Length);
        }

        [Fact]
        public void GetPhoneDigits_RemovesLeading1_Test()
        {
            string result = PersonUtility.GetPhoneDigits("13145551212", 10, false);
            Assert.Equal("3145551212", result);
            Assert.Equal(10, result.Length);
        }

        [Fact]
        public void GetPhoneDigits_Returns15Characters_WhenOver15Requested_Test()
        {
            string result = PersonUtility.GetPhoneDigits("01234567890123456789", 15, false);
            Assert.Equal("012345678901234", result);
            Assert.Equal(15, result.Length);
        }

    }
}
