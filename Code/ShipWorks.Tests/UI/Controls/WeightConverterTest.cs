using System.Diagnostics.CodeAnalysis;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Tests.UI.Controls
{
    [SuppressMessage("CSharp", "CS3016",
        Justification = "We don't need to worry about tests being CLS compliant")]
    public class WeightConverterTest
    {
        [Fact]
        public void ParseWeight_Null_ReturnsNull_Test()
        {
            WeightConverter converter = new WeightConverter(new Mock<IUserSession>().Object);

            Assert.Equal(null, converter.ParseWeight(null));
        }

        [Theory]
        [InlineData(0.0625, "0.06 lbs")]
        [InlineData(1.0, "1.0 lbs")]
        [InlineData(1.0625, "1.06 lbs")]
        public void Convert_Double_FractionalPoundsString_Test(double input, string output)
        {
            WeightConverter converter = new WeightConverter(new Mock<IUserSession>().Object);

            Assert.Equal(output, converter.FormatWeight(input));
        }

        [Theory]
        [InlineData(0.0625, "0 lbs  1 oz")]
        [InlineData(1.0, "1 lbs  0 oz")]
        [InlineData(1.0625, "1 lbs  1 oz")]
        public void Convert_Double_PoundsAndOuncesString_Test(double input, string output)
        {
            Mock<IUserSession> session = new Mock<IUserSession>();
            session.Setup(x => x.User).Returns(new UserEntity
            {
                Settings = new UserSettingsEntity { ShippingWeightFormat = 1 }
            });

            WeightConverter converter = new WeightConverter(session.Object);

            Assert.Equal(output, converter.FormatWeight(input));
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
        public void ParseWeight_String_ToDouble_Test(string input, double output)
        {
            WeightConverter converter = new WeightConverter(new Mock<IUserSession>().Object);

            Assert.Equal(output, converter.ParseWeight(input));
        }
    }
}
