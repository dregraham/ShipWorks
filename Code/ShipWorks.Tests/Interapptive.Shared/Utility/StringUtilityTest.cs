using System.Collections.Generic;
using Interapptive.Shared.Utility;
using Xunit;

namespace ShipWorks.Tests.Interapptive.Shared.Utility
{
    public class StringUtilityTest
    {
        [Fact]
        public void Truncate_ReturnsNull_WhenInputIsNull()
        {
            string result = ((string) null).Truncate(20);
            Assert.Null(result);
        }

        [Fact]
        public void Truncate_ReturnsEmptyString_WhenInputIsEmpty()
        {
            string result = string.Empty.Truncate(20);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Truncate_ReturnsOriginalString_WhenInputEqualsMaxLength()
        {
            string input = new string('x', 20);
            string result = input.Truncate(20);
            Assert.Same(input, result);
        }

        [Fact]
        public void Truncate_ReturnsTruncatedString_WhenInputIsLongerThanMaxLength()
        {
            string result = "Foobar".Truncate(3);
            Assert.Equal("Foo", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsCurrencyString_WhenCentsAreWhole()
        {
            string result = StringUtility.FormatFriendlyCurrency(.45M);
            Assert.Equal("$0.45", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsCurrencyStringWithHalf_WhenCentsHaveHalfValue()
        {
            string result = StringUtility.FormatFriendlyCurrency(.455M);
            Assert.Equal("$0.45\u00bd", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsRoundedUpCent_WhenCentsHaveOverHalfCent()
        {
            string result = StringUtility.FormatFriendlyCurrency(.456M);
            Assert.Equal("$0.46", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsRoundedDownCent_WhenCentsHaveLessHalfCent()
        {
            string result = StringUtility.FormatFriendlyCurrency(.452M);
            Assert.Equal("$0.45", result);
        }

        [Theory]
        [InlineData(10, "{0:#,##0}", "10 Bytes")]
        [InlineData(1023, "{0:#,##0}", "1,023 Bytes")]
        [InlineData(1024, "{0:#,##0}", "1 KB")]
        [InlineData(20971519, "{0:#,##0}", "20,480 KB")]
        [InlineData(20971520, "{0:#,##0}", "20 MB")]
        [InlineData(1073741823, "{0:#,##0}", "1,024 MB")]
        [InlineData(1073741824, "{0:#,##0}", "1 GB")]
        [InlineData(1288490188.8, "{0:#,##0}", "1 GB")]
        [InlineData(1610612736, "{0:#,##0}", "2 GB")]
        [InlineData(10, "{0:#,##0.00}", "10.00 Bytes")]
        [InlineData(1023, "{0:#,##0.00}", "1,023.00 Bytes")]
        [InlineData(1024, "{0:#,##0.00}", "1.00 KB")]
        [InlineData(20971519, "{0:#,##0.00}", "20,480.00 KB")]
        [InlineData(20971520, "{0:#,##0.00}", "20.00 MB")]
        [InlineData(1073741823, "{0:#,##0.00}", "1,024.00 MB")]
        [InlineData(1073741824, "{0:#,##0.00}", "1.00 GB")]
        [InlineData(1288490188.8, "{0:#,##0.00}", "1.20 GB")]
        [InlineData(1610612736, "{0:#,##0.00}", "1.50 GB")]
        public void FormatByteCount_ReturnsFriendlyValue(long byteSize, string format, string expectedResult)
        {
            string result = StringUtility.FormatByteCount(byteSize, format);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ToInsecureString_ReturnsOriginalString_WhenReceivesSecureStringFromToSecureString()
        {
            string originalString = "The original string";
            var secureString = originalString.ToSecureString();
            string result = secureString.ToInsecureString();

            Assert.Equal(originalString, result);
        }

        [Fact]
        public void RemoveSymbols_RemovesRegisteredTrademarkSymbol()
        {
            string testObject = "FedEx Priority Overnight®";

            Assert.Equal("FedEx Priority Overnight", testObject.RemoveSymbols());
        }

        [Fact]
        public void RemoveSymbols_DoesNotRemovePunctuation()
        {
            string testObject = ".()-";

            Assert.Equal(testObject, testObject.RemoveSymbols());
        }
    }
}
