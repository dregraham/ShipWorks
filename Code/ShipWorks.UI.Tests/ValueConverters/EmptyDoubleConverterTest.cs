using System.Globalization;
using ShipWorks.Shipping;
using ShipWorks.UI.ValueConverters;
using Xunit;

namespace ShipWorks.UI.Tests.ValueConverters
{
    public class EmptyDoubleConverterTest
    {
        [Theory]
        [InlineData(null, 0)]
        [InlineData(null, 1.23)]
        [InlineData(null, 1523.23)]
        [InlineData(null, "1523.23")]
        [InlineData(null, -1.23)]
        [InlineData(null, -1523.23)]
        [InlineData(null, "-1523.23")]
        public void Convert_ReturnsDefaultValue_WhenValueIsNull(object value, object parameter)
        {
            double defaultValue = 0D;
            if (!string.IsNullOrWhiteSpace(parameter?.ToString()))
            {
                double.TryParse(parameter.ToString(), out defaultValue);
            }

            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.Convert(value, typeof(double), parameter, null);
            Assert.Equal(defaultValue, result);
        }

        [Theory]
        [InlineData("", 0)]
        [InlineData("", 1.23)]
        [InlineData("", 1523.23)]
        [InlineData("", "1523.23")]
        [InlineData("", -1.23)]
        [InlineData("", -1523.23)]
        [InlineData("", "-1523.23")]
        public void Convert_ReturnsDefaultValue_WhenValueIsBlankString(object value, object parameter)
        {
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.Convert(string.Empty, typeof(double), null, null);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Convert_Returns0_WhenValueIsDobuleAndIsDefault()
        {
            double value = default(double);
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.Convert(value, typeof(double), null, null);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Convert_Returns0_WhenValueIsDecimalAndIsDefault()
        {
            decimal value = default(decimal);
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.Convert(value, typeof(double), null, null);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Convert_ReturnsFiveDollars_WhenValueFive()
        {
            decimal value = 5.00M;
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.Convert(value, typeof(double), null, null);
            Assert.Equal(5.00, result);
        }

        [Theory]
        [InlineData("1501.23")]
        [InlineData("1,501.23")]
        public void Convert_ReturnsCorrectDoubleValue_WhenAString(string value)
        {
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.Convert(value, typeof(double), null, null);
            Assert.Equal(1501.23, result);
        }

        [Theory]
        [InlineData("-1501.23")]
        [InlineData("-1,501.23")]
        public void Convert_ReturnsCorrectDoubleValue_WhenNegativeAndAString(string value)
        {
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.Convert(value, typeof(double), null, null);
            Assert.Equal(-1501.23, result);
        }

        [Fact]
        public void ConvertBack_Returns0_WhenValueIsNull()
        {
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.ConvertBack(null, typeof(double), null, null);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ConvertBack_ReturnsDefaultValueOfParameter_WhenValueIsNull()
        {
            double defaultValue = 3.3;

            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.ConvertBack(null, typeof(double), defaultValue, null);
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void ConvertBack_Returns0_WhenValueIsBlankString()
        {
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.ConvertBack(string.Empty, typeof(double), null, CultureInfo.CurrentCulture);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ConvertBack_ReturnsDefaultValueOfParameter_WhenValueIsBlankString()
        {
            double defaultValue = 3.3;

            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double result = (double)testObject.ConvertBack(string.Empty, typeof(double), defaultValue, CultureInfo.CurrentCulture);
            Assert.Equal(defaultValue, result);
        }

        [Theory]
        [InlineData("1501.23", null)]
        [InlineData("1,501.23", null)]
        [InlineData("1501.23", 0)]
        [InlineData("1,501.23", 0)]
        [InlineData("1501.23", 3.3)]
        [InlineData("1,501.23", 3.3)]
        public void ConvertBack_ReturnsCorrectDoubleValue_WhenAString(string value, double defaultValue)
        {
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double convertResult = (double)testObject.Convert(value, typeof(double), defaultValue, null);
            double convertBackResult = (double)testObject.ConvertBack(convertResult, typeof(double), defaultValue, CultureInfo.CurrentCulture);
            Assert.Equal(convertBackResult, convertResult);
        }

        [Theory]
        [InlineData("-1501.23", null)]
        [InlineData("-1,501.23", null)]
        [InlineData("-1501.23", 0)]
        [InlineData("-1,501.23", 0)]
        [InlineData("-1501.23", -3.3)]
        [InlineData("-1,501.23", -3.3)]
        public void ConvertBack_ReturnsCorrectDoubleValue_WhenNegativeAndAString(string value, double defaultValue)
        {
            EmptyDoubleConverter testObject = new EmptyDoubleConverter();
            double convertResult = (double)testObject.Convert(value, typeof(double), defaultValue, null);
            double convertBackResult = (double)testObject.ConvertBack(convertResult, typeof(double), defaultValue, CultureInfo.CurrentCulture);
            Assert.Equal(convertBackResult, convertResult);
        }
    }
}
