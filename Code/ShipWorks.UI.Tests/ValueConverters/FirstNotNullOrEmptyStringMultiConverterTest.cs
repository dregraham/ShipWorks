using ShipWorks.UI.ValueConverters;
using Xunit;

namespace ShipWorks.UI.Tests.ValueConverters
{
    public class FirstNotNullOrEmptyStringMultiConverterTest
    {
        private readonly FirstNotNullOrEmptyStringMultiConverter testObject;
        
        public FirstNotNullOrEmptyStringMultiConverterTest()
        {
            testObject = new FirstNotNullOrEmptyStringMultiConverter();
        }
        
        [Theory]
        [InlineData("Foo", new []{"Foo"})]
        [InlineData("Foo", new []{"", "Foo"})]
        [InlineData("Foo", new []{null, "Foo"})]
        [InlineData("Foo", new []{null, "Foo", "Bar"})]
        [InlineData("Foo", new []{"Foo", "Bar", ""})]
        public void Convert_ReturnsFirstNotNullOrEmptyString(string expectedResult, string[] values)
        {
            string actualResult = (string) testObject.Convert(values, null, null, null);
            
            Assert.Equal(expectedResult, actualResult);
        }
        
        [Fact]
        public void Convert_ReturnsEmptyString_WhenNoItems()
        {
            string result = (string) testObject.Convert(new object[0], null, null, null);
            
            Assert.Equal(string.Empty, result);
        }
        
        [Fact]
        public void Convert_ReturnsEmptyString_WhenAllItemsNull()
        {
            string result = (string) testObject.Convert(new object[]{null, null}, null, null, null);
            
            Assert.Equal(string.Empty, result);
        }
        
        [Fact]
        public void Convert_ReturnsEmptyString_WhenAllItemsEmptyStrings()
        {
            string result = (string) testObject.Convert(new object[]{"", ""}, null, null, null);
            
            Assert.Equal(string.Empty, result);
        }
    }
}