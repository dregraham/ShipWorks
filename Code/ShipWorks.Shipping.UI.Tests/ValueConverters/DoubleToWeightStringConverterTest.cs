using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.ValueConverters;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ValueConverters
{
    public class DoubleToWeightStringConverterTest
    {
        [Fact]
        public void Convert_Null_ThrowsInvalidOperationException_Test()
        {
            DoubleToWeightStringConverter converter = new DoubleToWeightStringConverter();

            Exception ex = Assert.Throws<InvalidOperationException>(() => converter.Convert(null, null, null, null));

            Assert.Equal("Value is not a double", ex.Message);
        }

        [Fact]
        public void ConvertBack_Null_ReturnsNull_Test()
        {
            DoubleToWeightStringConverter converter = new DoubleToWeightStringConverter();

            Assert.Equal(null, converter.ConvertBack(null, null, null, null));
        }

        [Theory]
        [InlineData(0.0625, "0.06 lbs")]
        [InlineData(1.0, "1.0 lbs")]
        [InlineData(1.0625, "1.06 lbs")]
        public void Convert_Double_FractionalPoundsString_Test(double input, string output)
        {
            UserEntity user = new UserEntity()
            {
                Settings = new UserSettingsEntity() {ShippingWeightFormat = 0}
            };
            UserSessionWrapper session = new UserSessionWrapper(user);

            DoubleToWeightStringConverter converter = new DoubleToWeightStringConverter(session);

            Assert.Equal(output, converter.Convert(input, null, null, null));
        }

        [Theory]
        [InlineData(0.0625, "0 lbs  1 oz")]
        [InlineData(1.0, "1 lbs  0 oz")]
        [InlineData(1.0625, "1 lbs  1 oz")]
        public void Convert_Double_PoundsAndOuncesString_Test(double input, string output)
        {
            UserEntity user = new UserEntity()
            {
                Settings = new UserSettingsEntity() { ShippingWeightFormat = 1 }
            };
            UserSessionWrapper session = new UserSessionWrapper(user);

            DoubleToWeightStringConverter converter = new DoubleToWeightStringConverter(session);

            Assert.Equal(output, converter.Convert(input, null, null, null));
        }

        [Theory]
        [InlineData("1lbs", 1.0)]
        [InlineData("1lbs.", 1.0)]
        [InlineData("1lb", 1.0)]
        [InlineData("1lb.", 1.0)]
        [InlineData("1l", 1.0)]
        [InlineData("1pounds", 1.0)]
        [InlineData("1pound", 1.0)]
        [InlineData("1o", 0.0625)]
        [InlineData("1oz", 0.0625)]
        [InlineData("1oz.", 0.0625)]
        [InlineData("1ounce", 0.0625)]
        [InlineData("1ounces", 0.0625)]
        [InlineData("1 lbs", 1.0)]
        [InlineData("1 lbs.", 1.0)]
        [InlineData("1 lb", 1.0)]
        [InlineData("1 lb.", 1.0)]
        [InlineData("1 l", 1.0)]
        [InlineData("1 pounds", 1.0)]
        [InlineData("1 pound", 1.0)]
        [InlineData("1 o", 0.0625)]
        [InlineData("1 oz", 0.0625)]
        [InlineData("1 oz.", 0.0625)]
        [InlineData("1 ounce", 0.0625)]
        [InlineData("1 ounces", 0.0625)]
        [InlineData("1 pounds 1 ounces", 1.0625)]
        [InlineData("1 lbs 1 oz", 1.0625)]
        [InlineData("1 lbs. 1 oz.", 1.0625)]
        [InlineData("0 lbs. 1 oz.", 0.0625)]
        public void ConvertBack_String_ToDouble_Test(string input, double output)
        {
            DoubleToWeightStringConverter converter = new DoubleToWeightStringConverter();

            Assert.Equal(output, converter.ConvertBack(input, null, null, null));
        }
    }
}
