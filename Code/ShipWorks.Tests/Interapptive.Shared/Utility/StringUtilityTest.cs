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
            Assert.IsNull(result);
        }

        [Fact]
        public void Truncate_ReturnsEmptyString_WhenInputIsEmpty()
        {
            string result = string.Empty.Truncate(20);
            Assert.AreEqual(string.Empty, result);
        }

        [Fact]
        public void Truncate_ReturnsOriginalString_WhenInputEqualsMaxLength()
        {
            string input = new string('x', 20);
            string result = input.Truncate(20);
            Assert.AreSame(input, result);
        }

        [Fact]
        public void Truncate_ReturnsTruncatedString_WhenInputIsLongerThanMaxLength()
        {
            string result = "Foobar".Truncate(3);
            Assert.AreEqual("Foo", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsCurrencyString_WhenCentsAreWhole()
        {
            string result = StringUtility.FormatFriendlyCurrency(.45M);
            Assert.AreEqual("$0.45", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsCurrencyStringWithHalf_WhenCentsHaveHalfValue()
        {
            string result = StringUtility.FormatFriendlyCurrency(.455M);
            Assert.AreEqual("$0.45\u00bd", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsRoundedUpCent_WhenCentsHaveOverHalfCent()
        {
            string result = StringUtility.FormatFriendlyCurrency(.456M);
            Assert.AreEqual("$0.46", result);
        }

        [Fact]
        public void FormatFriendlyCurrenty_ReturnsRoundedDownCent_WhenCentsHaveLessHalfCent()
        {
            string result = StringUtility.FormatFriendlyCurrency(.452M);
            Assert.AreEqual("$0.45", result);
        }
    }
}
