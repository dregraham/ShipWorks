using ShipWorks.Shipping;
using ShipWorks.UI.ValueConverters;
using Xunit;

namespace ShipWorks.UI.Tests.ValueConverters
{
    public class StringEmptyWhenEqualToParameterConverterTest
    {
        [Fact]
        public void Convert_ReturnsNull_WhenValueIsNull()
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = testObject.Convert(null, typeof(string), "bar", null);
            Assert.Null(result);
        }

        [Fact]
        public void Convert_ReturnsValue_WhenParameterIsNull()
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = testObject.Convert("foo", typeof(string), null, null);
            Assert.Equal("foo", result);
        }

        [Fact]
        public void ConvertBack_ReturnsNull_WhenValueIsNull()
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = testObject.ConvertBack(null, typeof(string), "foo", null);
            Assert.Null(result);
        }

        [Fact]
        public void ConvertBack_ReturnsValue_WhenParameterIsNull()
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = testObject.ConvertBack("foo", typeof(string), null, null);
            Assert.Equal("foo", result);
        }

        [Theory]
        [InlineData("foo", "foo")]
        [InlineData("1", 1)]
        [InlineData("False", false)]
        [InlineData("Other", ShipmentTypeCode.Other)]
        public void Convert_ReturnsEmpty_WhenValueEqualsParameter(object value, object parameter)
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = (string) testObject.Convert(value, typeof(string), parameter, null);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("foo", "bar")]
        [InlineData("1", 2)]
        [InlineData("False", true)]
        [InlineData("Other", ShipmentTypeCode.Usps)]
        public void Convert_ReturnsValue_WhenValueDoesNotEqualParameter(object value, object parameter)
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = (string) testObject.Convert(value, typeof(string), parameter, null);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("foo", "bar")]
        [InlineData(1, 2)]
        [InlineData(false, true)]
        [InlineData(ShipmentTypeCode.Other, ShipmentTypeCode.Usps)]
        public void ConvertBack_ReturnsValue_WhenValueDoesNotEqualEmpty(object value, object parameter)
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = testObject.ConvertBack(value, parameter.GetType(), parameter, null);
            Assert.Equal(value, result);
        }

        [Theory]
        [InlineData("bar")]
        [InlineData(2)]
        [InlineData(true)]
        [InlineData(ShipmentTypeCode.Usps)]
        public void ConvertBack_ReturnsParameter_WhenValueEqualsEmpty(object parameter)
        {
            var testObject = new StringEmptyWhenEqualToParameterConverter();
            var result = testObject.ConvertBack(string.Empty, parameter.GetType(), parameter, null);
            Assert.Equal(parameter, result);
        }
    }
}

