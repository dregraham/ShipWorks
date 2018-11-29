using ShipWorks.UI.Controls;
using Xunit;

namespace ShipWorks.Tests.UI.Controls
{
    public class LengthConverterTest
    {
        [Fact]
        public void ParseLength_ReturnsNull_WhenValueIsNull()
        {
            LengthConverter converter = new LengthConverter();

            Assert.Equal(null, converter.ParseLength(null));
        }
        
        [Fact]
        public void ParseLength_ReturnsNull_WhenValueIsInvalid()
        {
            LengthConverter converter = new LengthConverter();

            Assert.Equal(null, converter.ParseLength("foo"));
        }

        [Theory]
        [InlineData(0.0625, "0.06 in")]
        [InlineData(1.0, "1.0 in")]
        [InlineData(1.0625, "1.06 in")]
        public void FormatLength_ReturnsCorrectValue(double input, string output)
        {
            LengthConverter converter = new LengthConverter();

            Assert.Equal(output, converter.FormatLength(input));
        }

        [Theory]
        [InlineData("1inches", 1.0)]
        [InlineData("1inch", 1.0)]
        [InlineData("1in.", 1.0)]
        [InlineData("1in", 1.0)]
        [InlineData("1i", 1.0)]
        [InlineData(@"1""", 1.0)]
        [InlineData("1feet", 12.0)]
        [InlineData("1foot", 12.0)]
        [InlineData("1ft.", 12.0)]
        [InlineData("1ft", 12.0)]
        [InlineData("1f", 12.0)]
        [InlineData("1'", 12.0)]
        [InlineData("1 inches", 1.0)]
        [InlineData("1 inch", 1.0)]
        [InlineData("1 in.", 1.0)]
        [InlineData("1 in", 1.0)]
        [InlineData("1 i", 1.0)]
        [InlineData(@"1 """, 1.0)]
        [InlineData("1 feet", 12.0)]
        [InlineData("1 foot", 12.0)]
        [InlineData("1 ft.", 12.0)]
        [InlineData("1 ft", 12.0)]
        [InlineData("1 f", 12.0)]
        [InlineData("1 '", 12.0)]
        [InlineData("1 feet 1 inches", 13)]
        [InlineData("1 foot 1 inch", 13)]
        [InlineData("1 ft. 1 in.", 13)]
        [InlineData("1 ft 1 in", 13)]
        [InlineData("1 f 1 i", 13)]
        [InlineData("0 feet 1 inches", 1)]
        public void ParseLength_String_ToDouble_Test(string input, double output)
        {
            LengthConverter converter = new LengthConverter();

            Assert.Equal(output, converter.ParseLength(input));
        }
    }
}