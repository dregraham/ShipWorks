using System.Globalization;
using ShipWorks.Shipping;
using ShipWorks.UI.ValueConverters;
using Xunit;

namespace ShipWorks.UI.Tests.ValueConverters
{
    public class IntToEnumConverterTest
    {
        private IntToEnumConverter testObject;
        
        public IntToEnumConverterTest()
        {
            testObject = new IntToEnumConverter();
        }        

        [Fact]
        public void Convert_ReturnsCorrectEnum_WhenValueIsIntAndConverterParameterIsAnEnumType()
        {
            object result = testObject.Convert(0, null, typeof(ShipmentTypeCode), CultureInfo.InvariantCulture);
            
            Assert.Equal(ShipmentTypeCode.UpsOnLineTools, result);
        }
        
        [Fact]
        public void Convert_ReturnsNullWhenValueIsNotAnInt()
        {
            object result = testObject.Convert("Not an int", null, typeof(ShipmentTypeCode), CultureInfo.InvariantCulture);
            
            Assert.Null(result);
        }
        
        [Fact]
        public void Convert_ReturnsNullWhenConverterParameterIsNotAnEnumType()
        {
            object result = testObject.Convert(0, null, "Not an enum Type", CultureInfo.InvariantCulture);
            
            Assert.Null(result);
        }

        [Fact]
        public void Convert_ReturnsNullWhenAllParametersAreNull()
        {
            object result = testObject.Convert(null, null, null, null);

            Assert.Null(result);
        }

        [Fact]
        public void ConvertBack_ReturnsCorrectInt_WhenValueIsIntAndConverterParameterIsAnEnumType()
        {
            object result = testObject.ConvertBack(0, null, typeof(ShipmentTypeCode), CultureInfo.InvariantCulture);
            
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void ConvertBack_ReturnsNullWhenValueIsNotAnInt()
        {
            object result = testObject.ConvertBack("Not an int", null, typeof(ShipmentTypeCode), CultureInfo.InvariantCulture);
            
            Assert.Null(result);
        }
        
        [Fact]
        public void ConvertBack_ReturnsNullWhenConverterParameterIsNotAnEnumType()
        {
            object result = testObject.ConvertBack(0, null, "Not an enum Type", CultureInfo.InvariantCulture);
            
            Assert.Null(result);
        }

        [Fact]
        public void ConvertBack_ReturnsNullWhenAllParametersAreNullParameterIsNotAnEnumType()
        {
            object result = testObject.ConvertBack(null, null, null, null);

            Assert.Null(result);
        }
    }
}