using System.Globalization;
using ShipWorks.UI.ValueConverters;
using Xunit;

namespace ShipWorks.UI.Tests.ValueConverters
{
    public class EmptyMoneyConverterTest
    {
        private readonly string zeroDollars = 0.ToString("C", CultureInfo.CurrentCulture);
        private readonly string fiveDollars = (5.00M).ToString("C", CultureInfo.CurrentCulture);

        [Fact]
        public void Convert_ReturnsZeroDollars_WhenValueIsNull()
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string result = testObject.Convert(null, typeof(string), null, null).ToString();
            Assert.Equal(zeroDollars, result);
        }

        [Fact]
        public void Convert_ReturnsZeroDollars_WhenValueIsBlankString()
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string result = testObject.Convert(string.Empty, typeof(string), null, null).ToString();
            Assert.Equal(zeroDollars, result);
        }

        [Fact]
        public void Convert_ReturnsZeroDollars_WhenValueIsDobuleAndIsDefault()
        {
            double value = default(double);
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string result = testObject.Convert(value, typeof(string), null, null).ToString();
            Assert.Equal(zeroDollars, result);
        }

        [Fact]
        public void Convert_ReturnsZeroDollars_WhenValueIsDecimalAndIsDefault()
        {
            decimal value = default(decimal);
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string result = testObject.Convert(value, typeof(string), null, null).ToString();
            Assert.Equal(zeroDollars, result);
        }

        [Fact]
        public void Convert_ReturnsFiveDollars_WhenValueFive()
        {
            decimal value = 5.00M;
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string result = testObject.Convert(value, typeof(string), null, null).ToString();
            Assert.Equal(fiveDollars, result);
        }

        [Theory]
        [InlineData("1501.23")]
        [InlineData("1,501.23")]
        [InlineData("$1501.23")]
        [InlineData("$1,501.23")]
        public void Convert_ReturnsCorrectDecimalValue_WhenAString(string value)
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string result = testObject.Convert(value, typeof(string), null, null).ToString();
            Assert.Equal((1501.23M).ToString("C", CultureInfo.CurrentCulture), result);
        }

        [Theory]
        [InlineData("-1501.23")]
        [InlineData("-1,501.23")]
        [InlineData("-$1501.23")]
        [InlineData("-$1,501.23")]
        public void Convert_ReturnsCorrectDecimalValue_WhenNegativeAndAString(string value)
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string result = testObject.Convert(value, typeof(string), null, null).ToString();
            Assert.Equal((-1501.23M).ToString("C", CultureInfo.CurrentCulture), result);
        }

        [Fact]
        public void ConvertBack_ReturnsZeroDollars_WhenValueIsNull()
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            decimal result = (decimal) testObject.ConvertBack(null, typeof(decimal), null, null);
            Assert.Equal(0M, result);
        }

        [Fact]
        public void ConvertBack_ReturnsZeroDollars_WhenValueIsBlankString()
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            decimal result = (decimal) testObject.ConvertBack(string.Empty, typeof(decimal), null, CultureInfo.CurrentCulture);
            Assert.Equal(0M, result);
        }

        [Theory]
        [InlineData("1501.23")]
        [InlineData("1,501.23")]
        [InlineData("$1501.23")]
        [InlineData("$1,501.23")]
        public void ConvertBack_ReturnsCorrectDecimalValue_WhenAString(string value)
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string convertResult = testObject.Convert(value, typeof(string), null, null).ToString();
            decimal convertBackResult = (decimal) testObject.ConvertBack(convertResult, typeof(decimal), null, CultureInfo.CurrentCulture);
            Assert.Equal(convertBackResult.ToString("C", CultureInfo.CurrentCulture), convertResult);
        }

        [Theory]
        [InlineData("-1501.23")]
        [InlineData("-1,501.23")]
        [InlineData("-$1501.23")]
        [InlineData("-$1,501.23")]
        public void ConvertBack_ReturnsCorrectDecimalValue_WhenNegativeAndAString(string value)
        {
            EmptyMoneyConverter testObject = new EmptyMoneyConverter();
            string convertResult = testObject.Convert(value, typeof(string), null, null).ToString();
            decimal convertBackResult = (decimal) testObject.ConvertBack(convertResult, typeof(decimal), null, CultureInfo.CurrentCulture);
            Assert.Equal(convertBackResult.ToString("C", CultureInfo.CurrentCulture), convertResult);
        }
    }
}
