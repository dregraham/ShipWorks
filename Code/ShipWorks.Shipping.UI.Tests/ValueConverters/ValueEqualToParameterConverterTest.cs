using ShipWorks.Shipping.UI.ValueConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ValueConverters
{
    public class ValueEqualToParameterConverterTest
    {
        [Fact]
        public void Convert_ThrowsInvalidOperationException_WhenDestinationTypeIsNotBool()
        {
            var testObject = new ValueEqualToParameterConverter(false, true);
            Assert.Throws<InvalidOperationException>(() => testObject.Convert("foo", typeof(string), "bar", null));
        }

        [Fact]
        public void Convert_ReturnsTrue_WhenInDesignMode()
        {
            var testObject = new ValueEqualToParameterConverter(false, true);
            var result = (bool)testObject.Convert("foo", typeof(bool), "bar", null);
            Assert.True(result);
        }

        [Fact]
        public void Convert_ReturnsFalse_WhenValueIsNull()
        {
            var testObject = new ValueEqualToParameterConverter(false, false);
            var result = (bool)testObject.Convert(null, typeof(bool), "foo", null);
            Assert.False(result);
        }

        [Fact]
        public void Convert_ReturnsFalse_WhenParameterIsNull()
        {
            var testObject = new ValueEqualToParameterConverter(false, false);
            var result = (bool)testObject.Convert("foo", typeof(bool), null, null);
            Assert.False(result);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData(1)]
        [InlineData(false)]
        [InlineData(ShipmentTypeCode.Other)]
        public void Convert_ReturnsTrue_WhenValueEqualsParameter(object value)
        {
            var testObject = new ValueEqualToParameterConverter(false, false);
            var result = (bool)testObject.Convert(value, typeof(bool), value, null);
            Assert.True(result);
        }

        [Theory]
        [InlineData("foo")]
        [InlineData(1)]
        [InlineData(false)]
        [InlineData(ShipmentTypeCode.Other)]
        public void Convert_ReturnsFalse_WhenValueEqualsParameterAndInvertIsTrue(object value)
        {
            var testObject = new ValueEqualToParameterConverter(true, false);
            var result = (bool)testObject.Convert(value, typeof(bool), value, null);
            Assert.False(result);
        }

        [Theory]
        [InlineData("foo", "bar")]
        [InlineData(1, 2)]
        [InlineData(false, true)]
        [InlineData(ShipmentTypeCode.Other, ShipmentTypeCode.Usps)]
        public void Convert_ReturnsFalse_WhenValueDoesNotEqualParameter(object value1, object value2)
        {
            var testObject = new ValueEqualToParameterConverter(false, false);
            var result = (bool)testObject.Convert(value1, typeof(bool), value2, null);
            Assert.False(result);
        }

        [Theory]
        [InlineData("foo", "bar")]
        [InlineData(1, 2)]
        [InlineData(false, true)]
        [InlineData(ShipmentTypeCode.Other, ShipmentTypeCode.Usps)]
        public void Convert_ReturnsTrue_WhenValueDoeNotEqualParameterAndInvertIsTrue(object value1, object value2)
        {
            var testObject = new ValueEqualToParameterConverter(true, false);
            var result = (bool)testObject.Convert(value1, typeof(bool), value2, null);
            Assert.True(result);
        }
    }
}
