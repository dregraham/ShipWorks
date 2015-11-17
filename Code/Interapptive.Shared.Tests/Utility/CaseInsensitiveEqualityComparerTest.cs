using Interapptive.Shared.Utility;
using Xunit;

namespace Interapptive.Shared.Tests.Utility
{
    public class CaseInsensitiveEqualityComparerTest
    {
        [Theory]
        [InlineData("foo", "FOO")]
        [InlineData("FOO", "foo")]
        [InlineData("fOo", "FoO")]
        public void Equals_ReturnsTrue_WhenValuesDifferOnlyByCase(string left, string right)
        {
            CaseInsensitiveEqualityComparer testObject = new CaseInsensitiveEqualityComparer();
            bool result = testObject.Equals(left, right);
            Assert.True(result);
        }

        [Theory]
        [InlineData("FOO", "FOO")]
        [InlineData("foo", "foo")]
        [InlineData("FoO", "FoO")]
        [InlineData("", "")]
        public void Equals_ReturnsTrue_WhenValuesAreIdentical(string left, string right)
        {
            CaseInsensitiveEqualityComparer testObject = new CaseInsensitiveEqualityComparer();
            bool result = testObject.Equals(left, right);
            Assert.True(result);
        }

        [Theory]
        [InlineData("foo", "bar")]
        [InlineData("", "foo")]
        [InlineData("fOo", "")]
        public void Equals_ReturnsFalse_WhenValuesAreDifferent(string left, string right)
        {
            CaseInsensitiveEqualityComparer testObject = new CaseInsensitiveEqualityComparer();
            bool result = testObject.Equals(left, right);
            Assert.False(result);
        }

        [Theory]
        [InlineData(null, "foo")]
        [InlineData("fOo", null)]
        public void Equals_ReturnsFalse_WhenOneValueIsNull(string left, string right)
        {
            CaseInsensitiveEqualityComparer testObject = new CaseInsensitiveEqualityComparer();
            bool result = testObject.Equals(left, right);
            Assert.False(result);
        }

        [Fact]
        public void Equals_ReturnsTrue_BothValuesAreNull()
        {
            CaseInsensitiveEqualityComparer testObject = new CaseInsensitiveEqualityComparer();
            bool result = testObject.Equals(null, null);
            Assert.True(result);
        }
    }
}
